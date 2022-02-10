using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Model.ServerModels
{
    public class Lesson
    {
        public int id { get; set; }
        public int groupAtCourseId { get; set; }
        public int lastMaterialPage { get; set; }
        public DateTime startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int teacherId { get; set; }
        public InnerModels.LessonType type { get; set; }
    }
}
