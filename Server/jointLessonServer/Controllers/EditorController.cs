using JL.ApiModels.EditorModels.Request;
using JL.ApiModels.EditorModels.Response;
using JL.BLL.Behavior;
using JL.Utility2L.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace jointLessonServer.Controllers
{
    [ApiController]
    public class EditorController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AuthController> _logger;

        public EditorController(ILogger<AuthController> logger, IServiceProvider provider)
        {
            _logger = logger;
            _serviceProvider = provider;
        }

        [HttpPost]
        [JwtAuthentication(role: "Editor")]
        [Route("/editor/material")]
        public async Task<NewMaterialResponse> NewMaterial([FromBody] NewMaterialRequest request)
        {
            try
            {
                var editorBehavior = new EditorBehavior(_serviceProvider);
                return await editorBehavior.NewMaterial(request);
            }
            catch (Exception er)
            {
                return new NewMaterialResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPut]
        [JwtAuthentication(role: "Editor")]
        [Route("/editor/material")]
        public async Task<UpdateMaterialResponse> UpdateMaterial([FromBody] UpdateMaterialRequest request)
        {
            try
            {
                var editorBehavior = new EditorBehavior(_serviceProvider);
                return await editorBehavior.UpdateMaterial(request);
            }
            catch (Exception er)
            {
                return new UpdateMaterialResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpGet]
        [JwtAuthentication(role: "User")]
        [Route("/editor/material/{fileId}")]
        public async Task<GetMaterialResponse> GetMaterial([FromRoute][Required] int fileId)
        {
            try
            {
                var editorBehavior = new EditorBehavior(_serviceProvider);
                return await editorBehavior.GetMaterialData(fileId);
            }
            catch (Exception er)
            {
                return new GetMaterialResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpGet]
        [JwtAuthentication(role: "User")]
        [Route("/editor/course-material/{id}")]
        public async Task<GetCourseManualResponse> GetCourseMaterial([FromRoute][Required] int id)
        {
            try
            {
                var editorBehavior = new EditorBehavior(_serviceProvider);
                return await editorBehavior.GetMaterialById(id);
            }
            catch (Exception er)
            {
                return new GetCourseManualResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }


        [HttpGet]
        [JwtAuthentication(role: "Editor")]
        [Route("/editor/my-materials")]
        public async Task<GetMyMaterialsResponse> MyMaterials()
        {
            try
            {
                var editorBehavior = new EditorBehavior(_serviceProvider);
                return await editorBehavior.GetMyMaterials();
            }
            catch (Exception er)
            {
                return new GetMyMaterialsResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }

        }
    }
}