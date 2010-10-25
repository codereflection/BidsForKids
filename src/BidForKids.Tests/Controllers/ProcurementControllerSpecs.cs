using System.Collections.Generic;
using System.Web.Mvc;
using BidsForKids.Controllers;
using BidsForKids.Data.Models;
using Machine.Specifications;
using NSubstitute;

namespace BidsForKids.Tests.Controllers
{
    [Ignore]
    public abstract class with_a_procurement_controller
    {
        protected static ProcurementController controller;
        protected static IProcurementRepository repo;
        protected static FormCollection collection;
        protected static int procurementId;

        Establish context = () =>
                                {
                                    repo = Substitute.For<IProcurementRepository>();
                                    repo.GetDonors().Returns(new List<Donor> { new Donor { Donor_ID = 1, BusinessName = "Bob's Donations" } });
                                    repo.GetAuctions().Returns(new List<Auction> { new Auction { Auction_ID = 1, Year = 2011 } });
                                    repo.GetCategories().Returns(new List<Category> { new Category { Category_ID = 1, CategoryName = "Donations" } });
                                    repo.GetProcurers().Returns(new List<Procurer> { new Procurer { Procurer_ID = 1, FirstName = "Robert", LastName = "Brown" } });
                                    controller = new ProcurementController(repo);
                                };
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
                           {"Certificate", ""}
                       };
        }

        Because of = () =>
            result = controller.Edit(procurementId, collection);

        It should_have_a_result = () =>
            result.ShouldNotBeNull();
    }
}