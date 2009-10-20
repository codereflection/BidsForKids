using System;
using System.Collections.Generic;
namespace BidForKids.Models
{
    public interface IProcurementFactory
    {
        List<Procurement> GetProcurements();
        SerializableProcurement ConvertProcurementToSerializableProcurement(Procurement procurement);
        List<SerializableProcurement> GetSerializableProcurements(string sortIndex, string sortOrder, bool search, Dictionary<string, string> searchParams);
        Procurement GetProcurement(int id);
        Procurement GetNewProcurement();
        int AddProcurement(Procurement procurement);
        bool SaveProcurement(Procurement procurement);
        bool DeleteProcurement(int id);

        List<Auction> GetAuctions();
        Auction GetAuction(int id);

        List<Contact> GetContacts();
        Contact GetContact(int id);
        Contact GetNewContact();
        int AddContact(Contact contact);
        bool SaveContact(Contact contact);

        List<GeoLocation> GetGeoLocations();
        GeoLocation GetGeoLocation(int id);
        GeoLocation GetNewGeoLocation();
        int AddGeoLocation(GeoLocation geoLocation);
        bool SaveGeoLocation(GeoLocation geoLocation);

        List<Category> GetCategories();
        Category GetCategory(int id);



    }

}
