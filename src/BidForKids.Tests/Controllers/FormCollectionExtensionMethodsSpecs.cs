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

    public class when_getting_a_list_of_values_from_numerically_named_ids_from_a_form_collection_where_the_first_does_not_have_a_number
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

    public class when_getting_a_list_of_values_from_numerically_named_ids_from_a_form_collection_where_all_are_numbered
    {
        static FormCollection collection;
        static List<string> result;

        Establish context = () =>
            collection = new FormCollection { { "DonorId_1", "1" }, { "DonorId_2", "2" } };

        Because of = () =>
            result = collection.GetDonorIdsFromFormCollection("DonorId");

        It should_return_a_list_of_all_the_values = () =>
            result.Count.ShouldEqual(2);
    }

    public class when_getting_a_list_of_values_where_the_id_is_not_in_the_collection
    {
        static FormCollection collection;
        static List<string> result;

        Establish context = () =>
            collection = new FormCollection { { "Key", "Value" } };

        Because of = () => 
            result = collection.GetDonorIdsFromFormCollection("DonorId");

        It should_not_return_null = () =>
            result.ShouldNotBeNull();

        It should_return_an_empty_collection = () =>
            result.Count.ShouldEqual(0);
    }
}
