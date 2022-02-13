using System;
using System.Collections.Generic;
using System.Globalization;
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

        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            var fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            double val;
            e.Handled = !double.TryParse(fullText,
                                         NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                                         CultureInfo.InvariantCulture,
                                         out val);
        }
    }
}
