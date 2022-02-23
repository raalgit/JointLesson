using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.Material
{
    public class Page : Block, IBlock
    {
        #region Открытые поля для отпрвки на сервер
        public int type { get { return _type; } set { _type = value; OnPropsChanged("type"); } }
        public int fileDataId { get { return _fileDataId; } set { _fileDataId = value; OnPropsChanged("fileDataId"); } }
        public List<Module> modules { get; set; }
        #endregion

        public Page()
        {
            RemoveCommand = new RelayCommand(x => OnPageRemove?.Invoke(this, null));
            SelectCommand = new RelayCommand(x => OnPageSelected?.Invoke(this, null));
        }

        #region Открытые поля, не входящие в json
        [JsonIgnore]
        public RelayCommand RemoveCommand { get; set; }
        [JsonIgnore]
        public RelayCommand SelectCommand { get; set; }
        [JsonIgnore]
        public EventHandler OnPageRemove { get; set; }
        [JsonIgnore]
        public EventHandler OnPageSelected { get; set; }
        #endregion

        private int _type;
        private int _fileDataId;
    }
}
