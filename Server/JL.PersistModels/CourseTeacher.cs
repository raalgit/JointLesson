using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    public class CourseTeacher
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }
    }
}
