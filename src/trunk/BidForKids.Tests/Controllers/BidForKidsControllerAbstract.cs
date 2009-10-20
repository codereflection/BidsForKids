using BidForKids.Models;

namespace BidForKids.Tests.Controllers
{
    public class BidForKidsController
    {
        static public IProcurementFactory _ProcurementFactory;
        public BidForKidsController()
        {
            _ProcurementFactory = ProcurementFactoryHelper.GenerateMockProcurementFactory();
        }
    }
}
