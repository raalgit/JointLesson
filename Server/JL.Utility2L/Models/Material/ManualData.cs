using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility2L.Models.Material
{
    [Serializable]
    public class ManualData : Block, IBlock
    {
        public List<Author> Authors { get; set; }
        public MaterialDate? MaterialDate { get; set; }
        public string Discipline { get; set; }

        public List<Chapter> Chapters { get; set; }
    }
}
