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
        public ImageSource minimizeBtnImage { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            maximizeBtnImage = new BitmapImage(new Uri("../../Images/max-btn.png", UriKind.Relative));
            minimizeBtnImage = new BitmapImage(new Uri("../../Images/min-btn.png", UriKind.Relative));
            WinStateBtnImage.Source = minimizeBtnImage;
            mainWin.StateChanged += MainWin_StateChanged;
            mainWin.MouseDoubleClick += MainWin_MouseDoubleClick;
        }

        private void MainWin_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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

        private void MainWin_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Normal || WindowState == WindowState.Minimized)
            {
                WinStateBtnImage.Source = maximizeBtnImage;
            }
            else
            {
                WinStateBtnImage.Source = minimizeBtnImage;
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
    }
}
