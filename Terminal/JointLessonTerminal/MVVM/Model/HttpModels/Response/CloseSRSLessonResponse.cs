using JointLessonTerminal.Core.HTTPRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.HttpModels.Response
{
    [Serializable]
    public class CloseSRSLessonResponse : IResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
    }
}
