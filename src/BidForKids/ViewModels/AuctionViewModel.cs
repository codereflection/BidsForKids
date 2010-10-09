using AutoMapper;
using BidsForKids.Data.Models;

namespace BidsForKids.ViewModels
{
    public class AuctionViewModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }

        public static IMappingExpression<Auction, AuctionViewModel> CreateDestinationMap()
        {
            return Mapper.CreateMap<Auction, AuctionViewModel>()
                                        .ForMember(dest => dest.Id, opt => opt.MapFrom(a => a.Auction_ID));
        }

        public static IMappingExpression<AuctionViewModel, Auction> CreateSourceMap()
        {
            return Mapper.CreateMap<AuctionViewModel, Auction>()
                                        .ForMember(dest => dest.Auction_ID, opt => opt.MapFrom(a => a.Id));
        }

    }
}