﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Utilities.Abstraction
{
    public interface ITestUtility1 : IUtility<int, int>
    {
        int getData();
    }
}
