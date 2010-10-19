using System.Web.Mvc;
using BidsForKids.Data.Models;

namespace BidsForKids.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private IProcurementRepository factory;

        public HomeController(IProcurementRepository factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Initializes a new instance of the HomeController class.
        /// </summary>
        public HomeController()
        {
            
        }

        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to the Gatewood Elementary 'Bids For Kids' Auction Procurement Database!";

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

        public ActionResult GeoLocationDonorList()
        {
            return PartialView(factory.GetGeoLocations());
        }

        public ActionResult GeoLocationReports()
        {
            return View(factory.GetGeoLocations());
        }
    }
}
