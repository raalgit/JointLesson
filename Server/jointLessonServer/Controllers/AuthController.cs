using BLL.Behavior;
using JL.Settings;
using jointLessonServer.ModelsAPI.AuthModels.Request;
using jointLessonServer.ModelsAPI.AuthModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jointLessonServer.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger, IServiceProvider provider)
        {
            _logger = logger;
            _serviceProvider = provider;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/auth/login")]
        public async Task<LoginResponse> Login([FromBody] LoginRequest request)
        {
            try
            {
                var authBehavior = new AuthBehavior(_serviceProvider);
                return await authBehavior.Login(request);
            }
            catch (Exception er)
            {
                return new LoginResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }
    }
}
