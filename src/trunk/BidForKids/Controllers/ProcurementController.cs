using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BidForKids.Models;
using BidForKids.Models.SerializableObjects;
using System.Collections.Specialized;

namespace BidForKids.Controllers
{

    public class ProcurementController : Controller
    {
        private IProcurementFactory factory;

        public ProcurementController(IProcurementFactory factory)
        {
            this.factory = factory;
        }

        public ActionResult ProcurementList(int Year)
        {
            ViewData["Year"] = Year.ToString();
            return View(factory.GetProcurements(Year));
        }

        //
        // GET: /Procurement/GridIndex

        //
        // GET: /Procurement/

        public ActionResult Index()
        {
            GetCategoryJSONString();

            ViewData["Auction_ID"] = GetAuctionSelectList(null);
            var lAuctionQuery = factory.GetAuctions();
            if (lAuctionQuery != null && lAuctionQuery.Count > 0)
            {
                Auction lAuction = lAuctionQuery.OrderByDescending(x => x.Year).First();
                ViewData["DefaultSearchYear"] = lAuction.Year.ToString();
            }
            else
            {
                ViewData["DefaultSearchYear"] = DateTime.Today.Year.ToString();
            }
            return View(factory.GetProcurements());
        }

        private void GetCategoryJSONString()
        {
            var lCategoryString = "{";

            foreach (var lCategory in factory.GetCategories())
            {
                lCategoryString += String.Format("{0}:'{1}',", lCategory.Category_ID, lCategory.CategoryName);
            }
            lCategoryString = lCategoryString.TrimEnd(new[] { ',' }) + "}";

            ViewData["CategoryJsonString"] = lCategoryString;
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetProcurement(int? id)
        {
            SerializableProcurement lProcurement = SerializableProcurement.ConvertProcurementToSerializableProcurement(factory.GetProcurement((int)id));

            return Json(lProcurement, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetProcurements(string _search, string nd, int page, int rows, string sidx, string sord)
        public ActionResult GetProcurements()
        {
            jqGridLoadOptions loadOptions = jqGridLoadOptions.GetLoadOptions(Request.QueryString);


            JsonResult lResult = new JsonResult();
            lResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            List<SerializableProcurement> lRows = factory.GetSerializableProcurements(loadOptions);

            int lTotalRows = lRows.Count;

            lRows = lRows.Skip((loadOptions.page - 1) * loadOptions.rows).Take(loadOptions.rows).ToList();

            int lTotalPages = (int)Math.Ceiling((decimal)lTotalRows / (decimal)loadOptions.rows);

            lResult.Data = new { total = lTotalPages, page = loadOptions.page, records = lTotalRows.ToString(), rows = lRows };

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

            if (Request != null && string.IsNullOrEmpty(Request.QueryString["Donor_ID"]) == false)
                ViewData["Donor_ID"] = GetContactsSelectList(int.Parse(Request.QueryString["Donor_ID"].ToString()));
            else
                ViewData["Donor_ID"] = GetContactsSelectList(null);

            ViewData["Category_ID"] = GetCategoriesSelectList(null);
        }

        private void SetupEditViewData(ContactProcurement contactProcurement)
        {
            int? lAuctionId = null;
            int? lContactId = null;
            int? lCategoryId = null;
            int? lProcurerID = null;

            if (contactProcurement != null)
            {
                lAuctionId = contactProcurement.Auction_ID;
                lContactId = contactProcurement.Donor_ID;
                lCategoryId = contactProcurement.Procurement.Category_ID;
                lProcurerID = contactProcurement.Procurer_ID;
            }

            ViewData["Auction_ID"] = GetAuctionSelectList(lAuctionId);
            ViewData["Donor_ID"] = GetContactsSelectList(lContactId);
            ViewData["Category_ID"] = GetCategoriesSelectList(lCategoryId);
            ViewData["Procurer_ID"] = GetProcurerSelectList(lProcurerID);
        }

        private SelectList GetAuctionSelectList(int? selectedValue)
        {
            return new SelectList(factory.GetAuctions().OrderByDescending(x => x.Year), "Auction_ID", "Year", selectedValue);
        }

        private SelectList GetContactsSelectList(int? selectedValue)
        {
            IEnumerable<Donor> lContacts = factory.GetDonors();
            return new SelectList(lContacts.OrderBy(x => x.BusinessName), "Donor_ID", "BusinessName", selectedValue);
        }


        private SelectList GetCategoriesSelectList(int? selectedValue)
        {
            IEnumerable<Category> lCategories = factory.GetCategories();
            return new SelectList(lCategories.OrderBy(x => x.CategoryName), "Category_ID", "CategoryName", selectedValue);
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
                        "Category_ID"
                    });

                UpdateModel<ContactProcurement>(lNewProcurement.ContactProcurement,
                    new[] {
                        "Donor_ID",
                        "Auction_ID",
                        "Procurer_ID",
                        "GeoLocation_ID"
                    });

                int lNewProcurementID = factory.AddProcurement(lNewProcurement);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public Action Edit()
        //{
        //    NameValueCollection collection = Request.Form;

        //    return null;
        //}

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

        public ActionResult CategorySelectList()
        {

            ViewData["Category_ID"] = GetCategoriesSelectList(null);

            return PartialView();
        }

        public ActionResult AjaxEdit(int? id, FormCollection collection)
        {
            if (id.HasValue == false)
            {
                return this.JavaScript("alert('Error saving.');");
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
                        "Category_ID"
                    });

                UpdateModel<ContactProcurement>(lProcurement.ContactProcurement,
                    new[] {
                        "Donor_ID",
                        "Auction_ID",
                        "Procurer_ID",
                        "GeoLocation_ID"
                    });

                if (factory.SaveProcurement(lProcurement) == false)
                {
                    return this.JavaScript("alert('Error saving.');");
                    throw new ApplicationException("Unable to save procurement");
                }

                return this.JavaScript("alert('Saved.');");
            }
            catch
            {
                Procurement lProcurement = factory.GetProcurement((int)id);

                SetupEditViewData(lProcurement.ContactProcurement);

                return this.JavaScript("alert('Error saving.');");
                //return View(lProcurement);
            }
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
                        "Category_ID"
                    });

                UpdateModel<ContactProcurement>(lProcurement.ContactProcurement,
                    new[] {
                        "Donor_ID",
                        "Auction_ID",
                        "Procurer_ID",
                        "GeoLocation_ID"
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
