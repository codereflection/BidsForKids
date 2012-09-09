using System;
using BidsForKids.Data.Models;
using System.Collections.Generic;
using NSubstitute;

namespace BidsForKids.Tests.Controllers
{
    public class ProcurementFactoryHelper
    {
        public static Func<Donor> GetTestDonor = () => new Donor();

        public static IProcurementRepository GenerateMockProcurementFactory()
        {
            var factory = Substitute.For<IProcurementRepository>();
            factory.GetProcurements().Returns(new List<Procurement>());
            factory.GetProcurement(Arg.Any<int>()).Returns(new Procurement { ProcurementType = new ProcurementType() });

            factory.GetAuctions().Returns(new List<Auction>());

            factory.GetDonors().Returns(new List<Donor>());
            factory.GetDonor(Arg.Any<int>()).Returns(x => GetTestDonor.Invoke());

            factory.GetGeoLocations().Returns(new List<GeoLocation>());
            factory.GetGeoLocation(Arg.Any<int>()).Returns(new GeoLocation());

            factory.GetCategories().Returns(new List<Category>());

            factory.GetProcurers().Returns(new List<Procurer>());
            return factory;
        }
    }
}
