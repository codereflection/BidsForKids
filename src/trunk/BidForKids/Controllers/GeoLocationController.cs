using System;
using System.Web.Mvc;
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

        /// <summary>
        /// Redirects to ReturnTo query string value, passing GeoLocation_ID=[NewGeoLocationID], else redirects to GeoLocation index
        /// </summary>
        /// <param name="NewGeoLocationID">New Donor_ID to pass back to ReturnTo url</param>
        /// <returns></returns>
        private ActionResult ReturnToOrRedirectToIndex(int NewGeoLocationID)
        {
            if (string.IsNullOrEmpty(Request.QueryString["ReturnTo"]) == false)
            {
                string lServerUrlDecode = Server.UrlDecode(Request.QueryString["ReturnTo"]);
                if (lServerUrlDecode.IndexOf("http:") == -1 && lServerUrlDecode.IndexOf("/") != 0)
                {
                    lServerUrlDecode = "/" + lServerUrlDecode;
                }

                lServerUrlDecode += lServerUrlDecode.IndexOf("?") == -1 ? "?GeoLocation_ID=" + NewGeoLocationID.ToString() : "&GeoLocation_ID=" + NewGeoLocationID.ToString();

                return Redirect(lServerUrlDecode);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /GeoLocation/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                GeoLocation lNewGeoLocation = factory.GetNewGeoLocation();

                UpdateModel<GeoLocation>(lNewGeoLocation,
                    new[] {
                        "GeoLocationName",
                        "Description"
                    });

                int lNewGeoLocationID = factory.AddGeoLocation(lNewGeoLocation);

                // return if ReturnTo parameter present

                return ReturnToOrRedirectToIndex(lNewGeoLocationID);
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

                UpdateModel<GeoLocation>(lGeoLocation, 
                    new[] {
                        "GeoLocationName",
                        "Description"
                    });

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
    }
}
