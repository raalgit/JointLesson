using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    [Table("WorkBook", Schema = "JL")]
    public class WorkBook : IPersist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FileDataId { get; set; }
        public string Page { get; set; }
    }
}
