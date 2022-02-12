using JL.Utility2L.Models.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.EditorModels.Request
{
    [Serializable]
    public class NewMaterialRequest : IRequest
    {
        public string OriginalName { get; set; }
        
        public ManualData? ManualData { get; set; }
    }
}
