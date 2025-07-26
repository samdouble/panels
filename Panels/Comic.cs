using iText.Kernel.Geom;
using iText.Layout;
using Panels.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Panels
{
    class Comic : IRenderable
    {
        private Document document;
        private List<IRenderable> children = new List<IRenderable>();
        protected const int DEFAULT_FONT_SIZE = 12;
        protected const int DEFAULT_ROWS_PER_PAGE = 3;
        private int fontSize;
        private float marginLeft;
        private float marginRight;
        private float marginTop;
        private float marginBottom;
        public int RowsPerPage { get; private set; }
        private float horizontalPanelSpacing;
        public float VerticalPanelSpacing { get; private set; }
        public string ImagesFolderPath { get; private set; }
        public int CurrentPage { get; set; } = 1;
        public int CurrentRow { get; set; } = 1;
        public float CurrentX { get; set; } = 0;
        public float CurrentY { get; set; } = 0;

        public Comic(Document document, string configFile, string imagesFolderPath)
        {
            this.document = document;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.WriteLine($"Reading config file at {configFile}");
            XmlNode xmlComic = XmlParser.Read(configFile);
            this.ImagesFolderPath = imagesFolderPath;
            this.fontSize = xmlComic?.Attributes["fontSize"] != null
                ? int.Parse(xmlComic.Attributes["fontSize"].InnerText)
                : DEFAULT_FONT_SIZE;
            this.marginLeft = xmlComic?.Attributes["marginLeft"] != null
                ? float.Parse(xmlComic.Attributes["marginLeft"].InnerText)
                : 0;
            this.marginRight = xmlComic?.Attributes["marginRight"] != null
                ? float.Parse(xmlComic.Attributes["marginRight"].InnerText)
                : 0;
            this.marginTop = xmlComic?.Attributes["marginTop"] != null
                ? float.Parse(xmlComic.Attributes["marginTop"].InnerText)
                : 0;
            this.marginBottom = xmlComic?.Attributes["marginBottom"] != null
                ? float.Parse(xmlComic.Attributes["marginBottom"].InnerText)
                : 0;
            this.RowsPerPage = xmlComic?.Attributes["rowsPerPage"] != null
                ? int.Parse(xmlComic.Attributes["rowsPerPage"].InnerText)
                : DEFAULT_ROWS_PER_PAGE;
            this.horizontalPanelSpacing = xmlComic?.Attributes["horizontalPanelSpacing"] != null
                ? float.Parse(xmlComic.Attributes["horizontalPanelSpacing"].InnerText)
                : 0;
            this.VerticalPanelSpacing = xmlComic?.Attributes["verticalPanelSpacing"] != null
                ? float.Parse(xmlComic.Attributes["verticalPanelSpacing"].InnerText)
                : 0;

            List<XmlNode> xmlNodes = new List<XmlNode>(xmlComic.ChildNodes.Cast<XmlNode>());
            SlotOptions slotOptions = new SlotOptions {
                FontSize = this.fontSize
            };
            foreach (XmlNode xmlNode in xmlNodes) {
                if (xmlNode.Name == "newpage") {
                    this.children.Add(new NewPage(document, this, xmlNode));
                }
                if (xmlNode.Name == "slot") {
                    this.children.Add(new Slot(document, this, xmlNode, slotOptions));
                }
            }
        }

        // IRenderable
        public void Render(LogWriter logWriter)
        {
            PageSize pageSize = this.document.GetPdfDocument().GetDefaultPageSize();
            float panelHeight = (pageSize.GetHeight() - this.marginTop - this.marginBottom - (this.RowsPerPage - 1) * this.VerticalPanelSpacing) / this.RowsPerPage;
            float rowWidth = pageSize.GetWidth() - this.marginRight - this.marginLeft;
            this.CurrentY = panelHeight + this.VerticalPanelSpacing;
            for (int i = 0; i < this.children.Count;)
            {
                // Handle newpage elements
                if (this.children[i].GetType() == typeof(NewPage)) {
                    this.children[i].Render(logWriter);
                    i++;
                    continue;
                }

                int nbPanelsInRow = 0;
                // We find the number of cases that can fit in the row
                float minWidth = 0;
                float maxWidth = 0;
                for (; i + nbPanelsInRow < this.children.Count && minWidth < rowWidth; ++nbPanelsInRow)
                {
                    if (this.children[i + nbPanelsInRow].GetType() != typeof(Slot))
                        break;
                    Slot slot = (Slot) this.children[i + nbPanelsInRow];
                    slot.SetHeight(panelHeight);
                    float minPanelWidth = slot.GetMinWidth();
                    float maxPanelWidth = slot.GetMaxWidth();
                    if (minWidth + minPanelWidth + (nbPanelsInRow >= 1 ? this.horizontalPanelSpacing : 0) > rowWidth)
                        break;
                    minWidth += minPanelWidth + (nbPanelsInRow >= 1 ? this.horizontalPanelSpacing : 0);
                    maxWidth += maxPanelWidth + (nbPanelsInRow >= 1 ? this.horizontalPanelSpacing : 0);
                }

                // =============================

                float cropping = 0;
                float totalCropping = rowWidth - minWidth;
                float allowedCroppingPerPanel = totalCropping / nbPanelsInRow;

                List<Slot> slotsOnCurrentRow = new List<Slot>();
                for (int j = 0; j < nbPanelsInRow; ++j)
                    slotsOnCurrentRow.Add((Slot) this.children[i + j]);

                // We try to equalize the sides of each image border
                foreach (Slot slot in slotsOnCurrentRow)
                {
                    float leftCropping, rightCropping;
                    float possibleLeftCropping = slot.Width * slot.MaxLeftPaddingPct / 100;
                    float possibleRightCropping = slot.Width * slot.MaxRightPaddingPct / 100;

                    if (possibleLeftCropping + possibleRightCropping <= allowedCroppingPerPanel)
                    {
                        float balancedCropping = Math.Min(possibleLeftCropping, possibleRightCropping);
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

                if (this.CurrentRow % this.RowsPerPage == 0)
                {
                    this.document.GetPdfDocument().AddNewPage();
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