<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="procurementTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Procurements
</asp:Content>

<asp:Content ID="procurementContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.Encode(ViewData["Message"]) %></h2>
</asp:Content>
