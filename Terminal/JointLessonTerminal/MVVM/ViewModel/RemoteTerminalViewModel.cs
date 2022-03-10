using JointLessonTerminal.Core;
using JointLessonTerminal.Core.RemoteTerminalClient.Models;
using JointLessonTerminal.Core.RemoteTerminalServer;
using JointLessonTerminal.Core.WinApi;
using RDPCOMAPILib;
using System;
using System.Windows;

namespace JointLessonTerminal.MVVM.ViewModel
{
    public class RemoteTerminalViewModel : ObservableObject
    {
        private string _serverConnectionText;
        private string _teminalEventText;
        private bool _actionChoosen = false;

        public RemoteTerminalViewModel()
        {
            RdpManagerInst = new RdpManager() { SmartSizing = true };

            RdpManagerInst.OnConnectionTerminated += (reason, info) => SessionTerminated();
            RdpManagerInst.OnGraphicsStreamPaused += (sender, args) => SessionTerminated();
            RdpManagerInst.OnAttendeeDisconnected += info => SessionTerminated();

            SingleStartCommand = new RelayCommand(x => SingleStart(), o => !_actionChoosen);
            ConnectCommand = new RelayCommand(x => Connect());
            ServerStartCommand = new RelayCommand(x => ServerStart(), o => !_actionChoosen);
            CopyCommand = new RelayCommand(x => Copy());
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
        public RelayCommand ConnectCommand { get; set; }

        public string TeminalEventText
        {
            get { return _teminalEventText; }
            set { _teminalEventText = value; OnPropsChanged("TeminalEventText"); }
        }

        private string GroupName
        {
            get { return Environment.UserName; }
        }

        private string Password
        {
            get { return "Pa$$w0rrrd"; }
        }

        private void SingleStart()
        {
            if (!SupportUtils.CheckOperationSytem())
            {
                UnsupportingVersion();
                return;
            }

            var server = new RdpSessionServer();
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

            var server = new RdpSessionServer();
            server.Open();

            ServerConnectionText = server.CreateInvitation(GroupName, Password);
            ServerStarted();
        }

        private void ServerStarted()
        {
            _actionChoosen = true;
        }

        private void Connect()
        {
            RdpManagerInst.GetType();
            RdpManagerInst.Connect(ConnectionText, GroupName, Password);
        }
    }
}
