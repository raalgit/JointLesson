using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.EventModels
{
    public class OnPageChangeEventArg : EventArgs
    {
        public string NewPageId { get; set; }
    }
}
