using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    [Table("Discipline", Schema = "JL")]
    public class Discipline : IPersist
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
