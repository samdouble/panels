using iText.Layout;
using Panels.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Panels
{
    public struct SlotOptions
    {
        public int FontSize { get; init; }
    }

    class Slot : IRenderable
    {
        private Document document;
        private Comic parent;
        private List<Panel> panels = new List<Panel>();
        public float MaxLeftPaddingPct { get; set; }
        public float MaxRightPaddingPct { get; set; }
        public float PaddingLeft { get; set; }
        public float PaddingRight { get; set; }
        public float Height { get; set; }
        public float Width {
            get {
                return panels.Select(panel => panel.Width).Max();
            }
        }

        public Slot(Document document, Comic parent, XmlNode xmlSlot, SlotOptions slotOptions = new SlotOptions())
        {
            this.document = document;
            this.parent = parent;
            this.MaxLeftPaddingPct = xmlSlot?.Attributes["maxCropLeft"] != null ? float.Parse(xmlSlot.Attributes["maxCropLeft"].InnerText) : 0f;
            this.MaxRightPaddingPct = xmlSlot?.Attributes["maxCropRight"] != null ? float.Parse(xmlSlot.Attributes["maxCropRight"].InnerText) : 0f;
            this.PaddingLeft = 0f;
            this.PaddingRight = 0f;
            
            PanelOptions panelOptions = new PanelOptions {
                FontSize = slotOptions.FontSize
            };
            List<XmlNode> xmlPanels = new List<XmlNode>(xmlSlot.ChildNodes.Cast<XmlNode>());
            this.panels.AddRange(xmlPanels.Select(xmlPanel => new Panel(document, parent, xmlPanel, panelOptions)));
        }

        public void SetHeight(float height)
        {
            this.Height = height;
            int nbPanelsInSlot = this.panels.Count;
            float panelHeight = (height - (nbPanelsInSlot - 1) * parent.VerticalPanelSpacing) / nbPanelsInSlot;
            this.panels.ForEach(panel => {
                panel.Height = panelHeight;
            });
        }

        public float GetMinWidth()
        {
            float minPctAvailable = 1 - ((this.MaxLeftPaddingPct + this.MaxRightPaddingPct) / 100);
            return minPctAvailable * this.Width;
        }

        public float GetMaxWidth()
        {
            return panels.Select(panel => panel.Width).Max();
        }

        public void Crop()
        {
            int nbPanelsInSlot = this.panels.Count;
            float leftCropping = (this.MaxLeftPaddingPct * this.Width / 100) - this.PaddingLeft;
            float rightCropping = (this.MaxRightPaddingPct * this.Width / 100) - this.PaddingRight;
            for (int i = 0; i < nbPanelsInSlot; i++)
            {
                Panel panel = this.panels[i];
                panel.Crop(0, rightCropping, 0, leftCropping);
            }
        }

        // IPositionable
        public void SetPosition(int noPage, float x, float y)
        {
            int nbPanelsInSlot = this.panels.Count;
            float panelHeight =
                (this.Height - (nbPanelsInSlot - 1) * parent.VerticalPanelSpacing) / nbPanelsInSlot;
            for (int i = 0; i < nbPanelsInSlot; i++)
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
