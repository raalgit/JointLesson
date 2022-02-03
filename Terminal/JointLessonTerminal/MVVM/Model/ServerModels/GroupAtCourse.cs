using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Model.ServerModels
{
    public class GroupAtCourse
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int GroupId { get; set; }
        public int LastMaterialPage { get; set; }
        public bool IsActive { get; set; }
    }
}
