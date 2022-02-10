using BLL.Behavior;
using JL.ApiModels.UserModels.Response;
using jointLessonServer.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
