using iText.Layout;
using Newtonsoft.Json;
using Panels.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Panels
{
	[XmlType("slot")]
	[JsonObject]
	public class Slot : IRenderable
	{
		[XmlIgnore]
		private Document? document;

		[XmlIgnore]
		private Comic? parent;

		[XmlIgnore]
		[JsonIgnore]
		internal Comic? Parent => parent;

		[XmlIgnore]
		[JsonIgnore]
		private float? fontSize { get; set; }

		[XmlAttribute("fontSize")]
		[JsonProperty("fontSize")]
		public float FontSize
		{
			get { return fontSize ?? parent?.fontSize ?? 12f; }
			set { fontSize = value; }
		}

		[XmlIgnore]
		[JsonIgnore]
		public string? ImagesFolderPath
		{
			get { return parent?.ImagesFolderPath; }
		}

		[XmlIgnore]
		[JsonIgnore]
		public float MaxLeftPaddingPct { get; set; }

		[XmlIgnore]
		[JsonIgnore]
		public float MaxRightPaddingPct { get; set; }

		[XmlAttribute("maxPaddingLeft")]
		[JsonProperty("maxPaddingLeft")]
		public string MaxLeftPadding
		{
			get => PercentageParser.Format(this.MaxLeftPaddingPct);
			set => this.MaxLeftPaddingPct = PercentageParser.ParseRequired(value, "maxPaddingLeft");
		}

		[XmlAttribute("maxPaddingRight")]
		[JsonProperty("maxPaddingRight")]
		public string MaxRightPadding
		{
			get => PercentageParser.Format(this.MaxRightPaddingPct);
			set => this.MaxRightPaddingPct = PercentageParser.ParseRequired(value, "maxPaddingRight");
		}

		[XmlIgnore]
		[JsonIgnore]
		public float PaddingLeft { get; set; }

		[XmlIgnore]
		[JsonIgnore]
		public float PaddingRight { get; set; }

		[XmlIgnore]
		[JsonIgnore]
		public float Height { get; set; }

		[XmlIgnore]
		[JsonIgnore]
		public float Width
		{
			get
			{
				return panels.Count > 0 ? panels.Select(panel => panel.Width).Max() : 0f;
			}
		}

		[XmlElement("panel", Type = typeof(Panel))]
		[JsonProperty("panels")]
		public List<Panel> panels = new List<Panel>();

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
				panel.Initialize(document, this);
			}
		}

		public void SetHeight(float height)
		{
			this.Height = height;
			var nbPanelsInSlot = this.panels.Count;
			var verticalSpacing = this.parent?.VerticalPanelSpacing ?? 0f;
			var panelHeight = (height - (nbPanelsInSlot - 1) * verticalSpacing) / nbPanelsInSlot;
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
			var verticalSpacing = this.parent?.VerticalPanelSpacing ?? 0f;
			var panelHeight =
				(this.Height - (nbPanelsInSlot - 1) * verticalSpacing) / nbPanelsInSlot;
			for (var i = 0; i < nbPanelsInSlot; i++)
			{
				Panel panel = this.panels[i];
				panel.Crop(0, 0, 0, 0);
				panel.SetPosition(noPage, x, y - i * panelHeight - (i - 1) * verticalSpacing);
			}
		}

		// IRenderable
		public void Render(LogWriter logWriter)
		{
			this.panels.ForEach(panel => panel.Render(logWriter));
		}
	}
}