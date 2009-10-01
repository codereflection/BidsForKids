using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using BidForKids.Models;

namespace BidForKids.Controllers
{
    public class ContactController : Controller
    {
        private IProcurementFactory factory;

        public ContactController(IProcurementFactory factory)
        {
            this.factory = factory;
        }

        //
        // GET: /Contact/

        public ActionResult Index()
        {
            return View(factory.GetContacts());
        }

        //
        // GET: /Contact/Details/5

        public ActionResult Details(int id)
        {
            return View(factory.GetContact(id));
        }

        //
        // GET: /Contact/Create

        public ActionResult Create()
        {
            return View();
        }

        private void SetContactValues(FormCollection collection, Contact contact)
        {
            contact.Address = collection["Address"];
            contact.BusinessName = collection["BusinessName"];
            contact.City = collection["City"];
            contact.FirstName = collection["FirstName"];
            contact.LastName = collection["LastName"];
            contact.Notes = collection["Notes"];
            contact.Phone1 = collection["Phone1"];
            contact.Phone1Desc = collection["Phone1Desc"];
            contact.Phone2 = collection["Phone2"];
            contact.Phone2Desc = collection["Phone2Desc"];
            contact.Phone3 = collection["Phone3"];
            contact.Phone3Desc = collection["Phone3Desc"];
            contact.State = collection["State"];
            contact.ZipCode = collection["ZipCode"];
        }
        //
        // POST: /Contact/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Contact lNewContact = factory.GetNewContact();

                SetContactValues(collection, lNewContact);

                int lNewContactID = factory.AddContact(lNewContact);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Contact/Edit/5

        public ActionResult Edit(int id)
        {
            return View(factory.GetContact(id));
        }

        //
        // POST: /Contact/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Contact lContact = factory.GetContact(id);

                SetContactValues(collection, lContact);

                if (factory.SaveContact(lContact) == false)
                {
                    throw new ApplicationException("Unable to save contact");
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(factory.GetContact(id));
            }
        }
    }
}
