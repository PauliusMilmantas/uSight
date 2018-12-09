using System;
using System.Web;
using System.Web.Mvc;
using uSight_Web.Models;
using uSight_Web.Entities;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;
using System.Device.Location;
using System.Linq;

namespace uSight_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string uSightLogo = "/Content/Images/default.jpg";

        public ActionResult Index(HttpPostedFileBase file, string text)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string ext = file.FileName.Substring(file.FileName.Length - 4, 4).ToLower();

                    if (!ext.Equals(".jpg") && !ext.Equals(".png"))
                    {
                        ViewBag.ImageData = uSightLogo;
                        throw new Exception("Invalid file format.");
                    }
                    else
                    {
                        ShowUploadedImage(file);

                        string foundLP = new RecognitionBuilder(file, Server.MapPath(".")).GetFoundLP();

                        string originalLP = foundLP;
                        foundLP = foundLP.Replace(" ", String.Empty);
                        bool confirmedLP = new Regex("[A-Z]{3}[0-9]{3}").IsMatch(foundLP);

                        if (!confirmedLP || foundLP.Length != 6) SetViewBagLabels();
                        else
                        {
                            bool wanted = PoliceAPI.Instance.IsStolen(foundLP);
                            SetViewBagLabels(originalLP, wanted);
                            SaveUploadSearchRecord(foundLP.ToUpper(), wanted, true);
                            if (Request.IsAuthenticated) RefreshImageSearchAchievements();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message.ToString();
                    ViewBag.ImageData = uSightLogo;
                }
            else if (text != null && text.Length > 0)
                try
                {
                    bool confirmedLP = new Regex("[A-Z]{3} *[0-9]{3}").IsMatch(text.ToUpper());
                    string foundLP = text.Replace(" ", String.Empty);

                    if (!confirmedLP || foundLP.Length != 6)
                    {
                        ViewBag.ImageData = uSightLogo;
                        throw new Exception("Invalid text format.");
                    }
                    else
                    {
                        string viewLP = (foundLP.Substring(0,3) + " " + foundLP.Substring(3, 3)).ToUpper();
                        bool wanted = PoliceAPI.Instance.IsStolen(foundLP);
                        SetViewBagLabels(viewLP, wanted);
                        ViewBag.ImageData = "";
                        SaveUploadSearchRecord(foundLP.ToUpper(), wanted, false);
                        if (Request.IsAuthenticated) RefreshTextSearchAchievements();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage2 = ex.Message.ToString();
                    ViewBag.ImageData = uSightLogo;
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


        static String GetLocationProperty()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

            // Do not suppress prompt, and wait 1000 milliseconds to start.
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));

            GeoCoordinate coord = watcher.Position.Location;

            if (coord.IsUnknown != true)
            {
                return coord.Latitude + "," + coord.Longitude;
            }

            return null;
        }

        private void ShowUploadedImage(HttpPostedFileBase file)
        {
            byte[] image = new byte[file.ContentLength];
            file.InputStream.Read(image, 0, image.Length);
            string imreBase64Data = Convert.ToBase64String(image);
            string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
            ViewBag.ImageData = imgDataURL;
        }

        private void SaveUploadSearchRecord (string foundLP, bool wanted, bool locate)
        {
            ApplicationDbContext dbc = ApplicationDbContext.Create();
            SearchRecord sr = new SearchRecord();
            sr.Time = DateTime.Now;
            sr.PlateNumber = foundLP;
            sr.Stolen = wanted;
            sr.Latitude = null;
            sr.Longitude = null;
            if (Request.IsAuthenticated) sr.UserId = User.Identity.GetUserId();
            else sr.UserId = null;
            if (locate)
            {
                (string city, string region, string country, double latitude, double longitude) = GeolocationAPI.Instance.GetInfo(Request.UserHostAddress);
                sr.City = city;
                sr.Region = region;
                sr.Country = country;
                sr.Latitude = latitude;
                sr.Longitude = longitude;
            }
            dbc.SearchRecords.Add(sr);
            dbc.SaveChanges();
        }

        private void SetViewBagLabels (string originalLP, bool wanted)
        {
            ViewBag.LicensePlate = originalLP;
            ViewBag.LicensePlateColor = wanted ? "red" : "black";
            ViewBag.Label = wanted ? "Vehicle wanted!" : "Vehicle not wanted.";
            ViewBag.LabelColor = wanted ? "red" : "green";
        }

        private void SetViewBagLabels ()
        {
            ViewBag.Label = "Sorry! License plate unrecognised.";
            ViewBag.LabelColor = "red";
        }
    
        private void RefreshTextSearchAchievements()
        {
            ApplicationDbContext db = ApplicationDbContext.Create();
            SearchRecord sr = new SearchRecord();
            AchievementData ad = new AchievementData();
            string userID = User.Identity.GetUserId();

            string groupName = "Text Searcher";
            var srQuery =
                from i in db.SearchRecords
                where i.UserId == userID && i.Latitude == null
                select i.PlateNumber;
            int count = srQuery.Count();      // count of text searches
            RefreshTierAndName(userID, groupName, count);


            groupName = "Wanted Plates Finder";
            srQuery =
                from i in db.SearchRecords
                where i.UserId == userID && i.Stolen == true
                select i.PlateNumber;
            count = srQuery.Count();      // count of all wanted plates found
            RefreshTierAndName(userID, groupName, count);


            groupName = "Overall Achiever";
            RefreshTierAndName(userID, groupName, ad.GetOverallAchieverCount(userID));
        }

        private void RefreshImageSearchAchievements()
        {
            ApplicationDbContext db = ApplicationDbContext.Create();
            SearchRecord sr = new SearchRecord();
            AchievementData ad = new AchievementData();
            string userID = User.Identity.GetUserId();

            string groupName = "Image Searcher";
            var srQuery =
                from i in db.SearchRecords
                where i.UserId == userID && i.Latitude != null
                select i.PlateNumber;
            int count = srQuery.Count();      // count of image searches
            RefreshTierAndName(userID, groupName, count);


            groupName = "Wanted Images Finder";
            srQuery =
                from i in db.SearchRecords
                where i.UserId == userID && i.Stolen == true && i.Latitude != null
                select i.PlateNumber;
            count = srQuery.Count();      // count of all images with wanted plates found
            RefreshTierAndName(userID, groupName, count);


            groupName = "Wanted Plates Finder";
            srQuery =
                from i in db.SearchRecords
                where i.UserId == userID && i.Stolen == true
                select i.PlateNumber;
            count = srQuery.Count();      // count of all wanted plates found
            RefreshTierAndName(userID, groupName, count);


            groupName = "Overall Achiever";
            RefreshTierAndName(userID, groupName, ad.GetOverallAchieverCount(userID));
        }

        private void RefreshTierAndName (string userID, string groupName, int count)
        {
            ApplicationDbContext db = ApplicationDbContext.Create();
            AchievementData ad = new AchievementData();

            var adQuery =
                from i in db.Achievements
                where i.UserId == userID && i.GroupName == groupName
                select i.Tier;
            int currentTier = adQuery.ToList()[0];

            int trueTier = ad.GetTier(groupName, count);
            if (currentTier != trueTier)
            {
                Achievement existing = db.Achievements.Find(new object[] { userID, groupName, currentTier });
                existing.Tier = trueTier;
                existing.Name = ad.GetTierName(groupName, trueTier);
                db.SaveChanges();
            }
        }
    }
}