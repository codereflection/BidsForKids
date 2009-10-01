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
            ViewData["Auctions"] = GetAuctionSelectList(null);
            ViewData["Contacts"] = GetContactsSelectList(null);
        }

        private void SetupEditViewData(ContactProcurement contactProcurement)
        {
            int? lAuctionId = null;
            int? lContactId = null;

            if (contactProcurement != null)
            {
                lAuctionId = contactProcurement.Auction_ID;
                lContactId = contactProcurement.Contact_ID;
            }

            ViewData["Auctions"] = GetAuctionSelectList(lAuctionId);
            ViewData["Contacts"] = GetContactsSelectList(lContactId);
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

        //
        // POST: /Procurement/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            SetupCreateViewData();

            try
            {
                Procurement lNewProcurement = factory.GetNewProcurement();
                Auction lAuction = factory.GetAuction(int.Parse(collection["Auctions"]));
                Contact lContact = factory.GetContact(int.Parse(collection["Contacts"]));

                SetProcurementValues(collection, lNewProcurement, lAuction, lContact);

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

            SetupEditViewData(lProcurement.ContactProcurements);

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

                SetupEditViewData(lProcurement.ContactProcurements);

                Auction lAuction = factory.GetAuction(int.Parse(collection["Auctions"]));
                Contact lContact = factory.GetContact(int.Parse(collection["Contacts"]));

                SetProcurementValues(collection, lProcurement, lAuction, lContact);

                if (factory.SaveProcurement(lProcurement) == false)
                {
                    throw new ApplicationException("Unable to save procurement");
                }

                return RedirectToAction("Index");
            }
            catch
            {
                Procurement lProcurement = factory.GetProcurement((int)id);

                SetupEditViewData(lProcurement.ContactProcurements);

                return View(lProcurement);
            }
        }

        private static void SetProcurementValues(FormCollection collection, Procurement procurement, Auction auction, Contact contact)
        {
            procurement.Code = collection["Code"];
            procurement.Description = collection["Description"];
            procurement.Notes = collection["Notes"];
            procurement.PerItemValue = decimal.Parse(collection["PerItemValue"].ToString());
            procurement.Quantity = double.Parse(collection["Quantity"].ToString());
            if (procurement.ContactProcurements == null)
            {
                procurement.ContactProcurements = new ContactProcurement() { Auction = auction, Contact = contact };
            }
            else
            {
                if (auction != null)
                    procurement.ContactProcurements.Auction = auction;
                if (contact != null)
                    procurement.ContactProcurements.Contact = contact;
            }
        }
    }
}
