using System.Collections.Generic;
using System.Linq;
using BidsForKids.Data.Models;

namespace BidsForKids.Data.Repositories
{
    public interface IAuctionRepository
    {
        Auction GetBy(int year);
        IEnumerable<Auction> GetAll();
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