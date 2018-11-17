using System;
using System.Web;
using System.Web.Mvc;
using uSight_Web.Entities;

namespace uSight_Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(HttpPostedFileBase file)
        {
            string serverMapPath = Server.MapPath(".");

            if (file != null && file.ContentLength > 0)
                try
                {
                    string foundLP = new RecognitionBuilder(file, serverMapPath).GetFoundLP();
                    ViewBag.Message = foundLP;
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR: " + ex.Message.ToString();
                }
        
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}