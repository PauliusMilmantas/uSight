using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uSight_Web.Entities;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;

namespace uSight_Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    //string path = Path.Combine(Server.MapPath("~/Content/Uploaded_files"), Path.GetFileName(file.FileName));
                    string path = Path.Combine(Server.MapPath("~/Content/Uploaded_files"), "upload1.jpg");
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                    var imagePath = Server.MapPath(".") + "\\Content/Uploaded_files/upload1.jpg";

                    var currentImageSource = new ImageSource(new Image<Bgr, byte>(imagePath));
                    var image = currentImageSource[0].Bitmap;
                    var scaledImage = ScaleImage(image, 900, 700);
                    var img = UtilFunctions.GetMatFromImage(scaledImage);
                    UMat uImg = img.GetUMat(AccessType.ReadWrite);
                    UtilFunctions f = new UtilFunctions(Server.MapPath(".") + "\\tessdata");
                    List<String> strings = f.ProcessImage(uImg);
                    string foundLP = new FormData(strings).GetLicensePlate();
                    ViewBag.Message = foundLP;
                    //System.IO.File.Delete(imagePath);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR: " + ex.Message.ToString();
                }
        

            return View();
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

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}