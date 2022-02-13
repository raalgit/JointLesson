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
        public string id { get; set; }
        public int access { get; set; }
        public int parts { get; set; }
        public int number { get; set; }
        public string name { get; set; }
    }
}
