using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using Panels.Utils;
using System;

namespace Panels
{
    public class Image : IPositionable, IRenderable
    {
        private iText.Layout.Element.Image image;
        protected int noPage;
        protected float x;
        protected float y;
        public float Height {
            get {
                return this.image.GetImageScaledHeight();
            }
            set {
                float pctScaling = value / this.image.GetImageHeight();
                this.image.Scale(pctScaling, pctScaling);
            }
        }
        public float Width {
            get {
                return this.image.GetImageScaledWidth();
            }
        }

        public Image(string src)
        {
            this.image = new iText.Layout.Element.Image(ImageDataFactory.Create(src));
        }

        public Image(byte[] bytes)
        {
            this.image = new iText.Layout.Element.Image(ImageDataFactory.Create(bytes));
        }

        public void Crop(Document doc, float leftCropping, float horizontalOffset, float decoupageHaut = 0, float verticalOffset = 0)
        {
            this.image.SetFixedPosition(-leftCropping, -decoupageHaut);
            Rectangle rectangle = new Rectangle(this.image.GetImageScaledWidth() - horizontalOffset, this.image.GetImageScaledHeight() - verticalOffset);
            PdfFormXObject template = new PdfFormXObject(rectangle);
            Canvas canvas = new Canvas(template, doc.GetPdfDocument());
            canvas.Add(this.image);
            this.image = new iText.Layout.Element.Image(template);
        }

        // IPositionable
        public void SetPosition(int noPage, float x, float y)
        {
            this.noPage = noPage;
            this.x = x;
            this.y = y;
            this.image.SetFixedPosition(this.noPage, this.x, this.y - this.image.GetImageScaledHeight());
        }

        // IRenderable
        public void Render(Document doc, LogWriter logWriter)
        {
            doc.Add(this.image);

            // Add borders
            iText.Kernel.Pdf.Canvas.PdfCanvas canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(doc.GetPdfDocument().GetPage(this.noPage));
            canvas.SetStrokeColor(ColorConstants.BLACK);
            canvas.SetLineWidth(2f);
            canvas.Rectangle(this.x, this.y - image.GetImageScaledHeight(), image.GetImageScaledWidth(), image.GetImageScaledHeight());
            canvas.Stroke();
            logWriter.Log($"IMAGE - {this.image.GetImageScaledWidth()}x{this.image.GetImageScaledHeight()} at {this.x}, {this.y}");
        }
    }
}
