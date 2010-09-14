using System.Web.Mvc;
using Xunit;
using BidForKids.Controllers;
using BidForKids.Models;
using Rhino.Mocks;
using System.Collections.Generic;
using System;

namespace BidForKids.Tests.Controllers
{
    public class GeoLocationControllerFacts
    {
        public class Index : BidForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                var controller = new GeoLocationController(_ProcurementFactory);

                // Act
                var result = controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithGeoLocationModel()
            {
                // Arrange
                var controller = new GeoLocationController(_ProcurementFactory);

                // Act
                var result = controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<List<GeoLocation>>(viewResult.ViewData.Model);
            }
        }

        public class Create : BidForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                var controller = new GeoLocationController(_ProcurementFactory);

                // Act
                var result = controller.Create();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }
        }

        public class Edit : BidForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                var controller = new GeoLocationController(_ProcurementFactory);

                // Act
                var result = controller.Edit(0);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithGeoLocationModel()
            {
                // Arrange
                var controller = new GeoLocationController(_ProcurementFactory);

                // Act
                var result = controller.Edit(0);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<GeoLocation>(viewResult.ViewData.Model);
            }
        }

        public class Details : BidForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrage
                var controller = new GeoLocationController(_ProcurementFactory);

                // Act
                var result = controller.Details(0);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithGeoLocationModel()
            {
                // Arrange
                var controller = new GeoLocationController(_ProcurementFactory);

                // Act
                var result = controller.Details(0);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<GeoLocation>(viewResult.ViewData.Model);
            }
        }
    }
}
