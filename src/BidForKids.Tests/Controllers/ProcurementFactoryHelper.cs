using System.Web.Mvc;
using Xunit;
using BidsForKids.Controllers;
using BidsForKids.Data.Models;
using Rhino.Mocks;
using System.Collections.Generic;

namespace BidsForKids.Tests.Controllers
{
    public class ProcurementFactoryHelper
    {
        public static IProcurementRepository GenerateMockProcurementFactory()
        {
            IProcurementRepository factory = MockRepository.GenerateStub<IProcurementRepository>();
            factory.Stub(x => x.GetProcurements()).Return(new List<Procurement>());
            factory.Stub(x => x.GetProcurement(0)).IgnoreArguments().Return(new Procurement());
            
            factory.Stub(x => x.GetAuctions()).Return(new List<Auction>());

            factory.Stub(x => x.GetDonors()).Return(new List<Donor>());
            factory.Stub(x => x.GetDonor(0)).IgnoreArguments().Return(new Donor());

            factory.Stub(x => x.GetGeoLocations()).Return(new List<GeoLocation>());
            factory.Stub(x => x.GetGeoLocation(0)).IgnoreArguments().Return(new GeoLocation());

            factory.Stub(x => x.GetCategories()).Return(new List<Category>());

            factory.Stub(x => x.GetProcurers()).Return(new List<Procurer>());
            return factory;
        }
    }
}
