using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility2L.Models.Material
{
    [Serializable]
    public class Block : IBlock
    {
        public string Id { get; set; }
        public int Access { get; set; }
        public int Parts { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
    }
}
