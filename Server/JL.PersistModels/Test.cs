using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    [Table("Test", Schema = "JL")]
    public class Test : IPersist
    {
        public int Id { get; set; }
        public int BlockId { get; set; }
        public int UserId { get; set; }
        public int LessonId { get; set; }
        public DateTime SendDate { get; set; }
        public string ResultId { get; set; }
    }
}
