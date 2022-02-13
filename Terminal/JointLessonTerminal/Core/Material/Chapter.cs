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
    public class Chapter : Block, IBlock
    {
        /// <summary>
        /// Список тем
        /// </summary>
        public ObservableCollection<Topic> topics { get { return _topics; } set { _topics = value; OnPropsChanged("topics"); } }
        private ObservableCollection<Topic> _topics;

        public Chapter()
        {
            AddCommand = new RelayCommand(x => AddTopic(newItemName, newItemAccess));
            newItemName = "Новая тема";
            newItemAccess = 1;
        }

        /// <summary>
        /// Выбранная тема
        /// </summary>
        [JsonIgnore]
        public Topic SelectedTopic { 
            get { return selectedTopic; } 
            set { selectedTopic = value; OnPropsChanged("SelectedTopic"); } 
        }
        private Topic selectedTopic;

        [JsonIgnore]
        public Visibility AddingNewUnitVisibility { get; set; }

        /// <summary>
        /// Имя новой темы
        /// </summary>
        [JsonIgnore]
        public string NewItemName { get { return newItemName; } set { newItemName = value; OnPropsChanged("NewItemName"); } }
        private string newItemName;

        /// <summary>
        /// Уровень доступа новой темы
        /// </summary>
        [JsonIgnore]
        public int NewItemAccess { get { return newItemAccess; } set { newItemAccess = value; OnPropsChanged("NewItemAccess"); } }
        private int newItemAccess;

        [JsonIgnore]
        public RelayCommand AddCommand { get; set; }

        /// <summary>
        /// Добавление новой темы
        /// </summary>
        /// <param name="name">Название новой темы</param>
        /// <param name="access">Уровень доступа новой темы</param>
        public void AddTopic(string name, int access)
        {
            if (topics == null) topics = new ObservableCollection<Topic>();

            topics.Add(new Topic()
            {
                name = name,
                access = access,
                number = topics.Count,
                didacticUnits = new ObservableCollection<DidacticUnit>(),
                parts = 0,
                id = Guid.NewGuid().ToString()
            });

            parts++;
            OnPropsChanged("Chapter");
        }
    }
}
