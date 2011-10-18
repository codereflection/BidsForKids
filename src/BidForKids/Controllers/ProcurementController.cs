using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BidsForKids.Data.Models;
using BidsForKids.ViewModels;

namespace BidsForKids.Controllers
{
    [Authorize(Roles = "Administrator, Procurements")]
    public class ProcurementController : BidsForKidsControllerBase
    {
        private static void SetupMaps()
        {
            ProcurementDonorViewModel.CreateDestinationMaps();
            ProcurementDetailsViewModel.CreateDestinationMap();
            EditableProcurementViewModel.CreateDestinationMap();
        }
        
        public ProcurementController()
        {
            SetupMaps();
        }

        public ProcurementController(IProcurementRepository repository)
        {
            this.Repository = repository;
            SetupMaps();
        }

        public ActionResult ProcurementList(int year)
        {
            ViewData["Year"] = year.ToString();
            return View(Repository.GetProcurements(year));
        }

        private void SetupIndex(string procurementType)
        {
            SetCategoryViewData();

            ViewData["Auction_ID"] = GetAuctionSelectList(null);
            var auctions = Repository.GetAuctions();
            if (auctions != null && auctions.Count > 0)
            {
                var auction = auctions.OrderByDescending(x => x.Year).First();
                ViewData["DefaultSearchYear"] = auction.Year.ToString();
            }
            else
            {
                ViewData["DefaultSearchYear"] = DateTime.Today.Year.ToString();
            }
            ViewData["ProcurementType"] = procurementType;

            if (string.IsNullOrEmpty(procurementType) == false)
            {
                ViewData["ProcurementCreateLink"] = Url.Action("CreateByType", 
                                                               new { id = procurementType });
            }
            else
            {
                ViewData["ProcurementCreateLink"] = Url.Action("Create");
            }

        }

        public ActionResult Index()
        {
            SetupIndex(null);
            return View();
        }

        public ActionResult Business()
        {
            SetupIndex("Business");
            SetViewDataDonorDisplayField("Business");
            return View("Index");
        }

        public ActionResult Parent()
        {
            SetupIndex("Parent");
            SetViewDataDonorDisplayField("Parents");
            return View("Index");
        }

        public ActionResult Adventure()
        {
            SetupIndex("Adventure");
            SetViewDataDonorDisplayField("Adventure");
            return View("Index");
        }

        private void SetCategoryViewData()
        {
            var categoryString = Repository.GetCategories()
                                         .Aggregate("{ \"\": \"\",", (current, lCategory) => 
                                                                        current + String.Format("{0}:'{1}',", lCategory.Category_ID, lCategory.CategoryName));

            categoryString = categoryString.TrimEnd(new[] { ',' }) + "}";

            ViewData["CategoryJsonString"] = categoryString;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetProcurement(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id", "id cannot be null");
            }

            var result = Mapper.Map<Procurement, ProcurementDetailsViewModel>(Repository.GetProcurement(id.Value));

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private void SetViewDataDonorDisplayField(string procurementType)
        {
            string field;
            switch (procurementType)
            {
                case "Adventure":
                    field = "Donors";
                    break;
                case "Parents":
                    field = "Donors";
                    break;
                case "Business":
                    field = "BusinessName";
                    break;
                default:
                    field = string.Empty;
                    break;
            }
            ViewData["DonorDisplayField"] = field;
        }

        public ActionResult GetProcurements(string id)
        {
            var loadOptions = jqGridLoadOptions.GetLoadOptions(Request.QueryString);

            var result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            // FAIL: Should not have to fetch all of the rows here, need to refactor
            var rows = Repository.GetSerializableProcurements(loadOptions);

            if (loadOptions.sortIndex == null)
                rows = rows.OrderByDescending(x => x.CreatedOn).ToList();

            if (string.IsNullOrEmpty(id) == false)
            {
                var procurementType = Repository.GetProcurementTypeByName(id);
                rows = rows.Where(x => x.ProcurementType_ID == procurementType.ProcurementType_ID).ToList();
            }

            var totalRows = rows.Count;

            rows = rows.Skip((loadOptions.page - 1) * loadOptions.rows).Take(loadOptions.rows).ToList();

            var totalPages = (int)Math.Ceiling((decimal)totalRows / (decimal)loadOptions.rows);

            result.Data = new
                              {
                                  total = totalPages, 
                                  loadOptions.page, 
                                  records = totalRows.ToString(), 
                                  rows
                              };

            return result;
        }

        public ActionResult Deleted()
        {
            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id.HasValue == false)
                {
                    throw new ArgumentException("id did not have a value", "id");
                }

                var lResult = Repository.DeleteProcurement((int)id);

                var contentResult = new ContentResult {Content = lResult.ToString()};

                return contentResult;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                var contentResult = new ContentResult {Content = ex.Message};

                return contentResult;
            }
        }


