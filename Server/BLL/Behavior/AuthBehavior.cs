using JL.ApiModels.ApiModelExtensions;
using JL.Service.Auth.Abstraction;
using jointLessonServer.ModelsAPI.AuthModels.Request;
using jointLessonServer.ModelsAPI.AuthModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace BLL.Behavior
{
    public class AuthBehavior
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthService _authService;

        public AuthBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _authService = _serviceProvider.GetService<IAuthService>() ?? throw new NullReferenceException(nameof(_authService));
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            if (!request.ValidateOfNull())
            {
                throw new ArgumentNullException(nameof(request));
            }
            try
            {
                return await _authService.Login(request);
            }
            catch(Exception er)
            {
                throw er;
            }

        }

        public async Task<LogoutResponse> Logout()
        {
            return await _authService.Logout();
        }
    }
}
