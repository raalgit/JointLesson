using JointLessonTerminal.MVVM.View;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Xps.Packaging;

namespace JointLessonTerminal.Core.Material
{
    public class Page : Block, IBlock
    {
        #region Открытые поля для отпрвки на сервер
        public int type { get { return _type; } set { _type = value; OnPropsChanged("type"); } }
        public int fileDataId { get { return _fileDataId; } set { _fileDataId = value; OnPropsChanged("fileDataId"); } }
        public string dirPath { get { return _dirPath; } set { _dirPath = value; OnPropsChanged("DirPath"); } }
        public string fileName { get { return _fileName; } set { _fileName = value; OnPropsChanged("FileName"); } }
        public List<Module> modules { get; set; }
        #endregion

        public Page()
        {
            RemoveCommand = new RelayCommand(x => OnPageRemove?.Invoke(this, null));
            SelectCommand = new RelayCommand(x => OnPageSelected?.Invoke(this, null));
            ShowPageCommand = new RelayCommand(x =>
            {
                if (!string.IsNullOrEmpty(dirPath) && !string.IsNullOrEmpty(fileName))
                {
                    var previewWin = new WordDocumentPreviewWindow();
                    previewWin.Show();
                    if (fileName.Contains(".doc"))
                    {
                        var xpsPath = Path.Combine(dirPath, fileName) + ".xps";

                        if (!System.IO.File.Exists(xpsPath))
                        {
                            saveXPSDoc(Path.Combine(dirPath, fileName), xpsPath);
                        }

                        if (System.IO.File.Exists(xpsPath))
                        {
                            var sequence = getFixedDoc(xpsPath).GetFixedDocumentSequence();
                            previewWin.SetDocument(sequence);
                        }
                    }
                }
            });
            ChooseFile = new RelayCommand(x =>
            {
                var fileDialog = new OpenFileDialog();
                fileDialog.DefaultExt = ".doc";
                fileDialog.Filter = "Word documents|*.doc;*.docx";
                var result = fileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    dirPath = Path.GetDirectoryName(fileDialog.FileName);
                    fileName = Path.GetFileName(fileDialog.FileName);
                    fileDataId = -1;
                }
            });
        }

        #region Открытые поля, не входящие в json
        [JsonIgnore]
        public RelayCommand RemoveCommand { get; set; }
        [JsonIgnore]
        public RelayCommand ChooseFile { get; set; }
        [JsonIgnore]
        public RelayCommand ShowPageCommand { get; set; }
        [JsonIgnore]
        public RelayCommand SelectCommand { get; set; }
        [JsonIgnore]
        public EventHandler OnPageRemove { get; set; }
        [JsonIgnore]
        public EventHandler OnPageSelected { get; set; }
        #endregion

        private string _dirPath;
        private string _fileName;
        private int _type;
        private int _fileDataId;

        private void saveXPSDoc(string wordDocName, string xpsDocName)
        {
            try
            {
                Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();
                wordApplication.Documents.Add(wordDocName);
                Document doc = wordApplication.ActiveDocument;
                doc.SaveAs(xpsDocName, WdSaveFormat.wdFormatXPS);
                wordApplication.Quit();
                MessageBox.Show(
                    $"Сгенерирован промежуточный файл {xpsDocName} для отображения WORD документа.\n" +
                    $"При необходимости файл может быть удален после завершения работы", "XPS документ", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private XpsDocument getFixedDoc(string xpsFilePath)
        {
            XpsDocument xpsDoc = new XpsDocument(xpsFilePath, FileAccess.Read);
            return xpsDoc;
        }
    }
}
