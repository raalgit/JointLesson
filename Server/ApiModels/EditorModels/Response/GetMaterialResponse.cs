using JL.Utility2L.Models.Material;
using jointLessonServer.ModelsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.EditorModels.Response
{
    public class GetMaterialResponse : ResponseBase, IResponse
    {
        public string OriginalName { get; set; }
        public ManualData ManualData { get; set; }
    }
}
