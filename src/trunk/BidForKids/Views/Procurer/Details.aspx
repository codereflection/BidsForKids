<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BidForKids.Models.Procurer>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

    <fieldset>
        <legend>Fields</legend>
        <p>
            Procurer_ID:
            <%= Html.Encode(Model.Procurer_ID) %>
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
            Phone:
            <%= Html.Encode(Model.Phone) %>
        </p>
        <p>
            Email:
            <%= Html.Encode(Model.Email) %>
        </p>
    </fieldset>
    <p>

        <%=Html.ActionLink("Edit", "Edit", new { id=Model.Procurer_ID }) %> |
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

