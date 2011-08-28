using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using BidsForKids.Data.Models.SerializableObjects;

namespace BidsForKids.Data.Models
{
    public class ProcurementRepository : IProcurementRepository, IDisposable
    {
        readonly ProcurementDataClassesDataContext dc;

        public ProcurementRepository()
        {
            dc = new ProcurementDataClassesDataContext();
        }

        public ProcurementRepository(string connectionString)
        {
            dc = new ProcurementDataClassesDataContext(connectionString);
        }


        public List<Procurement> GetProcurements()
        {
            return dc.Procurements.ToList();
        }

        public List<Procurement> GetProcurements(int year)
        {
            var procurements = dc.Procurements.Where(x => x.ContactProcurement.Auction.Year == year).ToList();
            return procurements;
        }




        public List<SerializableProcurement> GetSerializableProcurements(jqGridLoadOptions loadOptions)
        {
            if (loadOptions == null)
                throw new ArgumentNullException("loadOptions", "loadOptions is null.");

            var sortIndex = loadOptions.sortIndex;
            var sortOrder = loadOptions.sortOrder;

            if (string.IsNullOrEmpty(sortIndex))
            {
                sortIndex = "CreatedOn";
                sortOrder = "desc";
            }
            else
            {
                if (string.IsNullOrEmpty(sortOrder))
                    sortOrder = "asc";
            }

            IEnumerable<Procurement> procurements;

            if (loadOptions.search == false)
            {
                procurements = dc.Procurements.OrderBy(sortIndex + " " + sortOrder);
            }
            else
            {
                AddLikePercentsToValues(loadOptions.searchParams);

                var sql = BuildSerializableProcurementSqlStatement(sortIndex, sortOrder, loadOptions.searchParams);

                procurements = dc.ExecuteQuery<Procurement>(sql, loadOptions.searchParams.Values.ToArray());
            }

            return procurements.Select(SerializableProcurement.ConvertProcurementToSerializableProcurement)
                .ToList();
        }



        public List<SerializableDonor> GetSerializableBusinesses(jqGridLoadOptions loadOptions)
        {
            var donorType = GetDonorTypeByName("Business");

            return GetSerializableDonors(loadOptions, donorType.DonorType_ID, "BusinessName");
        }

        public List<SerializableDonor> GetSerializableParents(jqGridLoadOptions loadOptions)
        {
            var donorType = GetDonorTypeByName("Parent");

            return GetSerializableDonors(loadOptions, donorType.DonorType_ID, "LastName");
        }


        public DonorType GetDonorTypeByName(string donorTypeDesc)
        {
            var donorType = (from d in dc.DonorTypes where d.DonorTypeDesc == donorTypeDesc select d).FirstOrDefault();

            if (donorType == null)
            {
                throw new ApplicationException("Unable to find donor type " + donorTypeDesc + " in DonorTypes");
            }
            return donorType;
        }

        public DonorType GetDonorTypeByID(int donorTypeId)
        {
            var donorType = (from d in dc.DonorTypes where d.DonorType_ID == donorTypeId select d).FirstOrDefault();

            if (donorType == null)
            {
                throw new ApplicationException("Unable to find DonorType_ID " + donorTypeId + " in DonorTypes");
            }
            return donorType;
        }


        public ProcurementType GetProcurementTypeByName(string procurementTypeDesc)
        {
            var procurementType = (from d in dc.ProcurementTypes where d.ProcurementTypeDesc == procurementTypeDesc select d).FirstOrDefault();

            if (procurementType == null)
            {
                throw new ApplicationException("Unable to find procurement type " + procurementTypeDesc + " in ProcurementTypes");
            }
            return procurementType;
        }


