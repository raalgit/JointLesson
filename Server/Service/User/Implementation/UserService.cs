using JL.ApiModels.UserModels.Response;
using JL.DAL.Repository.Abstraction;
using JL.Persist;
using JL.Service.User.Abstraction;
using JL.Settings;
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

        public UserService(IServiceProvider serviceProvider,
                           ICourseRepository courseRepository,
                           ICourseTeacherRepository courseTeacherRepository,
                           IGroupAtCourseRepository groupAtCourseRepository)
        {
            _serviceProvider = serviceProvider;
            _courseRepository = courseRepository;
            _courseTeacherRepository = courseTeacherRepository;
            _groupAtCourseRepository = groupAtCourseRepository;
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
    }
}
