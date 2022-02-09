using JL.DAL.Repository.Abstraction;
using JL.Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.DAL.Repository.Implementation
{
    public class AuthDataRepository : RepositoryBase<AuthData>, IAuthDataRepository
    {
        public AuthDataRepository(JLContext context) : base(context)
        {
        }
    }
}
