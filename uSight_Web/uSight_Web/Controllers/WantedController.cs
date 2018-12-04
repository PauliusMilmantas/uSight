using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uSight_Web.Models;

namespace uSight_Web.Controllers
{
    public class WantedController : Controller
    {
        // GET: Wanted
        public ActionResult Index(string plate)
        {
            if (Request.IsAuthenticated)
            {
                var dbc = ApplicationDbContext.Create();
                if (plate != null)
                {
                    WantedRecord wr = new WantedRecord();
                    wr.UserId = User.Identity.GetUserId();
                    wr.PlateNumber = plate;
                    WantedRecord existing = dbc.WantedRecords.Find(new object[] { wr.UserId, wr.PlateNumber });
                    if (existing == null)
                    {
                        dbc.WantedRecords.Add(wr);
                        dbc.SaveChanges();
                    }
                }
                string userId = User.Identity.GetUserId();
                var myWantedQuery =
                    from wr in dbc.WantedRecords
                    where wr.UserId == userId
                    select wr.PlateNumber;
                var myWantedList = new List<Tuple<string, List<Tuple<bool, DateTime>>>>();
                foreach (var pName in myWantedQuery.ToList())
                {
                    var matchQuery =
                        from sr in dbc.SearchRecords
                        where sr.PlateNumber == pName
                        orderby sr.Time descending
                        select new { sr.Stolen, sr.Time };
                    var matchList = matchQuery.ToList().Select(x => new Tuple<bool, DateTime>(x.Stolen, x.Time)).ToList();
                    myWantedList.Add(new Tuple<string, List<Tuple<bool, DateTime>>>(pName, matchList));
                }
                return View(myWantedList);
            }
            else
            {
                return View(new List<Tuple<string, List<Tuple<bool, DateTime>>>>());
            }
        }

        public ActionResult Delete(string plate)
        {
            if (Request.IsAuthenticated && plate != null)
            {
                var dbc = ApplicationDbContext.Create();
                string userId = User.Identity.GetUserId();
                WantedRecord wr = dbc.WantedRecords.Find(new object[] {userId, plate});
                if (wr != null)
                {
                    dbc.WantedRecords.Remove(wr);
                    dbc.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}