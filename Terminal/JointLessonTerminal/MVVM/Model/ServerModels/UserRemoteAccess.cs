using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.ServerModels
{
    [Serializable]
    public class UserRemoteAccess
    {
        public int id { get; set; }
        public int courseId { get; set; }
        public int userId { get; set; }
        public string connectionData { get; set; }
        public DateTime startDate { get; set; }
    }
}
