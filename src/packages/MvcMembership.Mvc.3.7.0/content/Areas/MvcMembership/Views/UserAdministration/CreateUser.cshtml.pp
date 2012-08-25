﻿@model $rootnamespace$.Areas.MvcMembership.Models.UserAdministration.CreateUserViewModel
@{
	ViewBag.Title = "Create New User";
	Layout = "~/Views/Shared/_Layout.cshtml";
}

<link href='@Url.Content("~/Content/MvcMembership.css")' rel="stylesheet" type="text/css" />

<h2 class="mvcMembership">Create New User</h2>

<div class="mvcMembership-account">
	@using(Html.BeginForm("CreateUser", "UserAdministration")){ 
		@Html.ValidationSummary(true)

	 	<fieldset>
			@Html.EditorForModel()
		</fieldset>

		if (Model.InitialRoles.Count > 0) {
			<fieldset>
				<h3 class="mvcMembership">Initial Roles</h3>
				@for(var i = 0; i < Model.InitialRoles.Count; i++){ 
					var role = Model.InitialRoles.ElementAt(i);
					<div>
						<input name="InitialRoles[@i].Key" type="hidden" value="@role.Key" />
						<label>@Html.CheckBox("InitialRoles[" + i + "].Value", role.Value) @role.Key</label>
					</div>
				}
			</fieldset>
		}
	 
		<input type="submit" value="Create" />
	} 
</div>
