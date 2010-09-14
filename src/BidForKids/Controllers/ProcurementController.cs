using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BidsForKids.Models;
using BidsForKids.Models.SerializableObjects;
using System.Collections.Specialized;

namespace BidsForKids.Controllers
{

    public class ProcurementController : BidsForKidsControllerBase
    {
        public ProcurementController() { }

        public ProcurementController(IProcurementRepository factory)
        {
            this.factory = factory;
        }

        public ActionResult ProcurementList(int Year)
        {
            ViewData["Year"] = Year.ToString();
            return View(factory.GetProcurements(Year));
        }

        private void SetupIndex(string procurementType)
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
            ViewData["ProcurementType"] = procurementType;

            if (string.IsNullOrEmpty(procurementType) == false)
            {
                ViewData["ProcurementCreateLink"] = Url.Action("CreateByType", new { id = procurementType });
            }
            else
            {
                ViewData["ProcurementCreateLink"] = Url.Action("Create");
            }

        }

        //
        // GET: /Procurement/

        public ActionResult Index()
        {
            SetupIndex(null);
            return View();
        }

        public ActionResult BusinessIndex()
        {
            SetupIndex("Business");
            return View("Index");
        }

        public ActionResult ParentIndex()
        {
            SetupIndex("Parent");
            return View("Index");
        }

        public ActionResult AdventureIndex()
        {
            SetupIndex("Adventure");
            return View("Index");
        }


        private void GetCategoryJSONString()
        {
            var lCategoryString = "{ \"\": \"\",";

            foreach (var lCategory in factory.GetCategories())
                lCategoryString += String.Format("{0}:'{1}',", lCategory.Category_ID, lCategory.CategoryName);

            lCategoryString = lCategoryString.TrimEnd(new[] { ',' }) + "}";

            ViewData["CategoryJsonString"] = lCategoryString;
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetProcurement(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id", "id cannot be null");
            }

            var lProcurement = SerializableProcurement.
                ConvertProcurementToSerializableProcurement(factory.GetProcurement(id.Value));

            return Json(lProcurement, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetProcurements(string _search, string nd, int page, int rows, string sidx, string sord)
        public ActionResult GetProcurements(string id)
        {
            var loadOptions = jqGridLoadOptions.GetLoadOptions(Request.QueryString);

            var lResult = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            // TODO: Should not have to fetch all of the rows here, need to refactor
            List<SerializableProcurement> lRows = factory.GetSerializableProcurements(loadOptions);

            if (loadOptions.sortIndex == null)
                lRows = lRows.OrderByDescending(x => x.CreatedOn).ToList<SerializableProcurement>();

            if (string.IsNullOrEmpty(id) == false)
            {
                ProcurementType procurementType = factory.GetProcurementTypeByName(id);
                lRows = lRows.Where(x => x.ProcurementType_ID == procurementType.ProcurementType_ID).ToList<SerializableProcurement>();
            }

            var lTotalRows = lRows.Count;

            lRows = lRows.Skip((loadOptions.page - 1) * loadOptions.rows).Take(loadOptions.rows).ToList();

            var lTotalPages = (int)Math.Ceiling((decimal)lTotalRows / (decimal)loadOptions.rows);

            lResult.Data = new { total = lTotalPages, page = loadOptions.page, records = lTotalRows.ToString(), rows = lRows };

            return lResult;
        }

        public ActionResult Deleted()
        {
            return null;
        }

        //
        // GET: /Procurement/Delete/5

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id.HasValue == false)
                {
                    throw new ArgumentException("id did not have a value", "id");
                }

                bool lResult = factory.DeleteProcurement((int)id);

                ContentResult contentResult = new ContentResult();
                contentResult.Content = lResult.ToString();

                return contentResult;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                ContentResult contentResult = new ContentResult();
                contentResult.Content = ex.Message;

                return contentResult;
            }
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
            SetupCreateViewData(null);

