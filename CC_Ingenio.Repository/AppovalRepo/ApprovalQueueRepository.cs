using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using CC_Ingenio.Core.ApprovalRepo.ApprovalAggregate;
using CC_Ingenio.Data;

namespace CC_Ingenio.Repository.AppovalRepo
{
    public class ApprovalQueueRepository : GenericRepository<ApprovalQueue>, IApprovalQueueRepository
    {
        public ApprovalQueueRepository(UnitOfWork AUnitOfWork)
               :base(AUnitOfWork) 
        {

        }
    }
}   
