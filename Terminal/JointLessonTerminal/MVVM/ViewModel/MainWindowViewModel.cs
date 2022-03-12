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

        #region Обработчики окон
        public AuthWindowViewModel AuthVM { get; set; }
        public CourseListViewModel CourseVM { get; set; }
        public EditorWindowViewModel EditorVM { get; set; }
        public CurrentCourseWindowViewModel CurrentCourseVM { get; set; }
        public LessonWindowViewModel LessonVM { get; set; }
        public SrsLessonWindowViewModel SrsLessonVM { get; set; }
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

        public string FIO { get { return fio; } set { fio = value; OnPropsChanged("FIO"); } }
        private string fio;

        private readonly Notifier _notifier;
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

        #region Сигналы от дочерних окон
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
            switch (e.Type)
            {
                case WindowEventType.EDITOR_MYMANUALSLOADED:
                    _notifier.ShowInformation(e.Argument.ToString());
                    break;
                case WindowEventType.EDITOR_MANUALDATALOADED:
                    _notifier.ShowInformation(e.Argument.ToString());
                    break;
                case WindowEventType.EDITOR_MANUALCREATED:
                    _notifier.ShowSuccess(e.Argument.ToString());
                    break;
                case WindowEventType.EDITOR_MANUALUPDATED:
                    _notifier.ShowSuccess(e.Argument.ToString());
                    break;
                case WindowEventType.EDITOR_MANUALUPDATEDERROR:
                    _notifier.ShowError(e.Argument.ToString());
                    break;
                case WindowEventType.EDITOR_MANUALCREATEDERROR:
                    _notifier.ShowError(e.Argument.ToString());
                    break;
                case WindowEventType.EDITOR_ONFILEUPLOAD:
                    _notifier.ShowSuccess(e.Argument.ToString());
                    break;
                case WindowEventType.EDITOR_ONFILEUPLOADERROR:
                    _notifier.ShowError(e.Argument.ToString());
                    break;
            }
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
                case WindowEventType.LESSON_LOADMANUALERROR:
                    _notifier.ShowError(e.Argument.ToString());
                    break;
                case WindowEventType.LESSON_SYNCERROR:
                    _notifier.ShowError(e.Argument.ToString());
                    break;
            }
        }

        private void onCurrentCourseCompleted(object sender, WindowEvent e)
        {
            switch (e.Type)
            {
                case WindowEventType.NEEDTOOPENLESSONPPAGE:
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
                    EditorVM.InitData();
                    MenuVisibility.BackBtnVisibility = Visibility.Visible;
                    CurrentView = EditorVM;
                    break;
                case WindowEventType.COURSELIST_GETERROR:
                    _notifier.ShowError(e.Argument.ToString());
                    break;
            }
        }

        private void onAuthCompleted(object sender, WindowEvent e)
        {
            switch (e.Type)
            {
                case WindowEventType.AUTHORIZED:
                    _notifier.ShowSuccess("Вы успешно вошли в систему!");
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
                case WindowEventType.AUTH_ERROR:
                    _notifier.ShowError(e.Argument.ToString());
                    break;
            }
        } 
        #endregion
    }
}
