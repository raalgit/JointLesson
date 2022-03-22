using JointLessonTerminal.Core.File;
using JointLessonTerminal.MVVM.Model.EventModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.Material
{
    public class DidacticUnit : Block, IBlock
    {
        #region Открытые поля для отпрвки на сервер
        /// <summary>
        /// Список страниц
        /// </summary>
        public ObservableCollection<Page> pages { 
            get { return _pages; } 
            set { 
                _pages = value; 
                OnPropsChanged("pages");
                foreach (var page in _pages)
                {
                    page.OnPageRemove += RemovePage;
                }
            } 
        }
        #endregion

        public DidacticUnit()
        {
            AddCommand = new RelayCommand(x => AddPage(newItemName, newItemAccess, newItemDocId, newItemType));
            UploadCommand = new RelayCommand(async x => await UploadFile());
            RemoveCommand = new RelayCommand(x => OnUnitRemove?.Invoke(this, null));
            newItemName = "Новая страница";
            newItemAccess = 0;
            newItemType = 1;
            NewItemDocId = -1;
        }

        #region Открытые поля, не входящие в json
        [JsonIgnore]
        public Page SelectedPage { get { return selectedPage; } set { selectedPage = value; OnPropsChanged("SelectedPage"); } }
        [JsonIgnore]
        public string NewItemName { get { return newItemName; } set { newItemName = value; OnPropsChanged("NewItemName"); } }
        [JsonIgnore]
        public int NewItemAccess { get { return newItemAccess; } set { newItemAccess = value; OnPropsChanged("NewItemAccess"); } }
        [JsonIgnore]
        public int NewItemDocId { get { return newItemDocId; } set { newItemDocId = value; OnPropsChanged("NewItemDocId"); } }
        [JsonIgnore]
        public int NewItemType { get { return newItemType; } set { newItemType = value; OnPropsChanged("NewItemType"); } }
        [JsonIgnore]
        public RelayCommand AddCommand { get; set; }
        [JsonIgnore]
        public RelayCommand UploadCommand { get; set; }
        [JsonIgnore]
        public RelayCommand RemoveCommand { get; set; }
        [JsonIgnore]
        public EventHandler OnUnitRemove { get; set; }
        #endregion


        private ObservableCollection<Page> _pages;
        private Page selectedPage;
        private string newItemName;
        private int newItemAccess;
        private int newItemDocId;
        private int newItemType;
    
        /// <summary>
        /// Добавление новой страницы
        /// </summary>
        /// <param name="name"></param>
        /// <param name="access"></param>
        /// <param name="fileDataId"></param>
        /// <param name="type"></param>
        public void AddPage(string name, int access, int fileDataId, int type)
        {
            if (pages == null) pages = new ObservableCollection<Page>();
            if (newItemDocId == -1) return;

            var newPage = new Page()
            {
                name = name,
                access = access,
                number = pages.Count,
                modules = new List<Module>(),
                parts = 0,
                fileDataId = fileDataId,
                type = type,
                id = Guid.NewGuid().ToString()
            };
            pages.Add(newPage);
            OnPropsChanged("pages");
            parts++;
            newPage.OnPageRemove += RemovePage;
        }

        /// <summary>
        /// Загрузка файла страницы
        /// </summary>
        public async Task UploadFile()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".doc";
            fileDialog.Filter = "Word documents|*.doc;*.docx";
            var result = fileDialog.ShowDialog();

            if (result == true)
            {
                var sr = new System.IO.StreamReader(fileDialog.FileName);
                var fileHandler = new FileHandler();
                var bytes = default(byte[]);
                using (var memstream = new MemoryStream())
                {
                    sr.BaseStream.CopyTo(memstream);
                    bytes = memstream.ToArray();
                }
                sr.Close();
                var uploadStatus = await fileHandler.UploadFile(bytes, fileDialog.FileName);
                NewItemDocId = uploadStatus.FileId;
            }
        }

        public void RemovePage(object sender, EventArgs args)
        {
            var page = (Page)sender;
            if (page == null) return;
            pages.Remove(page);
            OnPropsChanged("pages");
            parts--;
        }
    }
}
