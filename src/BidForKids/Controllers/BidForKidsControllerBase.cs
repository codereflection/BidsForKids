using System.Web.Mvc;
using BidsForKids.Data.Models;

namespace BidsForKids.Controllers
{
    public class BidsForKidsControllerBase : Controller
    {
        public IProcurementRepository Repository;

        public BidsForKidsControllerBase()
        {
            
        }
        public BidsForKidsControllerBase(IProcurementRepository repository)
        {
            this.Repository = repository;
        }
    }
}