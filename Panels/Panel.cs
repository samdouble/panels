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
        private Comic parent;
        private Image image;
        private List<Element> elements = new List<Element>();
        public PointF Position { get; private set; }
        public float Height {
            get {
                return this.image.Height;
            }
            set {
                this.image.Height = value;
            }
        }
        public float Width {
            get {
                return this.image.Width;
            }
        }

        public Panel(Comic parent, XmlNode xmlPanel, PanelOptions panelOptions = new PanelOptions())
        {
            this.parent = parent;
            if (xmlPanel.Attributes["image"] == null)
                throw new Exception("A panel must have an image attribute");

            string imageSrc = xmlPanel.Attributes["image"].InnerText;
            string imagesFolderPath = parent.GetImagesFolderPath();
            string fullImagePath = Path.Combine(imagesFolderPath, imageSrc);

            Console.WriteLine($"Getting image at {fullImagePath}");
            this.image = File.Exists(fullImagePath)
                ? new Image(fullImagePath)
                : new Image(Properties.Resources.temp);

            foreach (XmlNode xmlElement in xmlPanel.ChildNodes)
            {
                Element element = null;
                if (xmlElement.Name == "description") {
                    element = new Description(xmlElement, this);
                }
                else if (xmlElement.Name == "text") {
                    TextOptions textOptions = new TextOptions {
                        FontSize = panelOptions.FontSize
                    };
                    element = new Text(xmlElement, this, textOptions);
                }

                if (element != null)
                {
                    this.elements.Add(element);
                }
            }
        }

        public void Crop(Document doc, float leftCropping, float horizontalOffset, float decoupageHaut = 0, float verticalOffset = 0)
        {
            this.image.Crop(doc, leftCropping, horizontalOffset, decoupageHaut, verticalOffset);
            foreach (Element element in elements)
                element.Crop(doc, leftCropping, horizontalOffset);
        }

        // IPositionable
        public void SetPosition(int noPage, float x, float y)
        {
            this.Position = new PointF(x, y);
            this.image.SetPosition(noPage, x, y);
            this.elements.ForEach(element => element.SetPosition(noPage, x, y));
        }

        // IRenderable
        public void Render(Document doc, LogWriter logWriter)
        {
            this.image.Render(doc, logWriter);
            this.elements.ForEach(element => element.Render(doc, logWriter));
        }
    }
}
