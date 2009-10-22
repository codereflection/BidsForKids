using System;
using System.Collections.Generic;

namespace BidForKids.Models
{
    public class jqGridLoadOptions
    {
        public string sortIndex { get; set; }
        public string sortOrder { get; set; }
        public bool search { get; set; }
        public Dictionary<string, string> searchParams { get; set; }
        public int page { get; set; }
        public int rows { get; set; }
        public static jqGridLoadOptions GetLoadOptions(System.Collections.Specialized.NameValueCollection QueryString)
        {

            System.Collections.Specialized.NameValueCollection lParams = QueryString;

            jqGridLoadOptions loadOptions = new jqGridLoadOptions
            {
                search = string.IsNullOrEmpty(lParams["_search"]) ? false : bool.Parse(lParams["_search"]),
                sortIndex = string.IsNullOrEmpty(lParams["sidx"]) ? null : lParams["sidx"],
                sortOrder = string.IsNullOrEmpty(lParams["sord"]) ? null : lParams["sord"],
                page = string.IsNullOrEmpty(lParams["page"]) ? 0 : int.Parse(lParams["page"]),
                rows = string.IsNullOrEmpty(lParams["rows"]) ? 0 : int.Parse(lParams["rows"])
            };



            loadOptions.searchParams = new Dictionary<string, string>();

            if (loadOptions.search == true)
            {
                foreach (var param in lParams.AllKeys)
                {
                    if (param != "_search" && param != "nd" && param != "page" && param != "rows" && param != "sidx" && param != "sord")
                    {
                        loadOptions.searchParams.Add(param, lParams[param]);
                    }
                }
            }
            return loadOptions;
        }
    }
}
