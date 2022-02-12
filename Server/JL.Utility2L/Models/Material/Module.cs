using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility2L.Models.Material
{
    public class Module : Block, IBlock
    {
        public string Doc { get; set; }
        public int Type { get; set; }
    }
}
