using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using BidForKids.Models;

namespace BidForKids.Controllers
{
    public class GeoLocationController : Controller
    {
        private IProcurementFactory factory;

        public GeoLocationController(IProcurementFactory factory)
        {
            this.factory = factory;
        }

        //
        // GET: /GeoLocation/

        public ActionResult Index()
        {
            return View(factory.GetGeoLocations());
        }

        //
        // GET: /GeoLocation/Details/5

        public ActionResult Details(int id)
        {
            return View(factory.GetGeoLocation(id));
        }

        //
        // GET: /GeoLocation/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /GeoLocation/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                GeoLocation lNewGeoLocation = factory.GetNewGeoLocation();

                SetGeoLocationValues(collection, lNewGeoLocation);

                int lNewGeoLocationID = factory.AddGeoLocation(lNewGeoLocation);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /GeoLocation/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View(factory.GetGeoLocation(id));
        }

        //
        // POST: /GeoLocation/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                GeoLocation lGeoLocation = factory.GetGeoLocation(id);

                UpdateModel(lGeoLocation, 
                    new[] {
                        "GeoLocationName",
                        "Description"
                    });
                //SetGeoLocationValues(collection, lGeoLocation);

                if (factory.SaveGeoLocation(lGeoLocation) == false)
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

        private static void SetGeoLocationValues(FormCollection collection, GeoLocation geoLocation)
        {
            geoLocation.GeoLocationName = collection["GeoLocationName"];
            geoLocation.Description = collection["Description"];
        }
    }
}
