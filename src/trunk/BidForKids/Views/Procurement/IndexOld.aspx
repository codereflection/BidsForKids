<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<BidForKids.Models.Procurement>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../../Scripts/jquery-1.3.2.js" type="text/javascript"></script>

    <script src="../../Scripts/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>

    <script src="../../Scripts/jqGrid/grid.locale-en.js" type="text/javascript"></script>

    <script src="../../Scripts/jqGrid/jquery.jqGrid.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            tableToGrid('#ProcurementList', {
                caption: 'Procurement List',
                pager: jQuery('#pager')
            });
            /*
            tableToGrid("#ProcurementList", {
            colNames: ['Code', 'Description', 'Quantity', 'Per Item Value', 'Notes', 'ID', 'Year'],
            colModel: [
            { name: 'Code', index: 'Code', width: 55 },
            { name: 'Description', index: 'Description', width: 55 },
            { name: 'Quantity', index: 'Quantity', width: 55 },
            { name: 'PerItemValue', index: 'PerItemValue', width: 55 },
            { name: 'Notes', index: 'Notes', width: 55 },
            { name: 'Procurement_ID', index: 'Procurement_ID', width: 55 },
            { name: 'Year', index: 'Year', width: 55 }
            ],
            rowNum: 10,
            viewrecords: true,
            caption: 'Procurement List'
            });
            */
        });
    </script>

    <h2>
        Index</h2>
    <div id="mysearch">
    </div>
    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>
    <table id="ProcurementList">
        <tr>
            <th>
            </th>
            <th>
                Catalog #
            </th>
            <th>
                Description
            </th>
            <th>
                Quantity
            </th>
            <th>
                Estimated Value
            </th>
            <th>
                Notes
            </th>
            <th>
                Auction Year
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <% Html.RenderPartial("ProcurementTemplate", item); %>
        <% } %>
    </table>
    <div id="pager">
    </div>
    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>
</asp:Content>
