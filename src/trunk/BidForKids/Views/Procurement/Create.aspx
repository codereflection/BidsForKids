<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BidForKids.Models.Procurement>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Create</h2>
    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
    <% using (Html.BeginForm())
       {%>
    <fieldset>
        <legend>Fields</legend>
        <p>
            <label for="Donor_ID">
                Donor
            </label>
            <%= Html.DropDownList("Donor_ID") %>&nbsp;<%= Html.ActionLink("new", "Create", "Donor", new { ReturnTo = Server.UrlEncode("Procurement/Create") }, null)%>
        </p>
        <p>
            <label for="Auction_ID">
                Year</label>
            <%= Html.DropDownList("Auction_ID")%>
        </p>
        <p>
            <label for="Catalog #">
                Catalog #:</label>
            <%= Html.TextBox("CatalogNumber")%>
            <%= Html.ValidationMessage("CatalogNumber", "*")%>
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
            <%= Html.TextBox("AuctionNumber")%>
            <%= Html.ValidationMessage("AuctionNumber", "*")%>
        </p>
        <p>
            <label for="ItemNumber">
                Item #:</label>
            <%= Html.TextBox("ItemNumber")%>
            <%= Html.ValidationMessage("ItemNumber", "*")%>
        </p>
        <p>
            <label for="Description">
                Description:</label>
            <%= Html.TextBox("Description") %>
            <%= Html.ValidationMessage("Description", "*") %>
        </p>
        <p>
            <label for="Quantity">
                Quantity:</label>
            <%= Html.TextBox("Quantity") %>
            <%= Html.ValidationMessage("Quantity", "*") %>
        </p>
        <p>
            <label for="EstimatedValue">
                Estimated Value:</label>
            <%= Html.TextBox("EstimatedValue")%>
            <%= Html.ValidationMessage("EstimatedValue", "*")%>
        </p>
        <p>
            <label for="SoldFor">
                Sold For:</label>
            <%= Html.TextBox("SoldFor")%>
            <%= Html.ValidationMessage("SoldFor", "*")%>
        </p>
        <p>
            <label for="Notes">
                Notes:</label>
            <%= Html.TextArea("Notes", "", 3, 50, null) %>
            <%= Html.ValidationMessage("Notes", "*") %>
        </p>
        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
    <% } %>
    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>
</asp:Content>
