using System.Xml.Serialization;
using Print.Templates.Core.EntryHost;

namespace Print.Templates.Core.Section
{
    public class HeaderSection
    {
        [XmlElement] public BlankLineParameter BlankLineParameter { get; set; }
        [XmlElement] public TitleParameter TitleParameter { get; set; }
        [XmlElement] public PrintItemParameter PrintItemParameter { get; set; }
    }
}
