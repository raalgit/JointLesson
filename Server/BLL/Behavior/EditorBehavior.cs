using JL.ApiModels.EditorModels.Request;
using JL.ApiModels.EditorModels.Response;
using JL.Service.Editor.Abstraction;
using JL.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.BLL.Behavior
{
    public class EditorBehavior
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserSettings _userSettings;
        private readonly IEditorService _editorService;

        public EditorBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>() ??
                throw new NullReferenceException(nameof(_httpContextAccessor));
            _userSettings = new UserSettings((JL.Persist.User)(_httpContextAccessor.HttpContext.Items["User"] ??
                throw new NullReferenceException("Данные пользователя не найдены")));
            _editorService = serviceProvider.GetService<IEditorService>() ??
                throw new NullReferenceException(nameof(_editorService));
        }

        public async Task<NewMaterialResponse> NewMaterial(NewMaterialRequest request)
        {
            try
            {
                return await _editorService.NewMaterial(request, _userSettings);
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        public async Task<UpdateMaterialResponse> UpdateMaterial(UpdateMaterialRequest request)
        {
            try
            {
                return await _editorService.UpdateMaterial(request, _userSettings);
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        public async Task<GetMaterialResponse> GetMaterialData(int fileId)
        {
            try
            {
                return await _editorService.GetMaterialData(fileId, _userSettings);
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        public async Task<GetCourseManualResponse> GetMaterialById(int courseId) 
        {
            try
            {
                return await _editorService.GetMaterialById(courseId);
            }
            catch(Exception er)
            {
                throw er;
            }
        }

        public async Task<GetMyMaterialsResponse> GetMyMaterials()
        {
            try
            {
                return await _editorService.GetMyMaterials(_userSettings);   
            }
            catch (Exception er)
            {
                throw er;
            }
        }
    }
}
