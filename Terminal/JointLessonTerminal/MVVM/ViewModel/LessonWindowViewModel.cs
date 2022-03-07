using JointLessonTerminal.Core;
using JointLessonTerminal.MVVM.Model.ServerModels;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.MVVM.Model.HttpModels.Response;
using System.Windows.Xps.Packaging;
using System.Windows.Documents;
using JointLessonTerminal.Model.ServerModels;
using JointLessonTerminal.Core.Material;
using JointLessonTerminal.MVVM.Model.EventModels;
using System.Windows;
using Task = System.Threading.Tasks.Task;
using JointLessonTerminal.MVVM.Model.HttpModels.Request;
using JointLessonTerminal.MVVM.Model.SignalR;
using JointLessonTerminal.MVVM.Model;

namespace JointLessonTerminal.MVVM.ViewModel
{
    public class LessonWindowViewModel : ObservableObject
    {
        private ManualData manual;
        public ManualData Manual { get; set; }

        public Visibility NextPageBtnVisibility { get { return nextPageBtnVisibility; } set { nextPageBtnVisibility = value; OnPropsChanged("NextPageBtnVisibility"); } }
        private Visibility nextPageBtnVisibility;
        public Visibility PrevPageBtnVisibility { get { return prevPageBtnVisibility; } set { prevPageBtnVisibility = value; OnPropsChanged("PrevPageBtnVisibility"); } }
        private Visibility prevPageBtnVisibility;

        private int courseId;
        private string currentPageId;
        private Core.Material.Page currentPage;

        private List<FileData> manualFiles;
        private List<Core.Material.Page> manualPages;


        private FixedDocumentSequence activeDocument;
        public FixedDocumentSequence ActiveDocument { get { return activeDocument; } set { activeDocument = value; OnPropsChanged("ActiveDocument"); } }


        private readonly string wordDirPath;
        private string documentXpsPath;
        private string documentWordPath;
        private SignalHub hub;
        private FixedDocumentSequence sequence;

