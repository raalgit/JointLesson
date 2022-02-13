using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility2L.Models.Material
{
    public interface IBlock
    {
        string Id { get; set; }
        int Access { get; set; }
        int Parts { get; set; }
        int Number { get; set; }
        string Name { get; set; }
    }
}
