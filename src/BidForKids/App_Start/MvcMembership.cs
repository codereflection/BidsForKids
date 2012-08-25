using System.Web.Mvc;
using MvcMembership;

[assembly: WebActivator.PreApplicationStartMethod(typeof(BidsForKids.App_Start.MvcMembership), "Start")]

namespace BidsForKids.App_Start
{
	public static class MvcMembership
	{
		public static void Start()
		{
			GlobalFilters.Filters.Add(new TouchUserOnEachVisitFilter());
		}
	}
}