        public ActionResult Details(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(Repository.GetProcurement((int)id));
        }

        public ActionResult Create()
        {
            throw new NotSupportedException("Direct creation of procurements without a type is not supported.");
        }

        public ActionResult CreateByType(string id)
        {
            SetupCreateViewData(id);

            return View("Create");
        }

        private void SetupCreateViewData(string createType)
        {
            ViewData["Auction_ID"] = GetAuctionSelectList(null);

            var procurementType = Repository.GetProcurementTypeByName(createType);

            if (Request != null && string.IsNullOrEmpty(Request.QueryString["Donor_ID"]) == false)
                ViewData["Donor_ID"] = GetDonorsSelectList(int.Parse(Request.QueryString["Donor_ID"].ToString()), procurementType.DonorType_ID);
            else
                ViewData["Donor_ID"] = GetDonorsSelectList(null, procurementType.DonorType_ID);

            ViewData["Category_ID"] = GetCategoriesSelectList(null);
            ViewData["Procurer_ID"] = GetProcurerSelectList(null);
            ViewData["CertificateOptions"] = GetCertificateSelectListItems();
            ViewData["CreateType"] = createType ?? "";
            ViewData["ItemNumberPrefixes"] = SetupItemNumberPrefixSelectListItems();
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
            var certOptions = new List<SelectListItem>
                                  {
                                      new SelectListItem() {Text = "", Value = "", Selected = true},
                                      new SelectListItem() {Text = "Create", Value = "Create"},
                                      new SelectListItem() {Text = "Provided", Value = "Provided"}
                                  };
            return certOptions;
        }

        private static List<SelectListItem> SetupItemNumberPrefixSelectListItems()
        {
            // FAIL: Refactor this stuff out to the database
            var result = new List<SelectListItem>
                                  {
                                        new SelectListItem() {Text = "des", Value = "des"},
                                        new SelectListItem() {Text = "din", Value = "din"},
                                        new SelectListItem() {Text = "ent", Value = "ent"},
                                        new SelectListItem() {Text = "fam", Value = "fam"},
                                        new SelectListItem() {Text = "hbb", Value = "hbb"},
                                        new SelectListItem() {Text = "hng", Value = "hng"},
                                        new SelectListItem() {Text = "mis", Value = "mis"},
                                        new SelectListItem() {Text = "prt", Value = "prt"},
                                        new SelectListItem() {Text = "spr", Value = "spr"},
                                        new SelectListItem() {Text = "srv", Value = "srv"},
                                        new SelectListItem() {Text = "vac", Value = "vac"}
                                  };
            return result;
        }

        private void SetupEditViewData(ContactProcurement contactProcurement)
        {
            int? auctionId = null;
            int? contactId = null;
            int? categoryId = null;
            int? procurerId = null;

            if (contactProcurement != null)
            {
                auctionId = contactProcurement.Auction_ID;
                categoryId = contactProcurement.Procurement.Category_ID;
                procurerId = contactProcurement.Procurer_ID;
            }
            else
                ViewData["Donors"] = GetDonorsSelectList(contactId, null);

            ViewData["Auction_ID"] = GetAuctionSelectList(auctionId);
            ViewData["Category_ID"] = GetCategoriesSelectList(categoryId);
            ViewData["Procurer_ID"] = GetProcurerSelectList(procurerId);
            ViewData["CertificateOptions"] = GetCertificateSelectListItems();
            ViewData["ItemNumberPrefixes"] = SetupItemNumberPrefixSelectListItems();
        }

