using System.Web.Mvc;
using BidsForKids.Models;

namespace BidsForKids.Controllers
{
    public class BidForKidsControllerBase : Controller
    {
        public IProcurementRepository factory;

        public BidForKidsControllerBase()
        {
            
        }
        public BidForKidsControllerBase(IProcurementRepository factory)
        {
            this.factory = factory;
        }
    }
}