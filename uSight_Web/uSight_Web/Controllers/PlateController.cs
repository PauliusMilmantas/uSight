using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uSight_Web.Entities;
using uSight_Web.Models;

namespace uSight_Web.Controllers
{
    public class PlateController : Controller
    {
        // GET: Plate
        public ActionResult Index(String PlateNumber = "", int CPost = 0)
        {
            ViewBag.Search = true;

            String plateNumber = PlateNumber;

            if (PlateNumber.Equals("")) {
                plateNumber = Request.Form["plateNumber"];
            }
                        
            ApplicationDbContext dbc = ApplicationDbContext.Create();
            
            //Comment trigger
            if (CPost == 1) {

                String text = Request.Form["comment"];
                DateTime tm = DateTime.Now;
                String uId = User.Identity.GetUserId().ToString();
                string plate = plateNumber;

                Models.Comment cmy = new Comment();
                
                cmy.Text = text.ToString();
                cmy.UserId = uId;
                cmy.PlateNumber = plate;
                cmy.Time = tm;

                dbc.Comments.Add(cmy);
                dbc.SaveChanges();   
                
                return RedirectToAction("Index");
            }
            
            var list = dbc.SearchRecords.ToList();

            var list2 = list.DistinctBy(x => x.PlateNumber);
            
            if (plateNumber == null || plateNumber.Equals(""))
            {
                ViewData["Action"] = 0;
            }
            else {
                ViewData["Action"] = 1;

                SearchRecord rr = list.Find(x => x.PlateNumber.Equals(plateNumber));

                if (rr == null)
                {
                    rr = new SearchRecord();

                    rr.PlateNumber = plateNumber;
                    rr.Time = DateTime.Now;
                }

                if (rr.PlateNumber != null) { ViewData["LicensePlate"] = rr.PlateNumber; } else { ViewData["LicensePlate"] = ""; }
                var markers = new List<MapMarker>();
                if (rr.PlateNumber != null)
                {
                    var query = from sr in dbc.SearchRecords
                                where sr.Latitude != null && sr.PlateNumber == rr.PlateNumber
                                select new {sr.Latitude, sr.Longitude, sr.Time, sr.Stolen};
                    string format = "<p><b>Latitude:</b> {0}</p>" +
                                    "<p><b>Longitude:</b> {1}</p>" +
                                    "<p><b>Time:</b> {2}</p>" +
                                    "<p><b>Was stolen:</b> {3}</p>";
                    markers = query.ToList().Select(x => new MapMarker { lat = (double)x.Latitude, lng = (double)x.Longitude, desc = string.Format(format, (double)x.Latitude, (double)x.Longitude, x.Time, x.Stolen ? "Yes": "No") }).ToList();
                }
                MapData mapData = new MapData(markers);
                ViewBag.MapData = mapData;
                ViewBag.Search = false;
            }
            
            IEnumerable<Models.Comment> tre3 = dbc.Comments.ToList().FindAll(x => x.PlateNumber.Equals(plateNumber));
            
            CommentViewModel cvm = new CommentViewModel();

            cvm.search = list2;
            cvm.comment = tre3;

            return View(cvm);
        }
    }
}