using BLL.Behavior;
using JL.ApiModels.TeacherModels.Request;
using JL.ApiModels.TeacherModels.Response;
using JL.Utility2L.Attributes;
using JL.Utility2L.Models.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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

        [HttpPost]
        [JwtAuthentication(role: "Teacher")]
        [Route("/teacher/start-sync-lesson")]
        public async Task<StartSyncLessonResponse> StartSyncLesson([FromBody]StartSyncLessonRequest request)
        {
            try
            {
                var teacherBehavior = new TeacherBehavior(_serviceProvider);
                return await teacherBehavior.StartSyncLesson(request);
            }
            catch (Exception er)
            {
                return new StartSyncLessonResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "Teacher")]
        [Route("/teacher/close-sync-lesson")]
        public async Task<CloseLessonResponse> CloseLesson([FromBody] CloseLessonRequest request)
        {
            try
            {
                var teacherBehavior = new TeacherBehavior(_serviceProvider);
                return await teacherBehavior.CloseLesson(request);
            }
            catch (Exception er)
            {
                return new CloseLessonResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "Teacher")]
        [Route("/teacher/change-page")]
        public async Task<ChangeLessonManualPageResponse> ChangeActivePage([FromBody] ChangeLessonManualPageRequest request)
        {
            try
            {
                var teacherBehavior = new TeacherBehavior(_serviceProvider);
                return await teacherBehavior.ChangeActivePage(request);
            }
            catch (Exception er)
            {
                return new ChangeLessonManualPageResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }
    }
}
