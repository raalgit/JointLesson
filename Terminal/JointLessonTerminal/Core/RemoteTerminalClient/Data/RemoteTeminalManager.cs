using AxRDPCOMAPILib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.RemoteTerminalClient.Data
{
    internal class RemoteTeminalManager : IRemoteTerminal
    {
        private readonly AxRDPViewer _axRdpViewer;

        public RemoteTeminalManager(AxRDPViewer axRdpViewer)
        {
            if (axRdpViewer == null)
            {
                throw new ArgumentNullException(nameof(axRdpViewer));
            }

            _axRdpViewer = axRdpViewer;
        }

        public bool SmartSizing
        {
            get
            {
                return _axRdpViewer.SmartSizing;
            }

            set
            {
                _axRdpViewer.SmartSizing = value;
            }
        }

        public AxRDPViewer RdpViewer
        {
            get
            {
                return _axRdpViewer;
            }
        }

        public void Connect(string connectionString, string groupName, string passowrd)
        {
            _axRdpViewer.Connect(connectionString, groupName, passowrd);
        }

        public string StartReverseConnectListener(string connectionString, string groupName, string passowrd)
        {
            return _axRdpViewer.StartReverseConnectListener(connectionString, groupName, passowrd);
        }

        public void Disconnect()
        {
            _axRdpViewer.Disconnect();
        }
    }
}
