using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Data.Linq;

namespace BidForKids.Models
{
    public class ProcurementFactory : BidForKids.Models.IProcurementFactory, IDisposable
    {
        ProcurementDataClassesDataContext dc;

        public ProcurementFactory()
        {
            dc = new ProcurementDataClassesDataContext();
        }


        /// <summary>
        /// Returns a list of all Procurements
        /// </summary>
        /// <returns></returns>
        public List<Procurement> GetProcurements()
        {
            var lProcurements = dc.Procurements.ToList();
            return lProcurements;
        }

        public List<Procurement> GetProcurements(int Year)
        {
            var lProcurements = dc.Procurements.Where(x => x.ContactProcurement.Auction.Year == Year).ToList();
            return lProcurements;
        }


        public List<SerializableProcurement> GetSerializableProcurements(string sortIndex, string sortOrder, bool search, Dictionary<string, string> searchParams)
        {
            if (string.IsNullOrEmpty(sortIndex))
                sortIndex = "Description";

            if (string.IsNullOrEmpty(sortOrder))
                sortOrder = "asc";

            IEnumerable<Procurement> lProcurements;

            if (search == false)
            {
                lProcurements = dc.Procurements.OrderBy(sortIndex + " " + sortOrder);
            }
            else
            {
                AddLikePercentsToValues(searchParams);

                string lSql = BuildSerializableSqlStatement(sortIndex, sortOrder, searchParams);

                lProcurements = dc.ExecuteQuery<Procurement>(lSql, searchParams.Values.ToArray<string>());
            }

            List<SerializableProcurement> lResult = new List<SerializableProcurement>();

            foreach (var lProcurement in lProcurements)
            {
                lResult.Add(ConvertProcurementToSerializableProcurement(lProcurement));
            }

            return lResult;
        }

        public SerializableProcurement ConvertProcurementToSerializableProcurement(Procurement procurement)
        {
            return new SerializableProcurement()
            {
                CatalogNumber = procurement.CatalogNumber,
                Description = procurement.Description,
                Procurement_ID = procurement.Procurement_ID,
                Year = procurement.ContactProcurement.Auction.Year,
                AuctionNumber = procurement.AuctionNumber,
                ItemNumber = procurement.ItemNumber,
                Quantity = procurement.Quantity,
                EstimatedValue = procurement.EstimatedValue,
                SoldFor = procurement.SoldFor,
                Category_ID = procurement.Category_ID,
                CategoryName = procurement.Category == null ? "" : procurement.Category.CategoryName,
                GeoLocation_ID = procurement.ContactProcurement.Donor == null ? null : procurement.ContactProcurement.Donor.GeoLocation_ID,
                GeoLocationName = (procurement.ContactProcurement.Donor == null || procurement.ContactProcurement.Donor.GeoLocation == null) ? "" : procurement.ContactProcurement.Donor.GeoLocation.GeoLocationName,
                PerItemValue = procurement.PerItemValue,
                BusinessName = procurement.ContactProcurement.Donor.BusinessName,
                PersonName = procurement.ContactProcurement.Donor == null ? "" : procurement.ContactProcurement.Donor.FirstName + " " + procurement.ContactProcurement.Donor.LastName,
                Procurer_ID = procurement.Procurement_ID,
                ProcurerName = procurement.ContactProcurement.Procurer == null ? "" : procurement.ContactProcurement.Procurer.FirstName + " " + procurement.ContactProcurement.Procurer.LastName
            };
        }

