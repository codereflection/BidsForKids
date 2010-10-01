using System;
using System.Web.Mvc;
using BidsForKids.Data.Models;
using BidsForKids.Data.Repositories;

namespace BidsForKids.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IAuctionRepository _repo;

        public AuctionController(IAuctionRepository repo)
        {
            _repo = repo;
        }


        public ActionResult Index()
        {
            return View(_repo.GetAll());
        }

        public ActionResult Edit(int id)
        {
            return View(_repo.GetById(id));
        }

        public ActionResult Details(int id)
        {
            return View(_repo.GetById(id));
        }

        public ActionResult Delete()
        {
            throw new NotImplementedException();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Auction auction)
        {
            _repo.Save(auction);

            return RedirectToAction("Index");
        }
    }
}