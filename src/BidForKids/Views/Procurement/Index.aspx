<%@ Page Title="Procurement Search" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<BidsForKids.Data.Models.Procurement>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= ViewData["ProcurementType"] %>
    Procurement Search
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.2.custom.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqGrid/grid.locale-en.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqGrid/jquery.jqGrid.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/datejs/date.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        function convertBoolToString(value)
        {
            if (value == null)
                return '';
                
            if (value === true)
                return 'Yes';
                
            if (value == false)
                return 'No';
        }

        function getProcurement(id) {
            var url = '<%= Url.Action("GetProcurement") %>/' + id.toString();
            $.get(url, {}, function(result) {
                var context = $('#summary');
                $('#AuctionNumber', context).html(result.AuctionNumber);
                $('#ItemNumber', context).html(result.ItemNumber);
                $('#Year', context).html(result.Year);
                $('#EstimatedValue', context).html(result.EstimatedValue);
                $('#Donor', context).html(result.DisplayDonor);
                $('#Category', context).html(result.Category);
                $('#GeoLocation', context).html(result.GeoLocation);
                $('#Notes', context).html(result.Notes);
                $('#Description', context).html(result.Description);
                $('#Title', context).html(result.Title);
                $('#Donation', context).html(result.Donation);
                $('#ThankYouLetterSent', context).html(convertBoolToString(result.ThankYouLetterSent));
                $('#Certificate', context).html(result.Certificate);
            }, 'json');
        }
        var lastsel;

        function GetJSDate(DateStr) {
            // given the date in the format /Date(1241161200000)/
            var date = eval(DateStr.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
            // returns Fri May 1 00:00:00 PDT 2009
            return date;
        }

        function dateFormatter(cellValue, options, rowObject) {
            if (cellValue === null || cellValue == "") {
                return "";
            }
            var dateFormat = 'M/d/yy - hh:mm t';
            var tryParse = Date.parseExact(cellValue, dateFormat);
            var resultDate;
            if (tryParse !== null) {
                resultDate = tryParse;
            }
            else {
                resultDate = GetJSDate(cellValue);
            }
            return resultDate.toString(dateFormat);
        }

        pricelessCurrencyFormatter = function (cellval, opts, rowObject) {
    		var op = $.extend({},opts.currency);
    		if(!isUndefined(opts.colModel.formatoptions)) {
    			op = $.extend({},op,opts.colModel.formatoptions);
    		}
    		if(isEmpty(cellval)) {
    			return "";
    		}
            if(cellval == "-1") {
                return "priceless";
            }
    		return $.fmatter.util.NumberFormat(cellval,op);
    	};

 
 
        $(document).ready(function() {
            var procurementGrid = $("#procurementGrid").jqGrid({
                datatype: 'local',
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                    id: "4"
                },
                colModel: [
                    { name: 'act', index: 'act', width: 20, sortable: false, search: false, label: ' ', align: 'center' },
                    { name: 'AuctionNumber', index: 'AuctionNumber', width: 32, label: 'Auc #', editable: true },
                    { name: 'ItemNumber', index: 'ItemNumber', width: 40, label: 'Itm #' },
                    { name: 'Donation', index: 'Donation' },
                    { name: 'EstimatedValue', index: 'EstimatedValue', width: 40, formatter: pricelessCurrencyFormatter, align: 'right', label: 'Value', editable: true },
                    { name: '<%= ViewData["DonorDisplayField"] %>', index: '<%= ViewData["DonorDisplayField"] %>', label: 'Donor', sortable: false },
                    { name: 'GeoLocationName', index: 'GeoLocationName', width: 100, label: 'Geo Location', sortable: false },
                    { name: 'CategoryName', index: 'CategoryName', label: 'Category', hidden: true, sortable: false, editable: false },
                    { name: 'Category_ID', index: 'Category_ID', label: 'Category', width: 100, sortable: true, editable: true, edittype: 'select', editoptions: { value: <%= ViewData["CategoryJsonString"] %> }, formatter: 'select' },
                    { name: 'ProcurerName', index: 'ProcurerName', label: 'Procurer', width: 100, sortable: false },
                    { name: 'Year', index: 'Year', width: 30, sortable: false, searchoptions: { defaultValue: '<%= ViewData["DefaultSearchYear"] %>' } },
                    { name: 'Procurement_ID', index: 'Procurement_ID', width: 30, hidden: true, key: true },
                    { name: 'ThankYouLetterSent', index: 'ThankYouLetterSent', label: '<span style="font-size: 0.75em;">Thank You Ltr</span>', width: 50, formatter: 'checkbox', editable: true, edittype: 'checkbox', editoptions: { value: "true:false" } },
                    { name: 'CreatedOn', index: 'CreatedOn', label: 'Entered', formatter: dateFormatter, editable: false, width: 80 }
                ],
                pager: '#pager',
                viewrecords: false,
                editurl: '<%= Url.Action("AjaxEdit") %>',
                rowNum: 20,
                rowList: [2, 10, 20, 30, 40, 50, 100, 200, 300],
                width: $("#procurementGrid").parent().width() - 10,
                height: 'auto',
                loadComplete: function() {
                    var ids = $('#procurementGrid').getDataIDs();
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        var editUrl = '<%= Url.Action("Edit") %>';
                        var editLink = "<a href='" + editUrl + "/" + cl + "'>Edit</a>";
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
            procurementGrid.navGrid("#pager", { edit: false, add: false, del: false, search: false });

            procurementGrid.filterToolbar({ autosearch: true});
            procurementGrid.setGridParam({ datatype: 'json', url: '<%= Url.Action("GetProcurements", new { id = ViewData["ProcurementType"] }) %>' });
            procurementGrid[0].triggerToolbar();
            
            $("#Auction_ID").change(function() {
                if (typeof procurementGrid != 'undefined' && $("#Auction_ID").val() != "") {
                    $("#gs_Year").val($("#Auction_ID").text());
                    procurementGrid.triggerToolbar();
                }
            });
        });
    </script>
    <h2>
        <%= ViewData["ProcurementType"] %>
        Procurement Search</h2>
    <div class="ProcurementListHeader">
        <div class="CreateLinkTop">
            <a href="<%= ViewData["ProcurementCreateLink"] %>">Create</a>
        </div>
        <!--
        <div class="AuctionYearContainer">
            Year to view:
            <%= Html.DropDownList("Auction_ID", "") %>
        </div>
        -->
    </div>
    <table id="procurementGrid">
    </table>
    <div id="pager">
    </div>
    <div id="filter" style="margin-left: 30%; display: none">
        Search
        <%= ViewData["ProcurementType"] %>
        Procurements</div>
    <p>
        <a href="<%= ViewData["ProcurementCreateLink"] %>">Create</a>
    </p>
    <div id="summary">
        <h2>
            '<span id="Donation"></span>' item details:</h2>
        <div id="summaryContainer">
            <div>
                <table>
                    <tr>
                        <td class="labelCell">
                            Category
                        </td>
                        <td class="dataCell">
                            <div id="Category">
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
                            <span id="Description"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelCell">
                            Title
                        </td>
                        <td class="dataCell" colspan="7">
                            <span id="Title"></span>
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
                        </td>
                        <td class="dataCell">
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
                        <td class="labelCell" colspan="2">
                            Donor
                        </td>
                        <td class="dataCell" colspan="2">
                            <div id="Donor">
                            </div>
                        </td>
                        <td class="labelCell" colspan="2">
                            Location
                        </td>
                        <td class="dataCell" colspan="2">
                            <div id="GeoLocation">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelCell">
                            Notes
                        </td>
                        <td class="dataCell" colspan="7">
                            <div id="Notes">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
