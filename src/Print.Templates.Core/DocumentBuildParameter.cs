using System.IO;
using System.Text;
using System.Xml.Serialization;
using Print.Templates.Core.Section;

namespace Print.Templates.Core
{
    public class DocumentBuildParameter
    {
        [XmlElement] public HeaderSection Header { get; set; }
        [XmlElement] public BodySection Body { get; set; }
        [XmlElement] public FootSection Footer { get; set; }
    }
}
