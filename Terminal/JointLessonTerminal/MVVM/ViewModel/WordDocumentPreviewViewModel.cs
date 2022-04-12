using JointLessonTerminal.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace JointLessonTerminal.MVVM.ViewModel
{
    public class WordDocumentPreviewViewModel : ObservableObject
    {
        public FixedDocumentSequence ActiveDocument { get { return activeDocument; } set { activeDocument = value; OnPropsChanged("ActiveDocument"); } }
        private FixedDocumentSequence activeDocument;

    }
}
