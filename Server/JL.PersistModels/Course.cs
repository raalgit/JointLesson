using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
        public int AvatarId { get; set; }
        public int ManualId { get; set; }
        public int DisciplineId { get; set; }
    }
}
