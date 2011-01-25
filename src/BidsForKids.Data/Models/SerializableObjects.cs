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

    public class SerializableDonor
    {
        public int Donor_ID { get; set; }
        public string BusinessName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone1Desc { get; set; }
        public string Phone2 { get; set; }
        public string Phone2Desc { get; set; }
        public string Phone3 { get; set; }
        public string Phone3Desc { get; set; }
        public string Email { get; set; }
        public int? GeoLocation_ID { get; set; }
        public string GeoLocationName { get; set; }
        public string Notes { get; set; }
        public string Website { get; set; }
        public int? Donates { get; set; }
        public bool? MailedPacket { get; set; }
        public int? Procurer_ID { get; set; }
        public string ProcurerName { get; set; }
        public int? DonorType_ID { get; set; }
        public string DonorTypeDesc { get; set; }
        public static SerializableDonor ConvertDonorToSerializableProcurement(Donor donor)
        {
            return new SerializableDonor()
            {
                Donor_ID = donor.Donor_ID,
                BusinessName = donor.BusinessName,
                FirstName = donor.FirstName,
                LastName = donor.LastName,
                Address = donor.Address,
                City = donor.City,
                State = donor.State,
                ZipCode = donor.ZipCode,
                Phone1 = donor.Phone1,
                Phone1Desc = donor.Phone1Desc,
                Phone2 = donor.Phone2,
                Phone2Desc = donor.Phone2Desc,
                Phone3 = donor.Phone3,
                Phone3Desc = donor.Phone3Desc,
                Email = donor.Email,
                Website = donor.Website,
                Notes = donor.Notes,
                GeoLocation_ID = donor.GeoLocation_ID,
                GeoLocationName = (donor.GeoLocation == null) ? "" : donor.GeoLocation.GeoLocationName,
                Donates = donor.Donates ?? 2, // TODO: Remove hard coded unknown value of 2 for Donor.Donates when null
                MailedPacket = donor.MailedPacket,
                Procurer_ID = donor.Procurer_ID,
                ProcurerName = donor.Procurer == null ? "" : donor.Procurer.FirstName + " " + donor.Procurer.LastName,
                DonorType_ID = donor.DonorType_ID,
                DonorTypeDesc = donor.DonorType == null ? "" : donor.DonorType.DonorTypeDesc
            };
        }
    }
}
