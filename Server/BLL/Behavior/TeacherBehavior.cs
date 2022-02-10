using JL.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Behavior
{
    public class TeacherBehavior
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserSettings _userSettings;

        public TeacherBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>() ??
                throw new NullReferenceException(nameof(_httpContextAccessor));
            _userSettings = new UserSettings((JL.Persist.User)(_httpContextAccessor.HttpContext.Items["User"] ??
                throw new NullReferenceException("Данные пользователя не найдены")));
        }
    }
}
