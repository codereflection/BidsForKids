<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BidForKids.Models.Procurement>" %>
<tr>
    <td>
        <%= Html.ActionLink("Edit", "Edit", new { id = Model.Procurement_ID })%>
        |
        <%= Html.ActionLink("Details", "Details", new { id = Model.Procurement_ID })%>
    </td>
    <td>
        <%= Html.Encode(Model.CatalogNumber)%>
    </td>
    <td>
        <%= Html.Encode(Model.Description)%>
    </td>
    <td>
        <%= Html.Encode(String.Format("{0:F}", Model.Quantity))%>
    </td>
    <td>
        <%= Html.Encode(String.Format("{0:F}", Model.EstimatedValue))%>
    </td>
    <td>
        <%= Html.Encode(Model.Notes)%>
    </td>
    <td>
        <% if (Model.ContactProcurement != null)
           { 
               Response.Write(Html.Encode(Model.ContactProcurement.Auction.Year.ToString())); 
           } else { Response.Write(Html.Encode("Unknown")); } %>
    </td>
</tr>
