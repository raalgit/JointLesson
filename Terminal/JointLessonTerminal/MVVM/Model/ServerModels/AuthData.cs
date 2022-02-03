using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Model.ServerModels
{
    public class AuthData
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
    }
}
