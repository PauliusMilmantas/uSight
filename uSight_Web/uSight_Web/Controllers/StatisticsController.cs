using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uSight_Web.Entities;
using uSight_Web.Models;

namespace uSight_Web.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
            ApplicationDbContext dbc = ApplicationDbContext.Create();

            ChartData timeCountSearchesCD;
            {
                var allQuery =
                    from sr in dbc.SearchRecords
                    group sr by DbFunctions.TruncateTime(sr.Time) into g
                    orderby g.Key
                    select new { date = g.Key, count = g.Count() };

                var stolenQuery =
                    from sr in dbc.SearchRecords
                    group sr by DbFunctions.TruncateTime(sr.Time) into g
                    orderby g.Key
                    select new { date = g.Key, count = g.Count(x => x.Stolen) };
                
                var labels = allQuery.ToList().Select(x => x.date.HasValue ? x.date.Value.ToShortDateString() : x.ToString()).ToList();
                var series = new List<string>() { "All", "Stolen" };
                var data = new List<List<int>>();
                data.Add(allQuery.Select(x => x.count).ToList());
                data.Add(stolenQuery.Select(x => x.count).ToList());
                timeCountSearchesCD = new ChartData(labels, series, data);
            }

            ViewBag.timeCountSearchesCD = timeCountSearchesCD;

            return View();
            //return HttpNotFound();
        }

        // GET: Statistics/General
        public ViewResult General()
        {
            var stats = new Statistics() {Name = "<Some general statistics here>" };
            return View(stats);
        }
    }
}