using JL.ApiModels.TeacherModels.Request;
using JL.ApiModels.TeacherModels.Response;
using JL.DAL.Mongo.Repository;
using JL.DAL.Repository.Abstraction;
using JL.Persist;
using JL.Service.Teacher.Abstraction;
using JL.Settings;
using JL.Utility2L.Abstraction;
using JL.Utility2L.Implementation;
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
        private readonly ILessonRepository _lessonRepository;

        private readonly IFileUtility _fileUtility;

        public TeacherService(IServiceProvider serviceProvider,
                             IMongoRepository mongoRepository,
                             IFileDataRepository fileDataRepository,
                             IManualRepository manualRepository,
                             IGroupAtCourseRepository groupAtCourseRepository,
                             ILessonRepository lessonRepository)
        {
            _serviceProvider = serviceProvider;
            _mongoRepository = mongoRepository;
            _fileDataRepository = fileDataRepository;
            _manualRepository = manualRepository;
            _groupAtCourseRepository = groupAtCourseRepository;
            _lessonRepository = lessonRepository;

            _fileUtility = new FileUtility(_mongoRepository, _fileDataRepository);
        }

        public async Task<StartSyncLessonResponse> StartSyncLesson(StartSyncLessonRequest request, UserSettings userSettings)
        {
            var response = new StartSyncLessonResponse();
            
            var groupsAtCourse = _groupAtCourseRepository.Get().Where(x => x.CourseId == request.CourseId).ToList();
            if (groupsAtCourse == null || groupsAtCourse.Count == 0) throw new ArgumentNullException(nameof(groupsAtCourse));

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
                    GroupAtCourseId = group.Id
                };

                _lessonRepository.Insert(newLesson);
            }

            _groupAtCourseRepository.UpdateMany(groupsAtCourse);

            _lessonRepository.SaveChanges();
            _groupAtCourseRepository.SaveChanges();

            response.CanConnectToSyncLesson = true;
            return response;
        }

        public async Task<CloseLessonResponse> CloseLesson(CloseLessonRequest request, UserSettings userSettings)
        {
            var response = new CloseLessonResponse();

            var groupsAtCourse = _groupAtCourseRepository.Get().Where(x => x.CourseId == request.CourseId).ToList();
            if (groupsAtCourse == null || groupsAtCourse.Count == 0) throw new NullReferenceException(nameof(response));

            var groupsAtCourseIds = groupsAtCourse.Select(x => x.Id).ToList();
            var closedLessons = _lessonRepository.Get().Where(x => x.GroupAtCourseId.HasValue && groupsAtCourseIds.Contains(x.GroupAtCourseId.Value)).ToList();

            closedLessons.ForEach(x => x.EndDate = DateTime.Now);
            groupsAtCourse.ForEach(x => x.IsActive = false);

            _groupAtCourseRepository.UpdateMany(groupsAtCourse);
            _lessonRepository.UpdateMany(closedLessons);

            _groupAtCourseRepository.SaveChanges();
            _lessonRepository.SaveChanges();

            response.CanConnectToSyncLesson = false;
            return response;
        }
    }
}
