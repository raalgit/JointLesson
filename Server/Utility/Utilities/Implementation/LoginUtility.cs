using JL.Persist;
using JL.Settings;
using JL.Utility.Utilities.Abstraction;
using JL.Utility.UtilityModels.Request;
using JL.Utility.UtilityModels.Response;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility.Utilities.Implementation
{
    public class LoginUtility : ILoginUtility
    {
        private readonly ApplicationSettings _appSettings;

        public LoginUtility(IOptions<ApplicationSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }


        public async Task<LoginResponse> Login(LoginRequest request, User user) 
        {
            var response = new LoginResponse();

            try
            {
                response.JWT = generateJwtToken(user);
                response.IsSuccess = true;
            }
            catch (Exception er)
            {
                response.IsSuccess = false;
            }
            
            return response;
        }


        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretWord);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
