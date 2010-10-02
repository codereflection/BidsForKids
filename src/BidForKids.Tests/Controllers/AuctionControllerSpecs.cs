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
                                    var auctions = new[] { new Auction { Year = 2009, Auction_ID = 1}, new Auction { Year = 2010, Auction_ID = 2} };
                                    repo.GetAll().Returns(auctions);
                                    repo.GetById(1).Returns(auctions.Where(x => x.Auction_ID == 1).FirstOrDefault());
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

    public class when_getting_an_auction_details_to_view : with_an_auction_controller
    {
        private static ViewResult result;

        Because of = () =>
            result = controller.Details(1) as ViewResult;

        It should_return_a_result = () =>
            result.ShouldNotBeNull();

        It should_get_the_auction_by_id = () =>
            repo.Received().GetById(1);

        It should_have_an_auction_in_the_view_model = () =>
            result.ViewData.Model.ShouldBeOfType<Auction>();

        It should_return_the_correct_view_model = () =>
            (result.ViewData.Model as Auction).Year.ShouldEqual(2009);
    }

    public class when_getting_auction_details_to_edit : with_an_auction_controller
    {
        private static ViewResult result;

        Because of = () =>
            result = controller.Edit(1) as ViewResult;

        It should_return_the_correct_result = () =>
            result.ShouldNotBeNull();

        It should_get_the_auction_by_id = () =>
            repo.Received().GetById(1);

        It should_have_an_auction_in_the_view_model = () =>
            result.ViewData.Model.ShouldBeOfType<Auction>();

        It should_return_the_correct_view_model = () =>
            (result.ViewData.Model as Auction).Year.ShouldEqual(2009);
    }

    public class when_updating_an_auction : with_an_auction_controller
    {
        private static RedirectToRouteResult result;
        private static Auction auction = new Auction();

        Establish context = () => 
            auction = new Auction { Year = 2010, Name = "Save the day!" };

        Because of = () =>
            result = controller.Edit(auction) as RedirectToRouteResult;

        It should_have_a_result = () =>
            result.ShouldNotBeNull();

        It should_save_the_auction = () =>
            repo.Received().Save(auction);

        It should_redirect_to_the_index_after_saving = () =>
            result.RouteValues["action"].ShouldEqual("Index");
    }
}