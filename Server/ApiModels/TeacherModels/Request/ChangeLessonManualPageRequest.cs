using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.TeacherModels.Request
{
    [Serializable]
    public class ChangeLessonManualPageRequest : IRequest
    {
        public int CourseId { get; set; }
        public string NextPage { get; set; }
    }
}
