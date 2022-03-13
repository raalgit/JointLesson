using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace JointLessonTerminal.MVVM.Model.UIBehaviors
{
    public class RichTextSelectionBehavior : Behavior<RichTextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += RichTextBoxSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= RichTextBoxSelectionChanged;
        }

        void RichTextBoxSelectionChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            SelectedText = AssociatedObject.Selection;
        }

        public TextSelection SelectedText
        {
            get { return (TextSelection)GetValue(SelectedTextProperty); }
            set { SetValue(SelectedTextProperty, value); }
        }

        public static readonly DependencyProperty SelectedTextProperty =
            DependencyProperty.Register(
                "SelectedText",
                typeof(TextSelection),
                typeof(RichTextSelectionBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTextChanged));

        private static void OnSelectedTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as RichTextSelectionBehavior;
            if (behavior == null || behavior.SelectedText == null)
                return;
            behavior.AssociatedObject.Selection.Text = behavior.SelectedText.Text;
        }
    }
}
