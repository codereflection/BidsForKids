using System.Data.Linq;
using System.Web.Mvc;
using BidsForKids.Data.Models.SerializableObjects;
using NSubstitute;
using Simple.Data;
using Xunit;
using BidsForKids.Controllers;
using BidsForKids.Data.Models;
using System.Collections.Generic;
using System;

namespace BidsForKids.Tests.Controllers
{
    public class DonorControllerFacts
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
            public GetDonors()
            {
                InMemoryAdapter.SetKeyColumn("DonorType", "DonorType_ID");
            }

            [Fact(Skip = "Converting to Simple.Data")]
            public void Throws_exception_when_QueryString_parameters_are_not_present()
            {
                var controller =
                    SetupNewControllerWithMockContext<DonorController>(ProcurementFactory);

                ActionResult result = null;

                Assert.Throws<ApplicationException>(() => result = controller.GetDonors());
            }

            [Fact(Skip = "Converting to Simple.Data")]
            public void Returns_an_empty_Json_object_array_of_Donors()
            {
                var controller =
                    SetupNewControllerWithMockContext<DonorController>(ProcurementFactory);
                controller = SetupQueryStringParameters<DonorController>(controller, "_search=true&sidx=&sord=&page=&rows=");
                ProcurementFactory.GetSerializableBusinesses(Arg.Any<jqGridLoadOptions>()).Returns(new List<SerializableDonor>());

                var result = controller.GetDonors();

                ProcurementFactory.Received().GetSerializableBusinesses(Arg.Any<jqGridLoadOptions>());
                var viewResult = Assert.IsType<JsonResult>(result);
                Assert.True(viewResult.Data.ToString().Contains("records = 0"));
            }

            [Fact(Skip = "Converting to Simple.Data")]
            public void Returns_a_json_object_with_one_record()
            {
                var controller =
                    SetupNewControllerWithMockContext<DonorController>(ProcurementFactory);
                controller = SetupQueryStringParameters<DonorController>(controller, "_search=false&sidx=&sord=&page1=&rows=25");


                var donors = new List<SerializableDonor> { new SerializableDonor() };
                ProcurementFactory.GetSerializableBusinesses(Arg.Any<jqGridLoadOptions>()).Returns(donors);

                var result = controller.GetDonors();

                ProcurementFactory.Received().GetSerializableBusinesses(Arg.Any<jqGridLoadOptions>());
                var viewResult = Assert.IsType<JsonResult>(result);
                Assert.True(viewResult.Data.ToString().Contains("records = 1"));
            }

            [Fact]
            public void returns_json_list_of_donors_for_get_donors_action()
            {
                var controller = SetupNewControllerWithMockContext<DonorController>();
                controller = SetupQueryStringParameters<DonorController>(controller, "_search=false&rows=20&page=1&sidx=&sord=asc");

                var db = Database.Open();
                db.DonorType.Insert(DonorType_ID: 1, DonorTypeDesc: "Business");
                db.Donors.Insert(BusinessName: "Aidan's Halloween Shop", DonorType_ID: 1);

                var result = controller.GetDonors();

                dynamic data = result.Data;

                Assert.Equal("1", data.records);
            }

            [Fact]
            public void returns_json_list_of_donors_which_match_search_criteria()
            {
                var controller = SetupNewControllerWithMockContext<DonorController>();
                controller = SetupQueryStringParameters<DonorController>(controller, "_search=true&BusinessName=Aidan&rows=20&page=1&sidx=&sord=asc");

                var db = Database.Open();
                db.DonorType.Insert(DonorType_ID: 1, DonorTypeDesc: "Business");
                db.Donors.Insert(BusinessName: "Aidan's Halloween Shop", DonorType_ID: 1);

                var result = controller.GetDonors();

                dynamic data = result.Data;

                Assert.Equal("1", data.records);
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
                ProcurementFactory.Received().GetDonor(0);
            }

            [Fact]
            public void it_should_save_the_donor_with_the_closed_flag_set_to_true()
            {
                ProcurementFactory.Received().SaveDonor(Arg.Is<Donor>(y => y.Closed == true));
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
                ProcurementFactory.Received().GetDonor(0);
            }

            [Fact]
            public void it_should_delete_the_donor()
            {
                ProcurementFactory.Received().DeleteDonor(Arg.Is<Donor>(y => y.Donor_ID == 0));
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
            readonly JsonResult result;

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
                                        Procurement = new Procurement {Procurement_ID = 1}
                                    }
                            }
                    };
                result = controller.Delete(1);
            }

            [Fact]
            public void it_should_not_delete_the_donor()
            {
                ProcurementFactory.DidNotReceive().DeleteDonor(Arg.Is<Donor>(y => y.Donor_ID == 1));
            }

            [Fact]
            public void it_should_show_that_the_process_was_not_successful()
            {
                var model = (DeleteDonorViewModel) result.Data;
                Assert.False(model.Successful);
            }

            [Fact]
            public void it_should_tell_us_why_the_donor_could_not_be_deleted()
            {
                Assert.Equal("Cannot delete because the donor has associated procurements, use Close Donor instead", ((DeleteDonorViewModel) result.Data).Message);
            }
        }
    }
}
