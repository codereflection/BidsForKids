using System.Linq;
using BidsForKids.Data.Models;
using BidsForKids.Data.Repositories;

namespace BidsForKids.Tests.Data
{
    public class AuctionRepository : RepositoryBase<Auction>
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