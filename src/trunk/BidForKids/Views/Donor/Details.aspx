<%@ Page Title="Business or Donor Details" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master"
    Inherits="System.Web.Mvc.ViewPage<BidForKids.Models.Donor>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Donor Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Donor Details</h2>
    <fieldset>
        <legend>Fields</legend>
        <p>
            Donor_ID:
            <%= Html.Encode(Model.Donor_ID) %>
        </p>
        <p>
            Donates:
            <%= Html.Encode(Model.Donates) %>
        </p>
        <p>
            Mailed Packet:
            <%= Html.Encode(Model.MailedPacket) %>
        </p>
        <p>
            Business Name:
            <%= Html.Encode(Model.BusinessName) %>
        </p>
        <p>
            First Name:
            <%= Html.Encode(Model.FirstName) %>
        </p>
        <p>
            Last Name:
            <%= Html.Encode(Model.LastName) %>
        </p>
        <p>
            Address:
            <%= Html.Encode(Model.Address) %>
        </p>
        <p>
            City:
            <%= Html.Encode(Model.City) %>
        </p>
        <p>
            State:
            <%= Html.Encode(Model.State) %>
        </p>
        <p>
            Zip Code:
            <%= Html.Encode(Model.ZipCode) %>
        </p>
        <p>
            Phone 1:
            <%= Html.Encode(Model.Phone1) %>
        </p>
        <p>
            Phone 1 Desc:
            <%= Html.Encode(Model.Phone1Desc) %>
        </p>
        <p>
            Phone 2:
            <%= Html.Encode(Model.Phone2) %>
        </p>
        <p>
            Phone 2 Desc:
            <%= Html.Encode(Model.Phone2Desc) %>
        </p>
        <p>
            Phone 3:
            <%= Html.Encode(Model.Phone3) %>
        </p>
        <p>
            Phone 3 Desc:
            <%= Html.Encode(Model.Phone3Desc) %>
        </p>
        <p>
            Email:
            <%= Html.Encode(Model.Email) %>
        </p>
        <p>
            Website:
            <%= Html.Encode(Model.Website) %>
        </p>
        <p>
            Notes:
            <%= Html.Encode(Model.Notes) %>
        </p>
        <p>
            Created On:
            <%= Html.Encode(Model.CreatedOn) %>
        </p>
        <p>
            Modified On:
            <%= Html.Encode(Model.ModifiedOn) %></p>
        <p>
            Procurer:
            <%= Html.Encode(Model.Procurer == null ? "" : Model.Procurer.FirstName + " " + Model.Procurer.LastName)  %></p>
        <p>
            Geo Location:
            <%= Html.Encode(Model.GeoLocation == null ? "" : Model.GeoLocation.GeoLocationName) %>
        </p>
    </fieldset>
    <p>
        <%=Html.ActionLink("Edit", "Edit", new { id=Model.Donor_ID }) %>
        |
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>
</asp:Content>
