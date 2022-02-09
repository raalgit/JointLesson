using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    [Table("Group", Schema = "JL")]
    public class Group : IPersist
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
