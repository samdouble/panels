using iText.Kernel.Colors;
using iText.Layout;
using Newtonsoft.Json;
using Panels.Elements;
using Panels.Utils;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Panels
{
	[XmlType("panel")]
	[JsonObject]
	public class Panel : IPositionable, IRenderable
	{
		[XmlIgnore]
		[JsonIgnore]
		private Document? document;

		[XmlIgnore]
		[JsonIgnore]
		private Slot? parent;

		[XmlIgnore]
		[JsonIgnore]
		private float? fontSize;

		[XmlIgnore]
		[JsonIgnore]
		private Image? image;

		[XmlElement("description", Type = typeof(Description))]
		[XmlElement("text", Type = typeof(Text))]
		[JsonProperty("elements")]
		public List<Element> elements = new List<Element>();

		[XmlIgnore]
		[JsonIgnore]
		public PointF Position { get; private set; }

		[XmlAttribute("borders")]
		[JsonProperty("borders")]
		public string? Borders { get; set; }

		[XmlAttribute("bordersColor")]
		[JsonProperty("bordersColor")]
		public string? BordersColor { get; set; }

		[XmlAttribute("bordersWidth")]
		[JsonProperty("bordersWidth")]
		public string? BordersWidth { get; set; }

		[XmlAttribute("fontSize")]
		[JsonProperty("fontSize")]
		public float FontSize
		{
			get { return fontSize ?? parent?.FontSize ?? 12f; }
			set { fontSize = value; }
		}

		[XmlAttribute("image")]
		[JsonProperty("image")]
		public string? ImagePath { get; set; }

		[XmlAttribute("paddingBottom")]
		[JsonProperty("paddingBottom")]
		public float PaddingBottom { get; set; }

		[XmlAttribute("paddingTop")]
		[JsonProperty("paddingTop")]
		public float PaddingTop { get; set; }

		[XmlIgnore]
		[JsonIgnore]
		public float Height
		{
			get
			{
				return this.image?.Height ?? 0f;
			}
			set
			{
				this.image?.Height = value;
			}
		}

		[XmlIgnore]
		[JsonIgnore]
		public float Width
		{
			get
			{
				return this.image?.Width ?? 0f;
			}
		}

		public Panel()
		{
			this.PaddingBottom = 0;
			this.PaddingTop = 0;
		}

		public void Initialize(Document document, Slot parent)
		{
			this.document = document;
			this.parent = parent;
			if (!string.IsNullOrEmpty(this.ImagePath) && !string.IsNullOrEmpty(parent.ImagesFolderPath))
			{
				var fullImagePath = Path.Combine(parent.ImagesFolderPath, this.ImagePath);
				Console.WriteLine($"Getting image at {fullImagePath}");
				this.image = File.Exists(fullImagePath)
					? new Image(document, fullImagePath)
					: new Image(document, Properties.Resources.temp);
			}
			else
			{
				this.image = new Image(document, Properties.Resources.temp);
			}
			this.image.PaddingBottom = this.PaddingBottom;
			this.image.PaddingTop = this.PaddingTop;
			this.image.ShowBorders = !string.Equals(this.Borders, "none", StringComparison.OrdinalIgnoreCase);
			this.image.BorderColor = ParseBorderColor(this.BordersColor ?? parent.Parent?.BordersColor) ?? ColorConstants.BLACK;
			this.image.BordersWidth = ParseBordersWidth(this.BordersWidth) ?? ParseBordersWidth(parent.Parent?.BordersWidth) ?? 2f;

			foreach (var element in this.elements)
			{
				if (element is Description description)
				{
					description.Initialize(document, this);
				}
				else if (element is Text text)
				{
					text.Initialize(document, this);
				}
			}
		}

		private static iText.Kernel.Colors.Color? ParseBorderColor(string? hex)
		{
			if (string.IsNullOrWhiteSpace(hex))
			{
				return null;
			}
			var s = hex.TrimStart('#');
			if (s.Length != 6)
			{
				return null;
			}
			if (!int.TryParse(s.AsSpan(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var r) ||
				!int.TryParse(s.AsSpan(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var g) ||
				!int.TryParse(s.AsSpan(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var b))
			{
				return null;
			}
			return new DeviceRgb(r / 255f, g / 255f, b / 255f);
		}

		private static float? ParseBordersWidth(string? value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			return float.TryParse(value.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var w) ? w : null;
		}

		public void Crop(
			float topCropping = 0,
			float rightCropping = 0,
			float bottomCropping = 0,
			float leftCropping = 0
		)
		{
			this.image?.Crop(
				topCropping,
				rightCropping,
				bottomCropping,
				leftCropping
			);
		}

		// IPositionable
		public void SetPosition(int noPage, float x, float y)
		{
			this.Position = new PointF(x, y);
			this.image?.SetPosition(noPage, x, y);
			this.elements.ForEach(element => element.SetPosition(noPage, x, y));
		}

		// IRenderable
		public void Render(LogWriter logWriter)
		{
			this.image?.Render(logWriter);
			this.elements.ForEach(element => element.Render(logWriter));
		}
	}
}