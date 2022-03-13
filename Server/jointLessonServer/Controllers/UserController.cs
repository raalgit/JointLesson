using BLL.Behavior;
using JL.ApiModels.UserModels.Request;
using JL.ApiModels.UserModels.Response;
using JL.Utility2L.Attributes;
using JL.Utility2L.Models.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace jointLessonServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<SignalHub> _hubContext;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IServiceProvider provider, IHubContext<SignalHub> hubContext)
        {
            _logger = logger;
            _serviceProvider = provider;
            _hubContext = hubContext;
        }

        [HttpGet]
        [JwtAuthentication(role: "User")]
        [Route("/user/my-courses")]
        public async Task<GetMyCoursesResponse> GetMyCourses()
        {
            try
            {
                var userBehavior = new UserBehavior(_serviceProvider);
                return await userBehavior.GetMyCourses();
            }
            catch (Exception er)
            {
                return new GetMyCoursesResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/file")]
        public async Task<AddNewFileResponse> AddFile(AddNewFileRequest request)
        {
            try
            {
                var userBehavior = new UserBehavior(_serviceProvider);
                return await userBehavior.AddNewFile(request);
            }
            catch (Exception er)
            {
                return new AddNewFileResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/get-manual-files")]
        public async Task<GetManualFilesResponse> GetManualFiles(GetManualFilesRequest request)
        {
            try
            {
                var userBehavior = new UserBehavior(_serviceProvider);
                var files = await userBehavior.GetManualFiles(request);
                return files;
            }
            catch (Exception er)
            {
                return new GetManualFilesResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpGet]
        [JwtAuthentication(role: "User")]
        [Route("/user/file/{fileId}")]
        public async Task<GetFileResponse> GetFile([FromRoute][Required] int fileId)
        {
            try
            {
                var userBehavior = new UserBehavior(_serviceProvider);
                return await userBehavior.GetFile(fileId);
            }
            catch (Exception er)
            {
                return new GetFileResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpGet]
        [JwtAuthentication(role: "User")]
        [Route("/user/register-signal-connection/{connectionId}")]
        public async Task<RegisterSignalConnectionResponse> RegisterSignalConnection([FromRoute][Required] string connectionId)
        {
            try
            {
                var userBehavior = new UserBehavior(_serviceProvider);
                return await userBehavior.RegisterSignalConnection(connectionId);
            }
            catch (Exception er)
            {
                return new RegisterSignalConnectionResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpGet]
        [JwtAuthentication(role: "User")]
        [Route("/user/course-data/{courseId}")]
        public async Task<GetCourseDataResponse> GetCourseData([FromRoute][Required] int courseId)
        {
            try
            {
                var userBehavior = new UserBehavior(_serviceProvider);
                return await userBehavior.GetCourseData(courseId);
            }
            catch (Exception er)
            {
                return new GetCourseDataResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/start-srs-lesson")]
        public async Task<StartSRSLessonResponse> StartSRSLesson([FromBody] StartSRSLessonRequest request)
        {
            try
            {
                var userBehavior = new UserBehavior(_serviceProvider);
                return await userBehavior.StartSRSLesson(request);
            }
            catch (Exception er)
            {
                return new StartSRSLessonResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/change-page-srs-lesson")]
        public async Task<ChangeSRSLessonManualPageResponse> ChangeActivePage([FromBody] ChangeSRSLessonManualPageRequest request)
        {
            try
            {
                var userBehavior = new UserBehavior(_serviceProvider);
                return await userBehavior.ChangeActivePage(request);
            }
            catch (Exception er)
            {
                return new ChangeSRSLessonManualPageResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/close-srs-lesson")]
        public async Task<CloseSRSLessonResponse> CloseLesson(CloseSRSLessonRequest request)
        {
            try
            {
                var userBehavior = new UserBehavior(_serviceProvider);
                return await userBehavior.CloseLesson(request);
            }
            catch (Exception er)
            {
                return new CloseSRSLessonResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/get-remote-access-data")]
        public async Task<GetRemoteAccessDataResponse> GetRemoteAccessData(GetRemoteAccessDataRequest request)
        {
            try
            {
                var userBehavior = new UserBehavior(_serviceProvider);
                return await userBehavior.GetRemoteAccessData(request);
            }
            catch (Exception er)
            {
                return new GetRemoteAccessDataResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/create-remote-access")]
        public async Task<CreateRemoteAccessResponse> CreateRemoteAccess(CreateRemoteAccessRequest request)
        {
            try
            {
                var userBehavior = new UserBehavior(_serviceProvider);
                return await userBehavior.CreateRemoteAccess(request);
            }
            catch (Exception er)
            {
                return new CreateRemoteAccessResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpGet]
        [JwtAuthentication(role: "User")]
        [Route("/user/remote-connection-list/{courseId}")]
        public async Task<GetRemoteAccessListResponse> GetRemoteAccessList([FromRoute][Required] int courseId)
        {
            try
            {
                var userBehavior = new UserBehavior(_serviceProvider);
                return await userBehavior.GetRemoteAccessList(courseId);
            }
            catch (Exception ex)
            {
                return new GetRemoteAccessListResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/up-hand")]
        public async Task<UpHandResponse> UpHand(UpHandRequest request)
        {
            try
            {
                var userBehavior = new UserBehavior(_serviceProvider);
                return await userBehavior.UpHand(request);
            }
            catch (Exception er)
            {
                return new UpHandResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/join-lesson")]
        public async Task<JoinLessonResponse> JoinLesson(JoinLessonRequest request)
        {
            try
            {
                var userBehavior = new UserBehavior(_serviceProvider);
                return await userBehavior.JoinLesson(request);
            }
            catch (Exception er)
            {
                return new JoinLessonResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/leave-lesson")]
        public async Task<LeaveLessonResponse> JoinLesson(LeaveLessonRequest request)
        {
            try
            {
                var userBehavior = new UserBehavior(_serviceProvider);
                return await userBehavior.LeaveLesson(request);
            }
            catch (Exception er)
            {
                return new LeaveLessonResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }
    }
}
