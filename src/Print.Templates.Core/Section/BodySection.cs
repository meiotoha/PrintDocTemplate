using System.Xml.Serialization;
using Print.Templates.Core.EntryHost;

namespace Print.Templates.Core.Section
{
    public class BodySection
    {
        [XmlElement] public TitleParameter TitleParameter { get; set; }
        [XmlElement] public PrintItemParameter PrintItemParameter { get; set; }
    }
}
