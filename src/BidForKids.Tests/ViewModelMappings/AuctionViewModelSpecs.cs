using System.Collections.Generic;
using AutoMapper;
using BidsForKids.Data.Models;
using BidsForKids.ViewModels;
using FizzWare.NBuilder;
using Machine.Specifications;
using Machine.Specifications.Utility;
using System.Linq;

namespace BidsForKids.Tests.ViewModelMappings
{
	public class when_mapping_a_list_of_auctions_to_a_list_of_auction_view_models
	{
		private static IEnumerable<Auction> auctions;
		private static IEnumerable<AuctionViewModel> result;

		Establish context = () =>
								{
									auctions = new[]
												   {
													   new Auction { Auction_ID = 1, Year = 2009, Name = "Test1" },
													   new Auction { Auction_ID = 2, Year = 2010, Name = "Test2" }
												   };
									AuctionViewModel.CreateDestinationMap();
								};

		Because of = () =>
			result = Mapper.Map<IEnumerable<Auction>, IEnumerable<AuctionViewModel>>(auctions);

		It should_have_a_result = () =>
			result.Count().ShouldEqual(2);

		It should_have_the_correct_auction_items = () =>
			auctions.Each(item => result.Count(x => x.Id == item.Auction_ID &&
													x.Name == item.Name &&
													x.Year == item.Year)
										.ShouldEqual(1));

	}

	public class when_mapping_a_list_of_auction_view_models_to_a_list_of_auctions
	{
		private static IEnumerable<Auction> result;
		private static IEnumerable<AuctionViewModel> auctionViewModels;

		Establish context = () =>
		{
			auctionViewModels = new[]
								{
									new AuctionViewModel { Id = 1, Year = 2009, Name = "Test1" },
									new AuctionViewModel { Id = 2, Year = 2010, Name = "Test2" }
								};
			AuctionViewModel.CreateSourceMap();
		};

		Because of = () =>
			result = Mapper.Map<IEnumerable<AuctionViewModel>, IEnumerable<Auction>>(auctionViewModels);

		It should_have_a_result = () =>
			result.Count().ShouldEqual(2);

		It should_have_the_correct_auction_items = () =>
			auctionViewModels.Each(item => result.Count(x => x.Auction_ID == item.Id &&
														x.Name == item.Name &&
														x.Year == item.Year)
										.ShouldEqual(1));
	}

	public class when_mapping_a_procurement_to_a_procurement_view_model
	{
		private static Procurement procurement;
		private static ProcurementViewModel result;

		Establish context = () =>
								{
									procurement = Builder<Procurement>
												  .CreateNew()
												  .With(x => x.ItemNumber = "mis - 100")
												  .Build();
									procurement.ProcurementDonors.Add(Builder<ProcurementDonor>.CreateNew().Build());
									procurement.ProcurementDonors.First().Donor = Builder<Donor>.CreateNew().Build();
									ProcurementDonorViewModel.CreateDestinationMaps();
									ProcurementViewModel.CreateDestinationMap();
								};

		Because of = () =>
			result = Mapper.Map<Procurement, ProcurementViewModel>(procurement);

		It should_have_a_result = () =>
			result.ShouldNotBeNull();

		It should_have_the_correct_id = () =>
			result.Id.ShouldEqual(procurement.Procurement_ID);

		It should_have_the_correct_description = () =>
			result.Description.ShouldEqual(procurement.Description);

		It should_have_the_correct_donation = () =>
			result.Donation.ShouldEqual(procurement.Donation);

		It should_have_the_correct_number_of_donors = () =>
			result.Donors.Count().ShouldEqual(procurement.ProcurementDonors.Count);

        It should_have_the_correct_item_number_prefix = () =>
            result.ItemNumberPrefix.ShouldEqual("mis");

        It should_have_the_correct_item_number_suffix = () =>
            result.ItemNumberSuffix.ShouldEqual("100");
	}

	public class when_mapping_a_donor_to_a_procurement_donor_view_model
	{
		private static Donor donor;
		private static ProcurementDonorViewModel result;

		Establish context = () =>
								{
									donor = Builder<Donor>.CreateNew().Build();
									ProcurementDonorViewModel.CreateDestinationMaps();
								};

		Because of = () =>
			result = Mapper.Map<Donor, ProcurementDonorViewModel>(donor);

		It should_have_a_result = () =>
			result.ShouldNotBeNull();

		It should_have_the_correct_id = () =>
			result.Id.ShouldEqual(donor.Donor_ID);

		It should_have_the_first_name = () =>
			result.FirstName.ShouldEqual(donor.FirstName);

		It should_have_the_last_name = () =>
			result.LastName.ShouldEqual(donor.LastName);

		It should_have_the_busines_name = () =>
			result.BusinessName.ShouldEqual(donor.BusinessName);
	}
}