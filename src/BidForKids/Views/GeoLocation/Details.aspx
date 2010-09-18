<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<BidsForKids.Data.Models.GeoLocation>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

    <fieldset>
        <legend>Fields</legend>
        <p>
            GeoLocation_ID:
            <%= Html.Encode(Model.GeoLocation_ID) %>
        </p>
        <p>
            Geographic Location Name:
            <%= Html.Encode(Model.GeoLocationName) %>
        </p>
        <p>
            Description:
            <%= Html.Encode(Model.Description) %>
        </p>
    </fieldset>
    <p>

        <%=Html.ActionLink("Edit", "Edit", new { id=Model.GeoLocation_ID }) %> |
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

