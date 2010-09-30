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

    }
}