using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using AutoMapper;
using BidsForKids.Data.Models;

namespace BidsForKids.ViewModels
{
    public class ProcurementViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Donation { get; set; }
        public int Quantity { get; set; }
        public string AuctionNumber { get; set; }
        public string ItemNumber { get; set; }
        public double EstimatedValue { get; set; }
        public int CategoryId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool ThankYouLetterSent { get; set; }
        public int YearId { get; set; }
        public int ProcurerId { get; set; }
        public int ProcurementTypeId { get; set; }
        public string Certificate { get; set; }
        public string Notes { get; set; }

        public IEnumerable<ProcurementDonorViewModel> Donors { get; set; }


        public ProcurementViewModel()
        {
            Donors = new List<ProcurementDonorViewModel>();
        }

        public static IMappingExpression<Procurement, ProcurementViewModel> CreateDestinationMap()
        {
            return Mapper.CreateMap<Procurement, ProcurementViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Procurement_ID))
                .ForMember(dest => dest.Donors, 
                           opt => opt.MapFrom(p =>
                                                Mapper.Map<IEnumerable<Donor>, IEnumerable<ProcurementDonorViewModel>>(p.ProcurementDonors.Select(x => x.Donor))
                                              ));
        }

        public static IMappingExpression<ProcurementViewModel, Procurement> CreateSourceMap()
        {
            return Mapper.CreateMap<ProcurementViewModel, Procurement>();
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