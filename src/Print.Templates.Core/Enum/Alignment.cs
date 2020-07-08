using System.Xml.Serialization;

namespace Print.Templates.Core.Enum
{
    public enum Alignment
    {
        [XmlEnum(Name = nameof(Left))] Left = 0,
        [XmlEnum(Name = nameof(Right))] Right = 1,
        [XmlEnum(Name = nameof(Center))] Center = 2
    }
}
