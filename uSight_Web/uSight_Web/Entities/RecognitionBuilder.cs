using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using System.Web;

namespace uSight_Web.Entities
{
    public class RecognitionBuilder
    {
        private readonly string foundLP;

        public string GetFoundLP () { return foundLP; }

        public RecognitionBuilder (HttpPostedFileBase file, string serverMapPath)            // Constructor
        {
            var currentImageSource = new ImageSource(new Image<Bgr, byte>(new Bitmap(Image.FromStream(file.InputStream))));
            var image = currentImageSource[0].Bitmap;
            var scaledImage = ScaleImage(image, 900, 700);
            var img = UtilFunctions.GetMatFromImage(scaledImage);
            UMat uImg = img.GetUMat(AccessType.ReadWrite);
            UtilFunctions f = new UtilFunctions( new LicenesePlateDetector(serverMapPath + "\\tessdata"));
            List<String> strings = f.ProcessImage(uImg);
            foundLP = new FormData(strings).GetLicensePlate();
        }

        private Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
    }
}