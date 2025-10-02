using iText.Layout;
using Panels.Elements;
using Panels.Utils;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Panels
{
	[XmlType("panel")]
	public class Panel : IPositionable, IRenderable
	{
		[XmlIgnore]
		private Document document;

		[XmlIgnore]
		private Comic parent;

		[XmlIgnore]
		private Image image;

		[XmlAttribute("image")]
		public string ImagePath { get; set; }

		[XmlElement("description", Type = typeof(Description))]
		[XmlElement("text", Type = typeof(Text))]
		public List<Element> elements = new List<Element>();
		
		[XmlIgnore]
		public PointF Position { get; private set; }
		
		[XmlAttribute("cropBottom")]
		public float CropBottom { get; set; }
		
		[XmlAttribute("cropTop")]
		public float CropTop { get; set; }
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

		public Panel()
		{
			this.CropBottom = 0;
			this.CropTop = 0;
		}

		public void Initialize(Document document, Comic parent)
		{
			this.document = document;
			this.parent = parent;
			if (!string.IsNullOrEmpty(this.ImagePath))
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
			this.image.CropBottom = this.CropBottom;
			this.image.CropTop = this.CropTop;

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