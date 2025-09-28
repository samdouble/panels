using iText.Kernel.Colors;
using iText.Layout;
using Panels.Elements;
using Panels.Utils;
using System.Xml;

namespace Panels.Elements
{
	class Description : Text
	{
		private readonly Document document;
		private readonly bool visible = true;

		public Description(
			Document document,
			XmlNode element,
			Panel parent,
			TextOptions textOptions = new TextOptions()
		) : base(document, element, parent, textOptions)
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