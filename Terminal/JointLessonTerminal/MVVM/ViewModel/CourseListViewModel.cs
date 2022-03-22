using JointLessonTerminal.Core;
using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.MVVM.Model;
using JointLessonTerminal.MVVM.Model.HttpModels.Response;
using JointLessonTerminal.MVVM.Model.SignalR;
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
        #region открытые поля
        public RelayCommand OpenEditorPageCommand { get; set; }
        public bool IsTeacher { get; set; } = false;
        public bool IsEditor { get; set; } = false;
        public bool IsStudent { get; set; } = false;
        public CourseCollection CourseCollection { get; set; }
        public Visibility EnterBtnVisibility
        {
            get { return enterBtnVisibility; }
            set { enterBtnVisibility = value; OnPropsChanged("EnterBtnVisibility"); }
        }
        public CourseModel SelectedCourse
        {
            get { return selectedCourse; }
            set { OpenCourse(value); OnPropsChanged("SelectedCourse"); }
        }
        #endregion

        #region закрытые поля
        private UserSettings userSettings;
        private CourseModel selectedCourse;
        private Visibility enterBtnVisibility;
        #endregion

        public CourseListViewModel()
        {
            CourseCollection = new CourseCollection();
        }

        #region открытые методы
        public void InitCourseData()
        {
            EnterBtnVisibility = Visibility.Hidden;
            userSettings = UserSettings.GetInstance();

            if (userSettings.Roles != null && userSettings.Roles.Length > 0)
            {
                IsEditor = userSettings.Roles.Any(x => x.systemName == "Editor");
                IsTeacher = userSettings.Roles.Any(x => x.systemName == "Teacher");
                IsStudent = userSettings.Roles.Any(x => x.systemName == "Student");
            }

            if (IsStudent || IsTeacher) getMyCourses();
            if (IsEditor) EnterBtnVisibility = Visibility.Visible;

            OpenEditorPageCommand = new RelayCommand(x => OpenEditorPage());
        }

        public void OpenCourse(object course)
        {
            if (course == null) return;

            var signal = new WindowEvent();
            signal.Argument = course as CourseModel;
            signal.Type = WindowEventType.COURSESELECTED;
            SendEventSignal(signal);
        }

        public void OpenEditorPage()
        {
            var signal = new WindowEvent();
            signal.Type = WindowEventType.NEEDTOOPENEDITORPAGE;
            SendEventSignal(signal);
        }

        #endregion

        #region закрытые методы
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
                        CourseId = x.id,
                        ManualId = x.manualId
                    }).ToList();
                }
                else
                {
                    var signal = new WindowEvent();
                    signal.Type = WindowEventType.COURSELIST_GETERROR;
                    signal.Argument = "Не удалось получить данные курсов!";
                    SendEventSignal(signal);
                }
            }
            catch (Exception er)
            {
                var signal = new WindowEvent();
                signal.Type = WindowEventType.COURSELIST_GETERROR;
                signal.Argument = er.Message;
                SendEventSignal(signal);
            }
        }
        #endregion
    }
}
