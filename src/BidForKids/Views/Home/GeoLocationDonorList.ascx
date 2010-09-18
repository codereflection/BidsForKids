<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<BidsForKids.Data.Models.GeoLocation>>" %>
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
