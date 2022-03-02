using JL.Utility2L.Abstraction;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility2L.Implementation
{
    public class SignalRUserProvider : IUserIdProvider, ISignalRUserProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity.Name;
        }
    }
}
