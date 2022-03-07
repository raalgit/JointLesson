using JointLessonTerminal.Core;
using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.Core.Material;
using JointLessonTerminal.Model.ServerModels;
using JointLessonTerminal.MVVM.Model;
using JointLessonTerminal.MVVM.Model.EventModels;
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
    public class CurrentCourseWindowViewModel : ObservableObject
    {
        public CourseModel Course { get; set; }

        public string CurrentPageId { get { return currentPageId; } set { currentPageId = value; OnPropsChanged("CurrentPageId"); } }
        private string currentPageId;

        public Manual Manual { get { return manual; } set { manual = value; OnPropsChanged("Manual"); } }
        private Manual manual;

        private MaterialHandler materialHandler;

        private ManualData manualData;
        public ManualData ManualData { get { return manualData; } set { manualData = value; OnPropsChanged("ManualData"); } }

        private bool isCourseTeacher;
        public bool IsCourseTeacher { get { return isCourseTeacher; } set { isCourseTeacher = value; OnPropsChanged("IsCourseTeacher"); } }

        private bool isCourseActive;
        public bool IsCourseActive { get { return isCourseActive; } set { isCourseActive = value; OnPropsChanged("IsCourseActive"); } }

        private Visibility selectStartPageBtnVisibility;
        public Visibility SelectStartPageBtnVisibility { get { return selectStartPageBtnVisibility; } set { selectStartPageBtnVisibility = value; OnPropsChanged("SelectStartPageBtnVisibility"); } }

        private Visibility startLessonBtnVisibility;
        public Visibility StartLessonBtnVisibility { get { return startLessonBtnVisibility; } set { startLessonBtnVisibility = value; OnPropsChanged("StartLessonBtnVisibility"); } }

        private Visibility endLessonBtnVisibility;
        public Visibility EndtLessonBtnVisibility { get { return endLessonBtnVisibility; } set { endLessonBtnVisibility = value; OnPropsChanged("EndtLessonBtnVisibility"); } }

        private Visibility joinLessonBtnVisibility;
        public Visibility JoinLessonBtnVisibility { get { return joinLessonBtnVisibility; } set { joinLessonBtnVisibility = value; OnPropsChanged("JoinLessonBtnVisibility"); } }


        public RelayCommand StartLessonCommand { get; set; }
        public RelayCommand EndLessonCommand { get; set; }
        public RelayCommand EnterLessonCommand { get; set; }

        public CurrentCourseWindowViewModel()
        {

        }

        public void InitData(CourseModel course)
        {
            Course = course;
            Manual = null;
            ManualData = null;

            materialHandler = new MaterialHandler();
            StartLessonBtnVisibility = Visibility.Collapsed;
            EndtLessonBtnVisibility = Visibility.Collapsed;
            JoinLessonBtnVisibility = Visibility.Collapsed;
            SelectStartPageBtnVisibility = Visibility.Collapsed;

            StartLessonCommand = new RelayCommand(async x => {
                await StartLesson(CurrentPageId ?? "");
            });
            EndLessonCommand = new RelayCommand(async x => {
                await EndLesson();
            });
            EnterLessonCommand = new RelayCommand(async x => {
                await JoinLesson();
            });

            Task.Factory.StartNew(async x =>
            {
                var resp = await LoadCourseData();
                CurrentPageId = resp.lastPage;
                IsCourseTeacher = resp.isTeacher;
                IsCourseActive = resp.lessonIsActive;
                if (IsCourseTeacher)
                {
                    SelectStartPageBtnVisibility = Visibility.Visible;
                }
                updateBtnVisibility();
            }, null);

            Task.Factory.StartNew(async x => 
            {
                ManualData = await loadManualData();
                if (IsCourseTeacher) onSelectPageSubscribe();
            }, null);
        }

        private void onSelectPageSubscribe()
        {
            if (manualData == null) throw new NullReferenceException(nameof(manual));

            foreach (var chapter in manualData.chapters)
            {
                if (chapter.topics == null || chapter.topics.Count == 0) continue;
                foreach (var topic in chapter.topics)
                {
                    if (topic.didacticUnits == null || topic.didacticUnits.Count == 0) continue;
                    foreach (var unit in topic.didacticUnits)
                    {
                        if (unit.pages == null || unit.pages.Count == 0) continue;
                        foreach (var page in unit.pages)
                        {
                            page.OnPageSelected += onPageSelect;
                        }
                    }
                }
            }
            throw new Exception("Страница не найдена");
        }

        private void onPageSelect(object sender, EventArgs args)
        {
            var page = (Page)sender;
            if (page != null && !string.IsNullOrEmpty(page.id))
            {
                CurrentPageId = page.id;
            }
        }

        private async Task<Manual> loadManual(int manualId)
        {
            var courseManual = await materialHandler.LoadManualById(manualId);
            Manual = courseManual;
            return courseManual;
        }

        private async Task<ManualData> loadManualData()
        {
            var currentManual = await loadManual(Course.ManualId);
            ManualData = await materialHandler.LoadById(manual.fileDataId.Value);
            return ManualData;
        }

        private async Task<GetCourseDataResponse> LoadCourseData()
        {
            var manualGetRequest = new RequestModel<object>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Get,
                UrlFilter = $"/{Course.CourseId}"
            };
            var sender = new RequestSender<object, GetCourseDataResponse>();
            var responsePost = await sender.SendRequest(manualGetRequest, "/user/course-data");
            return responsePost;
        }

        private async Task StartLesson(string page)
        {
            var startLessonRequest = new RequestModel<StartSyncLessonRequest>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Post,
                Body = new StartSyncLessonRequest()
                {
                    StartPage = page,
                    CourseId = Course.CourseId
                }
            };
            var sender = new RequestSender<StartSyncLessonRequest, StartSyncLessonResponse>();
            var responsePost = await sender.SendRequest(startLessonRequest, "/teacher/start-sync-lesson");
            if (responsePost.isSuccess)
            {
                IsCourseActive = responsePost.canConnectToSyncLesson;
                updateBtnVisibility();
            }
        }

        private async Task EndLesson()
        {
            var closeLessonRequest = new RequestModel<CloseLessonRequest>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Post,
                Body = new CloseLessonRequest()
                {
                    CourseId = Course.CourseId
                }
            };
            var sender = new RequestSender<CloseLessonRequest, CloseLessonResponse>();
            var responsePost = await sender.SendRequest(closeLessonRequest, "/teacher/close-sync-lesson");
            if (responsePost.isSuccess)
            {
                IsCourseActive = responsePost.canConnectToSyncLesson;
                updateBtnVisibility();
            }
        }

        private async Task JoinLesson()
        {
            openLessonPage();
        }

        private void updateBtnVisibility()
        {
            if (IsCourseTeacher && IsCourseActive) EndtLessonBtnVisibility = Visibility.Visible;
            else EndtLessonBtnVisibility = Visibility.Collapsed;

            if (IsCourseTeacher && !IsCourseActive) StartLessonBtnVisibility = Visibility.Visible;
            else StartLessonBtnVisibility = Visibility.Collapsed;

            if (IsCourseActive) JoinLessonBtnVisibility = Visibility.Visible;
            else JoinLessonBtnVisibility = Visibility.Collapsed;
        }

        private void openLessonPage()
        {
            var signal = new WindowEvent();
            signal.Type = WindowEventType.NEEDTOOPENLESSONPPAGE;
            var arg = new OnOpenCourseModel();
            arg.CourseId = Course.CourseId;
            arg.Manual = ManualData;
            arg.Page = CurrentPageId;
            signal.Argument = arg;
            SendEventSignal(signal);
        }
    }
}
