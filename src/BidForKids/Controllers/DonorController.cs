using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BidsForKids.Data.Models;
using BidsForKids.Data.Models.SerializableObjects;
using Simple.Data;

namespace BidsForKids.Controllers
{

    [Authorize(Roles = "Administrator, Procurements")]
    public class DonorController : BidsForKidsControllerBase
    {
        dynamic db;

        public DonorController()
        {
            db = Database.Open();
        }

        public DonorController(IProcurementRepository repository)
        {
            db = Database.Open();
            this.Repository = repository;
        }

        public JsonResult GetDonors()
        {
            var loadOptions = jqGridLoadOptions.GetLoadOptions(Request.QueryString);

            var result = new JsonResult
                              {
                                  JsonRequestBehavior = JsonRequestBehavior.AllowGet
                              };

            var donorType = db.DonorType.FindByDonorTypeDesc("Business");

            Promise<int> count;

            List<dynamic> businesses = db.Donors
                                         .FindAll(db.Donors.DonorType_Id == donorType.DonorType_ID)
                                         .WithTotalCount(out count)
                                         .Skip((loadOptions.page - 1) * loadOptions.rows)
                                         .Take(loadOptions.rows)
                                         .ToList();

            //var businesses = Repository.GetSerializableBusinesses(loadOptions);

            //businesses = businesses.Skip((loadOptions.page - 1) * loadOptions.rows).Take(loadOptions.rows).ToList();

            var pages = count == 0 ? 0 : (int)Math.Ceiling((decimal)count / (decimal)loadOptions.rows);

            result.Data = new { total = pages, page = loadOptions.page, records = count.Value.ToString(), rows = businesses };

            return result;
        }


        public ActionResult Index()
        {
            ViewData["GeoLocationJsonString"] = GetGeoLocationJSONString(); ;

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

        public ActionResult GridIndex()
        {
            return View();
        }

        public ActionResult GeoList(int? id)
        {
            if (id.HasValue == true && id > 0)
            {
                var donors = Repository.GetDonors().Where(x => x.GeoLocation_ID == id).OrderBy(x => x.BusinessName);

                return View(donors);
            }
            else if (id.HasValue == true && id == 0)
            {
                var donors = Repository.GetDonors().Where(x => x.GeoLocation_ID == null).OrderBy(x => x.BusinessName);

                return View(donors);
            }
            else
            {
                var donors = Repository.GetDonors().OrderBy(x => x.BusinessName);

                return View(donors);
            }
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
                ViewData["GeoLocation_ID"] = GetGeoLocationsSelectList(int.Parse(Request.QueryString["GeoLocation_ID"].ToString()));
            else
                ViewData["GeoLocation_ID"] = GetGeoLocationsSelectList(null);

            ViewData["Donates"] = GetDonatesSelectList(null);
            ViewData["Procurer_ID"] = GetProcurerSelectList(null);
        }

        private void SetupEditViewData(Donor donor)
        {
            ViewData["GeoLocation_ID"] = GetGeoLocationsSelectList(donor.GeoLocation_ID);
            ViewData["Procurer_ID"] = GetProcurerSelectList(donor.Procurer_ID);
            ViewData["Donates"] = GetDonatesSelectList(donor.Donates);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                SetupCreateViewData();

                var newDonor = Repository.GetNewDonor();

                UpdateModel<Donor>(newDonor, new[] {
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

                newDonor.DonorType_ID = Repository.GetDonorTypeByName("Business").DonorType_ID;

                var newDonorId = Repository.AddDonor(newDonor);

                return ControllerHelper.ReturnToOrRedirectToIndex(this, newDonorId, "Donor_ID");
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
            if (donatesRef != null)
                return new SelectList(donatesRef, "Donates_ID", "Description", selectedValue ?? 2); // TODO: Fix hard coded 2 value for unknown Donates value                
            else
                return new SelectList(new List<DonatesReference>(), "Donates_ID", "Description");
        }

        private SelectList GetProcurerSelectList(int? selectedValue)
        {
            IEnumerable<Procurer> procurers = Repository.GetProcurers();
            var procurerQuery = from P in procurers
                                select new
                                {
                                    Procurer_ID = P.Procurer_ID,
                                    FullName = P.FirstName + " " + P.LastName
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

                UpdateModel<Donor>(donor, new[] {
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

                UpdateModel<Donor>(donor, new[] {
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
        public ActionResult Delete(int id)
        {
            var donor = Repository.GetDonor(id);

            if (donor.ProcurementDonors.Count > 0)
                return new JsonResult { Data = new DeleteDonorViewModel
                                                   {
                                                       Successful = false, 
                                                       Message = "Cannot delete because the donor has associated procurements, use Close Donor instead"
                                                   }};

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
