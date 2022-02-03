using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Model.ServerModels
{
    public class Lesson
    {
        public int Id { get; set; }
        public int GroupAtCourseId { get; set; }
        public int LastMaterialPage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TeacherId { get; set; }
        public InnerModels.LessonType Type { get; set; }
    }
}