        private FileData fileData;
        public FileData FileData { 
            get { 
                return fileData; 
            } 
            set { 
                fileData = value;

                if (fileData.mongoName.Contains(".doc"))
                {
                    var wordPath = Path.Combine(wordDirPath, fileData.mongoName.Replace(":", "-"));
                    wordPath = wordPath.Replace("\\", "/").Replace(" ", "_");

                    var xpsPath = wordPath + ".xps";

                    documentXpsPath = wordPath;
                    documentXpsPath = xpsPath;

                    if (!File.Exists(wordPath))
                    {
                        Task.Factory.StartNew(async x => 
                        {
                            var resp = await downloadWord(fileData.id, wordPath, xpsPath);
                            if (resp.isSuccess)
                            {
                                File.WriteAllBytes(wordPath, resp.file);
                                saveXPSDoc(wordPath, xpsPath);
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    sequence = getFixedDoc(documentXpsPath).GetFixedDocumentSequence();
                                    ActiveDocument = sequence;
                                });
                            }
                        }, new System.Threading.CancellationToken());
                    }
                    else
                    {
                        sequence = getFixedDoc(xpsPath).GetFixedDocumentSequence();
                        ActiveDocument = sequence;
                    }
                }
            } 
        }

        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PrevPageCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }

        public LessonWindowViewModel()
        {
            wordDirPath = AppDomain.CurrentDomain.BaseDirectory + "WORD";
            if (!Directory.Exists(wordDirPath))
            {
                Directory.CreateDirectory(wordDirPath);
            }

            NextPageCommand = new RelayCommand(async x => await nextPage(true));
            PrevPageCommand = new RelayCommand(async x => await nextPage(false));
            ExitCommand = new RelayCommand(x => exit());
        }

        public void InitData(OnOpenCourseModel data)
        {
            manualPages = new List<Core.Material.Page>();
            manual = data.Manual;
            courseId = data.CourseId;
            currentPageId = data.Page;

            var user = UserSettings.GetInstance();
            if (user.Roles.Select(x => x.systemName).Contains("Teacher"))
            {
                NextPageBtnVisibility = Visibility.Visible;
                PrevPageBtnVisibility = Visibility.Visible;
            }
            else
            {
                NextPageBtnVisibility = Visibility.Collapsed;
                PrevPageBtnVisibility = Visibility.Collapsed;
            }

            hub = SignalHub.GetInstance();
            hub.OnPageSync += onSyncPageEvent;

            try
            {
                Task.Factory.StartNew(async x => await loadManualFiles(), null); //.GetAwaiter().OnCompleted(showWordPage);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void onSyncPageEvent(object o, EventArgs e)
        {
            var arg = e as OnPageChangeEventArg;
            if (arg != null)
            {
                if (currentPageId != arg.NewPageId)
                {
                    currentPageId = arg.NewPageId;
                    currentPage = getPageById(currentPageId);
                    showWordPage();
                }
            }
        }

        private async Task loadManualFiles()
        {
            var loadManualFilesRequest = new RequestModel<GetManualFilesRequest>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Post,
                Body = new GetManualFilesRequest()
                {
                     FileDataIds = initManualPagesDataIds()
                }
            };
            var sender = new RequestSender<GetManualFilesRequest, GetManualFilesResponse>();
            var responsePost = await sender.SendRequest(loadManualFilesRequest, "/user/get-manual-files");

            if (responsePost.isSuccess)
            {
                manualFiles = responsePost.fileDatas;

                if (string.IsNullOrEmpty(currentPageId)) currentPage = getFirstPage();
                else currentPage = getPageById(currentPageId);

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    showWordPage();
                });
            }
        }

        private void exit()
        {
            var signal = new WindowEvent();
            signal.Type = WindowEventType.EXITFROMLESSON;
            signal.Argument = courseId;
            SendEventSignal(signal);
        }

        private async Task <bool> nextPage(bool forward)
        {
            var currentPageIndex = manualPages.FindIndex(x => x.id == currentPageId);
            var nextPageIndex = currentPageIndex;
            if (forward)
            {
                nextPageIndex++;
                if (nextPageIndex >= manualPages.Count) return false;
            }
            else
            {
                nextPageIndex--;
                if (nextPageIndex < 0) return false;
            }

            currentPage = manualPages.ElementAt(nextPageIndex);
            currentPageId = currentPage.id;

            var response = await SyncPage();
            showWordPage();
            return true;
        }

        private async Task<ChangeLessonManualPageResponse> SyncPage()
        {
            var changePageRequest = new RequestModel<ChangeLessonManualPageRequest>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Post,
                Body = new ChangeLessonManualPageRequest()
                {
                    CourseId = courseId,
                    NextPage = currentPage.id
                }
            };
            var sender = new RequestSender<ChangeLessonManualPageRequest, ChangeLessonManualPageResponse>();
            var responsePost = await sender.SendRequest(changePageRequest, "/teacher/change-page");
            if (!responsePost.isSuccess)
            {
                MessageBox.Show(responsePost.message);
                throw new Exception(responsePost.message);
            }
            return responsePost;
        }

        private void showWordPage()
        {
            var currentFileData = manualFiles.Where(x => x.id == currentPage.fileDataId).FirstOrDefault()
                ?? throw new Exception("Данные для страницы не найдены");
            FileData = currentFileData;
        }

        private Core.Material.Page getPageById(string id)
        {
            if (manual == null) throw new NullReferenceException(nameof(manual));

            Core.Material.Page page = null;
            foreach (var chapter in manual.chapters)
            {
                if (chapter.topics == null || chapter.topics.Count == 0) continue;
                foreach (var topic in chapter.topics)
                {
                    if (topic.didacticUnits == null || topic.didacticUnits.Count == 0) continue;
                    foreach (var unit in topic.didacticUnits)
                    {
                        if (unit.pages == null || unit.pages.Count == 0) continue;
                        page = unit.pages.FirstOrDefault(x => x.id == id);
                        if (page != null) return page;
                    }
                }
            }
            throw new Exception("Страница не найдена");
        }

        private List<int> initManualPagesDataIds()
        {
            if (manual == null) throw new NullReferenceException(nameof(manual));

            List<int> Ids = new List<int>();
            foreach (var chapter in manual.chapters)
            {
                if (chapter.topics == null || chapter.topics.Count == 0) continue;
                foreach (var topic in chapter.topics)
                {
                    if (topic.didacticUnits == null || topic.didacticUnits.Count == 0) continue;
                    foreach (var unit in topic.didacticUnits)
                    {
                        if (unit.pages == null || unit.pages.Count == 0) continue;
                        Ids.AddRange(unit.pages.Select(x => x.fileDataId));
                        manualPages.AddRange(unit.pages);
                    }
                }
            }
            return Ids;
        }

        private Core.Material.Page getFirstPage()
        {
            if (manual == null) throw new NullReferenceException(nameof(manual));

            Core.Material.Page page = null;
            foreach (var chapter in manual.chapters)
            {
                if (chapter.topics == null || chapter.topics.Count == 0) continue;
                foreach (var topic in chapter.topics)
                {
                    if (topic.didacticUnits == null || topic.didacticUnits.Count == 0) continue;
                    foreach (var unit in topic.didacticUnits)
                    {
                        if (unit.pages == null || unit.pages.Count == 0) continue;
                        page = unit.pages.FirstOrDefault();
                        if (page != null) return page;
                    }
                }
            }
            throw new Exception("Страниц в материале нет");
        }

        private async Task<GetFileResponse> downloadWord(int fileDataId, string wordPath, string xpsPath)
        {
            var fileGetRequest = new RequestModel<object>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Get,
                UrlFilter = $"/{fileDataId}"
            };
            var sender = new RequestSender<object, GetFileResponse>();
            return await sender.SendRequest(fileGetRequest, "/user/file");
        }

        private string saveXPSDoc(string wordDocName, string xpsDocName)
        {
            Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();
            wordApplication.Documents.Add(wordDocName);
            Document doc = wordApplication.ActiveDocument;
            try
            {
                doc.SaveAs(xpsDocName, WdSaveFormat.wdFormatXPS);
                wordApplication.Quit();
                return xpsDocName;
            }
            catch (Exception exp)
            {
                string str = exp.Message;
            }
            return null;
        }
        private XpsDocument getFixedDoc(string xpsFilePath)
        {
            XpsDocument xpsDoc = new XpsDocument(xpsFilePath, FileAccess.Read);
            return xpsDoc;
        }
    }
}
