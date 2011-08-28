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
    [Authorize(Roles = "Administrator, Procurements")]
    public class AuctionController : Controller
    {
        private readonly IAuctionRepository repo;

        public AuctionController(IAuctionRepository repo)
        {
            this.repo = repo;
            AuctionViewModel.CreateDestinationMap();
            AuctionViewModel.CreateSourceMap();
        }


        public ActionResult Index()
        {
            var auctions = repo.GetAll().ToList();

            return View(Mapper.Map<IEnumerable<Auction>, IEnumerable<AuctionViewModel>>(auctions));
        }

        public ActionResult Edit(int id)
        {
            return View(Mapper.Map<Auction, AuctionViewModel>(repo.GetById(id)));
        }

        public ActionResult Details(int id)
        {
            return View(Mapper.Map<Auction, AuctionViewModel>(repo.GetById(id)));
        }

        public ActionResult Delete()
        {
            throw new NotImplementedException();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(AuctionViewModel updatedAuction)
        {
            var auction = repo.GetById(updatedAuction.Id);

            Mapper.Map(updatedAuction, auction);

            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(AuctionViewModel newAuction)
        {
            var auction = Mapper.Map<AuctionViewModel, Auction>(newAuction);

            repo.Add(auction);

            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}