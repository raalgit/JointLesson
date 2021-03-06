using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.HttpModels.Request
{
    [Serializable]
    public class ChangeLessonManualPageRequest
    {
        public int CourseId { get; set; }
        public string NextPage { get; set; }
        public bool isOnline { get; set; }
    }
}
