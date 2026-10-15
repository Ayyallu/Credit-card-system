
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class AdjustmentFinancialAccountFactory
    {
        public static AdjustmentFinancialAccount CreateFinancialAccount(int AAdjustmentFinancialAccountId, string ABranchCode, string AModuleName, 
                string AProductCode, int ATransactionSubCodeId, string ADebitAccountNr, string ADebitNarration, string AAdjustmentCode,
                string AAdjustmentFinancialActivityCode, string ACreditAccountNr, string ACreditNarration, DateTime? AEffectiveDate, 
                string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AOrganizationCode, string AApprovedBy = null)
        {

            AdjustmentFinancialAccount objAdjustmentFinancialAccount = new AdjustmentFinancialAccount()
            {
                AdjustmentFinancialAccountId = AAdjustmentFinancialAccountId,
                BranchCode = ABranchCode,
                ModuleName = AModuleName,
                ProductCode = AProductCode,
                TransactionSubCodeId = ATransactionSubCodeId,
                AdjustmentCode = AAdjustmentCode,
                AdjustmentFinancialActivityCode = AAdjustmentFinancialActivityCode,
                DebitAccountNr = ADebitAccountNr,
                DebitNarration = ADebitNarration,
                CreditAccountNr = ACreditAccountNr,
                CreditNarration = ACreditNarration,
                OrganizationCode = AOrganizationCode,
                EffectiveDate = AEffectiveDate,

                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objAdjustmentFinancialAccount;
        }

    }
}
