using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.RemoteTerminalClient.Data
{
    public interface IRemoteTerminal
    {
        bool SmartSizing { get; set; }
        void Connect(string connectionString, string groupName, string passowrd);
        string StartReverseConnectListener(string connectionString, string groupName, string passowrd);
        void Disconnect();
    }
}
