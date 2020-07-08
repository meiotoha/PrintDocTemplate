using System.Xml.Serialization;
using Print.Templates.Core.EntryHost;

namespace Print.Templates.Core.Section
{
    public class FootSection
    {
        [XmlElement] public PrintItemParameter PrintItemParameter { get; set; }
    }
}
