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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Service.User.Implementation
{
    public class UserService : IUserService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IGroupAtCourseRepository _groupAtCourseRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseTeacherRepository _courseTeacherRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IMongoRepository _mongoRepository;
        private readonly IFileDataRepository _fileDataRepository;
        private readonly ISignalUserConnectionRepository _signalUserConnectionRepository;
        private readonly IFileUtility _fileUtility;

        public UserService(IServiceProvider serviceProvider,
                           ICourseRepository courseRepository,
                           ICourseTeacherRepository courseTeacherRepository,
                           IGroupAtCourseRepository groupAtCourseRepository,
                           ILessonRepository lessonRepository,
                           ISignalUserConnectionRepository signalUserConnectionRepository,
                           IMongoRepository mongoRepository,
                           IFileDataRepository fileDataRepository)
        {
            _serviceProvider = serviceProvider;
            _courseRepository = courseRepository;
            _courseTeacherRepository = courseTeacherRepository;
            _groupAtCourseRepository = groupAtCourseRepository;
            _signalUserConnectionRepository = signalUserConnectionRepository;
            _lessonRepository = lessonRepository;
            _mongoRepository = mongoRepository;
            _fileDataRepository = fileDataRepository;

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

            response.LastPage = groupsAtCourse.FirstOrDefault()?.LastMaterialPage;

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
    }
}
