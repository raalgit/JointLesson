using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Utilities.Abstraction;

namespace Utility.Utilities.Implementation
{
    public class TestUtility2 : ITestUtility2
    {
        public int getData()
        {
            Random random = new Random();
            return random.Next(20000);
        }
    }
}
