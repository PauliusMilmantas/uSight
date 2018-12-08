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
        public ActionResult Index()
        {
            ApplicationDbContext dbc = ApplicationDbContext.Create();

            var list = dbc.SearchRecords.ToList();

            var list2 = list.DistinctBy(x => x.PlateNumber);

            return View(list2);
        }

        public ActionResult Details(String PlateNumber = "")
        {
            String plateNumber = PlateNumber;
            
            if(plateNumber.Equals("")) plateNumber = Request["plateNumber"];

            ApplicationDbContext dbc = ApplicationDbContext.Create();

            SearchRecord record = dbc.SearchRecords.ToList().Find(x => x.PlateNumber.Equals(plateNumber));

            return View(record);
        }

        [HttpPost]
        public ActionResult Details() {

            String plateNumber = Request.Form["plateNumber"];
      
            ApplicationDbContext dbc = ApplicationDbContext.Create();

            SearchRecord record = dbc.SearchRecords.ToList().Find(x => x.PlateNumber.Equals(plateNumber));

            
            if (record == null)
            {
                record = new SearchRecord();
                record.PlateNumber = plateNumber;
            }

            return View(record);
        }
    }
}