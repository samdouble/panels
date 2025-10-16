using iText.Kernel.Colors;
using iText.Layout;
using Newtonsoft.Json;
using Panels.Elements;
using Panels.Utils;
using System.Xml;
using System.Xml.Serialization;

namespace Panels.Elements
{
	[XmlType("description")]
	[JsonObject]
	public class Description : Text
	{
		[XmlIgnore]
		[JsonIgnore]
		private Document? document;

		[XmlIgnore]
		[JsonIgnore]
		private new Color color = ColorConstants.RED;

		[XmlIgnore]
		[JsonIgnore]
		private bool visible = true;

		public Description()
		{
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
		public new string? TextContent
		{
			get { return base.TextContent; }
			set { base.TextContent = value; }
		}

		public new void Initialize(Document document, Panel parent)
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