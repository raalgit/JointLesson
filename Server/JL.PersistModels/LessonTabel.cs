using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    public class LessonTabel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LessonId { get; set; }
        public DateTime EnterDate { get; set; }
        public DateTime LeaveDate { get; set; }
    }
}
