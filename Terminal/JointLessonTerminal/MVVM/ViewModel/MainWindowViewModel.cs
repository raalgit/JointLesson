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
        #endregion

        #region Отображение кнопок в верхнем меню
        public RelayCommand ExitFromSystemCommand { get; set; }

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
        }

        #region Сигналы от дочерних окон
        private void subscribeOnChildrenWindowSignals()
        {
            AuthVM.WindowStateChanged += onAuthCompleted;
        }
        private void onAuthCompleted(object sender, WindowEvent e)
        {
            switch (e.Type)
            {
                case WindowEventType.AUTHORIZED:
                    MenuVisibility.ExitBtnVisibility = Visibility.Visible;
                    MenuVisibility.BackBtnVisibility = Visibility.Hidden;
                    MenuVisibility.ProfileBtnVisibility = Visibility.Visible;
                    CurrentView = CourseVM;
                    
                    break;
            }
        } 
        #endregion
    }
}
