using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class SystemMaintenanceConfigurationFactory
    {
        public static SystemMaintenanceConfiguration CreateSystemMaintenanceConfiguration( string ABaseCurrency,
        int  AWeeklyGlobalCount ,
        int AWeeklySetUp,
        int ACashLimit ,
        int ACreditLimit,
        string ADefaultBoost ,
        int AReissueDamaged ,
        int AClosedDamaged ,
        string ADefaultMerchantChargeType ,
        string ADefaultChargeTypeDescrption ,
        int AMinimumDueMerchantCode ,
        int ANumberOfSegments ,
        string ACreatedBy,
            string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
             SystemMaintenanceConfiguration objSystemMaintenanceConfiguration= new  SystemMaintenanceConfiguration()
            {
                BaseCurrency = ABaseCurrency,
                WeeklyGlobalCount = AWeeklyGlobalCount,
                WeeklySetUp = AWeeklySetUp,
                CashLimit = ACashLimit,
                CreditLimit = ACreditLimit,
                DefaultBoost = ADefaultBoost,
                ReissueDamaged = AReissueDamaged,
                ClosedDamaged = AClosedDamaged,
                DefaultMerchantChargeType = ADefaultMerchantChargeType,
                DefaultChargeTypeDescrption = ADefaultChargeTypeDescrption,
                MinimumDueMerchantCode = AMinimumDueMerchantCode,
                NumberOfSegments = ANumberOfSegments,
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
             return objSystemMaintenanceConfiguration;
        }
    }
}