        public List<SerializableDonor> GetSerializableDonors(jqGridLoadOptions loadOptions, int donorTypeId, string defaultSortColumnName)
        {
            if (loadOptions == null)
                throw new ArgumentNullException("loadOptions", "loadOptions is null.");

            if (string.IsNullOrEmpty(loadOptions.sortIndex))
                loadOptions.sortIndex = defaultSortColumnName;

            if (string.IsNullOrEmpty(loadOptions.sortOrder))
                loadOptions.sortOrder = "asc";

            IEnumerable<Donor> donors;

            if (loadOptions.search == false)
            {
                donors = dc.Donors.Where(x => x.DonorType_ID == donorTypeId).OrderBy(loadOptions.sortIndex + " " + loadOptions.sortOrder);
            }
            else
            {
                AddLikePercentsToValues(loadOptions.searchParams);

                var sql = BuildSerializableDonorSqlStatement(loadOptions.sortIndex, loadOptions.sortOrder, loadOptions.searchParams, donorTypeId);

                donors = dc.ExecuteQuery<Donor>(sql, loadOptions.searchParams.Values.ToArray());
            }

            return donors.Select(SerializableDonor.ConvertDonorToSerializableProcurement).ToList();
        }



        private static string BuildSerializableDonorSqlStatement(string sortIndex, string sortOrder, Dictionary<string, string> searchParams, int donorTypeId)
        {
            var sql =
                @"select Donor.*, GeoLocation.GeoLocationName
                FROM Donor
                LEFT JOIN GeoLocation ON Donor.GeoLocation_ID = GeoLocation.GeoLocation_ID
                where Donor.DonorType_ID = " + donorTypeId;

            var paramCount = 0;
            foreach (var item in searchParams.Keys.ToList())
            {
                var field = item;

                sql += " AND ";

                // TODO: Fix the hack on the table name
                if (field.ToLower() == "description")
                {
                    field = "Donor.Description";
                }

                // TODO: Fix the hack on the table name
                if (field.ToLower() == "geolocation_id")
                {
                    field = "GeoLocation.GeoLocationName";
                }


                if (field.IndexOf("_ID") > 0)
                {
                    sql += field + " = {" + paramCount + "} ";
                }
                else if (searchParams[item].ToLower() == "null")
                {
                    sql += field + " IS NULL ";
                }
                else
                {
                    // only perform LIKE on non-_ID fields
                    sql += field + " LIKE {" + paramCount + "} ";
                }

                paramCount += 1;
            }
            sql += " order by " + sortIndex + " " + sortOrder;
            return sql;
        }

        private static string BuildSerializableProcurementSqlStatement(string sortIndex, string sortOrder, Dictionary<string, string> searchParams)
        {
            string sql =  @"select Procurement.*, Auction.*, GeoLocation.GeoLocationName, Category.CategoryName from Procurement
                          JOIN ContactProcurement CP on Procurement.Procurement_ID = CP.Procurement_ID
                          JOIN Auction ON CP.Auction_ID = Auction.Auction_ID
                          LEFT JOIN Donor ON CP.Donor_ID = Donor.Donor_ID
                          LEFT JOIN GeoLocation ON Donor.GeoLocation_ID = GeoLocation.GeoLocation_ID
                          LEFT JOIN Category ON Procurement.Category_ID = Category.Category_ID
                          LEFT JOIN Procurer ON CP.Procurer_ID = Procurer.Procurer_ID
                          where ";
            var paramCount = 0;

            var SpecialKeys = new[] {"Donors"};

            foreach (var item in searchParams.Keys.Where(x => !SpecialKeys.Contains(x)).ToList())
            {
                var field = item;
                if (paramCount > 0)
                    sql += " AND ";

                // TODO: Fix the hack on the table name
                if (field.ToLower() == "description")
                {
                    field = "Procurement.Description";
                }

                // TODO: Fix the hack on the table name
                if (field.ToLower() == "procurername")
                {
                    field = "Procurer.FirstName + ' ' + Procurer.LastName";
                }

                // TODO: Fix the hack on the table name
                if (field.ToLower() == "businessname")
                {
                    field = "Donor.BusinessName";
                }

                // TODO: Fix the hack on the table name
                if (field.ToLower() == "auction_id")
                {
                    field = "CP.Auction_ID";
                }

                // TODO: Fix the hack on the table name
                if (field.ToLower() == "category_id")
                {
                    field = "Category.CategoryName";
                }

                if (field.IndexOf("_ID") > 0)
                {
                    sql += field + " = {" + paramCount + "} ";
                }
                else if (searchParams[item].ToLower() == "null")
                {
                    sql += field + " IS NULL ";
                }
                else
                {
                    // only perform LIKE on non-_ID fields
                    sql += field + " LIKE {" + paramCount + "} ";
                }

                paramCount += 1;
            }

            if (searchParams.Count(x => x.Key.Equals("donors", StringComparison.CurrentCultureIgnoreCase)) > 0)
            {
                var key = searchParams.Where(x => x.Key.Equals("donors", StringComparison.CurrentCultureIgnoreCase)).First();
                if (paramCount > 0)
                    sql += " AND ";
                sql += " (";
                sql += " Donor.BusinessName LIKE '" + key.Value + "' OR ";
                sql += " Donor.FirstName LIKE '" + key.Value + "' OR ";
                sql += " Donor.LastName LIKE '" + key.Value + "'";
                sql += " )";
            }
            
            sql += " order by " + sortIndex + " " + sortOrder;
            return sql;
        }

