using System.Web.Mvc;
using Xunit;
using BidForKids.Controllers;
using BidForKids.Models;
using Rhino.Mocks;
using System.Collections.Generic;
using System;

namespace BidForKids.Tests.Controllers
{
    internal class DonorControllerFacts
    {
        public class Index : BidForKidsController
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                var controller = new DonorController(_ProcurementFactory);

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
                var controller = new DonorController(_ProcurementFactory);

                // Act
                var result = controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
                Assert.IsType<List<Donor>>(viewResult.ViewData.Model);
            }
        }

        public class Create : BidForKidsController
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                var controller = new DonorController(_ProcurementFactory);

                // Act
                var result = controller.Create();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<SelectList>(viewResult.ViewData["GeoLocation_ID"]);
                Assert.Empty(viewResult.ViewName);
            }
        }

        public class Edit : BidForKidsController
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                var controller = new DonorController(_ProcurementFactory);

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
                var controller = new DonorController(_ProcurementFactory);

                // Act
                var result = controller.Edit(0);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<SelectList>(viewResult.ViewData["GeoLocation_ID"]);
                Assert.IsType<Donor>(viewResult.ViewData.Model);
            }
        }

        public class Details : BidForKidsController
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                var controller = new DonorController(_ProcurementFactory);

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
                var controller = new DonorController(_ProcurementFactory);

                // Act
                var result = controller.Details(0);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
                Assert.IsType<Donor>(viewResult.ViewData.Model);
            }
        }

        public class GetDonors : BidForKidsController
        {
            [Fact]
            public void ReturnsJsonResult()
            {
                // Arrange
                var controller = new DonorController(_ProcurementFactory);

                // Act
                var result = controller.GetDonors();

                // Assert
                var viewResult = Assert.IsType<JsonResult>(result);
            }
        }
    }
}
