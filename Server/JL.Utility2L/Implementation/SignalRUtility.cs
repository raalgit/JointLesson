using JL.Utility2L.Abstraction;
using JL.Utility2L.Models.SignalR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility2L.Implementation
{
    public class SignalRUtility : ISignalRUtility
    {
        private readonly IHubContext<SignalHub> _hubContext;

        public SignalRUtility(IHubContext<SignalHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendMessage(SignalType type, string message, string connectionId)
        {
            string content = null;
            switch (type)
            {
                case SignalType.POSTED:
                    content = "Posted";
                    break;
                default:
                    throw new ArgumentException(nameof(type));
            }

            try
            {
                await _hubContext.Clients.Client(connectionId).SendAsync(content, message);
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
            }
        }
    }
}
