using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using BidsForKids.Data.Models;
using BidsForKids.ViewModels;
using Simple.Data;

namespace BidsForKids.Controllers
{
    [Authorize(Roles = "Administrator, Procurements")]
    public class DonorController : BidsForKidsControllerBase
    {
        readonly dynamic db;

        public DonorController()
        {
            db = Database.Open();
        }

        public DonorController(IProcurementRepository repository)
        {
            db = Database.Open();
            Repository = repository;
        }

        public JsonNetResult GetDonors()
        {
            var loadOptions = jqGridLoadOptions.GetLoadOptions(Request.QueryString);

            var result = new JsonNetResult();

            var donorType = db.DonorType.FindByDonorTypeDesc("Business");

            //var typeExpression = new SimpleExpression(db.DonorType.DonorType_ID, donorType.DonorType_ID, SimpleExpressionType.Equal);
            var table = new ObjectReference("Donors", null);
            var typeExpression = new SimpleExpression(new ObjectReference("DonorType_ID", table), donorType.DonorType_ID, SimpleExpressionType.Equal);

            var compiled = typeExpression;

            if (loadOptions.search)
                loadOptions.searchParams.ToList().ForEach(param =>
                {
                    compiled = compiled & new SimpleExpression(new ObjectReference(param.Key, table), new SimpleFunction("like", new[] { "%" + param.Value + "%" }), SimpleExpressionType.Function);
                });

            //var values = new Dictionary<string, object> {{"DonorType_ID", donorType.DonorType_ID}};

            //if (loadOptions.search)
            //    loadOptions.searchParams.ToList().ForEach(param => values.Add(param.Key, param.Value));

            //var expression = Simple.Data.ExpressionHelper.CriteriaDictionaryToExpression("Donors", values);

            Promise<int> count;
            List<dynamic> businesses = db.Donors
                                         .FindAll(compiled)
                                         .WithTotalCount(out count)
                                         .Skip((loadOptions.page - 1) * loadOptions.rows)
                                         .Take(loadOptions.rows)
                                         .OrderBy(SortColumn(loadOptions), SortOrder(loadOptions))
                                         .ToList();

            var pages = count == 0 ? 0 : (int)Math.Ceiling((decimal)count / (decimal)loadOptions.rows);

            result.Data = new { total = pages, loadOptions.page, records = count.Value.ToString(CultureInfo.InvariantCulture), rows = businesses };

            return result;
        }

        static OrderByDirection SortOrder(jqGridLoadOptions loadOptions)
        {
            return loadOptions.sortOrder == "asc" ? OrderByDirection.Ascending : OrderByDirection.Descending;
        }

        static ObjectReference SortColumn(jqGridLoadOptions loadOptions)
        {
            return loadOptions.sortIndex != null ? ObjectReference.FromString(loadOptions.sortIndex) : ObjectReference.FromString("BusinessName");
        }


        public ActionResult Index()
        {
            ViewData["GeoLocationJsonString"] = GetGeoLocationJSONString();

            ViewData["ProcurerJsonString"] = GetProcurerJSONString();

            return View(Repository.GetDonors());
        }


        private string GetGeoLocationJSONString()
        {
            var geoLocations = Repository.GetGeoLocations();

            var locationString = "{ \"\": \"\",";

            foreach (var lGeoLocation in geoLocations)
                locationString += String.Format("{0}:'{1}',", lGeoLocation.GeoLocation_ID,
                                                    lGeoLocation.GeoLocationName);

            locationString = locationString.TrimEnd(new[] { ',' }) + "}";
            return locationString;
        }

        private string GetProcurerJSONString()
        {
            var procurers = Repository.GetProcurers();

            var procurerString = "{ \"\": \"\",";

            foreach (var lProcurer in procurers)
                procurerString += string.Format("{0}:'{1}',", lProcurer.Procurer_ID,
                                                 lProcurer.FirstName + " " + lProcurer.LastName);

            procurerString = procurerString.TrimEnd(new[] { ',' }) + "}";
            return procurerString;
        }

        public ActionResult GeoList(int? id)
        {
            if (id.HasValue && id > 0)
                return View(Repository.GetDonors()
                    .Where(x => x.GeoLocation_ID == (id > 0 ? id : null))
                                      .OrderBy(x => x.BusinessName));

            return View(Repository.GetDonors().OrderBy(x => x.BusinessName));
        }

        public ActionResult Details(int id)
        {
            return View(Repository.GetDonor(id));
        }

        public ActionResult Create()
        {
            SetupCreateViewData();

            return View();
        }

