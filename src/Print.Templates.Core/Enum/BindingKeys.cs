using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Print.Templates.Core.Enum
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]

    public enum BindingKeys
    {
        [XmlEnum(Name    = nameof(NONE))]
        NONE             =0,
        [XmlEnum(Name    = nameof(BILL_BILLNAME))]
        BILL_BILLNAME    = 0b00000001,
        [XmlEnum(Name    = nameof(BILL_TABLENO))]
        BILL_TABLENO     = 0b00000010,
        [XmlEnum(Name    = nameof(BILL_TABLENONAME))]
        BILL_TABLENONAME = 0b00000011,
        [XmlEnum(Name    = nameof(BILL_PEOPLE))]
        BILL_PEOPLE      = 0b00000100,
        [XmlEnum(Name    = nameof(BILL_WAITERNAME))]
        BILL_WAITERNAME  = 0b00000101,
        [XmlEnum(Name    = nameof(BILL_SUBMITTIME))]
        BILL_SUBMITTIME  = 0b00000110,
        [XmlEnum(Name    = nameof(BILL_ALLREMARK))]
        BILL_ALLREMARK   = 0b00000111,
        [XmlEnum(Name    = nameof(BILL_PRINTTIME))]
        BILL_PRINTTIME   = 0b00001000,
        [XmlEnum(Name    = nameof(BILL_ENDMARK))]
        BILL_ENDMARK     = 0b00001001,
        [XmlEnum(Name    = nameof(BILL_REMARK))]
        BILL_REMARK      = 0b00001011,
        [XmlEnum(Name    = nameof(DISH_DISHNAME))]
        DISH_DISHNAME    = 0b10000001,
        [XmlEnum(Name    = nameof(DISH_DISHNAMETYPE))]
        DISH_DISHNAMETYPE= 0b10000010,
        [XmlEnum(Name    = nameof(DISH_AMOUNT))]
        DISH_AMOUNT      = 0b10000011,
        [XmlEnum(Name    = nameof(DISH_UNIT))]
        DISH_UNIT        = 0b10000100,
        [XmlEnum(Name    = nameof(DISH_AMOUNTUNIT))]
        DISH_AMOUNTUNIT  = 0b10000101,
        [XmlEnum(Name    = nameof(DISH_PRICE))]
        DISH_PRICE       = 0b10000110,
        [XmlEnum(Name    = nameof(DISH_TOTALMONEY))]
        DISH_TOTALMONEY  = 0b10000111,
        [XmlEnum(Name    = nameof(DISH_REMARK))]
        DISH_REMARK      = 0b10001000,
    }
}
