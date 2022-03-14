using JointLessonTerminal.MVVM.Model.EventModels.Inner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.EventModels
{
    public class OnLessonUserListUpdateArg : EventArgs
    {
        public List<UserAtLesson> UserAtLessons { get; set; }
    }
}
