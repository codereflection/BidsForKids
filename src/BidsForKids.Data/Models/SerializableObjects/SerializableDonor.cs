namespace BidsForKids.Data.Models.SerializableObjects
{
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
