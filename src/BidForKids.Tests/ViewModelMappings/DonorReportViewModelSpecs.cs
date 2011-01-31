using AutoMapper;
using BidsForKids.Data.Models;
using BidsForKids.ViewModels;
using FizzWare.NBuilder;
using Machine.Specifications;

namespace BidsForKids.Tests.ViewModelMappings
{
	public class when_mapping_a_donor_to_a_donor_report_view_model
	{
		private static Donor donor;
		private static DonorReportViewModel result;

		Establish context = () =>
								{
									donor = Builder<Donor>.CreateNew()
										.With(x => x.DonorType = Builder<DonorType>.CreateNew().Build())
										.With(x => x.GeoLocation = Builder<GeoLocation>.CreateNew().Build())
										.With(x => x.Procurer = Builder<Procurer>.CreateNew().Build())
                                        .With(x => x.Donates = 1)
										.Build();
									DonorReportViewModel.CreateDestinationMaps();
								};

		Because of = () =>
			result = Mapper.Map<Donor, DonorReportViewModel>(donor);

		It should_have_a_result = () =>
			result.ShouldNotBeNull();

		It should_have_the_correct_id = () =>
			result.Id.ShouldEqual(donor.Donor_ID);

		It should_have_the_correct_business_name = () =>
			result.BusinessName.ShouldEqual(donor.BusinessName);

        It should_have_the_geo_location_name = () =>
            result.GeoLocation.ShouldEqual(donor.GeoLocation.GeoLocationName);

        It should_have_the_procurer_name = () =>
            result.Procurer.ShouldEqual(string.Format("{0} {1}", donor.Procurer.FirstName, donor.Procurer.LastName));

        It should_have_the_donor_type = () =>
            result.DonorType.ShouldEndWith(donor.DonorType.DonorTypeDesc);

        It should_have_the_correct_donates_text = () =>
            result.Donates.ShouldEqual("Yes");
	}

    public class when_mapping_a_donor_to_a_donor_report_view_model_and_the_procurer_does_not_exist
    {
        private static Donor donor;
        private static DonorReportViewModel result;

        Establish context = () =>
        {
            donor = Builder<Donor>.CreateNew()
                .With(x => x.DonorType = Builder<DonorType>.CreateNew().Build())
                .With(x => x.GeoLocation = Builder<GeoLocation>.CreateNew().Build())
                .With(x => x.Procurer = null)
                .With(x => x.Donates = 1)
                .Build();
            DonorReportViewModel.CreateDestinationMaps();
        };

        Because of = () =>
            result = Mapper.Map<Donor, DonorReportViewModel>(donor);

        It should_have_an_empty_procurer_name = () =>
            result.Procurer.ShouldBeEmpty();

        It should_have_zero_for_the_procurer_id = () =>
            result.ProcurerId.ShouldEqual(0);
    }

    public class when_mapping_a_donor_to_a_donor_report_view_model_and_the_geo_location_does_not_exist
    {
        private static Donor donor;
        private static DonorReportViewModel result;

        Establish context = () =>
        {
            donor = Builder<Donor>.CreateNew()
                .With(x => x.DonorType = Builder<DonorType>.CreateNew().Build())
                .With(x => x.GeoLocation = null)
                .With(x => x.Procurer = Builder<Procurer>.CreateNew().Build())
                .With(x => x.Donates = 1)
                .Build();
            DonorReportViewModel.CreateDestinationMaps();
        };

        Because of = () =>
            result = Mapper.Map<Donor, DonorReportViewModel>(donor);

        It should_have_an_empty_geo_location = () =>
            result.GeoLocation.ShouldBeEmpty();

        It should_have_an_zero_for_the_geo_location_id = () =>
            result.GeoLocationId.ShouldEqual(0);
    }

    public class when_mapping_a_donor_to_a_donor_report_view_model_and_the_donor_type_does_not_exist
    {
        private static Donor donor;
        private static DonorReportViewModel result;

        Establish context = () =>
        {
            donor = Builder<Donor>.CreateNew()
                .With(x => x.DonorType = null)
                .With(x => x.GeoLocation = null)
                .With(x => x.Procurer = Builder<Procurer>.CreateNew().Build())
                .With(x => x.Donates = 1)
                .Build();
            DonorReportViewModel.CreateDestinationMaps();
        };

        Because of = () =>
            result = Mapper.Map<Donor, DonorReportViewModel>(donor);

        It should_have_an_donor_type = () =>
            result.DonorType.ShouldBeEmpty();
    }
}