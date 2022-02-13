using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.Material
{
    public class Module : Block, IBlock
    {
        public int fileDataId { get; set; }
        public int type { get; set; }
    }
}
