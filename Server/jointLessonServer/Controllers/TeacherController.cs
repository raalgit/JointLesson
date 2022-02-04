using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jointLessonServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TeacherController> _logger;

        public TeacherController(ILogger<TeacherController> logger, IServiceProvider provider)
        {
            _logger = logger;
            _serviceProvider = provider;
        }
    }
}
