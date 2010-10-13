<%@ Page Title="Report Generator - Donors" Language="C#" Inherits="System.Web.Mvc.ViewPage"
    MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">
    Report Generator - Donors
</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <script src="<%= Url.Content("~/Scripts/ReportGen.js") %>" type="text/javascript"></script>

    <h2>
        Report Generator - Donors</h2>
    <div id="ReportSettingsContainer">
        <% using (Html.BeginForm("GenerateDonorReport", "ReportGen", FormMethod.Post, new { id = "CreateReportForm" }))
           { %>
        <fieldset id="ReportOptions">
            <legend>Report Options and Types</legend>
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
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset id="ReportColumns">
            <legend>Report Columns</legend>
            <table style="width: 100%">
                <tr>
                    <td>
                        <label for="BusinessNameColumn">
                            BusinessName</label><%= Html.CheckBox("BusinessNameColumn", true)%><br />
                        <label for="FirstNameColumn">
                            First Name</label><%= Html.CheckBox("FirstNameColumn", true)%><br />
                        <label for="LastNameColumn">
                            Last Name</label><%= Html.CheckBox("LastNameColumn", true)%><br />
                        <label for="AddressColumn">
                            Address</label><%= Html.CheckBox("AddressColumn", true)%><br />
                        <label for="CityColumn">
                            City</label><%= Html.CheckBox("CityColumn", true)%><br />
                        <label for="StateColumn">
                            State</label><%= Html.CheckBox("StateColumn", true)%><br />
                        <label for="ZipCodeColumn">
                            Zip Code</label><%= Html.CheckBox("ZipCodeColumn", true)%><br />
                        <label for="GeoLocationColumn">
                            Geo Location</label><%= Html.CheckBox("GeoLocationColumn", true)%><br />
                    </td>
                    <td>
                        <label for="Phone1Column">
                            Phone 1</label><%= Html.CheckBox("Phone1Column", true)%><br />
                        <label for="Phone1DescColumn">
                            Phone 1 Desc</label><%= Html.CheckBox("Phone1DescColumn", true)%><br />
                        <label for="WebSiteColumn">
                            WebSite</label><%= Html.CheckBox("WebSiteColumn", true)%><br />
                        <label for="EmailColumn">
                            Email</label><%= Html.CheckBox("EmailColumn", true)%><br />
                        <label for="DonorTypeColumn">
                            Donor Type</label><%= Html.CheckBox("DonorTypeColumn", true)%><br />
                        <label for="ProcurerColumn">
                            Procurer</label><%= Html.CheckBox("ProcurerColumn", true)%><br />
                        <label for="NotesColumn">
                            Notes</label><%= Html.CheckBox("NotesColumn", true)%><br />
                        <label for="DonatesColumn">
                            Donates</label><%= Html.CheckBox("DonatesColumn", true)%><br />
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset id="ReportFilters">
            <legend>Report Filters</legend>
            <table style="width: 100%">
                <tr>
                    <td>
                        <label for="AuctionYearFilter">
                            Donated in Auction Year (leave blank for all donors)</label>
                        <%= Html.TextBox("AuctionYearFilter")%><br />
                        <label for="GeoLocationFilter">
                            Geo Location
                        </label>
                        <%= Html.TextBox("GeoLocationFilter") %><br />
                        <label for="ProcurerFilter">
                            Procurer
                        </label>
                        <%= Html.TextBox("ProcurerFilter")%><br />
                        <label for="DonatesFilter">
                            Donates
                        </label>
                        <%= Html.TextBox("DonatesFilter")%><br />
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
