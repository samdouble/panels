using iText.Layout;
using Panels.Utils;

namespace Panels
{
    interface IRenderable
    {
        void Render(Document doc, LogWriter logWriter);
    }
}
