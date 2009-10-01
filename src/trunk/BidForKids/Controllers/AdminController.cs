using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BidForKids.Controllers
{
    [HandleError]
    public class AdminController : Controller
    {
        public ActionResult Menu()
        {
            ViewData["Message"] = "Please choose an admin item from the menu below";
            return View();
        }
    }
}
