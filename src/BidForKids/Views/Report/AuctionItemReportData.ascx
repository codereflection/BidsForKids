<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<BidsForKids.Data.Models.AuctionItem>>" %>

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
                    Title
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
                        var Donation = item.Items.Aggregate("", (current, procItem) => current + ("<span class=\"groupedItemPart\">" + procItem.Donation + "</span> & "));
                        Donation = Donation.Substring(0, Donation.Length - 3);
                    %>
                    <%= Donation %>
                </td>
                <td>
                    <%
                        var Title = item.Items.Aggregate("", (current, procItem) => current + ("<span class=\"groupedItemPart\">" + procItem.Title + "</span> & "));
                        Title = Title.Substring(0, Title.Length - 3);
                    %>
                    <%= Title %>
                </td>
                <td>
                    <%
                        var Description = item.Items.Aggregate("", (current, procItem) => current + ("<span class=\"groupedItemPart\">" + procItem.Description + "</span> "));
                        Description = Description.Substring(0, Description.Length - 1);
                    %>
                    <%= Description %>
                </td>
                <td>
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
                </td>
                <td>
                    <%
                        var EstimatedValue = BidsForKids.Data.Models.AuctionItem.GetAuctionItemTotal(item);
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

