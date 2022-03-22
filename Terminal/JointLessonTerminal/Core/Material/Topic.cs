using JointLessonTerminal.MVVM.Model.EventModels;
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
        #region Открытые поля для отпрвки на сервер
        /// <summary>
        /// Список дидактических единиц
        /// </summary>
        public ObservableCollection<DidacticUnit> didacticUnits { 
            get { return _didacticUnits; } 
            set { 
                _didacticUnits = value; 
                OnPropsChanged("didacticUnits");
                foreach(var unit in _didacticUnits)
                {
                    unit.OnUnitRemove += RemoveUnit;
                }
            } 
        }
        #endregion

        public Topic()
        {
            AddCommand = new RelayCommand(x => AddUnit(newItemName, newItemAccess));
            RemoveCommand = new RelayCommand(x => OnTopicRemove?.Invoke(this, null));
            newItemName = "Новая единица";
            NewItemAccess = 1;
        }

        #region Открытые поля, не входящие в json
        [JsonIgnore]
        public DidacticUnit SelectedDidacticUnit
        { 
            get { return selectedDidacticUnit; }
            set { 
                selectedDidacticUnit = value; 
                OnPropsChanged("SelectedDidacticUnit"); 
                OnPropsChanged("AddingNewPageVisibility"); 
            }
        }
        [JsonIgnore]
        public string NewItemName { get { return newItemName; } set { newItemName = value; OnPropsChanged("NewItemName"); } }
        [JsonIgnore]
        public int NewItemAccess { get { return newItemAccess; } set { newItemAccess = value; OnPropsChanged("NewItemAccess"); } }

        private ObservableCollection<DidacticUnit> _didacticUnits;
        [JsonIgnore]
        public RelayCommand AddCommand { get; set; }
        [JsonIgnore]
        public RelayCommand RemoveCommand { get; set; }
        [JsonIgnore]
        public EventHandler OnTopicRemove { get; set; }
        #endregion

        private DidacticUnit selectedDidacticUnit;
        private string newItemName;
        private int newItemAccess;

        /// <summary>
        /// Добавление новой единицы
        /// </summary>
        /// <param name="name">Название новой единицы</param>
        /// <param name="access">Уровень доступа новой единицы</param>
        public void AddUnit(string name, int access)
        {
            if (didacticUnits == null) didacticUnits = new ObservableCollection<DidacticUnit>();

            var newDidacticUnit = new DidacticUnit()
            {
                name = name,
                access = access,
                number = didacticUnits.Count,
                pages = new ObservableCollection<Page>(),
                parts = 0,
                id = Guid.NewGuid().ToString()
            };
            didacticUnits.Add(newDidacticUnit);
            OnPropsChanged("didacticUnits");
            newDidacticUnit.OnUnitRemove += RemoveUnit;
            parts++;
            
        }

        public void RemoveUnit(object sender, EventArgs args)
        {
            var unit = (DidacticUnit)sender;
            if (unit == null) return;
            didacticUnits.Remove(unit);
            OnPropsChanged("didacticUnits");
            parts--;
        }
    }
}
