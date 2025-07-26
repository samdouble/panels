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
        private Document document;
        private iText.Layout.Element.Image image;
        protected int noPage;
        protected float x;
        protected float y;
        private float originalHeight;
        private float originalWidth;

        public float CropBottom {
            get {
                return 0;
            }
            set {
                this.Crop(0, 0, value * this.originalHeight / 100, 0);
            }
        }
        public float CropTop {
            get {
                return 0;
            }
            set {
                this.Crop(value * this.originalHeight / 100, 0, 0, 0);
            }
        }
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

        public Image(Document document, string src)
        {
            this.document = document;
            this.image = new iText.Layout.Element.Image(ImageDataFactory.Create(src));
            this.originalHeight = this.image.GetImageHeight();
            this.originalWidth = this.image.GetImageWidth();
        }

        public Image(Document document, byte[] bytes)
        {
            this.document = document;
            this.image = new iText.Layout.Element.Image(ImageDataFactory.Create(bytes));
        }

        public void Crop(
            float topCropping = 0,
            float rightCropping = 0,
            float bottomCropping = 0,
            float leftCropping = 0
        )
        {
            float horizontalOffset = leftCropping + rightCropping;
            float verticalOffset = topCropping + bottomCropping;
            this.image.SetFixedPosition(-leftCropping, -bottomCropping);
            Rectangle rectangle = new Rectangle(
                this.image.GetImageScaledWidth() - horizontalOffset,
                this.image.GetImageScaledHeight() - verticalOffset
            );
            PdfFormXObject template = new PdfFormXObject(rectangle);
            Canvas canvas = new Canvas(template, this.document.GetPdfDocument());
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
        public void Render(LogWriter logWriter)
        {
            this.document.Add(this.image);

            // Add borders
            iText.Kernel.Pdf.Canvas.PdfCanvas canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(this.document.GetPdfDocument().GetPage(this.noPage));
            canvas.SetStrokeColor(ColorConstants.BLACK);
            canvas.SetLineWidth(2f);
            canvas.Rectangle(this.x, this.y - image.GetImageScaledHeight(), image.GetImageScaledWidth(), image.GetImageScaledHeight());
            canvas.Stroke();
            logWriter.Log($"IMAGE - {this.image.GetImageScaledWidth()}x{this.image.GetImageScaledHeight()} at {this.x}, {this.y}");
        }
    }
}
