using iText.Layout;
using Panels.Elements;
using Panels.Utils;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Panels
{
	public struct PanelOptions
	{
		public int FontSize { get; init; }
	}

	class Panel : IPositionable, IRenderable
	{
		private readonly Document document;
		private readonly Comic parent;
		private readonly Image image;
		private readonly List<Element> elements = new List<Element>();
		public PointF Position { get; private set; }
		public float CropBottom { get; private set; }
		public float CropTop { get; private set; }
		public float Height
		{
			get
			{
				return this.image.Height;
			}
			set
			{
				this.image.Height = value;
			}
		}
		public float Width
		{
			get
			{
				return this.image.Width;
			}
		}

		public Panel(Document document, Comic parent, XmlNode xmlPanel, PanelOptions panelOptions = new PanelOptions())
		{
			this.document = document;
			this.parent = parent;

			var cropBottom = xmlPanel.Attributes["cropBottom"] != null
				? float.Parse(xmlPanel.Attributes["cropBottom"].InnerText)
				: 0;
			var cropTop = xmlPanel.Attributes["cropTop"] != null
				? float.Parse(xmlPanel.Attributes["cropTop"].InnerText)
				: 0;

			var imageSrc = xmlPanel.Attributes["image"].InnerText;
			var fullImagePath = Path.Combine(parent.ImagesFolderPath, imageSrc);

			Console.WriteLine($"Getting image at {fullImagePath}");
			this.image = File.Exists(fullImagePath)
				? new Image(document, fullImagePath)
				: new Image(document, Properties.Resources.temp);
			this.image.CropBottom = cropBottom;
			this.image.CropTop = cropTop;

			foreach (XmlNode xmlElement in xmlPanel.ChildNodes)
			{
				Element element = null;
				TextOptions textOptions = new TextOptions
				{
					FontSize = panelOptions.FontSize
				};
				if (xmlElement.Name == "description")
				{
					element = new Description(this.document, xmlElement, this, textOptions);
				}
				else if (xmlElement.Name == "text")
				{
					element = new Text(this.document, xmlElement, this, textOptions);
				}
				this.elements.Add(element);
			}
		}

		public void Crop(
			float topCropping = 0,
			float rightCropping = 0,
			float bottomCropping = 0,
			float leftCropping = 0
		)
		{
			this.image.Crop(
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
			this.image.SetPosition(noPage, x, y);
			this.elements.ForEach(element => element.SetPosition(noPage, x, y));
		}

		// IRenderable
		public void Render(LogWriter logWriter)
		{
			this.image.Render(logWriter);
			this.elements.ForEach(element => element.Render(logWriter));
		}
	}
}