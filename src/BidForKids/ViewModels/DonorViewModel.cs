using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BidsForKids.ViewModels
{
    public class DonorViewModel
    {
        public DonorViewModel()
        {
            ContactProcurementViewModels = new List<ContactProcurementViewModel>();
        }
        public int Donor_ID { get; set; }
        [StringLength(255)]
        public string Address { get; set; }
        [Required]
        [StringLength(255)]
        public string BusinessName { get; set; }
        [StringLength(255)]
        public string City { get; set; }
        [StringLength(255)]
        public string FirstName { get; set; }
        [StringLength(255)]
        public string LastName { get; set; }
        [StringLength(10000)]
        public string Notes { get; set; }
        [StringLength(20)]
        public string Phone1 { get; set; }
        [StringLength(10)]
        public string Phone1Desc { get; set; }
        [StringLength(20)]
        public string Phone2 { get; set; }
        [StringLength(10)]
        public string Phone2Desc { get; set; }
        [StringLength(20)]
        public string Phone3 { get; set; }
        [StringLength(10)]
        public string Phone3Desc { get; set; }
        [Required]
        [StringLength(2)]
        public string State { get; set; }
        [StringLength(10)]
        public string ZipCode { get; set; }
        public int? Donates { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(1024)]
        public string Website { get; set; }
        public bool MailedPacket { get; set; }
        public int GeoLocation_ID { get; set; }
        public int Procurer_ID { get; set; }
        [Editable(false)]
        public int DonorType_ID { get; set; }
        public List<ContactProcurementViewModel> ContactProcurementViewModels { get; set; }
        [Editable(false)]
        public DateTime CreatedOn { get; set; }
        [Editable(false)]
        public DateTime ModifiedOn { get; set; }
    }
}