        private SelectList GetAuctionSelectList(int? selectedValue)
        {
            return new SelectList(Repository.GetAuctions().OrderByDescending(x => x.Year), "Auction_ID", "Year", selectedValue);
        }

        private SelectList GetDonorsSelectList(int? selectedValue, int? donorTypeID)
        {
            var donors = Repository.GetDonors();

            if (donorTypeID != null && donorTypeID != 0)
            {
                var donorType = Repository.GetDonorTypeByID(donorTypeID.Value);

                // FAIL: Refactor the logic to build a Business / Parent select list
                switch (donorType.DonorTypeDesc)
                {
                    case "Business":
                        {
                            var businesses = from D in donors
                                             where D.DonorType_ID == donorType.DonorType_ID
                                             orderby D.BusinessName
                                             select new
                                                        {
                                                            D.BusinessName, 
                                                            D.Donor_ID
                                                        };
                            return new SelectList(businesses, "Donor_ID", "BusinessName", selectedValue);
                        }
                    case "Parent":
                        {
                            var parents = from D in donors
                                          where D.DonorType_ID == donorType.DonorType_ID
                                          orderby D.FirstName, D.LastName
                                          select new
                                                     {
                                                         D.FirstName,
                                                         D.LastName,
                                                         D.Donor_ID,
                                                         FullName = D.FirstName + " " + D.LastName
                                                     };
                            return new SelectList(parents, "Donor_ID", "FullName", selectedValue);
                        }
                }
            }
            return new SelectList(donors.OrderBy(x => x.BusinessName), "Donor_ID", "BusinessName", selectedValue);
        }

        private SelectList GetCategoriesSelectList(int? selectedValue)
        {
            var categories = Repository.GetCategories();
            return new SelectList(categories.OrderBy(x => x.CategoryName), "Category_ID", "CategoryName", selectedValue);
        }

        private SelectList GetProcurerSelectList(int? selectedValue)
        {
            var lProcurers = Repository.GetProcurers();

            var procurerList = from P in lProcurers
                                select new
                                {
                                    Procurer_ID = P.Procurer_ID,
                                    FirstName = P.FirstName,
                                    LastName = P.LastName,
                                    FullName = P.FirstName + " " + P.LastName
                                };

            return new SelectList(procurerList.OrderBy(x => x.LastName), "Procurer_ID", "FullName", selectedValue);
        }

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
                var newProcurement = Repository.GetNewProcurement();
                var newContactProcurement = new ContactProcurement();
                newProcurement.ContactProcurement = newContactProcurement;

                UpdateModel(newProcurement,
                    ProcurementColumns());

                UpdateModel(newProcurement.ContactProcurement,
                    ContactProcurementColumns());

                newProcurement.ItemNumber = collection["ItemNumberPrefix"] + " - " + collection["ItemNumberSuffix"];

                SetupProcurementDonors(newProcurement, collection);

                var actionToRedirectTo = "";

                if (collection["procurementType"] != null)
                {
                    var procurementType = Repository.GetProcurementTypeByName(collection["procurementType"]);
                    newProcurement.ProcurementType_ID = procurementType.ProcurementType_ID;
                    actionToRedirectTo = collection["procurementType"];
                }

                Repository.AddProcurement(newProcurement);

