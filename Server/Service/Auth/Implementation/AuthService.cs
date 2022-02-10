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
using JL.DAL.Repository.Abstraction;

namespace JL.Service.Auth.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthDataRepository _authDataRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository ;


        public AuthService(
            IServiceProvider serviceProvider,
            IAuthDataRepository authDataRepository,
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository)
        {
            _serviceProvider = serviceProvider;
            _authDataRepository = authDataRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }


        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var response = new LoginResponse();

            ILoginUtility? loginUtility = null;

            // Получение хэша пароля
            var passwordHash = request.Password;

            // Поиск авторизационных данных по логину и паролю
            var authData = _authDataRepository.Get().FirstOrDefault(x => x.Login == request.Login && x.PasswordHash == passwordHash) 
                ?? throw new Exception("Логин и/или пароль не найден(ы)");

            // Поиск данных пользователя
            var user = _userRepository.Get().FirstOrDefault(x => x.Id == authData.UserId) 
                ?? throw new Exception("Данные пользователя не найдены");

            var roles = await GetRolesByUserId(user.Id);
            
            loginUtility = _serviceProvider.GetService<ILoginUtility>() ?? throw new NullReferenceException(nameof(loginUtility));

            var utilityResponse = await loginUtility.Login(new Utility.UtilityModels.Request.LoginRequest(), user);
            if (!utilityResponse.IsSuccess)
            {
                throw new Exception(nameof(utilityResponse));
            }

            response.JWT = utilityResponse.JWT;
            response.Roles = roles;
            response.User = user;

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
            return _userRepository.Get().FirstOrDefault(x => x.Id == id) ?? throw new Exception($"Пользователь с номером {id} не найден");
        }

        public async Task<List<Persist.Role>> GetRolesByUserId(int id)
        {
            return (from user in _userRepository.Get()
                    join user_role in _userRoleRepository.Get() on user.Id equals user_role.UserId
                    join role in _roleRepository.Get() on user_role.RoleId equals role.Id
                    where user.Id == id
                    select role).ToList();
        }
    }
}
