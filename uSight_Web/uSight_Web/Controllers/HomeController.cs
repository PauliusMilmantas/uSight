using System;
using System.Web;
using System.Web.Mvc;
using uSight_Web.Entities;

namespace uSight_Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(HttpPostedFileBase file)
        {
<<<<<<< HEAD
            //string path = Path.Combine(Server.MapPath("~/Content/Uploaded_files"), Path.GetFileName(file.FileName));
            string path = Path.Combine(Server.MapPath("~/Content/Uploaded_files"), "u1.jpg");
            try
            {
                System.IO.File.Delete(path);
            }
            catch (Exception)
            {

            }

=======
>>>>>>> WebDev_AK
            if (file != null && file.ContentLength > 0)
                try
                {
                    ShowUploadedImage(file);
                    ViewBag.Message = new RecognitionBuilder(file, Server.MapPath(".")).GetFoundLP();
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR: " + ex.Message.ToString();
                }
            else ViewBag.ImageData = "/Content/Images/default.jpg";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        private void ShowUploadedImage(HttpPostedFileBase file)
        {
            byte[] image = new byte[file.ContentLength];
            file.InputStream.Read(image, 0, image.Length);
            string imreBase64Data = Convert.ToBase64String(image);
            string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
            ViewBag.ImageData = imgDataURL;
        }
    }
}