        private void SetupCreateViewData()
        {
            if (string.IsNullOrEmpty(Request.QueryString["GeoLocation_ID"]) == false)
                ViewData["GeoLocation_ID"] = GetGeoLocationsSelectList(int.Parse(Request.QueryString["GeoLocation_ID"]));
            else
                ViewData["GeoLocation_ID"] = GetGeoLocationsSelectList(null);

            if (string.IsNullOrEmpty(Request.QueryString["Procurer_ID"]) == false)
                ViewData["Procurer_ID"] = GetProcurerSelectList(int.Parse(Request.QueryString["Procurer_ID"]));
            else
                ViewData["Procurer_ID"] = GetProcurerSelectList(null);

            ViewData["Donates"] = GetDonatesSelectList(null);
        }

        private void SetupEditViewData(Donor donor)
        {
            ViewData["GeoLocation_ID"] = GetGeoLocationsSelectList(donor.GeoLocation_ID);
            ViewData["Procurer_ID"] = GetProcurerSelectList(donor.Procurer_ID);
            ViewData["Donates"] = GetDonatesSelectList(donor.Donates);
        }


        [HttpPost]
        public ActionResult Create(DonorViewModel model)
        {
            try
            {
                SetupCreateViewData();

                if (!ModelState.IsValid)
                    return View();

                model.DonorType_ID = db.DonorTypes.FindByDonorTypeDesc("Business").DonorType_ID;

                var donor = db.Donors.Insert(model);

                return ControllerHelper.ReturnToOrRedirectToIndex(this, donor.Donor_ID, "Donor_ID");
            }
            catch
            {
                return View();
            }
        }

        private SelectList GetGeoLocationsSelectList(int? selectedValue)
        {
            IEnumerable<GeoLocation> geoLocations = Repository.GetGeoLocations();
            return new SelectList(geoLocations.OrderBy(x => x.GeoLocationName), "GeoLocation_ID", "GeoLocationName", selectedValue);
        }

        private SelectList GetDonatesSelectList(int? selectedValue)
        {
            IEnumerable<DonatesReference> donatesRef = Repository.GetDonatesReferenceList();
            return donatesRef != null ?
                new SelectList(donatesRef, "Donates_ID", "Description", selectedValue ?? 2) :
                new SelectList(new List<DonatesReference>(), "Donates_ID", "Description");
        }

        private SelectList GetProcurerSelectList(int? selectedValue)
        {
            IEnumerable<Procurer> procurers = Repository.GetProcurers();
            var procurerQuery = from p in procurers
                                select new
                                {
                                    p.Procurer_ID,
                                    FullName = p.FirstName + " " + p.LastName
                                };

            return new SelectList(procurerQuery, "Procurer_ID", "FullName", selectedValue);
        }


        public ActionResult Edit(int id)
        {
            var donor = Repository.GetDonor(id);

            SetupEditViewData(donor);

            return View(Repository.GetDonor(id));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AjaxEdit(int id, FormCollection collection)
        {
            try
            {
                var donor = Repository.GetDonor(id);

                SetupEditViewData(donor);

                UpdateModel(donor, new[] {
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
                    "GeoLocation_ID",
                    "Donates",
                    "Email",
                    "Website",
                    "MailedPacket",
                    "Procurer_ID"
                });

                if (Repository.SaveDonor(donor) == false)
                {
                    throw new ApplicationException("Unable to save Donor");
                }

                return Content("");
            }
            catch
            {
                throw new ApplicationException("Unable to save Donor");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var donor = Repository.GetDonor(id);

                SetupEditViewData(donor);

                UpdateModel(donor, new[] {
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
                    "GeoLocation_ID",
                    "Donates",
                    "Email",
                    "Website",
                    "MailedPacket",
                    "Procurer_ID"
                });

                if (Repository.SaveDonor(donor) == false)
                {
                    throw new ApplicationException("Unable to save changes to Business");
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(Repository.GetDonor(id));
            }
        }

        [HttpPost]
        public ActionResult Close(int id)
        {
            var donor = Repository.GetDonor(id);

            donor.Closed = true;

            Repository.SaveDonor(donor);

            return new JsonResult { Data = new CloseDonorViewModel { Successful = true } };
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var donor = Repository.GetDonor(id);

            if (donor.ProcurementDonors.Count > 0)
                return new JsonResult
                {
                    Data = new DeleteDonorViewModel
                               {
                                   Successful = false,
                                   Message = "Cannot delete because the donor has associated procurements, use Close Donor instead"
                               }
                };

            Repository.DeleteDonor(donor);

            return new JsonResult { Data = new DeleteDonorViewModel { Successful = true } };
        }
    }

    public class DeleteDonorViewModel
    {
        public bool Successful { get; set; }
        public string Message { get; set; }
    }

    public class CloseDonorViewModel
    {
        public bool Successful { get; set; }
    }
}
