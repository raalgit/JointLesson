using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.Model.ServerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.HttpModels.Response
{
    [Serializable]
    public class LoginResponse : IResponse
    {
        public User user { get; set; }
        public List<Role> roles { get; set; }
        public string jwt { get; set; }

        public bool isSuccess { get; set; }
        public string message { get; set; }
    }
}
