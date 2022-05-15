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
using System.IO;
using Microsoft.JScript;
using System.CodeDom.Compiler;
using System.Reflection;

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

            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run("function main() { /* Точка входа в программу */ }"));
            ScriptTextBox.Document.Blocks.Add(paragraph);

            var subparagraph = new Paragraph();
            subparagraph.Inlines.Add(new Run("/* Поле для ввода кода , который можно вызвать из main */"));
            ScriptTextBox2.Document.Blocks.Add(subparagraph);

            var resparagraph = new Paragraph();
            resparagraph.Inlines.Add(new Run("Результат выполнения скрипта"));
            ScriptResponse.Document.Blocks.Add(resparagraph);
        }

        private async void ExecuteJavaScriptBtn_Click(object sender, RoutedEventArgs e)
        {
            string script = new TextRange(ScriptTextBox.Document.ContentStart, ScriptTextBox.Document.ContentEnd).Text.Replace('\r', ' ');

            if (string.IsNullOrEmpty(script)) return;

            string subscript = new TextRange(ScriptTextBox2.Document.ContentStart, ScriptTextBox2.Document.ContentEnd).Text.Replace('\r', ' ');

            StringBuilder sb = new StringBuilder();
            sb.Append("package pack").Append(" {")
            .Append("\n class program").Append(" {")
            .Append("\n")
            .Append(script)
            .Append(" }")
            .Append("\n}")
            .Append(subscript);
            script = sb.ToString();

            using (var provider = new JScriptCodeProvider())
            {
                CompilerParameters options = new CompilerParameters();
                options.GenerateExecutable = false;
                options.GenerateInMemory = true;
                options.ReferencedAssemblies.Add("system.dll");
                options.ReferencedAssemblies.Add("system.windows.forms.dll");

                var results = provider.CompileAssemblyFromSource(options, script);

                if (results.Errors.HasErrors)
                {
                    var paragraphProg = new Paragraph();
                    StringBuilder linesWithNumbers = new StringBuilder();
                    int index = 1;
                    var lines = script.Split('\n');
                    foreach (var line in lines)
                    {
                        linesWithNumbers.Append(index).Append("). ").Append(line).Append("\n");
                        index++;
                    }
                    paragraphProg.Inlines.Add(new Run(string.Format("Ошибка в программе > \n{0}", linesWithNumbers.ToString())));
                    paragraphProg.FontSize = 12;
                    paragraphProg.FontStyle = FontStyles.Italic;
                    ScriptResponse.Document.Blocks.Add(paragraphProg);
                    foreach (var error in results.Errors)
                    {
                        var paragraphError = new Paragraph();
                        paragraphError.Inlines.Add(new Run(string.Format("Ошибка > {0}", error.ToString())));
                        ScriptResponse.Document.Blocks.Add(paragraphError);
                    }
                    return;
                }
                var assembly = results.CompiledAssembly;

                try
                {
                    var myblahtype = assembly.GetType("pack.program");
                    var myblah = Activator.CreateInstance(myblahtype);
                    var result = myblahtype.InvokeMember("main", System.Reflection.BindingFlags.InvokeMethod, null, myblah, new string[0]);

                    if (result == null) return;

                    var paragraph = new Paragraph();
                    paragraph.Inlines.Add(new Run(string.Format("Результат > {0}", result.ToString())));
                    ScriptResponse.Document.Blocks.Add(paragraph);
                }
                catch (Exception er)
                {
                    var paragraph = new Paragraph();
                    paragraph.Inlines.Add(new Run(string.Format("Ошибка выполнения скрипта > {0}", er.Message)));
                    ScriptResponse.Document.Blocks.Add(paragraph);
                }
            }
        }

        private void ClearJavaScriptBtn_Click(object sender, RoutedEventArgs e)
        {
            ScriptResponse.Document.Blocks.Clear();
        }
    }
}
