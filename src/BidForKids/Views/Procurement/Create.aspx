<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BidsForKids.ViewModels.EditableProcurementViewModel>" %>
<%@ Import Namespace="BidsForKids.ViewModels" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create
    <%= ViewData["CreateType"] %>
    Procurement
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Create
        <%= ViewData["CreateType"] %>
        Procurement</h2>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/Procurement/CreateEdit.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery.validate.min.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/MicrosoftMvcJQueryValidation.js") %>"></script>
    <script type="text/javascript">
        GetItemNumber = function () {
            if ($("#ItemNumberPrefix").val().length === 0)
                return "";

            return $("#ItemNumberPrefix").val() + " - " + $.trim($("#ItemNumberSuffix").val());
        }

        checkItemNumber = function () {
            var itemValue = GetItemNumber();
            var itemId = -1;
            var auctionId = $("#Auction_ID").val();
            $.ajax({
                url: '<%= Url.Action("CheckItemNumber") %>',
                data: { id: itemId, itemNumber: itemValue, auctionId: auctionId },
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
            var itemNumberPrefix = $("#ItemNumberPrefix").val();

            if (itemNumberPrefix.length === 0) {
                return;
            }

            var auctionId = $("#Auction_ID").val();

            var itemId = -1;
            $.ajax({
                url: '<%= Url.Action("GetLastItemNumber") %>',
                data: { id: itemId, itemNumberPrefix: itemNumberPrefix, auctionId: auctionId },
                type: 'POST',
                dataType: 'text',
                success: function (data, textStatus, XMLHttpRequest) {
                    if ($.trim(data).length > 0)
                        $("#LastItemNumber").text('Last similar item number: "' + data + '"');
                    else
                        $("#LastItemNumber").text('No procurements have been assigned this item number prefix for the selected auction year');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('Error: ' + textStatus);
                }
            });
        }

        $(document).ready(function () {
            $("#ItemNumberSuffix").setMask({
                mask: '99999'
            });
            $("#ItemNumberPrefix").change(getLastItemNumber);
            $("#ItemNumberSuffix").blur(checkItemNumber).keyup(getLastItemNumber);

            createEdit.donorEditAction = "<%= Url.Action("Edit", "Donor") %>";
            createEdit.loadForm();

            <% if (!string.IsNullOrEmpty(Request.QueryString["Donor_ID"])) 
            { %>

            createEdit.addSelectedDonor("<%= Request.QueryString["Donor_ID"] %>");
            <% 
            } %>

        });
    </script>
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm())
       {%>
    <fieldset>
        <legend>Fields</legend>
        <div class="editor-field">
            <label for="ThankYouLetterSent">
                Thank You Letter Sent</label>
            <%= Html.CheckBox("ThankYouLetterSent", false) %>
        </div>
        <div class="editor-field">
            <label for="donors">
                Donors
            </label>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td id="donors">
                        <ul id="donorList" style="list-style: none">
                            <% Html.RenderPartial("ProcurementDonor", new ProcurementDonorViewModel()); %>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td>
                        <a href="#" id="addDonor">Add another</a>
                        <%= Html.ActionLink("Create New Donor", "Create", ViewData["CreateNewController"].ToString(), new { ReturnTo = ViewData["ReturnToUrl"] }, new { id = "createNewDonor" })%>
                    </td>
                </tr>
            </table>
        </div>
        <div class="editor-field">
            <label for="Auction_ID">
                Year</label>
            <%= Html.DropDownList("Auction_ID")%>
        </div>
        <div class="editor-field">
            <label for="Procurer_ID">
                Procurer
            </label>
            <%= Html.DropDownList("Procurer_ID", "")%>
        </div>
        <div class="editor-field">
            <label for="Category_ID">
                Category
            </label>
            <%= Html.DropDownList("Category_ID", "")%>
        </div>
        <div class="editor-field">
            <label for="AuctionNumber">
                Auction #:</label>
            <%= Html.TextBox("AuctionNumber", null, new { maxlength = 20 })%>
        </div>
        <div class="editor-field">
            <label for="ItemNumberSuffix">
                Item #:</label>
            <%= Html.DropDownList("ItemNumberPrefix", (List<SelectListItem>)ViewData["ItemNumberPrefixes"], string.Empty)%>&nbsp;-&nbsp;
            <%= Html.TextBox("ItemNumberSuffix", null, new { maxlength = 20 })%>&nbsp;
            <span id="LastItemNumber"></span>
        </div>
        <div class="editor-field">
            <%= Html.LabelFor(model => model.Donation) %>
            <%= Html.TextAreaFor(model => model.Donation, 3, 50, null) %>
            <%= Html.ValidationMessageFor(model => model.Donation) %>
        </div>
        <div class="editor-field">
            <%= Html.LabelFor(model => model.Description) %>
            <%= Html.TextAreaFor(model => model.Description, 3, 50, null) %>
            <%= Html.ValidationMessageFor(model => model.Description) %>
        </div>
        <div class="editor-field">
            <label for="Title">
                Title:</label>
            <%= Html.TextArea("Title", null, 3, 50, null)%>
        </div>
        <div class="editor-field">
            <label for="Certificate">
                Certificate
            </label>
            <%= Html.DropDownList("Certificate", (List<SelectListItem>)ViewData["CertificateOptions"])%>
        </div>
        <div class="editor-field">
            <label for="Quantity">
                Quantity:</label>
            <%= Html.TextBox("Quantity", null, new { maxlength = 10 })%>
        </div>
        <div class="editor-field">
            <label for="EstimatedValue">
                Estimated Value:</label>
            <%= Html.TextBox("EstimatedValue", null, new { maxlength = 10 })%>
        </div>
        <div class="editor-field">
            <label for="SoldFor">
                Sold For:</label>
            <%= Html.TextBox("SoldFor", null, new { maxlength = 10 })%>
        </div>
        <div class="editor-field">
            <label for="Notes">
                Notes:</label>
            <%= Html.TextArea("Notes", "", 3, 50, null) %>
        </div>
        <div class="editor-field">
            <%= Html.Hidden("ProcurementType", ViewData["CreateType"]) %>
            <input type="submit" value="Create" id="createProcurement" />&nbsp;
            <input type="reset" id="resetCreateProcurementForm" style="float: right" />
        </div>
    </fieldset>
    <% } %>
    <div>
        <%=Html.ActionLink("Back to List", ViewData["CreateType"] + "Index") %>
    </div>
</asp:Content>
