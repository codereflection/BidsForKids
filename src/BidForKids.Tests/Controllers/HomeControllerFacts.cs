using System.Web.Mvc;
using Xunit;
using BidsForKids.Controllers;

namespace BidsForKids.Tests.Controllers
{
    public class HomeControllerFacts
    {
        public class Index : BidsForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                var controller = new HomeController(ProcurementFactory);

                var result = controller.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithNoModel()
            {
                var controller = new HomeController(ProcurementFactory);

                var result = controller.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Equal("Welcome to the Gatewood Elementary 'Bids For Kids' Auction Procurement Database!", viewResult.ViewData["Message"]);
                Assert.Null(viewResult.ViewData.Model);
            }

            [Fact]
            public void ReturnsProcurementViewResultWithDefaultViewName()
            {
                var controller = new HomeController(ProcurementFactory);

                var result = controller.Procurement();

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void ReturnsReportsViewResultwithDefaultViewName()
            {
                var controller = new HomeController(ProcurementFactory);

                var result = controller.Reports();

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }
        }

        public class About : BidsForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                var controller = new HomeController(ProcurementFactory);

                var result = controller.About();

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithNoModel()
            {
                var controller = new HomeController(ProcurementFactory);

                var result = controller.About();

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Null(viewResult.ViewData.Model);
            }
        }
    }
}