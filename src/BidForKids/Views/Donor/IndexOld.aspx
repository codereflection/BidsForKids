<%@ Page Title="Businesses and Donors" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<BidsForKids.Data.Models.Donor>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Donors
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Donors</h2>
    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>
    <table>
        <tr>
            <th>
            </th>
            <th>
                ID
            </th>
            <th>
                Business Name
            </th>
            <th>
                First Name
            </th>
            <th>
                Last Name
            </th>
            <th>
                Address
            </th>
            <th>
                City
            </th>
            <th>
                State
            </th>
            <th>
                Zip Code
            </th>
            <th>
                Phone 1
            </th>
            <th>
                Phone 1 Desc
            </th>
            <th>
                Phone 2
            </th>
            <th>
                Phone 2 Desc
            </th>
            <th>
                Location
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { id=item.Donor_ID }) %>
                |
                <%= Html.ActionLink("Details", "Details", new { id=item.Donor_ID })%>
            </td>
            <td>
                <%= Html.Encode(item.Donor_ID) %>
            </td>
            <td>
                <%= Html.Encode(item.BusinessName) %>
            </td>
            <td>
                <%= Html.Encode(item.FirstName) %>
            </td>
            <td>
                <%= Html.Encode(item.LastName) %>
            </td>
            <td>
                <%= Html.Encode(item.Address) %>
            </td>
            <td>
                <%= Html.Encode(item.City) %>
            </td>
            <td>
                <%= Html.Encode(item.State) %>
            </td>
            <td>
                <%= Html.Encode(item.ZipCode) %>
            </td>
            <td>
                <%= Html.Encode(item.Phone1) %>
            </td>
            <td>
                <%= Html.Encode(item.Phone1Desc) %>
            </td>
            <td>
                <%= Html.Encode(item.Phone2) %>
            </td>
            <td>
                <%= Html.Encode(item.Phone2Desc) %>
            </td>
            <td>
                <%= item.GeoLocation == null ? "" : Html.Encode(item.GeoLocation.GeoLocationName) %>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                Notes:
            </td>
            <td colspan="12">
                <%= Html.Encode(item.Notes) %>
            </td>
        </tr>
        <% } %>
    </table>
    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>
</asp:Content>
