using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class FinancialAccountFactory
    {
        public static FinancialAccount CreateFinancialAccount(int AFinancialAccountId, string ABranchCode, string AModuleName, string AProductCode,
                int ATransactionSubCodeId, string ADebitAccountNr, string ADebitAccountCode, string ADebitSubTransactionCode,
                string ADebitNarration, bool AIsDebitConsolidated, string ACreditAccountNr, string ACreditAccountCode,
                string ACreditSubTransactionCode, bool AIsCreditConsolidated, string ACreditNarration, DateTime? AEffectiveDate, string APortfolio,
                string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AOrganizationCode, string AApprovedBy = null)
        {

            FinancialAccount objFinancialAccount = new FinancialAccount()
            {
                FinancialAccountId = AFinancialAccountId,
                BranchCode = ABranchCode,
                ModuleName = AModuleName,
                ProductCode = AProductCode,
                TransactionSubCodeId = ATransactionSubCodeId,
                DebitAccountNr = ADebitAccountNr,
                DebitAccountCode = ADebitAccountCode,
                DebitSubTransactionCode = ADebitSubTransactionCode,
                DebitNarration = ADebitNarration,
                IsDebitConsolidated = AIsDebitConsolidated,
                CreditAccountNr = ACreditAccountNr,
                CreditAccountCode = ACreditAccountCode,
                CreditSubTransactionCode = ACreditSubTransactionCode,
                IsCreditConsolidated = AIsCreditConsolidated,
                CreditNarration = ACreditNarration,
                OrganizationCode = AOrganizationCode,
                EffectiveDate = AEffectiveDate,
                Portfolio = APortfolio,

                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objFinancialAccount;
        }

    }
}
