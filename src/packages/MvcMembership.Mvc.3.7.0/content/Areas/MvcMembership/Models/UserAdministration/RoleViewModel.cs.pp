using System.Collections.Generic;
using System.Web.Security;

namespace $rootnamespace$.Areas.MvcMembership.Models.UserAdministration
{
	public class RoleViewModel
	{
		public string Role { get; set; }
		public IDictionary<string, MembershipUser> Users { get; set; }
	}
}
