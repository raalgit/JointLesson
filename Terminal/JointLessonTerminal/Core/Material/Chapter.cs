using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.Material
{
    [Serializable]
    public class Chapter : Block, IBlock
    {
        public List<Topic> topics { get; set; }
    }
}
