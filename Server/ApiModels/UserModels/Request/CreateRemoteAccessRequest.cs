using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.UserModels.Request
{
    [Serializable]
    public class CreateRemoteAccessRequest : IRequest
    {
        public string ConnectionData { get; set; }
        public int CourseId { get; set; }
    }
}
