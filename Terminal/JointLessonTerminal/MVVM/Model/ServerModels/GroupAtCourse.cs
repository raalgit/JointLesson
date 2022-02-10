using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Model.ServerModels
{
    public class GroupAtCourse
    {
        public int id { get; set; }
        public int courseId { get; set; }
        public int groupId { get; set; }
        public int lastMaterialPage { get; set; }
        public bool isActive { get; set; }
    }
}
