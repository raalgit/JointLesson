using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.Material
{
    public class DidacticUnit : Block, IBlock
    {
        public List<Page> pages { get; set; }
    }
}
