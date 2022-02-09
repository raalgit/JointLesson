using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    [Table("Manual", Schema = "JL")]
    public class Manual : IPersist
    {
        public int Id { get; set; }
        public string FileId { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
    }
}
