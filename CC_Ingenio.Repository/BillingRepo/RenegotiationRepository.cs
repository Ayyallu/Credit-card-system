using CC_Ingenio.Core.BillingRepo.BillingAggregate;
using CC_Ingenio.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Repository.BillingRepo
{
    public class RenegotiationRepository: GenericRepository<Renegotiation>,IRenegotiationRepository
    {
        public RenegotiationRepository(UnitOfWork AUnitOfWork)
            : base(AUnitOfWork)
        {

        }
    }
}
