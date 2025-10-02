using iText.Kernel.Colors;
using iText.Layout;
using Panels.Elements;
using Panels.Utils;
using System.Xml;
using System.Xml.Serialization;

namespace Panels.Elements
{
	[XmlType("description")]
	public class Description : Text
	{
		[XmlIgnore]
		private Document document;

		[XmlIgnore]
		private Color color = ColorConstants.RED;
		private bool visible = true;

		public Description()
		{
			this.color = ColorConstants.RED;
		}

		[XmlAttribute("visible")]
		public bool Visible
		{
			get { return visible; }
			set { visible = value; }
		}

		[XmlAttribute("text")]
		public string TextContent
		{
			get { return base.text; }
			set { base.text = value; }
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