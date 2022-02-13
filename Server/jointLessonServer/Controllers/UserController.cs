using BLL.Behavior;
using JL.ApiModels.UserModels.Request;
using JL.ApiModels.UserModels.Response;
using jointLessonServer.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace jointLessonServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IServiceProvider provider)
        {
            _logger = logger;
            _serviceProvider = provider;
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
    }
}
