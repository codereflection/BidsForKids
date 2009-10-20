<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BidForKids.Models.Procurer>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Edit</h2>
    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>
    <% using (Html.BeginForm())
       {%>
    <fieldset>
        <legend>Fields</legend>
        <p>
            Procurer_ID:
            <%= Model.Procurer_ID %>
        </p>
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
            <label for="Phone">
                Phone:</label>
            <%= Html.TextBox("Phone", Model.Phone) %>
            <%= Html.ValidationMessage("Phone", "*") %>
        </p>
        <p>
            <label for="Email">
                Email:</label>
            <%= Html.TextBox("Email", Model.Email) %>
            <%= Html.ValidationMessage("Email", "*") %>
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
