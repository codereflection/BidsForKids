﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<BidForKids.Models.AuctionItem>>" %>

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
                        var Donation = item.Items.Aggregate("", (current, procItem) => current + (procItem.Donation + ", "));
                        Donation = Donation.Substring(0, Donation.Length - 2);
                    %>
                    <%= Donation %>
                </td>
                <td>
                    <%
                        var Description = item.Items.Aggregate("", (current, procItem) => current + (procItem.Description + ", "));
                        Description = Description.Substring(0, Description.Length - 2);
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
                                Donor += procItem.ContactProcurement.Donor.BusinessName + " and ";
                            }
                            else
                            {
                                Donor += procItem.ContactProcurement.Donor.FirstName + " " +
                                         procItem.ContactProcurement.Donor.LastName + " and ";
                            }
                        }
                        Donor = Donor.Substring(0, Donor.Length - 5);
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

