<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<BidsForKids.Data.Models.GeoLocation>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Reports
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <th>
                GeoLocation_ID
            </th>
            <th>
                GeoLocation Name
            </th>
            <th>
                Description
            </th>
            <th>
                Reports
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%= Html.Encode(item.GeoLocation_ID) %>
            </td>
            <td>
                <%= Html.Encode(item.GeoLocationName) %>
            </td>
            <td>
                <%= Html.Encode(item.Description) %>
            </td>
            <td>
                <%= Html.ActionLink("View Donors", "GeoList", "Donor", new { id = item.GeoLocation_ID }, null)%>
            </td>
        </tr>
        <% } %>
    </table>
    <div>
        <%= Html.ActionLink("View Donors with a location assigned", "GeoList", "Donor", new { id = 0 }, null)%>
    </div>
</asp:Content>
