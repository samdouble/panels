using iText.Kernel.Colors;
using iText.Layout;
using Panels.Elements;
using Panels.Utils;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Panels.Elements
{
	[XmlType("description")]
	[JsonObject]
	public class Description : Text
	{
		[XmlIgnore]
		[JsonIgnore]
		private Document document;

		[XmlIgnore]
		[JsonIgnore]
		private Color color = ColorConstants.RED;
		private bool visible = true;

		public Description()
		{
			this.color = ColorConstants.RED;
		}

		[XmlAttribute("visible")]
		[JsonProperty("visible")]
		public bool Visible
		{
			get { return visible; }
			set { visible = value; }
		}

		[XmlAttribute("text")]
		[JsonProperty("text")]
		public new string TextContent
		{
			get { return base.TextContent; }
			set { base.TextContent = value; }
		}

		public void Initialize(Document document, Panel parent)
		{
			this.document = document;
			base.Initialize(document, parent);
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