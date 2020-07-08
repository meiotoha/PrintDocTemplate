using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Print.Templates.Core;
using Print.Templates.Core.Entry;
using Print.Templates.Core.EntryHost;
using Print.Templates.Core.Enum;

namespace Print.Templates.Parser
{
    internal class XamlParser : Parser
    {
        private static class Elements
        {
            private const string NS_PRESENTATION                              = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
            private const string NS_XAML                                      = "http://schemas.microsoft.com/winfx/2006/xaml";
            public static readonly XNamespace Ns                              = XNamespace.Get(NS_PRESENTATION);
            public static readonly XNamespace NsX                             = XNamespace.Get(NS_XAML);
            public static readonly XName XmlnsX                               = XNamespace.Xmlns + "x"; // xmlns:x
            public static readonly XName XmlnsXName                           = NsX + "Name";     //x:Name
            public static readonly XName FlowDocument                         = XName.Get("FlowDocument", NS_PRESENTATION);
            public static readonly XName FlowDocumentResources                = XName.Get("FlowDocument.Resources", NS_PRESENTATION);
            public static readonly XName Style                                = XName.Get("Style", NS_PRESENTATION);
            public static readonly XName Setter                               = XName.Get("Setter", NS_PRESENTATION);
            public static readonly XName Table                                = XName.Get("Table", NS_PRESENTATION);
            public static readonly XName TableRowGroup                        = XName.Get("TableRowGroup", NS_PRESENTATION);
            public static readonly XName TableRowGroupTag                     = XName.Get("TableRowGroup.Tag", NS_PRESENTATION);
            public static readonly XName TableColumn                          = XName.Get("TableColumn", NS_PRESENTATION);
            public static readonly XName TableRow                             = XName.Get("TableRow", NS_PRESENTATION);
            public static readonly XName TableCell                            = XName.Get("TableCell", NS_PRESENTATION);
            public static readonly XName Paragraph                            = XName.Get("Paragraph", NS_PRESENTATION);
            public static readonly XName Run                                  = XName.Get("Run", NS_PRESENTATION);
            public static readonly XName LineBreak                            = XName.Get("LineBreak", NS_PRESENTATION);
            public static readonly XAttribute PreserveSpace                   = new XAttribute(XNamespace.Xml + "space", "preserve");

