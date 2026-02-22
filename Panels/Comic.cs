using iText.Kernel.Geom;
using iText.Layout;
using Newtonsoft.Json;
using Panels.Configuration;
using Panels.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Panels
{
	[XmlRoot("comic")]
	[JsonObject]
	public class Comic : IRenderable
	{
		private Document? document;
		protected const int DEFAULT_FONT_SIZE = 12;
		protected const int DEFAULT_ROWS_PER_PAGE = 3;

		[XmlAttribute("fontSize")]
		[JsonProperty("fontSize")]
		public int fontSize = DEFAULT_FONT_SIZE;

		[XmlAttribute("marginLeft")]
		[JsonProperty("marginLeft")]
		public float marginLeft = 0;

		[XmlAttribute("marginRight")]
		[JsonProperty("marginRight")]
		public float marginRight = 0;

		[XmlAttribute("marginTop")]
		[JsonProperty("marginTop")]
		public float marginTop = 0;

		[XmlAttribute("marginBottom")]
		[JsonProperty("marginBottom")]
		public float marginBottom = 0;

		[XmlAttribute("rowsPerPage")]
		[JsonProperty("rowsPerPage")]
		public int rowsPerPage { get; set; } = DEFAULT_ROWS_PER_PAGE;

		[XmlAttribute("showPageNumbers")]
		[JsonProperty("showPageNumbers")]
		public bool ShowPageNumbers { get; set; } = true;

		[XmlAttribute("horizontalPanelSpacing")]
		[JsonProperty("horizontalPanelSpacing")]
		public float horizontalPanelSpacing = 0;

		[XmlAttribute("verticalPanelSpacing")]
		[JsonProperty("verticalPanelSpacing")]
		public float VerticalPanelSpacing { get; set; } = 0;

		[XmlElement("newpage", Type = typeof(NewPage))]
		[XmlElement("slot", Type = typeof(Slot))]
		[JsonProperty("children")]
		public List<object> children { get; set; } = new List<object>();

		[XmlIgnore]
		public string? ImagesFolderPath { get; private set; }
		public int CurrentPage { get; set; } = 1;
		public int CurrentRow { get; set; } = 1;
		public float CurrentX { get; set; } = 0;
		public float CurrentY { get; set; } = 0;

		public Comic()
		{
		}

		public void Initialize(Document document, string imagesFolderPath)
		{
			this.document = document;
			this.ImagesFolderPath = imagesFolderPath;
			foreach (var child in this.children)
			{
				if (child is Slot slot)
				{
					slot.Initialize(this.document, this);
				}
				else if (child is NewPage newPage)
				{
					newPage.Initialize(this.document, this);
				}
			}
		}

		// IRenderable
		public void Render(LogWriter logWriter)
		{
			PageSize pageSize = this.document?.GetPdfDocument().GetDefaultPageSize() ?? PageSize.A4;
			var panelHeight = (pageSize.GetHeight() - this.marginTop - this.marginBottom - (this.rowsPerPage - 1) * this.VerticalPanelSpacing) / this.rowsPerPage;
			var rowWidth = pageSize.GetWidth() - this.marginRight - this.marginLeft;
			this.CurrentY = panelHeight + this.VerticalPanelSpacing;
			for (var i = 0; i < this.children.Count;)
			{
				// Handle newpage elements
				if (this.children[i] is NewPage newPage)
				{
					newPage.Render(logWriter);
					i++;
					continue;
				}

				var nbPanelsInRow = 0;
				// We find the number of cases that can fit in the row
				float minWidth = 0;
				float maxWidth = 0;
				for (; i + nbPanelsInRow < this.children.Count && minWidth < rowWidth; ++nbPanelsInRow)
				{
					if (!(this.children[i + nbPanelsInRow] is Slot))
						break;
					Slot slot = (Slot) this.children[i + nbPanelsInRow];
					slot.SetHeight(panelHeight);
					var minPanelWidth = slot.GetMinWidth();
					var maxPanelWidth = slot.GetMaxWidth();
					if (minWidth + minPanelWidth + (nbPanelsInRow >= 1 ? this.horizontalPanelSpacing : 0) > rowWidth)
						break;
					minWidth += minPanelWidth + (nbPanelsInRow >= 1 ? this.horizontalPanelSpacing : 0);
					maxWidth += maxPanelWidth + (nbPanelsInRow >= 1 ? this.horizontalPanelSpacing : 0);
				}

				// =============================

				float cropping = 0;
				var totalCropping = rowWidth - minWidth;
				var allowedCroppingPerPanel = totalCropping / nbPanelsInRow;

				List<Slot> slotsOnCurrentRow = new List<Slot>();
				for (var j = 0; j < nbPanelsInRow; ++j)
					slotsOnCurrentRow.Add((Slot) this.children[i + j]);

				// We try to equalize the sides of each image border
				foreach (Slot slot in slotsOnCurrentRow)
				{
					float leftCropping, rightCropping;
					var possibleLeftCropping = slot.Width * slot.MaxLeftPaddingPct / 100;
					var possibleRightCropping = slot.Width * slot.MaxRightPaddingPct / 100;

					if (possibleLeftCropping + possibleRightCropping <= allowedCroppingPerPanel)
					{
						var balancedCropping = Math.Min(possibleLeftCropping, possibleRightCropping);
						leftCropping = Math.Min(balancedCropping, allowedCroppingPerPanel / 2);
						rightCropping = Math.Min(balancedCropping, allowedCroppingPerPanel / 2);
						slot.PaddingLeft = leftCropping;
						slot.PaddingRight = rightCropping;

						cropping += leftCropping;
						cropping += rightCropping;
					}
				}

				// We add the padding to fill the row as evenly as possible
				List<Slot> sortedSlotsOnCurrentRow = slotsOnCurrentRow.OrderBy(e => e.MaxLeftPaddingPct + e.MaxRightPaddingPct).ToList();
				while (cropping < Math.Min(totalCropping, slotsOnCurrentRow.Sum(e => (e.MaxLeftPaddingPct + e.MaxRightPaddingPct) * e.Width / 100)))
				{
					foreach (Slot slot in sortedSlotsOnCurrentRow)
					{
						if (cropping < totalCropping && slot.PaddingLeft < (slot.MaxLeftPaddingPct * slot.Width / 100))
						{
							slot.PaddingLeft++;
							cropping++;
						}

						if (cropping < totalCropping && slot.PaddingRight < (slot.MaxRightPaddingPct * slot.Width / 100))
						{
							slot.PaddingRight++;
							cropping++;
						}
					}
				}

				// We proceed to the cropping and positioning of the image
				foreach (Slot slot in slotsOnCurrentRow)
				{
					slot.Crop();
					slot.SetPosition(
						this.CurrentPage,
						this.marginLeft + this.CurrentX,
						pageSize.GetHeight() - this.marginTop - this.CurrentY
					);
					slot.Render(logWriter);

					this.CurrentX += slot.Width + this.horizontalPanelSpacing;
				}
				this.CurrentX = 0;
				i += nbPanelsInRow;
				++this.CurrentRow;

				if (this.CurrentRow % this.rowsPerPage == 0)
				{
					this.document?.GetPdfDocument().AddNewPage();
					++this.CurrentPage;
					this.CurrentY = 0;
				}
				else
				{
					this.CurrentY += panelHeight + this.VerticalPanelSpacing;
				}
			}
		}
	}
}