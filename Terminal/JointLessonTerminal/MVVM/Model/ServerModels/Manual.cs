using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Model.ServerModels
{
    public class Manual
    {
        public int Id { get; set; }
        public string FileId { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
    }
}
