using System.Xml.Serialization;

namespace Print.Templates.Core.Enum
{
    public enum FontSize
    {
        [XmlEnum(Name = nameof(Small))] 
        Small         = 13,
        [XmlEnum(Name = nameof(Medium))] 
        Medium        = 16,
        [XmlEnum(Name = nameof(Large))] 
        Large         = 22
    }
}
