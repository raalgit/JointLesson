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

        public TeacherBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
