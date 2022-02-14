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

        #region Открытые поля для отпрвки на сервер
        /// <summary>
        /// Список тем
        /// </summary>
        public ObservableCollection<Topic> topics { 
            get { return _topics; } 
            set { 
                _topics = value;
                OnPropsChanged("topics"); 
                foreach (var topic in _topics)
                {
                    topic.OnTopicRemove += RemoveTopic;
                }
            } 
        }
        #endregion

        public Chapter()
        {
            AddCommand = new RelayCommand(x => AddTopic(newItemName, newItemAccess));
            RemoveCommand = new RelayCommand(x => OnChapterRemove?.Invoke(this, null));
            newItemName = "Новая тема";
            newItemAccess = 1;
        }

        #region Открытые поля, не входящие в json
        [JsonIgnore]
        public Topic SelectedTopic
        { get { return selectedTopic; } set { selectedTopic = value; OnPropsChanged("SelectedTopic"); } }
        [JsonIgnore]
        public string NewItemName { get { return newItemName; } set { newItemName = value; OnPropsChanged("NewItemName"); } }
        [JsonIgnore]
        public int NewItemAccess { get { return newItemAccess; } set { newItemAccess = value; OnPropsChanged("NewItemAccess"); } }
        [JsonIgnore]
        public RelayCommand AddCommand { get; set; }
        [JsonIgnore]
        public RelayCommand RemoveCommand { get; set; }
        [JsonIgnore]
        public EventHandler OnChapterRemove { get; set; }
        #endregion

        private ObservableCollection<Topic> _topics;
        private Topic selectedTopic;
        private string newItemName;
        private int newItemAccess;

        

        /// <summary>
        /// Добавление новой темы
        /// </summary>
        /// <param name="name">Название новой темы</param>
        /// <param name="access">Уровень доступа новой темы</param>
        public void AddTopic(string name, int access)
        {
            if (topics == null) topics = new ObservableCollection<Topic>();
            var newTopic = new Topic()
            {
                name = name,
                access = access,
                number = topics.Count,
                didacticUnits = new ObservableCollection<DidacticUnit>(),
                parts = 0,
                id = Guid.NewGuid().ToString()
            };

            topics.Add(newTopic);
            parts++;
            newTopic.OnTopicRemove += RemoveTopic;
            OnPropsChanged("Chapter");
        }

        public void RemoveTopic(object sender, EventArgs args)
        {
            var topic = (Topic)sender;
            if (topic == null) return;
            topics.Remove(topic);
            OnPropsChanged("topics");
            parts--;
        }
    }
}
