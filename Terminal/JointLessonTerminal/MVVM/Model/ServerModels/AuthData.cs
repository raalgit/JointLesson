using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Model.ServerModels
{
    public class AuthData
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string login { get; set; }
        public string passwordHash { get; set; }
    }
}
