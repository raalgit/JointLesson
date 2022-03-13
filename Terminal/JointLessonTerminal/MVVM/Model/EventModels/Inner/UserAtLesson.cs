using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.EventModels.Inner
{
    [Serializable]
    public class UserAtLesson
    {
        public int UserId { get; set; }
        public bool UpHand { get; set; }
        public bool IsTeacher { get; set; }
    }
}
