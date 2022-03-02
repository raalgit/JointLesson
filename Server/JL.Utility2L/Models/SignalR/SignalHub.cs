using JL.DAL.Repository.Abstraction;
using JL.Utility2L.Attributes;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility2L.Models.SignalR
{
    public class SignalHub : Hub
    {
        private readonly ISignalUserConnectionRepository _signalUserConnectionRepository;
        public SignalHub(ISignalUserConnectionRepository signalUserConnectionRepository)
        {
            _signalUserConnectionRepository = signalUserConnectionRepository;
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            deleteConnection(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        private void deleteConnection(string connectionId)
        {
            var domain = _signalUserConnectionRepository.Get().FirstOrDefault(x => x.ConnectionId == connectionId);
            if (domain != null)
            {
                _signalUserConnectionRepository.Delete(domain);
                _signalUserConnectionRepository.SaveChanges();
            }
        }
    }
}
