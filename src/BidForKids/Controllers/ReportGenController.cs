using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using BidsForKids.Data.Models;
using BidsForKids.ViewModels;

namespace BidsForKids.Controllers
{
    [Authorize(Roles = "Administrator, Procurements")]
    public class ReportGenController : BidsForKidsControllerBase
    {
        private readonly IProcurementRepository repo;

        public ReportGenController(IProcurementRepository repo)
        {
            this.repo = repo;
            DonorReportViewModel.CreateDestinationMaps();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Donor()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GenerateDonorReport(DonorReportSetupVideModel reportSetup)
        {

            var donors = reportSetup.AuctionYearFilter == 0 ? 
                Mapper.Map<IEnumerable<Donor>, IEnumerable<DonorReportViewModel>>(repo.GetDonors()).ToList()
                :
                Mapper.Map<IEnumerable<Donor>, IEnumerable<DonorReportViewModel>>(repo.GetDonors(reportSetup.AuctionYearFilter)).ToList();

            donors = ApplyDonorFilters(donors, reportSetup);

            if (reportSetup.BusinessType == false)
                donors.Where(x => x.DonorType == "Business").ToList().ForEach(x => donors.Remove(x));

            if (reportSetup.ParentType == false)
                donors.Where(x => x.DonorType == "Parent").ToList().ForEach(x => donors.Remove(x));

            var reportHtml = new StringBuilder();

            reportHtml.AppendLine("<h3>" + reportSetup.ReportTitle + "</h3>");
            reportHtml.AppendLine("<table class=\"customReport\">");
            reportHtml.AppendLine("<tbody>");

            var selectedColumns = GetSelectedColumns(reportSetup);

            BuildHeaders(selectedColumns, reportHtml, reportSetup.IncludeRowNumbers);

            BuildReportBody(selectedColumns, donors, reportHtml, reportSetup.IncludeRowNumbers);

            reportHtml.AppendLine("</tbody>");
            reportHtml.AppendLine("</table>");

            var result = new ContentResult() { Content = reportHtml.ToString() };

            return result;
        }

        private static List<DonorReportViewModel> ApplyDonorFilters(List<DonorReportViewModel> donors, DonorReportSetupVideModel reportSetup)
        {
            if (!string.IsNullOrEmpty(reportSetup.GeoLocationFilter))
                donors.Where(x => x.GeoLocation == null || !x.GeoLocation.ToLower().Contains(reportSetup.GeoLocationFilter.ToLower()))
                      .ToList()
                      .ForEach(x => donors.Remove(x));

            if (!string.IsNullOrEmpty(reportSetup.ProcurerFilter))
                donors.Where(x => x.Procurer == null || !x.Procurer.ToLower().Contains(reportSetup.ProcurerFilter.ToLower()))
                      .ToList()
                      .ForEach(x => donors.Remove(x));

            if (!string.IsNullOrEmpty(reportSetup.DonatesFilter))
                donors.Where(x => x.Donates == null || !x.Donates.ToLower().Contains(reportSetup.DonatesFilter.ToLower()))
                      .ToList()
                      .ForEach(x => donors.Remove(x));


            return donors;
        }


        private static void BuildReportBody(IEnumerable<KeyValuePair<string, PropertyInfo>> selectedColumns, IEnumerable<DonorReportViewModel> donors, StringBuilder reportHtml, bool includeRowNumbers)
        {
            var donorList = donors.ToList();
            for (var index = 0; index < donorList.Count; index++)
            {
                var donor = donorList[index];

                reportHtml.AppendFormat("<tr>");

                if (includeRowNumbers)
                    reportHtml.AppendLine(string.Format("<td>{0}</td>", index));

                foreach (var col in selectedColumns)
                {
                    reportHtml.AppendFormat("<td>{0}</td>", donor.GetColumnValue(col.Key, col.Value));
                }
                reportHtml.AppendFormat("</tr>\n");
            }
        }

        private static void BuildHeaders(IEnumerable<KeyValuePair<string, PropertyInfo>> selectedColumns, StringBuilder reportHtml, bool includeRowNumbers)
        {
            reportHtml.AppendLine("<thead><tr>");
            if (includeRowNumbers == true)
                reportHtml.AppendLine("<th></th>");
            foreach (var col in selectedColumns)
            {
                reportHtml.AppendFormat("<th>{0}</th>\n", col.Key.Replace("Column", string.Empty));
            }
            reportHtml.AppendLine("</tr></thead>");
        }

        private static IEnumerable<KeyValuePair<string, PropertyInfo>> GetSelectedColumns(IReportSetupViewModel reportSetup)
        {
            var result = new Dictionary<string, PropertyInfo>();

            var columnProperties = reportSetup.GetType().GetProperties()
                                                        .Where(x =>
                                                            x.Name.EndsWith("Column") &&
                                                            x.PropertyType == typeof(bool)
                                                            )
                                                        .ToList();

            columnProperties.ForEach(col =>
                                         {
                                             if ((bool)(col.GetValue(reportSetup, null)) == true)
                                                 result.Add(col.Name, col);
                                         });

            return result;
        }
    }

    internal static class ReflectionHelpers
    {
        internal static string GetColumnValue(this object itemToInspect, string name, PropertyInfo propertyInfo)
        {
            var value = itemToInspect.GetType()
                .GetProperties()
                .Where(x => x.Name == name.Replace("Column", string.Empty))
                .FirstOrDefault()
                .GetValue(itemToInspect, null);

            return value == null ? string.Empty : value.ToString();
        }
    }
}