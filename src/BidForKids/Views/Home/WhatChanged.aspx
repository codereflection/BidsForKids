<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">
    What's changed in the Bids for Kids database
</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <h1>
        What's changed in the Bids for Kids database
    </h1>
    <br />
    <table>
        <thead>
            <tr>
                <td>
                    Version
                </td>
                <td>
                    Date
                </td>
                <td>
                    Changes
                </td>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>
                    v1.2.1.2
                </td>
                <td>
                    Jan 17th, 2011
                </td>
                <td>
                    Fix Adventures grid to show the donor
                </td>
            </tr>
            <tr>
                <td>
                    v1.2.1.1
                </td>
                <td>
                    Jan 6th, 2011
                </td>
                <td>
                    Allow alphanumeric characters at the end of a donation's item number
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
