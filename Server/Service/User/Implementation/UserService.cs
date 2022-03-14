using JL.ApiModels.UserModels.Request;
using JL.ApiModels.UserModels.Response;
using JL.DAL.Mongo.Repository;
using JL.DAL.Repository.Abstraction;
using JL.Persist;
using JL.PersistModels;
using JL.Service.User.Abstraction;
using JL.Settings;
using JL.Utility2L.Abstraction;
using JL.Utility2L.Implementation;
using JL.Utility2L.Models.SignalR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Service.User.Implementation
{
    public class UserService : IUserService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILessonTabelRepository _lessonTabelRepository;
        private readonly IGroupAtCourseRepository _groupAtCourseRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseTeacherRepository _courseTeacherRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IMongoRepository _mongoRepository;
        private readonly IFileDataRepository _fileDataRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository; 
        private readonly ISignalUserConnectionRepository _signalUserConnectionRepository;
        private readonly IUserRemoteAccessRepository _userRemoteAccessRepository;
        private readonly IFileUtility _fileUtility;

        private readonly IHubContext<SignalHub> _hubContext;

        public UserService(IServiceProvider serviceProvider,
                           ILessonTabelRepository lessonTabelRepository,
                           ICourseRepository courseRepository,
                           ICourseTeacherRepository courseTeacherRepository,
                           IGroupAtCourseRepository groupAtCourseRepository,
                           ILessonRepository lessonRepository,
                           ISignalUserConnectionRepository signalUserConnectionRepository,
                           IMongoRepository mongoRepository,
                           IGroupRepository groupRepository,
                           IUserRepository userRepository,
                           IUserRemoteAccessRepository userRemoteAccessRepository,
                           IFileDataRepository fileDataRepository,
                           IHubContext<SignalHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _lessonTabelRepository = lessonTabelRepository;
            _courseRepository = courseRepository;
            _groupRepository = groupRepository;
            _userRemoteAccessRepository = userRemoteAccessRepository;
            _courseTeacherRepository = courseTeacherRepository;
            _groupAtCourseRepository = groupAtCourseRepository;
            _userRepository = userRepository;
            _signalUserConnectionRepository = signalUserConnectionRepository;
            _lessonRepository = lessonRepository;
            _mongoRepository = mongoRepository;
            _fileDataRepository = fileDataRepository;

            _hubContext = hubContext;
            _fileUtility = new FileUtility(_mongoRepository, _fileDataRepository);
        }

        /// Если у пользователя установлена группа, то он студент
        /// В этом случае мы возвращаем курсы, к которым есть доступ у группы
        /// студента. Если пользователь не студент, то выдаем курсы, которые
        /// ведет преподаватель
        public async Task<GetMyCoursesResponse> GetMyCourses(UserSettings userSettings)
        {
            var response = new GetMyCoursesResponse();

            var userId = userSettings.User.Id;
            var courses = (List<Course>?)null;

            if (userSettings.User.GroupId.HasValue)
            {
                int groupId = userSettings.User.GroupId.Value;
                courses = (from groupAtCourse in _groupAtCourseRepository.Get()
                           join course in _courseRepository.Get() on groupAtCourse.CourseId equals course.Id
                           where groupAtCourse.GroupId == groupId
                           select course).ToList();
            }
            else
            {
                courses = (from teacherCourse in _courseTeacherRepository.Get()
                           join course in _courseRepository.Get() on teacherCourse.CourseId equals course.Id
                           where teacherCourse.UserId == userId
                           select course).ToList();
            }

            response.Courses = courses;
            return response;
        }

        public async Task<GetFileResponse> GetFile(int fileDataId)
        {
            var response = new GetFileResponse();

            var fileData = _fileDataRepository.GetById(fileDataId);
            response.File = await _fileUtility.GetFileAsBytesById(fileData.MongoId);
            return response;
        }

        public async Task<GetManualFilesResponse> GetManualFiles(GetManualFilesRequest request)
        {
            var response = new GetManualFilesResponse();

            var fileDatas = _fileDataRepository.Get().Where(x => request.FileDataIds.Distinct().Contains(x.Id)).ToList();
            response.FileDatas = fileDatas;
            return response;
        }

        public async Task<AddNewFileResponse> AddNewFile(AddNewFileRequest request)
        {
            var response = new AddNewFileResponse();

            string extension = Path.GetExtension(request.Name);
            Stream stream = new MemoryStream(request.File);
            int fileDataId = await _fileUtility.CreateNewFileAsync(stream, request.Name, extension);
            response.FileDataId = fileDataId;

            return response;
        }

        public async Task<RegisterSignalConnectionResponse> RegisterSignalConnection(string connectionId, UserSettings userSettings)
        {
            var response = new RegisterSignalConnectionResponse();

            var oldConnectionData = _signalUserConnectionRepository.Get().Where(x => x.UserId == userSettings.User.Id).ToList();
            if (oldConnectionData != null && oldConnectionData.Count > 0)
            {
                foreach (var data in oldConnectionData) _signalUserConnectionRepository.Delete(data);
            }

            var domain = new SignalUserConnection()
            {
                UserId = userSettings.User.Id,
                ConnectionId = connectionId
            };

            _signalUserConnectionRepository.Insert(domain);
            _signalUserConnectionRepository.SaveChanges();
            return response;
        }

        public async Task<GetCourseDataResponse> GetCourseData(int courseId, UserSettings userSettings)
        {
            var response = new GetCourseDataResponse();

            // Получение преподавателей текущего курса
            var courseTeachers = _courseTeacherRepository.Get().Where(x => x.CourseId == courseId).ToList();
            if (courseTeachers == null || courseTeachers.Count == 0) throw new NullReferenceException(nameof(courseTeachers));
            response.CourseTeachers = courseTeachers;

            // Проверка пользователя на статус преподавателя
            response.IsTeacher = courseTeachers.Select(x => x.UserId).Contains(userSettings.User.Id);

            // Получение данных курсовых занятий для всех групп
            var groupsAtCourse = _groupAtCourseRepository.Get().Where(x => x.CourseId == courseId)
                ?? throw new ArgumentException(nameof(courseId));

            response.LastPage = groupsAtCourse.FirstOrDefault()?.LastMaterialPage ?? string.Empty;

            /// Для каждой группы может вестись отдельное занятие 
            /// При входе преподавателя необходимо проверять для каких именно групп активно
            /// занятие, но при текущей реализации это не обязательно. При необходимости
            /// разбиения занятий по группам, необходимо доработать сервис, добавив 
            /// информацию об активных занятиях для каждой группы
            if (response.IsTeacher)
            {
                response.LessonIsActive = groupsAtCourse.Any(x => x.IsActive);
            } 
            else
            {
                var groupAtCourse = groupsAtCourse.FirstOrDefault(x => x.GroupId == userSettings.User.GroupId) 
                    ?? throw new Exception("Не найдены данные курса для пользователя");

                response.CourseData = groupAtCourse;

                if (groupAtCourse.IsActive)
                {
                    var lesson = _lessonRepository.Get().FirstOrDefault(x => x.GroupAtCourseId == groupAtCourse.Id);
                    response.Lesson = lesson;
                    response.LessonIsActive = true;
                }
                else
                {
                    response.LessonIsActive = false;
                }
            }

            return response;
        }

        public async Task<StartSRSLessonResponse> StartSRSLesson(StartSRSLessonRequest request, UserSettings userSettings)
        {
            var response = new StartSRSLessonResponse();

            var userId = userSettings.User.Id;
            var srsUserLessons = _lessonRepository
                .Get()
                .Where(x => 
                    x.TeacherId == userId && 
                    x.Type == "SRS" && 
                    x.CourseId == request.CourseId)
                .ToList();

            var activeLesson = srsUserLessons.FirstOrDefault(x => !x.EndDate.HasValue);
            
            if (activeLesson == null)
            {
                var lastLesson = srsUserLessons.OrderByDescending(x => x.Id).FirstOrDefault();
                var newLesson = new Lesson()
                {
                    CourseId = request.CourseId,
                    StartDate = DateTime.Now,
                    GroupAtCourseId = null,
                    LastMaterialPage = lastLesson != null ? lastLesson.LastMaterialPage : null,
                    EndDate = (DateTime?)null,
                    TeacherId = userId,
                    Type = "SRS"
                };
                _lessonRepository.Insert(newLesson);
            }
            else
            {
                response.Page = activeLesson.LastMaterialPage;
            }

            _lessonRepository.SaveChanges();

            return response;
        }

        public async Task<ChangeSRSLessonManualPageResponse> ChangeActivePage(ChangeSRSLessonManualPageRequest request, UserSettings userSettings)
        {
            var response = new ChangeSRSLessonManualPageResponse();

            var userId = userSettings.User.Id;
            var activeLesson = _lessonRepository
                .Get()
                .FirstOrDefault(x =>
                    x.TeacherId == userId &&
                    x.Type == "SRS" &&
                    x.CourseId == request.CourseId &&
                    !x.EndDate.HasValue);

            if (activeLesson != null)
            {
                activeLesson.LastMaterialPage = request.NextPage;
                _lessonRepository.SaveChanges();
            }

            return response;
        }

        public async Task<CloseSRSLessonResponse> CloseLesson(CloseSRSLessonRequest request, UserSettings userSettings)
        {
            var response = new CloseSRSLessonResponse();

            var userId = userSettings.User.Id;
            var activeLesson = _lessonRepository
                .Get()
                .FirstOrDefault(x =>
                    x.TeacherId == userId &&
                    x.Type == "SRS" &&
                    x.CourseId == request.CourseId &&
                    !x.EndDate.HasValue);

            if (activeLesson != null)
            {
                activeLesson.EndDate = DateTime.Now;
                _lessonRepository.Update(activeLesson);
            }

            _lessonRepository.SaveChanges();

            return response;
        }

        public async Task<GetRemoteAccessDataResponse> GetRemoteAccessData(GetRemoteAccessDataRequest request)
        {
            var response = new GetRemoteAccessDataResponse();

            var data = _userRemoteAccessRepository.Get().FirstOrDefault(x =>
                            x.CourseId == request.CourseId &&
                            x.UserId == request.UserId &&
                            (DateTime.Now - x.StartDate).TotalHours < 1
                       );

            response.ConnectionData = data != null ? data.ConnectionData : String.Empty;
            return response;
        }

        public async Task<CreateRemoteAccessResponse> CreateRemoteAccess(CreateRemoteAccessRequest request, UserSettings userSettings)
        {
            var response = new CreateRemoteAccessResponse();

            var oldData = _userRemoteAccessRepository.Get().FirstOrDefault(x => x.UserId == userSettings.User.Id);
            if (oldData != null)
            {
                _userRemoteAccessRepository.Delete(oldData);
            }

            _userRemoteAccessRepository.Insert(new UserRemoteAccess()
            {
                ConnectionData = request.ConnectionData,
                StartDate = DateTime.Now,
                UserId = userSettings.User.Id,
                CourseId = request.CourseId
            });
            _userRemoteAccessRepository.SaveChanges();

            return response;
        }

        public async Task<GetRemoteAccessListResponse> GetRemoteAccessList(int courseId)
        {
            var response = new GetRemoteAccessListResponse();

            var list = _userRemoteAccessRepository
                .Get()
                .Where(x => x.CourseId == courseId)
                .ToList();

            list = list.Where(x => (DateTime.Now - x.StartDate).Hours < 1).ToList();

            response.UserRemoteAccesses = list;
            return response;
        }

        public async Task<JoinLessonResponse> JoinLesson(JoinLessonRequest request, UserSettings userSettings) 
        {
            var response = new JoinLessonResponse();

            // если пользователь - не преподаватель
            if (userSettings.User.GroupId != null)
            {
                // проверка старых записей на табель
                var oldActiveTabels = _lessonTabelRepository.Get().Where(x => x.UserId == userSettings.User.Id && !x.LeaveDate.HasValue).ToList();
                if (oldActiveTabels != null && oldActiveTabels.Count > 0)
                {
                    foreach (var tabel in oldActiveTabels)
                    {
                        tabel.LeaveDate = DateTime.Now;
                        _lessonTabelRepository.Update(tabel);
                    }
                }

                // получение данных по группе пользователя
                var courseGroup = _groupAtCourseRepository.Get().FirstOrDefault(x => x.CourseId == request.CourseId && x.GroupId == userSettings.User.GroupId)
                    ?? throw new Exception("Группа для данного курса не найдена");

                // получение активного занятия
                var lesson = _lessonRepository.Get().FirstOrDefault(x => x.GroupAtCourseId == courseGroup.Id && !x.EndDate.HasValue)
                    ?? throw new Exception("Занятие для Вашей группы на данный момент не ведется");

                // добавление записи в табель занятия
                var newTabel = new LessonTabel()
                {
                    EnterDate = DateTime.Now,
                    LeaveDate = null,
                    LessonId = lesson.Id,
                    UserId = userSettings.User.Id
                };

                _lessonTabelRepository.Insert(newTabel);
                _lessonTabelRepository.SaveChanges();
            }
            else
            {
                var courseTeacher = _courseTeacherRepository.Get().FirstOrDefault(x => x.UserId == userSettings.User.Id && x.CourseId == request.CourseId)
                    ?? throw new Exception("Запись о преподавателе курса не найдена");

                courseTeacher.OnLesson = true;
                _courseTeacherRepository.Update(courseTeacher);
                _courseTeacherRepository.SaveChanges();
            }
            
            await onLessonUserListUpdateSendSignalR(request.CourseId);
            return response;
        }

        public async Task<LeaveLessonResponse> LeaveLesson(LeaveLessonRequest request, UserSettings userSettings) 
        {
            var response = new LeaveLessonResponse();

            // если пользователь - не преподаватель
            if (userSettings.User.GroupId != null)
            {
                // проверка старых записей на табель
                var oldActiveTabels = _lessonTabelRepository.Get().Where(x => x.UserId == userSettings.User.Id && !x.LeaveDate.HasValue).ToList();
                if (oldActiveTabels != null && oldActiveTabels.Count > 0)
                {
                    foreach (var tabel in oldActiveTabels)
                    {
                        tabel.LeaveDate = DateTime.Now;
                        _lessonTabelRepository.Update(tabel);
                    }
                }
                _lessonTabelRepository.SaveChanges();
            }
            else
            {
                var courseTeacher = _courseTeacherRepository.Get().FirstOrDefault(x => x.UserId == userSettings.User.Id && x.CourseId == request.CourseId)
                    ?? throw new Exception("Запись о преподавателе курса не найдена");

                courseTeacher.OnLesson = false;
                _courseTeacherRepository.Update(courseTeacher);
                _courseTeacherRepository.SaveChanges();
            }

            await onLessonUserListUpdateSendSignalR(request.CourseId);
            return response;
        }

        public async Task<UpHandResponse> UpHand(UpHandRequest request, UserSettings userSettings)
        {
            var response = new UpHandResponse();
            if (userSettings.User.GroupId == null) throw new Exception("Только учащийся занятия может поднять руку");

            // получение данных по группе пользователя
            var courseGroup = _groupAtCourseRepository.Get().FirstOrDefault(x => x.CourseId == request.CourseId && x.GroupId == userSettings.User.GroupId)
                ?? throw new Exception("Группа для данного курса не найдена");

            // получение активного занятия
            var lesson = _lessonRepository.Get().FirstOrDefault(x => x.GroupAtCourseId == courseGroup.Id && !x.EndDate.HasValue)
                ?? throw new Exception("Занятие для Вашей группы на данный момент не ведется");

            var tabel = _lessonTabelRepository.Get().FirstOrDefault(x => x.LessonId == lesson.Id && x.UserId == userSettings.User.Id && !x.LeaveDate.HasValue)
                ?? throw new Exception("Не найден табель занятия");

            tabel.HandUp = !tabel.HandUp;
            _lessonTabelRepository.Update(tabel);
            _lessonTabelRepository.SaveChanges();

            await onLessonUserListUpdateSendSignalR(request.CourseId);
            return response;
        }


        private async Task<bool> onLessonUserListUpdateSendSignalR(int courseId)
        {
            var usersAtLessonSignalRModel = new List<UserAtLesson>();

            // получение списка учащихся на занятии
            var groupsData = (from grpAtCourse in _groupAtCourseRepository.Get()
                              join les in _lessonRepository.Get() on grpAtCourse.Id equals les.GroupAtCourseId
                              join lesTbl in _lessonTabelRepository.Get() on les.Id equals lesTbl.LessonId
                              join user in _userRepository.Get() on grpAtCourse.GroupId equals user.GroupId
                              where
                              grpAtCourse.CourseId == courseId &&
                              !les.EndDate.HasValue &&
                              !lesTbl.LeaveDate.HasValue &&
                              lesTbl.UserId == user.Id
                              select new
                              {
                                  grpAtCourse,
                                  lesTbl,
                                  user
                              }).ToList();

            // добавление участников занятия к списку нотификации signalR
            usersAtLessonSignalRModel.AddRange(groupsData.Select(x => new UserAtLesson()
            {
                UserId = x.user.Id,
                IsTeacher = false,
                UpHand = x.lesTbl.HandUp,
                UserFio = x.user.FirstName + " " + x.user.ThirdName
            }).ToList());

            var userIds = groupsData.Select(x => x.user).Select(x => x.Id).Distinct().ToList();

            // добавление списка преподавателей
            var teachers = (from courseTeacher in _courseTeacherRepository.Get()
                            join user in _userRepository.Get() on courseTeacher.UserId equals user.Id
                            where
                            courseTeacher.CourseId == courseId &&
                            courseTeacher.OnLesson
                            select user).ToList();

            usersAtLessonSignalRModel.AddRange(teachers.Select(x => new UserAtLesson()
            {
                UserId = x.Id,
                IsTeacher = true,
                UpHand = false,
                UserFio = x.FirstName + " " + x.ThirdName
            }).ToList());
            userIds.AddRange(teachers.Select(x => x.Id));

            // отправка нотификации
            var connectionInfo = _signalUserConnectionRepository.Get().Where(x => userIds.Contains(x.UserId)).ToList();
            var connectionIds = connectionInfo.Select(x => x.ConnectionId).ToArray();

            // отправка signalR нотификации об изменении страницы всем участникам групп
            string lessonUsersJson = JsonSerializer.Serialize(usersAtLessonSignalRModel);
            await _hubContext.Clients.Clients(connectionIds).SendAsync("LessonUsersUpdate", lessonUsersJson);

            return true;
        }

        [Serializable]
        class UserAtLesson
        {
            public int UserId { get; set; }
            public string UserFio { get; set; }
            public bool UpHand { get; set; }
            public bool IsTeacher { get; set; }
        }
    }
}
