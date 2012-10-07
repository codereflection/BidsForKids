using System.Web.Mvc;
using BidsForKids.Data.Models;
using BidsForKids.ViewModels;
using Simple.Data;

namespace BidsForKids.Controllers
{
    [Authorize(Roles = "Administrator, Procurements")]
    public class GeoLocationController : Controller
    {
        readonly dynamic db;
        private readonly IProcurementRepository factory;

        public GeoLocationController()
        {
            db = Database.Open();
        }

        public GeoLocationController(IProcurementRepository factory)
        {
            db = Database.Open();
            this.factory = factory;
        }

        public ActionResult Index()
        {
            var geoLocations = db.GeoLocations.All();
            return View(geoLocations.ToList<GeoLocationViewModel>());
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(db.GeoLocations.FindAllByGeoLocation_ID(id).SingleOrDefault<GeoLocationViewModel>());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(GeoLocationViewModel model)
        {
            db.GeoLocations.Insert(
                GeoLocationName: model.GeoLocationName,
                Description: model.Description
            );

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(db.GeoLocations.FindAllByGeoLocation_ID(id).SingleOrDefault<GeoLocationViewModel>());
        }

        [HttpPost]
        public ActionResult Edit(GeoLocationViewModel model)
        {
            db.GeoLocations.Update(model);

            return RedirectToAction("Index");
        }
    }
}