        private static string BuildSerializableSqlStatement(string sortIndex, string sortOrder, Dictionary<string, string> searchParams)
        {
            string lSql = "select Procurement.*, Auction.*, GeoLocation.GeoLocationName, Category.CategoryName from Procurement "
                + " JOIN ContactProcurement CP on Procurement.Procurement_ID = CP.Procurement_ID "
                + " JOIN Auction ON CP.Auction_ID = Auction.Auction_ID "
                + " LEFT JOIN GeoLocation ON Procurement.GeoLocation_ID = GeoLocation.GeoLocation_ID "
                + " LEFT JOIN Category ON Procurement.Category_ID = Category.Category_ID "
                + " LEFT JOIN Procurer ON CP.Procurer_ID = Procurer.Procurer_ID "
                + " LEFT JOIN Donor ON CP.Donor_ID = Donor.Donor_ID "
                + " where ";
            int lParamCount = 0;
            foreach (string item in searchParams.Keys.ToList())
            {
                string lField = item;
                if (lParamCount > 0)
                    lSql += " AND ";

                // TODO: Fix the hack on the table name
                if (lField.ToLower() == "description")
                {
                    lField = "Procurement.Description";
                }

                // TODO: Fix the hack on the table name
                if (lField.ToLower() == "procurername")
                {
                    lField = "Procurer.FirstName + ' ' + Procurer.LastName";
                }

                // TODO: Fix the hack on the table name
                if (lField.ToLower() == "businessname")
                {
                    lField = "Donor.BusinessName";
                }


                lSql += lField + " LIKE {" + lParamCount.ToString() + "} ";
                lParamCount += 1;
            }
            lSql += " order by " + sortIndex + " " + sortOrder;
            return lSql;
        }

        private static void AddLikePercentsToValues(Dictionary<string, string> searchParams)
        {
            foreach (string key in searchParams.Keys.ToList<string>())
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
            var lProcurement = from P in dc.Procurements where P.Procurement_ID == id select P;

            if (lProcurement == null || lProcurement.Count() == 0)
            {
                throw new ApplicationException("Unable to locate procurement by ID " + id.ToString());
            }

            return lProcurement.First();
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

            bool lResult = false;
            Procurement lOld = GetProcurement(procurement.Procurement_ID);

            lOld = procurement;

            dc.SubmitChanges();

            // TODO: Compare object properties here
            lResult = true;
            return lResult;
        }



        /// <summary>
        /// Removes a Procurement from the database
        /// </summary>
        /// <param name="id">ID of the Procurement to remove</param>
        /// <returns>True if successfull.</returns>
        public bool DeleteProcurement(int id)
        {
            bool lResult = false;

            Procurement lProcurementToDelete = GetProcurement(id);

            if (lProcurementToDelete == null)
                return lResult;

            ContactProcurement lContactProcurement = lProcurementToDelete.ContactProcurement;

            if (lContactProcurement != null)
                dc.ContactProcurements.DeleteOnSubmit(lContactProcurement);

            dc.Procurements.DeleteOnSubmit(lProcurementToDelete);

            dc.SubmitChanges();

            lResult = true;

            return lResult;
        }


        /// <summary>
        /// Saves changes to the Donor object passed
        /// </summary>
        /// <param name="procurement">Donor object with changes to be saved</param>
        /// <returns>True if save was successful, false if it was not.</returns>
        public bool SaveDonor(Donor Donor)
        {
            if (Donor == null)
            {
                throw new ArgumentNullException("Donor");
            }

            bool lResult = false;
            Donor lOld = GetDonor(Donor.Donor_ID);

            lOld = Donor;

            dc.SubmitChanges();

            // TODO: Compare object properties here
            lResult = true;
            return lResult;
        }


        /// <summary>
        /// Saves changesto the GeoLocation object passed
        /// </summary>
        /// <param name="geoLocation">GeoLocation object with changes to be saved</param>
        /// <returns>True if save was successful, false if it was not.</returns>
        public bool SaveGeoLocation(GeoLocation geoLocation)
        {
            if (geoLocation == null)
            {
                throw new ArgumentNullException("geoLocation");
            }

            bool lResult = false;
            GeoLocation lOld = GetGeoLocation(geoLocation.GeoLocation_ID);

            lOld = geoLocation;

            dc.SubmitChanges();

            lResult = true;
            return lResult;
        }


