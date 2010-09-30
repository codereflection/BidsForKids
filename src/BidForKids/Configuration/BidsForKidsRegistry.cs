using System.Configuration;
using System.Data.Linq;
using BidsForKids.Data.Repositories;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;
using BidsForKids.Data.Models;

namespace BidsForKids.Configuration
{
    public class BidsForKidsRegistry : Registry
    {
        public BidsForKidsRegistry()
        {
            ForRequestedType<IProcurementRepository>().TheDefault.Is.OfConcreteType<ProcurementRepository>()
                .WithCtorArg("connectionString").EqualTo(
                    ConfigurationManager.ConnectionStrings["BidsForKidsConnectionString"].ConnectionString);

            SelectConstructor<DataContext>(() => new DataContext("whatchoodoo"));

            ForConcreteType<DataContext>().Configure
                .WithCtorArg("fileOrServerOrConnection")
                .EqualTo(ConfigurationManager.ConnectionStrings["BidsForKidsConnectionString"].ConnectionString);

            ForRequestedType<IUnitOfWork>()
                .CacheBy(InstanceScope.Hybrid)
                .TheDefault.Is.OfConcreteType<DatabaseUnitOfWork>();

            Scan(assemblyScanner =>
                {
                    assemblyScanner.TheCallingAssembly();
                    assemblyScanner.AssemblyContainingType<IProcurementRepository>();
                    assemblyScanner.WithDefaultConventions();
                });
        }
    }
}
