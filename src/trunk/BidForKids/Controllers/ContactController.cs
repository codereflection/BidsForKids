using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using BidForKids.Models;

namespace BidForKids.Controllers
{
    public class DonorController : Controller
    {
        private IProcurementFactory factory;

        public DonorController(IProcurementFactory factory)
        {
            this.factory = factory;
        }

        //
        // GET: /Donor/

        public ActionResult Index()
        {
            return View(factory.GetDonors());
        }

        //
        // GET: /Donor/Details/5

        public ActionResult Details(int id)
        {
            return View(factory.GetDonor(id));
        }

        //
        // GET: /Donor/Create

        public ActionResult Create()
        {
            return View();
        }

        private void SetContactValues(FormCollection collection, Donor Donor)
        {
            Donor.Address = collection["Address"];
            Donor.BusinessName = collection["BusinessName"];
            Donor.City = collection["City"];
            Donor.FirstName = collection["FirstName"];
            Donor.LastName = collection["LastName"];
            Donor.Notes = collection["Notes"];
            Donor.Phone1 = collection["Phone1"];
            Donor.Phone1Desc = collection["Phone1Desc"];
            Donor.Phone2 = collection["Phone2"];
            Donor.Phone2Desc = collection["Phone2Desc"];
            Donor.Phone3 = collection["Phone3"];
            Donor.Phone3Desc = collection["Phone3Desc"];
            Donor.State = collection["State"];
            Donor.ZipCode = collection["ZipCode"];
        }
        //
        // POST: /Donor/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Donor lNewContact = factory.GetNewDonor();

                SetContactValues(collection, lNewContact);

                int lNewContactID = factory.AddDonor(lNewContact);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Donor/Edit/5

        public ActionResult Edit(int id)
        {
            return View(factory.GetDonor(id));
        }

        //
        // POST: /Donor/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Donor lContact = factory.GetDonor(id);

                SetContactValues(collection, lContact);

                if (factory.SaveDonor(lContact) == false)
                {
                    throw new ApplicationException("Unable to save Donor");
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
