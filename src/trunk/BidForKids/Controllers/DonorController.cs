using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using BidForKids.Models;

namespace BidForKids.Controllers
{
    public class DonorController : Controller
    {
        private IProcurementFactory factory;

        public DonorController(IProcurementFactory factory)
        {
            this.factory = factory;
        }

        //
        // GET: /Donor/

        public ActionResult Index()
        {
            return View(factory.GetDonors());
        }

        //
        // GET: /Donor/Details/5

        public ActionResult Details(int id)
        {
            return View(factory.GetDonor(id));
        }

        //
        // GET: /Donor/Create

        public ActionResult Create()
        {
            SetupCreateViewData();

            return View();
        }

        private void SetupCreateViewData()
        {
            ViewData["GeoLocation_ID"] = GetGeoLocationsSelectList(null);
        }

        private void SetupEditViewData(Donor donor)
        {
            ViewData["GeoLocation_ID"] = GetGeoLocationsSelectList(donor.GeoLocation_ID);
        }

        //
        // POST: /Donor/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                SetupCreateViewData();

                Donor lNewDonor = factory.GetNewDonor();

                UpdateModel<Donor>(lNewDonor, new[] {
                    "Address",
                    "BusinessName",
                    "City",
                    "FirstName",
                    "LastName",
                    "Notes",
                    "Phone1",
                    "Phone1Desc",
                    "Phone2",
                    "Phone2Desc",
                    "Phone3",
                    "Phone3Desc",
                    "State",
                    "ZipCode",
                    "GeoLocation_ID"
                });

                int lNewDonorID = factory.AddDonor(lNewDonor);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private SelectList GetGeoLocationsSelectList(int? selectedValue)
        {
            IEnumerable<GeoLocation> lGeoLocations = factory.GetGeoLocations();
            return new SelectList(lGeoLocations.OrderBy(x => x.GeoLocationName), "GeoLocation_ID", "GeoLocationName", selectedValue);
        }

        //
        // GET: /Donor/Edit/5

        public ActionResult Edit(int id)
        {
            Donor lDonor = factory.GetDonor(id);

            SetupEditViewData(lDonor);

            return View(factory.GetDonor(id));
        }

        //
        // POST: /Donor/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Donor lDonor = factory.GetDonor(id);

                SetupEditViewData(lDonor);

                UpdateModel<Donor>(lDonor, new[] {
                    "Address",
                    "BusinessName",
                    "City",
                    "FirstName",
                    "LastName",
                    "Notes",
                    "Phone1",
                    "Phone1Desc",
                    "Phone2",
                    "Phone2Desc",
                    "Phone3",
                    "Phone3Desc",
                    "State",
                    "ZipCode",
                    "GeoLocation_ID"
                });

                if (factory.SaveDonor(lDonor) == false)
                {
                    throw new ApplicationException("Unable to save Donor");
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(factory.GetDonor(id));
            }
        }
    }
}
