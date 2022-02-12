using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility2L.Models.Material
{
    public class DidacticUnit : Block, IBlock
    {
        public List<Page>? Pages { get; set; }
    }
}
