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
        private List<object> children = new List<object>();
        protected const int DEFAULT_FONT_SIZE = 12;
        protected const int DEFAULT_ROWS_PER_PAGE = 3;
        private int fontSize;
        private float marginLeft;
        private float marginRight;
        private float marginTop;
        private float marginBottom;
        private float horizontalPanelSpacing;
        private float verticalPanelSpacing;
        private float rowsPerPage;
        private string imagesFolderPath;

        public Comic(string configFile, string imagesFolderPath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.WriteLine($"Reading config file at {configFile}");
            XmlNode xmlComic = XmlParser.Read(configFile);
            this.imagesFolderPath = imagesFolderPath;
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
            this.horizontalPanelSpacing = xmlComic?.Attributes["horizontalPanelSpacing"] != null
                ? float.Parse(xmlComic.Attributes["horizontalPanelSpacing"].InnerText)
                : 0;
            this.verticalPanelSpacing = xmlComic?.Attributes["verticalPanelSpacing"] != null
                ? float.Parse(xmlComic.Attributes["verticalPanelSpacing"].InnerText)
                : 0;
            this.rowsPerPage = xmlComic?.Attributes["rowsPerPage"] != null
                ? float.Parse(xmlComic.Attributes["rowsPerPage"].InnerText)
                : DEFAULT_ROWS_PER_PAGE;

            List<XmlNode> xmlNodes = new List<XmlNode>(xmlComic.ChildNodes.Cast<XmlNode>());
            SlotOptions slotOptions = new SlotOptions {
                FontSize = this.fontSize
            };
            foreach (XmlNode xmlNode in xmlNodes) {
                if (xmlNode.Name == "newpage") {
                    this.children.Add(new NewPage(this, xmlNode));
                }
                if (xmlNode.Name == "slot") {
                    this.children.Add(new Slot(this, xmlNode, slotOptions));
                }
            }
        }

        public string GetImagesFolderPath()
        {
            return this.imagesFolderPath;
        }

        public float GetVerticalPanelSpacing()
        {
            return this.verticalPanelSpacing;
        }

        // IRenderable
        public void Render(Document doc, LogWriter logWriter)
        {
            PageSize pageSize = doc.GetPdfDocument().GetDefaultPageSize();
            float panelHeight = (pageSize.GetHeight() - this.marginTop - this.marginBottom - (this.rowsPerPage - 1) * this.verticalPanelSpacing) / this.rowsPerPage;
            float rowWidth = pageSize.GetWidth() - this.marginRight - this.marginLeft;
            int page = 1;
            float x = 0;
            float y = panelHeight + this.verticalPanelSpacing;
            float rowNo = 1;
            for (int i = 0; i < this.children.Count;)
            {
                // Handle newpage elements
                if (this.children[i].GetType() == typeof(NewPage)) {
                    if (x != 0 || y != 0) {
                        doc.GetPdfDocument().AddNewPage();
                        logWriter.Log("NEW PAGE");
                        page++;
                        rowNo = page * this.rowsPerPage;
                        x = 0;
                        y = 0;
                    }
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
                    slot.Crop(doc);
                    slot.SetPosition(doc, page, this.marginLeft + x, pageSize.GetHeight() - this.marginTop - y);
                    slot.Render(doc, logWriter);

                    x += slot.Width + this.horizontalPanelSpacing;
                }
                x = 0;
                i += nbPanelsInRow;
                ++rowNo;

                if (rowNo % this.rowsPerPage == 0)
                {
                    doc.GetPdfDocument().AddNewPage();
                    page++;
                    y = 0;
                }
                else
                {
                    y += panelHeight + this.verticalPanelSpacing;
                }
            }
        }
    }
}