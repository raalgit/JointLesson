using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Model.ServerModels
{
    public class Test
    {
        public int id { get; set; }
        public int blockId { get; set; }
        public int userId { get; set; }
        public int lessonId { get; set; }
        public DateTime sendDate { get; set; }
        public string resultId { get; set; }
    }
}
