using jointLessonServer.ModelsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.UserModels.Response
{
    public class GetRemoteAccessDataResponse : ResponseBase, IResponse
    {
        public string ConnectionData { get; set; }
    }
}
