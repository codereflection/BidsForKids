using System.Collections.Generic;
using AutoMapper;
using BidsForKids.Data.Models;
using BidsForKids.ViewModels;
using Machine.Specifications;
using Machine.Specifications.Utility;
using System.Linq;

namespace BidsForKids.Tests.ViewModelMappings
{
    public class when_mapping_a_list_of_auctions_to_a_list_of_auction_view_models
    {
        private static IEnumerable<Auction> auctions;
        private static IEnumerable<AuctionViewModel> result;

        Establish context = () =>
                                {
                                    auctions = new[]
												   {
													   new Auction { Auction_ID = 1, Year = 2009, Name = "Test1" },
													   new Auction { Auction_ID = 2, Year = 2010, Name = "Test2" }
												   };
                                    AuctionViewModel.CreateDestinationMap();
                                };

        Because of = () =>
            result = Mapper.Map<IEnumerable<Auction>, IEnumerable<AuctionViewModel>>(auctions);

        It should_have_a_result = () =>
            result.Count().ShouldEqual(2);

        It should_have_the_correct_auction_items = () =>
            auctions.Each(item => result.Count(x => x.Id == item.Auction_ID &&
                                                    x.Name == item.Name &&
                                                    x.Year == item.Year)
                                        .ShouldEqual(1));

    }

    public class when_mapping_a_list_of_auction_view_models_to_a_list_of_auctions
    {
        private static IEnumerable<Auction> result;
        private static IEnumerable<AuctionViewModel> auctionViewModels;

        Establish context = () =>
        {
            auctionViewModels = new[]
												   {
													   new AuctionViewModel { Id = 1, Year = 2009, Name = "Test1" },
													   new AuctionViewModel { Id = 2, Year = 2010, Name = "Test2" }
												   };
            AuctionViewModel.CreateSourceMap();
        };

        Because of = () =>
            result = Mapper.Map<IEnumerable<AuctionViewModel>, IEnumerable<Auction>>(auctionViewModels);

        It should_have_a_result = () =>
            result.Count().ShouldEqual(2);

        It should_have_the_correct_auction_items = () =>
            auctionViewModels.Each(item => result.Count(x => x.Auction_ID == item.Id &&
                                                        x.Name == item.Name &&
                                                        x.Year == item.Year)
                                        .ShouldEqual(1));
    }
}