<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BidForKids.Models.Procurement>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Procurement Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Procurement Details</h2>

    <fieldset>
        <legend>Fields</legend>
        <p>
            Code:
            <%= Html.Encode(Model.Code) %>
        </p>
        <p>
            Description:
            <%= Html.Encode(Model.Description) %>
        </p>
        <p>
            Quantity:
            <%= Html.Encode(String.Format("{0:F}", Model.Quantity)) %>
        </p>
        <p>
            Per Item Value:
            <%= Html.Encode(String.Format("{0:F}", Model.PerItemValue)) %>
        </p>
        <p>
            Notes:
            <%= Html.Encode(Model.Notes) %>
        </p>
        <p>
            Database ID:
            <%= Html.Encode(Model.Procurement_ID) %>
        </p>
    </fieldset>
    <p>

        <%=Html.ActionLink("Edit", "Edit", new { id=Model.Procurement_ID }) %> |
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

