<%@ Page Title="Admin Menu" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="adminTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Admin Menu
</asp:Content>
<asp:Content ID="adminContent" ContentPlaceHolderID="MainContent" runat="server">        
    <p>
    <%= Html.ActionLink("Businesses & Contacts", "Index", "Contact")%>
    </p>
</asp:Content>
