using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate
{

    public class ForceAuthorizationFactory
    {
        public static ForceAuthorization CreateCustomer(string ATransactionCode, string ACardNumber, string AAccountNo, string AAuthCode, DateTime ATxnDate,
                string AResponseCode, string AMerchantCode, string AMCCCode, decimal AAmount, string ABatchNo, bool AIsPosted, bool AIsManualEntry,bool AIsChecked,
                string AAuthCancellationStatus
               )
        {
            ForceAuthorization objForceAuthorization = new ForceAuthorization()
            {

                TransactionCode = ATransactionCode,
                CardNumber = ACardNumber,
                AccountNo = AAccountNo,
                AuthCode = AAuthCode ,
                TxnDate = ATxnDate,
                ResponseCode = AResponseCode,
                MerchantCode = AMerchantCode,
                MCCCode  = AMCCCode,
                Amount = AAmount ,
                BatchNo = ABatchNo ,
                IsPosted = AIsPosted ,
                IsManualEntry = AIsManualEntry,
                IsChecked = AIsChecked,
                AuthCancellationStatus = AAuthCancellationStatus
            };
           
            return objForceAuthorization;
        }
    }
}