                return RedirectToAction(actionToRedirectTo);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                return View("CreateByType", new { id = collection["ProcurementType"] });
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }

            var procurement = Repository.GetProcurement((int)id);

            ViewData["RedirectToIndex"] = procurement.ProcurementType.ProcurementTypeDesc;

            SetupEditViewData(procurement.ContactProcurement);

            var result = Mapper.Map<Procurement, EditableProcurementViewModel>(procurement);

            foreach (var donor in result.Donors)
            {
                ViewData["Donor-" + donor.Id] = GetDonorsSelectList(donor.Id, procurement.ProcurementType.DonorType_ID);
            }

            return View(result);
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
            }

            try
            {
                var procurement = Repository.GetProcurement((int)id);

                SetupEditViewData(procurement.ContactProcurement);

                UpdateModel(procurement,
                    ProcurementColumns());

                UpdateModel(procurement.ContactProcurement,
                    ContactProcurementColumns());

                if (Repository.SaveProcurement(procurement) == false)
                {
                    return this.JavaScript("alert('Error saving.');");
                }

                return this.JavaScript("alert('Saved.');");
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                var procurement = Repository.GetProcurement((int)id);

                SetupEditViewData(procurement.ContactProcurement);

                return this.JavaScript("alert('Error saving.');");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int? id, FormCollection collection)
        {
            if (id.HasValue == false)
            {
                throw new ArgumentException("Invalid Procurement ID was passed.");
            }


            var procurement = Repository.GetProcurement(id.Value);

            if (procurement.Procurement_ID != id)
            {
                throw new ApplicationException("Unable to load procurement from database for editing by id " + id);
            }

            SetupEditViewData(procurement.ContactProcurement);

            UpdateModel(procurement,
                ProcurementColumns());

            UpdateModel(procurement.ContactProcurement,
                ContactProcurementColumns());

            UpdateProcurementDonors(procurement, collection);

            procurement.ItemNumber = collection["ItemNumberPrefix"] + " - " + collection["ItemNumberSuffix"];

            if (Repository.SaveProcurement(procurement) == false)
            {
                throw new ApplicationException("Unable to save procurement");
            }

            var actionToRedirectTo = procurement.ProcurementType.ProcurementTypeDesc;

            return RedirectToAction(actionToRedirectTo); 
        }
        private void UpdateProcurementDonors(Procurement procurement, FormCollection collection)
        {
            var donors = GetDonorsFromFormCollection(collection, "DonorId");

            if (donors == null) return;

            var newDonors = donors.Where(x => procurement.ProcurementDonors.Count(y => y.Donor_ID.ToString() == x ) == 0).ToList();

            var removedDonors = procurement.ProcurementDonors.Where(x => !donors.Contains(x.Donor_ID.ToString())).ToList();

            newDonors.ForEach(id => procurement.ProcurementDonors.Add(new ProcurementDonor
                                                                          {
                                                                              Donor       = Repository.GetDonor(int.Parse(id)),
                                                                              Procurement = procurement
                                                                          }));

            removedDonors.ForEach(donor => procurement.ProcurementDonors.Remove(donor));
        }

        private List<string> GetDonorsFromFormCollection(FormCollection collection, string DonorSelectFieldId)
        {
            return string.IsNullOrEmpty(collection[DonorSelectFieldId]) ? 
                                                                   null : 
                                                                            collection[DonorSelectFieldId]
                                                                                .Split(',')
                                                                                .Where(x => !string.IsNullOrEmpty(x))
                                                                                .ToList();
        }

        private void SetupProcurementDonors(Procurement procurement, FormCollection collection)
        {
            var donors = GetDonorsFromFormCollection(collection, "Donor_ID");

            if (donors == null) return;

            donors.ForEach(id => procurement.ProcurementDonors.Add(new ProcurementDonor
                                                                        {
                                                                            Donor = Repository.GetDonor(int.Parse(id)),
                                                                            Procurement = procurement
                                                                        }));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CheckItemNumber(int id, string itemNumber)
        {
            try
            {
                var result = new ContentResult
                                 {
                                     Content = Repository.ItemNumberExists(id, itemNumber)
                                                   ? itemNumber + " already exists in the database"
                                                   : "false"
                                 };

                return result;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                var errResult = new ContentResult
                                    {
                                        Content = "Error checking for item number: " + ex.Message
                                    };
                return errResult;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetLastItemNumber(int id, string itemNumberPrefix, int auctionId)
        {
            var lastSimilar = Repository.CheckForLastSimilarItemNumber(id, itemNumberPrefix, auctionId);

            var result = new ContentResult
                             {
                                 Content = string.IsNullOrEmpty(lastSimilar) == false
                                               ? lastSimilar
                                               : string.Empty
                             };

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
                        "Certificate",
                        "Title"
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
