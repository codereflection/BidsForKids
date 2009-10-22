using System.Web.Mvc;
using Xunit;
using BidForKids.Controllers;
using BidForKids.Models;
using Rhino.Mocks;
using System.Collections.Generic;

namespace BidForKids.Tests.Controllers
{
    public class ProcurementControllerFacts
    {
        public class Details : BidForKidsController
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                var controller = new ProcurementController(_ProcurementFactory);

                // Act
                int parameter = (int)1;
                var result = controller.Details(parameter);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithProcurementModel()
            {
                // Arrange
                var controller = new ProcurementController(_ProcurementFactory);

                // Act
                int parameter = (int)1;
                var result = controller.Details(parameter);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<Procurement>(viewResult.ViewData.Model);
            }

            [Fact]
            public void RedirectsToHomeWhenNoIdIsPassed()
            {
                // Arrange
                var controller = new ProcurementController(_ProcurementFactory);

                // Act
                int? parameter = null;
                var result = controller.Details(parameter);

                // Assert
                var viewResult = Assert.IsType<RedirectToRouteResult>(result);
                Assert.Equal("Home", viewResult.RouteValues["controller"]);
                Assert.Equal("Index", viewResult.RouteValues["action"]);
            }
        }

        public class Create : BidForKidsController
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                var controller = new ProcurementController(_ProcurementFactory);

                // Act
                var result = controller.Create();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithSelectLists()
            {
                // Arrange
                var controller = new ProcurementController(_ProcurementFactory);

                // Act
                var result = controller.Create();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
                Assert.IsType<SelectList>(viewResult.ViewData["Auction_ID"]);
                Assert.IsType<SelectList>(viewResult.ViewData["Donor_ID"]);
                Assert.IsType<SelectList>(viewResult.ViewData["Category_ID"]);
            }
        }

        public class Edit : BidForKidsController
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                var controller = new ProcurementController(_ProcurementFactory);

                // Act
                var result = controller.Edit((int?)0);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithSelectLists()
            {
                // Arrange
                var controller = new ProcurementController(_ProcurementFactory);

                // Act
                var result = controller.Edit((int?)0);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
                Assert.IsType<SelectList>(viewResult.ViewData["Auction_ID"]);
                Assert.IsType<SelectList>(viewResult.ViewData["Donor_ID"]);
                Assert.IsType<SelectList>(viewResult.ViewData["Category_ID"]);
            }

            [Fact]
            public void ReturnsIndexViewAfterSave()
            {
                // Arrange
                var controller = new ProcurementController(_ProcurementFactory);
                FormCollection collection = new FormCollection();

                // Act
                var result = controller.Edit(1, collection);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }
        }
    }
}
