using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uSight_Web.Models;

namespace uSight_Web.Controllers
{
    public class DataController : Controller
    {
        // GET: Data
        public ActionResult Index(int filter = 0)
        {
            ApplicationDbContext dbc = ApplicationDbContext.Create();

            ViewData["Filter"] = filter;

            return View(dbc.SearchRecords.ToList());
        }
        
        public void Update(int filter) {

            ViewData["Filter"] = filter;
        }

        public ActionResult Delete(String PlateNumber, String Time, int TAction = 0) {

            string plate = PlateNumber;
            string time = Time;

            string tt;

            ApplicationDbContext dbc = ApplicationDbContext.Create();

            List<SearchRecord> rec = dbc.SearchRecords.ToList();
            SearchRecord rrr = rec.Find(x => x.PlateNumber == plate && x.Time.ToString() == time);

            try
            {
                tt = this.RouteData.Values["Action"].ToString();
            } catch (Exception e) {
                return View(rrr);
            }
            
            if (tt.Equals("1") || TAction == 1) {
                dbc.SearchRecords.Remove(rrr);
                dbc.SaveChanges();

                return RedirectToAction("Index");
            }
            

            return View(rrr);
        }
    }
}