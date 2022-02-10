using JL.Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Settings
{
    public class UserSettings
    {
        public User User { get; set; }
        
        public UserSettings(User user)
        {
            User = user;
        }
    }
}
