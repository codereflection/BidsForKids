using BidsForKids.Data.Repositories;
using Machine.Specifications;

namespace BidsForKids.Tests.Data
{
    public abstract class InMemorySpecificationBase
    {
        protected static IUnitOfWork unitOfWork;

        Establish context = () =>
                            unitOfWork = new InMemoryUnitOfWork();
    }
}