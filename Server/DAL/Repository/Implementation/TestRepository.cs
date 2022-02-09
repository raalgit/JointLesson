using JL.DAL.Repository.Abstraction;
using JL.Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.DAL.Repository.Implementation
{
    public class TestRepository : RepositoryBase<Test>, ITestRepository
    {
        public TestRepository(JLContext context) : base(context)
        {
        }
    }
}
