using System;
using System.Web;
using System.Web.Mvc;
using uSight_Web.Models;
using uSight_Web.Entities;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;
using System.Device.Location;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Globalization;

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
                            SaveUploadSearchRecord(foundLP.ToUpper(), wanted);
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
                        SaveUploadSearchRecord(foundLP.ToUpper(), wanted);
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

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        // NOT WORKING
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

        public static IpInfo GetUserInfo(string ip)
        {
            IpInfo ipInfo = new IpInfo();
            try
            {
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
            }
            catch (Exception)
            {
                ipInfo.Country = null;
            }

            return ipInfo;
        }

        private void SaveUploadSearchRecord (string foundLP, bool wanted)
        {
            IpInfo info = GetUserInfo(GetIPAddress());
            
            ApplicationDbContext dbc = ApplicationDbContext.Create();
            SearchRecord sr = new SearchRecord();
            sr.Time = DateTime.Now;
            sr.PlateNumber = foundLP;
            sr.Stolen = wanted;
            sr.City = info.City;
            sr.Region = info.Region;
            sr.Country = info.Country;
            

            /*
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        */


            if (Request.IsAuthenticated) sr.UserId = User.Identity.GetUserId();
            else sr.UserId = null;
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
    }
}