using System.Web.Mvc;
using BidForKids.Models;

namespace BidForKids.Controllers
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