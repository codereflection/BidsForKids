using System.Web.Mvc;
using Xunit;
using BidsForKids.Controllers;
using BidsForKids.Data.Models;
using System.Collections.Generic;

namespace BidsForKids.Tests.Controllers
{
    public class GeoLocationControllerFacts
    {
        public class Index : BidsForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                var controller = new GeoLocationController(ProcurementFactory);

                var result = controller.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithGeoLocationModel()
            {
                var controller = new GeoLocationController(ProcurementFactory);

                var result = controller.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<List<GeoLocation>>(viewResult.ViewData.Model);
            }
        }

        public class Create : BidsForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                var controller = new GeoLocationController(ProcurementFactory);

                var result = controller.Create();

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }
        }

        public class Edit : BidsForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                var controller = new GeoLocationController(ProcurementFactory);

                var result = controller.Edit(0);

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithGeoLocationModel()
            {
                var controller = new GeoLocationController(ProcurementFactory);

                var result = controller.Edit(0);

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<GeoLocation>(viewResult.ViewData.Model);
            }
        }

        public class Details : BidsForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                var controller = new GeoLocationController(ProcurementFactory);

                var result = controller.Details(0);

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithGeoLocationModel()
            {
                var controller = new GeoLocationController(ProcurementFactory);

                var result = controller.Details(0);

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<GeoLocation>(viewResult.ViewData.Model);
            }
        }
    }
}
