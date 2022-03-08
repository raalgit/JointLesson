using JL.ApiModels.UserModels.Request;
using JL.ApiModels.UserModels.Response;
using JL.Settings;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Service.User.Abstraction
{
    public interface IUserService : IServiceJL
    {
        Task<GetMyCoursesResponse> GetMyCourses(UserSettings userSettings);
        Task<AddNewFileResponse> AddNewFile(AddNewFileRequest request);
        Task<GetFileResponse> GetFile(int fileDataId);
        Task<GetCourseDataResponse> GetCourseData(int courseId, UserSettings userSettings);
        Task<GetManualFilesResponse> GetManualFiles(GetManualFilesRequest request);
        Task<RegisterSignalConnectionResponse> RegisterSignalConnection(string connectionId, UserSettings userSettings);

        Task<StartSRSLessonResponse> StartSRSLesson(StartSRSLessonRequest request, UserSettings userSettings);
        Task<ChangeSRSLessonManualPageResponse> ChangeActivePage(ChangeSRSLessonManualPageRequest request, UserSettings userSettings);
        Task<CloseSRSLessonResponse> CloseLesson(CloseSRSLessonRequest request, UserSettings userSettings);
    }
}
