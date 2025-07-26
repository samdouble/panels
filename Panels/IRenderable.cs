using iText.Layout;
using Panels.Utils;

namespace Panels
{
    interface IRenderable
    {
        void Render(LogWriter logWriter);
    }
}
