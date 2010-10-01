using System;
using System.Web.Mvc;
using BidsForKids.Data.Repositories;

namespace BidsForKids.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IAuctionRepository _auctionRepository;

        public AuctionController(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }


        public ActionResult Index()
        {
            return View(_auctionRepository.GetAll());
        }

        public ActionResult Edit(int id)
        {
            return View(_auctionRepository.GetById(id));
        }

        public ActionResult Details(int id)
        {
            return View(_auctionRepository.GetById(id));
        }

        public ActionResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}