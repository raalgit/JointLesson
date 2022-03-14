﻿using JointLessonTerminal.Core;
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
using JointLessonTerminal.MVVM.View;
using System.Collections.ObjectModel;
using JointLessonTerminal.MVVM.Model.EventModels.Inner;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace JointLessonTerminal.MVVM.ViewModel
{
    public class LessonWindowViewModel : ObservableObject
    {
        private ManualData manual;
        public ManualData Manual { get; set; }

        private List<UserAtLesson> usersAtLesson;
        public List<UserAtLesson> UsersAtLesson { get { return usersAtLesson; } set { usersAtLesson = value; OnPropsChanged("UsersAtLesson"); } }

        public Visibility NextPageBtnVisibility { get { return nextPageBtnVisibility; } set { nextPageBtnVisibility = value; OnPropsChanged("NextPageBtnVisibility"); } }
        private Visibility nextPageBtnVisibility;
        public Visibility PrevPageBtnVisibility { get { return prevPageBtnVisibility; } set { prevPageBtnVisibility = value; OnPropsChanged("PrevPageBtnVisibility"); } }
        private Visibility prevPageBtnVisibility;

        private Visibility offlineManualVisibility;
        public Visibility OfflineManualVisibility { get { return offlineManualVisibility; } set { offlineManualVisibility = value; OnPropsChanged("OfflineManualVisibility"); } }

        private int courseId;

        private string currentPageId;
        private Core.Material.Page currentPage;

        private string currentOfflinePageId;
        private Core.Material.Page currentOfflinePage;

        private List<FileData> manualFiles;
        private List<Core.Material.Page> manualPages;


        private FixedDocumentSequence activeDocument;
        public FixedDocumentSequence ActiveDocument { get { return activeDocument; } set { activeDocument = value; OnPropsChanged("ActiveDocument"); } }

        private FixedDocumentSequence activeOfflineDocument;
        public FixedDocumentSequence ActiveOfflineDocument { get { return activeOfflineDocument; } set { activeOfflineDocument = value; OnPropsChanged("ActiveOfflineDocument"); } }

        private readonly string wordDirPath;
        
        private string documentXpsPath;
        private string documentWordPath;

        private SignalHub hub;
        private FixedDocumentSequence sequence;
        private FixedDocumentSequence offlineSequence;

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

        private FileData fileDataOffline;
        public FileData FileDataOffline
        {
            get
            {
                return fileDataOffline;
            }
            set
            {
                fileDataOffline = value;

                if (fileDataOffline.mongoName.Contains(".doc"))
                {
                    var wordPath = Path.Combine(wordDirPath, fileDataOffline.mongoName.Replace(":", "-"));
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
                                    offlineSequence = getFixedDoc(documentXpsPath).GetFixedDocumentSequence();
                                    ActiveOfflineDocument = offlineSequence;
                                });
                            }
                        }, new System.Threading.CancellationToken());
                    }
                    else
                    {
                        offlineSequence = getFixedDoc(xpsPath).GetFixedDocumentSequence();
                        ActiveOfflineDocument = offlineSequence;
                    }
                }
            }
        }

        public RelayCommand NextOfflinePageCommand { get; set; }
        public RelayCommand PrevOfflinePageCommand { get; set; }
        
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PrevPageCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand UpHandButton { get; set; }
        public RelayCommand OpenRemoteTerminalCommand { get; set; }
        public RelayCommand ShowOfflineManual { get; set; }

        public RelayCommand NoteOpenCommand { get; set; }
        public RelayCommand NoteSaveCommand { get; set; }
        public RelayCommand NoteBoldCommand { get; set; }
        public RelayCommand NoteItallicCommand { get; set; }
        public RelayCommand NoteUnderLineCommand { get; set; }
        

        private TextSelection selection;
        public TextSelection Selection { 
            get 
            { 
                return selection; 
            } 
            set 
            { 
                selection = value;
                var prop = selection.GetPropertyValue(Inline.FontWeightProperty);
                BoldIsChecked = (prop != DependencyProperty.UnsetValue) && (prop.Equals(FontWeights.Bold));
                
                var prop2 = selection.GetPropertyValue(Inline.FontStyleProperty);
                ItalicIsChecked = (prop2 != DependencyProperty.UnsetValue) && (prop2.Equals(FontStyles.Italic));

                UnderlineIsChecked = false;
                var prop3 = selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection;
                if (prop3 != null && prop3 != DependencyProperty.UnsetValue && prop3.Count > 0)
                {
                    if (prop3.Contains(TextDecorations.Underline[0])){
                        UnderlineIsChecked = true;
                    }
                }

                var prop4 = selection.GetPropertyValue(Inline.FontSizeProperty) as double?;
                if (prop4.HasValue && prop4.Value > 0)
                {
                    NoteTextSizeSelected = prop4.Value;
                }

                var prop5 = selection.GetPropertyValue(Inline.FontFamilyProperty) as System.Windows.Media.FontFamily;
                if (prop5 != null && prop5 != DependencyProperty.UnsetValue)
                {
                    NoteFontSelected = prop5;
                }
            } 
        }

        public ReadOnlyCollection<System.Windows.Media.FontFamily> NoteFonts { get; set; } = (ReadOnlyCollection<System.Windows.Media.FontFamily>)System.Windows.Media.Fonts.SystemFontFamilies;
        private System.Windows.Media.FontFamily noteFontSelected;
        public System.Windows.Media.FontFamily NoteFontSelected
        {
            get { return noteFontSelected; }
            set
            {
                if (selection != null)
                {
                    selection.ApplyPropertyValue(Inline.FontFamilyProperty, value);
                }
                noteFontSelected = value;
                OnPropsChanged("NoteFontSelected");
            }
        }

        public List<double> NoteTextSizes { get; set; } = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        private double noteTextSizeSelected;
        public double NoteTextSizeSelected
        {
            get { return noteTextSizeSelected; }
            set
            {
                if (selection != null) 
                {
                    selection.ApplyPropertyValue(Inline.FontSizeProperty, value);
                }
                noteTextSizeSelected = value;
                OnPropsChanged("NoteTextSizeSelected");
            }
        }


        private bool boldIsChecked;
        public bool BoldIsChecked { get { return boldIsChecked; } set { boldIsChecked = value; OnPropsChanged("BoldIsChecked"); } }

        private bool italicIsChecked;
        public bool ItalicIsChecked { get { return italicIsChecked; } set { italicIsChecked = value; OnPropsChanged("ItalicIsChecked"); } }

        private bool underlineIsChecked;
        public bool UnderlineIsChecked { get { return underlineIsChecked; } set { underlineIsChecked = value; OnPropsChanged("UnderlineIsChecked"); } }

        public RemoteTerminalViewModel RemoteTerminalVM { get; set; }

        public LessonWindowViewModel()
        {
            wordDirPath = AppDomain.CurrentDomain.BaseDirectory + "WORD";
            if (!Directory.Exists(wordDirPath))
            {
                Directory.CreateDirectory(wordDirPath);
            }

            NextPageCommand = new RelayCommand(async x => await nextPage(true, true));
            PrevPageCommand = new RelayCommand(async x => await nextPage(false, true));
            NextOfflinePageCommand = new RelayCommand(async x => await nextPage(true, false));
            PrevOfflinePageCommand = new RelayCommand(async x => await nextPage(false, false));
            ShowOfflineManual = new RelayCommand(x =>
            {
                if (OfflineManualVisibility == Visibility.Visible) OfflineManualVisibility = Visibility.Collapsed;
                else OfflineManualVisibility = Visibility.Visible;
            });
            NoteBoldCommand = new RelayCommand(x =>
            {
                if (selection != null)
                {
                    var textWeight = selection.GetPropertyValue(Inline.FontWeightProperty);

                    if (textWeight != null && textWeight != DependencyProperty.UnsetValue)
                    {
                        if ((FontWeight)textWeight == FontWeights.Normal) 
                        {
                            textWeight = FontWeights.Bold;
                        }
                        else
                        {
                            textWeight = FontWeights.Normal;
                        }
                    }
                    selection.ApplyPropertyValue(Inline.FontWeightProperty, textWeight);
                }
            });
            NoteItallicCommand = new RelayCommand(x =>
            {
                if (selection != null)
                {
                    var textStyle = selection.GetPropertyValue(Inline.FontStyleProperty);

                    if (textStyle != null && textStyle != DependencyProperty.UnsetValue)
                    {
                        if ((FontStyle)textStyle == FontStyles.Normal)
                        {
                            textStyle = FontStyles.Italic;
                        }
                        else
                        {
                            textStyle = FontStyles.Normal;
                        }
                    }
                    selection.ApplyPropertyValue(Inline.FontStyleProperty, textStyle);
                }
            });
            NoteUnderLineCommand = new RelayCommand(x =>
            {
                if (selection != null)
                {
                    var textDecorations = new TextDecorationCollection();
                    textDecorations.Add(selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection);

                    var prop3 = selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection;
                    if (prop3 != null && prop3 != DependencyProperty.UnsetValue)
                    {
                        if (prop3.Count > 0 && prop3.Contains(TextDecorations.Underline[0]))
                        {
                            textDecorations.Remove(TextDecorations.Underline[0]);
                        }
                        else
                        {
                            textDecorations.Add(TextDecorations.Underline);
                        }
                    }
                    selection.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
                }
            });
            NoteSaveCommand = new RelayCommand(x =>
            {
                var document = x as FlowDocument;
                if (document != null)
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
                    var answere = dlg.ShowDialog();
                    if (answere == DialogResult.OK || answere == DialogResult.Yes)
                    {
                        FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                        TextRange range = new TextRange(document.ContentStart, document.ContentEnd);
                        range.Save(fileStream, System.Windows.Forms.DataFormats.Rtf);
                    }
                }
            });
            NoteOpenCommand = new RelayCommand(x =>
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
                var answere = dlg.ShowDialog();
                if (answere == DialogResult.OK || answere == DialogResult.Yes)
                {
                    FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                    TextRange range = new TextRange((x as FlowDocument).ContentStart, (x as FlowDocument).ContentEnd);
                    range.Load(fileStream, System.Windows.Forms.DataFormats.Rtf);
                }
            });
            ExitCommand = new RelayCommand(x => exit());
            UpHandButton = new RelayCommand(async x => {
                var req = new UpHandRequest()
                {
                    CourseId = courseId
                };
                var resp = await upHand(req); 
            });
            OpenRemoteTerminalCommand = new RelayCommand(x => openRemoteTerminal());
        }

        public void InitData(OnOpenCourseModel data)
        {
            manualPages = new List<Core.Material.Page>();
            manual = data.Manual;
            courseId = data.CourseId;
            currentPageId = data.Page;
            currentOfflinePageId = data.Page;

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
            hub.OnLessonUserListUpdate += onLessonUserListUpdateEvent;

            if (hub.IsConnected) onSignalRConnected(null, null);
            else hub.OnConnected += onSignalRConnected;

            try
            {
                Task.Factory.StartNew(async x => await loadManualFiles(), null);
            }
            catch (Exception er)
            {
                var signal = new WindowEvent();
                signal.Type = WindowEventType.LESSON_LOADMANUALERROR;
                signal.Argument = er.Message;
                SendEventSignal(signal);
            }
        }
        private void onSignalRConnected(object o, EventArgs e)
        {
            Task.Factory.StartNew(async x =>
            {
                var req = new JoinLessonRequest()
                {
                    CourseId = courseId
                };
                var resp = await JoinLesson(req);
            }, null);
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
                    showWordPage(true);
                }
            }
        }
        private void onLessonUserListUpdateEvent(object o, EventArgs e)
        {
            var arg = e as OnLessonUserListUpdateArg;
            if (arg != null)
            {
                UsersAtLesson = arg.UserAtLessons;
            }
        }
        private async Task<JoinLessonResponse> JoinLesson(JoinLessonRequest request)
        {
            var joinLessonRequest = new RequestModel<JoinLessonRequest>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Post,
                Body = request
            };
            var sender = new RequestSender<JoinLessonRequest, JoinLessonResponse>();
            var responsePost = await sender.SendRequest(joinLessonRequest, "/user/join-lesson");
            if (!responsePost.isSuccess)
            {
                System.Windows.Forms.MessageBox.Show("Error");
            }
            return responsePost;
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
                
                currentOfflinePage = currentPage;
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    showWordPage(true);
                    showWordPage(false);
                });
            }
        }
        private void openRemoteTerminal()
        {
            var terminal = new RemoteTerminalWindow(courseId);
            terminal.Show();
        }
        private async Task<UpHandResponse> upHand(UpHandRequest request)
        {
            var upHandRequest = new RequestModel<UpHandRequest>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Post,
                Body = request
            };
            var sender = new RequestSender<UpHandRequest, UpHandResponse>();
            var responsePost = await sender.SendRequest(upHandRequest, "/user/up-hand");
            if (!responsePost.isSuccess)
            {
                MessageBox.Show("Error");
            }
            return responsePost;
        }
        private void exit()
        {
            var signal = new WindowEvent();
            signal.Type = WindowEventType.EXITFROMLESSON;
            signal.Argument = courseId;
            SendEventSignal(signal);

            Task.Factory.StartNew(async () =>
            {
                var req = new LeaveLessonRequest()
                {
                    CourseId = courseId
                };
                var resp = await leaveFromLesson(req);
            });
        }
        private async Task<LeaveLessonResponse> leaveFromLesson(LeaveLessonRequest request)
        {
            var leaveLessonRequest = new RequestModel<LeaveLessonRequest>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Post,
                Body = request
            };
            var sender = new RequestSender<LeaveLessonRequest, LeaveLessonResponse>();
            var responsePost = await sender.SendRequest(leaveLessonRequest, "/user/leave-lesson");
            if (!responsePost.isSuccess)
            {
                MessageBox.Show("Error");
            }
            return responsePost;
        }
        private async Task <bool> nextPage(bool forward, bool online)
        {
            if (online)
            {
                var hasHand = usersAtLesson.Any(x => x.UpHand);
                if (hasHand)
                {
                    var answere =
                        MessageBox.Show(
                            "Один или несколько участников занятия подняли руку. Вы уверены, что хотите перейти на новую страницу?",
                            "Подтверждение перехода",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                    if (answere == DialogResult.Cancel || answere == DialogResult.No)
                    {
                        return false;
                    }
                }
            }

            var currentPageIndex = -1;

            if (online) currentPageIndex = manualPages.FindIndex(x => x.id == currentPageId);
            else currentPageIndex = manualPages.FindIndex(x => x.id == currentOfflinePageId);

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

            if (online)
            {
                currentPage = manualPages.ElementAt(nextPageIndex);
                currentPageId = currentPage.id;
                var response = await SyncPage();
            }
            else
            {
                currentOfflinePage = manualPages.ElementAt(nextPageIndex);
                currentOfflinePageId = currentOfflinePage.id;
            }

            showWordPage(online);
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
                var signal = new WindowEvent();
                signal.Type = WindowEventType.LESSON_SYNCERROR;
                signal.Argument = "Возникла ошибка при синхронизации страницы!";
                SendEventSignal(signal);
            }
            return responsePost;
        }
        private void showWordPage(bool online)
        {
            if (online)
            {
                var currentFileData = manualFiles.Where(x => x.id == currentPage.fileDataId).FirstOrDefault()
                    ?? throw new Exception("Данные для страницы не найдены");
                FileData = currentFileData;
            }
            else
            {
                var currentFileData = manualFiles.Where(x => x.id == currentOfflinePage.fileDataId).FirstOrDefault()
                    ?? throw new Exception("Данные для страницы не найдены");
                FileDataOffline = currentFileData;
            }
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
