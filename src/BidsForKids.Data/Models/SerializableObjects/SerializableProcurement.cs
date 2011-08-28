using System;
using System.Collections.Generic;
using System.Linq;

namespace BidsForKids.Data.Models.SerializableObjects
{
    public class SerializableProcurement
    {
        public int Procurement_ID { get; set; }
        public string CatalogNumber { get; set; }
        public string AuctionNumber { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public decimal? PerItemValue { get; set; }
        public decimal? EstimatedValue { get; set; }
        public decimal? SoldFor { get; set; }
        public int? GeoLocation_ID { get; set; }
        public string GeoLocationName { get; set; }
        public int? Category_ID { get; set; }
        public string CategoryName { get; set; }
        public double? Quantity { get; set; }
        public string BusinessName { get; set; }
        public string Donors { get; set; }
        public string ProcurerName { get; set; }
        public int Procurer_ID { get; set; }
        public string Notes { get; set; }
        public string Donation { get; set; }
        public bool? ThankYouLetterSent { get; set; }
        public string Certificate { get; set; }
        public string Limitations { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ProcurementType_ID { get; set; }
        public string ProcurementType { get; set; }
        public string Title { get; set; }
        public static SerializableProcurement ConvertProcurementToSerializableProcurement(Procurement procurement)
        {
            return new SerializableProcurement()
                       {
                           CatalogNumber      = procurement.CatalogNumber,
                           Description        = procurement.Description,
                           Procurement_ID     = procurement.Procurement_ID,
                           Year               = procurement.ContactProcurement.Auction.Year,
                           AuctionNumber      = procurement.AuctionNumber,
                           ItemNumber         = procurement.ItemNumber,
                           Quantity           = procurement.Quantity,
                           EstimatedValue     = procurement.EstimatedValue,
                           SoldFor            = procurement.SoldFor,
                           Category_ID        = procurement.Category_ID,
                           CategoryName       = procurement.Category == null ? "" : procurement.Category.CategoryName,
                           GeoLocation_ID     = procurement.ContactProcurement.Donor == null ? null : procurement.ContactProcurement.Donor.GeoLocation_ID,
                           GeoLocationName    = (procurement.ContactProcurement.Donor == null || procurement.ContactProcurement.Donor.GeoLocation == null) ? "" : procurement.ContactProcurement.Donor.GeoLocation.GeoLocationName,
                           PerItemValue       = procurement.PerItemValue,
                           BusinessName       = GetBusinessName(procurement),
                           Donors             = GetDonors(procurement),
                           Procurer_ID        = procurement.Procurement_ID,
                           ProcurerName       = procurement.ContactProcurement.Procurer == null ? "" : procurement.ContactProcurement.Procurer.FirstName + " " + procurement.ContactProcurement.Procurer.LastName,
                           Notes              = procurement.Notes,
                           Donation           = procurement.Donation,
                           ThankYouLetterSent = procurement.ThankYouLetterSent,
                           Certificate        = procurement.Certificate,
                           Limitations        = procurement.Limitations,
                           CreatedOn          = procurement.CreatedOn,
                           ProcurementType_ID = procurement.ProcurementType_ID,
                           ProcurementType    = procurement.ProcurementType == null ? "" : procurement.ProcurementType.ProcurementTypeDesc,
                           Title              = procurement.Title
                       };
        }


        private static string GetBusinessName(Procurement procurement)
        {
            return procurement.ProcurementDonors.FirstOrDefault().Donor.BusinessName;
        }

        private static string GetDonors(Procurement procurement)
        {
            var donor = procurement.ProcurementDonors.FirstOrDefault().Donor;

            var result = donor == null ? "" : donor.FirstName + " " + donor.LastName;

            if (procurement.ProcurementDonors.Count > 1)
                result += " ++";

            return result;
        }

        public static List<SerializableProcurement> ConvertProcurementListToSerializableProcurementList(List<Procurement> procurementList)
        {
            var result = new List<SerializableProcurement>();

            procurementList.ForEach(row => result.Add(ConvertProcurementToSerializableProcurement(row)));

            return result;
        }
    }
}