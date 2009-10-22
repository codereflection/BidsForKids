<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BidForKids.Models.Procurement>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="../../Scripts/MicrosoftAjax.js"></script>

    <script type="text/javascript">
        function deleteRecord(id) {
            var answer = confirm("Are you sure you want to delete this procurement?");
            if (answer === false)
                return;
                
            var action = "/Procurement/Delete/" + id;

            var request = new Sys.Net.WebRequest();
            request.set_httpVerb("DELETE");
            request.set_url(action);
            request.add_completed(deleteCompleted);
            request.invoke();
        }

        function deleteCompleted(c1, c2, c3) {
            if (c1._xmlHttpRequest.responseText == "True") {
                alert("Procurement deleted.");
                window.location = "/Procurement";
            }
            else {
                alert("Unknown result: " + c1._xmlHttpRequest.ResponseText);
            }
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
            <label for="Procurement_ID">
                Procurement_ID:</label>
            <%= Html.Encode(Model.Procurement_ID) %>
        </p>
        <p>
            <label for="Category_ID">
                Category
            </label>
            <%= Html.DropDownList("Category_ID", "")%>
        </p>
        <p>
            <label for="CatalogNumber">
                Catalog #:</label>
            <%= Html.TextBox("CatalogNumber", Model.CatalogNumber)%>
            <%= Html.ValidationMessage("CatalogNumber", "*")%>
        </p>
        <p>
            <label for="AuctionNumber">
                Auction #:</label>
            <%= Html.TextBox("AuctionNumber", Model.AuctionNumber)%>
            <%= Html.ValidationMessage("AuctionNumber", "*")%>
        </p>
        <p>
            <label for="ItemNumber">
                Item #:</label>
            <%= Html.TextBox("ItemNumber", Model.ItemNumber)%>
            <%= Html.ValidationMessage("ItemNumber", "*")%>
        </p>
        <p>
            <label for="Description">
                Description:</label>
            <%= Html.TextBox("Description", Model.Description) %>
            <%= Html.ValidationMessage("Description", "*") %>
        </p>
        <p>
            <label for="Quantity">
                Quantity:</label>
            <%= Html.TextBox("Quantity", String.Format("{0:F}", Model.Quantity)) %>
            <%= Html.ValidationMessage("Quantity", "*") %>
        </p>
        <p>
            <label for="EstimatedValue">
                Estimated Value:</label>
            <%= Html.TextBox("EstimatedValue", String.Format("{0:F}", Model.EstimatedValue))%>
            <%= Html.ValidationMessage("EstimatedValue", "*")%>
        </p>
        <p>
            <label for="Per Item Value">
                PerItemValue:</label>
            <%= Html.TextBox("PerItemValue", String.Format("{0:F}", Model.PerItemValue)) %>
            <%= Html.ValidationMessage("PerItemValue", "*") %>
        </p>
        <p>
            <label for="SoldFor">
                Sold For:</label>
            <%= Html.TextBox("SoldFor", String.Format("{0:F}", Model.SoldFor))%>
            <%= Html.ValidationMessage("SoldFor", "*")%>
        </p>
        <p>
            <label for="Auction_ID">
                Year</label>
            <%= Html.DropDownList("Auction_ID","")%>
        </p>
        <p>
            <label for="Donor_ID">
                Business/Person
            </label>
            <%= Html.DropDownList("Donor_ID", "")%>
        </p>
        <p>
            <label for="GeoLocation_ID">
                Geographic Location
            </label>
            <%= Html.DropDownList("GeoLocation_ID", "")%>
        </p>
        <p>
            <label for="Procurer_ID">
                Procurer
            </label>
            <%= Html.DropDownList("Procurer_ID", "")%>
        </p>
        <p>
            <label for="Notes">
                Notes:</label>
            <%= Html.TextArea("Notes") %>
            <%= Html.ValidationMessage("Notes", "*") %>
        </p>
        <p>
            <input type="submit" value="Save" /><br />
            <a onclick="deleteRecord(<%= Model.Procurement_ID %>)" href="javascript:void(0);">Delete
                this procurement item</a>
        </p>
    </fieldset>
    <% } %>
    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>
</asp:Content>
