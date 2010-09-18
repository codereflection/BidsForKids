<%@ Page Title="Add new business" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<BidsForKids.Data.Models.Donor>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Add new business
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Add new business</h2>
    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
    <% using (Html.BeginForm())
       {%>
    <fieldset>
        <legend>Fields</legend>
        <p>
            <label for="GeoLocation_ID">
                Geographic Location
            </label>
            <%= Html.DropDownList("GeoLocation_ID", "")%>&nbsp;<%= Html.ActionLink("new", "Create", "GeoLocation", new { ReturnTo = Server.UrlEncode("Donor/Create") }, null)%>
        </p>
        <p>
            <label for="Procurer_ID">
                Default Procurer
            </label>
            <%= Html.DropDownList("Procurer_ID", "") %>
        </p>
        <p>
            <label for="Donates">
                Donates:</label>
            <%= Html.DropDownList("Donates") %>
            <%= Html.ValidationMessage("Donates", "") %>
        </p>
        <p>
            <label for="MailedPacket">
                MailedPacket:</label>
            <%= Html.CheckBox("MailedPacket") %>
            <%= Html.ValidationMessage("MailedPacket", "") %>
        </p>
        <p>
            <label for="BusinessName">
                Business Name:</label>
            <%= Html.TextBox("BusinessName") %>
            <%= Html.ValidationMessage("BusinessName", "*") %>
        </p>
        <p>
            <label for="FirstName">
                First Name:</label>
            <%= Html.TextBox("FirstName") %>
            <%= Html.ValidationMessage("FirstName", "*") %>
        </p>
        <p>
            <label for="LastName">
                Last Name:</label>
            <%= Html.TextBox("LastName") %>
            <%= Html.ValidationMessage("LastName", "*") %>
        </p>
        <p>
            <label for="Address">
                Address:</label>
            <%= Html.TextBox("Address") %>
            <%= Html.ValidationMessage("Address", "*") %>
        </p>
        <p>
            <label for="City">
                City:</label>
            <%= Html.TextBox("City") %>
            <%= Html.ValidationMessage("City", "*") %>
        </p>
        <p>
            <label for="State">
                State:</label>
            <%= Html.TextBox("State") %>
            <%= Html.ValidationMessage("State", "*") %>
        </p>
        <p>
            <label for="ZipCode">
                Zip Code:</label>
            <%= Html.TextBox("ZipCode") %>
            <%= Html.ValidationMessage("ZipCode", "*") %>
        </p>
        <p>
            <label for="Phone1">
                Phone 1:</label>
            <%= Html.TextBox("Phone1") %>
            <%= Html.ValidationMessage("Phone1", "*") %>
        </p>
        <p>
            <label for="Phone1Desc">
                Phone 1 Desc:</label>
            <%= Html.TextBox("Phone1Desc") %>
            <%= Html.ValidationMessage("Phone1Desc", "*") %>
        </p>
        <p>
            <label for="Phone2">
                Phone 2:</label>
            <%= Html.TextBox("Phone2") %>
            <%= Html.ValidationMessage("Phone2", "*") %>
        </p>
        <p>
            <label for="Phone2Desc">
                Phone 2 Desc:</label>
            <%= Html.TextBox("Phone2Desc") %>
            <%= Html.ValidationMessage("Phone2Desc", "*") %>
        </p>
        <p>
            <label for="Phone3">
                Phone 3:</label>
            <%= Html.TextBox("Phone3") %>
            <%= Html.ValidationMessage("Phone3", "*") %>
        </p>
        <p>
            <label for="Phone3Desc">
                Phone 3 Desc:</label>
            <%= Html.TextBox("Phone3Desc") %>
            <%= Html.ValidationMessage("Phone3Desc", "*") %>
        </p>
        <p>
            <label for="Email">
                Email:</label>
            <%= Html.TextBox("Email") %>
            <%= Html.ValidationMessage("Email", "*") %>
        </p>
        <p>
            <label for="Website">
                Website:</label>
            <%= Html.TextBox("Website")%>
            <%= Html.ValidationMessage("Website", "*")%>
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
