using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using BidsForKids.Data.Models;
using BidsForKids.Data.Models.ReportModels;
using BidsForKids.Data.Models.SerializableObjects;

namespace BidsForKids.Controllers
{
    [Authorize(Roles = "Administrator, Procurements")]
    public class ReportController : Controller
    {
        private readonly IProcurementRepository factory;

        public ReportController(IProcurementRepository factory)
        {
            this.factory = factory;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateByType(string id)
        {
            SetupCreateReportViewData(id);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RunCreateByTypeReport(FormCollection collection)
        {
            try
            {

                var result = new ContentResult();

                var reportHtml = new StringBuilder();

                reportHtml.AppendLine("<h3>" + collection["ReportTitle"] + "</h3>");


                var columns = GetSelectedColumns(collection);

                if (columns == null)
                {
                    reportHtml.AppendLine("<br />No columns found");
                    result.Content = reportHtml.ToString();
                    return result;
                }

                reportHtml.AppendLine("<table class=\"customReport\">");

                var includeRowNumbers = false;
                if (collection["IncludeRowNumbers"].Contains("true"))
                    includeRowNumbers = true;

                BuildReportHeader(reportHtml, columns, includeRowNumbers);

                var report = GetReportData(collection);


                BuildReportBody(reportHtml, columns, report, includeRowNumbers);
                reportHtml.AppendLine("</table>");

                result.Content = reportHtml.ToString();
                return result;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return null;
        }

        public ActionResult AuctionItem()
        {
            SetupAuctionItemViewData();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RunAuctionReport(FormCollection collection)
        {
            int? year = null;
            if (!string.IsNullOrEmpty(collection["YearFilter"]))
                year = int.Parse(collection["YearFilter"]);

            var category = collection["CategoryNameFilter"];
            var auctionNumberStart = collection["AuctionNumberStartFilter"];
            var auctionNumberEnd = collection["AuctionNumberEndFilter"];

            var auctionItems = GetAuctionItems(year);

            if (!string.IsNullOrEmpty(category))
            {
                auctionItems = from p in auctionItems
                               where p.Items.Any(x => x.Category != null ? x.Category.Category_ID == int.Parse(category) : false)
                               select p;
            }

            if (!string.IsNullOrEmpty(auctionNumberStart))
            {
                auctionItems = from p in auctionItems
                               where string.CompareOrdinal(p.AuctionNumber, auctionNumberStart) >= 0
                               select p;
            }

            if (!string.IsNullOrEmpty(auctionNumberEnd))
            {
                auctionItems = from p in auctionItems
                               where string.CompareOrdinal(p.AuctionNumber, auctionNumberEnd) <= 0
                               select p;
            }

            return PartialView(collection["CatalogLayout"].Contains("true") 
                ? "AuctionItemReportDataCatalogLayout" 
                : "AuctionItemReportData", auctionItems);
        }

        private void SetupCreateReportViewData(string createByType)
        {
            ViewData["CreateByType"] = createByType;

            ViewData["ReportType"] = GetReportTypeSelectList();
        }

        private ProcurementReport GetReportData(FormCollection collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            var report = new ProcurementReport();

            var procurements = SerializableProcurement.ConvertProcurementListToSerializableProcurementList(factory.GetProcurements());

            var filters = GetFilteredColumns(collection);
            var procurmentTypes = new List<string>();

            if (collection["BusinessType"].Contains("true"))
                procurmentTypes.Add("Business");
            if (collection["ParentType"].Contains("true"))
                procurmentTypes.Add("Parent");
            if (collection["AdventureType"].Contains("true"))
                procurmentTypes.Add("Adventure");

            // OMFG, what the hell was I thinking? This is _really_ fucking inefficient, and I don't like it one bit. >8(

            foreach (var item in procurements.OrderBy((x) => x.ItemNumber).ToArray())
            {
                if (procurmentTypes.Contains(item.ProcurementType) == false)
                    procurements.Remove(item);
            }

            if (filters != null && filters.Count() > 0)
            {
                foreach (var filter in filters)
                {
                    var propInfo = new SerializableProcurement().GetType().GetProperty(filter.Key);
                    if (propInfo != null || filter.Key == "Donor")
                    {
                        try
                        {
                            foreach (SerializableProcurement item in procurements.ToArray())
                            {
                                // TODO: Fix ugly special case
                                if (filter.Key == "Donor")
                                {
                                    if ((item.BusinessName == null || item.Donors == null) ||
                                        (item.BusinessName.IndexOf(filter.Value, StringComparison.CurrentCultureIgnoreCase) == -1 &&
                                        item.Donors.IndexOf(filter.Value, StringComparison.CurrentCultureIgnoreCase) == -1))

                                        procurements.Remove(item);

                                    continue;
                                }

                                var value = propInfo.GetValue(item, null);

                                if (value == null)
                                    procurements.Remove(item);
                                else
                                {
                                    if (value is Int32 || value is Int32?)
                                    {
                                        if ((Int32)value != Int32.Parse(filter.Value))
                                            procurements.Remove(item);
                                    }
                                    else if (value is Int64 || value is Int64?)
                                    {
                                        if ((Int64)value != Int64.Parse(filter.Value))
                                            procurements.Remove(item);
                                    }
                                    else if (value is decimal || value is decimal?)
                                    {
                                        if ((decimal)value != decimal.Parse(filter.Value))
                                            procurements.Remove(item);
                                    }
                                    else if (value is double || value is double?)
                                    {
                                        if ((double)value != double.Parse(filter.Value))
                                            procurements.Remove(item);
                                    }
                                    else
                                    {
                                        if (value.ToString().IndexOf(filter.Value, StringComparison.CurrentCultureIgnoreCase) == -1)
                                            procurements.Remove(item);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                        }
                    }
                }
            }

            report.rows = procurements;

            return report;
        }

        private static List<SelectListItem> GetReportTypeSelectList()
        {
            var reportTypeOptions = new List<SelectListItem>
                                        {
                                            new SelectListItem() {Text = "", Value = "", Selected = true},
                                            new SelectListItem() {Text = "Business", Value = "Business"},
                                            new SelectListItem() {Text = "Parent", Value = "Parent"}
                                        };
            return reportTypeOptions;
        }

        private static void BuildReportBody(StringBuilder reportHtml, IEnumerable<string> columns, ProcurementReport report, bool includeRowNumbers)
        {
            reportHtml.AppendLine("<tbody>");

            var rowCounter = 0;
            report.rows.ForEach(row =>
            {
                rowCounter += 1;
                reportHtml.AppendLine("<tr>");

                if (includeRowNumbers)
                    reportHtml.AppendLine("<td>" + rowCounter.ToString() + "</td>");

                foreach (var item in columns)
                {
                    if (item == "Donor")
                    {
                        reportHtml.AppendLine("<td>");
                        if (string.IsNullOrEmpty(row.BusinessName) == false)
                            reportHtml.Append(row.BusinessName);
                        if (row.Donors != null && string.IsNullOrEmpty(row.Donors.Trim()) == false)
                        {
                            if (string.IsNullOrEmpty(row.BusinessName) == false)
                                reportHtml.Append(": " + row.Donors);
                            else
                                reportHtml.Append(row.Donors);
                        }
                        if (string.IsNullOrEmpty(row.BusinessName) == true && string.IsNullOrEmpty(row.Donors) == true)
                        {
                            reportHtml.AppendLine("&nbsp;");
                        }
                        reportHtml.AppendLine("</td>");
                        continue;
                    }

                    if (item == "EstimatedValue")
                    {
                        reportHtml.AppendLine("<td>");
                        if (string.IsNullOrEmpty(row.EstimatedValue.ToString()) == false)
                        {
                            reportHtml.Append(row.EstimatedValue == -1 ? "priceless" : row.EstimatedValue == null ? "" : row.EstimatedValue.Value.ToString("C"));
                        }
                        reportHtml.AppendLine("</td>");
                        continue;
                    }

                    var propInfo = row.GetType().GetProperty(item);
                    if (propInfo == null) continue;
                    
                    var value = propInfo.GetValue(row, null);

                    reportHtml.AppendLine(FormatHtmlTableCellValueByType(value));
                }
                reportHtml.AppendLine("</tr>");
            });
            reportHtml.AppendLine("</tbody>");
        }

        private static string FormatHtmlTableCellValueByType(object value)
        {
            if (value == null)
                return string.Format("<td>&nbsp;</td>");
            else
            {
                if (value is Int32 || value is Int32?)
                {
                    return string.Format("<td class='reportNumber'>{0:D}</td>", Int32.Parse(value.ToString()));
                }
                else if (value is Int64 || value is Int64?)
                {
                    return string.Format("<td class='reportNumber'>{0:D}</td>", Int64.Parse(value.ToString()));
                }
                else if (value is decimal || value is decimal?)
                {
                    return string.Format("<td class='reportNumber'>{0:N2}</td>", decimal.Parse(value.ToString()));
                }
                else if (value is double || value is double?)
                {
                    return string.Format("<td class='reportNumber'>{0:N2}</td>", double.Parse(value.ToString()));
                }
                else
                {
                    return string.Format("<td>{0}</td>", value.ToString());
                }
            }
        }

        private static void BuildReportHeader(StringBuilder reportHtml, IEnumerable<string> columns, bool includeRowNumbers)
        {
            reportHtml.AppendLine("<thead>");
            reportHtml.AppendLine("<tr>");
            if (includeRowNumbers == true)
                reportHtml.AppendLine("<th></th>");
            foreach (var item in columns)
            {
                reportHtml.AppendLine("<th>" + item + "</th>");
            }
            reportHtml.AppendLine("</tr>");
            reportHtml.AppendLine("</thead>");
        }

        private static List<string> GetSelectedColumns(FormCollection collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            var columnQuery = from i in collection.AllKeys.AsQueryable() where i.EndsWith("Column") == true select i;

            if (columnQuery == null || columnQuery.Count() == 0)
                return null;

            return (from item in columnQuery
                    where collection[item].Contains("true")
                    select item.Replace("Column", string.Empty)).ToList();
        }

        private static Dictionary<string, string> GetFilteredColumns(NameValueCollection collection)
        {
            var filterQuery = from i in collection.AllKeys.AsQueryable() where i.EndsWith("Filter") == true select i;

            if (filterQuery.Count() == 0)
                return null;

            return filterQuery.Where(item => string.IsNullOrEmpty(collection[item]) == false)
                .ToDictionary(item => item.Replace("Filter", string.Empty), item => collection[item]);
        }

        private SelectList GetCategoriesSelectList()
        {
            IEnumerable<Category> lCategories = factory.GetCategories();
            return new SelectList(lCategories.OrderBy(x => x.CategoryName), "Category_ID", "CategoryName");
        }

        private void SetupAuctionItemViewData()
        {
            ViewData["CategoryList"] = GetCategoriesSelectList();
        }

        private IEnumerable<AuctionItem> GetAuctionItems(int? year)
        {
            var procurementItems = year != null 
                                        ? factory.GetProcurements(year.Value) 
                                        : factory.GetProcurements();

            return from p in procurementItems
                   where p.AuctionNumber != null
                   group p by p.AuctionNumber
                       into g
                       select new AuctionItem()
                                  {
                                      AuctionNumber = g.Key,
                                      Items = g.OrderByDescending((x) => x.EstimatedValue)
                                  };
        }
    }
}
