using JointLessonTerminal.Core.HTTPRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.HttpModels.Request
{
    [Serializable]
    public class RegisterSignalConnectionRequest : IRequest
    {
        public string ConnectionId { get; set; }
    }
}
