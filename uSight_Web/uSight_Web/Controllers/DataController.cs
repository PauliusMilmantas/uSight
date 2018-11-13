using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace uSight_Web.Controllers
{
    public class DataController : Controller
    {
        private Models.RecordsEntities1 entities;

        // GET: Data
        public ActionResult Index()
        {
            entities = new Models.RecordsEntities1();

            return View(entities.Records.ToList());
        }

        public ActionResult AddRecord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRecord(Models.Record rr) {

            entities = new Models.RecordsEntities1();

            if (ModelState.IsValid) {
                entities.Records.Add(rr);
                entities.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult DeleteRecord() {
            return View();
        }

        /*
        [HttpPost]
        public ActionResult DeleteRecord(int id)
        {
            entities = new Models.RecordsEntities1();
            
            

            return RedirectToAction("Index");
        }*/
    }
}