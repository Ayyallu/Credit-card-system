using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
   public class MerchantsFactory
    {
       public static Merchants CreateMerchant(string AMerchantCode, string AMerchantName, string ACountryCode, string ARegionCode, 
                        string ACityCode, string ACommunaCode,int ASettlementInDays, string ACommercialUEN, string ACommercialUENName,
                        string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string  AOrganizationCode,string AApprovedBy = null)

          
       {
           Merchants objCity = new Merchants()
           {
               MerchantCode = AMerchantCode,
               OrganizationCode = AOrganizationCode ,
               MerchantName = AMerchantName,
               CountryCode = ACountryCode,
               RegionCode = ARegionCode,
               CityCode = ACityCode,
               CommunaCode = ACommunaCode,
               SettlementInDays=ASettlementInDays,
               CommercialUEN = ACommercialUEN,
               CommercialUENName = ACommercialUENName,

               CreatedBy = ACreatedBy,
               LastUpdatedBy = ALastUpdatedBy,
               ApprovedBy = AApprovedBy,
               LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
               CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
               ApprovedOn = null,
               ApprovalStatus = AApprovalStatus
           };

           return objCity;
       }
    }
}
