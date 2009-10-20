using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BidForKids.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to the Gatewood Elementary 'Bid For Kids' Auction Procurement Database!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Procurement()
        {
            ViewData["Message"] = "Procurement List";
            return View();
        }

        public ActionResult GeoLocation()
        {
            return View();
        }

        public ActionResult Reports()
        {
            return View();
        }
    }
}
