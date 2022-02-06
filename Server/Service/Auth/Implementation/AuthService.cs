using JL.Service.Auth.Abstraction;
using JL.Utility.Utilities.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using jointLessonServer.ModelsAPI.AuthModels.Request;
using jointLessonServer.ModelsAPI.AuthModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Service.Auth.Implementation
{
    public class AuthService : IAuthService
    {
        public IServiceProvider _serviceProvider { get; }

        public AuthService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var response = new LoginResponse();

            ILoginUtility? loginUtility = null;

            // get user fron repo by login & pass
            var user = new Persist.User()
            {
                ThirdName = "edwaeaw",
                SecondName = "dwaw",
                Id = 1,
                GroupId = 2,
                FirstName = "31231",
                AvatarId = "3223"
            };

            if (user == null) throw new NullReferenceException(nameof(user));

            loginUtility = _serviceProvider.GetService<ILoginUtility>() ?? throw new NullReferenceException(nameof(loginUtility));

            var utilityResponse = await loginUtility.Login(new Utility.UtilityModels.Request.LoginRequest(), user);
            if (!utilityResponse.IsSuccess)
            {
                throw new Exception(nameof(utilityResponse));
            }

            response.JWT = utilityResponse.JWT;
            return response;
        }

        public async Task<LogoutResponse> Logout()
        {
            var response = new LogoutResponse();

            ILogoutUtility? logoutUtility = null;

            logoutUtility = _serviceProvider.GetService<ILogoutUtility>() ?? throw new NullReferenceException(nameof(logoutUtility));

            var utilityResponse = await logoutUtility.Logout(new Utility.UtilityModels.Request.LogoutRequest());
            if (!utilityResponse.IsSuccess)
            {
                throw new Exception(nameof(utilityResponse));
            }

            return new LogoutResponse();
        }

        public async Task<Persist.User> GetUserById(int id)
        {
            return new Persist.User()
            {
                AvatarId = "3123123",
                FirstName = "ivan",
                GroupId = 32,
                Id = 2,
                SecondName = "3422",
                ThirdName = "4334"
            };
        }

        public async Task<List<Persist.Role>> GetRolesByUserId(int id)
        {
            return new List<Persist.Role>(2)
            {
                new Persist.Role()
                {
                     DisplayName = "User",
                     SystemName = "User"
                },
                new Persist.Role()
                {
                     DisplayName = "Student",
                     SystemName = "Student"
                }
            };
        }
    }
}
