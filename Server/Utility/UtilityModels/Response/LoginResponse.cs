using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility.UtilityModels.Response
{
    public class LoginResponse : UtilityResponse, IUtilityResponse
    {
        public string JWT { get; set; }
    }
}
