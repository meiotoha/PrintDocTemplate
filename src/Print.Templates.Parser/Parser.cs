using System.Collections.Generic;
using System.Xml.Linq;
using Print.Templates.Core;
using Print.Templates.Core.EntryHost;
using Print.Templates.Core.Enum;
using Print.Templates.Core.Section;

namespace Print.Templates.Parser
{
    public abstract class Parser
    {
        public static Parser GetXamlParser()
        {
            return new XamlParser();
        }
        public static Parser GetXmlParser()
        {
            return new XmlParser();
        }

        // ReSharper disable once InconsistentNaming
        protected const string COMMENTSTR ="此模板由生成器自动生成,删除或更改此文件可能导致软件执行异常,在编辑器中编辑模板保存将会覆盖对此文件的任何更改。\n" +
                                           "TO MODIFY THIS FILE ,SEE FLOWDOCUMENT DOCUMENT( FOR DRIVER ) OR CORETEMPLATE.XSD (FOR ESC/POS) ";

        public virtual XDocument Build(DocumentBuildParameter parameter)
        {
            var doc = BuildDocument();
            var root = BuildRoot();
            if (parameter.Header != null)
            {
                var header = BuildHeader(parameter.Header);
                root.Add(header);
            }
            if (parameter.Body != null)
            {
                var body = BuildBody(parameter.Body);
                root.Add(body);
            }
            if (parameter.Footer != null)
            {
                var footer = BuildFooter(parameter.Footer);
                root.Add(footer);
            }

            doc.Add(root);
            return doc;
        }
        protected virtual XDocument BuildDocument()
        {
            var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XComment(COMMENTSTR));
            return document;
        }
        protected abstract XElement BuildRoot();
        protected virtual IEnumerable<XElement> BuildHeader(HeaderSection section)
        {
            var titles = BuildHeaderTitle(section.TitleParameter, section.BlankLineParameter);
            foreach (var xElement in titles)
            {
                yield return xElement;
            }

            var printinfos = BuildPrintInfo(section.PrintItemParameter);
            foreach (var xElement in printinfos)
            {
                yield return xElement;
            }
        }
        protected virtual IEnumerable<XElement> BuildBody(BodySection section)
        {
            var body = BuildBodyTable(section.TitleParameter, section.PrintItemParameter);
            foreach (var xElement in body)
            {
                yield return xElement;
            }
        }
        protected virtual IEnumerable<XElement> BuildFooter(FootSection section)
        {
            var printinfos = BuildPrintInfo(section.PrintItemParameter);
            foreach (var xElement in printinfos)
            {
                yield return xElement;
            }
        }
        protected abstract IEnumerable<XElement> BuildHeaderTitle(TitleParameter title, BlankLineParameter line);
        protected abstract IEnumerable<XElement> BuildPrintInfo(PrintItemParameter parameter);
        protected abstract IEnumerable<XElement> BuildBodyTable(TitleParameter title, PrintItemParameter parameter);
        
        protected virtual string ParseBindingKey(BindingKeys bindingKey)
        {
            return bindingKey.ToString();
        }
    }
}
