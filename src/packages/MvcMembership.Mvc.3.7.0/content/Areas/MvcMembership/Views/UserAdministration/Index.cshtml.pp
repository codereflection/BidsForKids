@using PagedList.Mvc
@model $rootnamespace$.Areas.MvcMembership.Models.UserAdministration.IndexViewModel
@{
	ViewBag.Title = "User Administration";
	Layout = "~/Views/Shared/_Layout.cshtml";
}

<link href='@Url.Content("~/Content/MvcMembership.css")' rel="stylesheet" type="text/css" />
<link href='@Url.Content("~/Content/PagedList.css")' rel="stylesheet" type="text/css" />

@if(!Model.IsRolesEnabled){
	<p class="mvcMembership-warning">WARNING: Your Role Provider is currently disabled - any user may currently visit this page.<br />Change &lt;roleManager enabled="false"&gt; to &lt;roleManager enabled="true"&gt; in your web.config file.</p>
}

<h2 class="mvcMembership">User Administration</h2>

<h3 class="mvcMembership">Find a User
	@if (!string.IsNullOrWhiteSpace(Model.Search)) { 
		<span>(@Html.ActionLink("Clear Search", "Index"))</span>
	}
</h3>
<form method="get" class="mvcMembership-searchForm">
	<fieldset>
		<label>
			User Name or Email Address:
			<input type="text" name="search" value="@Model.Search" />
			<input type="submit" value="Search" />
		</label>
	</fieldset>
</form>
	
<h3 class="mvcMembership">Users (@Html.ActionLink("New User", "CreateUser", "UserAdministration"))</h3>
<div class="mvcMembership-allUsers">
@if(Model.Users.Count > 0){
	<ul class="mvcMembership mvcMembership-users">
		@foreach(var user in Model.Users){
		<li>
			<span class="mvcMembership-username">@Html.ActionLink(user.UserName, "Details", new{ id = user.ProviderUserKey})</span>
			<span class="mvcMembership-email"><a href="mailto:@user.Email">@user.Email</a></span>
			@if(user.IsOnline){
				<span class="mvcMembership-isOnline">Online</span>
			}else{
				<span class="mvcMembership-isOffline">Offline for
					@{
						var offlineSince = (DateTime.Now - user.LastActivityDate);
						if (offlineSince.TotalSeconds <= 60){
							<text>1 minute.</text>
						}else if(offlineSince.TotalMinutes < 60){
							<text>@Math.Floor(offlineSince.TotalMinutes) minutes.</text>
						}else if (offlineSince.TotalMinutes < 120){
							<text>1 hour</text>
						}else if (offlineSince.TotalHours < 24){
							<text>@Math.Floor(offlineSince.TotalHours) hours.</text>
						}else if (offlineSince.TotalHours < 48){
							<text>1 day.</text>
						}else{
							<text>@Math.Floor(offlineSince.TotalDays) days.</text>
						}
					}
				</span>
			}
			@if(!string.IsNullOrEmpty(user.Comment)){
				<span class="mvcMembership-comment">@user.Comment</span>
			}
		</li>
		}
	</ul>
	@Html.PagedListPager(Model.Users, page => Url.Action("Index", new { page, search = Model.Search }))
}else{
	<p>No users have registered.</p>
}
</div>

@if(Model.IsRolesEnabled){
	<h3 class="mvcMembership">Roles</h3>
	<div class="mvcMembership-allRoles">
	@if(Model.Roles.Count() > 0 ){
		<ul class="mvcMembership">
			@foreach(var role in Model.Roles){
			<li>
				@Html.ActionLink(role, "Role", new{id = role})
				@using(Html.BeginForm("DeleteRole", "UserAdministration", new{id=role})){
				<input type="submit" value="Delete" />
				}
			</li>
			}
		</ul>
	}else{
		<p>No roles have been created.</p>
	}

	@using(Html.BeginForm("CreateRole", "UserAdministration")){
		<fieldset>
			<label for="id">Role:</label>
			@Html.TextBox("id")
			<input type="submit" value="Create Role" />
		</fieldset>
	}
	</div>
}
