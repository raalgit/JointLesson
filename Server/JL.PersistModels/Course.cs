using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    [Table("Course", Schema = "JL")]
    public class Course : IPersist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
        public int? AvatarId { get; set; }
        public int ManualId { get; set; }
        public int DisciplineId { get; set; }
    }
}
