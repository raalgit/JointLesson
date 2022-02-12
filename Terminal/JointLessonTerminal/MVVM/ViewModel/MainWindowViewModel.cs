using JointLessonTerminal.Core;
using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.MVVM.Model;
using JointLessonTerminal.MVVM.Model.HttpModels.Request;
using JointLessonTerminal.MVVM.Model.HttpModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JointLessonTerminal.MVVM.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {

        #region Обработчики окон
        public AuthWindowViewModel AuthVM { get; set; }
        public CourseListViewModel CourseVM { get; set; }
        public EditorWindowViewModel EditorVM { get; set; }
        public CurrentCourseWindowViewModel CurrentCourseVM { get; set; }
        public LessonWindowViewModel LessonVM { get; set; }
        #endregion

        #region Отображение кнопок в верхнем меню
        public RelayCommand ExitFromSystemCommand { get; set; }
        public RelayCommand BackCommand { get; set; }

        public TopMenuVisibility MenuVisibility { get; set; }
        #endregion



        private object _currentView;
        public object CurrentView { 
            get {
                return _currentView;
            }
            set
            {
                _currentView = value;
                OnPropsChanged();
            }
        }


        public MainWindowViewModel()
        {
            MenuVisibility = new TopMenuVisibility()
            {
                BackBtnVisibility = Visibility.Hidden,
                ExitBtnVisibility = Visibility.Hidden,
                ProfileBtnVisibility = Visibility.Hidden
            };

            AuthVM = new AuthWindowViewModel();
            CourseVM = new CourseListViewModel();
            EditorVM = new EditorWindowViewModel();
            CurrentCourseVM = new CurrentCourseWindowViewModel();
            LessonVM = new LessonWindowViewModel();

            CurrentView = AuthVM;

            subscribeOnChildrenWindowSignals();

            ExitFromSystemCommand = new RelayCommand(x =>
            {
                var settings = UserSettings.GetInstance();
                settings.JWT = String.Empty;
                settings.CurrentUser = null;
                settings.Roles = null;

                MenuVisibility.ProfileBtnVisibility = Visibility.Hidden;
                MenuVisibility.ExitBtnVisibility = Visibility.Hidden;
                MenuVisibility.BackBtnVisibility = Visibility.Hidden;
                CurrentView = AuthVM;
            });

            BackCommand = new RelayCommand(x =>
            {
                MenuVisibility.BackBtnVisibility = Visibility.Hidden;
                CurrentView = CourseVM;
            });
        }

        #region Сигналы от дочерних окон
        private void subscribeOnChildrenWindowSignals()
        {
            AuthVM.WindowStateChanged += onAuthCompleted;
            CourseVM.WindowStateChanged += onCourseCompleted;
        }

        private void onCourseCompleted(object sender, WindowEvent e)
        {
            switch (e.Type)
            {
                case WindowEventType.COURSESELECTED:
                    CurrentCourseVM.InitData(e.Argument as CourseModel);
                    MenuVisibility.BackBtnVisibility = Visibility.Visible;
                    CurrentView = CurrentCourseVM;
                    break;
                case WindowEventType.NEEDTOOPENEDITORPAGE:
                    EditorVM.InitData();
                    MenuVisibility.BackBtnVisibility = Visibility.Visible;
                    CurrentView = EditorVM;
                    break;
            }
        }

        private void onAuthCompleted(object sender, WindowEvent e)
        {
            switch (e.Type)
            {
                case WindowEventType.AUTHORIZED:
                    MenuVisibility.ExitBtnVisibility = Visibility.Visible;
                    MenuVisibility.BackBtnVisibility = Visibility.Hidden;
                    MenuVisibility.ProfileBtnVisibility = Visibility.Visible;
                    CourseVM.InitCourseData();
                    CurrentView = CourseVM;
                    break;
            }
        } 
        #endregion
    }
}
