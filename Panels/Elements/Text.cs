using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Element;
using Newtonsoft.Json;
using Panels.Utils;
using System;
using System.Xml;
using System.Xml.Serialization;

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
		private PdfFont? font;
		private float? fontSize;
		private float _left = 0;
		private string? _text;
		private float _top = 0;
		private float? _width;

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
			get { return _left; }
			set { _left = value; }
		}

		[XmlAttribute("text")]
		[JsonProperty("text")]
		public string? TextContent
		{
			get { return _text; }
			set { _text = value; }
		}

		[XmlAttribute("top")]
		[JsonProperty("top")]
		public float Top
		{
			get { return _top; }
			set { _top = value; }
		}

		[XmlAttribute("width")]
		public string? WidthXml
		{
			get => _width.HasValue ? XmlConvert.ToString(_width.Value) : null;
			set => _width = string.IsNullOrEmpty(value) ? null : (float?)XmlConvert.ToSingle(value);
		}

		[XmlIgnore]
		[JsonProperty("width")]
		public float? Width
		{
			get { return _width; }
			set { _width = value; }
		}

		public Text()
		{
		}

		public void Initialize(Document document, Panel parent)
		{
			this.document = document;
			this.parent = parent;
		}

		private PdfFont GetFont()
		{
			if (this.font == null)
			{
				this.font = PdfFontFactory.CreateFont(Properties.Resources.Comicsam_Bold, PdfEncodings.CP1252);
			}
			return this.font;
		}

		public override void Render(LogWriter logWriter)
		{
			if (this.parent == null || this.document == null || this._text == null) return;

			var left = this.parent.Position.X + this._left + MARGIN;
			float right;
			if (this._width is float _width)
			{
				right = this.parent.Position.X + this._left + Math.Min(_width, this.parent.Width - this._left) - MARGIN;
			}
			else
			{
				right = this.parent.Position.X + this.parent.Width - MARGIN;
			}

			var top = this.parent.Position.Y - this._top - 3;
			var bottom = this.parent.Position.Y - this.parent.Height + MARGIN;
			var phraseWidth = right - left;
			Paragraph phrase = new Paragraph(this._text);
			phrase.SetFixedLeading(LINE_HEIGHT);
			phrase.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.TOP);
			phrase.SetHeight(top - bottom);
			phrase.SetFont(this.GetFont());
			phrase.SetFontSize(this.FontSize);
			phrase.SetFixedPosition(this.noPage, left, bottom, phraseWidth);
			phrase.SetFontColor(this.color);
			logWriter.Log("TEXT - " + this._text + " at " + left + ", " + bottom + " with width " + phraseWidth);
			this.document.Add(phrase);
		}
	}
}