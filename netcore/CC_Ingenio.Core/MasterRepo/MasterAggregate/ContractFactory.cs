using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class ContractFactory 
    {
        public static Contract CreateContract(string AContractCode,string AContractDescription,//string AProductCode,
            string AContractTemplateCode,DateTime AEffectiveFromDate,DateTime AEffectiveToDate,string ANotaryName,
            DateTime ANotarySignDate, string ARepertoire, string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, 
            string AOrganizationCode ,string AApprovedBy = null)
        {
            Contract objContract = new Contract()
            {
                ContractCode = AContractCode,
                OrganizationCode = AOrganizationCode,
                ContractDescription = AContractDescription,
                ContractTemplateCode = AContractTemplateCode,
                //ProductCode = AProductCode,
                EffectiveFromDate = AEffectiveFromDate,
                EffectiveToDate = AEffectiveToDate,
                NotaryName = ANotaryName,
                NotarySignDate = ANotarySignDate,
                Repertoire = ARepertoire,
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objContract;
        }
    }
}
