using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JointLessonTerminal.Core.Material
{
    [Serializable]
    public class ManualData : Block, IBlock
    {

        #region Открытые поля для отправки на сервер
        /// <summary>
        /// Список авторов
        /// </summary>
        public List<Author> authors { get { return _authors; } set { _authors = value; OnPropsChanged("authors"); } }
        /// <summary>
        /// Данные о создании и зменении материала
        /// </summary>
        public MaterialDate materialDate { get; set; }
        /// <summary>
        /// Дисциплина материала
        /// </summary>
        public string discipline { get { return _discipline; } set { _discipline = value; OnPropsChanged("discipline"); } }
        /// <summary>
        /// Список авторов материала
        /// </summary>
        private List<Author> _authors;
        /// <summary>
        /// Список глав материала
        /// </summary>
        public ObservableCollection<Chapter> chapters
        {
            get { return _chapters; }
            set
            {
                _chapters = value;
                OnPropsChanged("chapters");
                foreach (var chapter in _chapters)
                {
                    chapter.OnChapterRemove += RemoveChapter;
                }
            }
        }
        #endregion

        public ManualData()
        {
            AddCommand = new RelayCommand(x => AddChapter(newItemName, newItemAccess));
            NewItemName = "Новая глава";
            NewItemAccess = 1;
        }

        #region Открытые поля, не входящие в json
        [JsonIgnore]
        public Chapter SelectedChapter
        { get { return selectedChapter; } set { selectedChapter = value; OnPropsChanged("SelectedChapter"); } }
        [JsonIgnore]
        public string NewItemName { get { return newItemName; } set { newItemName = value; OnPropsChanged("NewItemName"); } }
        [JsonIgnore]
        public int NewItemAccess { get { return newItemAccess; } set { newItemAccess = value; OnPropsChanged("NewItemAccess"); } }
        [JsonIgnore]
        public RelayCommand AddCommand { get; set; }
        #endregion

        private string _discipline;
        private ObservableCollection<Chapter> _chapters;
        private Chapter selectedChapter;
        private string newItemName;
        private int newItemAccess;


        /// <summary>
        /// Добавление новой главы
        /// </summary>
        /// <param name="name">Имя новой главы</param>
        /// <param name="access">Уровень доступа новой главы</param>
        public void AddChapter(string name, int access)
        {
            if (chapters == null) chapters = new ObservableCollection<Chapter>();

            var newChapter = new Chapter()
            {
                name = name,
                access = access,
                number = chapters.Count,
                topics = new ObservableCollection<Topic>(),
                parts = 0,
                id = Guid.NewGuid().ToString()
            };
            chapters.Add(newChapter);
            newChapter.OnChapterRemove += RemoveChapter;
            parts++;
        }

        public void RemoveChapter(object sender, EventArgs args)
        {
            var chapter = (Chapter)sender;
            if (chapter == null) return;
            chapters.Remove(chapter);
            OnPropsChanged("chapters");
            parts--;
        }
    }
}
