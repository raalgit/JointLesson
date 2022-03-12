using JL.PersistModels;
using jointLessonServer.ModelsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.UserModels.Response
{
    public class GetRemoteAccessListResponse : ResponseBase, IResponse
    {
        public List<UserRemoteAccess> UserRemoteAccesses { get; set; }
    }
}
