using iText.Layout;
using Panels.Utils;
using System.Xml;

namespace Panels.Elements
{
    public abstract class Element : IPositionable, IRenderable
    {
        protected int noPage;
        protected float x;
        protected float y;

        public Element(XmlNode element)
        {

        }

        // IPositionable
        public void SetPosition(int noPage, float x, float y)
        {
            this.noPage = noPage;
            this.x = x;
            this.y = y;
        }

        // IRenderable
        public abstract void Render(LogWriter logWriter);
    }
}
