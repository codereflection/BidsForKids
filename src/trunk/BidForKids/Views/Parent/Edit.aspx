<%@ Page Title="Edit Parent" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<BidForKids.Models.Donor>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit Parent
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Edit Parent</h2>
    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>
    <% using (Html.BeginForm())
       {%>
    <fieldset>
        <legend>Fields</legend>
        <p>
            <label for="FirstName">
                First Name:</label>
            <%= Html.TextBox("FirstName", Model.FirstName) %>
            <%= Html.ValidationMessage("FirstName", "*") %>
        </p>
        <p>
            <label for="LastName">
                Last Name:</label>
            <%= Html.TextBox("LastName", Model.LastName) %>
            <%= Html.ValidationMessage("LastName", "*") %>
        </p>
        <p>
            <label for="Address">
                Address:</label>
            <%= Html.TextBox("Address", Model.Address) %>
            <%= Html.ValidationMessage("Address", "*") %>
        </p>
        <p>
            <label for="City">
                City:</label>
            <%= Html.TextBox("City", Model.City) %>
            <%= Html.ValidationMessage("City", "*") %>
        </p>
        <p>
            <label for="State">
                State:</label>
            <%= Html.TextBox("State", Model.State) %>
            <%= Html.ValidationMessage("State", "*") %>
        </p>
        <p>
            <label for="ZipCode">
                Zip Code:</label>
            <%= Html.TextBox("ZipCode", Model.ZipCode) %>
            <%= Html.ValidationMessage("ZipCode", "*") %>
        </p>
        <p>
            <label for="Phone1">
                Phone 1:</label>
            <%= Html.TextBox("Phone1", Model.Phone1) %>
            <%= Html.ValidationMessage("Phone1", "*") %>
        </p>
        <p>
            <label for="Phone1Desc">
                Phone 1 Desc:</label>
            <%= Html.TextBox("Phone1Desc", Model.Phone1Desc) %>
            <%= Html.ValidationMessage("Phone1Desc", "*") %>
        </p>
        <p>
            <label for="Phone2">
                Phone 2:</label>
            <%= Html.TextBox("Phone2", Model.Phone2) %>
            <%= Html.ValidationMessage("Phone2", "*") %>
        </p>
        <p>
            <label for="Phone2Desc">
                Phone 2 Desc:</label>
            <%= Html.TextBox("Phone2Desc", Model.Phone2Desc) %>
            <%= Html.ValidationMessage("Phone2Desc", "*") %>
        </p>
        <p>
            <label for="Phone3">
                Phone 3:</label>
            <%= Html.TextBox("Phone3", Model.Phone3) %>
            <%= Html.ValidationMessage("Phone3", "*") %>
        </p>
        <p>
            <label for="Phone3Desc">
                Phone 3 Desc:</label>
            <%= Html.TextBox("Phone3Desc", Model.Phone3Desc) %>
            <%= Html.ValidationMessage("Phone3Desc", "*") %>
        </p>
        <p>
            <label for="Email">
                Email:</label>
            <%= Html.TextBox("Email", Model.Email) %>
            <%= Html.ValidationMessage("Email", "*") %>
        </p>
        <p>
            <label for="Notes">
                Notes:</label>
            <%= Html.TextArea("Notes", Model.Notes, 3, 50, null) %>
            <%= Html.ValidationMessage("Notes", "*") %>
        </p>
        <p>
            Created On:
            <%= Html.Encode(Model.CreatedOn) %>&nbsp;|&nbsp;Modified On:
            <%= Html.Encode(Model.ModifiedOn) %>
        </p>
        <p>
            <input type="submit" value="Save" />
        </p>
    </fieldset>
    <% } %>
    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>
</asp:Content>
