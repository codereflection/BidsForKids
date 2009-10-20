<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Reports
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Reports</h2>
    <ul>
        <li><%= Html.ActionLink("2008 Procurement List", "ProcurementList", "Procurement", new { Year = "2008" }, null)%></li>
        <li><%= Html.ActionLink("2009 Procurement List", "ProcurementList", "Procurement", new { Year = "2009" }, null)%></li>
        <li><%= Html.ActionLink("2010 Procurement List", "ProcurementList", "Procurement", new { Year = "2010" }, null)%></li>
    </ul>
</asp:Content>
