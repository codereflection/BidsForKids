<%@ Page Title="Auction Items" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Auction Items
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Auction Items grouped by Auction Number</h2>
    <div id="container">
        <%= ViewData["AuctionItems"] %>
    </div>
</asp:Content>
