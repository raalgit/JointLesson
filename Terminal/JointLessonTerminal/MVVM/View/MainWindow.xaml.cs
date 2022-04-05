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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JointLessonTerminal.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ImageSource maximizeBtnImage { get; set; }
        public ImageSource normalizeBtnImage { get; set; }
        int lastClickTimeStamp;

        public MainWindow()
        {
            InitializeComponent();
            lastClickTimeStamp = 1000;
            maximizeBtnImage = new BitmapImage(new Uri("../../Images/max-btn.png", UriKind.Relative));
            normalizeBtnImage = new BitmapImage(new Uri("../../Images/norm-btn.png", UriKind.Relative));
            WinStateBtnImage.Source = normalizeBtnImage;
            mainWin.StateChanged += MainWin_StateChanged;
            topMenu.MouseLeftButtonUp += TopMenu_MouseLeftButtonUp;
        }

        private void TopMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Timestamp - lastClickTimeStamp < 300)
            {
                if (WindowState == WindowState.Normal || WindowState == WindowState.Minimized)
                {
                    WindowState = WindowState.Maximized;
                }
                else
                {
                    WindowState = WindowState.Normal;
                }
            }
            lastClickTimeStamp = e.Timestamp;
        }

        private void MainWin_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Normal || WindowState == WindowState.Minimized)
            {
                WinStateBtnImage.Source = maximizeBtnImage;
            }
            else
            {
                WinStateBtnImage.Source = normalizeBtnImage;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void maximize_btn_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal || WindowState == WindowState.Minimized) {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void exit_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void mini_btn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
