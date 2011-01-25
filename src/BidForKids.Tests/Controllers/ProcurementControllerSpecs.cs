using System.Collections.Generic;
using System.Web.Mvc;
using BidsForKids.Controllers;
using BidsForKids.Data.Models;
using BidsForKids.ViewModels;
using FizzWare.NBuilder;
using Machine.Specifications;
using NSubstitute;

namespace BidsForKids.Tests.Controllers
{
    public abstract class with_a_procurement_controller
    {
        protected static ProcurementController controller;
        protected static IProcurementRepository repo;
        protected static FormCollection collection;
        protected static int procurementId = 1;

        Establish context = () =>
                                {
                                    repo = Substitute.For<IProcurementRepository>();
                                    repo.GetDonors().Returns(new List<Donor> { new Donor { Donor_ID = 1, BusinessName = "Bob's Donations" } });
                                    repo.GetAuctions().Returns(new List<Auction> { new Auction { Auction_ID = 1, Year = 2011 } });
                                    repo.GetCategories().Returns(new List<Category> { new Category { Category_ID = 1, CategoryName = "Donations" } });
                                    repo.GetProcurers().Returns(new List<Procurer> { new Procurer { Procurer_ID = 1, FirstName = "Robert", LastName = "Brown" } });
                                    procurement = Builder<Procurement>.CreateNew().Build();
                                    repo.GetProcurement(procurementId).Returns(procurement);
                                    controller = new ProcurementController(repo);
                                    ProcurementDetailsViewModel.CreateDestinationMap();
                                };

        protected static Procurement procurement;
    }

    public class when_getting_a_procurement_for_details_display : with_a_procurement_controller
    {
        private static ActionResult result;

        Because of = () =>
            result = controller.GetProcurement(procurementId);

        It should_have_a_result = () =>
            result.ShouldNotBeNull();

        It should_have_a_json_result = () =>
            result.ShouldBeOfType<JsonResult>();

        It should_have_the_procurement = () =>
            (result as JsonResult).Data.ShouldNotBeNull();

        It should_have_the_procurement_id = () =>
            GetViewModel().Id.ShouldEqual(procurementId);

        It should_have_the_donation = () =>
            GetViewModel().Donation.ShouldEqual(procurement.Donation);

        private static ProcurementDetailsViewModel GetViewModel()
        {
            return ((result as JsonResult).Data as ProcurementDetailsViewModel);
        }
    }

    [Ignore]
    public class when_editing_a_procurement : with_a_procurement_controller
    {
        private static ActionResult result;

        Establish context = () =>
                                {
                                    procurementId = 1;
                                    collection = SetupEditFormCollection();
                                    repo.GetProcurement(procurementId).Returns(new Procurement { Procurement_ID = procurementId });
                                };

        private static FormCollection SetupEditFormCollection()
        {
            return new FormCollection
                       {
                           {"Procurement_ID", "1"},
                           {"ItemNumberPrefix", "mis"},
                           {"ItemNumberSuffix", "1"},
                           {"CatalogNumber", ""}, 
                           {"AuctionNumber", "1"}, 
                           {"Description", "Test"}, 
                           {"Quantity", "1"}, 
                           {"PerItemValue", ""}, 
                           {"Notes", "Test"}, 
                           {"EstimatedValue", "100.00"}, 
                           {"SoldFor", ""}, 
                           {"Category_ID", "1"}, 
                           {"Donation", "Test"}, 
                           {"ThankYouLetterSent", "False"}, 
                           {"Limitations", ""}, 
                           {"Certificate", ""},
                           {"Title", "My Title"}
                       };
        }

        Because of = () =>
            result = controller.Edit(procurementId, collection);

        It should_have_a_result = () =>
            result.ShouldNotBeNull();
    }
}