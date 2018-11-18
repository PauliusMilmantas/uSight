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

        public ActionResult Delete(string PlateNumber, string Time) {

            string plate = PlateNumber;
            string time = Time;


         /*   try
            {
                plate = this.RouteData.Values["PlateNumber"].ToString();
                time = this.RouteData.Values["Time"].ToString();
            }
            catch (Exception) {
                return RedirectToAction("Index");
            }
            */

            Models.Search entities = new Models.Search();

            List<Models.SearchRecord> rec = entities.SearchRecords.ToList();

            Models.SearchRecord rrr = rec.Find(x => x.Time.ToString() == time);// && x.PlateNumber == plate);


    
            //entities.SearchRecords.Remove(rrr);

           // entities.SaveChanges();

            return View(rrr);           
        }
    }
}