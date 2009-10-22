using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using BidForKids.Models;
using BidForKids.Models.SerializableObjects;

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
        // GET: /GetDonors/

        public ActionResult GetDonors()
        {
            jqGridLoadOptions loadOptions = jqGridLoadOptions.GetLoadOptions(Request.QueryString);

            JsonResult lResult = new JsonResult();

            List<SerializableDonor> lRows = factory.GetSerializableDonors(loadOptions);

            if (lRows == null)
            {
                throw new ApplicationException("Unable to load Donors list");
            }

            int lTotalRows = lRows.Count;

            lRows = lRows.Skip((loadOptions.page - 1) * loadOptions.rows).Take(loadOptions.rows).ToList();

            int lTotalPages = (int)Math.Ceiling((decimal)lTotalRows / (decimal)loadOptions.rows);

            lResult.Data = new { total = lTotalPages, page = loadOptions.page, records = lTotalRows.ToString(), rows = lRows };

            return lResult;
        }

        //
        // GET: /Donor/

        public ActionResult Index()
        {
            return View(factory.GetDonors());
        }

        //
        // GET: /Donor/GridIndex

        public ActionResult GridIndex()
        {
            return View();
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
            if (string.IsNullOrEmpty(Request.QueryString["GeoLocation_ID"]) == false)
                ViewData["GeoLocation_ID"] = GetGeoLocationsSelectList(int.Parse(Request.QueryString["GeoLocation_ID"].ToString()));
            else
                ViewData["GeoLocation_ID"] = GetGeoLocationsSelectList(null);

        }

        private void SetupEditViewData(Donor donor)
        {
            ViewData["GeoLocation_ID"] = GetGeoLocationsSelectList(donor.GeoLocation_ID);
        }

        /// <summary>
        /// Redirects to ReturnTo query string value, passing Donor_ID=[NewDonorID], else redirects to Donor index
        /// </summary>
        /// <param name="NewDonorID">New Donor_ID to pass back to ReturnTo url</param>
        /// <returns></returns>
        private ActionResult ReturnToOrRedirectToIndex(int NewDonorID)
        {
            if (string.IsNullOrEmpty(Request.QueryString["ReturnTo"]) == false)
            {
                string lServerUrlDecode = Server.UrlDecode(Request.QueryString["ReturnTo"]);
                if (lServerUrlDecode.IndexOf("http:") == -1 && lServerUrlDecode.IndexOf("/") != 0)
                {
                    lServerUrlDecode = "/" + lServerUrlDecode;
                }

                lServerUrlDecode += lServerUrlDecode.IndexOf("?") == -1 ? "?Donor_ID=" + NewDonorID.ToString() : "&Donor_ID=" + NewDonorID.ToString();

                return Redirect(lServerUrlDecode);
            }
            else
            {
                return RedirectToAction("Index");
            }
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

                return ReturnToOrRedirectToIndex(lNewDonorID);
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
