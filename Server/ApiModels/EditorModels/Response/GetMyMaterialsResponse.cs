using JL.Persist;
using jointLessonServer.ModelsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.EditorModels.Response
{
    public class GetMyMaterialsResponse : ResponseBase, IResponse
    {
        public List<Manual> Manuals { get; set; }
    }
}
