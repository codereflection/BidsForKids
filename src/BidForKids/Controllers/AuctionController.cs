using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using BidsForKids.Data.Models;
using BidsForKids.Data.Repositories;
using System.Linq;
using BidsForKids.ViewModels;

namespace BidsForKids.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IAuctionRepository _repo;

        public AuctionController(IAuctionRepository repo)
        {
            _repo = repo;
            AuctionViewModel.CreateDestinationMap();
            AuctionViewModel.CreateSourceMap();
        }


        public ActionResult Index()
        {
            var auctions = _repo.GetAll().ToList();

            return View(Mapper.Map<IEnumerable<Auction>, IEnumerable<AuctionViewModel>>(auctions));
        }

        public ActionResult Edit(int id)
        {
            return View(Mapper.Map<Auction, AuctionViewModel>(_repo.GetById(id)));
        }

        public ActionResult Details(int id)
        {
            return View(Mapper.Map<Auction, AuctionViewModel>(_repo.GetById(id)));
        }

        public ActionResult Delete()
        {
            throw new NotImplementedException();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(AuctionViewModel updatedAuction)
        {
            var auction = _repo.GetById(updatedAuction.Id);

            Mapper.Map<AuctionViewModel, Auction>(updatedAuction, auction);

            _repo.Save(auction);

            return RedirectToAction("Index");
        }
    }
}