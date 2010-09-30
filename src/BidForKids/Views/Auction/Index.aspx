<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<BidsForKids.Data.Models.Auction>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<h2>Index</h2>

	<table>
		<tr>
			<th></th>
			<th>
				Auction_ID
			</th>
			<th>
				Year
			</th>
			<th>
				Name
			</th>
		</tr>

	<% foreach (var item in Model) { %>
	
		<tr>
			<td>
				<%: Html.ActionLink("Edit", "Edit", new { id=item.Auction_ID }) %> |
				<%: Html.ActionLink("Details", "Details", new { id=item.Auction_ID })%> |
				<%: Html.ActionLink("Delete", "Delete", new { id=item.Auction_ID })%>
			</td>
			<td>
				<%: item.Auction_ID %>
			</td>
			<td>
				<%: item.Year %>
			</td>
			<td>
				<%: item.Name %>
			</td>
		</tr>
	
	<% } %>

	</table>

	<p>
		<%: Html.ActionLink("Create New", "Create") %>
	</p>

</asp:Content>

