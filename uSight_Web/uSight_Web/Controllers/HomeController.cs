using System;
using System.Web;
using System.Web.Mvc;
using uSight_Web.Entities;
using uSight_Web.Models;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;

namespace uSight_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string uSightLogo = "/Content/Images/default.jpg";

        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    ViewBag.LabelColor = "red";
                    string ext = file.FileName.Substring(file.FileName.Length - 4, 4).ToLower();

                    if (!ext.Equals(".jpg") && !ext.Equals(".png"))
                    {
                        ViewBag.Label = "Invalid file format.";
                        ViewBag.ImageData = uSightLogo;
                    }
                    else
                    {
                        ShowUploadedImage(file);

                        string foundLP = new RecognitionBuilder(file, Server.MapPath(".")).GetFoundLP();
                        string originalLP = foundLP;

                        foundLP = foundLP.Replace(" ", String.Empty);
                        bool confirmedLP = new Regex("[A-Z]{3}[0-9]{3}").IsMatch(foundLP);

                        if (!confirmedLP) throw new Exception("Sorry! License plate unrecognised.");
                        else
                        {
                            bool wanted = PoliceAPI.Instance.IsStolen(foundLP);
                            SetViewBagLabels(originalLP, wanted);
                            SaveUploadSearchRecord(foundLP, wanted);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Label = ex.Message.ToString();
                }
            else ViewBag.ImageData = uSightLogo;

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

        private void SaveUploadSearchRecord (string foundLP, bool wanted)
        {
            Search s = new Search();
            SearchRecord sr = new SearchRecord();
            sr.Time = DateTime.Now;
            sr.PlateNumber = foundLP;
            sr.Stolen = wanted;
            if (Request.IsAuthenticated) sr.UserId = User.Identity.GetUserId();
            else sr.UserId = null;
            s.SearchRecords.Add(sr);
            s.SaveChanges();
        }

        private void SetViewBagLabels (string originalLP, bool wanted)
        {
            ViewBag.LicensePlate = originalLP;
            ViewBag.LicensePlateColor = wanted ? "red" : "black";
            ViewBag.Label = wanted ? "Vehicle wanted!" : "Vehicle not wanted.";
            ViewBag.LabelColor = wanted ? "red" : "green";
        }
    }
}