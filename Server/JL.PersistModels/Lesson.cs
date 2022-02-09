using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    [Table("Lesson", Schema = "JL")]
    public class Lesson : IPersist
    {
        public int Id { get; set; }
        public int GroupAtCourseId { get; set; }
        public int LastMaterialPage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TeacherId { get; set; }
        public InnerModels.LessonType Type { get; set; }
    }
}
