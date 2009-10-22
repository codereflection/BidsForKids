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

            lProcurementFactory.Stub(x => x.GetDonors()).Return(new List<Donor>());
            lProcurementFactory.Stub(x => x.GetDonor(0)).IgnoreArguments().Return(new Donor());

            lProcurementFactory.Stub(x => x.GetGeoLocations()).Return(new List<GeoLocation>());
            lProcurementFactory.Stub(x => x.GetGeoLocation(0)).IgnoreArguments().Return(new GeoLocation());

            lProcurementFactory.Stub(x => x.GetCategories()).Return(new List<Category>());
            return lProcurementFactory;
        }
    }
}
