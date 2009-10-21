<%@ Page Title="Procurement Search" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<BidForKids.Models.Procurement>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Procurement Search
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script src="Scripts/jquery-1.3.2.js" type="text/javascript"></script>

    <script src="Scripts/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>

    <script src="Scripts/jqGrid/grid.locale-en.js" type="text/javascript"></script>

    <script src="Scripts/jqGrid/jquery.jqGrid.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function getProcurement(id) {
            $.get("Procurement/GetProcurement/" + id.toString(), {}, function(result) {
                var lContext = $("#summary");
                $("#CatalogNumber", lContext).html(result.CatalogNumber);
                $("#AuctionNumber", lContext).html(result.AuctionNumber);
                $("#ItemNumber", lContext).html(result.ItemNumber);
                $("#Year", lContext).html(result.Year);
                $("#EstimatedValue", lContext).html(result.EstimatedValue);
                $("#SoldFor", lContext).html(result.SoldFor);
                $("#Contact", lContext).html(result.BusinessName + ' : ' + result.PersonName);
                $("#Category", lContext).html(result.Category);
                $("#GeoLocation", lContext).html(result.GeoLocation);
            }, "json");
        }

        $(document).ready(function() {
            var procurementGrid = $("#procurementGrid").jqGrid({
                datatype: 'json',
                url: 'Procurement/GetProcurements',
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                    id: "4"
                },
                //colNames: ['Actions', 'CatalogNumber', 'AuctionNumber', 'ItemNumber', 'Description', 'EstimatedValue', 'GeoLocation',  'Year', 'ID'],
                colModel: [
                    { name: 'act', index: 'act', width: 20, sortable: false, search: false, label: ' ', align: 'center' },
                    { name: 'CatalogNumber', index: 'CatalogNumber', width: 32, label: 'Catalog #' },
                    { name: 'AuctionNumber', index: 'AuctionNumber', width: 32, label: 'Auction #' },
                    { name: 'ItemNumber', index: 'ItemNumber', width: 32, label: 'Item #' },
                    { name: 'Description', index: 'Description' },
                    { name: 'BusinessName', index: 'BusinessName' },
                    { name: 'EstimatedValue', index: 'EstimatedValue', width: 40, formatter: 'currency', align: 'right', label: 'Estimated $' },
                    { name: 'GeoLocationName', index: 'GeoLocationName', width: 100, label: 'Geo Location', sortable: false },
                    { name: 'CategoryName', index: 'CategoryName', label: 'Category', width: 100, sortable: false },
                    { name: 'ProcurerName', index: 'ProcurerName', label: 'Procurer', width: 100, sortable: false },
                    { name: 'Year', index: 'Year', width: 30, sortable: false },
                    { name: 'Procurement_ID', index: 'Procurement_ID', width: 30, hidden: true, key: true }
                ],
                pager: '#pager',
                viewrecords: true,
                rowNum: 20,
                rowList: [2, 10, 20, 30],
                width: 1500,
                height: 'auto',
                loadComplete: function() {
                    var ids = $("#procurementGrid").getDataIDs();
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        var editLink = "<a href='Procurement/Edit/" + cl + "'>Edit</a>";
                        var detailsLink = ""; //= "&nbsp;|&nbsp;<a href='Details/" + cl + "'>Details</a>";
                        jQuery("#procurementGrid").setRowData(ids[i], { act: editLink + detailsLink });
                    }
                },
                onSelectRow: function(rowid, status) {
                    var lData = procurementGrid.getRowData(rowid);
                    var lID = lData.Procurement_ID;
                    getProcurement(lID);
                },
                multiselect: false
            });
            procurementGrid.filterToolbar();
            procurementGrid.navGrid("#pager", { edit: false, add: false, del: false, search: false });
        });
    </script>

    <h2>
        Procurement Search</h2>
    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>
    <table id="procurementGrid">
    </table>
    <div id="pager">
    </div>
    <div id="filter" style="margin-left: 30%; display: none">
        Search Procurements</div>
    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>
    <h3>Selected item details:</h2>
    <div id="summaryContainer">
        <div id="summary">
            <table>
                <tr>
                    <td class="labelCell">
                        Catalog #
                    </td>
                    <td class="dataCell">
                        <div id="CatalogNumber">
                        </div>
                    </td>
                    <td class="labelCell">
                        Auction #
                    </td>
                    <td class="dataCell">
                        <div id="AuctionNumber">
                        </div>
                    </td>
                    <td class="labelCell">
                        Item #
                    </td>
                    <td class="dataCell">
                        <div id="ItemNumber">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="labelCell">
                        Year
                    </td>
                    <td class="dataCell">
                        <div id="Year">
                        </div>
                    </td>
                    <td class="labelCell">
                        Estimated Value
                    </td>
                    <td class="dataCell">
                        <div id="EstimatedValue">
                        </div>
                    </td>
                    <td class="labelCell">
                        Sold For
                    </td>
                    <td class="dataCell">
                        <div id="SoldFor">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="labelCell">
                        Business / Person
                    </td>
                    <td class="dataCell">
                        <div id="Contact">
                        </div>
                    </td>
                    <td class="labelCell">
                        Category
                    </td>
                    <td class="dataCell">
                        <div id="Category">
                        </div>
                    </td>
                    <td class="labelCell">
                        Location
                    </td>
                    <td class="dataCell">
                        <div id="GeoLocation">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
