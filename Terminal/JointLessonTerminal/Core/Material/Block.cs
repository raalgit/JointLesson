using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.Material
{
    [Serializable]
    public class Block : ObservableObject, IBlock
    {
        public string id { get { return _id; } set { _id = value; OnPropsChanged("id"); } }
        public int access { get { return _access; } set { _access = value; OnPropsChanged("access"); } }
        public int parts { get { return _parts; } set { _parts = value; OnPropsChanged("parts"); } }
        public int number { get { return _number; } set { _number = value; OnPropsChanged("number"); } }
        public string name { get { return _name; } set { _name = value; OnPropsChanged("name"); } }

        private string _id;
        private int _access;
        private int _parts;
        private int _number;
        private string _name;
    }
}
