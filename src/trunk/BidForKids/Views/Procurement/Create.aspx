<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BidForKids.Models.Procurement>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create <%= ViewData["CreateType"] %> Procurement
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Create <%= ViewData["CreateType"] %> Procurement</h2>
        <script type="text/javascript">
            checkItemNumber = function () {
                var itemValue = $("#ItemNumber").val();
                var itemId = -1;
                $.ajax({
                    url: '<%= Url.Action("CheckItemNumber") %>',
                    data: { id: itemId, itemNumber: itemValue },
                    type: 'POST',
                    dataType: 'text',
                    success: function (data, textStatus, XMLHttpRequest) {
                        if (data != 'false') {
                            alert(data);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert('Error: ' + textStatus);
                    }
                });
            }

            getLastItemNumber = function () {
                var itemValue = $("#ItemNumber").val();

                if (itemValue.indexOf(" -") === -1) {
                    return;
                }

                var itemId = -1;
                $.ajax({
                    url: '<%= Url.Action("GetLastItemNumber") %>',
                    data: { id: itemId, itemNumber: itemValue },
                    type: 'POST',
                    dataType: 'text',
                    success: function (data, textStatus, XMLHttpRequest) {
                        if ($.trim(data).length > 0) {
                            $("#LastItemNumber").text('Last similar item number: "' + data + '"');
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert('Error: ' + textStatus);
                    }
                });
            }

            $(document).ready(function () {
                $("#ItemNumber").blur(checkItemNumber).keyup(getLastItemNumber);
            });
        </script>
    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
    <% using (Html.BeginForm())
       {%>
    <fieldset>
        <legend>Fields</legend>
        <p>
            <label for="ThankYouLetterSent">
                Thank You Letter Sent</label>
            <%= Html.CheckBox("ThankYouLetterSent") %>
        </p>
        <p>
            <label for="Donor_ID">
                Donor
            </label>
            <%= Html.DropDownList("Donor_ID", "") %>&nbsp;<%= Html.ActionLink("new", "Create", ViewData["CreateNewController"].ToString(), 
                                                              new { ReturnTo = ViewData["ReturnToUrl"] }, null)%>
        </p>
        <p>
            <label for="Auction_ID">
                Year</label>
            <%= Html.DropDownList("Auction_ID")%>
        </p>
        <p>
            <label for="Procurer_ID">
                Procurer
            </label>
            <%= Html.DropDownList("Procurer_ID", "")%>
        </p>
        <p>
            <label for="Category_ID">
                Category
            </label>
            <%= Html.DropDownList("Category_ID", "")%>
        </p>
        <p>
            <label for="AuctionNumber">
                Auction #:</label>
            <%= Html.TextBox("AuctionNumber", null, new { maxlength = 20 })%>
            <%= Html.ValidationMessage("AuctionNumber", "*")%>
        </p>
        <p>
            <label for="ItemNumber">
                Item #:</label>
            <%= Html.TextBox("ItemNumber", null, new { maxlength = 20 })%>&nbsp;
            <span id="LastItemNumber"></span>
            <%= Html.ValidationMessage("ItemNumber", "*")%>
        </p>
        <p>
            <label for="Donation">
                Donation:</label>
            <%= Html.TextArea("Donation", null, 3, 50, null)%>
            <%= Html.ValidationMessage("Donation", "*")%>
        </p>
        <p>
            <label for="Description">
                Description:</label>
            <%= Html.TextArea("Description", null, 3, 50, null) %>
            <%= Html.ValidationMessage("Description", "*") %>
        </p>
        <p>
            <label for="Certificate">
                Certificate
            </label>
            <%= Html.DropDownList("Certificate", (List<SelectListItem>)ViewData["CertificateOptions"])%>
        </p>
        <p>
            <label for="Quantity">
                Quantity:</label>
            <%= Html.TextBox("Quantity", null, new { maxlength = 10 })%>
            <%= Html.ValidationMessage("Quantity", "*") %>
        </p>
        <p>
            <label for="EstimatedValue">
                Estimated Value:</label>
            <%= Html.TextBox("EstimatedValue", null, new { maxlength = 10 })%>
            <%= Html.ValidationMessage("EstimatedValue", "*")%>
        </p>
        <p>
            <label for="SoldFor">
                Sold For:</label>
            <%= Html.TextBox("SoldFor", null, new { maxlength = 10 })%>
            <%= Html.ValidationMessage("SoldFor", "*")%>
        </p>
        <p>
            <label for="Notes">
                Notes:</label>
            <%= Html.TextArea("Notes", "", 3, 50, null) %>
            <%= Html.ValidationMessage("Notes", "*") %>
        </p>
        <p>
            <%= Html.Hidden("ProcurementType", ViewData["CreateType"]) %>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
    <% } %>
    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>
</asp:Content>
