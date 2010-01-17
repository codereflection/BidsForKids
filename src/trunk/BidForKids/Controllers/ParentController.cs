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
    public class ParentController : Controller
    {
        private IProcurementFactory factory;

        public ParentController(IProcurementFactory factory)
        {
            this.factory = factory;
        }

        // GetParents
        public ActionResult GetParents()
        {
            jqGridLoadOptions loadOptions = jqGridLoadOptions.GetLoadOptions(Request.QueryString);

            JsonResult lResult = new JsonResult();
            lResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            List<SerializableDonor> lRows = factory.GetSerializableParents(loadOptions);

            if (lRows == null)
            {
                throw new ApplicationException("Unable to load Donors list");
            }

            int lTotalRows = lRows.Count;

            lRows = lRows.Skip((loadOptions.page - 1) * loadOptions.rows).Take(loadOptions.rows).ToList();

            int lTotalPages = loadOptions.rows == 0 ? 0 : (int)Math.Ceiling((decimal)lTotalRows / (decimal)loadOptions.rows);

            lResult.Data = new { total = lTotalPages, page = loadOptions.page, records = lTotalRows.ToString(), rows = lRows };

            return lResult;
        }

        //
        // GET: /Parent/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Parent/Details/5

        public ActionResult Details(int id)
        {
            return View(factory.GetDonor(id));
        }

        //
        // GET: /Parent/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Parent/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Donor lNewParent = factory.GetNewDonor();

                UpdateModel<Donor>(lNewParent, new[] {
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

                lNewParent.DonorType_ID = factory.GetDonorTypeByName("Parent").DonorType_ID;

                int lNewDonorID = factory.AddDonor(lNewParent);

                return ControllerHelper.ReturnToOrRedirectToIndex(this, lNewDonorID, "Donor_ID");
            }
            catch
            {
                return View();
            }
        }
        //
        // GET: /Parent/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View(factory.GetDonor(id));
        }

        //
        // POST: /Parent/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Donor lDonor = factory.GetDonor(id);


                UpdateModel<Donor>(lDonor, new[] {
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

                if (factory.SaveDonor(lDonor) == false)
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
