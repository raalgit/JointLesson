using JointLessonTerminal.Core.HTTPRequests.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.HTTPRequests
{
    public class RequestModel<TReq>
    {
        public RequestMethod Method { get; set; }
        public TReq Body { get; set; }
        public string UrlFilter { get; set; }
        public bool UseCurrentToken { get; set; } = true;
        public bool UploadFile { get; set; } = false;
        public MultipartFormDataContent MultipartFormData { get; set; }
    }
}