        public bool SaveProcurer(Procurer procurer)
        {
            if (procurer == null)
            {
                throw new ArgumentNullException("procurer");
            }

            bool lResult = false;
            Procurer lOld = GetProcurer(procurer.Procurer_ID);

            lOld = procurer;

            dc.SubmitChanges();

            lResult = true;
            return lResult;
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
        /// <param name="geoLocation">Instnace of GeoLocation oject to add to collection</param>
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
            List<Auction> lResult = dc.Auctions.ToList();

            return lResult;
        }


        /// <summary>
        /// Returns a List of Donor objects
        /// </summary>
        /// <returns>A list of Donor objects</returns>
        public List<Donor> GetDonors()
        {
            List<Donor> lResult = dc.Donors.ToList();

            return lResult;
        }


        /// <summary>
        /// Returns a List of GeoLocation objects
        /// </summary>
        /// <returns>A list of GeoLocation objects</returns>
        public List<GeoLocation> GetGeoLocations()
        {
            List<GeoLocation> lResult = dc.GeoLocations.ToList();

            return lResult;
        }


        /// <summary>
        /// Returns a List of Category objects
        /// </summary>
        /// <returns>A list of Category objects</returns>
        public List<Category> GetCategories()
        {
            List<Category> lResult = dc.Categories.ToList();

            return lResult;
        }


        public List<Procurer> GetProcurers()
        {
            List<Procurer> lResult = dc.Procurers.ToList();

            return lResult;
        }

        /// <summary>
        /// Returns an Auction object by Auction_ID
        /// </summary>
        /// <param name="id">ID of the Auction object to return</param>
        /// <returns>An instance of an Auction object</returns>
        public Auction GetAuction(int id)
        {
            var lAuction = from A in dc.Auctions where A.Auction_ID == id select A;

            if (lAuction == null || lAuction.Count() == 0)
            {
                throw new ApplicationException("Unable to locate auction by ID " + id.ToString());
            }

            return lAuction.First();
        }


        /// <summary>
        /// Returns a Donor object by Donor_ID
        /// </summary>
        /// <param name="id">ID of the Donor object to return</param>
        /// <returns>An instance of a Donor object</returns>
        public Donor GetDonor(int id)
        {
            var lContact = from C in dc.Donors where C.Donor_ID == id select C;

            if (lContact == null || lContact.Count() == 0)
            {
                throw new ApplicationException("Unable to locate Donor by ID " + id.ToString());
            }

            return lContact.First();
        }


        
        /// <summary>
        /// Returns a GeoLocation object by GeoLocation_ID
        /// </summary>
        /// <param name="id">ID of the GeoLocation object to return</param>
        /// <returns>An instance of a GeoLocation object</returns>
        public GeoLocation GetGeoLocation(int id)
        {
            var lGeoLocation = from G in dc.GeoLocations where G.GeoLocation_ID == id select G;

            if (lGeoLocation == null || lGeoLocation.Count() == 0)
            {
                throw new ApplicationException("Unable to locate GeoLocation by ID " + id.ToString());
            }

            return lGeoLocation.First();
        }


        /// <summary>
        /// Returns a Category object by Category_ID
        /// </summary>
        /// <param name="id">ID of the Category object to return</param>
        /// <returns>An instance of a Category object</returns>
        public Category GetCategory(int id)
        {
            var lCategory = from C in dc.Categories where C.Category_ID == id select C;

            if (lCategory == null || lCategory.Count() == 0)
            {
                throw new ApplicationException("Unable to locate Category by ID " + id.ToString());
            }

            return lCategory.First();
        }


        public Procurer GetProcurer(int id)
        {
            var lProcurer = from P in dc.Procurers where P.Procurer_ID == id select P;

            if (lProcurer == null || lProcurer.Count() == 0)
            {
                throw new ApplicationException("Unable to locate Procurer by ID " + id.ToString());
            }

            return lProcurer.First();
        }

        #region IDisposable Members

        public void Dispose()
        {
            dc.Dispose();
        }

        #endregion
    }
}
