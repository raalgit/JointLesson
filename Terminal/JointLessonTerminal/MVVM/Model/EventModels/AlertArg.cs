using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.EventModels
{
    public class AlertArg : EventArgs
    {
        public AlertArg(string title, bool success)
        {
            Title = title;
            Success = success;
        }

        public bool Success { get; set; }
        public string Title { get; set; }
    }
}
