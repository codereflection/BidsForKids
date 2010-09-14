using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap;
using StructureMap.Configuration.DSL;
using BidForKids.Models;

namespace BidForKids.Configuration
{
    public class BidForKidsRegistry : Registry
    {
        public BidForKidsRegistry()
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
