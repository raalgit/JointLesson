using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.Material
{
    public class Page : Block, IBlock
    {
        public int type { get; set; }
        public string doc { get; set; }
        public List<Module> modules { get; set; }
    }
}
