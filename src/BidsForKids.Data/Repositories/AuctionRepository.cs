using System.Linq;
using BidsForKids.Data.Models;

namespace BidsForKids.Data.Repositories
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