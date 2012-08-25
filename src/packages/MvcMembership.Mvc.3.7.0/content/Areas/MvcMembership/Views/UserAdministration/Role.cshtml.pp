@model $rootnamespace$.Areas.MvcMembership.Models.UserAdministration.RoleViewModel
@{
	ViewBag.Title = "Role: " + Model.Role;
	Layout = "~/Views/Shared/_Layout.cshtml";
}

<link href='@Url.Content("~/Content/MvcMembership.css")' rel="stylesheet" type="text/css" />

<h2 class="mvcMembership">Role: @Model.Role</h2>
<div class="mvcMembership-roleUsers">
	@if(Model.Users.Count() > 0){
		<ul class="mvcMembership">
			@foreach(var key in Model.Users.Keys){ var user = Model.Users[key];
			<li>
				@if(user == null){
					<span>@key <em>(Deleted)</em></span>
				}else{
					@Html.ActionLink(user.UserName, "Details", new{id=user.ProviderUserKey})
					using(Html.BeginForm("RemoveFromRole", "UserAdministration", new{id = user.ProviderUserKey, role = Model.Role})){
						<input type="submit" value="Remove From" />
					}
				}
			</li>
			}
		</ul>
	}else{
	<p>No users are in this role.</p>
	}
</div>
