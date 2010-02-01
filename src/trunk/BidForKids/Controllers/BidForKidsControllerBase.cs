using System.Web.Mvc;
using BidForKids.Models;

namespace BidForKids.Controllers
{
    public class BidForKidsControllerBase : Controller
    {
        public IProcurementFactory factory;

        public BidForKidsControllerBase()
        {
            
        }
        public BidForKidsControllerBase(IProcurementFactory factory)
        {
            this.factory = factory;
        }
    }
}