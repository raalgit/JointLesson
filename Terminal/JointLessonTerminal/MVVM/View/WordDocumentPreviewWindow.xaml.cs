using JointLessonTerminal.MVVM.ViewModel;
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
    /// Логика взаимодействия для WordDocumentPreviewWindow.xaml
    /// </summary>
    public partial class WordDocumentPreviewWindow : Window
    {
        public WordDocumentPreviewWindow()
        {
            InitializeComponent();
            DataContext = new WordDocumentPreviewViewModel();
        }

        public void SetDocument(FixedDocumentSequence sequence)
        {
            (DataContext as WordDocumentPreviewViewModel).ActiveDocument = sequence;
        }
    }
}
