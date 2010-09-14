using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BidsForKids.Models.SerializableObjects;
using Xunit;
using BidsForKids.Controllers;
using BidsForKids.Models;
using Rhino.Mocks;
using System.Collections.Generic;
using System;

namespace BidsForKids.Tests.Controllers
{
    internal class DonorControllerFacts
    {
        public class Index : BidsForKidsControllerTestBase
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

        public class Create : BidsForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                // Arrange
                var controller = SetupNewControllerWithMockContext<DonorController>(_ProcurementFactory);

                controller.ControllerContext.HttpContext
                    .Request.Expect(x => x.QueryString["GeoLocation_ID"]).Return("");

                // Act
                var result = controller.Create();

                // Assert
                controller.ControllerContext.HttpContext
                    .VerifyAllExpectations();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<SelectList>(viewResult.ViewData["GeoLocation_ID"]);
                Assert.Empty(viewResult.ViewName);
            }

        }

        public class Edit : BidsForKidsControllerTestBase
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

        public class Details : BidsForKidsControllerTestBase
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

        public class GetDonors : BidsForKidsControllerTestBase
        {
            [Fact]
            public void Throws_exception_when_QueryString_parameters_are_not_present()
            {
                var controller =
                    SetupNewControllerWithMockContext<DonorController>(_ProcurementFactory);

                ActionResult result = null;

                Assert.Throws<ApplicationException>(() => result = controller.GetDonors());
            }


            [Fact]
            public void Returns_an_empty_Json_object_array_of_Donors()
            {
                // Arrange
                var controller =
                    SetupNewControllerWithMockContext<DonorController>(_ProcurementFactory);
                controller = SetupQueryStringParameters<DonorController>(controller, "_search=true&sidx=&sord=&page=&rows=");
                _ProcurementFactory.Stub(x => x.GetSerializableBusinesses(new jqGridLoadOptions())).IgnoreArguments().Return(new List<SerializableDonor>());
                
                // Act
                var result = controller.GetDonors();

                // Assert
                controller.HttpContext.Request.VerifyAllExpectations();
                var viewResult = Assert.IsType<JsonResult>(result);
                Assert.True(viewResult.Data.ToString().Contains("records = 0"));
            }

            [Fact]
            public void Returns_a_json_object_with_one_record()
            {
                // Arrange
                var controller =
                    SetupNewControllerWithMockContext<DonorController>(_ProcurementFactory);
                controller = SetupQueryStringParameters<DonorController>(controller, "_search=false&sidx=&sord=&page1=&rows=25");


                var objToReturn = new List<SerializableDonor>();
                objToReturn.Add(new SerializableDonor());
                _ProcurementFactory.Expect(x => x.GetSerializableBusinesses(new jqGridLoadOptions())).IgnoreArguments().
                    Return(objToReturn);
                    
                // Act
                var result = controller.GetDonors();

                // Assert
                controller.HttpContext.Request.VerifyAllExpectations();
                var viewResult = Assert.IsType<JsonResult>(result);
                Assert.True(viewResult.Data.ToString().Contains("records = 1"));
            }
        }
    }
}
