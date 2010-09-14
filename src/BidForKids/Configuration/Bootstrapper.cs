using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace BidsForKids.Configuration
{
    public class Bootstrapper
    {
        public static void ConfigureStructureMap()
        {
            ObjectFactory.Initialize(x => x.AddRegistry(new BidsForKidsRegistry()));
        }
    }
}
