using System;
using System.Collections.Generic;
using BidForKids.Models.SerializableObjects;

namespace BidForKids.Models
{
    public interface IProcurementFactory
    {
        List<Procurement> GetProcurements();
        List<Procurement> GetProcurements(int Year);
        List<SerializableProcurement> GetSerializableProcurements(jqGridLoadOptions loadOptions);
        Procurement GetProcurement(int id);
        Procurement GetNewProcurement();
        int AddProcurement(Procurement procurement);
        bool SaveProcurement(Procurement procurement);
        bool DeleteProcurement(int id);

        List<Auction> GetAuctions();
        Auction GetAuction(int id);

        List<Donor> GetDonors();
        List<SerializableDonor> GetSerializableDonors(jqGridLoadOptions loadOptions);
        Donor GetDonor(int id);
        Donor GetNewDonor();
        int AddDonor(Donor Donor);
        bool SaveDonor(Donor Donor);

        List<GeoLocation> GetGeoLocations();
        GeoLocation GetGeoLocation(int id);
        GeoLocation GetNewGeoLocation();
        int AddGeoLocation(GeoLocation geoLocation);
        bool SaveGeoLocation(GeoLocation geoLocation);

        List<Category> GetCategories();
        Category GetCategory(int id);

        List<Procurer> GetProcurers();
        Procurer GetProcurer(int id);
        Procurer GetNewProcurer();
        int AddProcurer(Procurer procurer);
        bool SaveProcurer(Procurer procurer);
    }

}
