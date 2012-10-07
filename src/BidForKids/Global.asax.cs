using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BidsForKids.Configuration;
using BidsForKids.Data.Repositories;
using StructureMap;

namespace BidsForKids
{
    public class WebApplication : HttpApplication
    {
        const string AdministratorRoleName = "Administrator";
        const string ProcurementsRoleName = "Procurements";
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
            CheckApplicationSetup();
        }

        void CheckApplicationSetup()
        {
            CreateRoles();
            CreateUsers();
        }

        void CreateRoles()
        {
            if (!Roles.RoleExists(AdministratorRoleName))
                Roles.CreateRole(AdministratorRoleName);
            if (!Roles.RoleExists(ProcurementsRoleName))
                Roles.CreateRole(ProcurementsRoleName);
        }

        void CreateUsers()
        {
            if (Membership.FindUsersByName("Admin").Count == 0)
            {
                var admin = Membership.CreateUser("Admin", "TheAdministrator");
                if (!Roles.IsUserInRole(admin.UserName, AdministratorRoleName))
                    Roles.AddUserToRole(admin.UserName, AdministratorRoleName);
                
            }
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