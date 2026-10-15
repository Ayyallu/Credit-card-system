using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class ContractTemplateFactory
    {
        public ContractTemplate CreateContractTemplate(string AContractTemplateCode, string AContractTemplateDescription, string AOrganizationCode,
            string ACreatedBy, string ALastUpdatedBy, string AApprovedBy = null)
        {
            ContractTemplate objContractTemplate = new ContractTemplate()
            {
                ContractTemplateCode = AContractTemplateCode,
                ContractTemplateDescription = AContractTemplateDescription,
                OrganizationCode = AOrganizationCode,
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null
            };

            return objContractTemplate;        
        }
    }
}
