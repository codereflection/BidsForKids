using StructureMap;

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
