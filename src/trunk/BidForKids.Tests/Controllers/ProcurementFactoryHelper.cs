using System.Web.Mvc;
using Xunit;
using BidForKids.Controllers;
using BidForKids.Models;
using Rhino.Mocks;
using System.Collections.Generic;

namespace BidForKids.Tests.Controllers
{
    public class ProcurementFactoryHelper
    {
        public static IProcurementFactory GenerateMockProcurementFactory()
        {
            IProcurementFactory lProcurementFactory = MockRepository.GenerateStub<IProcurementFactory>();
            lProcurementFactory.Stub(x => x.GetProcurements()).Return(new List<Procurement>());
            lProcurementFactory.Stub(x => x.GetProcurement(0)).IgnoreArguments().Return(new Procurement());
            lProcurementFactory.Stub(x => x.GetAuctions()).Return(new List<Auction>());
            lProcurementFactory.Stub(x => x.GetContacts()).Return(new List<Contact>());
            lProcurementFactory.Stub(x => x.GetContact(0)).IgnoreArguments().Return(new Contact());
            return lProcurementFactory;
        }
    }
}
