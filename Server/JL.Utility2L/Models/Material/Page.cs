using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility2L.Models.Material
{
    public class Page : Block, IBlock
    {
        public int Type { get; set; }
        public int FileDataId { get; set; }
        public List<Module>? Modules { get; set; }
    }
}
