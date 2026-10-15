using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate
{
    public static class EMITransactionFactory
    {
        public static EMITransaction CreateManualTransaction( int APlanId, string ACardNumber, string AMerchantCode,
            DateTime ALoanBookedDate, string AAuthCode, decimal ALoanAmount, int ATotalInstallment,
            string ACreatedBy, string ALastUpdatedBy, string AApprovedBy = null)
       
        {
            EMITransaction objEMITransaction = new EMITransaction()
            {               
                CardNumber = ACardNumber,            
                LoanBookedDate  = ALoanBookedDate,
                EMIAuthNo = AAuthCode,
                LoanAmount = ALoanAmount,
                MerchantCode = AMerchantCode,
                TotalInstallment = ATotalInstallment,
                PlanId = APlanId,  
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
               
            };

            return objEMITransaction;
        }

    }
}