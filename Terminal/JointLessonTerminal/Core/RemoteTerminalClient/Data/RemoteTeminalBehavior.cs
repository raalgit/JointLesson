using JointLessonTerminal.Core.RemoteTerminalClient.Models;
using JointLessonTerminal.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JointLessonTerminal.Core.RemoteTerminalClient.Data
{
    internal static class RemoteTeminalBehavior
    {
        public static void InitializeDependencyProperties()
        {
            RemoteTerminalWindow.RdpManagerProperty = DependencyProperty.Register(
                nameof(RemoteTerminalWindow.RdpManager),
                typeof(RdpManager),
                typeof(RemoteTerminalWindow),
                new PropertyMetadata(default(RdpManager), RdpManagerChangedCallback));
        }

        public static void RdpManagerChangedCallback(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs args)
        {
            var terminal = (RemoteTerminalWindow)dependencyObject;
            var oldManager = (RdpManager)args.OldValue;
            var newManager = (RdpManager)args.NewValue;

            oldManager?.Detach();
            newManager?.Attach(terminal.Manager);
        }
    }
}
