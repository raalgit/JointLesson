using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.Material
{
    public class Topic : Block, IBlock
    {
        /// <summary>
        /// Список дидактических единиц
        /// </summary>
        public ObservableCollection<DidacticUnit> didacticUnits { get { return _didacticUnits; } set { _didacticUnits = value; OnPropsChanged("didacticUnits"); } }
        private ObservableCollection<DidacticUnit> _didacticUnits;

        public Topic()
        {
            AddCommand = new RelayCommand(x => AddUnit(newItemName, newItemAccess));
            newItemName = "Новая единица";
            NewItemAccess = 1;
        }

        /// <summary>
        /// Выбранная дидактическая единица
        /// </summary>
        [JsonIgnore]
        public DidacticUnit SelectedDidacticUnit { 
            get { return selectedDidacticUnit; } 
            set { selectedDidacticUnit = value; OnPropsChanged("SelectedDidacticUnit"); OnPropsChanged("AddingNewPageVisibility"); } 
        }
        private DidacticUnit selectedDidacticUnit;

        /// <summary>
        /// Название новой единицы
        /// </summary>
        [JsonIgnore]
        public string NewItemName { get { return newItemName; } set { newItemName = value; OnPropsChanged("NewItemName"); } }
        private string newItemName;

        /// <summary>
        /// Уровень доступа новой единицы
        /// </summary>
        [JsonIgnore]
        public int NewItemAccess { get { return newItemAccess; } set { newItemAccess = value; OnPropsChanged("NewItemAccess"); } }
        private int newItemAccess;

        [JsonIgnore]
        public RelayCommand AddCommand { get; set; }

        /// <summary>
        /// Добавление новой единицы
        /// </summary>
        /// <param name="name">Название новой единицы</param>
        /// <param name="access">Уровень доступа новой единицы</param>
        public void AddUnit(string name, int access)
        {
            if (didacticUnits == null) didacticUnits = new ObservableCollection<DidacticUnit>();

            didacticUnits.Add(new DidacticUnit()
            {
                name = name,
                access = access,
                number = didacticUnits.Count,
                pages = new ObservableCollection<Page>(),
                parts = 0,
                id = Guid.NewGuid().ToString()
            });

            parts++;
            OnPropsChanged("didacticUnits");
        }
    }
}
