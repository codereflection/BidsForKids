using System.Collections.Generic;
using System.Linq;
using BidsForKids.Data.Models;

namespace BidsForKids.Data.Repositories
{
    public interface IAuctionRepository
    {
        Auction GetBy(int year);
        Auction GetById(int id);
        IEnumerable<Auction> GetAll();
        void Add(Auction auction);
    }

    public class AuctionRepository : RepositoryBase<Auction>, IAuctionRepository
    {
        public AuctionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public Auction GetBy(int year)
        {
            return _source.Where(x => x.Year == year).FirstOrDefault();
        }
    }
}