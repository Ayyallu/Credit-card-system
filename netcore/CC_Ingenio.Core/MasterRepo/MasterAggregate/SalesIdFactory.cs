using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Common;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class SalesIdFactory
    {
        public static SaleId CreateSalesID(string AOrganizationCode, string ASalesID,string ARUT,string ABranchCode,
               string AAddress, string ACommunaCode, string ACityCode, 
                string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus) 
        {
            SaleId objSalesId= new SaleId() 
            { 
                OrganizationCode = AOrganizationCode,
                SalesId = ASalesID ,
                RUT = ARUT,
                BranchCode = ABranchCode,
                Address = AAddress,                
                CommunaCode = ACommunaCode,
                CityCode = ACityCode, 
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                CreatedOn = CC_Ingenio .Common .Utils .Utils .BusinessDate ,
                LastUpdatedOn  = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovalStatus = AApprovalStatus
            };
            return objSalesId;
        }
    }
}
