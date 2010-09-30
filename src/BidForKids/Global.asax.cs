using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BidsForKids.Configuration;
using BidsForKids.Data.Repositories;
using StructureMap;

namespace BidsForKids
{
    public class MvcApplication : HttpApplication
    {
        private static IUnitOfWork _unitOfWork;

        public static IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
            Bootstrapper.ConfigureStructureMap();
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            _unitOfWork = ObjectFactory.GetInstance<IUnitOfWork>();
        }
        
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (_unitOfWork != null)
                _unitOfWork.Dispose();
        }
    }
}