using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Model.ServerModels
{
    public class WorkBook
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int lessonId { get; set; }
        public string text { get; set; }
        public int page { get; set; }
    }
}
