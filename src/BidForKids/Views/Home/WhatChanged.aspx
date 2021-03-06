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
                    v1.2.1.7
                </td>
                <td>
                    Jan 31th, 2012
                </td>
                <td>
                    Added ability to assign multiple donors when entering a new procurement<br />
                    The Description and Donation fields are now required when entering a new procurement<br />
                </td>
            </tr>
            <tr>
                <td>
                    v1.2.1.6
                </td>
                <td>
                    Nov 22nd, 2011
                </td>
                <td>
                    Fixed issue false "item number already exists" error when checking for an existing item number during the entering or editing of a procurement.<br />
                </td>
            </tr>
            <tr>
                <td>
                    v1.2.1.5
                </td>
                <td>
                    Oct 19th, 2011
                </td>
                <td>
                    Added capability to Close & Delete donors. Donors w/ associated procurements cannot be deleted, only Closed.<br />
                    Fixed issue with getting sent back to the wrong page after creating a new procurement.<br />
                </td>
            </tr>
            <tr>
                <td>
                    v1.2.1.4
                </td>
                <td>
                    Jan 30th, 2011
                </td>
                <td>
                    Added Title field to the Auction Item Report<br />
                    Replaced Donation field with Title field in the Auction Item Report - Catalog View<br />
                    Changed "Year" to be a required filter on the Auction Item Report
                </td>
            </tr>
            <tr>
                <td>
                    v1.2.1.3
                </td>
                <td>
                    Jan 24th, 2011
                </td>
                <td>
                    Added 'Title' field to the Procurement
                </td>
            </tr>
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
