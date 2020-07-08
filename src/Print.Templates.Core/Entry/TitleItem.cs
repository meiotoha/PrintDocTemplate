using System.Xml.Serialization;
using Print.Templates.Core.Enum;

namespace Print.Templates.Core.Entry
{
    public class TitleItem : ObservableObject
    {
        private string _title;
        private FontSize _fontSize;
        private Alignment _alignment;
        private bool _bold;

        [XmlAttribute]
        public string Title
        {
            get => _title;
            set => SetAndNotify(ref _title, value);
        }

        [XmlAttribute]
        public FontSize FontSize
        {
            get => _fontSize;
            set => SetAndNotify(ref _fontSize, value);
        }

        [XmlAttribute]
        public Alignment Alignment
        {
            get => _alignment;
            set => SetAndNotify(ref _alignment, value);
        }

        [XmlAttribute]
        public bool Bold
        {
            get => _bold;
            set => SetAndNotify(ref _bold, value);
        }

    }
}
