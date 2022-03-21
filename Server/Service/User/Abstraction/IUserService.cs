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
        Task<LoadNoteResponse> LoadNoteAsync(LoadNoteRequest request, UserSettings userSettings);
        Task<SendNoteResponse> SendNoteAsync(SendNoteRequest request, UserSettings userSettings);
        Task<JoinLessonResponse> JoinLessonAsync(JoinLessonRequest request, UserSettings userSettings);
        Task<LeaveLessonResponse> LeaveLessonAsync(LeaveLessonRequest request, UserSettings userSettings);
        Task<UpHandResponse> UpHandAsync(UpHandRequest request, UserSettings userSettings);
        Task<GetRemoteAccessListResponse> GetRemoteAccessListAsync(int courseId);
        Task<CreateRemoteAccessResponse> CreateRemoteAccessAsync(CreateRemoteAccessRequest request, UserSettings userSettings);
        Task<GetRemoteAccessDataResponse> GetRemoteAccessDataAsync(GetRemoteAccessDataRequest request);
        Task<GetMyCoursesResponse> GetMyCoursesAsync(UserSettings userSettings);
        Task<AddNewFileResponse> AddNewFileAsync(AddNewFileRequest request);
        Task<GetFileResponse> GetFileAsync(int fileDataId);
        Task<GetCourseDataResponse> GetCourseDataAsync(int courseId, UserSettings userSettings);
        Task<GetManualFilesResponse> GetManualFilesAsync(GetManualFilesRequest request);
        Task<RegisterSignalConnectionResponse> RegisterSignalConnectionAsync(string connectionId, UserSettings userSettings);
        Task<StartSRSLessonResponse> StartSRSLessonAsync(StartSRSLessonRequest request, UserSettings userSettings);
        Task<ChangeSRSLessonManualPageResponse> ChangeActivePageAsync(ChangeSRSLessonManualPageRequest request, UserSettings userSettings);
        Task<CloseSRSLessonResponse> CloseLessonAsync(CloseSRSLessonRequest request, UserSettings userSettings);
    }
}
