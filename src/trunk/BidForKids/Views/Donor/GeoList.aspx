<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<BidForKids.Models.Donor>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Geographic List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Geographic List</h2>

    <table>
        <tr>
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
                Zip
            </th>
            <th>
                Phone1
            </th>
            <th>
                Phone1 Desc
            </th>
            <th>
                Website
            </th>
            <th>
                Email
            </th>
            <th>
                Geo Location
            </th>
            <th>
                Donates
            </th>
            <th>
                Notes
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>            
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
                <%= Html.Encode(item.Website) %>
            </td>
            <td>
                <%= Html.Encode(item.Email) %>
            </td>
            <td>
                <%= Html.Encode(item.GeoLocation == null ? "" : item.GeoLocation.GeoLocationName) %>
            </td>
            <td>
                <%= Html.Encode(item.Donates) %>
            </td>
            <td>
                <%= Html.Encode(item.Notes) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

