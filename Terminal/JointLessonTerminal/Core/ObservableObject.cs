using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonTerminal.Core
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<WindowEvent> WindowStateChanged;

        protected void OnPropsChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected void SendEventSignal(WindowEvent winEvent)
        {
            WindowStateChanged?.Invoke(this, winEvent);
        }
    }
}