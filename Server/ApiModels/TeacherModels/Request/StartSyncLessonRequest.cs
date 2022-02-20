using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.TeacherModels.Request
{
    [Serializable]
    public class StartSyncLessonRequest : IRequest
    {
        public string StartPage { get; set; }
        public int CourseId { get; set; }
    }
}
