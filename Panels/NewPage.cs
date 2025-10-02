using iText.Layout;
using Panels.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Panels
{
	[XmlType("newpage")]
	[JsonObject]
	public class NewPage : IRenderable
	{
		[XmlIgnore]
		private Document document;
		[XmlIgnore]
		private Comic parent;

		public NewPage()
		{
		}

		public void Initialize(Document document, Comic parent)
		{
			this.document = document;
			this.parent = parent;
		}

		// IRenderable
		public void Render(LogWriter logWriter)
		{
			if (this.parent.CurrentX != 0 || this.parent.CurrentY != 0)
			{
				this.document.GetPdfDocument().AddNewPage();
				this.parent.CurrentPage++;
				this.parent.CurrentRow = this.parent.CurrentPage * this.parent.rowsPerPage;
				this.parent.CurrentX = 0;
				this.parent.CurrentY = 0;
			}
			logWriter.Log("NEW PAGE");
		}
	}
}