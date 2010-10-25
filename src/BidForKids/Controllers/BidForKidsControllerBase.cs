using System.Web.Mvc;
using BidsForKids.Data.Models;

namespace BidsForKids.Controllers
{
    public class BidsForKidsControllerBase : Controller
    {
        public IProcurementRepository repository;

        public BidsForKidsControllerBase()
        {
            
        }
        public BidsForKidsControllerBase(IProcurementRepository repository)
        {
            this.repository = repository;
        }
    }
}