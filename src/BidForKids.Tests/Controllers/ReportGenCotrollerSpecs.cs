using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BidsForKids.Controllers;
using BidsForKids.Data.Models;
using BidsForKids.ViewModels;
using FizzWare.NBuilder;
using Machine.Specifications;
using NSubstitute;

namespace BidsForKids.Tests.Controllers
{
    public abstract class with_a_report_gen_controller
    {
        protected static IProcurementRepository repo;
        protected static ReportGenController controller;

        Establish context = () =>
                                {
                                    repo = Substitute.For<IProcurementRepository>();
                                    controller = new ReportGenController(repo);
                                };
    }

    public class when_setting_up_the_controller : with_a_report_gen_controller
    {
        It should_have_setup_the_automappings = () =>
            Mapper.GetAllTypeMaps().Where(x => x.DestinationType == typeof(DonorReportViewModel)
                                            && x.SourceType == typeof(Donor)).Count().ShouldEqual(1);
    }

    public class when_creating_a_business_donor_report_for_a_specific_year : with_a_report_gen_controller
    {
        private static DonorReportSetupVideModel reportSetup;
        private static ActionResult result;
        private static IEnumerable<Donor> donors;
        private static int donationYear;
        private static string content;

        Establish context = () =>
                                {
                                    donationYear = 2010;
                                    reportSetup = new DonorReportSetupVideModel
                                                      {
                                                          ReportTitle = "If I only where a goth",
                                                          AuctionYearFilter = donationYear,
                                                          BusinessNameColumn = true,
                                                          BusinessType = true
                                                      };
                                    donors = Builder<Donor>.CreateListOfSize(10).Build();
                                    DonorReportViewModel.CreateDestinationMaps();
                                    repo.GetDonors(donationYear).Returns(donors);
                                };

        Because of = () =>
                         {
                             result = controller.GenerateDonorReport(reportSetup);
                             content = (result as ContentResult).Content;
                         };

        It should_have_a_result = () =>
            result.ShouldNotBeNull();

        It should_be_a_content_type_result = () =>
            result.ShouldBeOfType<ContentResult>();

        It should_call_the_repo_to_get_donors = () =>
            repo.Received().GetDonors(donationYear);

        It should_have_content = () =>
            string.IsNullOrEmpty(content).ShouldBeFalse();

        It should_have_the_donors_in_the_content = () =>
            donors.All(donor => content.Contains(donor.BusinessName)).ShouldBeTrue();

        It should_have_the_business_name_header = () =>
            content.Contains("BusinessName").ShouldBeTrue();

    }

    public class when_creating_a_business_donor_report : with_a_report_gen_controller
    {
        private static DonorReportSetupVideModel reportSetup;
        private static ActionResult result;
        private static IEnumerable<Donor> donors;
        private static int donationYear;
        private static string content;

        Establish context = () =>
        {
            donationYear = 2010;
            reportSetup = new DonorReportSetupVideModel
            {
                ReportTitle = "If I only where a goth",
                AuctionYearFilter = donationYear,
                BusinessNameColumn = true,
                BusinessType = true,
                DonorTypeColumn = true,
                ParentType = false
            };
            donors = Builder<Donor>.CreateListOfSize(10)
                .WhereTheFirst(5).Have(x => x.DonorType = Builder<DonorType>.CreateNew().With(y => y.DonorTypeDesc = "Business").Build())
                .AndTheNext(5).Have(x => x.DonorType = Builder<DonorType>.CreateNew().With(y => y.DonorTypeDesc = "Parent").Build())
                .Build();
            DonorReportViewModel.CreateDestinationMaps();
            repo.GetDonors(donationYear).Returns(donors);
        };

        Because of = () =>
        {
            result = controller.GenerateDonorReport(reportSetup);
            content = (result as ContentResult).Content;
        };

        It should_have_the_donor_type_column = () =>
            content.Contains("<th>DonorType</th>").ShouldBeTrue();

        It should_have_business_donors = () =>
            content.Contains("<td>Business</td>").ShouldBeTrue();

        It should_not_have_parent_donors = () =>
            content.Contains("<td>Parent</td>").ShouldBeFalse();
    }

    public class when_creating_a_parent_donor_report : with_a_report_gen_controller
    {
        private static DonorReportSetupVideModel reportSetup;
        private static ActionResult result;
        private static IEnumerable<Donor> donors;
        private static int donationYear;
        private static string content;

        Establish context = () =>
        {
            donationYear = 2010;
            reportSetup = new DonorReportSetupVideModel
            {
                ReportTitle = "If I only where a goth",
                AuctionYearFilter = donationYear,
                BusinessNameColumn = true,
                BusinessType = false,
                DonorTypeColumn = true,
                ParentType = true
            };
            donors = Builder<Donor>.CreateListOfSize(10)
                .WhereTheFirst(5).Have(x => x.DonorType = Builder<DonorType>.CreateNew().With(y => y.DonorTypeDesc = "Business").Build())
                .AndTheNext(5).Have(x => x.DonorType = Builder<DonorType>.CreateNew().With(y => y.DonorTypeDesc = "Parent").Build())
                .Build();
            DonorReportViewModel.CreateDestinationMaps();
            repo.GetDonors(donationYear).Returns(donors);
        };

        Because of = () =>
        {
            result = controller.GenerateDonorReport(reportSetup);
            content = (result as ContentResult).Content;
        };

        It should_have_the_donor_type_column = () =>
            content.Contains("<th>DonorType</th>").ShouldBeTrue();

        It should_have_parent_donors = () =>
            content.Contains("<td>Parent</td>").ShouldBeTrue();

        It should_not_have_business_donors = () =>
            content.Contains("<td>Business</td>").ShouldBeFalse();
    }

    public class when_creating_a_donor_report_without_specifying_a_donation_year : with_a_report_gen_controller
    {
        private static DonorReportSetupVideModel reportSetup;
        private static ActionResult result;
        private static IEnumerable<Donor> donors;
        private static int donationYear;
        private static string content;

        Establish context = () =>
        {
            donationYear = 0;
            reportSetup = new DonorReportSetupVideModel
            {
                ReportTitle = "If I only where a goth",
                AuctionYearFilter = donationYear,
                BusinessNameColumn = true,
                BusinessType = false,
                DonorTypeColumn = true,
                ParentType = true
            };
            donors = Builder<Donor>.CreateListOfSize(10)
                .WhereTheFirst(5).Have(x => x.DonorType = Builder<DonorType>.CreateNew().With(y => y.DonorTypeDesc = "Business").Build())
                .AndTheNext(5).Have(x => x.DonorType = Builder<DonorType>.CreateNew().With(y => y.DonorTypeDesc = "Parent").Build())
                .Build();
            DonorReportViewModel.CreateDestinationMaps();
            repo.GetDonors(donationYear).Returns(donors);
        };

        Because of = () =>
        {
            result = controller.GenerateDonorReport(reportSetup);
            content = (result as ContentResult).Content;
        };

        It should_not_get_donors_for_a_specific_donoration_year = () =>
            repo.DidNotReceive().GetDonors(donationYear);

        It should_get_all_donors = () =>
            repo.Received().GetDonors();
    }
}