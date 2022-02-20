using JL.ApiModels.TeacherModels.Request;
using JL.ApiModels.TeacherModels.Response;
using JL.Settings;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Service.Teacher.Abstraction
{
    public interface ITeacherService : IServiceJL
    {
        Task<StartSyncLessonResponse> StartSyncLesson(StartSyncLessonRequest request, UserSettings userSettings);
        Task<CloseLessonResponse> CloseLesson(CloseLessonRequest request, UserSettings userSettings);
    }
}
