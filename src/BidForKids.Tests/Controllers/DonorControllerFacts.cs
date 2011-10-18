using System.Data.Linq;
using System.Web.Mvc;
using BidsForKids.Data.Models.SerializableObjects;
using Xunit;
using BidsForKids.Controllers;
using BidsForKids.Data.Models;
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
                var controller = new DonorController(ProcurementFactory);

                var result = controller.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithContactModel()
            {
                var controller = new DonorController(ProcurementFactory);

                var result = controller.Index();

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
                var controller = SetupNewControllerWithMockContext<DonorController>(ProcurementFactory);

                controller.ControllerContext.HttpContext
                    .Request.Expect(x => x.QueryString["GeoLocation_ID"]).Return("");

                var result = controller.Create();

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
                var controller = new DonorController(ProcurementFactory);

                var result = controller.Edit(0);

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithContactModel()
            {
                var controller = new DonorController(ProcurementFactory);

                var result = controller.Edit(0);

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<SelectList>(viewResult.ViewData["GeoLocation_ID"]);
                Assert.IsType<Donor>(viewResult.ViewData.Model);
            }
        }

        public class when_marking_a_donor_as_closed : BidsForKidsControllerTestBase
        {
            readonly DonorController controller;
            readonly ActionResult result;

            public when_marking_a_donor_as_closed()
            {
                controller = new DonorController(ProcurementFactory);
                result = controller.Close(0);
            }

            [Fact]
            public void it_should_get_the_donor()
            {
                ProcurementFactory.AssertWasCalled(x => x.GetDonor(0));
            }

            [Fact]
            public void it_should_save_the_donor_with_the_closed_flag_set_to_true()
            {
                ProcurementFactory.AssertWasCalled(x => x.SaveDonor(Arg<Donor>.Matches(y => y.Closed == true)));
            }

            [Fact]
            public void it_should_return_a_json_result()
            {
                var jsonResult = Assert.IsType<JsonResult>(result);
            }

            [Fact]
            public void it_should_show_that_the_process_was_successful()
            {
                Assert.True(((result as JsonResult).Data as CloseDonorViewModel).Successful);
            }
        }

        public class when_marking_a_donor_as_deleted_when_the_donor_does_not_have_associated_procurements : BidsForKidsControllerTestBase
        {
            readonly DonorController controller;
            readonly ActionResult result;

            public when_marking_a_donor_as_deleted_when_the_donor_does_not_have_associated_procurements()
            {
                controller = new DonorController(ProcurementFactory);
                result = controller.Delete(0);
            }

            [Fact]
            public void it_should_get_the_donor()
            {
                ProcurementFactory.AssertWasCalled(x => x.GetDonor(0));
            }

            [Fact]
            public void it_should_delete_the_donor()
            {
                ProcurementFactory.AssertWasCalled(x => x.DeleteDonor(Arg<Donor>.Matches(y => y.Donor_ID == 0)));
            }

            [Fact]
            public void it_should_return_a_json_result()
            {
                var jsonResult = Assert.IsType<JsonResult>(result);
            }

            [Fact]
            public void it_should_show_that_the_process_was_successful()
            {
                Assert.True(((result as JsonResult).Data as DeleteDonorViewModel).Successful);
            }
        }

        public class when_marking_a_donor_as_deleted_when_the_donor_does_have_associated_procurements : BidsForKidsControllerTestBase
        {
            readonly DonorController controller;
            readonly ActionResult result;

            public when_marking_a_donor_as_deleted_when_the_donor_does_have_associated_procurements()
            {
                controller = new DonorController(ProcurementFactory);
                ProcurementFactoryHelper.GetTestDonor = () => new Donor
                                                                  {
                                                                      Donor_ID = 1,
                                                                      ProcurementDonors = new EntitySet<ProcurementDonor>
                                                                                              {
                                                                                                  new ProcurementDonor
                                                                                                      {
                                                                                                          Donor_ID = 1, 
                                                                                                          Procurement_ID = 1,
                                                                                                          Procurement = new Procurement{ Procurement_ID = 1}
                                                                                                      }
                                                                                              }
                                                                  };
                result = controller.Delete(0);
            }

            [Fact]
            public void it_should_get_the_donor()
            {
                ProcurementFactory.AssertWasCalled(x => x.GetDonor(0));
            }

            [Fact]
            public void it_should_not_delete_the_donor()
            {
                ProcurementFactory.AssertWasNotCalled(x => x.DeleteDonor(Arg<Donor>.Matches(y => y.Donor_ID == 1)));
            }

            [Fact]
            public void it_should_return_a_json_result()
            {
                var jsonResult = Assert.IsType<JsonResult>(result);
            }

            [Fact]
            public void it_should_show_that_the_process_was_not_successful()
            {
                Assert.False(((result as JsonResult).Data as DeleteDonorViewModel).Successful);
            }

            [Fact]
            public void it_should_tell_us_why_the_donor_could_not_be_deleted()
            {
                Assert.Equal("Cannot delete because the donor has associated procurements, use Close Donor instead", ((result as JsonResult).Data as DeleteDonorViewModel).Message);
            }
        }

        public class Details : BidsForKidsControllerTestBase
        {
            [Fact]
            public void ReturnsViewResultWithDefaultViewName()
            {
                var controller = new DonorController(ProcurementFactory);

                var result = controller.Details(0);

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Empty(viewResult.ViewName);
            }

            [Fact]
            public void SetsViewDataWithContactModel()
            {
                var controller = new DonorController(ProcurementFactory);

                var result = controller.Details(0);

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
                    SetupNewControllerWithMockContext<DonorController>(ProcurementFactory);

                ActionResult result = null;

                Assert.Throws<ApplicationException>(() => result = controller.GetDonors());
            }


            [Fact]
            public void Returns_an_empty_Json_object_array_of_Donors()
            {
                var controller =
                    SetupNewControllerWithMockContext<DonorController>(ProcurementFactory);
                controller = SetupQueryStringParameters<DonorController>(controller, "_search=true&sidx=&sord=&page=&rows=");
                ProcurementFactory.Stub(x => x.GetSerializableBusinesses(new jqGridLoadOptions())).IgnoreArguments().Return(new List<SerializableDonor>());

                var result = controller.GetDonors();

                controller.HttpContext.Request.VerifyAllExpectations();
                var viewResult = Assert.IsType<JsonResult>(result);
                Assert.True(viewResult.Data.ToString().Contains("records = 0"));
            }

            [Fact]
            public void Returns_a_json_object_with_one_record()
            {
                var controller =
                    SetupNewControllerWithMockContext<DonorController>(ProcurementFactory);
                controller = SetupQueryStringParameters<DonorController>(controller, "_search=false&sidx=&sord=&page1=&rows=25");


                var objToReturn = new List<SerializableDonor> { new SerializableDonor() };
                ProcurementFactory.Expect(x => x.GetSerializableBusinesses(new jqGridLoadOptions())).IgnoreArguments().
                    Return(objToReturn);

                var result = controller.GetDonors();

                controller.HttpContext.Request.VerifyAllExpectations();
                var viewResult = Assert.IsType<JsonResult>(result);
                Assert.True(viewResult.Data.ToString().Contains("records = 1"));
            }
        }
    }
}
