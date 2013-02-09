using System;
using System.Web.Mvc;
using BidsForKids.Data.Models;

namespace BidsForKids.Controllers
{
    [Authorize(Roles = "Administrator, Procurements")]
    public class GeoLocationController : Controller
    {
        private readonly IProcurementRepository factory;

        public GeoLocationController(IProcurementRepository factory)
        {
            this.factory = factory;
        }

        public ActionResult Index()
        {
            return View(factory.GetGeoLocations());
        }

        public ActionResult Details(int id)
        {
            return View(factory.GetGeoLocation(id));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var newGeoLocation = factory.GetNewGeoLocation();

                UpdateModel(newGeoLocation,
                    new[] {
                        "GeoLocationName",
                        "Description"
                    });

                var id = factory.AddGeoLocation(newGeoLocation);

                return ControllerHelper.ReturnToOrRedirectToIndex(this, id, "GeoLocation_ID");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View(factory.GetGeoLocation(id));
        }

        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var geoLocation = factory.GetGeoLocation(id);

                UpdateModel(geoLocation, 
                    new[] {
                        "GeoLocationName",
                        "Description"
                    });

                if (factory.SaveGeoLocation(geoLocation) == false)
                {
                    throw new ApplicationException("Unable to save Geo Location");
                }
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
