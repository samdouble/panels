using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Element;
using Panels.Utils;
using System;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Panels.Elements
{
	[XmlType("text")]
	[JsonObject]
	public class Text : Element
	{
		private Document? document;
		private Panel? parent;
		protected Color color { get; set; } = ColorConstants.BLACK;
		private const int LINE_HEIGHT = 11;
		private const int MARGIN = 5;
		private PdfFont font = PdfFontFactory.CreateFont(Properties.Resources.Comicsam_Bold, PdfEncodings.CP1252);
		private float? fontSize;
		private float left = 0;
		private string? text;
		private float top = 0;
		private float? width;

		[XmlAttribute("fontSize")]
		[JsonProperty("fontSize")]
		public float FontSize
		{
			get { return fontSize ?? parent?.FontSize ?? 12f; }
			set { fontSize = value; }
		}

		[XmlAttribute("left")]
		[JsonProperty("left")]
		public float Left
		{
			get { return left; }
			set { left = value; }
		}

		[XmlAttribute("text")]
		[JsonProperty("text")]
		public string? TextContent
		{
			get { return text; }
			set { text = value; }
		}

		[XmlAttribute("top")]
		[JsonProperty("top")]
		public float Top
		{
			get { return top; }
			set { top = value; }
		}

		[XmlIgnore]
		[JsonIgnore]
		public float? Width
		{
			get { return width; }
			set { width = value; }
		}

		public Text()
		{
			this.font = PdfFontFactory.CreateFont(Properties.Resources.Comicsam_Bold, PdfEncodings.CP1252);
		}

		public void Initialize(Document document, Panel parent)
		{
			this.document = document;
			this.parent = parent;
		}

		public override void Render(LogWriter logWriter)
		{
			if (this.parent == null || this.document == null || this.text == null) return;

			var left = this.parent.Position.X + this.left + MARGIN;
			float right;
			if (this.width is float width)
			{
				right = this.parent.Position.X + this.left + Math.Min(width, this.parent.Width - this.left) - MARGIN;
			}
			else
			{
				right = this.parent.Position.X + this.parent.Width - MARGIN;
			}

			var top = this.parent.Position.Y - this.top - 3;
			var bottom = this.parent.Position.Y - this.parent.Height + MARGIN;
			var phraseWidth = right - left;
			Paragraph phrase = new Paragraph(this.text);
			phrase.SetFixedLeading(LINE_HEIGHT);
			phrase.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.TOP);
			phrase.SetHeight(top - bottom);
			phrase.SetFont(this.font);
			phrase.SetFontSize(this.FontSize);
			phrase.SetFixedPosition(this.noPage, left, bottom, phraseWidth);
			phrase.SetFontColor(this.color);
			logWriter.Log("TEXT - " + this.text + " at " + left + ", " + bottom + " with width " + phraseWidth);
			this.document.Add(phrase);
		}
	}
}