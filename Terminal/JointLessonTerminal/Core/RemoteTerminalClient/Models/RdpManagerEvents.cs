using System;
using RDPCOMAPILib;

namespace JointLessonTerminal.Core.RemoteTerminalClient.Models
{
    public partial class RdpManager
    {
        public event _IRDPSessionEvents_OnApplicationCloseEventHandler OnApplicationClose;

        public event _IRDPSessionEvents_OnApplicationOpenEventHandler OnApplicationOpen;

        public event _IRDPSessionEvents_OnApplicationUpdateEventHandler OnApplicationUpdate;

        public event _IRDPSessionEvents_OnAttendeeConnectedEventHandler OnAttendeeConnected;

        public event _IRDPSessionEvents_OnAttendeeDisconnectedEventHandler OnAttendeeDisconnected;

        public event _IRDPSessionEvents_OnAttendeeUpdateEventHandler OnAttendeeUpdate;

        public event _IRDPSessionEvents_OnChannelDataReceivedEventHandler OnChannelDataReceived;

        public event _IRDPSessionEvents_OnChannelDataSentEventHandler OnChannelDataSent;

        public event EventHandler OnConnectionAuthenticated;

        public event EventHandler OnConnectionEstablished;

        public event EventHandler OnConnectionFailed;

        public event _IRDPSessionEvents_OnConnectionTerminatedEventHandler OnConnectionTerminated;

        public event _IRDPSessionEvents_OnControlLevelChangeRequestEventHandler OnControlLevelChangeRequest;

        public event _IRDPSessionEvents_OnErrorEventHandler OnError;

        public event _IRDPSessionEvents_OnFocusReleasedEventHandler OnFocusReleased;

        public event EventHandler OnGraphicsStreamPaused;

        public event EventHandler OnGraphicsStreamResumed;

        public event _IRDPSessionEvents_OnSharedDesktopSettingsChangedEventHandler OnSharedDesktopSettingsChanged;

        public event _IRDPSessionEvents_OnSharedRectChangedEventHandler OnSharedRectChanged;

        public event _IRDPSessionEvents_OnWindowCloseEventHandler OnWindowClose;

        public event _IRDPSessionEvents_OnWindowOpenEventHandler OnWindowOpen;

        public event _IRDPSessionEvents_OnWindowUpdateEventHandler OnWindowUpdate;

        private void Subsribe()
        {
            _manager.RdpViewer.OnApplicationClose += delegate (object application) { OnApplicationClose?.Invoke(application); };
            _manager.RdpViewer.OnApplicationOpen += delegate (object application) { OnApplicationOpen?.Invoke(application); };
            _manager.RdpViewer.OnApplicationUpdate += delegate (object application) { OnApplicationUpdate?.Invoke(application); };
            _manager.RdpViewer.OnAttendeeConnected += delegate (object attendee) { OnAttendeeConnected?.Invoke(attendee); };
            _manager.RdpViewer.OnAttendeeDisconnected += delegate (object info) { OnAttendeeDisconnected?.Invoke(info); };
            _manager.RdpViewer.OnAttendeeUpdate += delegate (object attendee) { OnAttendeeUpdate?.Invoke(attendee); };
            _manager.RdpViewer.OnChannelDataReceived += delegate (object channel, int id, string data) { OnChannelDataReceived?.Invoke(channel, id, data); };
            _manager.RdpViewer.OnChannelDataSent += delegate (object channel, int id, int sent) { OnChannelDataSent?.Invoke(channel, id, sent); };
            _manager.RdpViewer.OnConnectionAuthenticated += delegate (object sender, EventArgs args) { OnConnectionAuthenticated?.Invoke(sender, args); };
            _manager.RdpViewer.OnConnectionEstablished += delegate (object sender, EventArgs args) { OnConnectionEstablished?.Invoke(sender, args); };
            _manager.RdpViewer.OnConnectionFailed += delegate (object sender, EventArgs args) { OnConnectionFailed?.Invoke(sender, args); };
            _manager.RdpViewer.OnConnectionTerminated += delegate (int reason, int info) { OnConnectionTerminated?.Invoke(reason, info); };
            _manager.RdpViewer.OnControlLevelChangeRequest += delegate (object attendee, CTRL_LEVEL level) { OnControlLevelChangeRequest?.Invoke(attendee, level); };
            _manager.RdpViewer.OnError += delegate (object info) { OnError?.Invoke(info); };
            _manager.RdpViewer.OnFocusReleased += delegate (int direction) { OnFocusReleased?.Invoke(direction); };
            _manager.RdpViewer.OnGraphicsStreamPaused += delegate (object sender, EventArgs args) { OnGraphicsStreamPaused?.Invoke(sender, args); };
            _manager.RdpViewer.OnGraphicsStreamResumed += delegate (object sender, EventArgs args) { OnGraphicsStreamResumed?.Invoke(sender, args); };
            _manager.RdpViewer.OnSharedDesktopSettingsChanged += delegate (int width, int height, int colordepth) { OnSharedDesktopSettingsChanged?.Invoke(width, height, colordepth); };
            _manager.RdpViewer.OnSharedRectChanged += delegate (int left, int top, int right, int bottom) { OnSharedRectChanged?.Invoke(left, top, right, bottom); };
            _manager.RdpViewer.OnWindowClose += delegate (object window) { OnWindowClose?.Invoke(window); };
            _manager.RdpViewer.OnWindowOpen += delegate (object window) { OnWindowOpen?.Invoke(window); };
            _manager.RdpViewer.OnWindowUpdate += delegate (object window) { OnWindowUpdate?.Invoke(window); };
        }
    }
}
