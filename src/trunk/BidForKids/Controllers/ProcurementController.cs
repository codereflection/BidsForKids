using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Newtonsoft.Json;
using BidForKids.Models;
using System.Collections;

namespace BidForKids.Controllers
{

    public class ProcurementController : Controller
    {
        private IProcurementFactory factory;

        public ProcurementController(IProcurementFactory factory)
        {
            this.factory = factory;
        }

        //
        // GET: /Procurement/GridIndex

        public ActionResult GridIndex()
        {
            return View(factory.GetProcurements());
        }


        //
        // GET: /Procurement/

        public ActionResult Index()
        {
            return View(factory.GetProcurements());
        }

        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetProcurement(int? id)
        {
            SerializableProcurement lProcurement = factory.ConvertProcurementToSerializableProcurement(factory.GetProcurement((int)id));

            return Json(lProcurement);
        }

        //public ActionResult GetProcurements(string _search, string nd, int page, int rows, string sidx, string sord)
        public ActionResult GetProcurements()
        {
            Dictionary<string, string> lSearchParams = new Dictionary<string, string>();

            System.Collections.Specialized.NameValueCollection lParams = Request.QueryString;

            bool _search = bool.Parse(lParams["_search"]);
            string nd = lParams["nd"];
            int page = int.Parse(lParams["page"]);
            int rows = int.Parse(lParams["rows"]);
            string sidx = lParams["sidx"];
            string sord = lParams["sord"];

            Dictionary<string, string> searchParams = new Dictionary<string, string>();

            if (_search == true)
            {
                foreach (var param in lParams.AllKeys)
                {
                    if (param != "_search" && param != "nd" && param != "page" && param != "rows" && param != "sidx" && param != "sord")
                    {
                        searchParams.Add(param, lParams[param]);
                    }
                }
            }



            JsonResult lResult = new JsonResult();


            List<SerializableProcurement> lRows = factory.GetSerializableProcurements(sidx, sord, _search, searchParams);

            int lTotalRows = lRows.Count;

            lRows = lRows.Skip((page - 1) * rows).Take(rows).ToList();

            int lTotalPages = (int)Math.Ceiling((decimal)lTotalRows / (decimal)rows);

            lResult.Data = new { total = lTotalPages, page = page, records = lTotalRows.ToString(), rows = lRows };

            return lResult;
        }

        public ActionResult Deleted()
        {
            return null;
        }

        //
        // GET: /Procurement/Delete/5

        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }

            bool lResult = factory.DeleteProcurement((int)id);

            ContentResult contentResult = new ContentResult();
            contentResult.Content = lResult.ToString();

