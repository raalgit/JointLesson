using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.Material
{
    [Serializable]
    public class ManualData : Block, IBlock
    {
        public List<Author> authors { get; set; }
        public MaterialDate materialDate { get; set; }
        public string discipline { get; set; }

        public List<Chapter> chapters { get; set; }
    }
}
