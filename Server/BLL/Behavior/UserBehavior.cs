using JL.ApiModels.UserModels.Response;
using Microsoft.Extensions.DependencyInjection;
using JL.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JL.Service.User.Abstraction;
using JL.ApiModels.UserModels.Request;

namespace BLL.Behavior
{
    public class UserBehavior
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserSettings _userSettings;
        private readonly IUserService _userService;

        public UserBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>() ?? 
                throw new NullReferenceException(nameof(_httpContextAccessor));
            _userSettings = new UserSettings((JL.Persist.User)(_httpContextAccessor.HttpContext.Items["User"] ?? 
                throw new NullReferenceException("Данные пользователя не найдены")));
            _userService = serviceProvider.GetService<IUserService>() ?? 
                throw new NullReferenceException(nameof(_userService));
        }

        public async Task<GetMyCoursesResponse> GetMyCourses()
        {
            try
            {
                return await _userService.GetMyCourses(_userSettings);
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        public async Task<GetFileResponse> GetFile(int fileDataId)
        {
            try
            {
                return await _userService.GetFile(fileDataId);
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        public async Task<AddNewFileResponse> AddNewFile(AddNewFileRequest request)
        {
            try
            {
                return await _userService.AddNewFile(request);
            }
            catch (Exception er)
            {
                throw er;
            }
        }
    }
}
