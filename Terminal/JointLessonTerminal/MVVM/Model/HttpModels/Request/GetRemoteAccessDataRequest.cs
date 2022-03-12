using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.HttpModels.Request
{
    [Serializable]
    public class GetRemoteAccessDataRequest
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
    }
}
