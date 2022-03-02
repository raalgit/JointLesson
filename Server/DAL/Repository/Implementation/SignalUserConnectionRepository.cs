using JL.DAL.Repository.Abstraction;
using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.DAL.Repository.Implementation
{
    public class SignalUserConnectionRepository : RepositoryBase<SignalUserConnection>, ISignalUserConnectionRepository
    {
        public SignalUserConnectionRepository(JLContext context) : base(context)
        {
        }
    }
}
