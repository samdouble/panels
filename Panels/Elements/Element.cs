using iText.Layout;
using Panels.Utils;
using Newtonsoft.Json;

namespace Panels.Elements
{
	[JsonObject]
	public abstract class Element : IPositionable, IRenderable
	{
		protected int noPage { get; private set; }
		private float x;
		private float y;

		public Element()
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