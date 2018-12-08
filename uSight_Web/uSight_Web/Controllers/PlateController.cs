using Microsoft.Ajax.Utilities;
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
        public ActionResult Index(String PlateNumber = "")
        {
            String plateNumber = PlateNumber;

            if (PlateNumber.Equals("")) {
                plateNumber = Request.Form["plateNumber"];
            }
                        
            ApplicationDbContext dbc = ApplicationDbContext.Create();

            var list = dbc.SearchRecords.ToList();

            var list2 = list.DistinctBy(x => x.PlateNumber);
            
            if (plateNumber == null || plateNumber.Equals(""))
            {
                ViewData["Action"] = 0;
            }
            else {
                ViewData["Action"] = 1;

                SearchRecord rr = list.Find(x => x.PlateNumber.Equals(plateNumber));



                if (rr.PlateNumber != null) { ViewData["LicensePlate"] = rr.PlateNumber; } else { ViewData["LicensePlate"] = ""; }

                if (rr.Region != null) { ViewData["Region"] = rr.Region; } else { ViewData["Region"] = ""; }

                if (rr.Longitude != null) { ViewData["Longitude"] = rr.Longitude; } else { ViewData["Longitude"] = ""; }

                if (rr.Latitude != null) { ViewData["Latitude"] = rr.Latitude; } else { ViewData["Latitude"] = ""; }

                if (rr.Time != null) { ViewData["Time"] = rr.Time; } else { ViewData["Time"] = ""; }

               
            }
            
            CommentViewModel cvm = new CommentViewModel();
            cvm.search = list2;
            cvm.comment = dbc.Comments;

            return View(cvm);
        }
    }
}