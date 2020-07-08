using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Print.Templates.Core.EntryHost;
using Print.Templates.Core.Enum;

namespace Print.Templates.Parser
{
    internal class XmlParser : Parser
    {
        private static class Elements
        {
            private const string XSD                          = "http://otoha.moe/CoreTemplate.xsd";
            public static readonly XNamespace Ns              = XNamespace.Get(XSD);
            public static readonly XName CoreTemplate         = XName.Get(nameof(CoreTemplate),XSD);
            public static readonly XName LineFeed             = XName.Get(nameof(LineFeed),XSD);
            public static readonly XName TextEntry            = XName.Get(nameof(TextEntry), XSD);
            public static readonly XName TextElement          = XName.Get(nameof(TextElement), XSD);
            public static readonly XName Line                 = XName.Get(nameof(Line), XSD);
            public static readonly XName TableEntry           = XName.Get(nameof(TableEntry), XSD);
            public static readonly XName TableColumn          = XName.Get(nameof(TableColumn), XSD);
            public static readonly XName RowFooterTemplate    = XName.Get(nameof(RowFooterTemplate), XSD);
            public static readonly XAttribute LargeRowSpacing = new XAttribute("RowSpacing", "120");

            public static XAttribute FontSize(FontSize size = Core.Enum.FontSize.Small)
            {
                switch (size)
                {
                    case Core.Enum.FontSize.Small:
                        return new XAttribute("FontSize", "X1");
                    case Core.Enum.FontSize.Medium:
                        return new XAttribute("FontSize", "X1W2H");
                    case Core.Enum.FontSize.Large:
                        return new XAttribute("FontSize", "X2");
                    default:
                        throw new ArgumentOutOfRangeException(nameof(size), size, null);
                }
            }
            public static XAttribute Alignment(Alignment alignment = Core.Enum.Alignment.Left)
            {
                switch (alignment)
                {
                    case Core.Enum.Alignment.Left:
                        return new XAttribute("Alignment", "Left");
                    case Core.Enum.Alignment.Center:
                        return new XAttribute("Alignment", "Center");
                    case Core.Enum.Alignment.Right:
                        return new XAttribute("Alignment", "Right");
                    default:
                        throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null);
                }
            }
            public static XAttribute IsBold(bool bold)
            {
                return new XAttribute("IsBold", bold ? "True" : "False");
            }
        }

        protected override XElement BuildRoot()
        {
           var root = new XElement(Elements.CoreTemplate,
                Elements.FontSize(FontSize.Medium));
           return root;
        }

        protected override IEnumerable<XElement> BuildHeaderTitle(TitleParameter title, BlankLineParameter line)
        {
            var titleParameter = title?.Items.FirstOrDefault();
            if (titleParameter == null) yield break;
            if (string.IsNullOrWhiteSpace(titleParameter.Title)) yield break;

            var before    = line?.Before ?? 1;
            var after     = line?.After ?? 1;
            var alignment = titleParameter.Alignment ;
            var fontSize  = titleParameter.FontSize ;
            var bold      = titleParameter.Bold ;
            var titlestr  = titleParameter.Title ;

            for (var i = 0; i < before; i++)
            {
                yield return new XElement(Elements.LineFeed);
            }

            yield return new XElement(Elements.TextEntry,
                titlestr+"{BillName}",
                Elements.Alignment(alignment),
                Elements.FontSize(fontSize),
                Elements.IsBold(bold));

            for (var i = 0; i < after; i++)
            {
                yield return new XElement(Elements.LineFeed);
            }
        }

        protected override IEnumerable<XElement> BuildPrintInfo(PrintItemParameter parameter)
        {
            var src = parameter.Items.Where(x => x.Printable).OrderBy(x=>x.Order).ToList();
            if (!src.Any()) yield break;

            foreach (var item in src)
            {
                var entry = new XElement(Elements.TextEntry,
                    Elements.Alignment(item.Alignment),
                    Elements.FontSize(item.FontSize));
                if (item.FontSize != FontSize.Small)
                {
                    entry.Add(Elements.LargeRowSpacing);
                }

                entry.Add(new XElement(Elements.TextElement, item.Title));
                entry.Add(new XElement(Elements.TextElement, ParseBindingKey(item.BingdingKeys),
                    Elements.IsBold(item.ContentBold)));

                yield return entry;
            }
            
        }

