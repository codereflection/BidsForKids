using System;
using BidsForKids.Data.Models;
using Machine.Specifications;

namespace BidsForKids.Tests.Data
{
    public abstract class with_an_auction_repo
    {
        protected static AuctionRepository repo;
        protected static Auction result;
    }

    [Subject(typeof(AuctionRepository))]
    public class when_requesting_an_auction_by_year : with_an_auction_repo
    {

        Establish context = () => repo = new AuctionRepository();

        Because of = () => result = repo.GetBy(2010);

        It should_return_the_correct_auction_year = () => 
            result.Year.ShouldEqual(2010);
    }


    public class AuctionRepository
    {
        public AuctionRepository()
        {
            
        }

        public Auction GetBy(int year)
        {
            throw new NotImplementedException("sad pandas can't implement the codez");
        }
    }



}