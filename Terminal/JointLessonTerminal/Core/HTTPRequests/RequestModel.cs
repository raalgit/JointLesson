using JointLessonTerminal.Core.HTTPRequests.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.HTTPRequests
{
    public class RequestModel<TReq>
    {
        public RequestMethod Method { get; set; }
        public TReq Object { get; set; }

        public string JWT { get; set; }
    }
}
