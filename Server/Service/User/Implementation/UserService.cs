using JL.ApiModels.UserModels.Request;
using JL.ApiModels.UserModels.Response;
using JL.DAL.Mongo.Repository;
using JL.DAL.Repository.Abstraction;
using JL.Persist;
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
        private readonly IMongoRepository _mongoRepository;
        private readonly IFileDataRepository _fileDataRepository;
        private readonly IFileUtility _fileUtility;

        public UserService(IServiceProvider serviceProvider,
                           ICourseRepository courseRepository,
                           ICourseTeacherRepository courseTeacherRepository,
                           IGroupAtCourseRepository groupAtCourseRepository,
                           IMongoRepository mongoRepository,
                           IFileDataRepository fileDataRepository)
        {
            _serviceProvider = serviceProvider;
            _courseRepository = courseRepository;
            _courseTeacherRepository = courseTeacherRepository;
            _groupAtCourseRepository = groupAtCourseRepository;
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
            return null;
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
    }
}
