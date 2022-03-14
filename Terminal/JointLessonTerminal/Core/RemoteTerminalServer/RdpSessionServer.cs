using RDPCOMAPILib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core.RemoteTerminalServer
{
    public class RdpSessionServer : IDisposable
    {
        private readonly RDPSession _rdpSession;

        public RdpSessionServer()
        {
            _rdpSession = new RDPSession { colordepth = 8 };
            _rdpSession.add_OnAttendeeConnected(OnAttendeeConnected);
        }

        public bool ApplicationFilterEnabled
        {
            get
            {
                return _rdpSession.ApplicationFilter.Enabled;
            }

            set
            {
                _rdpSession.ApplicationFilter.Enabled = value;
            }
        }

        public RDPSRAPIApplicationList ApplicationList
        {
            get
            {
                return _rdpSession.ApplicationFilter.Applications;
            }
        }

        public void Open()
        {
            _rdpSession.Open();
        }

        public void Close()
        {
            _rdpSession.Close();
        }

        public void Pause()
        {
            _rdpSession.Pause();
        }

        public void Resume()
        {
            _rdpSession.Resume();
        }

        public void ConnectToClient(string connectionString)
        {
            _rdpSession.ConnectToClient(connectionString);
        }

        public string CreateInvitation(string groupName, string passowrd)
        {
            var invitation = _rdpSession.Invitations.CreateInvitation(null, groupName, passowrd, 2);
            return invitation.ConnectionString;
        }

        public void Dispose()
        {
            try
            {
                if (_rdpSession != null)
                {
                    foreach (IRDPSRAPIAttendee attendees in _rdpSession.Attendees)
                    {
                        attendees.TerminateConnection();
                    }

                    _rdpSession.Close();
                }
            }
            catch
            {
            }
        }

        private void OnAttendeeConnected(object pAttendee)
        {
            var attendee = (IRDPSRAPIAttendee)pAttendee;
            attendee.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_VIEW;
        }
    }
}
