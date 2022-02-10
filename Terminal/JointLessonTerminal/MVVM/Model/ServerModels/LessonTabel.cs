using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Model.ServerModels
{
    public class LessonTabel
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int lessonId { get; set; }
        public DateTime enterDate { get; set; }
        public DateTime leaveDate { get; set; }
    }
}
