using System.Web.Mvc;
using BidsForKids.Data.Models;

namespace BidsForKids.Controllers
{
    [Authorize(Roles = "Administrator, Procurements")]
    public class ProcurerController : Controller
    {
        private readonly IProcurementRepository factory;

        public ProcurerController(IProcurementRepository factory)
        {
            this.factory = factory;
        }

        public ActionResult Index()
        {
            return View(factory.GetProcurers());
        }

        public ActionResult Details(int id)
        {
            return View(factory.GetProcurer(id));
        }

        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var newProcurer = factory.GetNewProcurer();

                UpdateModel(newProcurer, new[] {
                        "FirstName",
                        "LastName",
                        "Phone",
                        "Email",
                        "Description"
                });

                var id = factory.AddProcurer(newProcurer);

                return ControllerHelper.ReturnToOrRedirectToIndex(this, id, "Procurer_ID");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View(factory.GetProcurer(id));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var procurer = factory.GetProcurer(id);

                UpdateModel(procurer, new[] {
                        "FirstName",
                        "LastName",
                        "Phone",
                        "Email",
                        "Description"
                });

                factory.SaveProcurer(procurer);

 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
