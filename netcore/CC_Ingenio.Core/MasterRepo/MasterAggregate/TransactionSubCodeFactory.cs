using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class TransactionSubCodeFactory
    {
        public static TransactionSubCode CreateTransactionSubCode
            (
                int ATransactionSubCodeId, string AOrganizationCode, string ATransactionCode, string ASubCode, string ADescription, string ACreditSubSegmentCode, 
                decimal AMinDuePercentage, Boolean AIsConsumption, string AIsDebitOrCredit, Boolean AIsIVARequired,
                int? AIncomingTransType, string AFeeType,
                string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null
            )
        {
            TransactionSubCode objTransactionSubCode = new TransactionSubCode()
            {
                TransactionSubCodeId = ATransactionSubCodeId,
                OrganizationCode = AOrganizationCode,
                TransactionCode = ATransactionCode,
                SubCode = ASubCode,
                Description = ADescription,
                CreditSubSegmentCode = ACreditSubSegmentCode,
                MinDuePercentage = AMinDuePercentage,
                IsConsumption = AIsConsumption,
                IsDebitOrCredit = AIsDebitOrCredit,
                IsIVARequired = AIsIVARequired,
                IncomingTransType = AIncomingTransType,
                FeeType = AFeeType,

                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objTransactionSubCode;
        }
    }
}