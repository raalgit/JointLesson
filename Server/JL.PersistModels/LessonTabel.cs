using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    [Table("LessonTabel", Schema = "JL")]
    public class LessonTabel : IPersist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LessonId { get; set; }
        public DateTime EnterDate { get; set; }
        public DateTime? LeaveDate { get; set; }
    }
}
