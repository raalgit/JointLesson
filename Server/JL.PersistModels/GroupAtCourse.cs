using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    [Table("GroupAtCourse", Schema = "JL")]
    public class GroupAtCourse : IPersist
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int GroupId { get; set; }
        public string LastMaterialPage { get; set; }
        public bool IsActive { get; set; }
    }
}
