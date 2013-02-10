using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BidsForKids.Controllers
{
    public static class FormCollectionExtensionMethods
    {
        public static List<string> GetDonorIdsFromFormCollection(this FormCollection collection, string donorSelectFieldId)
        {
            var items = collection.AllKeys.Where(item => item.StartsWith(donorSelectFieldId)).ToList();

            var donorIds = new List<string>();

            items.ForEach(item => donorIds.AddRange(collection[item].Split(',').Where(value => !string.IsNullOrEmpty(value))));

            return donorIds;
        }
    }
}