using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility2L.Models.Material
{
    public class Topic : Block, IBlock
    {
        public List<DidacticUnit>? DidacticUnits { get; set; }
    }
}
