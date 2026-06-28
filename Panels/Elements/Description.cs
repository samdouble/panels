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
		private new readonly Color color = ColorConstants.RED;

		[XmlIgnore]
		[JsonIgnore]
		private bool _visible = true;

		public Description()
		{
		}

		[XmlAttribute("text")]
		[JsonProperty("text")]
		public new string? TextContent
		{
			get { return base.TextContent; }
			set { base.TextContent = value; }
		}

		[XmlAttribute("visible")]
		[JsonProperty("visible")]
		public bool Visible
		{
			get { return _visible; }
			set { _visible = value; }
		}

		public new void Initialize(Document document, Panel parent)
		{
			this.document = document;
			base.Initialize(document, parent);
		}

		public override void Render(LogWriter logWriter)
		{
			if (this._visible)
			{
				base.Render(logWriter);
			}
		}
	}
}