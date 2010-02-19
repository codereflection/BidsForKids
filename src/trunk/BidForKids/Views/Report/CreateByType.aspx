<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create New
    <%= ViewData["CreateByType"] %>
    Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Create New
        <%= ViewData["CreateByType"] %>
        Report</h2>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#ResultContainer").hide();

            var theForm = $("#CreateReportForm");

            theForm.submit(function () {
                var action = theForm.attr("action");
                var serializedForm = theForm.serialize();

                $("#ReportStatus").text("Loading...").show();

                $.ajax({
                    url: action,
                    data: serializedForm,
                    type: "POST",
                    dataType: "html",
                    success: function (data, textStatus, XMLHttpRequest) {
                        $("#Result").html(data);
                        $("#ResultContainer").show();
                        $("#ReportStatus").text("Complete").fadeOut("slow");
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Error: " + textStatus);
                    }
                });

                return false;
            });

            //            $("label[for$='Column']", "#ReportColumns").click(function (label) {
            //                var currValue = $(label.target).text();
            //                alert(currValue);
            //            });
        });
    
    </script>
    <div id="ReportSettingsContainer">
        <% using (Html.BeginForm("RunReport", "Report", FormMethod.Post, new { id = "CreateReportForm" }))
           { %>
        <fieldset id="ReportOptions">
            <legend>Procurement Options and Types</legend>
            <table style="width: 100%">
                <tr>
                    <td>
                        <label for="ReportTitle">
                            Report Title</label>
                        <%= Html.TextBox("ReportTitle") %><br />
                        <label for="IncludeRowNumbers">
                            Include Row Numbers</label>
                        <%= Html.CheckBox("IncludeRowNumbers", true) %>
                    </td>
                    <td>
                        <label for="BusinessType">
                            Business</label>
                        <%= Html.CheckBox("BusinessType", true)%><br />
                        <label for="ParentType">
                            Parent</label>
                        <%= Html.CheckBox("ParentType", true)%><br />
                        <label for="AdventureType">
                            Adventure</label>
                        <%= Html.CheckBox("AdventureType", true)%>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset id="ReportColumns">
            <legend>Report Columns</legend>
            <table style="width: 100%">
                <tr>
                    <td>
                        <label for="DonationColumn">
                            Donation</label><%= Html.CheckBox("DonationColumn", true)%><br />
                        <label for="DescriptionColumn">
                            Description</label><%= Html.CheckBox("DescriptionColumn", true)%><br />
                        <label for="YearColumn">
                            Year</label><%= Html.CheckBox("YearColumn", true)%><br />
                        <label for="QuantityColumn">
                            Quantity</label><%= Html.CheckBox("QuantityColumn", true)%><br />
                        <label for="EstimatedValueColumn">
                            Estimated Value</label><%= Html.CheckBox("EstimatedValueColumn", true)%><br />
                        <label for="GeoLocationNameColumn">
                            Geo Location</label><%= Html.CheckBox("GeoLocationNameColumn", true)%><br />
                    </td>
                    <td>
                        <label for="CategoryNameColumn">
                            Category</label><%= Html.CheckBox("CategoryNameColumn", true)%><br />
                        <label for="CertificateColumn">
                            Certificate</label><%= Html.CheckBox("CertificateColumn", true)%><br />
                        <label for="DonorColumn">
                            Donor</label><%= Html.CheckBox("DonorColumn", true)%><br />
                        <label for="ProcurerNameColumn">
                            Procurer</label><%= Html.CheckBox("ProcurerNameColumn", true)%><br />
                        <label for="NotesColumn">
                            Notes</label><%= Html.CheckBox("NotesColumn", true)%><br />
                        <label for="ItemNumberColumn">
                            Item #</label><%= Html.CheckBox("ItemNumberColumn", true)%><br />
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset id="ReportFilters">
            <legend>Report Filters</legend>
            <table style="width: 100%">
                <tr>
                    <td>
                        <label for="DonationFilter">
                            Donation</label><%= Html.TextBox("DonationFilter")%><br />
                        <label for="DescriptionFilter">
                            Description</label><%= Html.TextBox("DescriptionFilter")%><br />
                        <label for="YearFilter">
                            Year</label><%= Html.TextBox("YearFilter")%><br />
                        <label for="QuantityFilter">
                            Quantity</label><%= Html.TextBox("QuantityFilter")%><br />
                        <label for="EstimatedValueFilter">
                            Estimated Value</label><%= Html.TextBox("EstimatedValueFilter")%><br />
                        <label for="GeoLocationNameFilter">
                            Geo Location</label><%= Html.TextBox("GeoLocationNameFilter")%><br />
                    </td>
                    <td>
                        <label for="CategoryNameFilter">
                            Category</label><%= Html.TextBox("CategoryNameFilter")%><br />
                        <label for="CertificateFilter">
                            Certificate</label><%= Html.TextBox("CertificateFilter")%><br />
                        <label for="DonorFilter">
                            Donor</label><%= Html.TextBox("DonorFilter")%><br />
                        <label for="ProcurerNameFilter">
                            Procurer</label><%= Html.TextBox("ProcurerNameFilter")%><br />
                        <label for="NotesFilter">
                            Notes</label><%= Html.TextBox("NotesFilter")%><br />
                        <label for="ItemNumberFilter">
                            Item #</label><%= Html.TextBox("ItemNumberFilter")%><br />
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset id="ReportCommands">
            <legend>Report Commands</legend>
            <table style="width: 100%">
                <tr>
                    <td>
                        <input type="submit" id="RunReport" value="Run Report" />&nbsp
                        <div style="background-color: Silver; color: Blue; font-size: large; font-family: Verdana;
                            float: right; display: none; width: 150px; height: 25px; text-align: center;
                            vertical-align: middle;" id="ReportStatus">
                        </div>
                    </td>
                    <%--                    <td>
                        <input type="button" id="SaveReport" value="Save Report" />
                    </td>--%>
                </tr>
            </table>
        </fieldset>
        <% } %>
    </div>
    <div id="ResultContainer">
        <hr />
        <h2>
            Report:</h2>
        <hr />
        <div id="Result">
        </div>
    </div>
</asp:Content>