        private static void AddLikePercentsToValues(Dictionary<string, string> searchParams)
        {
            foreach (var key in searchParams.Keys.ToList()
                                                .Where(key => (key.IndexOf("_ID") <= -1) 
                                                            && (key.ToLower() != "null")))
            {
                searchParams[key] = "%" + searchParams[key] + "%";
            }
        }


        /// <summary>
        /// Returns a Procurement object by Procurement_ID
        /// </summary>
        /// <param name="id">ID of the Procurement object to return</param>
        /// <returns>An instance of a Procurement object</returns>
        public Procurement GetProcurement(int id)
        {
            var procurement = from P in dc.Procurements where P.Procurement_ID == id select P;

            if (procurement == null || procurement.Count() == 0)
            {
                throw new ApplicationException("Unable to locate procurement by ID " + id);
            }

            return procurement.First();
        }


        /// <summary>
        /// Saves changes to the Procurement object passed
        /// </summary>
        /// <param name="procurement">Procurement object with changes to be saved</param>
        /// <returns>True if save was successful, false if it was not.</returns>
        public bool SaveProcurement(Procurement procurement)
        {
            if (procurement == null)
            {
                throw new ArgumentException("procurement was null", "procurement");
            }

            var old = GetProcurement(procurement.Procurement_ID);

            old = procurement;

            dc.SubmitChanges();

            // TODO: Compare object properties here
            return true;
        }



        /// <summary>
        /// Removes a Procurement from the database
        /// </summary>
        /// <param name="id">ID of the Procurement to remove</param>
        /// <returns>True if successful.</returns>
        public bool DeleteProcurement(int id)
        {

            var procurementToDelete = GetProcurement(id);

            if (procurementToDelete == null)
                return false;

            var contactProcurement = procurementToDelete.ContactProcurement;

            if (contactProcurement != null)
                dc.ContactProcurements.DeleteOnSubmit(contactProcurement);

            dc.Procurements.DeleteOnSubmit(procurementToDelete);

            dc.SubmitChanges();

            return true;
        }


        /// <summary>
        /// Saves changes to the Donor object passed
        /// </summary>
        /// <param name="donor"></param>
        /// <returns>True if save was successful, false if it was not.</returns>
        public bool SaveDonor(Donor donor)
        {
            if (donor == null)
            {
                throw new ArgumentNullException("Donor");
            }

            var lOld = GetDonor(donor.Donor_ID);

            lOld = donor;

            dc.SubmitChanges();

            // TODO: Compare object properties here
            return true;
        }


