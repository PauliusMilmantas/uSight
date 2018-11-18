using System;
using System.Web;
using System.Web.Mvc;
using uSight_Web.Entities;
using uSight_Web.Models;
using Microsoft.AspNet.Identity;

namespace uSight_Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    ShowUploadedImage(file);

                    string foundLP = new RecognitionBuilder(file, Server.MapPath(".")).GetFoundLP();
                    ViewBag.Message = foundLP;

                    foundLP = foundLP.Replace(" ", String.Empty);
                    bool wanted = new PoliceAPI().IsStolen(foundLP);

                    Search s = new Search();
                    SearchRecord sr = new SearchRecord();
                    sr.Time = DateTime.Now;
                    sr.PlateNumber = foundLP;
                    sr.Stolen = wanted;
                    if (Request.IsAuthenticated) sr.UserId = User.Identity.GetUserId();
                    else sr.UserId = null;
                    s.SearchRecords.Add(sr);
                    s.SaveChanges();



                    //return View(t.SearchRecords.ToList());
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