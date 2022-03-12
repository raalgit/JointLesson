using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.HttpModels.Request
{
    [Serializable]
    public class CreateRemoteAccessRequest
    {
        public string ConnectionData { get; set; }
        public int CourseId { get; set; }
    }
}
