using System.Web.Mvc;
using Xunit;
using BidsForKids.Controllers;

namespace BidsForKids.Tests.Controllers
{
    public class HomeControllerFacts
    {
        public class Index : BidForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                var controller = new HomeController(_ProcurementFactory);

                // Act
                var result = controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithNoModel()
            {
                // Arrange
                var controller = new HomeController(_ProcurementFactory);

                // Act
                var result = controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Equal("Welcome to the Gatewood Elementary 'Bid For Kids' Auction Procurement Database!", viewResult.ViewData["Message"]);
                Assert.Null(viewResult.ViewData.Model);
            }

            [Fact]
            public void ReturnsProcurementViewResultWithDefaultViewName()
            {
                // Arrange
                var controller = new HomeController(_ProcurementFactory);

                // Assert
                var result = controller.Procurement();

                // Act
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void ReturnsReportsViewResultwithDefaultViewName()
            {
                // Arrange
                var controller = new HomeController(_ProcurementFactory);

                // Act
                var result = controller.Reports();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }
        }

        public class About : BidForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                var controller = new HomeController(_ProcurementFactory);

                // Act
                var result = controller.About();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithNoModel()
            {
                // Arrange
                var controller = new HomeController(_ProcurementFactory);

                // Act
                var result = controller.About();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Null(viewResult.ViewData.Model);
            }
        }
    }
}