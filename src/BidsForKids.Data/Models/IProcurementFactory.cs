﻿using System;
using System.Collections.Generic;
using BidsForKids.Data.Models.SerializableObjects;

namespace BidsForKids.Data.Models
{
    public interface IProcurementRepository
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
        IEnumerable<Donor> GetDonors(int donationYear);
        List<SerializableDonor> GetSerializableBusinesses(jqGridLoadOptions loadOptions);
        List<SerializableDonor> GetSerializableParents(jqGridLoadOptions loadOptions);
        List<SerializableDonor> GetSerializableDonors(jqGridLoadOptions loadOptions, int donorTypeId, string defaultSortColumnName);
        DonorType GetDonorTypeByName(string donorTypeDesc);
        DonorType GetDonorTypeByID(int donorTypeId);
        Donor GetDonor(int id);
        Donor GetNewDonor();
        int AddDonor(Donor Donor);
        bool SaveDonor(Donor Donor);
        bool DeleteDonor(Donor Donor);

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

        List<DonatesReference> GetDonatesReferenceList();

        ProcurementType GetProcurementTypeByName(string procurementTypeDesc);

        bool ItemNumberExists(int id, string itemNumber, int auctionId);
        string CheckForLastSimilarItemNumber(int id, string itemNumberPrefix, int auctionId);
    }

}
