using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
        public ActionResult Index(int? totalStartYear, int? totalStartMonth, int? totalStartDay, int? totalEndYear, int? totalEndMonth, int? totalEndDay)
        {
            ApplicationDbContext dbc = ApplicationDbContext.Create();

            {
                DateTime start = DateTime.MinValue;
                DateTime end = DateTime.MaxValue;
                if (totalStartYear != null && totalStartMonth != null && totalStartDay != null && totalEndYear != null && totalEndMonth != null && totalStartDay != null)
                {
                    start = new DateTime(totalStartYear.Value, totalStartMonth.Value, totalStartDay.Value);
                    end = new DateTime(totalEndYear.Value, totalEndMonth.Value, totalEndDay.Value);
                    if (start > end)
                    {
                        DateTime tmp = start;
                        start = end;
                        end = tmp;
                    }
                }

                ChartData timeCountSearchesCD;
                var allQuery =
                    from sr in dbc.SearchRecords
                    group sr by DbFunctions.TruncateTime(sr.Time) into g
                    where start.Year <= g.Key.Value.Year && start.Month <= g.Key.Value.Month && start.Day <= g.Key.Value.Day && end.Year >= g.Key.Value.Year && end.Month >= g.Key.Value.Month && end.Day >= g.Key.Value.Day
                    orderby g.Key
                    select new { date = g.Key, count = g.Count() };

                var stolenQuery =
                    from sr in dbc.SearchRecords
                    group sr by DbFunctions.TruncateTime(sr.Time) into g
                    where start.Year <= g.Key.Value.Year && start.Month <= g.Key.Value.Month && start.Day <= g.Key.Value.Day && end.Year >= g.Key.Value.Year && end.Month >= g.Key.Value.Month && end.Day >= g.Key.Value.Day
                    orderby g.Key
                    select new { date = g.Key, count = g.Count(x => x.Stolen) };

                var labels = allQuery.ToList().Select(x => x.date.HasValue ? x.date.Value.ToShortDateString() : x.ToString()).ToList();
                var series = new List<string>() { "All", "Stolen" };
                var data = new List<List<double>>();
                data.Add(allQuery.Select(x => (double)x.count).ToList());
                data.Add(stolenQuery.Select(x => (double)x.count).ToList());
                timeCountSearchesCD = new ChartData(labels, series, data);
                ViewBag.timeCountSearchesCD = timeCountSearchesCD;
            }

            {
                ChartData monthlyCountSearchesCD;
                var allQuery =
                    from sr in dbc.SearchRecords
                    group sr by DbFunctions.CreateDateTime(sr.Time.Year, sr.Time.Month, 1, 0, 0, 0) into g
                    orderby g.Key
                    select new { date = g.Key, count = g.Count() };

                var stolenQuery =
                    from sr in dbc.SearchRecords
                    group sr by DbFunctions.CreateDateTime(sr.Time.Year, sr.Time.Month, 1, 0, 0, 0) into g
                    orderby g.Key
                    select new { date = g.Key, count = g.Count(x => x.Stolen) };

                var dtfi = new DateTimeFormatInfo();

                var labels = allQuery.ToList().Select(x => x.date.HasValue ? dtfi.GetMonthName(x.date.Value.Month) + " " + x.date.Value.Year : x.ToString()).ToList();
                var series = new List<string>() { "All", "All Per Day", "Stolen", "Stolen Per Day" };
                var data = new List<List<double>>();
                data.Add(allQuery.Select(x => (double)x.count).ToList());
                data.Add(allQuery.ToList().Select(x => (double)x.count / DateTime.DaysInMonth(x.date.Value.Year, x.date.Value.Month)).ToList());
                data.Add(stolenQuery.Select(x => (double)x.count).ToList());
                data.Add(stolenQuery.ToList().Select(x => (double)x.count / DateTime.DaysInMonth(x.date.Value.Year, x.date.Value.Month)).ToList());
                monthlyCountSearchesCD = new ChartData(labels, series, data);
                ViewBag.monthlyCountSearchesCD = monthlyCountSearchesCD;
            }

            {
                ChartData yearlyCountSearchesCD;
                var allQuery =
                    from sr in dbc.SearchRecords
                    group sr by DbFunctions.CreateDateTime(sr.Time.Year, 1, 1, 0, 0, 0) into g
                    orderby g.Key
                    select new { date = g.Key, count = g.Count() };

                var stolenQuery =
                    from sr in dbc.SearchRecords
                    group sr by DbFunctions.CreateDateTime(sr.Time.Year, 1, 1, 0, 0, 0) into g
                    orderby g.Key
                    select new { date = g.Key, count = g.Count(x => x.Stolen) };

                var labels = allQuery.ToList().Select(x => x.date.HasValue ? "" + x.date.Value.Year : x.ToString()).ToList();
                var series = new List<string>() { "All", "All Per Month", "Stolen", "Stolen Per Month" };
                var data = new List<List<double>>();
                data.Add(allQuery.Select(x => (double)x.count).ToList());
                data.Add(allQuery.ToList().Select(x => (double)x.count / 12).ToList());
                data.Add(stolenQuery.Select(x => (double)x.count).ToList());
                data.Add(stolenQuery.ToList().Select(x => (double)x.count / 12).ToList());
                yearlyCountSearchesCD = new ChartData(labels, series, data);
                ViewBag.yearlyCountSearchesCD = yearlyCountSearchesCD;
            }

            {
                ChartData everyMonthCountSearchesCD;
                var allQuery =
                    from sr in dbc.SearchRecords
                    group sr by DbFunctions.CreateDateTime(1, sr.Time.Month, 1, 0, 0, 0) into g
                    orderby g.Key
                    select new { date = g.Key, count = g.Count() };

                var stolenQuery =
                    from sr in dbc.SearchRecords
                    group sr by DbFunctions.CreateDateTime(1, sr.Time.Month, 1, 0, 0, 0) into g
                    orderby g.Key
                    select new { date = g.Key, count = g.Count(x => x.Stolen) };

                var dtfi = new DateTimeFormatInfo();

                var labels = allQuery.ToList().Select(x => x.date.HasValue ? dtfi.GetMonthName(x.date.Value.Month) : x.ToString()).ToList();
                var series = new List<string>() { "All", "All Per Day", "Stolen", "Stolen Per Day" };
                var data = new List<List<double>>();
                data.Add(allQuery.Select(x => (double)x.count).ToList());
                data.Add(allQuery.ToList().Select(x => (double)x.count / DateTime.DaysInMonth(2019, x.date.Value.Month)).ToList());
                data.Add(stolenQuery.Select(x => (double)x.count).ToList());
                data.Add(stolenQuery.ToList().Select(x => (double)x.count / DateTime.DaysInMonth(2019, x.date.Value.Month)).ToList());
                everyMonthCountSearchesCD = new ChartData(labels, series, data);
                ViewBag.everyMonthCountSearchesCD = everyMonthCountSearchesCD;
            }

            return View();
            //return HttpNotFound();
        }

        // GET: Statistics/General
        public ViewResult General()
        {
            var stats = new Statistics() { Name = "<Some general statistics here>" };
            return View(stats);
        }
    }
}