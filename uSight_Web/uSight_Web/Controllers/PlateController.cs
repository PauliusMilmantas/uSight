using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

                if (rr.Region != null) { ViewData["Region"] = rr.Region; } else { ViewData["Region"] = ""; }

                if (rr.Longitude != null) { ViewData["Longitude"] = rr.Longitude; } else { ViewData["Longitude"] = ""; }

                if (rr.Latitude != null) { ViewData["Latitude"] = rr.Latitude; } else { ViewData["Latitude"] = ""; }

                if (rr.Time != null) { ViewData["Time"] = rr.Time; } else { ViewData["Time"] = ""; }

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