using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap;
using StructureMap.Configuration.DSL;
using BidsForKids.Models;

namespace BidsForKids.Configuration
{
    public class BidsForKidsRegistry : Registry
    {
        public BidsForKidsRegistry()
        {
            //ForRequestedType<IProcurementFactory>().TheDefaultIsConcreteType<ProcurementFactory>();
            Scan(assemblyScanner =>
                {
                    assemblyScanner.TheCallingAssembly();
                    assemblyScanner.WithDefaultConventions();
                });
        }
    }
}
