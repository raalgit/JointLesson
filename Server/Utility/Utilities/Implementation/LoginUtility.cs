using JL.Utility.Utilities.Abstraction;
using JL.Utility.UtilityModels.Request;
using JL.Utility.UtilityModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility.Utilities.Implementation
{
    public class LoginUtility : ILoginUtility
    {
        public async Task<LoginResponse> Login(LoginRequest request) 
        {
            var response = new LoginResponse();

            response.IsSuccess = true;
            return response;
        }
    }
}
