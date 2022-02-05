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
    public class LogoutUtility : ILogoutUtility
    {
        public async Task<LogoutResponse> Logout(LogoutRequest request)
        {
            var response = new LogoutResponse();


            response.IsSuccess = true;
            return response;
        }
    }
}
