using JointLessonTerminal.Model.ServerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model
{
    public class UserSettings
    {
        private static UserSettings Instance { get; set; }
        protected UserSettings()
        {

        }

        public static UserSettings GetInstance()
        {
            if (Instance == null)
                Instance = new UserSettings();
            return Instance;
        }


        public User CurrentUser { get; set; }
        public Role[] Roles { get; set; }
        public string JWT { get; set; }

        
    }
}
