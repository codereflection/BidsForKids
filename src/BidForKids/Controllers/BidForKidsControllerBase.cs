using System.Web.Mvc;
using BidsForKids.Data.Models;
using BidsForKids.Data.Models;

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