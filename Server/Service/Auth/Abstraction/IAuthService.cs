using jointLessonServer.ModelsAPI.AuthModels.Request;
using jointLessonServer.ModelsAPI.AuthModels.Response;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Service.Auth.Abstraction
{
    public interface IAuthService : IServiceJL
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<LogoutResponse> Logout();



        Task<Persist.User> GetUserById(int id);
        Task<List<Persist.Role>> GetRolesByUserId(int id);
    }
}
