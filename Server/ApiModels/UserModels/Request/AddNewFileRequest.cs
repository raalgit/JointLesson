using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.UserModels.Request
{
    public class AddNewFileRequest : IRequest
    {
        public byte[] File { get; set; }
        public string Name { get; set; }
    }
}
