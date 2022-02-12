using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Model.ServerModels
{
    public class Manual
    {
        public int id { get; set; }
        public int? fileDataId { get; set; }
        public int authorId { get; set; }
        public string title { get; set; }
    }
}
