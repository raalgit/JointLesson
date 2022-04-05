using JointLessonTerminal.Core;
using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.MVVM.Model;
using JointLessonTerminal.MVVM.Model.EventModels;
using JointLessonTerminal.MVVM.Model.HttpModels.Request;
using JointLessonTerminal.MVVM.Model.HttpModels.Response;
using JointLessonTerminal.MVVM.Model.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Core;
using ToastNotifications.Lifetime.Clear;
using ToastNotifications.Position;

namespace JointLessonTerminal.MVVM.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        #region открытые поля
        public AuthWindowViewModel AuthVM { get; set; }
        public CourseListViewModel CourseVM { get; set; }
        public EditorWindowViewModel EditorVM { get; set; }
        public CurrentCourseWindowViewModel CurrentCourseVM { get; set; }
        public LessonWindowViewModel LessonVM { get; set; }
        public SrsLessonWindowViewModel SrsLessonVM { get; set; }
        public RelayCommand ExitFromSystemCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public TopMenuVisibility MenuVisibility { get; set; }
        public int ScreenWidth { get; set; }
        public object CurrentView { get { return _currentView; } set { _currentView = value; OnPropsChanged(); } }
        public string FIO { get { return fio; } set { fio = value; OnPropsChanged("FIO"); } }
        #endregion

        #region закрытые поля
        private object _currentView;
        private string fio;
        private static MainWindowViewModel instanse { get; set; }
        private readonly Notifier _notifier;
        #endregion

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
            SrsLessonVM = new SrsLessonWindowViewModel();

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

            _notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.BottomRight,
                    offsetX: 25,
                    offsetY: 100);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(4),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(4));

                cfg.Dispatcher = Application.Current.Dispatcher;

                cfg.DisplayOptions.TopMost = false;
                cfg.DisplayOptions.Width = 250;
            });
            _notifier.ClearMessages(new ClearAll());
        }

        #region открытые методы
        public static MainWindowViewModel GetInstance()
        {
            if (instanse == null)
                instanse = new MainWindowViewModel();
            return instanse;
        }
        public void ShowNotification(string text, NotificationType type)
        {
            switch (type)
            {
                case NotificationType.SUCCESS:
                    _notifier.ShowSuccess(text);
                    break;
                case NotificationType.WARNING:
                    _notifier.ShowWarning(text);
                    break;
                case NotificationType.ERROR:
                    _notifier.ShowError(text);
                    break;
                case NotificationType.INFO:
                    _notifier.ShowInformation(text);
                    break;
            }
        }
        #endregion

        #region закрытые методы
        private void subscribeOnChildrenWindowSignals()
        {
            AuthVM.WindowStateChanged += onAuthCompleted;
            CourseVM.WindowStateChanged += onCourseCompleted;
            CurrentCourseVM.WindowStateChanged += onCurrentCourseCompleted;
            LessonVM.WindowStateChanged += onLessonCompleted;
            EditorVM.WindowStateChanged += onEditorCompleted;
            SrsLessonVM.WindowStateChanged += onSrsLessonCompleted;
        }
        private void onEditorCompleted(object sender, WindowEvent e)
        {
        }
        private void onSrsLessonCompleted(object sender, WindowEvent e)
        {
            switch (e.Type)
            {
                case WindowEventType.EXITFROMSRSLESSON:
                    MenuVisibility.ExitBtnVisibility = Visibility.Visible;
                    MenuVisibility.BackBtnVisibility = Visibility.Hidden;
                    MenuVisibility.ProfileBtnVisibility = Visibility.Visible;
                    CurrentView = CourseVM;
                    break;
            }
        }
        private void onLessonCompleted(object sender, WindowEvent e)
        {
            switch (e.Type)
            {
                case WindowEventType.EXITFROMLESSON:
                    MenuVisibility.ExitBtnVisibility = Visibility.Visible;
                    MenuVisibility.BackBtnVisibility = Visibility.Hidden;
                    MenuVisibility.ProfileBtnVisibility = Visibility.Visible;
                    CurrentView = CourseVM;
                    break;
            }
        }
        private void onCurrentCourseCompleted(object sender, WindowEvent e)
        {
            switch (e.Type)
            {
                case WindowEventType.NEEDTOOPENLESSONPPAGE:
                    (e.Argument as OnOpenCourseModel).HalfOfScreenWidth = ScreenWidth / 2;
                    LessonVM.InitData(e.Argument as OnOpenCourseModel);
                    MenuVisibility.BackBtnVisibility = Visibility.Hidden;
                    CurrentView = LessonVM;
                    break;
                case WindowEventType.NEEDTOOPENSRSLESSON:
                    SrsLessonVM.InitData(e.Argument as OnOpenCourseModel);
                    MenuVisibility.BackBtnVisibility = Visibility.Visible;
                    CurrentView = SrsLessonVM;
                    break;
            }
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
                    var arg = e.Argument as OnOpenEditorPageArg;
                    EditorVM.InitData(arg.Offline);
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
                    var userData = UserSettings.GetInstance().CurrentUser;
                    FIO = userData.firstName + " " + userData.thirdName;
                    MenuVisibility.ExitBtnVisibility = Visibility.Visible;
                    MenuVisibility.BackBtnVisibility = Visibility.Hidden;
                    MenuVisibility.ProfileBtnVisibility = Visibility.Visible;
                    CourseVM.InitCourseData();
                    CurrentView = CourseVM;
                    break;
                case WindowEventType.AUTH_EMPTYLOGIN:
                    _notifier.ShowError("Пожалуйста, введите Ваш логин и пароль!");
                    break;
                case WindowEventType.NEEDTOOPENEDITORPAGE:
                    var arg = e.Argument as OnOpenEditorPageArg;
                    EditorVM.InitData(arg.Offline);
                    MenuVisibility.BackBtnVisibility = Visibility.Hidden;
                    MenuVisibility.ProfileBtnVisibility = Visibility.Hidden;
                    MenuVisibility.ExitBtnVisibility = Visibility.Hidden;
                    CurrentView = EditorVM;
                    break;
            }
        } 
        #endregion
    }
}
