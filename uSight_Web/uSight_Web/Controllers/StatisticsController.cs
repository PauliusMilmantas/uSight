using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uSight_Web.Models;

namespace uSight_Web.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
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