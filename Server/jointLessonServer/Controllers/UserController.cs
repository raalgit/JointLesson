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
    }
}
