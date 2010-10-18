using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BidsForKids.Data.Models;
using BidsForKids.Data.Models.SerializableObjects;
using BidsForKids.ViewModels;

namespace BidsForKids.Controllers
{

    public class ProcurementController : BidsForKidsControllerBase
    {
        public ProcurementController()
        {
            ProcurementDonorViewModel.CreateDestinationMaps();
            ProcurementViewModel.CreateDestinationMap();
        }

        public ProcurementController(IProcurementRepository factory)
        {
            this.factory = factory;
            ProcurementDonorViewModel.CreateDestinationMaps();
            ProcurementViewModel.CreateDestinationMap();
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
            ViewData["DonorDisplayField"] = "BusinessName";
            return View("Index");
        }

        public ActionResult ParentIndex()
        {
            SetupIndex("Parent");
            ViewData["DonorDisplayField"] = "Donors";
            return View("Index");
        }

        public ActionResult AdventureIndex()
        {
            SetupIndex("Adventure");
            return View("Index");
        }


        private void GetCategoryJSONString()
        {
            var categoryString = factory.GetCategories()
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

                var lResult = factory.DeleteProcurement((int)id);

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
            throw new NotSupportedException("Direct creation of procurements without a type is not supported.");
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
                //ContactId = contactProcurement.Donor_ID;
                categoryId = contactProcurement.Procurement.Category_ID;
                procurerId = contactProcurement.Procurer_ID;
                //ViewData["Donor_ID"] = GetDonorsSelectList(ContactId, contactProcurement.Procurement.ProcurementType.DonorType_ID);
                //ViewData["Donors"] = GetDonorsSelectList(null, contactProcurement.Procurement.ProcurementType.DonorType_ID);
            }
            else
                ViewData["Donors"] = GetDonorsSelectList(contactId, null);

            ViewData["Auction_ID"] = GetAuctionSelectList(auctionId);
            ViewData["Category_ID"] = GetCategoriesSelectList(categoryId);
            ViewData["Procurer_ID"] = GetProcurerSelectList(procurerId);
            ViewData["CertificateOptions"] = GetCertificateSelectListItems();
        }



        private SelectList GetAuctionSelectList(int? selectedValue)
        {
            return new SelectList(factory.GetAuctions().OrderByDescending(x => x.Year), "Auction_ID", "Year", selectedValue);
        }



        private SelectList GetDonorsSelectList(int? selectedValue, int? donorTypeID)
        {
            var donors = factory.GetDonors();

            if (donorTypeID != null && donorTypeID != 0)
            {
                var donorType = factory.GetDonorTypeByID(donorTypeID.Value);

                // TODO: Refactor the logic to build a Business / Parent select list
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
            var categories = factory.GetCategories();
            return new SelectList(categories.OrderBy(x => x.CategoryName), "Category_ID", "CategoryName", selectedValue);
        }

        private SelectList GetProcurerSelectList(int? selectedValue)
        {
            var lProcurers = factory.GetProcurers();

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
                var newProcurement = factory.GetNewProcurement();
                var newContactProcurement = new ContactProcurement();
                newProcurement.ContactProcurement = newContactProcurement;

                UpdateModel(newProcurement,
                    ProcurementColumns());

                UpdateModel(newProcurement.ContactProcurement,
                    ContactProcurementColumns());

                newProcurement.ItemNumber = collection["ItemNumberPrefix"] + " - " + collection["ItemNumber"];

                SetupProcurementDonors(newProcurement, collection);

                var actionToRedirectTo = "";

                if (collection["procurementType"] != null)
                {
                    var procurementType = factory.GetProcurementTypeByName(collection["procurementType"]);
                    newProcurement.ProcurementType_ID = procurementType.ProcurementType_ID;
                    actionToRedirectTo = collection["procurementType"];
                }

                factory.AddProcurement(newProcurement);

                return RedirectToAction(actionToRedirectTo + "Index");
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

            var procurement = factory.GetProcurement((int)id);

            SetupEditViewData(procurement.ContactProcurement);

            var result = Mapper.Map<Procurement, ProcurementViewModel>(procurement);

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
                throw new ArgumentException("Invalid Procurement ID was passed.");
            }

            try
            {
                var procurement = factory.GetProcurement((int)id);

                SetupEditViewData(procurement.ContactProcurement);

                UpdateModel<Procurement>(procurement,
                    ProcurementColumns());

                UpdateModel<ContactProcurement>(procurement.ContactProcurement,
                    ContactProcurementColumns());

                if (factory.SaveProcurement(procurement) == false)
                {
                    return this.JavaScript("alert('Error saving.');");
                    throw new ApplicationException("Unable to save procurement");
                }

                return this.JavaScript("alert('Saved.');");
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                var procurement = factory.GetProcurement((int)id);

                SetupEditViewData(procurement.ContactProcurement);

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


            var procurement = factory.GetProcurement(id.Value);

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

            if (factory.SaveProcurement(procurement) == false)
            {
                throw new ApplicationException("Unable to save procurement");
            }

            var actionToRedirectTo = procurement.ProcurementType.ProcurementTypeDesc;

            return RedirectToAction(actionToRedirectTo + "Index");
        }


        private void UpdateProcurementDonors(Procurement procurement, FormCollection collection)
        {
            var donors = GetDonorsFromFormCollection(collection, "DonorId");

            if (donors == null) return;

            var newDonors = donors.Where(x => procurement.ProcurementDonors.Count(y => y.Donor_ID.ToString() == x ) == 0).ToList();

            var removedDonors = procurement.ProcurementDonors.Where(x => !donors.Contains(x.Donor_ID.ToString())).ToList();

            newDonors.ForEach(id => procurement.ProcurementDonors.Add(new ProcurementDonor
                                                                          {
                                                                              Donor       = factory.GetDonor(int.Parse(id)),
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
                                                                            Donor = factory.GetDonor(int.Parse(id)),
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
                                     Content = factory.ItemNumberExists(id, itemNumber)
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
            var lastSimilar = factory.CheckForLastSimilarItemNumber(id, itemNumberPrefix, auctionId);

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
