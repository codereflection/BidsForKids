using System.Linq;
using AutoMapper;
using BidsForKids.Data.Models;
using BidsForKids.ViewModels;
using FizzWare.NBuilder;
using Machine.Specifications;

namespace BidsForKids.Tests.ViewModelMappings
{

    public class with_a_parent_procurement
    {
        protected static Procurement procurement;

        Establish context = () =>
        {
            procurement = Builder<Procurement>
                          .CreateNew()
                          .With(x => x.ItemNumber = "mis - 100")
                          .And(x => x.ProcurementType = new ProcurementType { ProcurementTypeDesc = "Parent"})
                          .Build();
            procurement.ContactProcurement = Builder<ContactProcurement>.CreateNew().Build();
            procurement.ContactProcurement.Auction = Builder<Auction>.CreateNew().Build();
            procurement.ContactProcurement.Donor = Builder<Donor>.CreateNew().Build();
            procurement.ContactProcurement.Donor.GeoLocation = Builder<GeoLocation>.CreateNew().Build();
            procurement.Category = Builder<Category>.CreateNew().Build();
            procurement.ProcurementDonors.Add(Builder<ProcurementDonor>.CreateNew().Build());
            procurement.ProcurementDonors.First().Donor = Builder<Donor>.CreateNew().Build();
            ProcurementDonorViewModel.CreateDestinationMaps();
        };
    }

    public class with_a_business_procurement
    {
        protected static Procurement procurement;

        Establish context = () =>
        {
            procurement = Builder<Procurement>.CreateNew()
                .With(x => x.ProcurementType = new ProcurementType {ProcurementTypeDesc = "Business"})
                .Build();
            procurement.ContactProcurement = Builder<ContactProcurement>.CreateNew().Build();
            procurement.ContactProcurement.Auction = Builder<Auction>.CreateNew().Build();
            procurement.ContactProcurement.Donor = Builder<Donor>.CreateNew().Build();
            procurement.ContactProcurement.Donor.GeoLocation = Builder<GeoLocation>.CreateNew().Build();
            procurement.Category = Builder<Category>.CreateNew().Build();
            procurement.ProcurementDonors.Add(Builder<ProcurementDonor>.CreateNew().Build());
            procurement.ProcurementDonors.First().Donor = Builder<Donor>.CreateNew().Build();
            ProcurementDonorViewModel.CreateDestinationMaps();
        };
    }

    public class when_mapping_a_procurement_to_a_procurment_details_view_model : with_a_business_procurement
    {
        private static ProcurementDetailsViewModel result;

        Establish context = () =>
            ProcurementDetailsViewModel.CreateDestinationMap();

        Because of = () =>
            result = Mapper.Map<Procurement, ProcurementDetailsViewModel>(procurement);

        It should_have_the_business_as_the_display_donor = () =>
            result.DisplayDonor.ShouldEqual(procurement.ProcurementDonors.First().Donor.BusinessName);
    }

    public class when_mapping_a_procurement_to_a_procurement_details_view_model : with_a_parent_procurement
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

        It should_have_the_category_name = () =>
            result.Category.ShouldEqual(procurement.Category.CategoryName);

        It should_have_the_geo_location_name = () =>
            result.GeoLocation.ShouldEqual(procurement.ContactProcurement.Donor.GeoLocation.GeoLocationName);

        It should_have_the_first_and_last_names_as_the_display_donor = () =>
            result.DisplayDonor.ShouldEqual(procurement.ProcurementDonors.First().Donor.FirstName + " " +
                                            procurement.ProcurementDonors.First().Donor.LastName);
    }

    public class when_mapping_a_procurement_to_an_editable_procurement_view_model : with_a_parent_procurement
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