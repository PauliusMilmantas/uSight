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
            Models.Search t = new Models.Search();

            return View(t.SearchRecords.ToList());
        }

        public ActionResult DeleteRecord() {

            /*string plate;


            try
            {
                plate = this.RouteData.Values["PlateNumber"].ToString();


                id = int.Parse(this.RouteData.Values["id"].ToString());
            }
            catch (Exception) {
                id = 0;

                return View();
            }

            entities = new Models.RecordsEntities1();

            List<Models.Record> rec = entities.Records.ToList();
            Models.Record rrr = rec.Find(x => x.Id == id);

            entities.Records.Remove(rrr);
            entities.SaveChanges();*/

            return RedirectToAction("Index");           
        }
    }
}