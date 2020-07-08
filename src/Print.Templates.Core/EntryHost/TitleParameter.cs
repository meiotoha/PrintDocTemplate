using System.Collections.Generic;
using System.Xml.Serialization;
using Print.Templates.Core.Entry;

namespace Print.Templates.Core.EntryHost
{
    public class TitleParameter
    {
        [XmlElement(nameof(TitleItem))] public List<TitleItem> Items { get; set; }
    }
}
