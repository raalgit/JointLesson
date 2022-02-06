using JointLessonTerminal.Core.HTTPRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.HttpModels.Request
{
    [Serializable]
    public class LoginRequest : IRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
