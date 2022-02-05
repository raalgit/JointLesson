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

        private readonly IAuthService authService;

        public AuthBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            authService = _serviceProvider.GetService<IAuthService>() ?? throw new NullReferenceException(nameof(authService));
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            if (!request.ValidateOfNull())
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await authService.Login(request);
        }

        public async Task<LogoutResponse> Logout()
        {
            return await authService.Logout();
        }
    }
}
