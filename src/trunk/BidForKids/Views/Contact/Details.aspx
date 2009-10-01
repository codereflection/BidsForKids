<%@ Page Title="Business or Contact Details" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<BidForKids.Models.Contact>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Business or Contact Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Business or Contact Details</h2>

    <fieldset>
        <legend>Fields</legend>
        <p>
            Contact_ID:
            <%= Html.Encode(Model.Contact_ID) %>
        </p>
        <p>
            Business Name:
            <%= Html.Encode(Model.BusinessName) %>
        </p>
        <p>
            First Name:
            <%= Html.Encode(Model.FirstName) %>
        </p>
        <p>
            Last Name:
            <%= Html.Encode(Model.LastName) %>
        </p>
        <p>
            Address:
            <%= Html.Encode(Model.Address) %>
        </p>
        <p>
            City:
            <%= Html.Encode(Model.City) %>
        </p>
        <p>
            State:
            <%= Html.Encode(Model.State) %>
        </p>
        <p>
            Zip Code:
            <%= Html.Encode(Model.ZipCode) %>
        </p>
        <p>
            Phone 1:
            <%= Html.Encode(Model.Phone1) %>
        </p>
        <p>
            Phone 1 Desc:
            <%= Html.Encode(Model.Phone1Desc) %>
        </p>
        <p>
            Phone 2:
            <%= Html.Encode(Model.Phone2) %>
        </p>
        <p>
            Phone 2 Desc:
            <%= Html.Encode(Model.Phone2Desc) %>
        </p>
        <p>
            Phone 3:
            <%= Html.Encode(Model.Phone3) %>
        </p>
        <p>
            Phone 3 Desc:
            <%= Html.Encode(Model.Phone3Desc) %>
        </p>
        <p>
            Notes:
            <%= Html.Encode(Model.Notes) %>
        </p>
    </fieldset>
    <p>

        <%=Html.ActionLink("Edit", "Edit", new { id=Model.Contact_ID }) %> |
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

