using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace uSight_Web.Controllers
{
    public class DataController : Controller
    {
        // GET: Data
        public ActionResult Index()
        {
            var entities = new Models.RecordsEntities1();

            return View(entities.Records.ToList());
        }

        // GET: Data
        public ActionResult Records()
        {
            return View();
        }

        public ActionResult AddRecord()
        {
            return View();
        }

        public ActionResult ViewRecords()
        {
            return View();
        }

        public ActionResult DeleteRecord()
        {
            return View();
        }
    }
}