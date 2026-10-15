using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class BlockCodeFactory
    {
        public static BlockCode CreateBlockCode(string ACode, string AOrganizationCode, string ADescription, string AShortDescription, 
            bool AIsAccountBlocked, bool AIsAccountBlockedForFinancialReasons, bool AIsAccountBlockedForNonFinancialReasons, 
            decimal AFinancialHierarchy, decimal ANonFinancialHierarchy, bool AIsCardBlocked, decimal ACardBlockHierarchy,
            bool AIsPermanantBlock, bool APlasticRecoveryNeeded, bool AInstallmentClose, bool AUnblock,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            BlockCode objBlockCode = new BlockCode()
            {
                Code = ACode,
                OrganizationCode = AOrganizationCode,
                Description = ADescription,
                ShortDescription = AShortDescription,
                IsAccountBlocked = AIsAccountBlocked,
                IsAccountBlockedForFinancialReasons = AIsAccountBlockedForFinancialReasons,
                IsAccountBlockedForNonFinancialReasons = AIsAccountBlockedForNonFinancialReasons,
                FinancialHierarchy = AFinancialHierarchy,
                NonFinancialHierarchy = ANonFinancialHierarchy,
                IsCardBlocked = AIsCardBlocked,
                CardBlockHierarchy = ACardBlockHierarchy,
                IsPermanantBlock = AIsPermanantBlock,
                PlasticRecoveryNeeded = APlasticRecoveryNeeded,
                InstallmentClose = AInstallmentClose,
                Unblock = AUnblock,
                
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objBlockCode;
        }
    }
}
