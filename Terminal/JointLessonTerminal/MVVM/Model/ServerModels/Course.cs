using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Model.ServerModels
{
    public class Course
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int creatorId { get; set; }
        public string avatarId { get; set; }
        public int manualId { get; set; }
        public int disciplineId { get; set; }
    }
}
