using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Text;
using BidForKids.Models;
using BidForKids.Models.ReportModels;
using System.Reflection;
using BidForKids.Models.SerializableObjects;

namespace BidForKids.Controllers
{
    public class ReportController : Controller
    {
        private IProcurementFactory factory;

        public ReportController(IProcurementFactory factory)
        {
            this.factory = factory;
        }


        //
        // GET: /Report/

        public ActionResult Index()
        {
            return View();
        }


        private static List<SelectListItem> GetReportTypeSelectList()
        {
            List<SelectListItem> reportTypeOptions = new List<SelectListItem>();
            reportTypeOptions.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            reportTypeOptions.Add(new SelectListItem() { Text = "Business", Value = "Business" });
            reportTypeOptions.Add(new SelectListItem() { Text = "Parent", Value = "Parent" });
            return reportTypeOptions;
        }



        void SetupCreateReportViewData(string createByType)
        {
            ViewData["CreateByType"] = createByType;

            ViewData["ReportType"] = GetReportTypeSelectList();
        }


        //
        // GET: /Report/CreateByType/{id}

        public ActionResult CreateByType(string id)
        {
            SetupCreateReportViewData(id);

            return View();
        }


        //
        // POST: /Report/RunReport

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RunReport(FormCollection collection)
        {
            ContentResult result = new ContentResult();

            StringBuilder reportHtml = new StringBuilder();

            reportHtml.AppendLine("<h3>" + collection["ReportTitle"] + "</h3>");


            List<string> columns = new List<string>();
            columns = GetSelectedColumns(collection);

            if (columns == null)
            {
                reportHtml.AppendLine("<br />No columns found");
                result.Content = reportHtml.ToString();
                return result;
            }

            reportHtml.AppendLine("<table class=\"customReport\">");

            bool includeRowNumbers = false;
            if (collection["IncludeRowNumbers"].Contains("true"))
                includeRowNumbers = true;

            BuildReportHeader(reportHtml, columns, includeRowNumbers);

            ProcurementReport report = GetReportData(collection);


            BuildReportBody(reportHtml, columns, report, includeRowNumbers);
            reportHtml.AppendLine("</table>");

            result.Content = reportHtml.ToString();
            return result;
        }

        private ProcurementReport GetReportData(FormCollection collection)
        {
            ProcurementReport report = new ProcurementReport();

            List<SerializableProcurement> procurements = SerializableProcurement.ConvertProcurementListToSerializableProcurementList(factory.GetProcurements());

            Dictionary<string, string> filters = GetFilteredColumns(collection);
            List<string> procurmentTypes = new List<string>();

            if (collection["BusinessType"].Contains("true"))
                procurmentTypes.Add("Business");
            if (collection["ParentType"].Contains("true"))
                procurmentTypes.Add("Parent");

            // This is really fucking inefficient, and I don't like it one bit.

            foreach (SerializableProcurement item in procurements.ToArray())
            {
                if (procurmentTypes.Contains(item.ProcurementType) == false)
                    procurements.Remove(item);
            }

            if (filters != null && filters.Count() > 0)
            {
                foreach (var filter in filters)
                {
                    PropertyInfo propInfo = new SerializableProcurement().GetType().GetProperty(filter.Key);
                    if (propInfo != null || filter.Key == "Donor")
                    {
                        try
                        {
                            foreach (SerializableProcurement item in procurements.ToArray())
                            {
                                // ugly special case
                                if (filter.Key == "Donor")
                                {
                                    if ((item.BusinessName == null || item.PersonName == null) ||
                                        (item.BusinessName.IndexOf(filter.Value, StringComparison.CurrentCultureIgnoreCase) == -1 &&
                                        item.PersonName.IndexOf(filter.Value, StringComparison.CurrentCultureIgnoreCase) == -1))
                                        
                                        procurements.Remove(item);

                                    continue;
                                }

                                object value = propInfo.GetValue(item, null);

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
                            string error = ex.ToString();
                        }
                    }
                }
            }

            report.rows = procurements;

            return report;
        }

        private void BuildReportBody(StringBuilder reportHtml, List<string> columns, ProcurementReport report, bool includeRowNumbers)
        {
            reportHtml.AppendLine("<tbody>");

            int rowCounter = 0;
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
                        if (string.IsNullOrEmpty(row.PersonName.Trim()) == false)
                        {
                            if (string.IsNullOrEmpty(row.BusinessName) == false)
                                reportHtml.Append(": " + row.PersonName);
                            else
                                reportHtml.Append(row.PersonName);
                        }
                        if (string.IsNullOrEmpty(row.BusinessName) == true && string.IsNullOrEmpty(row.PersonName) == true)
                        {
                            reportHtml.AppendLine("&nbsp;");
                        }
                        reportHtml.AppendLine("</td>");
                        continue;
                    }
                    
                    PropertyInfo propInfo = row.GetType().GetProperty(item);
                    if (propInfo != null)
                    {
                        var value = propInfo.GetValue(row, null);

                        reportHtml.AppendLine(FormatHtmlTableCellValueByType(value));
                    }
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

        private static void BuildReportHeader(StringBuilder reportHtml, List<string> columns, bool includeRowNumbers)
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

        private List<string> GetSelectedColumns(FormCollection collection)
        {
            var columnQuery = from i in collection.AllKeys.AsQueryable() where i.EndsWith("Column") == true select i;

            if (columnQuery == null || columnQuery.Count() == 0)
                return null;

            List<string> result = new List<string>();

            foreach (var item in columnQuery)
            {
                if (collection[item].Contains("true"))
                    result.Add(item.Replace("Column", string.Empty));
            }

            return result;
        }

        private Dictionary<string, string> GetFilteredColumns(FormCollection collection)
        {
            var filterQuery = from i in collection.AllKeys.AsQueryable() where i.EndsWith("Filter") == true select i;

            if (filterQuery == null || filterQuery.Count() == 0)
                return null;

            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var item in filterQuery)
            {
                if (string.IsNullOrEmpty(collection[item]) == false)
                {
                    result.Add(item.Replace("Filter", string.Empty), collection[item]);
                }
            }

            return result;
        }
    }
}
