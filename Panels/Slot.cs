using iText.Layout;
using Panels.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Panels
{
	[XmlType("slot")]
	public class Slot : IRenderable
	{
		[XmlIgnore]
		private Document document;
		[XmlIgnore]
		private Comic parent;

		[XmlElement("panel", Type = typeof(Panel))]
		public List<Panel> panels = new List<Panel>();
		
		[XmlAttribute("maxCropLeft")]
		public float MaxLeftPaddingPct { get; set; }
		
		[XmlAttribute("maxCropRight")]
		public float MaxRightPaddingPct { get; set; }
		
		[XmlIgnore]
		public float PaddingLeft { get; set; }
		
		[XmlIgnore]
		public float PaddingRight { get; set; }
		
		[XmlIgnore]
		public float Height { get; set; }
		public float Width
		{
			get
			{
				return panels.Count > 0 ? panels.Select(panel => panel.Width).Max() : 0f;
			}
		}

		public Slot()
		{
			this.PaddingLeft = 0f;
			this.PaddingRight = 0f;
		}

		public void Initialize(Document document, Comic parent)
		{
			this.document = document;
			this.parent = parent;
			foreach (var panel in this.panels)
			{
				panel.Initialize(document, parent);
			}
		}

		public void SetHeight(float height)
		{
			this.Height = height;
			var nbPanelsInSlot = this.panels.Count;
			var panelHeight = (height - (nbPanelsInSlot - 1) * parent.VerticalPanelSpacing) / nbPanelsInSlot;
			this.panels.ForEach(panel =>
			{
				panel.Height = panelHeight;
			});
		}

		public float GetMinWidth()
		{
			var minPctAvailable = 1 - ((this.MaxLeftPaddingPct + this.MaxRightPaddingPct) / 100);
			return minPctAvailable * this.Width;
		}

		public float GetMaxWidth()
		{
			return panels.Count > 0 ? panels.Select(panel => panel.Width).Max() : 0f;
		}

		public void Crop()
		{
			var nbPanelsInSlot = this.panels.Count;
			var leftCropping = (this.MaxLeftPaddingPct * this.Width / 100) - this.PaddingLeft;
			var rightCropping = (this.MaxRightPaddingPct * this.Width / 100) - this.PaddingRight;
			for (var i = 0; i < nbPanelsInSlot; i++)
			{
				Panel panel = this.panels[i];
				panel.Crop(0, rightCropping, 0, leftCropping);
			}
		}

		// IPositionable
		public void SetPosition(int noPage, float x, float y)
		{
			var nbPanelsInSlot = this.panels.Count;
			var panelHeight =
				(this.Height - (nbPanelsInSlot - 1) * parent.VerticalPanelSpacing) / nbPanelsInSlot;
			for (var i = 0; i < nbPanelsInSlot; i++)
			{
				Panel panel = this.panels[i];
				panel.Crop(0, 0, 0, 0);
				panel.SetPosition(noPage, x, y - i * panelHeight - (i - 1) * parent.VerticalPanelSpacing);
			}
		}

		// IRenderable
		public void Render(LogWriter logWriter)
		{
			this.panels.ForEach(panel => panel.Render(logWriter));
		}
	}
}