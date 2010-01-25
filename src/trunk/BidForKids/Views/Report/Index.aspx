<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Reports
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Reports</h2>
    <ul>
        <li>Create New Report</li>
        <li>Saved Reports
            <ul>
                <li>Report 1</li>
                <li>Report 2</li>
                <li>Report 3</li>
            </ul>
        </li>
    </ul>
</asp:Content>
