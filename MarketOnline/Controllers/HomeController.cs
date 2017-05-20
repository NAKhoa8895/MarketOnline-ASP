using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MarketOnline.Models;
namespace MarketOnline.Controllers
{
    public class HomeController : Controller
    {
        private MarketOnlineContext db = new MarketOnlineContext();
        public ActionResult Index()
        {
            ViewBag.city = new SelectList(db.City.ToList(), "Id", "Name");

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}