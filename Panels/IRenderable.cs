using iText.Layout;
using Panels.Utils;

namespace Panels
{
	public interface IRenderable
	{
		void Render(LogWriter logWriter);
	}
}