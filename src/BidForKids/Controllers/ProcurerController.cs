using System;
using System.Web.Mvc;
using BidsForKids.Data.Models;
using BidsForKids.Data.Models;

namespace BidsForKids.Controllers
{
    public class ProcurerController : Controller
    {
        private IProcurementRepository factory;

        public ProcurerController(IProcurementRepository factory)
        {
            this.factory = factory;
        }

        //
        // GET: /Procurer/

        public ActionResult Index()
        {
            return View(factory.GetProcurers());
        }

        //
        // GET: /Procurer/Details/5

        public ActionResult Details(int id)
        {
            return View(factory.GetProcurer(id));
        }

        //
        // GET: /Procurer/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Procurer/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Procurer lNewProcurer = factory.GetNewProcurer();

                UpdateModel(lNewProcurer, new[] {
                        "FirstName",
                        "LastName",
                        "Phone",
                        "Email",
                        "Description"
                });

                factory.AddProcurer(lNewProcurer);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Procurer/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View(factory.GetProcurer(id));
        }

        //
        // POST: /Procurer/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Procurer lProcurer = factory.GetProcurer(id);

                UpdateModel(lProcurer, new[] {
                        "FirstName",
                        "LastName",
                        "Phone",
                        "Email",
                        "Description"
                });

                factory.SaveProcurer(lProcurer);

 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
