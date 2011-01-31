<%@ Page Title="Report - Auction Items" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<BidsForKids.Data.Models.AuctionItem>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Report - Auction Items
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Report - Auction Items</h2>
        <h3 style="color:Red">Procurements must have an auction item number assigned for them to show up in this report.</h3>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#ResultContainer").hide();
            var theForm = $("#CreateReportForm");

            theForm.validate({
                errorPlacement: function (error, element) {
                    error.appendTo(element.parent("div"));
                },
                errorElement: "em"
            });


            theForm.submit(function () {

                if (theForm.valid() == false)
                    return false;

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
        });
    
    </script>
    <div id="ReportSettingsContainer">
        <% using (Html.BeginForm("RunAuctionReport", "Report", FormMethod.Post, new { id = "CreateReportForm" }))
           { %>
        <fieldset id="ReportFilters">
            <legend>Report Filters</legend>
            <label for="AuctionNumberStartFilter">
                Auction #:</label><%= Html.TextBox("AuctionNumberStartFilter")%>&nbsp;to&nbsp;<%= Html.TextBox("AuctionNumberEndFilter")%>
            <br />
            <label for="CategoryNameFilter">
                Category</label><%= Html.DropDownList("CategoryNameFilter",(IEnumerable<SelectListItem>)ViewData["CategoryList"],"")%><br />
            <div><label for="YearFilter">
                Year</label><%= Html.TextBox("YearFilter", "", new Dictionary<string, object> { {"class", "required"} })%></div>
            <label for="CatalogLayout">
                Catalog Layout</label>
            <%= Html.CheckBox("CatalogLayout", false) %>
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
    <%--<table class="customReport">
        <thead>
            <tr>
                <th>
                    Auction #
                </th>
                <th>
                    Donation
                </th>
                <th>
                    Description
                </th>
                <th>
                    Donor
                </th>
                <th>
                    Estimated Value
                </th>
                <th>
                    Certificate
                </th>
            </tr>
        </thead>
        <tbody>
            <% foreach (var item in Model.OrderBy((x) => x.AuctionNumber))
               { %>
            <tr>
                <td>
                    <%= item.AuctionNumber %>
                </td>
                <td>
                    <%
                        var Donation = item.Items.Aggregate("", (current, procItem) => current + (procItem.Donation + " & "));
                        Donation = Donation.Substring(0, Donation.Length - 3);
                    %>
                    <%= Donation %>
                </td>
                <td>
                    <%
                        var Description = item.Items.Aggregate("", (current, procItem) => current + (procItem.Description + " & "));
                        Description = Description.Substring(0, Description.Length - 3);
                    %>
                    <%= Description%>
                </td>
                <td>
                    <%
                        var Donor = "";
                        foreach (var procItem in item.Items)
                        {
                            if (procItem.ContactProcurement.Donor.DonorType.DonorTypeDesc == "Business")
                            {
                                Donor += procItem.ContactProcurement.Donor.BusinessName + " & ";
                            }
                            else
                            {
                                Donor += procItem.ContactProcurement.Donor.FirstName + " " +
                                         procItem.ContactProcurement.Donor.LastName + " & ";
                            }
                        }
                        Donor = Donor.Substring(0, Donor.Length - 3);
                    %>
                    <%= Donor%>
                </td>
                <td>
                    <%
                        var EstimatedValue = BidsForKids.Models.AuctionItem.GetAuctionItemTotal(item);
                    %>
                    <%= EstimatedValue%>
                </td>
                <td>
                    <%
                        var Certificate = "No";
                        if (item.Items.Any((x) => string.IsNullOrEmpty(x.Certificate) == false))
                            Certificate = "Yes";
                    %>
                    <%= Certificate%>
                </td>
            </tr>
            <% } %>
        </tbody>
    </table>--%>
</asp:Content>
