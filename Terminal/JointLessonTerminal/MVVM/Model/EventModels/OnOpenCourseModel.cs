using JointLessonTerminal.Core.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.EventModels
{
    public class OnOpenCourseModel
    {
        public int CourseId { get; set; }
        public ManualData Manual { get; set; }
        public string Page { get; set; }
    }
}
