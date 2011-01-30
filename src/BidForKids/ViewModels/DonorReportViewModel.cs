using System;
using AutoMapper;
using BidsForKids.Data.Models;

namespace BidsForKids.ViewModels
{
    public class DonorReportViewModel
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone1Desc { get; set; }
        public string Notes { get; set; }
        public string WebSite { get; set; }
        public string Email { get; set; }
        public int GeoLocationId { get; set; }
        public string GeoLocation { get; set; }
        public int DonorTypeId { get; set; }
        public string DonorType { get; set; }
        public int ProcurerId { get; set; }
        public string Procurer { get; set; }
        public string Donates { get; set; }

        public static IMappingExpression<Donor, DonorReportViewModel> CreateDestinationMaps()
        {
            return Mapper.CreateMap<Donor, DonorReportViewModel>()
                                        .ForMember(dest => dest.Id, opt => opt.MapFrom(a => a.Donor_ID))
                                        .ForMember(dest => dest.GeoLocation, opt => opt.MapFrom(a => a.GeoLocation == null ? string.Empty : a.GeoLocation.GeoLocationName))
                                        .ForMember(dest => dest.DonorType, opt => opt.MapFrom(a => a.DonorType.DonorTypeDesc))
                                        .ForMember(dest => dest.Procurer, opt => opt.MapFrom(a => a.Procurer == null ? string.Empty : string.Format("{0} {1}", a.Procurer.FirstName, a.Procurer.LastName)))
                                        .ForMember(dest => dest.Donates, opt => opt.ResolveUsing(new DonatesValueResolver()));
        }

        public static IMappingExpression<DonorReportViewModel, Donor> CreateSourceMaps()
        {
            return Mapper.CreateMap<DonorReportViewModel, Donor>()
                                        .ForMember(dest => dest.Donor_ID, opt => opt.MapFrom(a => a.Id));
        }
    }

    public class DonatesValueResolver : ValueResolver<Donor, string>
    {
        protected override string ResolveCore(Donor source)
        {
            var donor = source;
            var result = string.Empty;

            if (donor.Donates == 1)
                result = "Yes";
            else if (donor.Donates == 2)
                result = "Unknown";
            else if (donor.Donates == 0)
                result = "No";
            return result;
        }
    }


    public interface IReportSetupViewModel
    {
    }

    public class DonorReportSetupVideModel : IReportSetupViewModel
    {
        public string ReportTitle { get; set; }
        public bool IncludeRowNumbers { get; set; }
        public bool BusinessType { get; set; }
        public bool ParentType { get; set; }

        public bool BusinessNameColumn { get; set; }
        public bool FirstNameColumn { get; set; }
        public bool LastNameColumn { get; set; }
        public bool AddressColumn { get; set; }
        public bool CityColumn { get; set; }
        public bool StateColumn { get; set; }
        public bool ZipCodeColumn { get; set; }
        public bool Phone1Column { get; set; }
        public bool Phone1DescColumn { get; set; }
        public bool NotesColumn { get; set; }
        public bool WebSiteColumn { get; set; }
        public bool EmailColumn { get; set; }
        public bool GeoLocationColumn { get; set; }
        public bool DonorTypeColumn { get; set; }
        public bool ProcurerColumn { get; set; }
        public bool DonatesColumn { get; set; }

        public int AuctionYearFilter { get; set; }
        public string GeoLocationFilter { get; set; }
        public string ProcurerFilter { get; set; }
        public string DonatesFilter { get; set; }
    }
}