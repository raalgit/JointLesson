using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility2L.Models.Material
{
    [Serializable]
    public class Chapter : Block, IBlock
    {
        public List<Topic>? Topics { get; set; }
    }
}
