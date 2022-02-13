using JointLessonTerminal.Core.File;
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
        /// <summary>
        /// Список страниц
        /// </summary>
        public ObservableCollection<Page> pages { get { return _pages; } set { _pages = value; OnPropsChanged("pages"); } }
        private ObservableCollection<Page> _pages;

        public DidacticUnit()
        {
            AddCommand = new RelayCommand(x => AddPage(newItemName, newItemAccess, newItemDocId, newItemType));
            UploadCommand = new RelayCommand(async x => await UploadFile());
            newItemName = "Новая страница";
            newItemAccess = 0;
            newItemType = 1;
            NewItemDocId = -1;
        }

        /// <summary>
        /// Выбранная страница
        /// </summary>
        [JsonIgnore]
        public Page SelectedPage { get { return selectedPage; } set { selectedPage = value; OnPropsChanged("SelectedPage"); } }
        private Page selectedPage;

        /// <summary>
        /// Название новой страницы
        /// </summary>
        [JsonIgnore]
        public string NewItemName { get { return newItemName; } set { newItemName = value; OnPropsChanged("NewItemName"); } }
        private string newItemName;

        /// <summary>
        /// Уровень доступа новой страницы
        /// </summary>
        [JsonIgnore]
        public int NewItemAccess { get { return newItemAccess; } set { newItemAccess = value; OnPropsChanged("NewItemAccess"); } }
        private int newItemAccess;

        /// <summary>
        /// Номер документа для новой страницы
        /// </summary>
        [JsonIgnore]
        public int NewItemDocId { get { return newItemDocId; } set { newItemDocId = value; OnPropsChanged("NewItemDocId"); } }
        private int newItemDocId;

        /// <summary>
        /// Тип новой страницы
        /// </summary>
        [JsonIgnore]
        public int NewItemType { get { return newItemType; } set { newItemType = value; OnPropsChanged("NewItemType"); } }
        private int newItemType;

        [JsonIgnore]
        public RelayCommand AddCommand { get; set; }
        [JsonIgnore]
        public RelayCommand UploadCommand { get; set; }

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

            pages.Add(new Page()
            {
                name = name,
                access = access,
                number = pages.Count,
                modules = new List<Module>(),
                parts = 0,
                fileDataId = fileDataId,
                type = type, 
                id = Guid.NewGuid().ToString()
            });

            parts++;
            OnPropsChanged("pages");
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
                NewItemDocId = await fileHandler.UploadFile(bytes, fileDialog.FileName);
            }
        }
    }
}
