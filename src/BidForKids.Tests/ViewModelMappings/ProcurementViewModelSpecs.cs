using System.Linq;
using AutoMapper;
using BidsForKids.Data.Models;
using BidsForKids.ViewModels;
using FizzWare.NBuilder;
using Machine.Specifications;

namespace BidsForKids.Tests.ViewModelMappings
{

    public class with_a_procurement
    {
        protected static Procurement procurement;

        Establish context = () =>
        {
            procurement = Builder<Procurement>
                          .CreateNew()
                          .With(x => x.ItemNumber = "mis - 100")
                          .Build();
            procurement.ContactProcurement = Builder<ContactProcurement>.CreateNew().Build();
            procurement.ContactProcurement.Auction = Builder<Auction>.CreateNew().Build();
            procurement.ProcurementDonors.Add(Builder<ProcurementDonor>.CreateNew().Build());
            procurement.ProcurementDonors.First().Donor = Builder<Donor>.CreateNew().Build();
            ProcurementDonorViewModel.CreateDestinationMaps();
        };

    }

    public class when_mapping_a_procurement_to_a_procurement_details_view_model : with_a_procurement
    {
        private static ProcurementDetailsViewModel result;

        Establish context = () =>
            ProcurementDetailsViewModel.CreateDestinationMap();
			
        Because of = () =>
            result = Mapper.Map<Procurement, ProcurementDetailsViewModel>(procurement);

        It should_have_a_result = () =>
            result.ShouldNotBeNull();

        It should_have_the_correct_id = () =>
            result.Id.ShouldEqual(procurement.Procurement_ID);

        It should_have_the_correct_description = () =>
            result.Description.ShouldEqual(procurement.Description);

        It should_have_the_correct_donation = () =>
            result.Donation.ShouldEqual(procurement.Donation);

        It should_have_the_correct_auction_year = () =>
            result.Year.ShouldEqual(procurement.ContactProcurement.Auction.Year.ToString());
    }

    public class when_mapping_a_procurement_to_an_editable_procurement_view_model : with_a_procurement
    {
        private static EditableProcurementViewModel result;

        Establish context = () =>
                EditableProcurementViewModel.CreateDestinationMap();

        Because of = () =>
            result = Mapper.Map<Procurement, EditableProcurementViewModel>(procurement);

        It should_have_a_result = () =>
            result.ShouldNotBeNull();

        It should_have_the_correct_number_of_donors = () =>
            result.Donors.Count().ShouldEqual(procurement.ProcurementDonors.Count);

        It should_have_the_correct_item_number_prefix = () =>
            result.ItemNumberPrefix.ShouldEqual("mis");

        It should_have_the_correct_item_number_suffix = () =>
            result.ItemNumberSuffix.ShouldEqual("100");
    }
}