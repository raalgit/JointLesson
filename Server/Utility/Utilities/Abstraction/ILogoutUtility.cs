using JL.Utility.UtilityModels.Request;
using JL.Utility.UtilityModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace JL.Utility.Utilities.Abstraction
{
    public interface ILogoutUtility : IUtility<LogoutRequest, LogoutResponse>
    {
        Task<LogoutResponse> Logout(LogoutRequest request);
    }
}