        /// <summary>
        /// Saves changes to the GeoLocation object passed
        /// </summary>
        /// <param name="geoLocation">GeoLocation object with changes to be saved</param>
        /// <returns>True if save was successful, false if it was not.</returns>
        public bool SaveGeoLocation(GeoLocation geoLocation)
        {
            if (geoLocation == null)
            {
                throw new ArgumentNullException("geoLocation");
            }


            var old = GetGeoLocation(geoLocation.GeoLocation_ID);

            old = geoLocation;

            dc.SubmitChanges();

            return true;
        }


        public bool SaveProcurer(Procurer procurer)
        {
            if (procurer == null)
            {
                throw new ArgumentNullException("procurer");
            }

            var old = GetProcurer(procurer.Procurer_ID);

            old = procurer;

            dc.SubmitChanges();

            return true;
        }


        /// <summary>
        /// Returns a new empty Procurement object
        /// </summary>
        /// <returns></returns>
        public Procurement GetNewProcurement()
        {
            return new Procurement();
        }

        /// <summary>
        /// Returns a new empty Donor object
        /// </summary>
        /// <returns></returns>
        public Donor GetNewDonor()
        {
            return new Donor();
        }

        /// <summary>
        /// Returns a new empty GeoLocation object
        /// </summary>
        /// <returns></returns>
        public GeoLocation GetNewGeoLocation()
        {
            return new GeoLocation();
        }


        public Procurer GetNewProcurer()
        {
            return new Procurer();
        }


        /// <summary>
        /// Adds a new Procurement to the Procurements collection
        /// </summary>
        /// <param name="procurement">Instance of Procurement object to add to collection.</param>
        /// <returns>Newly added Procurement_ID</returns>
        public int AddProcurement(Procurement procurement)
        {
            if (procurement == null)
            {
                throw new ArgumentException("procurement was null", "procurement");
            }

            dc.Procurements.InsertOnSubmit(procurement);

            dc.SubmitChanges();

            return procurement.Procurement_ID;
        }


        /// <summary>
        /// Adds a new Donor to the Donors collection
        /// </summary>
        /// <param name="Donor">Instance of Donor object to add to collection.</param>
        /// <returns>Donor_ID of the newly added Donor</returns>
        public int AddDonor(Donor Donor)
        {
            if (Donor == null)
            {
                throw new ArgumentNullException("Donor");
            }

            dc.Donors.InsertOnSubmit(Donor);

            dc.SubmitChanges();

            return Donor.Donor_ID;
        }


        /// <summary>
        /// Adds a new Geo Location to the GeoLocation collection
        /// </summary>
        /// <param name="geoLocation">Instance of GeoLocation object to add to collection</param>
        /// <returns>GeoLocation_ID of the newly added GeoLocation</returns>
        public int AddGeoLocation(GeoLocation geoLocation)
        {
            if (geoLocation == null)
            {
                throw new ArgumentNullException("geoLocation");
            }

            dc.GeoLocations.InsertOnSubmit(geoLocation);

            dc.SubmitChanges();

            return geoLocation.GeoLocation_ID;
        }


        public int AddProcurer(Procurer procurer)
        {
            if (procurer == null)
            {
                throw new ArgumentNullException("procurer");
            }

            dc.Procurers.InsertOnSubmit(procurer);

            dc.SubmitChanges();

            return procurer.Procurer_ID;
        }



        /// <summary>
        /// Returns an List collection of Auction objects
        /// </summary>
        /// <returns>A List collection of Auction objects</returns>
        public List<Auction> GetAuctions()
        {
            return dc.Auctions.ToList();
        }


        /// <summary>
        /// Returns a List of Donor objects
        /// </summary>
        /// <returns>A list of Donor objects</returns>
        public List<Donor> GetDonors()
        {
            return dc.Donors.ToList();
        }


        /// <summary>
        /// Returns a list of Donors for the donation year
        /// </summary>
        /// <param name="donationYear"></param>
        /// <returns></returns>
        public IEnumerable<Donor> GetDonors(int donationYear)
        {
            var donorsByYear = dc.Procurements
                                    .Where(x => x.ContactProcurement.Auction.Year == donationYear)
                                    .Select(x => x.ContactProcurement.Donor_ID)
                                    .Distinct()
                                    .ToList();

            return dc.Donors.Where(x => donorsByYear.Contains(x.Donor_ID));
        }


