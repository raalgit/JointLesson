using JointLessonTerminal.Core;
using JointLessonTerminal.Core.HTTPRequests;
using JointLessonTerminal.Core.RemoteTerminalClient.Models;
using JointLessonTerminal.Core.RemoteTerminalServer;
using JointLessonTerminal.Core.WinApi;
using JointLessonTerminal.MVVM.Model.HttpModels.Request;
using JointLessonTerminal.MVVM.Model.HttpModels.Response;
using JointLessonTerminal.MVVM.Model.ServerModels;
using RDPCOMAPILib;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace JointLessonTerminal.MVVM.ViewModel
{
    public class RemoteTerminalViewModel : ObservableObject
    {
        #region открытые поля
        public Visibility StartTerminalVisibility { get { return startTerminalVisibility; } set { startTerminalVisibility = value; OnPropsChanged("StartTerminalVisibility"); } }
        public Visibility StopTerminalVisibility { get { return stopTerminalVisibility; } set { stopTerminalVisibility = value; OnPropsChanged("StopTerminalVisibility"); } }
        public ObservableCollection<UserRemoteAccessWithUserData> ConnectionList
        {
            get { return connectionList; }
            set { connectionList = value; OnPropsChanged("ConnectionList"); }
        }
        public UserRemoteAccessWithUserData SelectedConnection
        {
            get { return selectedConnection; }
            set
            {
                selectedConnection = value;
                ConnectToHost(selectedConnection);
                OnPropsChanged("SelectedConnection");
            }
        }
        public string ConnectionText { get; set; }
        public string ServerConnectionText
        {
            get { return _serverConnectionText; }
            set { _serverConnectionText = value; OnPropsChanged("ServerConnectionText"); }
        }
        public RdpManager RdpManagerInst { get; set; }
        public RelayCommand ServerStartCommand { get; set; }
        public RelayCommand CopyCommand { get; set; }
        public RelayCommand SingleStartCommand { get; set; }
        public RelayCommand StopCommand { get; set; }
        public RelayCommand ConnectCommand { get; set; }
        public RelayCommand DisconnectCommand { get; set; }
        public RelayCommand GetConnectionListCommand { get; set; }
        public string TeminalEventText
        {
            get { return _teminalEventText; }
            set { _teminalEventText = value; OnPropsChanged("TeminalEventText"); }
        }
        #endregion

        #region закрытые поля
        private readonly int _courseId;
        private string _serverConnectionText;
        private string _teminalEventText;
        private bool _actionChoosen = false;
        private RdpSessionServer server { get; set; }
        private Visibility startTerminalVisibility;
        private Visibility stopTerminalVisibility;
        private ObservableCollection<UserRemoteAccessWithUserData> connectionList;
        private string GroupName { get { return Environment.UserName; } }
        private string Password { get { return "Pa$$w0rrrd"; } }
        private UserRemoteAccessWithUserData selectedConnection;
        #endregion

        public RemoteTerminalViewModel(int courseId)
        {
            _courseId = courseId;
            RdpManagerInst = new RdpManager() { SmartSizing = true };
            StartTerminalVisibility = Visibility.Visible;
            StopTerminalVisibility = Visibility.Collapsed;

            RdpManagerInst.OnConnectionTerminated += (reason, info) => SessionTerminated();
            RdpManagerInst.OnGraphicsStreamPaused += (sender, args) => SessionTerminated();
            RdpManagerInst.OnAttendeeDisconnected += info => SessionTerminated();

            StopCommand = new RelayCommand(x => Stop());
            SingleStartCommand = new RelayCommand(x => SingleStart(), o => !_actionChoosen);
            ConnectCommand = new RelayCommand(x => Connect());
            DisconnectCommand = new RelayCommand(x => Disconnect());
            ServerStartCommand = new RelayCommand(x => ServerStart(), o => !_actionChoosen);
            CopyCommand = new RelayCommand(x => Copy());

            GetConnectionListCommand = new RelayCommand(async x => 
            { 
                var resp = await GetConnectionList();
                ConnectionList = resp.userRemoteAccesses;
            });
        }

        #region открытые методы

        #endregion

        #region закрытые методы
        private void Stop()
        {
            if (server != null)
            {
                server.Close();
                StartTerminalVisibility = Visibility.Visible;
                StopTerminalVisibility = Visibility.Collapsed;
            }
        }
        private void SingleStart()
        {
            if (!SupportUtils.CheckOperationSytem())
            {
                UnsupportingVersion();
                return;
            }

            server = new RdpSessionServer();
            server.Open();

            var executableName = GetApplicationName(AppDomain.CurrentDomain.FriendlyName);

            server.ApplicationFilterEnabled = true;
            foreach (RDPSRAPIApplication application in server.ApplicationList)
            {
                application.Shared = GetApplicationName(application.Name) == executableName;
            }

            ServerConnectionText = server.CreateInvitation(GroupName, Password);
            ServerStarted();
        }
        private string GetApplicationName(string fileName)
        {
            const string Executable = ".exe";

            return fileName.EndsWith(Executable)
                ? fileName.Substring(0, fileName.Length - Executable.Length)
                : fileName;
        }
        private void UnsupportingVersion()
        {
            MessageBox.Show("Support from Windows Vista only");
        }
        private void SessionTerminated()
        {
            MessageBox.Show("Session terminated");
        }
        private void Copy()
        {
            Clipboard.SetText(ServerConnectionText);
        }
        private void ServerStart()
        {
            if (!SupportUtils.CheckOperationSytem())
            {
                UnsupportingVersion();
                return;
            }

            server = new RdpSessionServer();
            server.Open();

            ServerConnectionText = server.CreateInvitation(GroupName, Password);
            ServerStarted();

            StartTerminalVisibility = Visibility.Collapsed;
            StopTerminalVisibility = Visibility.Visible;
            Task.Factory.StartNew(async x => { await sendConnectionData(ServerConnectionText); }, null);
        }
        private void ConnectToHost(UserRemoteAccessWithUserData selectedConnection)
        {
            if (selectedConnection == null) return;
            ConnectionText = selectedConnection.userRemote.connectionData;
            Connect();
        }
        private async Task<GetRemoteAccessListResponse> GetConnectionList()
        {
            var getListRequest = new RequestModel<object>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Get,
                UrlFilter = $"/{_courseId}"
            };

            var sender = new RequestSender<object, GetRemoteAccessListResponse>();
            var responsePost = await sender.SendRequest(getListRequest, "/user/remote-connection-list");
            if (responsePost.isSuccess)
            {
                // notification
            }
            else
            {
                // notification
            }

            return responsePost;
        }
        private async Task<CreateRemoteAccessResponse> sendConnectionData(string conData)
        {
            var sendDataRequest = new RequestModel<CreateRemoteAccessRequest>()
            {
                Method = Core.HTTPRequests.Enums.RequestMethod.Post,
                Body = new CreateRemoteAccessRequest()
                {
                    ConnectionData = conData,
                    CourseId = _courseId
                }
            };
            var sender = new RequestSender<CreateRemoteAccessRequest, CreateRemoteAccessResponse>();
            var responsePost = await sender.SendRequest(sendDataRequest, "/user/create-remote-access");
            if (responsePost.isSuccess)
            {
                // notification
            }
            else
            {
                // notification
            }

            return responsePost;
        }
        private void ServerStarted()
        {
            _actionChoosen = true;
        }
        private void Connect()
        {
            RdpManagerInst.Connect(ConnectionText, GroupName, Password);
        }
        private void Disconnect()
        {
            RdpManagerInst.Disconnect();
        }
        #endregion
    }
}
