using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.Material
{
    public class Page : Block, IBlock
    {
        public int type { get { return _type; } set { _type = value; OnPropsChanged("type"); } }
        private int _type;

        public int fileDataId { get { return _fileDataId; } set { _fileDataId = value; OnPropsChanged("fileDataId"); } }
        private int _fileDataId;

        public List<Module> modules { get; set; }
    }
}
