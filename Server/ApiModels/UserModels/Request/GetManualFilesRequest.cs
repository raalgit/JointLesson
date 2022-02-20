using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.UserModels.Request
{
    public class GetManualFilesRequest : IRequest
    {
        public List<int> FileDataIds { get; set; }
    }
}
