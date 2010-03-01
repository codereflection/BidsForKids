<%@ Page Title="Report - Auction Items" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<BidForKids.Models.AuctionItem>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Report - Auction Items
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Report - Auction Items</h2>
    <table class="customReport">
        <thead>
            <tr>
                <th>
                    Auction #
                </th>
                <th>
                    Donation
                </th>
                <th>
                    Description
                </th>
                <th>
                    Donor
                </th>
                <th>
                    Estimated Value
                </th>
                <th>
                    Certificate
                </th>
            </tr>
        </thead>
        <tbody>
            <% foreach (var item in Model.OrderBy((x) => x.AuctionNumber))
               { %>
            <tr>
                <td>
                    <%= item.AuctionNumber %>
                </td>
                <td>
                    <%
                        var Donation = item.Items.Aggregate("", (current, procItem) => current + (procItem.Donation + " & "));
                        Donation = Donation.Substring(0, Donation.Length - 3);
                    %>
                    <%= Donation %>
                </td>
                <td>
                    <%
                        var Description = item.Items.Aggregate("", (current, procItem) => current + (procItem.Description + " & "));
                        Description = Description.Substring(0, Description.Length - 3);
                    %>
                    <%= Description%>
                </td>
                <td>
                    <%
                        var Donor = "";
                        foreach (var procItem in item.Items)
                        {
                            if (procItem.ContactProcurement.Donor.DonorType.DonorTypeDesc == "Business")
                            {
                                Donor += procItem.ContactProcurement.Donor.BusinessName + " & ";
                            }
                            else
                            {
                                Donor += procItem.ContactProcurement.Donor.FirstName + " " +
                                         procItem.ContactProcurement.Donor.LastName + " & ";
                            }
                        }
                        Donor = Donor.Substring(0, Donor.Length - 3);
                    %>
                    <%= Donor%>
                </td>
                <td>
                    <%
                        var EstimatedValue = BidForKids.Models.AuctionItem.GetAuctionItemTotal(item);
                    %>
                    <%= EstimatedValue%>
                </td>
                <td>
                    <%
                        var Certificate = "No";
                        if (item.Items.Any((x) => string.IsNullOrEmpty(x.Certificate) == false))
                            Certificate = "Yes";
                    %>
                    <%= Certificate%>
                </td>
            </tr>
            <% } %>
        </tbody>
    </table>
</asp:Content>
