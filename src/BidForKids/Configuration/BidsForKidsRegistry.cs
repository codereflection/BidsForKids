using System.Configuration;
using System.Data.Linq;
using BidsForKids.Controllers;
using BidsForKids.Data.Repositories;
using SampleWebsite.Areas.UserAdministration.Controllers;
using StructureMap.Configuration.DSL;
using BidsForKids.Data.Models;

namespace BidsForKids.Configuration
{
    public class BidsForKidsRegistry : Registry
    {
        public BidsForKidsRegistry()
        {
            For<IProcurementRepository>().Use<ProcurementRepository>()
                .Ctor<string>("connectionString")
                .Is(ConfigurationManager.ConnectionStrings["BidsForKidsConnectionString"].ConnectionString);

            SelectConstructor(() => new DataContext("Use this flippin' constructorz!"));
            ForConcreteType<DataContext>().Configure
                .Ctor<string>("fileOrServerOrConnection")
                .Is(ConfigurationManager.ConnectionStrings["BidsForKidsConnectionString"].ConnectionString);

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
