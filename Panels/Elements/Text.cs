using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Element;
using Panels.Utils;
using System;
using System.Xml;

namespace Panels.Elements
{
    public struct TextOptions
    {
        public int FontSize { get; init; }
    }

    class Text : Element
    {
        private Document document;
        protected Panel parent;
        protected string text;
        protected Color color = ColorConstants.BLACK;
        protected const int LINE_HEIGHT = 11;
        protected const int MARGIN = 5;
        protected PdfFont font;
        protected int fontSize;
        protected float left = 0;
        protected float top = 0;
        protected float? width;

        public Text(Document document, XmlNode element, Panel parent, TextOptions textOptions = new TextOptions()) : base(element)
        {
            this.document = document;
            this.parent = parent;
            this.text = element.Attributes["text"]?.InnerText;
            this.fontSize = textOptions.FontSize;
            // Optional
            this.left = element?.Attributes["left"] != null ? float.Parse(element.Attributes["left"].InnerText) : 0.0f;
            this.top = element?.Attributes["top"] != null ? float.Parse(element.Attributes["top"].InnerText) : 0.0f;
            this.width = element?.Attributes["width"] != null ? float.Parse(element.Attributes["width"].InnerText) : (float?)null;
            // Load Font
            this.font = PdfFontFactory.CreateFont(Properties.Resources.Comicsam_Bold, PdfEncodings.CP1252);
        }

        public override void Render(LogWriter logWriter)
        {
            float left = this.parent.Position.X + this.left + MARGIN;
            float right;
            if (this.width is float width)
            {
                right = this.parent.Position.X + this.left + Math.Min(width, this.parent.Width - this.left) - MARGIN;
            }
            else
            {
                right = this.parent.Position.X + this.parent.Width - MARGIN;
            }

            float top = this.parent.Position.Y - this.top - 3;
            float bottom = this.parent.Position.Y - this.parent.Height + MARGIN;
            float phraseWidth = right - left;
            Paragraph phrase = new Paragraph(this.text);
            phrase.SetFixedLeading(LINE_HEIGHT);
            phrase.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.TOP);
            phrase.SetHeight(top - bottom);
            phrase.SetFont(this.font);
            phrase.SetFontSize(this.fontSize);
            phrase.SetFixedPosition(this.noPage, left, bottom, phraseWidth);
            phrase.SetFontColor(this.color);
            logWriter.Log("TEXT - " + this.text + " at " + left + ", " + bottom + " with width " + phraseWidth);
            this.document.Add(phrase);
        }
    }
}