        /// <summary>
        /// Returns a List of GeoLocation objects
        /// </summary>
        /// <returns>A list of GeoLocation objects</returns>
        public List<GeoLocation> GetGeoLocations()
        {
            return dc.GeoLocations.ToList();
        }


        /// <summary>
        /// Returns a List of Category objects
        /// </summary>
        /// <returns>A list of Category objects</returns>
        public List<Category> GetCategories()
        {
            return dc.Categories.ToList();
        }


        public List<Procurer> GetProcurers()
        {
            return dc.Procurers.ToList();
        }

        public List<DonatesReference> GetDonatesReferenceList()
        {
            return dc.DonatesReferences.ToList();
        }

        /// <summary>
        /// Returns an Auction object by Auction_ID
        /// </summary>
        /// <param name="id">ID of the Auction object to return</param>
        /// <returns>An instance of an Auction object</returns>
        public Auction GetAuction(int id)
        {
            var auctions = from A in dc.Auctions where A.Auction_ID == id select A;

            if (auctions == null || auctions.Count() == 0)
            {
                throw new ApplicationException("Unable to locate auction by ID " + id);
            }

            return auctions.First();
        }


        /// <summary>
        /// Returns a Donor object by Donor_ID
        /// </summary>
        /// <param name="id">ID of the Donor object to return</param>
        /// <returns>An instance of a Donor object</returns>
        public Donor GetDonor(int id)
        {
            var contacts = from C in dc.Donors where C.Donor_ID == id select C;

            if (contacts == null || contacts.Count() == 0)
            {
                throw new ApplicationException("Unable to locate Donor by ID " + id);
            }

            return contacts.First();
        }



        /// <summary>
        /// Returns a GeoLocation object by GeoLocation_ID
        /// </summary>
        /// <param name="id">ID of the GeoLocation object to return</param>
        /// <returns>An instance of a GeoLocation object</returns>
        public GeoLocation GetGeoLocation(int id)
        {
            var geoLocations = from G in dc.GeoLocations where G.GeoLocation_ID == id select G;

            if (geoLocations == null || geoLocations.Count() == 0)
            {
                throw new ApplicationException("Unable to locate GeoLocation by ID " + id);
            }

            return geoLocations.First();
        }


        /// <summary>
        /// Returns a Category object by Category_ID
        /// </summary>
        /// <param name="id">ID of the Category object to return</param>
        /// <returns>An instance of a Category object</returns>
        public Category GetCategory(int id)
        {
            var categories = from C in dc.Categories where C.Category_ID == id select C;

            if (categories == null || categories.Count() == 0)
            {
                throw new ApplicationException("Unable to locate Category by ID " + id);
            }

            return categories.First();
        }


        public Procurer GetProcurer(int id)
        {
            var procurers = from P in dc.Procurers where P.Procurer_ID == id select P;

            if (procurers == null || procurers.Count() == 0)
            {
                throw new ApplicationException("Unable to locate Procurer by ID " + id);
            }

            return procurers.First();
        }


        public bool ItemNumberExists(int id, string itemNumber)
        {
            var query = from P in dc.Procurements
                         where P.ItemNumber == itemNumber
                         && P.Procurement_ID != id
                         select P;

            return query.Count() != 0;
        }

        public string CheckForLastSimilarItemNumber(int id, string itemNumberPrefix, int auctionId)
        {
            var query =
                dc.Procurements.Where(P => P.ItemNumber.StartsWith(itemNumberPrefix) 
                                            && P.Procurement_ID != id
                                            && P.ContactProcurement.Auction_ID == auctionId)
                    .OrderByDescending(P => P.ItemNumber).Select(P => P.ItemNumber);

            return query.Count() == 0 ? "" : query.First();
        }

        #region IDisposable Members

        public void Dispose()
        {
            dc.Dispose();
        }

        #endregion
    }
}
