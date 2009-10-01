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
            <label for="Code">
                Code:</label>
            <%= Html.TextBox("Code") %>
            <%= Html.ValidationMessage("Code", "*") %>
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
            <label for="PerItemValue">
                Per Item Value:</label>
            <%= Html.TextBox("PerItemValue") %>
            <%= Html.ValidationMessage("PerItemValue", "*") %>
        </p>
        <p>
            <label for="Auctions">
                Year</label>
            <%= Html.DropDownList("Auctions")%>
        </p>
        <p>
            <label for="Contacts">Business/Person
            </label>
            <%= Html.DropDownList("Contacts") %>
        </p>
        <p>
            <label for="Notes">
                Notes:</label>
            <%= Html.TextArea("Notes") %>
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
