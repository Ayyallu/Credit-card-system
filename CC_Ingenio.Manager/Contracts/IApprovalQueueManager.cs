using CC_Ingenio.Core.ApprovalRepo.ApprovalAggregate;
using CC_Ingenio.DataTranfer.ApprovalQueueInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Manager.Contracts
{
   public interface IApprovalQueueManager
    {

       List<ApprovalQueueDTO> GetPendingItems(int APageNo, ApprovalQueueDTO AApprovalQueueFilter);
       int GetPendingItemsRecordCount(ApprovalQueueDTO AApprovalQueueFilter);
       string AuthorizeItems(ApprovalQueueDTO AApprovalQueueDTO, string ATableContext);
       string AuthorizeItemsAll(string AContextCode, string AReasonCode, string AReasonDescription, string AApprovedBy, string AApprovalStatus, DataTable ARequestIds);
       string GetEntityFromContext(string AContextCode);
    }
}
