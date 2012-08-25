@using System.Globalization
@model $rootnamespace$.Areas.MvcMembership.Models.UserAdministration.DetailsViewModel
@{
	ViewBag.Title = "User Details: " + Model.DisplayName;
	Layout = "~/Views/Shared/_Layout.cshtml";
}

<link href='@Url.Content("~/Content/MvcMembership.css")' rel="stylesheet" type="text/css" />

<h2 class="mvcMembership">User Details: @Model.DisplayName [@Model.Status]</h2>

<ul class="mvcMembership-tabs">
	<li>@Html.ActionLink("Details", "Details", "UserAdministration", new {id = Model.User.ProviderUserKey }, null)</li>
	<li>Password</li>
	@if(Model.IsRolesEnabled){
	<li>@Html.ActionLink("Roles", "UsersRoles", "UserAdministration", new{id = Model.User.ProviderUserKey}, null)</li>
	}
</ul>

<h3 class="mvcMembership">Password</h3>
<div class="mvcMembership-password">
	@if(Model.User.IsLockedOut){
		<p>Locked out since @Model.User.LastLockoutDate.ToString("MMMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture)</p>
		using(Html.BeginForm("Unlock", "UserAdministration", new{ id = Model.User.ProviderUserKey })){
		<input type="submit" value="Unlock Account" />
		}
	}else{

		if(Model.User.LastPasswordChangedDate == Model.User.CreationDate){
		<dl class="mvcMembership">
			<dt>Last Changed:</dt>
			<dd><em>Never</em></dd>
		</dl>
		}else{
		<dl class="mvcMembership">
			<dt>Last Changed:</dt>
			<dd>@Model.User.LastPasswordChangedDate.ToString("MMMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture)</dd>
		</dl>
		}

		if(Model.CanResetPassword && Model.RequirePasswordQuestionAnswerToResetPassword){
			using(Html.BeginForm("ResetPasswordWithAnswer", "UserAdministration", new{ id = Model.User.ProviderUserKey })){
			<fieldset>
				<p>
					<dl class="mvcMembership">
						<dt>Password Question:</dt>
						if(string.IsNullOrEmpty(Model.User.PasswordQuestion) || string.IsNullOrEmpty(Model.User.PasswordQuestion.Trim())){
						<dd><em>No password question defined.</em></dd>
						}else{
						<dd>@Model.User.PasswordQuestion</dd>
						}
					</dl>
				</p>
				<p>
					<label for="answer">Password Answer:</label>
					@Html.TextBox("answer")
				</p>
				<input type="submit" value="Reset to Random Password and Email User" />
			</fieldset>
			}
		}else if(Model.CanResetPassword){
			using(Html.BeginForm("SetPassword", "UserAdministration", new{ id = Model.User.ProviderUserKey })){
			<fieldset>
				<p>
					<label for="password">New Password:</label>
					@Html.TextBox("password")
				</p>
				<input type="submit" value="Change Password" />
			</fieldset>
			}
			using(Html.BeginForm("ResetPassword", "UserAdministration", new{ id = Model.User.ProviderUserKey })){
			<fieldset>
				<input type="submit" value="Reset to Random Password and Email User" />
			</fieldset>
			}
		}

	}
</div>
