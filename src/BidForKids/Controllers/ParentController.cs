using System;
using System.Linq;
using System.Web.Mvc;
using BidsForKids.Data.Models;

namespace BidsForKids.Controllers
{
    public class ParentController : Controller
    {
        private IProcurementRepository factory;

        public ParentController(IProcurementRepository factory)
        {
            this.factory = factory;
        }

        public ActionResult GetParents()
        {
            var loadOptions = jqGridLoadOptions.GetLoadOptions(Request.QueryString);

            var result = new JsonResult
                              {
                                  JsonRequestBehavior = JsonRequestBehavior.AllowGet
                              };

            var rows = factory.GetSerializableParents(loadOptions);

            if (rows == null)
            {
                throw new ApplicationException("Unable to load Donors list");
            }

            var totalRows = rows.Count;

            rows = rows.Skip((loadOptions.page - 1) * loadOptions.rows).Take(loadOptions.rows).ToList();

            var totalPages = loadOptions.rows == 0 ? 0 : (int)Math.Ceiling((decimal)totalRows / (decimal)loadOptions.rows);

            result.Data = new { total = totalPages, page = loadOptions.page, records = totalRows.ToString(), rows = rows };

            return result;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            return View(factory.GetDonor(id));
        }

        public ActionResult Create()
        {
            return View();
        } 

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var newParent = factory.GetNewDonor();

                UpdateModel(newParent, new[] {
                    "Address",
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
                    "Email",
                });

                newParent.DonorType_ID = factory.GetDonorTypeByName("Parent").DonorType_ID;

                var newDonorId = factory.AddDonor(newParent);

                return ControllerHelper.ReturnToOrRedirectToIndex(this, newDonorId, "Donor_ID");
            }
            catch
            {
                return View();
            }
        }
 
        public ActionResult Edit(int id)
        {
            return View(factory.GetDonor(id));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var donor = factory.GetDonor(id);


                UpdateModel(donor, new[] {
                    "Address",
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
                    "Email"
                });

                if (factory.SaveDonor(donor) == false)
                {
                    throw new ApplicationException("Unable to save changes to Parent");
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