            return View();
        }

        //
        // GET: /Procurement/Create

        public ActionResult CreateByType(string id)
        {
            SetupCreateViewData(id);

            return View("Create");
        }

        private void SetupCreateViewData(string createType)
        {
            ViewData["Auction_ID"] = GetAuctionSelectList(null);

            var procurementType = factory.GetProcurementTypeByName(createType);

            if (Request != null && string.IsNullOrEmpty(Request.QueryString["Donor_ID"]) == false)
                ViewData["Donor_ID"] = GetDonorsSelectList(int.Parse(Request.QueryString["Donor_ID"].ToString()), procurementType.DonorType_ID);
            else
                ViewData["Donor_ID"] = GetDonorsSelectList(null, procurementType.DonorType_ID);

            ViewData["Category_ID"] = GetCategoriesSelectList(null);
            ViewData["Procurer_ID"] = GetProcurerSelectList(null);
            ViewData["CertificateOptions"] = GetCertificateSelectListItems();
            ViewData["CreateType"] = createType ?? "";
            if (string.IsNullOrEmpty(createType) == false)
            {
                ViewData["ReturnToUrl"] = Server.UrlEncode(Url.Action("CreateByType", new { id = createType }));
                if (procurementType.DonorType.DonorTypeDesc == "Business")
                {
                    ViewData["CreateNewController"] = "Donor";
                }
                else if (procurementType.DonorType.DonorTypeDesc == "Parent")
                {
                    ViewData["CreateNewController"] = "Parent";
                }
            }
            else
            {
                ViewData["ReturnToUrl"] = Server.UrlEncode(Url.Action("Create"));
                ViewData["CreateNewController"] = "Donor";
            }
        }

        private static List<SelectListItem> GetCertificateSelectListItems()
        {
            List<SelectListItem> certOptions = new List<SelectListItem>();
            certOptions.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            certOptions.Add(new SelectListItem() { Text = "Create", Value = "Create" });
            certOptions.Add(new SelectListItem() { Text = "Provided", Value = "Provided" });
            return certOptions;
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
                ViewData["Donor_ID"] = GetDonorsSelectList(lContactId, contactProcurement.Procurement.ProcurementType.DonorType_ID);
            }
            else
                ViewData["Donor_ID"] = GetDonorsSelectList(lContactId, null);

            ViewData["Auction_ID"] = GetAuctionSelectList(lAuctionId);
            ViewData["Category_ID"] = GetCategoriesSelectList(lCategoryId);
            ViewData["Procurer_ID"] = GetProcurerSelectList(lProcurerID);
            ViewData["CertificateOptions"] = GetCertificateSelectListItems();
        }



        private SelectList GetAuctionSelectList(int? selectedValue)
        {
            return new SelectList(factory.GetAuctions().OrderByDescending(x => x.Year), "Auction_ID", "Year", selectedValue);
        }



        private SelectList GetDonorsSelectList(int? selectedValue, int? donorTypeID)
        {
            IEnumerable<Donor> lDonors = factory.GetDonors();

            if (donorTypeID != null && donorTypeID != 0)
            {
                DonorType donorType = factory.GetDonorTypeByID(donorTypeID.Value);

                // TODO: Refactor the logic to build a Business / Parent select list
                if (donorType.DonorTypeDesc == "Business")
                {
                    var lBusinesses = from D in lDonors
                                      where D.DonorType_ID == donorType.DonorType_ID
                                      orderby D.BusinessName
                                      select new
                                      {
                                          BusinessName = D.BusinessName,
                                          Donor_ID = D.Donor_ID
                                      };
                    return new SelectList(lBusinesses, "Donor_ID", "BusinessName", selectedValue);
                }
                else if (donorType.DonorTypeDesc == "Parent")
                {
                    var lParents = from D in lDonors
                                   where D.DonorType_ID == donorType.DonorType_ID
                                   orderby D.FirstName, D.LastName
                                   select new
                                   {
                                       FirstName = D.FirstName,
                                       LastName = D.LastName,
                                       Donor_ID = D.Donor_ID,
                                       FullName = D.FirstName + " " + D.LastName
                                   };
                    return new SelectList(lParents, "Donor_ID", "FullName", selectedValue);
                }
            }
            return new SelectList(lDonors.OrderBy(x => x.BusinessName), "Donor_ID", "BusinessName", selectedValue);
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
            return CreateNewProcurement(collection);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateByType(FormCollection collection)
        {
            return CreateNewProcurement(collection);
        }

        private ActionResult CreateNewProcurement(FormCollection collection)
        {
            SetupCreateViewData(collection["ProcurementType"]);

            try
            {
                Procurement lNewProcurement = factory.GetNewProcurement();
                ContactProcurement lNewContactProcurement = new ContactProcurement();
                lNewProcurement.ContactProcurement = lNewContactProcurement;

                UpdateModel<Procurement>(lNewProcurement,
                    ProcurementColumns());

                UpdateModel<ContactProcurement>(lNewProcurement.ContactProcurement,
                    ContactProcurementColumns());

                string actionToRedirectTo = "";

                if (collection["procurementType"] != null)
                {
                    ProcurementType procurementType = factory.GetProcurementTypeByName(collection["procurementType"]);
                    lNewProcurement.ProcurementType_ID = procurementType.ProcurementType_ID;
                    actionToRedirectTo = collection["procurementType"];
                }

                int lNewProcurementID = factory.AddProcurement(lNewProcurement);

                return RedirectToAction(actionToRedirectTo + "Index");
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

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
                return this.JavaScript("alert('Error saving. Please try again or refresh the page and try again.');");
                throw new ArgumentException("Invalid Procurement ID was passed.");
            }

            try
            {
                Procurement lProcurement = factory.GetProcurement((int)id);

                SetupEditViewData(lProcurement.ContactProcurement);

                UpdateModel<Procurement>(lProcurement,
                    ProcurementColumns());

                UpdateModel<ContactProcurement>(lProcurement.ContactProcurement,
                    ContactProcurementColumns());

                if (factory.SaveProcurement(lProcurement) == false)
                {
                    return this.JavaScript("alert('Error saving.');");
                    throw new ApplicationException("Unable to save procurement");
                }

                return this.JavaScript("alert('Saved.');");
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

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


            var lProcurement = factory.GetProcurement((int)id);

            if (lProcurement.Procurement_ID != id)
            {
                throw new ApplicationException("Unable to load procurement from database for editing by id " + id.ToString());
            }

            SetupEditViewData(lProcurement.ContactProcurement);

            UpdateModel<Procurement>(lProcurement,
                ProcurementColumns());

            UpdateModel<ContactProcurement>(lProcurement.ContactProcurement,
                ContactProcurementColumns());

            if (factory.SaveProcurement(lProcurement) == false)
            {
                throw new ApplicationException("Unable to save procurement");
            }

            string actionToRedirectTo = lProcurement.ProcurementType.ProcurementTypeDesc;

            return RedirectToAction(actionToRedirectTo + "Index");
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CheckItemNumber(int id, FormCollection collection)
        {
            try
            {
                ContentResult result = new ContentResult();

                string itemNumber = collection["itemNumber"];

                result.Content = factory.CheckForExistingItemNumber(id, itemNumber) == true
                                     ? itemNumber + " already exists in the database"
                                     : "false";

                return result;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                ContentResult errResult = new ContentResult();
                errResult.Content = "Error checking for item number: " + ex.Message;
                return errResult;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetLastItemNumber(int id, FormCollection collection)
        {
            ContentResult result = new ContentResult();

            string itemNumber = collection["itemNumber"];

            string lastSimilar = factory.CheckForLastSimilarItemNumber(id, itemNumber);

            result.Content =
                string.IsNullOrEmpty(lastSimilar) == false ?
                lastSimilar : string.Empty;

            return result;
        }

        private static string[] ProcurementColumns()
        {
            return new[] {
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
                        "Donation",
                        "ThankYouLetterSent",
                        "Limitations",
                        "Certificate"
                    };
        }

        private static string[] ContactProcurementColumns()
        {
            return new[] {
                        "Donor_ID",
                        "Auction_ID",
                        "Procurer_ID",
                        "GeoLocation_ID"
                    };
        }
    }
}
