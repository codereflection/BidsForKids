<%@ Page Title="Donors" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<BidForKids.Models.Donor>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Donors
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/jquery-1.3.2.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.2.custom.min.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jqGrid/grid.locale-en.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jqGrid/jquery.jqGrid.min.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            var donorGrid = $("#donorGrid").jqGrid({
                datatype: 'json',
                url: '/Donor/GetDonors/',
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
                    { name: 'FirstName', index: 'FirstName', label: 'First Name' },
                    { name: 'LastName', index: 'LastName', label: 'Last Name' },
                    { name: 'Address', index: 'Address', label: 'Address' },
                    { name: 'City', index: 'City', label: 'City' },
                    { name: 'State', index: 'State', label: 'State' },
                    { name: 'ZipCode', index: 'ZipCode', label: 'Zip' },
                    { name: 'Email', index: 'Email', label: 'Email' },
                    { name: 'Phone1', index: 'Phone1', label: 'Phone 1' },
                    { name: 'Phone1Desc', index: 'Phone1Desc', label: 'Phone 1 Desc' },
                    { name: 'Phone2', index: 'Phone2', label: 'Phone 2' },
                    { name: 'Phone2Desc', index: 'Phone2Desc', label: 'Phone 2 Desc' },
                    { name: 'GeoLocationName', index: 'GeoLocationName', label: 'GeoLocationName' },
                    { name: 'Donor_ID', index: 'Donor_ID', width: 30, hidden: true, key: true }
                ],
                pager: '#pager',
                viewrecords: true,
                rowNum: 20,
                rowList: [2, 10, 20, 30],
                width: 1500,
                height: 'auto',
                loadComplete: function() {
                var ids = $("#donorGrid").getDataIDs();
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        var editLink = "<a href='/Donor/Edit/" + cl + "'>Edit</a>";
                        var detailsLink = ""; //= "&nbsp;|&nbsp;<a href='Details/" + cl + "'>Details</a>";
                        jQuery("#donorGrid").setRowData(ids[i], { act: editLink + detailsLink });
                    }
                },
                multiselect: false
            });
            donorGrid.filterToolbar();
            donorGrid.navGrid("#pager", { edit: false, add: false, del: false, search: false });
        });
    </script>

    <h2>
        Donors</h2>
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
