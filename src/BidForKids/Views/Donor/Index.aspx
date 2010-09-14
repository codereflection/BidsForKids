<%@ Page Title="Donors" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<BidForKids.Models.Donor>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Donors
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.2.custom.min.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jqGrid/grid.locale-en.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jqGrid/jquery.jqGrid.min.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
        var lastsel;
        $(document).ready(function() {
            var donorGrid = $("#donorGrid").jqGrid({
                datatype: 'json',
                url: 'Donor/GetDonors/',
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
                    { name: 'BusinessName', index: 'BusinessName', label: 'Business Name' },
                    { name: 'FirstName', index: 'FirstName', label: 'First Name', editable: true },
                    { name: 'LastName', index: 'LastName', label: 'Last Name', editable: true },
                    { name: 'Address', index: 'Address', label: 'Address', editable: true },
                    { name: 'City', index: 'City', label: 'City', editable: true },
                    { name: 'State', index: 'State', label: 'State', editable: true },
                    { name: 'ZipCode', index: 'ZipCode', label: 'Zip', editable: true },
                    { name: 'Email', index: 'Email', label: 'Email', editable: true },
                    { name: 'Phone1', index: 'Phone1', label: 'Phone 1', editable: true },
                    { name: 'Phone1Desc', index: 'Phone1Desc', label: 'Phone 1 Desc', editable: true },
                    { name: 'GeoLocationName', index: 'GeoLocationName', label: 'GeoLocationName', hidden: true },
                    { name: 'GeoLocation_ID', index: 'GeoLocation_ID', label: 'Geo Location', editable: true, edittype: 'select', editoptions: { value: <%= ViewData["GeoLocationJsonString"] %> }, formatter: 'select' },
                    { name: 'Procurer_ID', index: 'Procurer_ID', label: 'Default Procurer', editable: true, edittype: 'select', editoptions: { value: <%= ViewData["ProcurerJsonString"] %> }, formatter: 'select' },
                    { name: 'Donates', index: 'Donates', label: 'Donates', formatter: 'select', editable: true, edittype: 'select', editoptions: { value: { 0: "No", 1: "Yes", 2: "Unknown" } } },
                    { name: 'MailedPacket', index: 'MailedPacket', label: 'MailedPacket', formatter: 'checkbox', editable: true, edittype: 'checkbox', editoptions: { value: "true:false" } },
                    { name: 'Donor_ID', index: 'Donor_ID', width: 30, hidden: true, key: true }
                ],
                pager: '#pager',
                viewrecords: true,
                rowNum: 20,
                rowList: [2, 10, 20, 30, 40, 50, 60, 70, 100],
                width: $("#donorGrid").parent().width() - 10,
                height: '460',
                loadComplete: function() {
                var ids = $("#donorGrid").getDataIDs();
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        var editLink = "<a href='Donor/Edit/" + cl + "'>Edit</a>";
                        var detailsLink = ""; //= "&nbsp;|&nbsp;<a href='Details/" + cl + "'>Details</a>";
                        jQuery("#donorGrid").setRowData(ids[i], { act: editLink + detailsLink });
                    }
                },
                multiselect: false,
                onSelectRow: function(rowid, status) {
                var lData = donorGrid.getRowData(rowid);
                    if (rowid && rowid !== lastsel) {
                        jQuery('#donorGrid').restoreRow(lastsel);
                        jQuery('#donorGrid').editRow(rowid, true);
                        lastsel = rowid;
                    }
                },
                editurl: 'Donor/AjaxEdit'
            });
            donorGrid.filterToolbar();
            donorGrid.navGrid("#pager", { edit: false, add: false, del: false, search: false });
        });
                            //{ name: 'Donates', index: 'Donates', label: 'Donates', formatter: 'checkbox', editable: true, edittype: 'checkbox', editoptions: { value:"true:false" } },

    </script>

    <h2>
        Donors</h2>
    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>
    <table id="donorGrid">
    </table>
    <div id="pager">
    </div>
    <div id="filter" style="margin-left: 30%; display: none">
        Search Donors</div>
    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>
</asp:Content>
