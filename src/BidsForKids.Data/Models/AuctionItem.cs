using System.Linq;

namespace BidsForKids.Data.Models
{
    public class AuctionItem
    {
        public string AuctionNumber { get; set; }
        public IOrderedEnumerable<Procurement> Items { get; set; }

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
                if (item.EstimatedValue != null) subTotal = subTotal + item.EstimatedValue.Value;
            }

            if (hasPriceless && subTotal > 0)
                return subTotal.ToString("C") + " & priceless";
            else if (hasPriceless && subTotal == 0)
                return "priceless";
            else
                return subTotal.ToString("C");
        }
    }
}
