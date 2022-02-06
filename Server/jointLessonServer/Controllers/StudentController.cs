using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jointLessonServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger, IServiceProvider provider)
        {
            _logger = logger;
            _serviceProvider = provider;
        }
    }
}
