@using System.Globalization
@model $rootnamespace$.Areas.MvcMembership.Models.UserAdministration.DetailsViewModel
@{
	ViewBag.Title = "User Details: " + Model.DisplayName;
	Layout = "~/Views/Shared/_Layout.cshtml";
}

<link href='@Url.Content("~/Content/MvcMembership.css")' rel="stylesheet" type="text/css" />

<h2 class="mvcMembership">User Details: @Model.DisplayName [@Model.Status]</h2>

<ul class="mvcMembership-tabs">
	<li>@Html.ActionLink("Details", "Details", "UserAdministration", new { id = Model.User.ProviderUserKey }, null)</li>
	<li>@Html.ActionLink("Password", "Password", "UserAdministration", new { id = Model.User.ProviderUserKey }, null)</li>
	<li>Roles</li>
</ul>

<h3 class="mvcMembership">Roles</h3>
<div class="mvcMembership-userRoles">
	<ul class="mvcMembership">
		@foreach(var role in Model.Roles){
		<li>
			@Html.ActionLink(role.Key, "Role", new{id = role.Key})
			@if(role.Value){
				using(Html.BeginForm("RemoveFromRole", "UserAdministration", new{id = Model.User.ProviderUserKey, role = role.Key})){
				<input type="submit" value="Remove From" />
				}
			}else{
				using(Html.BeginForm("AddToRole", "UserAdministration", new{id = Model.User.ProviderUserKey, role = role.Key})){
				<input type="submit" value="Add To" />
				}
			}
		</li>
		}
	</ul>
</div>
