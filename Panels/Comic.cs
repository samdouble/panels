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
        private float leftMargin;
        private float rightMargin;
        private float topMargin;
        private float bottomMargin;
        private float horizontalPanelSpacing;
        private float verticalPanelSpacing;
        private float rowsPerPage;
        private string imagesFolderPath;

        public Comic(string configFile, string imagesFolderPath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.WriteLine("Reading config file at " + @"" + configFile);
            this.imagesFolderPath = imagesFolderPath;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(@"" + configFile);
            XmlNode xmlComic = xmlDocument.DocumentElement;
            this.fontSize = xmlComic?.Attributes["fontSize"] != null
                ? int.Parse(xmlComic.Attributes["fontSize"].InnerText)
                : DEFAULT_FONT_SIZE;
            this.leftMargin = float.Parse(xmlComic?.Attributes["leftMargin"].InnerText);
            this.rightMargin = float.Parse(xmlComic?.Attributes["rightMargin"].InnerText);
            this.topMargin = float.Parse(xmlComic?.Attributes["topMargin"].InnerText);
            this.bottomMargin = float.Parse(xmlComic?.Attributes["bottomMargin"].InnerText);
            this.horizontalPanelSpacing = float.Parse(xmlComic?.Attributes["horizontalPanelSpacing"].InnerText);
            this.verticalPanelSpacing = float.Parse(xmlComic?.Attributes["verticalPanelSpacing"].InnerText);
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

        public float getVerticalPanelSpacing()
        {
            return this.verticalPanelSpacing;
        }

        // IRenderable
        public void Render(Document doc, LogWriter logWriter)
        {
            PageSize pageSize = doc.GetPdfDocument().GetDefaultPageSize();
            float panelHeight = (pageSize.GetHeight() - this.topMargin - this.bottomMargin - (this.rowsPerPage - 1) * this.verticalPanelSpacing) / this.rowsPerPage;
            float rowWidth = pageSize.GetWidth() - this.rightMargin - this.leftMargin;
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
                    float possibleLeftCropping = slot.GetWidth() * slot.maxLeftPaddingPct / 100;
                    float possibleRightCropping = slot.GetWidth() * slot.maxRightPaddingPct / 100;

                    if (possibleLeftCropping + possibleRightCropping <= allowedCroppingPerPanel)
                    {
                        float balancedCropping = Math.Min(possibleLeftCropping, possibleRightCropping);
                        leftCropping = Math.Min(balancedCropping, allowedCroppingPerPanel / 2);
                        rightCropping = Math.Min(balancedCropping, allowedCroppingPerPanel / 2);
                        slot.leftPadding = leftCropping;
                        slot.rightPadding = rightCropping;

                        cropping += leftCropping;
                        cropping += rightCropping;
                    }
                }

                // We add the padding to fill the row as evenly as possible
                List<Slot> sortedSlotsOnCurrentRow = slotsOnCurrentRow.OrderBy(e => e.maxLeftPaddingPct + e.maxRightPaddingPct).ToList();
                while (cropping < Math.Min(totalCropping, slotsOnCurrentRow.Sum(e => (e.maxLeftPaddingPct + e.maxRightPaddingPct) * e.GetWidth() / 100)))
                {
                    foreach (Slot slot in sortedSlotsOnCurrentRow)
                    {
                        if (cropping < totalCropping && slot.leftPadding < (slot.maxLeftPaddingPct * slot.GetWidth() / 100))
                        {
                            slot.leftPadding++;
                            cropping++;
                        }

                        if (cropping < totalCropping && slot.rightPadding < (slot.maxRightPaddingPct * slot.GetWidth() / 100))
                        {
                            slot.rightPadding++;
                            cropping++;
                        }
                    }
                }

                // We proceed to the cropping and positioning of the image
                foreach (Slot slot in slotsOnCurrentRow)
                {
                    slot.Crop(doc);
                    slot.SetPosition(doc, page, this.leftMargin + x, pageSize.GetHeight() - this.topMargin - y);
                    slot.Render(doc, logWriter);

                    x += slot.GetWidth() + this.horizontalPanelSpacing;
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