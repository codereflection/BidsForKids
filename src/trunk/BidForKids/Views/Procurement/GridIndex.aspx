<%@ Page Title="Procurement Search" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<BidForKids.Models.Procurement>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Procurement Search
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../../Scripts/jquery-1.3.2.js" type="text/javascript"></script>

    <script src="../../Scripts/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>

    <script src="../../Scripts/jqGrid/grid.locale-en.js" type="text/javascript"></script>

    <script src="../../Scripts/jqGrid/jquery.jqGrid.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            var procurementGrid = $("#procurementGrid").jqGrid({
                datatype: 'json',
                url: '/Procurement/GetProcurements',
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                    id: "4"
                },
                colNames: ['Actions', 'Code', 'Description', 'Year', 'ID'],
                colModel: [
                    { name: 'act', index: 'act', width: 20, sortable: false, search: false },
                    { name: 'Code', index: 'Code', width: 15 },
                    { name: 'Description', index: 'Description' },
                    { name: 'Year', index: 'Year', width: 15 },
                    { name: 'Procurement_ID', index: 'Procurement_ID', width: 15 }
                ],
                pager: '#pager',
                viewrecords: true,
                rowNum: 20,
                rowList: [2, 10, 20, 30],
                width: 800,
                loadComplete: function() {
                    var ids = $("#procurementGrid").getDataIDs();
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        var editLink = "<a href='Edit/" + cl + "'>Edit</a>&nbsp;|&nbsp;";
                        var detailsLink = "<a href='Details/" + cl + "'>Details</a>";
                        jQuery("#procurementGrid").setRowData(ids[i], { act: editLink + detailsLink });
                    }
                }
            });
            procurementGrid.filterToolbar();
            procurementGrid.navGrid("#pager", { edit: false, add: false, del: false, search: false });
        });
    </script>

    <h2>
        Procurement Search</h2>
    <table id="procurementGrid">
    </table>
    <div id="pager">
    </div>
    <div id="filter" style="margin-left: 30%; display: none">
        Search Procurements</div>
    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>
</asp:Content>
