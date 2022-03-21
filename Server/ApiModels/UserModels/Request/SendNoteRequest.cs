using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.UserModels.Request
{
    [Serializable]
    public class SendNoteRequest : IRequest
    {
        public byte[] File { get; set; }
        public string Page { get; set; }
    }
}
