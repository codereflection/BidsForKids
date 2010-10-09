using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BidsForKids.Controllers;
using BidsForKids.Data.Models;
using BidsForKids.Data.Repositories;
using BidsForKids.ViewModels;
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

    public class instantiating_the_controller : with_an_auction_controller
    {
        It should_have_setup_the_auction_to_auction_view_model_mapping = () =>
            Mapper.FindTypeMapFor(typeof(Auction), typeof(AuctionViewModel)).ShouldNotBeNull();

        It should_have_setup_the_auction_view_model_to_auction_mapping = () =>
            Mapper.FindTypeMapFor(typeof(AuctionViewModel), typeof(Auction)).ShouldNotBeNull();
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
                <IEnumerable<AuctionViewModel>>();

        It should_have_two_auctions_in_the_model = () => 
            (result.ViewData.Model as IEnumerable<AuctionViewModel>).Count().ShouldEqual(2);
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
            result.ViewData.Model.ShouldBeOfType<AuctionViewModel>();

        It should_return_the_correct_view_model = () =>
            (result.ViewData.Model as AuctionViewModel).Year.ShouldEqual(2009);
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
            result.ViewData.Model.ShouldBeOfType<AuctionViewModel>();

        It should_return_the_correct_view_model = () =>
            (result.ViewData.Model as AuctionViewModel).Year.ShouldEqual(2009);
    }

    public class when_updating_an_auction : with_an_auction_controller
    {
        private static RedirectToRouteResult result;
        private static AuctionViewModel updatedAuction;
        private static Auction auction;

        Establish context = () =>
                                {
                                    AuctionViewModel.CreateSourceMap();
                                    updatedAuction = new AuctionViewModel { Id = 1, Year = 2010, Name = "Save the day!" };
                                    auction = new Auction { Auction_ID = 1, Year = 1234, Name = "Sad Pandas" };
                                    repo.GetById(updatedAuction.Id).Returns(auction);
                                };
        
        Because of = () =>
            result = controller.Edit(updatedAuction) as RedirectToRouteResult;

        It should_have_a_result = () =>
            result.ShouldNotBeNull();

        It the_repo_should_be_called_once = () =>
            repo.ReceivedCalls().Count().ShouldEqual(1);

        It should_redirect_to_the_index_after_saving = () =>
            result.RouteValues["action"].ShouldEqual("Index");

        It should_correctly_update_the_auction_year = () =>
            auction.Year.ShouldEqual(updatedAuction.Year);

        It should_correctly_update_the_auction_name = () =>
            auction.Name.ShouldEqual(updatedAuction.Name);
    }

    public class when_creating_a_new_auction : with_an_auction_controller
    {
        private static ActionResult result;
        private static AuctionViewModel newAuction;

        Establish context = () =>
            newAuction = new AuctionViewModel { Id = 0, Name = "Test", Year = 2012 };

        Because of = () =>
            result = controller.Create(newAuction);

        It should_have_called_the_repo_once = () =>
            repo.Received().Add(Arg.Any<Auction>());

        It should_add_an_auction_with_the_correct_values = () =>
                                                               {
                                                                   var addedAuction = repo.ReceivedCalls().First().GetArguments().First() as Auction;
                                                                   addedAuction.Year.ShouldEqual(newAuction.Year);
                                                                   addedAuction.Name.ShouldEqual(newAuction.Name);
                                                               };

        It should_have_a_result = () =>
            result.ShouldNotBeNull();

        It should_redirect = () =>
            result.ShouldBeOfType<RedirectToRouteResult>();

        It should_redirect_back_to_the_index = () =>
            (result as RedirectToRouteResult).RouteValues["action"].ShouldEqual("Index");
			
    }
    
}