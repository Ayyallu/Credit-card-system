using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate
{

    public class EMIAuthorizationFactory
    {
        public static EMIAuthorization CreateCustomer(string AEMIAuthNo, string ACardNumber, string AAccountNo, string ATransactionCode, DateTime ALoanBookedDate,
                string AMerchantCode, int TotalInstallment, int ATotallInterestAmtPaid, int ATotalInterestAmt, int ATotalPrincipalAmt,
            int ATotalPrincipalAmtPaid,  bool AIsChecked ,bool AIsClosed,string ARut 
               )
        {
            EMIAuthorization objForceAuthorization = new EMIAuthorization()
            {

              
                RUT = ARut ,
                TransactionCode = ATransactionCode,
                CardNumber = ACardNumber,
                AccountNo = AAccountNo,
                EMIAuthNo = AEMIAuthNo  ,
                LoanBookedDate = ALoanBookedDate,
                TotalInstallment = TotalInstallment,
                MerchantCode = AMerchantCode,
                TotallInterestAmtPaid   = ATotallInterestAmtPaid,
                TotalInterestAmt = ATotalInterestAmt  ,
                TotalPrincipalAmt  = ATotalPrincipalAmt ,
                TotalPrincipalAmtPaid  = ATotalPrincipalAmtPaid ,               
                IsChecked = AIsChecked,
                IsClosed = AIsClosed

            };

           
            return objForceAuthorization;
        }
    }
}
