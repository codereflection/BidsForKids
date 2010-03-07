<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<BidForKids.Models.AuctionItem>>" %>
<style type='text/css'>
    .AuctionItem
    {
        border: solid 1px #e3e3e3;
        width: 500px;
        padding: 10px;
        font-family: arial;
    }
    .AuctionNumber
    {
        text-align: center;
        font-weight: bold;
        margin-bottom: 5px;
    }
    .Donation
    {
        text-align: center;
        font-weight: bold;
        margin-bottom: 5px;
    }
    .Description
    {
        margin-bottom: 5px;
    }
    .ThanksTo
    {
        font-style: italic;
        text-align: center;
    }
    .Value
    {
    }
</style>
<% foreach (var item in Model.OrderBy((x) => x.AuctionNumber))
   { %>
<div class='AuctionItem'>
    <div class='AuctionNumber'>
        <%= item.AuctionNumber %>
    </div>
    <div class='Donation'>
        <%
            var Donation = item.Items.Aggregate("", (current, procItem) => current + ("<span class=\"groupedItemPart\">" + procItem.Donation + "</span> & "));
            Donation = Donation.Substring(0, Donation.Length - 3);
        %>
        <%= Donation %>
    </div>
    <div class='Description'>
        <%
            var Description = item.Items.Aggregate("", (current, procItem) => current + ("<span class=\"groupedItemPart\">" + procItem.Description + "</span> "));
            Description = Description.Substring(0, Description.Length - 1);
        %>
        <%= Description%>
    </div>
    <div class='ThanksTo'>
        Thanks to:
        <%
            var Donor = "";
            foreach (var procItem in item.Items)
            {
                if (procItem.ContactProcurement.Donor.DonorType.DonorTypeDesc == "Business")
                {
                    Donor += "<span class=\"groupedItemPart\">" + procItem.ContactProcurement.Donor.BusinessName + "</span> and ";
                }
                else
                {
                    Donor += "<span class=\"groupedItemPart\">" + procItem.ContactProcurement.Donor.FirstName + " " +
                             procItem.ContactProcurement.Donor.LastName + "</span> and ";
                }
            }
            Donor = Donor.Substring(0, Donor.Length - 5);
        %>
        <%= Donor%>
    </div>
    <div class='Value'>
        Value:
        <%
            var EstimatedValue = BidForKids.Models.AuctionItem.GetAuctionItemTotal(item);
        %>
        <%= String.Format("{0:C}", EstimatedValue) %>
    </div>
</div>
<% } %>
