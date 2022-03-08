using jointLessonServer.ModelsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.UserModels.Response
{
    public class ChangeSRSLessonManualPageResponse : ResponseBase, IResponse
    {
        public string NewPage { get; set; }
    }
}
