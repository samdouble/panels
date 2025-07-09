using iText.Layout;
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
        private Comic parent;
        private List<Panel> panels = new List<Panel>();
        public float maxLeftPaddingPct { get; set; }
        public float maxRightPaddingPct { get; set; }
        public float leftPadding { get; set; }
        public float rightPadding { get; set; }
        public float height { get; set; }

        public Slot(Comic parent, XmlNode xmlSlot, SlotOptions slotOptions = new SlotOptions())
        {
            this.parent = parent;
            this.maxLeftPaddingPct = xmlSlot?.Attributes["maxCropLeft"] != null ? float.Parse(xmlSlot.Attributes["maxCropLeft"].InnerText) : 0f;
            this.maxRightPaddingPct = xmlSlot?.Attributes["maxCropRight"] != null ? float.Parse(xmlSlot.Attributes["maxCropRight"].InnerText) : 0f;
            this.leftPadding = 0f;
            this.rightPadding = 0f;
            
            PanelOptions panelOptions = new PanelOptions {
                FontSize = slotOptions.FontSize
            };
            List<XmlNode> xmlPanels = new List<XmlNode>(xmlSlot.ChildNodes.Cast<XmlNode>());
            this.panels.AddRange(xmlPanels.Select(xmlPanel => new Panel(parent, xmlPanel, panelOptions)));
        }

        public void SetHeight(float height)
        {
            this.height = height;
            int nbPanelsInSlot = this.panels.Count;
            float panelHeight = (height - (nbPanelsInSlot - 1) * parent.getVerticalPanelSpacing()) / nbPanelsInSlot;
            this.panels.ForEach(panel => panel.SetHeight(panelHeight));
        }

        public float GetWidth()
        {
            return panels[0].GetWidth();
        }

        public float GetMinWidth()
        {
            float minPctAvailable = 1 - ((this.maxLeftPaddingPct + this.maxRightPaddingPct) / 100);
            return minPctAvailable * this.GetWidth();
        }

        public float GetMaxWidth()
        {
            return panels[0].GetWidth();
        }

        public float GetHeight()
        {
            return this.height;
        }

        public void Crop(Document doc)
        {
            int nbPanelsInSlot = this.panels.Count;
            float leftCropping = (this.maxLeftPaddingPct * this.GetWidth() / 100) - this.leftPadding;
            float rightCropping = (this.maxRightPaddingPct * this.GetWidth() / 100) - this.rightPadding;
            float horizontalOffset = leftCropping + rightCropping;
            for (int i = 0; i < nbPanelsInSlot; i++)
            {
                Panel panel = this.panels[i];
                panel.Crop(doc, leftCropping, horizontalOffset);
            }
        }

        // IPositionable
        public void SetPosition(Document doc, int noPage, float x, float y)
        {
            int nbPanelsInSlot = this.panels.Count;
            float panelHeight =
                (this.height - (nbPanelsInSlot - 1) * parent.getVerticalPanelSpacing()) / nbPanelsInSlot;
            for (int i = 0; i < nbPanelsInSlot; i++)
            {
                Panel panel = this.panels[i];
                panel.Crop(doc, 0, 0, 0, 0);
                panel.SetPosition(noPage, x, y - i * panelHeight - (i - 1) * parent.getVerticalPanelSpacing());
            }
        }

        // IRenderable
        public void Render(Document doc)
        {
            this.panels.ForEach(panel => panel.Render(doc));
        }
    }
}
