using System.Web.Mvc;
using Xunit;
using BidForKids.Controllers;
using BidForKids.Models;
using Rhino.Mocks;
using System.Collections.Generic;

namespace BidForKids.Tests.Controllers
{
    public class ContactControllerFacts
    {
        public class Index
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                IProcurementFactory lProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
                var controller = new ContactController(lProcurementFactory);

                // Act
                var result = controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithContactModel()
            {
                // Arrange
                IProcurementFactory lProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
                var controller = new ContactController(lProcurementFactory);

                // Act
                var result = controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
                Assert.IsType<List<Contact>>(viewResult.ViewData.Model);
            }
        }

        public class Create
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                IProcurementFactory lProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
                var controller = new ContactController(lProcurementFactory);

                // Act
                var result = controller.Create();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }
        }

        public class Edit
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                IProcurementFactory lProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
                var controller = new ContactController(lProcurementFactory);

                // Act
                var result = controller.Edit(0);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithContactModel()
            {
                // Arrange
                IProcurementFactory lProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
                var controller = new ContactController(lProcurementFactory);

                // Act
                var result = controller.Edit(0);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
                Assert.IsType<Contact>(viewResult.ViewData.Model);
            }
        }

        public class Details
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                IProcurementFactory lProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
                var controller = new ContactController(lProcurementFactory);

                // Act
                var result = controller.Details(0);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithContactModel()
            {
                // Arrange
                IProcurementFactory lProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
                var controller = new ContactController(lProcurementFactory);

                // Act
                var result = controller.Details(0);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
                Assert.IsType<Contact>(viewResult.ViewData.Model);
            }
        }
    }
}
