using System.Collections.Generic;
using System.Web.Mvc;
using Machine.Specifications;
using BidsForKids.Controllers;

namespace BidsForKids.Tests.Controllers
{
    public class when_getting_a_list_values_from_same_named_ids_from_a_form_collection
    {
        static FormCollection collection;
        static List<string> result;

        Establish context = () =>
            collection = new FormCollection {{"DonorId", "1"}, {"DonorId", "2"}};

        Because of = () =>
            result = collection.GetDonorIdsFromFormCollection("DonorId");

        It should_return_a_list_of_values = () =>
            result.Count.ShouldEqual(2);
    }

    public class when_getting_a_list_of_values_from_numerically_named_ids_from_a_form_collection
    {
        static FormCollection collection;
        static List<string> result;

        Establish context = () =>
            collection = new FormCollection { { "DonorId", "1" }, { "DonorId_1", "2" } };

        Because of = () =>
            result = collection.GetDonorIdsFromFormCollection("DonorId");

        It should_return_a_list_of_all_the_values = () =>
            result.Count.ShouldEqual(2);
    }
}
