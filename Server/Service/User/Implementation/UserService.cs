using JL.Service.User.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Service.User.Implementation
{
    public class UserService : IUserService
    {
        public IServiceProvider _serviceProvider { get; }

        public UserService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        
    }
}
