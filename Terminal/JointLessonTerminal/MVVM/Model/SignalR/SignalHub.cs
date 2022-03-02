using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.MVVM.Model.HttpModels.Request;
using JointLessonTerminal.MVVM.Model.HttpModels.Response;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.MVVM.Model.SignalR
{
    public class SignalHub
    {
        private HubConnection _hubConnection;
        private string connectiodId;

        public SignalHub()
        {
            string serverUrl = ConfigurationManager.AppSettings["ServerUrl"].ToString();
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(serverUrl + "/signalHub")
                .Build();
            _hubConnection.Closed += _hubConnection_Closed;
            connect();
        }

        private async Task _hubConnection_Closed(Exception arg)
        {
            Console.WriteLine(arg.Message);
            await _hubConnection.StartAsync();
        }

        private async void connect()
        {
            _hubConnection.On<string>("Connected", async (connection) =>
            {
                connectiodId = connection;
                var user = UserSettings.GetInstance();
                user.SignalrConnectionId = connectiodId;
                var response = await registerConnection(user.SignalrConnectionId);
            });

            _hubConnection.On<string>("Posted", (val) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    Console.WriteLine(val);
                });
            });

            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
            }
        } 

        private async Task<RegisterSignalConnectionResponse> registerConnection(string connectionId)
        {
            var registerRequest = new RequestModel<object>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Get,
                UrlFilter = $"/{connectionId}"
            };
            var sender = new RequestSender<object, RegisterSignalConnectionResponse>();
            return await sender.SendRequest(registerRequest, "/user/register-signal-connection");
        }
    }
}
