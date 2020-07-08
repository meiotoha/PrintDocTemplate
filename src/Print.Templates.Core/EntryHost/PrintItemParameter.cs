using System.Collections.Generic;
using System.Xml.Serialization;
using Print.Templates.Core.Entry;

namespace Print.Templates.Core.EntryHost
{
    public class PrintItemParameter
    {
        [XmlElement(nameof(PrintItem))] public List<PrintItem> Items { get; set; }
    }
}
