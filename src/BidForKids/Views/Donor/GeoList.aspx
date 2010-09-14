<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<BidForKids.Models.Donor>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Geographic List
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function hideRow(rowId) {
            if (rowId === null)
                return;

            $("#" + rowId).remove();
        }

        function hideLinks() {
            $(".hideLink").remove();
        }
    </script>

    <h2>
        Geographic List</h2>
    <a href="javascript:void(0);" onclick="hideLinks();">Hide Links</a><br />
    <table>
        <tr>
            <th>
            </th>
            <th>
                Business Name
            </th>
            <th>
                Mailed Packet
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
                Phone 1
            </th>
            <th>
                Phone 1 Desc
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
        <% int rowCount = 1; %>
        <% foreach (var item in Model)
           { %>
        <tr id="row_<%= rowCount.ToString() %>">
            <td>
                <% 
                    Response.Write(rowCount.ToString());
                %>
                <div class="hideLink">
                    <br />
                    <a href="javascript:void(0);" onclick="hideRow('row_<%= rowCount.ToString() %>');">hide</a>
                </div>
                <% 
                    rowCount++;
                %>
            </td>
            <td>
                <%= Html.Encode(item.BusinessName) %>
            </td>
            <td>
                <% if (item.MailedPacket == false)
                       Response.Write("No");
                   else if (item.MailedPacket == true)
                       Response.Write("Yes"); %>
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
                <%
                    if (item.Donates == null)
                        Response.Write("Unknown");
                    else if (item.Donates == 0)
                        Response.Write("No");
                    else if (item.Donates == 1)
                        Response.Write("Yes");
                    else if (item.Donates == 2)
                        Response.Write("Unknown");
                %>
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
