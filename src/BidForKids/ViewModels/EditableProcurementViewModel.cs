using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BidsForKids.Data.Models;

namespace BidsForKids.ViewModels
{
    public class ProcurementDetailsViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, ErrorMessage = "Description cannot be over 2000 characters")]
        public string Description { get; set; }
        public string Donation { get; set; }
        public int Quantity { get; set; }
        public string AuctionNumber { get; set; }
        public string ItemNumber { get; set; }
        public double EstimatedValue { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool ThankYouLetterSent { get; set; }
        public string Certificate { get; set; }
        public string Notes { get; set; }
        public string Year { get; set; }
        public string Category { get; set; }
        public string GeoLocation { get; set; }
        public string DisplayDonor { get; set; }
        public string Title { get; set; }
        public IEnumerable<ProcurementDonorViewModel> Donors { get; set; }


        public static IMappingExpression<Procurement, ProcurementDetailsViewModel> CreateDestinationMap()
        {
            return Mapper.CreateMap<Procurement, ProcurementDetailsViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Procurement_ID))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(p => p.ContactProcurement.Auction.Year))
                .ForMember(dest => dest.Category, 
                           opt => opt.MapFrom(p => 
                                            p.Category != null ?
                                            p.Category.CategoryName :
                                            string.Empty))
                .ForMember(dest => dest.DisplayDonor, opt => opt.MapFrom(p => SelectDisplayDonor(p.ProcurementType.ProcurementTypeDesc, p.ProcurementDonors)))
                .ForMember(dest => dest.GeoLocation, 
                           opt => opt.MapFrom(p =>
                                            p.ContactProcurement.Donor.GeoLocation != null ? 
                                            p.ContactProcurement.Donor.GeoLocation.GeoLocationName :
                                            string.Empty))
                .ForMember(dest => dest.Donors,
                           opt => opt.MapFrom(p =>
                                              Mapper.Map<IEnumerable<Donor>, IEnumerable<ProcurementDonorViewModel>>(p.ProcurementDonors.Select(x => x.Donor))
                                      ));
        }

        private static string SelectDisplayDonor(string procurementTypeDesc, IEnumerable<ProcurementDonor> donors)
        {
            switch (procurementTypeDesc.ToLower())
            {
                case "adventure":
                    return donors.First().Donor.FirstName + " " + donors.First().Donor.LastName;
                case "parent":
                    return donors.First().Donor.FirstName + " " + donors.First().Donor.LastName;
                case "business":
                    return donors.First().Donor.BusinessName;
                default:
                    return string.Empty;
            }
        }

        public static IMappingExpression<EditableProcurementViewModel, Procurement> CreateSourceMap()
        {
            return Mapper.CreateMap<EditableProcurementViewModel, Procurement>();
        }
    }

    public class EditableProcurementViewModel : ProcurementDetailsViewModel
    {
        public string ItemNumberSuffix { get; set; }
        public string ItemNumberPrefix { get; set; }
        public int CategoryId { get; set; }
        public int YearId { get; set; }
        public int ProcurerId { get; set; }
        public int ProcurementTypeId { get; set; }

        public EditableProcurementViewModel()
        {
            Donors = new List<ProcurementDonorViewModel>();
        }

        public static IMappingExpression<Procurement, EditableProcurementViewModel> CreateDestinationMap()
        {
            return Mapper.CreateMap<Procurement, EditableProcurementViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Procurement_ID))
                .ForMember(dest => dest.ItemNumberPrefix, opt => opt.MapFrom(p => string.IsNullOrEmpty(p.ItemNumber) ? string.Empty : p.ItemNumber.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()))
                .ForMember(dest => dest.ItemNumberSuffix, opt => opt.MapFrom(p => string.IsNullOrEmpty(p.ItemNumber) ? string.Empty : p.ItemNumber.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault()))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(p => p.ContactProcurement.Auction.Year))
                .ForMember(dest => dest.Donors,
                           opt => opt.MapFrom(p =>
                                              Mapper.Map<IEnumerable<Donor>, IEnumerable<ProcurementDonorViewModel>>(p.ProcurementDonors.Select(x => x.Donor))
                                      ));
        }

    }

    public class ProcurementDonorViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }

        public static IMappingExpression<Donor, ProcurementDonorViewModel> CreateDestinationMaps()
        {
            return Mapper.CreateMap<Donor, ProcurementDonorViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(d => d.Donor_ID));
        }
    }
}