            return contentResult;
        }


        //
        // GET: /Procurement/Details/5

        public ActionResult Details(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(factory.GetProcurement((int)id));
        }

        //
        // GET: /Procurement/Create

        public ActionResult Create()
        {
            SetupCreateViewData();

            return View();
        }

        private void SetupCreateViewData()
        {
            ViewData["Auction_ID"] = GetAuctionSelectList(null);
            ViewData["Contact_ID"] = GetContactsSelectList(null);
            ViewData["GeoLocation_ID"] = GetGeoLocationsSelectList(null);
            ViewData["Category_ID"] = GetCategoriesSelectList(null);
        }

        private void SetupEditViewData(ContactProcurement contactProcurement)
        {
            int? lAuctionId = null;
            int? lContactId = null;
            int? lGeoLocationId = null;
            int? lCategoryId = null;
            int? lProcurerID = null;

            if (contactProcurement != null)
            {
                lAuctionId = contactProcurement.Auction_ID;
                lContactId = contactProcurement.Contact_ID;
                lGeoLocationId = contactProcurement.Procurement.GeoLocation_ID;
                lCategoryId = contactProcurement.Procurement.Category_ID;
                lProcurerID = contactProcurement.Procurer_ID;
            }

            ViewData["Auction_ID"] = GetAuctionSelectList(lAuctionId);
            ViewData["Contact_ID"] = GetContactsSelectList(lContactId);
            ViewData["GeoLocation_ID"] = GetGeoLocationsSelectList(lGeoLocationId);
            ViewData["Category_ID"] = GetCategoriesSelectList(lCategoryId);
            ViewData["Procurer_ID"] = GetProcurerSelectList(lProcurerID);
        }

        private SelectList GetAuctionSelectList(int? selectedValue)
        {
            return new SelectList(factory.GetAuctions(), "Auction_ID", "Year", selectedValue);
        }

        private SelectList GetContactsSelectList(int? selectedValue)
        {
            IEnumerable<Contact> lContacts = factory.GetContacts();
            return new SelectList(lContacts, "Contact_ID", "BusinessName", selectedValue);
        }

        private SelectList GetGeoLocationsSelectList(int? selectedValue)
        {
            IEnumerable<GeoLocation> lGeoLocations = factory.GetGeoLocations();
            return new SelectList(lGeoLocations, "GeoLocation_ID", "GeoLocationName", selectedValue);
        }

        private SelectList GetCategoriesSelectList(int? selectedValue)
        {
            IEnumerable<Category> lCategories = factory.GetCategories();
            return new SelectList(lCategories, "Category_ID", "CategoryName", selectedValue);
        }

        private SelectList GetProcurerSelectList(int? selectedValue)
        {
            IEnumerable<Procurer> lProcurers = factory.GetProcurers();

            var lProcurerList = from P in lProcurers
                                select new
                                {
                                    Procurer_ID = P.Procurer_ID,
                                    FirstName = P.FirstName,
                                    LastName = P.LastName,
                                    FullName = P.FirstName + " " + P.LastName
                                };

            return new SelectList(lProcurerList.OrderBy(x => x.LastName), "Procurer_ID", "FullName", selectedValue);
        }

        //
        // POST: /Procurement/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            SetupCreateViewData();

            try
            {
                Procurement lNewProcurement = factory.GetNewProcurement();
                ContactProcurement lNewContactProcurement = new ContactProcurement();
                lNewProcurement.ContactProcurement = lNewContactProcurement;

                UpdateModel<Procurement>(lNewProcurement,
                    new[] {
                        "CatalogNumber",
                        "AuctionNumber",
                        "ItemNumber",
                        "Description",
                        "Quantity",
                        "PerItemValue",
                        "Notes",
                        "EstimatedValue",
                        "SoldFor",
                        "Category_ID",
                        "GeoLocation_ID"
                    });

                UpdateModel<ContactProcurement>(lNewProcurement.ContactProcurement,
                    new[] {
                        "Contact_ID",
                        "Auction_ID",
                        "Procurer_ID"
                    });

                int lNewProcurementID = factory.AddProcurement(lNewProcurement);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Procurement/Edit/5

        public ActionResult Edit(int? id)
        {

            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }

            Procurement lProcurement = factory.GetProcurement((int)id);

            SetupEditViewData(lProcurement.ContactProcurement);

            return View(lProcurement);
        }

        //
        // POST: /Procurement/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int? id, FormCollection collection)
        {
            if (id.HasValue == false)
            {
                throw new ArgumentException("Invalid Procurement ID was passed.");
            }


            try
            {
                Procurement lProcurement = factory.GetProcurement((int)id);

                SetupEditViewData(lProcurement.ContactProcurement);

                UpdateModel<Procurement>(lProcurement,
                    new[] {
                        "CatalogNumber",
                        "AuctionNumber",
                        "ItemNumber",
                        "Description",
                        "Quantity",
                        "PerItemValue",
                        "Notes",
                        "EstimatedValue",
                        "SoldFor",
                        "Category_ID",
                        "GeoLocation_ID"
                    });

                UpdateModel<ContactProcurement>(lProcurement.ContactProcurement,
                    new[] {
                        "Contact_ID",
                        "Auction_ID",
                        "Procurer_ID"
                    });

                if (factory.SaveProcurement(lProcurement) == false)
                {
                    throw new ApplicationException("Unable to save procurement");
                }

                return RedirectToAction("Index");
            }
            catch
            {
                Procurement lProcurement = factory.GetProcurement((int)id);

                SetupEditViewData(lProcurement.ContactProcurement);

                return View(lProcurement);
            }
        }
    }
}
