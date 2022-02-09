using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    [Table("Role", Schema = "JL")]
    public class Role : IPersist
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
    }
}
