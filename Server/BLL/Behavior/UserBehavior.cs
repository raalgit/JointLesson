using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Behavior
{
    public class UserBehavior
    {
        private readonly IServiceProvider _serviceProvider;

        public UserBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
