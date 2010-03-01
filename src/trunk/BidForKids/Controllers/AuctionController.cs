using System.Linq;
using System.Text;
using System.Web.Mvc;
using BidForKids.Models;
using System.Collections.Generic;

namespace BidForKids.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IProcurementFactory factory;

        public AuctionController(IProcurementFactory factory)
        {
            this.factory = factory;
        }

        public void SetupViewData()
        {
            var tableSB = new StringBuilder();

            tableSB.AppendLine("<table class=\"customReport\">");

            tableSB.AppendLine("<thead>");
            tableSB.AppendLine("<tr>");
            tableSB.AppendLine("<th>Auction #</th>");
            tableSB.AppendLine("<th>Item #</th>");
            tableSB.AppendLine("<th>Donation</th>");
            tableSB.AppendLine("<th>Description</th>");
            tableSB.AppendLine("<th>Estimated Value</th>");
            tableSB.AppendLine("<th># Items</th>");
            tableSB.AppendLine("<th>Total Value</th>");
            tableSB.AppendLine("</tr>");
            tableSB.AppendLine("</thead>");


            var procurementItems = factory.GetProcurements(2010);

            var auctionItems = from P in procurementItems
                               where P.AuctionNumber != null
                               group P by P.AuctionNumber
                                   into g
                                   select new AuctionItem()
                                              {
                                                  AuctionNumber = g.Key,
                                                  Items = g
                                              };


            tableSB.AppendLine("<tbody>");

            foreach (var auctionItem in auctionItems)
            {
                tableSB.AppendLine("<tr class=\"customReportAuctionItemHeader\">");

                var auctionItemTotal = AuctionItem.GetAuctionItemTotal(auctionItem);

                tableSB.AppendFormat("<td>{0}</td><td></td><td></td><td></td><td></td><td>{1}</td><td>{2}</td>\n", auctionItem.AuctionNumber, auctionItem.Items.Count(), auctionItemTotal);
                foreach (var item in auctionItem.Items.OrderByDescending((x) => x.EstimatedValue))
                {
                    tableSB.AppendLine("<tr>");
                    tableSB.AppendLine("<td></td>");
                    tableSB.AppendFormat("<td>{0}</td>\n", item.ItemNumber);
                    tableSB.AppendFormat("<td>{0}</td>\n", item.Donation);
                    tableSB.AppendFormat("<td>{0}</td>\n", item.Description);
                    tableSB.AppendFormat("<td>{0}</td>\n", item.EstimatedValue == -1 ? "priceless" : item.EstimatedValue == null ? "" : item.EstimatedValue.Value.ToString("C"));
                    tableSB.AppendLine("<td></td><td></td>");
                    tableSB.AppendLine("</tr>");
                }
                tableSB.AppendLine("</tr>");
            }

            tableSB.AppendLine("</tbody>");


            tableSB.AppendLine("</table>");

            ViewData["AuctionItems"] = tableSB.ToString();
        }

        //
        // GET: /Auction/

        public ActionResult Index()
        {
            SetupViewData();

            return View();
        }

    }
}