using System.Configuration;
using System.Data.Linq;
using System.Web.Mvc;
using BidsForKids.Controllers;
using BidsForKids.Data.Repositories;
using SampleWebsite.Areas.UserAdministration.Controllers;
using StructureMap;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;
using BidsForKids.Data.Models;

namespace BidsForKids.Configuration
{
    public class BidsForKidsRegistry : Registry
    {
        public BidsForKidsRegistry()
        {
            For<IProcurementRepository>().Use<ProcurementRepository>()
                .WithCtorArg("connectionString").EqualTo(
                    ConfigurationManager.ConnectionStrings["BidsForKidsConnectionString"].ConnectionString);

            SelectConstructor(() => new DataContext("whatchoodoo"));

            ForConcreteType<DataContext>().Configure
                .WithCtorArg("fileOrServerOrConnection")
                .EqualTo(ConfigurationManager.ConnectionStrings["BidsForKidsConnectionString"].ConnectionString);

            For<IUnitOfWork>().HybridHttpOrThreadLocalScoped().Use<DatabaseUnitOfWork>();

            SelectConstructor(() => new UserAdministrationController());
            ForConcreteType<UserAdministrationController>().Configure.Named("User Administration Controller");
            SelectConstructor(() => new AccountController());
            ForConcreteType<AccountController>().Configure.Named("Account Controller");

            Scan(assemblyScanner =>
                {
                    assemblyScanner.TheCallingAssembly();
                    assemblyScanner.AssemblyContainingType<IProcurementRepository>();
                    assemblyScanner.WithDefaultConventions();
                });
        }
    }
}
