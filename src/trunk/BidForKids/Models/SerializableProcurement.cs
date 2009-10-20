using System;
namespace BidForKids.Models
{
    public class SerializableProcurement
    {
        public string CatalogNumber { get; set; }
        public string AuctionNumber { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public int Procurement_ID { get; set; }
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
        public string PersonName { get; set; }
        public string ProcurerName { get; set; }
        public int Procurer_ID { get; set; }
    }
}
