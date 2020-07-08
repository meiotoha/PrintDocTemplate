using System.Xml.Serialization;

namespace Print.Templates.Core.EntryHost
{
    public class BlankLineParameter  : ObservableObject
    {
        private int _before;
        private int _after;

        [XmlAttribute]
        public int Before
        {
            get => _before;
            set => SetAndNotify(ref _before, value);
        }

        [XmlAttribute]
        public int After
        {
            get => _after;
            set => SetAndNotify(ref _after, value);
        }
    }
}
