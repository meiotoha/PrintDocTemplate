using System.Xml.Serialization;
using Print.Templates.Core.Enum;

namespace Print.Templates.Core.Entry
{
    public class PrintItem : ObservableObject
    {
        private FontSize _fontSize;
        private Alignment _alignment;
        private bool _printable;
        private ushort _order;
        private string _width;
        private ushort _cwidth;
        private bool _contentBold;

        [XmlAttribute]
        public BindingKeys BingdingKeys { get; set; }

        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public string Content { get; set; }

        [XmlAttribute]
        public string Width
        {
            get => _width;
            set => SetAndNotify(ref _width, value);
        }

        [XmlAttribute]
        public ushort Order
        {
            get => _order;
            set => SetAndNotify(ref _order, value);
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
        public bool Printable
        {
            get => _printable;
            set => SetAndNotify(ref _printable, value);
        }

        [XmlAttribute]
        public ushort CWidth
        {
            get { return _cwidth; }
            set { SetAndNotify(ref _cwidth, value); }
        }

        [XmlAttribute]
        public bool ContentBold
        {
            get { return _contentBold; }
            set { SetAndNotify(ref _contentBold, value); }
        }

    }
}