        protected override IEnumerable<XElement> BuildBodyTable(TitleParameter title, PrintItemParameter parameter)
        {
            var titleParameter = title?.Items.FirstOrDefault();
            if (titleParameter == null) yield break;
            if (string.IsNullOrWhiteSpace(titleParameter.Title)) yield break;
            var cols = parameter?.Items.Where(x => x.Printable).ToList();
            if (cols?.Any() != true) yield break;

            var titlestr       = titleParameter.Title.Split(';');
            var titleFontSize  = titleParameter.FontSize;
            var titleAlignment = titleParameter.Alignment;
            var titleBold      = titleParameter.Bold;


            var body = new XElement(Elements.TableEntry,
                new XAttribute("ItemsSource", "PrintDetailsList"),
                new XAttribute("ShowRowFooter", "{HasRemark}"),
                Elements.FontSize(titleFontSize),
                new XAttribute("IsHeaderBold", titleBold),
                Elements.LargeRowSpacing);

            #region Columns

            for (int i = 0; i < cols.Count; i++)
            {
                var col = cols[i];
                if (col.BingdingKeys == BindingKeys.DISH_REMARK)
                {
                    continue;
                }
                var header = (titlestr.Length > i) ? titlestr[i] : " ";
                var element = new XElement(Elements.TableColumn,
                    new XAttribute("Width", col.CWidth),
                    new XAttribute("HeaderAlignment", titleAlignment),
                    new XAttribute("ContentAlignment", col.Alignment),
                    new XAttribute("Content", ParseBindingKey(col.BingdingKeys)),
                    new XAttribute("Header", header));

                body.Add(element);
            }

            var remarkCol = cols.FirstOrDefault(x => x.BingdingKeys == BindingKeys.DISH_REMARK && x.Printable);
            if (remarkCol != null)
            {
                var element = new XElement(Elements.RowFooterTemplate,
                    ParseBindingKey(remarkCol.BingdingKeys),
                    Elements.FontSize(remarkCol.FontSize),
                    Elements.Alignment(remarkCol.Alignment),
                    Elements.IsBold(remarkCol.ContentBold));
                body.Add(element);
            }

            #endregion
            yield return new XElement(Elements.Line);
            yield return body;
            yield return new XElement(Elements.Line);
        }

        protected override string ParseBindingKey(BindingKeys bindingKey)
        {
            switch (bindingKey)
            {
                case BindingKeys.BILL_BILLNAME:
                    return "{BillName}";
                case BindingKeys.BILL_TABLENO:
                    return "{TableNumber}";
                case BindingKeys.BILL_TABLENONAME:
                    return "{TableNo}";
                case BindingKeys.BILL_PEOPLE:
                    return "{DishNumber}";
                case BindingKeys.BILL_WAITERNAME:
                    return "{WaiterName}";
                case BindingKeys.BILL_SUBMITTIME:
                    return "{SubmitTime}";
                case BindingKeys.BILL_ALLREMARK:
                    return "{AllRemark}";
                case BindingKeys.BILL_PRINTTIME:
                    return "{PrintTime}";
                case BindingKeys.BILL_ENDMARK:
                    return "**本单结束**";
                case BindingKeys.BILL_REMARK:
                    return "{Remark}";
                case BindingKeys.DISH_DISHNAME:
                    return "{DishName}";
                case BindingKeys.DISH_DISHNAMETYPE:
                    return "{DishName}/{DishTypeName}";
                case BindingKeys.DISH_AMOUNT:
                    return "{Number}";
                case BindingKeys.DISH_UNIT:
                    return "{UnitName}";
                case BindingKeys.DISH_AMOUNTUNIT:
                    return "{Number}/{UnitName}";
                case BindingKeys.DISH_PRICE:
                    return "{Price}";
                case BindingKeys.DISH_TOTALMONEY:
                    return "{TotalMoney}";
                case BindingKeys.DISH_REMARK:
                    return "    备注：{Remark}";
                default:
                    return "";
            }
        }
    }
}
