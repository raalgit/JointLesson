using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Model.ServerModels
{
    public class Statistic
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string successExecution { get; set; }
        public string failedExecution { get; set; }
    }
}
