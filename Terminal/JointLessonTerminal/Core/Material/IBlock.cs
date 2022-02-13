using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.Material
{
    public interface IBlock
    {
        string id { get; set; }
        int access { get; set; }
        int parts { get; set; }
        int number { get; set; }
        string name { get; set; }
    }
}
