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


        public List<SerializableProcurement> GetSerializableProcurements(string sortIndex, string sortOrder, bool search, Dictionary<string, string> searchParams)
        {
            if (string.IsNullOrEmpty(sortIndex))
                sortIndex = "Code";

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
                lResult.Add(new SerializableProcurement()
                {
                    Code = lProcurement.Code,
                    Description = lProcurement.Description,
                    Procurement_ID = lProcurement.Procurement_ID,
                    Year = lProcurement.ContactProcurements.Auction.Year
                });
            }

            return lResult;
        }

        private static string BuildSerializableSqlStatement(string sortIndex, string sortOrder, Dictionary<string, string> searchParams)
        {
            string lSql = "select * from Procurement join ContactProcurement CP on Procurement.Procurement_ID = CP.Procurement_ID JOIN Auction ON CP.Auction_ID = Auction.Auction_ID where ";
            int lParamCount = 0;
            foreach (string item in searchParams.Keys.ToList())
            {
                if (lParamCount > 0)
                    lSql += " AND ";
                lSql += item + " LIKE {" + lParamCount.ToString() + "} ";
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

            ContactProcurement lContactProcurement = lProcurementToDelete.ContactProcurements;

            if (lContactProcurement != null)
                dc.ContactProcurements.DeleteOnSubmit(lContactProcurement);

            dc.Procurements.DeleteOnSubmit(lProcurementToDelete);

            dc.SubmitChanges();

            lResult = true;

            return lResult;
        }


        /// <summary>
        /// Saves changes to the Contact object passed
        /// </summary>
        /// <param name="procurement">Contact object with changes to be saved</param>
        /// <returns>True if save was successful, false if it was not.</returns>
        public bool SaveContact(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentException("contact was null", "contact");
            }

            bool lResult = false;
            Contact lOld = GetContact(contact.Contact_ID);

            lOld = contact;

            dc.SubmitChanges();

            // TODO: Compare object properties here
            lResult = true;
            return lResult;
        }


        /// <summary>
        /// Returns a new empty Procurement object
        /// </summary>
        /// <returns></returns>
        public Procurement GetNewProcurement()
        {
            Procurement lResult = new Procurement();
            return lResult;
        }

        /// <summary>
        /// Returns a new empty Contact object
        /// </summary>
        /// <returns></returns>
        public Contact GetNewContact()
        {
            Contact lResult = new Contact();
            return lResult;
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
        /// Adds a new Contact to the Contacts collection
        /// </summary>
        /// <param name="contact">Instance of Contact object to add to collection.</param>
        /// <returns>Newly added Contact_ID</returns>
        public int AddContact(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentException("contact was null", "contact");
            }

            dc.Contacts.InsertOnSubmit(contact);

            dc.SubmitChanges();

            return contact.Contact_ID;
        }


        /// <summary>
        /// Returns an List collection of Auction objects
        /// </summary>
        /// <returns>Returns an List collection of Auction objects</returns>
        public List<Auction> GetAuctions()
        {
            List<Auction> lResult = dc.Auctions.ToList();

            return lResult;
        }


        /// <summary>
        /// Returns an List collection of Contact objects
        /// </summary>
        /// <returns>Returns an List collection of Contact objects</returns>
        public List<Contact> GetContacts()
        {
            List<Contact> lResult = dc.Contacts.ToList();

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
        /// Returns a Contact object by Contact_ID
        /// </summary>
        /// <param name="id">ID of the Contact object to return</param>
        /// <returns>An instance of a Contact object</returns>
        public Contact GetContact(int id)
        {
            var lContact = from C in dc.Contacts where C.Contact_ID == id select C;

            if (lContact == null || lContact.Count() == 0)
            {
                throw new ApplicationException("Unable to locate contact by ID " + id.ToString());
            }

            return lContact.First();
        }


        #region IDisposable Members

        public void Dispose()
        {
            dc.Dispose();
        }

        #endregion
    }
}
