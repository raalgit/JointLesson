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
    /// Логика взаимодействия для EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : UserControl
    {
        public EditorWindow()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            TopicSelectionPanel.Visibility = item == null ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            UnitSelectionPanel.Visibility = item == null ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ListView_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            PagesSelectionPanel.Visibility = item == null ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
