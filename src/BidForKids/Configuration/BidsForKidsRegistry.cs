using System.Configuration;
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

            Scan(assemblyScanner =>
                {
                    assemblyScanner.TheCallingAssembly();
                    assemblyScanner.AssemblyContainingType<IProcurementRepository>();
                    assemblyScanner.WithDefaultConventions();
                });           
        }
    }
}
