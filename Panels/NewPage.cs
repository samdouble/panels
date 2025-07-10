using iText.Layout;
using Panels.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Panels
{
    class NewPage : IRenderable
    {
        private Comic parent;

        public NewPage(Comic parent, XmlNode xmlSlot)
        {
            this.parent = parent;
        }

        // IRenderable
        public void Render(Document doc, LogWriter logWriter)
        {
            // this.panels.ForEach(panel => panel.Render(doc));
        }
    }
}
