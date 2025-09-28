using iText.Layout;
using Panels.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Panels
{
	class NewPage : IRenderable
	{
		private readonly Document document;
		private readonly Comic parent;

		public NewPage(Document document, Comic parent, XmlNode xmlSlot)
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
				this.parent.CurrentRow = this.parent.CurrentPage * this.parent.RowsPerPage;
				this.parent.CurrentX = 0;
				this.parent.CurrentY = 0;
			}
			logWriter.Log("NEW PAGE");
		}
	}
}