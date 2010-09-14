using System;
using System.Web.Mvc;
using Xunit;
using BidsForKids.Controllers;
using BidsForKids.Models;
using Rhino.Mocks;
using System.Collections.Generic;

namespace BidsForKids.Tests.Controllers
{
    public class ProcurementControllerFacts
    {
        public class Details : BidsForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {

                var controller = new ProcurementController(_ProcurementFactory);


                int parameter = (int)1;
                var result = controller.Details(parameter);


                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithProcurementModel()
            {

                var controller = new ProcurementController(_ProcurementFactory);


                int parameter = (int)1;
                var result = controller.Details(parameter);


                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<Procurement>(viewResult.ViewData.Model);
            }

            [Fact]
            public void RedirectsToHomeWhenNoIdIsPassed()
            {

                var controller = new ProcurementController(_ProcurementFactory);


                int? parameter = null;
                var result = controller.Details(parameter);


                var viewResult = Assert.IsType<RedirectToRouteResult>(result);
                Assert.Equal("Home", viewResult.RouteValues["controller"]);
                Assert.Equal("Index", viewResult.RouteValues["action"]);
            }
        }

        public class Create : BidsForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                var controller = SetupNewControllerWithMockContext<ProcurementController>(_ProcurementFactory);
                
                var result = controller.Create();

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithSelectLists()
            {
                var controller = SetupNewControllerWithMockContext<ProcurementController>(_ProcurementFactory);

                var result = controller.Create();

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
                Assert.IsType<SelectList>(viewResult.ViewData["Auction_ID"]);
                Assert.IsType<SelectList>(viewResult.ViewData["Donor_ID"]);
                Assert.IsType<SelectList>(viewResult.ViewData["Category_ID"]);
            }
        }

        public class Edit : BidsForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                var controller = SetupNewControllerWithMockContext<ProcurementController>(_ProcurementFactory);

                var result = controller.Edit((int?)0);

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithSelectLists()
            {
                var controller = SetupNewControllerWithMockContext<ProcurementController>(_ProcurementFactory);

                var result = controller.Edit(0);

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
                Assert.IsType<SelectList>(viewResult.ViewData["Auction_ID"]);
                Assert.IsType<SelectList>(viewResult.ViewData["Donor_ID"]);
                Assert.IsType<SelectList>(viewResult.ViewData["Category_ID"]);
            }

            [Fact]
            public void ReturnsIndexViewAfterSave()
            {
                var controller = SetupNewControllerWithMockContext<ProcurementController>(_ProcurementFactory);
                var collection = new FormCollection();

                var result = controller.Edit(1, collection);

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void Throws_when_cannot_find_procurement_in_database_to_edit()
            {
                var controller = SetupNewControllerWithMockContext<ProcurementController>(_ProcurementFactory);
                var collection = new FormCollection();

                Assert.Throws<ApplicationException>(() => controller.Edit(1, collection));
            }
        }
    }
}
