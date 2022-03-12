using JointLessonTerminal.Core.RemoteTerminalClient.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.RemoteTerminalClient.Models
{
    public partial class RdpManager : IRemoteTerminal
    {
        private RemoteTeminalManager _manager;

        public bool SmartSizing { get; set; }

        public void Connect(string connectionString, string groupName, string passowrd)
        {
            CheckValid();
            _manager.SmartSizing = SmartSizing;
            _manager.Connect(connectionString, groupName, passowrd);
        }

        public string StartReverseConnectListener(string connectionString, string groupName, string passowrd)
        {
            CheckValid();
            _manager.SmartSizing = SmartSizing;
            return _manager.StartReverseConnectListener(connectionString, groupName, passowrd);
        }

        public void Disconnect()
        {
            CheckValid();
            _manager.Disconnect();
        }

        internal void Attach(RemoteTeminalManager manager)
        {
            Detach();
            _manager = manager;
            Subsribe();
        }

        internal void Detach()
        {
            _manager = null;
        }

        private void CheckValid()
        {
            if (_manager == null)
            {
                throw new NotSupportedException("RdpManager still not attached");
            }
        }
    }
}
