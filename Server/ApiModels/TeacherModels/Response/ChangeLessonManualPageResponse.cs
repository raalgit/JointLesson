using jointLessonServer.ModelsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.ApiModels.TeacherModels.Response
{
    public class ChangeLessonManualPageResponse : ResponseBase, IResponse
    {
        public string NewPage { get; set; }
        public string IsOnline { get; set; }
    }
}
