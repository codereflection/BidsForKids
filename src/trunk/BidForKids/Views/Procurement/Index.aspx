<%@ Page Title="Procurement Search" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<BidForKids.Models.Procurement>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Procurement Search
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/jquery-1.3.2.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.2.custom.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqGrid/grid.locale-en.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqGrid/jquery.jqGrid.min.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        function convertBoolToString(value)
        {
            if (value == null)
                return "";
                
            if (value === true)
                return "Yes";
                
            if (value == false)
                return "No";
        }

        function getProcurement(id) {
            $.get("Procurement.aspx/GetProcurement/" + id.toString(), {}, function(result) {
                var lContext = $("#summary");
                $("#CatalogNumber", lContext).html(result.CatalogNumber);
                $("#AuctionNumber", lContext).html(result.AuctionNumber);
                $("#ItemNumber", lContext).html(result.ItemNumber);
                $("#Year", lContext).html(result.Year);
                $("#EstimatedValue", lContext).html(result.EstimatedValue);
                $("#SoldFor", lContext).html(result.SoldFor);
                $("#Donor", lContext).html(result.BusinessName + ' : ' + result.PersonName);
                $("#Category", lContext).html(result.CategoryName);
                $("#GeoLocation", lContext).html(result.GeoLocationName);
                $("#Notes", lContext).html(result.Notes);
                $("#Description", lContext).html(result.Description);
                $("#Donation", lContext).html(result.Donation);
                $("#ThankYouLetterSent", lContext).html(convertBoolToString(result.ThankYouLetterSent));
                $("#Certificate", lContext).html(result.Certificate);
            }, "json");
        }
        var lastsel;

 
        $(document).ready(function() {
            var procurementGrid = $("#procurementGrid").jqGrid({
                datatype: 'json',
                url: 'Procurement.aspx/GetProcurements/',
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
                    { name: 'Donation', index: 'Donation' },
                    { name: 'EstimatedValue', index: 'EstimatedValue', width: 40, formatter: 'currency', align: 'right', label: 'Estimated $' },
                    { name: 'BusinessName', index: 'BusinessName', label: 'Donor' },
                    { name: 'GeoLocationName', index: 'GeoLocationName', width: 100, label: 'Geo Location', sortable: false },
                    { name: 'CategoryName', index: 'CategoryName', label: 'Category', hidden: true, sortable: false, editable: false },
                    { name: 'Category_ID', index: 'Category_ID', label: 'Category', width: 100, sortable: false, editable: true, edittype: 'select', editoptions: { value: <%= ViewData["CategoryJsonString"] %> }, formatter: 'select' },
                    { name: 'ProcurerName', index: 'ProcurerName', label: 'Procurer', width: 100, sortable: false },
                    { name: 'Year', index: 'Year', width: 30, sortable: false, searchoptions: { defaultValue: '<%= ViewData["DefaultSearchYear"] %>' } },
                    { name: 'Procurement_ID', index: 'Procurement_ID', width: 30, hidden: true, key: true },
                    { name: 'ThankYouLetterSent', index: 'ThankYouLetterSent', label: '<span style="font-size: 0.75em;">Thank You Ltr</span>', width: 50, formatter: 'checkbox', editable: true, edittype: 'checkbox', editoptions: { value: "true:false" } }
                ],
                pager: '#pager',
                viewrecords: false,
                editurl: 'Procurement.aspx/AjaxEdit',
                rowNum: 20,
                rowList: [2, 10, 20, 30, 40, 50, 100, 200, 300],
                width: 1170,
                height: 'auto',
                loadComplete: function() {
                    var ids = $("#procurementGrid").getDataIDs();
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        var editLink = "<a href='Procurement.aspx/Edit/" + cl + "'>Edit</a>";
                        var detailsLink = ""; //= "&nbsp;|&nbsp;<a href='Details/" + cl + "'>Details</a>";
                        jQuery("#procurementGrid").setRowData(ids[i], { act: editLink + detailsLink });
                    }
                },
                onSelectRow: function(rowid, status) {
                    var lData = procurementGrid.getRowData(rowid);
                    var lID = lData.Procurement_ID;
                    getProcurement(lID);

                    if (rowid && rowid !== lastsel) {
                        jQuery('#procurementGrid').restoreRow(lastsel);
                        jQuery('#procurementGrid').editRow(rowid, true);
                        lastsel = rowid;
                    }
                },
                loadBeforeSend: function(xhr) {
                    if (typeof procurementGrid != 'undefined' && $("#Auction_ID").val() != "") {
                        procurementGrid.appendPostData({ Auction_ID: $("#Auction_ID").val() });
                    }
                },
                multiselect: false
            });
            procurementGrid.filterToolbar();
            procurementGrid.navGrid("#pager", { edit: false, add: false, del: false, search: false });
            $("#Auction_ID").change(function() {
                debugger;
                if (typeof procurementGrid != 'undefined' && $("#Auction_ID").val() != "") {
                    $("#gs_Year").val($("#Auction_ID").text());
                    procurementGrid.triggerToolbar();
                }
            });
        });
    </script>
    <h2>
        Procurement Search</h2>
    <div class="ProcurementListHeader">
        <div class="CreateLinkTop">
            <%= Html.ActionLink("Create New", "Create") %>
        </div>
        <div class="AuctionYearContainer">
            Year to view:
            <%= Html.DropDownList("Auction_ID", "") %>
        </div>
    </div>
    <table id="procurementGrid">
    </table>
    <div id="pager">
    </div>
    <div id="filter" style="margin-left: 30%; display: none">
        Search Procurements</div>
    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>
    <div id="summary">
        <h2>
            '<span id="Donation"></span>' item details:</h2>
        <div id="summaryContainer">
            <div>
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
                        <td class="labelCell">
                            Certificate
                        </td>
                        <td class="dataCell">
                            <div id="Certificate">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelCell">
                            Description
                        </td>
                        <td class="dataCell" colspan="7">
                            <span id="Description">
                            </span>
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
                        <td class="labelCell">
                            Thank You Letter Sent
                        </td>
                        <td class="dataCell">
                            <div id="ThankYouLetterSent">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelCell">
                            Donor
                        </td>
                        <td class="dataCell">
                            <div id="Donor">
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
                    <tr>
                        <td class="labelCell">
                            Notes
                        </td>
                        <td class="dataCell" colspan="5">
                            <div id="Notes">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
