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
using AvalonDock.Layout.Serialization;
using AvalonDock.Layout;
using AvalonDock;
using System.Diagnostics;
using CefSharp;
using System.IO;

namespace JointLessonTerminal.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для CurrentCourseWindow.xaml
    /// </summary>
    public partial class LessonWindow : UserControl
    {
        public LessonWindow()
        {
            InitializeComponent();
            Browser.Loaded += Browser_Loaded;
        }

        private async void ExecuteJavaScriptBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Browser.CanExecuteJavascriptInMainFrame && !string.IsNullOrWhiteSpace(ScriptTextBox.Text))
            {
                JavascriptResponse response = await Browser.EvaluateScriptAsync(ScriptTextBox.Text);

                if (!response.Success) MessageBox.Show(response.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                if (response.Result != null)
                {
                    var paragraph = new Paragraph();
                    paragraph.Inlines.Add(new Run(string.Format("JavaScript Result {0}", response.Result.ToString())));
                    ScriptResponse.Document.Blocks.Add(paragraph);
                }
            }
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("index.html"))
            {
                var content = File.ReadAllText("index.html");
                Browser.LoadHtml(content);
            }
        }

        private void ClearJavaScriptBtn_Click(object sender, RoutedEventArgs e)
        {
            ScriptResponse.Document.Blocks.Clear();
        }

        private void BackJavaScriptBtn_Click(object sender, RoutedEventArgs e)
        {
            Browser.Back();
        }
    }
}
