using System;
using System.Web.Mvc;
using Xunit;
using BidsForKids.Controllers;
using BidsForKids.Data.Models;
using Rhino.Mocks;

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


                const int parameter = 1;
                var result = controller.Details(parameter);


                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithProcurementModel()
            {

                var controller = new ProcurementController(_ProcurementFactory);


                const int parameter = 1;
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
            public void ThrowsExceptionWhenCreateForProcurementIsCalledWithoutAType()
            {
                var controller = SetupNewControllerWithMockContext<ProcurementController>(_ProcurementFactory);

                Assert.Throws<NotSupportedException>(() => controller.Create());
            }
        }

        public class CreateByType : BidsForKidsControllerTestBase
        {
            [Fact(Skip="Not testing the right stuff")]
            public void SetsViewDataWithSelectLists()
            {
                var controller = SetupNewControllerWithMockContext<ProcurementController>(_ProcurementFactory);

                var result = controller.CreateByType("Business");

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

            [Fact(Skip="Method too complicated, needs to be refactored")]
            public void ReturnsIndexViewAfterSave()
            {
                var factory = MockRepository.GenerateMock<IProcurementRepository>();
                
                factory.Stub(x => x.GetProcurement(1)).IgnoreArguments().Return(new Procurement { Procurement_ID = 1 });
                var controller = SetupNewControllerWithMockContext<ProcurementController>(factory);

                var result = controller.Edit(1, new FormCollection());

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
