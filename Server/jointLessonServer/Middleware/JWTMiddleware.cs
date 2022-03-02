using JL.Service.Auth.Abstraction;
using JL.Service.User.Abstraction;
using JL.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace jointLessonServer.Middleware
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApplicationSettings _appSettings;

        public JWTMiddleware(RequestDelegate next, IOptions<ApplicationSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IAuthService authService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var user = attachUserToContext(context, authService, token);
                var roles = attachRolesToContext(context, authService, user);
            }
            await _next(context);
        }

        private JL.Persist.User attachUserToContext(HttpContext context, IAuthService authService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.SecretWord);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                var userData = authService.GetUserById(userId).Result;

                context.Items["User"] = userData;
                return userData;
            }
            catch
            {
                throw new Exception(nameof(token));
            }
        }

        private JL.Persist.Role[] attachRolesToContext(HttpContext context, IAuthService authService, JL.Persist.User user)
        {
            var rolesData = authService.GetRolesByUserId(user.Id).Result.ToArray();
            context.Items["Roles"] = rolesData;
            return rolesData;
        }
    }
}
