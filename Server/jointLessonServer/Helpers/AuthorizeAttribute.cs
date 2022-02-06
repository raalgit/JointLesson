using JL.Persist;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace jointLessonServer.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _role;
        public AuthorizeAttribute(string role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"] as User;
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            var userRoles = context.HttpContext.Items["Roles"] as Role[];
            if (userRoles == null || !userRoles.Select(x => x.SystemName).Contains(_role))
            {
                context.Result = new JsonResult(new { message = "Not acceptable" }) { StatusCode = StatusCodes.Status406NotAcceptable };
            }
        }
    }
}
