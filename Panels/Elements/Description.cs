using iText.Kernel.Colors;
using iText.Layout;
using Panels.Elements;
using Panels.Utils;
using System.Xml;

namespace Panels.Elements
{
    class Description : Text
    {
        private Document document;
        protected bool visible = true;

        public Description(Document document, XmlNode element, Panel parent) : base(document, element, parent)
        {
            this.document = document;
            this.color = ColorConstants.RED;
            this.visible = element.Attributes["visible"] != null
                ? bool.Parse(element.Attributes["visible"].InnerText)
                : true;
        }

        public override void Render(LogWriter logWriter)
        {
            if (this.visible)
            {
                base.Render(logWriter);
            }
        }
    }
}
