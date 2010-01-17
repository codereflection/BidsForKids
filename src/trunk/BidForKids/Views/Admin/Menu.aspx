﻿<%@ Page Title="Admin Menu" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="adminTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Admin Menu
</asp:Content>
<asp:Content ID="adminContent" ContentPlaceHolderID="MainContent" runat="server">        
    <p>
    <%= Html.ActionLink("Businesses", "Index", "Donor")%>
    <br />
    <%= Html.ActionLink("Parents", "Index", "Parent")%>
    <br />
    <%= Html.ActionLink("Procurers", "Index", "Procurer")%>
    </p>
</asp:Content>
