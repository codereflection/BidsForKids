﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BidsForKids.ViewModels.ProcurementDonorViewModel>" %>

<li id="donor_<%= Model.Id %>">
    <%= Html.DropDownList("DonorId_1", (IEnumerable<SelectListItem>)ViewData["Donor-" + Model.Id], string.Empty)%>
    &nbsp;<%= Html.ActionLink("view", "Edit", "Donor", new
                                                                    {
                                                                        id = Model.Id
                                                                    }, new
                                                                           {
                                                                               id = "ViewDonor", 
                                                                               target = "_blank", 
                                                                               title = "View donor in a new window"
                                                                           })%>
    &nbsp;<a title="remove" href="#" tag="<%= Model.Id %>" id="removeDonor">remove</a>
</li>
