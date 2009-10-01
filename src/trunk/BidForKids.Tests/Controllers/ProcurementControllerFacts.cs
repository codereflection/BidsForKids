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
        public class Details
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                IProcurementFactory lProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
                var controller = new ProcurementController(lProcurementFactory);

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
                IProcurementFactory lProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
                var controller = new ProcurementController(lProcurementFactory);

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
                IProcurementFactory lProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
                var controller = new ProcurementController(lProcurementFactory);

                // Act
                int? parameter = null;
                var result = controller.Details(parameter);

                // Assert
                var viewResult = Assert.IsType<RedirectToRouteResult>(result);
                Assert.Equal("Home", viewResult.RouteValues["controller"]);
                Assert.Equal("Index", viewResult.RouteValues["action"]);
            }
        }

        public class Create
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                IProcurementFactory lProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
                var controller = new ProcurementController(lProcurementFactory);

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
                IProcurementFactory lProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
                var controller = new ProcurementController(lProcurementFactory);

                // Act
                var result = controller.Create();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
                Assert.IsType<SelectList>(viewResult.ViewData["Auctions"]);
                Assert.IsType<SelectList>(viewResult.ViewData["Contacts"]);
            }
        }

        public class Edit
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                IProcurementFactory lProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
                var controller = new ProcurementController(lProcurementFactory);

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
                IProcurementFactory lProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
                var controller = new ProcurementController(lProcurementFactory);

                // Act
                var result = controller.Edit((int?)0);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
                Assert.IsType<SelectList>(viewResult.ViewData["Auctions"]);
                Assert.IsType<SelectList>(viewResult.ViewData["Contacts"]);
            }
        }
    }
}
