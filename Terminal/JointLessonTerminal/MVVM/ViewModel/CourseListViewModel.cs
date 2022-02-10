using JointLessonTerminal.Core;
using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.MVVM.Model;
using JointLessonTerminal.MVVM.Model.HttpModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace JointLessonTerminal.MVVM.ViewModel
{
    public class CourseListViewModel : ObservableObject
    {
        private UserSettings userSettings;

        public bool IsTeacher { get; set; } = false;
        public bool IsEditor { get; set; } = false;
        public bool IsStudent {get; set; } = false;

        public CourseCollection CourseCollection { get; set; }
        private CourseModel selectedCourse;
        public CourseModel SelectedCourse
        {
            get
            {
                return selectedCourse;
            }
            set
            {
                selectedCourse = value;
                openCourse(selectedCourse);
                OnPropsChanged("SelectedCourse");
            }
        }


        public CourseListViewModel()
        {
            CourseCollection = new CourseCollection();
        }

        public void InitCourseData()
        {
            userSettings = UserSettings.GetInstance();

            if (userSettings.Roles != null && userSettings.Roles.Length > 0)
            {
                IsEditor = userSettings.Roles.Any(x => x.systemName == "Editor");
                IsTeacher = userSettings.Roles.Any(x => x.systemName == "Teacher");
                IsStudent = userSettings.Roles.Any(x => x.systemName == "Student");
            }

            if (IsStudent || IsTeacher) getMyCourses();
        }

        public void openCourse(object course)
        {
            if (course == null) return;

            var signal = new WindowEvent();
            signal.Argument = course as CourseModel;
            signal.Type = WindowEventType.COURSESELECTED;
            SendEventSignal(signal);
        }


        private async void getMyCourses()
        {
            // Создание модели http запроса
            var getCoursesRequest = new RequestModel<object>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Get
            };

            try
            {
                // Отправка http запроса для авторизации
                var sender = new RequestSender<object, GetMyCoursesResponse>();
                var responseGet = await sender.SendRequest(getCoursesRequest, "/user/my-courses");

                // Если данные курсов получили
                if (responseGet.isSuccess)
                {
                    CourseCollection.CourseModels = responseGet.courses.Select(x => new CourseModel()
                    {
                        CourseImagePreview = null,
                        Description = x.description,
                        Title = x.name,
                        CourseId = x.id
                    }).ToList();
                }
                else
                {
                    MessageBox.Show("Не удалось получить данные курсов");
                }
            }
            catch (Exception er)
            {

            }
        }
    }
}
