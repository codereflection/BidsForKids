using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BidsForKids.Models;
using Xunit;

namespace BidsForKids.Tests.ModelLogic
{
    class AuctionItemSpecs
    {
        class GetAuctionItemTotalSpecs
        {
            [Fact]
            public void Return_priceless_when_has_single_item_with_negative_one_value()
            {
                var items = new List<Procurement> { new Procurement() { AuctionNumber = "100", EstimatedValue = -1 } };

                var itemGroup = from P in items
                                group P by P.AuctionNumber
                                    into g
                                    select new AuctionItem() { AuctionNumber = g.Key, Items = g.OrderByDescending((x) => x.EstimatedValue) };

                var result = AuctionItem.GetAuctionItemTotal(itemGroup.First());

                Assert.Equal(result, "priceless");
            }


            [Fact]
            public void Return_priceless_and_total_when_has_two_items_with_negative_one_value_and_monetary_value()
            {
                var items = new List<Procurement> { new Procurement() { AuctionNumber = "100", EstimatedValue = -1 }, new Procurement() { AuctionNumber = "100", EstimatedValue = 10 } };

                var itemGroup = from P in items
                                group P by P.AuctionNumber
                                    into g
                                    select new AuctionItem() { AuctionNumber = g.Key, Items = g.OrderByDescending((x) => x.EstimatedValue) };

                var result = AuctionItem.GetAuctionItemTotal(itemGroup.First());

                Assert.Equal(result, "$10.00 & priceless");
            }


            [Fact]
            public void Return_priceless_and_total_when_has_one_item_with_a_monetary_value()
            {
                var items = new List<Procurement> { new Procurement() { AuctionNumber = "100", EstimatedValue = 10 }};

                var itemGroup = from P in items
                                group P by P.AuctionNumber
                                    into g
                                    select new AuctionItem() { AuctionNumber = g.Key, Items = g.OrderByDescending((x) => x.EstimatedValue) };

                var result = AuctionItem.GetAuctionItemTotal(itemGroup.First());

                Assert.Equal(result, "$10.00");
            }
        }
    }
}
