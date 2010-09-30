using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BidsForKids.Controllers;
using BidsForKids.Data.Models;
using BidsForKids.Data.Repositories;
using Machine.Specifications;
using NSubstitute;

namespace BidsForKids.Tests.Controllers
{
    public class with_an_auction_controller
    {
        protected static IAuctionRepository repo;
        protected static AuctionController controller;

        Establish context = () =>
                                {
                                    repo = Substitute.For<IAuctionRepository>();
                                    repo.GetAll().Returns(new[] { new Auction { Year = 2009 }, new Auction { Year = 2010 } });
                                    controller = new AuctionController(repo);
                                };
    }

    public class when_viewing_a_list_of_auctions : with_an_auction_controller
    {
        private static ViewResult result;

        Because of = () => 
            result = controller.Index() as ViewResult;

        It should_return_a_result = () =>
            result.ShouldNotBeNull();

        It should_be_a_view_result_type = () =>
            result.ShouldBeOfType<ViewResult>();

        It should_get_all_of_the_auctions = () => 
            repo.Received().GetAll();

        It should_have_view_data = () =>
            result.ViewData.Model.ShouldNotBeNull();

        It should_have_view_data_of_type_auction = () =>
            result.ViewData.Model.ShouldBeOfType
                <IEnumerable<Auction>>();

        It should_have_two_auctions_in_the_model = () => 
            (result.ViewData.Model as IEnumerable<Auction>).Count().ShouldEqual(2);
    }
}