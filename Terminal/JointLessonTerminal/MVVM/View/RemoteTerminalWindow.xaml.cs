using JointLessonTerminal.Core;
using JointLessonTerminal.Core.RemoteTerminalClient.Data;
using JointLessonTerminal.Core.RemoteTerminalClient.Models;
using JointLessonTerminal.Core.RemoteTerminalServer;
using JointLessonTerminal.Core.WinApi;
using JointLessonTerminal.MVVM.ViewModel;
using RDPCOMAPILib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JointLessonTerminal.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для RemoteTerminalWindow.xaml
    /// </summary>
    public partial class RemoteTerminalWindow : Window
    {
        public static DependencyProperty RdpManagerProperty;
        internal readonly RemoteTeminalManager Manager;

        static RemoteTerminalWindow()
        {
            RemoteTeminalBehavior.InitializeDependencyProperties();
        }
        public RemoteTerminalWindow()
        {
            InitializeComponent();
            DataContext = new RemoteTerminalViewModel();
            Manager = new RemoteTeminalManager(RdpViewer);
            var res = (DataContext as RemoteTerminalViewModel).RdpManagerInst;
            res.Attach(Manager);
            RemoteTeminalBehavior.RdpManagerChangedCallback(this, new DependencyPropertyChangedEventArgs(RdpManagerProperty, res, res));
        }

        public RdpManager RdpManager
        {
            get
            {
                return (RdpManager)GetValue(RdpManagerProperty);
            }

            set
            {
                SetValue(RdpManagerProperty, value);
            }
        }
    }
}