            public static XAttribute FontSize(FontSize size = Core.Enum.FontSize.Small)
            {
                switch (size)
                {
                    case Core.Enum.FontSize.Small:
                        return new XAttribute("FontSize", "10pt");
                    case Core.Enum.FontSize.Medium:
                        return new XAttribute("FontSize", "12pt");
                    case Core.Enum.FontSize.Large:
                        return new XAttribute("FontSize", "16pt");
                    default:
                        throw new ArgumentOutOfRangeException(nameof(size), size, null);
                }
            }
            public static XAttribute Alignment(Alignment alignment = Core.Enum.Alignment.Left)
            {
                switch (alignment)
                {
                    case Core.Enum.Alignment.Left:
                        return new XAttribute("TextAlignment", "Left");
                    case Core.Enum.Alignment.Center:
                        return new XAttribute("TextAlignment", "Center");
                    case Core.Enum.Alignment.Right:
                        return new XAttribute("TextAlignment", "Right");
                    default:
                        throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null);
                }
            }
            public static XAttribute Bold(bool bold)
            {
                return new XAttribute("FontWeight", bold ? "Bold" : "Normal");
            }
        }
        protected override XElement BuildRoot()
        {
           var root = new XElement(Elements.FlowDocument,
                new XAttribute(Elements.XmlnsX, Elements.NsX),
                new XAttribute(Elements.XmlnsXName, "printArea"),
                new XAttribute("FontFamily", "Microsoft YaHei"),
                Elements.FontSize(FontSize.Small),
                new XAttribute("PageWidth", "215.433pt"),
                new XAttribute("PagePadding", "13pt 0 8.173pt 0"),
                new XAttribute("TextOptions.TextFormattingMode", "Display"));

            var resourcePart = new XElement(Elements.FlowDocumentResources,
                new XElement(Elements.Style, new XAttribute("TargetType", "{x:Type Paragraph}"),
                    new XElement(Elements.Setter, new XAttribute("Property", "Margin"),
                        new XAttribute("Value", 0))),
                new XElement(Elements.Style, new XAttribute("TargetType", "{x:Type Table}"),
                    new XElement(Elements.Setter, new XAttribute("Property", "Margin"),
                        new XAttribute("Value", 0))));
            root.Add(resourcePart);

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

            var titleCore = new XElement(Elements.Paragraph,
                Elements.Alignment(alignment),
                Elements.FontSize(fontSize),
                Elements.Bold(bold));

            for (var i = 0; i < before; i++)
            {
                titleCore.Add(new XElement(Elements.LineBreak));
            }

            titleCore.Add(new XElement(Elements.Run,
                new XAttribute("Text", titlestr)));
            titleCore.Add(new XElement(Elements.Run,
                new XAttribute("Text", "{Binding BillName}")));

            for (var i = 0; i < after; i++)
            {
                titleCore.Add(new XElement(Elements.LineBreak));
            }

            var titleRoot = new XElement(Elements.Table,
                new XAttribute("BorderThickness", "0"),
                new XAttribute("BorderBrush", "Black"),
                new XAttribute("Padding", "0,0,0,5"),
                new XElement(Elements.TableRowGroup,
                    new XElement(Elements.TableRow,
                        new XElement(Elements.TableCell, new XAttribute("ColumnSpan", 2),
                            titleCore))));

            yield return titleRoot;
        }
        protected override IEnumerable<XElement> BuildPrintInfo(PrintItemParameter parameter)
        {
            var src = parameter.Items.Where(x => x.Printable).OrderBy(x=>x.Order).ToList();
            if (!src.Any()) yield break;

            var root = new XElement(Elements.Table);
            var gp = new XElement(Elements.TableRowGroup);
            foreach (var item in src)
            {
                gp.Add(new XElement(Elements.TableRow,
                    new XElement(Elements.TableCell,
                        new XElement(Elements.Paragraph,
                            Elements.FontSize(item.FontSize),
                            Elements.Alignment(item.Alignment),
                            new XElement(Elements.Run,
                                new XAttribute("Text", item.Title)),
                            new XElement(Elements.Run,
                                new XAttribute("Text", ParseBindingKey(item.BingdingKeys)),
                                Elements.Bold(item.ContentBold))))));
            }
            root.Add(gp);
            yield return root;
        }
        protected override IEnumerable<XElement> BuildBodyTable(TitleParameter title, PrintItemParameter parameter)
        {
            var titleParameter = title?.Items.FirstOrDefault();
            if (titleParameter == null) yield break;
            if (string.IsNullOrWhiteSpace(titleParameter.Title)) yield break;
            var cols = parameter?.Items.Where(x => x.Printable).ToList();
            if (cols?.Any() != true) yield break;

            var body = new XElement(Elements.Table,
                new XAttribute("BorderBrush", "Black"),
                new XAttribute("BorderThickness", "0 1 0 1"));
            var titlestr       = titleParameter.Title.TrimEnd(';').Split(';');
            var titleFontSize  = titleParameter.FontSize;
            var titleAlignment = titleParameter.Alignment;
            var titleBold      = titleParameter.Bold;

            #region Table.Columns
            var c = new XElement(Elements.Table + ".Columns");
            foreach (var col in cols)
            {
                c.Add(new XElement(Elements.TableColumn, new XAttribute("Width", col.Width ?? "*")));
            }
            body.Add(c);
            #endregion

            #region TableRowGroup
            var b = new XElement(Elements.TableRowGroup, new XAttribute(Elements.XmlnsXName, "printDetailsList"));

            #region TableRowGroup.Tag ColSettings

            var extendInfo = Utils.SerializeToXml(cols);
         
            if (!string.IsNullOrWhiteSpace(extendInfo))
            {
                var tag = new XElement(Elements.TableRowGroupTag, extendInfo);
                b.Add(tag);
            }
            #endregion


            var g = new XElement(Elements.TableRow);
            foreach (var t in titlestr)
            {
                g.Add(new XElement(Elements.TableCell,
                    new XElement(Elements.Paragraph,
                        t,
                        Elements.Alignment(titleAlignment),
                        Elements.FontSize(titleFontSize)),
                        Elements.Bold(titleBold)));
            }
            g.Add(new XElement(Elements.TableCell)); //extend line
            b.Add(g);
            body.Add(b);
            #endregion

            yield return body;
        }
        protected override string ParseBindingKey(BindingKeys bindingKey)
        {
            switch (bindingKey)
            {
                case BindingKeys.NONE:
                    return string.Empty;
                case BindingKeys.BILL_BILLNAME:
                    return "{Binding BillName}";
                case BindingKeys.BILL_TABLENO:
                    return "{Binding TableNumber}";
                case BindingKeys.BILL_TABLENONAME:
                    return "{Binding TableNo}";
                case BindingKeys.BILL_PEOPLE:
                    return "{Binding DishNumber}";
                case BindingKeys.BILL_WAITERNAME:
                    return "{Binding WaiterName}";
                case BindingKeys.BILL_SUBMITTIME:
                    return "{Binding SubmitTime}";
                case BindingKeys.BILL_ALLREMARK:
                    return "{Binding AllRemark}";
                case BindingKeys.BILL_PRINTTIME:
                    return "{Binding PrintTime}";
                case BindingKeys.BILL_ENDMARK:
                    return "**本单结束**";
                case BindingKeys.BILL_REMARK:
                    return "{Binding Remark}";
                default:
                    return "";
            }
        }
    }
}
