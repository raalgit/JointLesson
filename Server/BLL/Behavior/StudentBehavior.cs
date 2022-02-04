using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Behavior
{
    public class StudentBehavior
    {
        private readonly IServiceProvider _serviceProvider;

        public StudentBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
