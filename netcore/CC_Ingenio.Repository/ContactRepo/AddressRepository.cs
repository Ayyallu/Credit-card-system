
using CC_Ingenio.Core.ContactRepo.AddressAggregate;
using CC_Ingenio.Data;
namespace CC_Ingenio.Repository.ContactRepo
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork)
        {

        }
    }
}
