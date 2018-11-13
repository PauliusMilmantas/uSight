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

        public ActionResult AddRecord()
        {
            return View();
        }

        public ActionResult DeleteRecord()
        {
            var entities = new Models.RecordsEntities1();

            return View();
        }
    }
}