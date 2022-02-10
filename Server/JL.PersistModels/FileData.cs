using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.PersistModels
{
    [Table("FileData", Schema = "JL")]
    public class FileData : IPersist
    {
        public int Id { get; set; }
        public string MongoName { get; set; }
        public string OriginalName { get; set; }
        public string MongoId { get; set; }
    }
}
