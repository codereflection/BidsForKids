using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BidsForKids.Controllers;
using BidsForKids.ViewModels;
using Machine.Specifications;
using Simple.Data;

namespace BidsForKids.Tests.Controllers
{
    public class with_a_geo_location_controller : BidsForKidsControllerTestBase
    {
        protected static dynamic db;
        protected static GeoLocationController controller;

        Establish context = () =>
        {
            db = Database.Open();
            db.SetKeyColumn("GeoLocations", "GeoLocation_ID");
            controller = new GeoLocationController();
        };
    }

    public class when_getting_a_list_of_geo_locations_for_the_index : with_a_geo_location_controller
    {
        static ViewResult result;

        Establish context = () =>
            db.GeoLocations.Insert(GeoLocationName: "Seattle", Description: "On Mars", GeoLocation_ID: 1);

        Because of = () =>
            result = (ViewResult)controller.Index();

        It should_have_a_result = () =>
            ((List<GeoLocationViewModel>)result.ViewData.Model).First().GeoLocationName.ShouldEqual("Seattle");
    }

    public class when_creating_a_new_geo_location : with_a_geo_location_controller
    {
        static ActionResult result;
        static GeoLocationViewModel model;

        Establish context = () =>
            model = new GeoLocationViewModel { GeoLocationName = "Seattle", Description = "On Mars" };

        Because of = () =>
            result = controller.Create(model);

        It should_have_saved_the_new_geo_location = () =>
        {
            var location = db.GeoLocations.FindByGeoLocationName("Seattle");
            string name = location.GeoLocationName;
            name.ShouldEqual("Seattle");
            string description = location.Description;
            description.ShouldEqual("On Mars");
        };
    }

    public class when_getting_the_geo_location_to_edit_the_correct_geo_location_should_be_returned : with_a_geo_location_controller
    {
        static ViewResult result;

        Establish context = () =>
        {
            db.GeoLocations.Insert(GeoLocationName: "Seattle", Description: "On Mars", GeoLocation_ID: 1);
            db.GeoLocations.Insert(GeoLocationName: "New York", Description: "On Mercury", GeoLocation_ID: 2);
        };

        Because of = () =>
            result = (ViewResult)controller.Edit(1);

        It should_return_the_correct_model = () =>
           ((GeoLocationViewModel)result.Model).GeoLocation_ID.ShouldEqual(1);
    }

    public class when_updating_a_geo_location_the_update_should_be_saved : with_a_geo_location_controller
    {
        static ActionResult result;
        static GeoLocationViewModel model;

        Establish context = () =>
        {
            db.GeoLocations.Insert(GeoLocationName: "Seattle", Description: "On Mars", GeoLocation_ID: 1);
            model = new GeoLocationViewModel { GeoLocation_ID = 1, GeoLocationName = "New York", Description = "On Mars" };
        };

        Because of = () =>
            result = controller.Edit(model);

        It should_update_the_database_with_the_new_model_data = () =>
        {
            var location = db.GeoLocations.FindByGeoLocation_ID(1);
            string name = location.GeoLocationName;
            name.ShouldEqual("New York");
            string description = location.Description;
            description.ShouldEqual("On Mars");
        };
    }
}
