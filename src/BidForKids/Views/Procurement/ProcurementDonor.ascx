<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BidsForKids.ViewModels.ProcurementDonorViewModel>" %>
<%@ Import Namespace="BidsForKids.ViewModels" %>
<li>
    <%= Html.DropDownList("DonorId", (IEnumerable<SelectListItem>)ViewData["Donor-" + Model.Id])%>
    &nbsp;<%= Html.ActionLink("view", "Edit", "Donor", new
                                                                    {
                                                                        id = Model.Id
                                                                    }, new
                                                                           {
                                                                               id = "ViewDonor", 
                                                                               target = "_blank", 
                                                                               title = "View donor in a new window"
                                                                           })%>
</li>
