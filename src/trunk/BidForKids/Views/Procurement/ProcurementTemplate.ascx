<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BidForKids.Models.Procurement>" %>
<tr>
    <td>
        <%= Html.ActionLink("Edit", "Edit", new { id = Model.Procurement_ID })%>
        |
        <%= Html.ActionLink("Details", "Details", new { id = Model.Procurement_ID })%>
    </td>
    <td>
        <%= Html.Encode(Model.Code)%>
    </td>
    <td>
        <%= Html.Encode(Model.Description)%>
    </td>
    <td>
        <%= Html.Encode(String.Format("{0:F}", Model.Quantity))%>
    </td>
    <td>
        <%= Html.Encode(String.Format("{0:F}", Model.PerItemValue))%>
    </td>
    <td>
        <%= Html.Encode(Model.Notes)%>
    </td>
    <td>
        <%= Html.Encode(Model.Procurement_ID) %>
    </td>
    <td>
        <% if (Model.ContactProcurements != null)
           { 
               Response.Write(Html.Encode(Model.ContactProcurements.Auction.Year.ToString())); 
           } else { Response.Write(Html.Encode("Unknown")); } %>
    </td>
</tr>
