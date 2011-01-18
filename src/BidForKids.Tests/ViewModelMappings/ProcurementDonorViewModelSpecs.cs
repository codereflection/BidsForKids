using AutoMapper;
using BidsForKids.Data.Models;
using BidsForKids.ViewModels;
using FizzWare.NBuilder;
using Machine.Specifications;

namespace BidsForKids.Tests.ViewModelMappings
{
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
