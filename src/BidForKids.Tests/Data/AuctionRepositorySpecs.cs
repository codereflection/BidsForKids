using System;
using System.Collections.Generic;
using BidsForKids.Data.Models;
using BidsForKids.Data.Repositories;
using Machine.Specifications;

namespace BidsForKids.Tests.Data
{
    public abstract class with_an_auction_repo : InMemorySpecificationBase
    {
        protected static AuctionRepository repo;
        protected static Auction result;

        Establish context = () => 
            repo = new AuctionRepository(unitOfWork);
    }

    [Subject(typeof(AuctionRepository))]
    public class when_requesting_an_auction_by_year : with_an_auction_repo
    {

        Establish context = () => 
            unitOfWork.GetDataSource<Auction>().InsertOnSubmit(new Auction { Year = 2010 });

        Because of = () => 
            result = repo.GetBy(2010);

        It should_return_the_correct_auction_year = () =>
            result.Year.ShouldEqual(2010);

    }
}