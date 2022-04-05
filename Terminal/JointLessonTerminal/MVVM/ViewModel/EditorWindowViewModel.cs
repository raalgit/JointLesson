using JointLessonTerminal.Core;
using JointLessonTerminal.Core.File;
using JointLessonTerminal.Core.Material;
using JointLessonTerminal.Model.ServerModels;
using JointLessonTerminal.MVVM.Model;
using JointLessonTerminal.MVVM.Model.EventModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace JointLessonTerminal.MVVM.ViewModel
{
    public class EditorWindowViewModel : ObservableObject
    {
        #region открытые поля
        public ManualData ManualData { get { return manualData; } set { manualData = value; OnPropsChanged("ManualData"); } }
        public ObservableCollection<Manual> MyManuals { get { return myManuals; } set { myManuals = value; OnPropsChanged("MyManuals"); } }
        public Manual SelectedManual
        {
            get { return selectedManual; }
            set
            {
                selectedManual = value;
                if (selectedManual != null && selectedManual.fileDataId.HasValue)
                {
                    Task.Factory.StartNew(async x =>
                    {
                        await loadMaterialData(SelectedManual);
                    }, null);
                }
            }
        }
        public bool UpdateMod
        {
            get { return updateMod; }
            set { updateMod = value; OnPropsChanged("UpdateMod"); }
        }
        public bool CreateMod
        {
            get { return createMod; }
            set { createMod = value; OnPropsChanged("CreateMod"); }
        }
        public RelayCommand CreateNewManualCommand { get; set; }
        public RelayCommand UpdateManualCommand { get; set; }
        public RelayCommand ImportManualCommand { get; set; }
        public RelayCommand ExportManualCommand { get; set; }
        public Visibility MyManualsVisibility { get { return myManualsVisibility; } set { myManualsVisibility = value; OnPropsChanged("MyManualsVisibility"); } }
        #endregion

        #region закрытые поля
        private bool offlineMod;
        private MaterialHandler materialHandler;
        private UserSettings userSettings;
        private ManualData manualData;
        private ObservableCollection<Manual> myManuals;
        private Manual selectedManual;
        private bool updateMod;
        private bool createMod;
        public Visibility myManualsVisibility;
        #endregion

        public EditorWindowViewModel()
        {

        }

        #region открытые методы
        public void InitData(bool offMod)
        {
            offlineMod = offMod;
            userSettings = UserSettings.GetInstance();
            materialHandler = new MaterialHandler();

            if (!offlineMod)
            {
                MyManualsVisibility = Visibility.Visible;
                UpdateMod = false;
                CreateMod = true;
                Task.Factory.StartNew(async () => {
                    await loadMyMaterials();
                });
            }
            else
            {
                MyManualsVisibility = Visibility.Collapsed;
                UpdateMod = false;
                CreateMod = false;
            }

            /// Шаблонная запись метариала
            ManualData = new ManualData()
            {
                authors = new List<Author>()
                {
                    new Author()
                    {
                        name = offlineMod ? "" : userSettings.CurrentUser.firstName + " " + userSettings.CurrentUser.secondName,
                        mail = "",
                        token = ""
                    }
                },
                access = 0,
                discipline = "Программная инженерия",
                name = "Материал программная инженерии",
                materialDate = new MaterialDate()
                {
                    created = DateTime.Now,
                    modified = DateTime.Now
                },
                number = 0,
                parts = 0,
                id = Guid.NewGuid().ToString(),
                chapters = new ObservableCollection<Chapter>()
            };

            CreateNewManualCommand = new RelayCommand(async x =>
            {
                bool isValid = manualIsValid();
                if (!isValid)
                {
                    System.Windows.Forms.MessageBox.Show("Проверьте, чтобы ко всем страницам были прикреплены файлы",
                                            "Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                    return;
                }
                await uploadFiles();
                bool resp = await materialHandler.SaveAtDataBase(manualData);
                if (resp) await loadMyMaterials();
            });
            UpdateManualCommand = new RelayCommand(async x =>
            {
                bool isValid = manualIsValid();
                if (!isValid)
                {
                    System.Windows.Forms.MessageBox.Show("Проверьте, чтобы ко всем страницам были прикреплены файлы",
                                            "Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                    return;
                }
                await uploadFiles();
                bool resp = await materialHandler.UpdateAtDataBase(manualData, selectedManual.fileDataId.Value);
            });
            ImportManualCommand = new RelayCommand(x => {
                using (var fileDialog = new OpenFileDialog())
                {
                    fileDialog.DefaultExt = ".jl";
                    fileDialog.Filter = "Joint lesson manual|*.jl";
                    var result = fileDialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        importManual(fileDialog.FileName);
                    }
                }
            });
            ExportManualCommand = new RelayCommand(x => {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        exportManual(fbd.SelectedPath);
                    }
                }
            });
        }
        #endregion

        #region закрытые методы
        private async Task loadMyMaterials()
        {
            MyManuals = await materialHandler.GetMyMaterials();
        }
        private async Task loadMaterialData(Manual manual)
        {
            if (manual == null) return;
            CreateMod = false;
            UpdateMod = true;
            ManualData = await materialHandler.LoadById(manual.fileDataId.Value);
        }
        private bool manualIsValid()
        {
            if (manualData == null) return false;
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
                            if (!string.IsNullOrEmpty(page.fileName) && !string.IsNullOrEmpty(page.dirPath))
                            {
                                string source = Path.Combine(page.dirPath, page.fileName);
                                if (!File.Exists(source))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
        private void exportManual(string folder)
        {
            if (manualData == null) return;
            if (string.IsNullOrEmpty(folder)) return;

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
                            if (!string.IsNullOrEmpty(page.fileName) && !string.IsNullOrEmpty(page.dirPath))
                            {
                                string source = Path.Combine(page.dirPath, page.fileName);
                                string dest = Path.Combine(folder, page.fileName);
                                if (!File.Exists(dest))
                                {
                                    try
                                    {
                                        File.Copy(source, dest);
                                        page.dirPath = folder;
                                    }
                                    catch (Exception er)
                                    {
                                        System.Windows.Forms.MessageBox.Show(er.Message,
                                            "Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            string jsonPost = JsonSerializer.Serialize<ManualData>(manualData);
            using (var file = File.Open(Path.Combine(folder, "Manual.jl"), FileMode.Create))
            {
                file.Seek(0, SeekOrigin.Begin);
                using (var stream = new StreamWriter(file, Encoding.UTF8))
                    stream.Write(jsonPost);
            }
            System.Windows.MessageBox.Show("Материал экспортирован", "Готово!");
        }
        private void importManual(string manualPath)
        {
            try
            {
                using (Stream stream = new FileStream(manualPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    if (File.Exists(manualPath) && stream.Length > 0)
                    {
                        string fileContents;
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            fileContents = reader.ReadToEnd();
                        }
                        ManualData = JsonSerializer.Deserialize<ManualData>(fileContents);
                        System.Windows.Forms.MessageBox.Show("Материал импортирован", "Готово!");
                    }
                }
            }
            catch (Exception er)
            {
                System.Windows.Forms.MessageBox.Show(er.Message, 
                    "Error",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }
        private async Task<bool> uploadFiles()
        {
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
                            if (!string.IsNullOrEmpty(page.fileName) && !string.IsNullOrEmpty(page.dirPath))
                            {
                                string source = Path.Combine(page.dirPath, page.fileName);

                                var sr = new System.IO.StreamReader(source);
                                var fileHandler = new FileHandler();
                                var bytes = default(byte[]);
                                using (var memstream = new MemoryStream())
                                {
                                    sr.BaseStream.CopyTo(memstream);
                                    bytes = memstream.ToArray();
                                }
                                sr.Close();
                                var uploadStatus = await fileHandler.UploadFile(bytes, page.fileName);
                                page.fileDataId = uploadStatus.FileId;
                            }
                        }
                    }
                }
            }
            return true;
        }
        #endregion
    }
}
