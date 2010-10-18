<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BidsForKids.ViewModels.ProcurementViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="../../Scripts/MicrosoftAjax.js"></script>
    <script type="text/javascript">
        GetItemNumber = function () {
            if ($("#ItemNumberPrefix").val().length === 0)
                return "";

            return $("#ItemNumberPrefix").val() + " - " + $.trim($("#ItemNumberSuffix").val());
        }

        checkItemNumber = function () {
            var itemValue = GetItemNumber();
            var itemId = $("#Procurement_ID").val();
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

        ChangeViewDonorLink = function () {
            $('#ViewDonor').attr('href', '<%= Url.Action("Edit", "Donor") %>' + '/' + $("#Donor_ID").val());
        };

        RemoveDonor = function () {
            var id = $(this).attr("tag");
            var liId = "#donor_" + id;
            var numberOfDonors = $("#donors li[id^='donor_']").length;

            if (numberOfDonors == 1)
                $(liId + " #DonorId").val("");
            else if (numberOfDonors > 1)
                $(liId).remove();

        }

        AddDonor = function () {
            var templateText = $("#donors li")[0];
            var template = $(templateText).clone();
            $("select", template).val("");
            template.appendTo("#donorList");
        }

        $(document).ready(function () {
            $("#ItemNumberSuffix").setMask({
                mask: '99999'
            });

            $("#ItemNumberPrefix").change(getLastItemNumber);
            $('#ItemNumberSuffix').blur(checkItemNumber).keyup(getLastItemNumber);

            $('#Donor_ID').change(ChangeViewDonorLink);

            $('#removeDonor').live('click', RemoveDonor);

            $("#addDonor").click(AddDonor);
        });

        function deleteRecord(id) {
            var answer = confirm("Are you sure you want to delete this procurement?");
            if (answer === false)
                return;

            $.ajax({
                url: '<%= Url.Action("Delete") %>', // + id.toString(),  // "/Procurement/Delete/" + id;
                data: { id: id.toString() },
                type: 'POST',
                dataType: 'text',
                success: function (data, textStatus, XMLHttpRequest) {
                    if (data == "True") {
                        alert("Procurement deleted, now sending you back to the home page.");
                        window.location = '<%= Url.Action("Index", "Home") %>';
                    }
                    else {
                        alert("Procurement was NOT deleted: " + data);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error: " + textStatus + " (" + XMLHttpRequest.statusText + ")");
                }
            });
        }
    </script>
    <h2>
        Edit</h2>
    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>
    <% using (Html.BeginForm())
       {%>
    <fieldset>
        <legend>Fields</legend>
        <p>
            <input type="submit" value="Save" />
        </p>
        <p style="display: none;">
            <label for="Procurement_ID">
                Procurement ID:</label>
            <%= Html.Hidden("Procurement_ID", Model.Id) %>
        </p>
        <p>
            <label for="ThankYouLetterSent">
                Thank You Letter Sent</label>
            <%= Html.CheckBox("ThankYouLetterSent") %>
        </p>
        <div>
            <label for="donors">
                Donors
            </label>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td id="donors">
                        <ul id="donorList" style="list-style: none">
                            <% foreach (var item in Model.Donors)
                               { %>
                            <% Html.RenderPartial("ProcurementDonor", item); %>
                            <% } %>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td>
                        <a href="#" id="addDonor">Add another</a>
                    </td>
                </tr>
            </table>
        </div>
        <p>
            <label for="Auction_ID">
                Year</label>
            <%= Html.DropDownList("Auction_ID","")%>
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
            <%= Html.TextBox("AuctionNumber", Model.AuctionNumber, new { maxlength = 20 })%>
            <%= Html.ValidationMessage("AuctionNumber", "*")%>
        </p>
        <p>
            <label for="ItemNumberSuffix">
                Item #:</label>
            <%= Html.DropDownList("ItemNumberPrefix", (List<SelectListItem>)ViewData["ItemNumberPrefixes"], string.Empty)%>&nbsp;-&nbsp;
            <%= Html.TextBox("ItemNumberSuffix", Model.ItemNumberSuffix, new { maxlength = 20 })%>&nbsp;
            <span id="LastItemNumber"></span>
            <%= Html.ValidationMessage("ItemNumberSuffix", "*")%>
        </p>
        <p>
            <label for="Donation">
                Donation:</label>
            <%= Html.TextArea("Donation", Model.Donation, 3, 50, null)%>
            <%= Html.ValidationMessage("Donation", "*")%>
        </p>
        <p>
            <label for="Description">
                Description:</label>
            <%= Html.TextArea("Description", Model.Description, 3, 50, null)%>
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
            <%= Html.TextBox("Quantity", String.Format("{0:F}", Model.Quantity), new { maxlength = 10 })%>
            <%= Html.ValidationMessage("Quantity", "*") %>
        </p>
        <p>
            <label for="EstimatedValue">
                Estimated Value:</label>
            <%= Html.TextBox("EstimatedValue", String.Format("{0:F}", Model.EstimatedValue), new { maxlength = 10 })%>
            <%= Html.ValidationMessage("EstimatedValue", "*")%>
        </p>
        <p>
            <label for="Notes">
                Notes:</label>
            <%= Html.TextArea("Notes", Model.Notes, 3, 50, null) %>
            <%= Html.ValidationMessage("Notes", "*") %>
        </p>
        <p>
            <input type="submit" value="Save" /><br />
            <a onclick="deleteRecord(<%= Model.Id %>)" href="javascript:void(0);">Delete this procurement
                item</a>
        </p>
    </fieldset>
    <% } %>
    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>
</asp:Content>
