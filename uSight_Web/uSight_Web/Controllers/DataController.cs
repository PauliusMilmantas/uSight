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
        public ActionResult Index(int filter = 0)
        {
            Models.Search t = new Models.Search();
            
            ViewData["Filter"] = filter;

            return View(t.SearchRecords.ToList());
        }
        
        public void Update(int filter) {

            ViewData["Filter"] = filter;
        }

        public ActionResult Delete(String PlateNumber, String Time, int Action = 0) {

              string plate = PlateNumber;
              string time = Time;

            string tt;

            Models.Search entities = new Models.Search();

            List<Models.SearchRecord> rec = entities.SearchRecords.ToList();
            Models.SearchRecord rrr = rec.Find(x => x.PlateNumber == plate && x.Time.ToString() == time);

            try
            {
                tt = this.RouteData.Values["Action"].ToString();
            } catch (Exception e) {
                return View(rrr);
            }

            if (Action == 1) {
                entities.SearchRecords.Remove(rrr);
                entities.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(rrr);
        }
    }
}