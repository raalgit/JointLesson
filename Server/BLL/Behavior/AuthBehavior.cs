using jointLessonServer.ModelsAPI.AuthModels.Request;
using jointLessonServer.ModelsAPI.AuthModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Behavior
{
    public class AuthBehavior
    {
        private readonly IServiceProvider _serviceProvider;

        public AuthBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public LoginResponse Login(LoginRequest request)
        {
            return new LoginResponse()
            {
                IsSuccess = false,
                JWT = null
            };
        }
    }
}
