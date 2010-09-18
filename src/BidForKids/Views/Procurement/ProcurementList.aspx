<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<BidsForKids.Data.Models.Procurement>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Procuremen tList
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Procurement List for
        <%= ViewData["Year"] %></h2>
    <table>
        <tr>
            <th>
                Description
            </th>
            <th>
                Quantity
            </th>
            <th>
                Notes
            </th>
            <th>
                Catalog #
            </th>
            <th>
                Auction #
            </th>
            <th>
                Item #
            </th>
            <th>
                Estimated Value
            </th>
            <th>
                Sold For
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%= Html.Encode(item.Description) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:F}", item.Quantity)) %>
            </td>
            <td>
                <%= Html.Encode(item.Notes) %>
            </td>
            <td>
                <%= Html.Encode(item.CatalogNumber) %>
            </td>
            <td>
                <%= Html.Encode(item.AuctionNumber) %>
            </td>
            <td>
                <%= Html.Encode(item.ItemNumber) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:F}", item.EstimatedValue)) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:F}", item.SoldFor)) %>
            </td>
        </tr>
        <% } %>
    </table>
</asp:Content>
