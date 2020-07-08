using System.Windows;
using System.Windows.Controls;
using Print.Templates.Core;

namespace Print.Templates.Editor
{
    public class DocumentEditor : Control
    {
        static DocumentEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DocumentEditor), new FrameworkPropertyMetadata(typeof(DocumentEditor)));
        }

        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(
            nameof(Document), typeof(DocumentBuildParameter), typeof(DocumentEditor), new PropertyMetadata(default(DocumentBuildParameter)));

        public DocumentBuildParameter Document
        {
            get { return (DocumentBuildParameter) GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }
    }
}
