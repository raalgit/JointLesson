using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Model.ServerModels
{
    public class Statistic
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SuccessExecution { get; set; }
        public string FailedExecution { get; set; }
    }
}
