using System.Web.Mvc;
using BidsForKids.Models;

namespace BidsForKids.Controllers
{
    public class BidsForKidsControllerBase : Controller
    {
        public IProcurementRepository factory;

        public BidsForKidsControllerBase()
        {
            
        }
        public BidsForKidsControllerBase(IProcurementRepository factory)
        {
            this.factory = factory;
        }
    }
}