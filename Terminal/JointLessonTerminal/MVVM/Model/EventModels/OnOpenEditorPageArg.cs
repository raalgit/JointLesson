using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.EventModels
{
    public class OnOpenEditorPageArg : EventArgs
    {
        public bool Offline { get; set; } = false;
    }
}
