using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Model.ServerModels
{
    public class Test
    {
        public int Id { get; set; }
        public int BlockId { get; set; }
        public int UserId { get; set; }
        public int LessonId { get; set; }
        public DateTime SendDate { get; set; }
        public string ResultId { get; set; }
    }
}
