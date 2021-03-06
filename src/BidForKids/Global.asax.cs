﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BidsForKids.Configuration;
using BidsForKids.Data.Repositories;
using StructureMap;

namespace BidsForKids
{
    public class WebApplication : HttpApplication
    {
        public IUnitOfWork UnitOfWork { get; private set; }

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
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            Bootstrapper.ConfigureStructureMap();
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            UnitOfWork = ObjectFactory.GetInstance<IUnitOfWork>();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (UnitOfWork == null) return;

            UnitOfWork.SubmitChanges();
            UnitOfWork.Dispose();
            UnitOfWork = null;
        }
    }
}