using System.Collections.Generic;
using System.Linq;

namespace BidForKids.Models
{
    public class AuctionItem
    {
        public string AuctionNumber { get; set; }
        public IGrouping<string, Procurement> Items { get; set; }

        public static string GetAuctionItemTotal(AuctionItem auctionItem)
        {
            decimal subTotal = 0;
            var hasPriceless = false;

            foreach (var item in auctionItem.Items.Where((x) => x.EstimatedValue != null))
            {
                if (item.EstimatedValue == -1)
                {
                    hasPriceless = true;
                    continue;
                }
                subTotal = subTotal + item.EstimatedValue.Value;
            }

            return hasPriceless ? subTotal.ToString("C") + " & priceless" : subTotal.ToString("C");
        }
    }
}
