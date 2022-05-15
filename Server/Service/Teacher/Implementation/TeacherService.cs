using JL.ApiModels.TeacherModels.Request;
using JL.ApiModels.TeacherModels.Response;
using JL.DAL.Mongo.Repository;
using JL.DAL.Repository.Abstraction;
using JL.Persist;
using JL.Service.Teacher.Abstraction;
using JL.Settings;
using JL.Utility2L.Abstraction;
using JL.Utility2L.Implementation;
using JL.Utility2L.Models.SignalR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Service.Teacher.Implementation
{
    public class TeacherService : ITeacherService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMongoRepository _mongoRepository;
        private readonly IFileDataRepository _fileDataRepository;
        private readonly IManualRepository _manualRepository;
        private readonly IGroupAtCourseRepository _groupAtCourseRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ISignalUserConnectionRepository _signalUserConnectionRepository;


        private readonly IHubContext<SignalHub> _hubContext;

        private readonly IFileUtility _fileUtility;

        public TeacherService(IServiceProvider serviceProvider,
                             IMongoRepository mongoRepository,
                             IFileDataRepository fileDataRepository,
                             IManualRepository manualRepository,
                             IGroupAtCourseRepository groupAtCourseRepository,
                             ILessonRepository lessonRepository, 
                             IUserRepository userRepository,
                             IGroupRepository groupRepository,
                             ISignalUserConnectionRepository signalUserConnectionRepository,
                             IHubContext<SignalHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _mongoRepository = mongoRepository;
            _fileDataRepository = fileDataRepository;
            _manualRepository = manualRepository;
            _userRepository = userRepository;
            _groupAtCourseRepository = groupAtCourseRepository;
            _signalUserConnectionRepository = signalUserConnectionRepository;
            _groupRepository = groupRepository;
            _lessonRepository = lessonRepository;
            
            _hubContext = hubContext;
            _fileUtility = new FileUtility(_mongoRepository, _fileDataRepository);
        }

        public async Task<StartSyncLessonResponse> StartSyncLesson(StartSyncLessonRequest request, UserSettings userSettings)
        {
            var response = new StartSyncLessonResponse();
            
            var groupsAtCourse = _groupAtCourseRepository.Get().Where(x => x.CourseId == request.CourseId).ToList();
            if (groupsAtCourse == null || groupsAtCourse.Count == 0) throw new ArgumentNullException("Не найдены группы, привязанные к курсу");

            foreach (var group in groupsAtCourse)
            {
                group.IsActive = true;

                var newLesson = new Lesson()
                {
                    StartDate = DateTime.Now,
                    EndDate = (DateTime?)null,
                    LastMaterialPage = string.IsNullOrEmpty(request.StartPage) ? group.LastMaterialPage : request.StartPage,
                    TeacherId = userSettings.User.Id,
                    Type = "ONLINE",
                    GroupAtCourseId = group.Id, 
                    CourseId = request.CourseId,
                };

                _lessonRepository.Insert(newLesson);
            }

            _groupAtCourseRepository.UpdateMany(groupsAtCourse);

            _lessonRepository.SaveChanges();
            _groupAtCourseRepository.SaveChanges();

            response.CanConnectToSyncLesson = true;
            response.Message = $"Занятие успешно активировано. Кабинет открыт";
            return response;
        }

        public async Task<CloseLessonResponse> CloseLesson(CloseLessonRequest request, UserSettings userSettings)
        {
            var response = new CloseLessonResponse();

            var groupsAtCourse = _groupAtCourseRepository.Get().Where(x => x.CourseId == request.CourseId).ToList();
            if (groupsAtCourse == null || groupsAtCourse.Count == 0) throw new NullReferenceException("Не найдены группы, привязанные к курсу");

            var groupsAtCourseIds = groupsAtCourse.Select(x => x.Id).ToList();
            var closedLessons = _lessonRepository.Get()
                .Where(x => 
                    x.GroupAtCourseId.HasValue && 
                    groupsAtCourseIds.Contains(x.GroupAtCourseId.Value) &&
                    !x.EndDate.HasValue
                    )
                .ToList();

            closedLessons.ForEach(x => x.EndDate = DateTime.Now);
            groupsAtCourse.ForEach(x => x.IsActive = false);

            _groupAtCourseRepository.UpdateMany(groupsAtCourse);
            _lessonRepository.UpdateMany(closedLessons);

            _groupAtCourseRepository.SaveChanges();
            _lessonRepository.SaveChanges();

            response.CanConnectToSyncLesson = false;
            response.Message = $"Занятие завершено. Кабинет закрыт";
            return response;
        }

        public async Task<ChangeLessonManualPageResponse> ChangeActivePage(ChangeLessonManualPageRequest request)
        {
            var response = new ChangeLessonManualPageResponse();
 
            // получение списка групп и участников групп
            var groupsData = (from grpAtCourse in _groupAtCourseRepository.Get()
                              join grp in _groupRepository.Get() on grpAtCourse.GroupId equals grp.Id
                              join user in _userRepository.Get() on grp.Id equals user.GroupId
                              where grpAtCourse.CourseId == request.CourseId
                              select new
                              {
                                  grpAtCourse,
                                  user
                              }).ToList();

            // получение групп текущего курса
            var groupsAtCourse = groupsData.Select(x => x.grpAtCourse).Distinct().ToList();
            
            // Получение участников групп текущего курса
            var users = groupsData.Select(x => x.user).Distinct().ToList();

            if (request.IsOnline)
            {

                // установка текущей страницы для групп курса
                groupsAtCourse.ForEach(x => x.LastMaterialPage = request.NextPage);

                var groupsAtCourseIds = groupsAtCourse.Select(x => x.Id);

                // получение списка активных занятий текущего курса
                var activeLessons = _lessonRepository.Get()
                    .Where(x =>
                        x.GroupAtCourseId.HasValue &&
                        groupsAtCourseIds.Contains(x.GroupAtCourseId.Value) &&
                        !x.EndDate.HasValue
                        )
                    .ToList();

                // установка текущей страницы для всех занятий
                activeLessons.ForEach(x => x.LastMaterialPage = request.NextPage);

                _groupAtCourseRepository.SaveChanges();
                _lessonRepository.SaveChanges();
            }

            var userIds = users.Select(x => x.Id);

            // получение списка signalR соединений для участников групп
            var connectionInfo = _signalUserConnectionRepository.Get().Where(x => userIds.Contains(x.UserId)).ToList();
            var connectionIds = connectionInfo.Select(x => x.ConnectionId).ToArray();

            // отправка signalR нотификации об изменении страницы всем участникам групп
            await _hubContext.Clients.Clients(connectionIds).SendAsync("PageSync", request.NextPage + "/" + request.IsOnline.ToString());

            response.Message = $"Текущая страница изменена на {request.NextPage} с синхронизацией";
            return response;
        }
    }
}
