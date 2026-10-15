using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate
{
    public static class EMITransactionInstallmentFactory
    {
        public static EMITransactionInstallment CreateEMITransactionInstallments(  string ACardNumber, 
            DateTime ADueDate, string AEMIAuthNo, int AInstallmentNo,int ATotalPrincipalAmt ,int ATotalInterestAmt,
            bool AIsPosted, bool AIsChecked,int  ATotalPrepaymentInterestAmount,int ATotalPrepaymentPrincipalAmount)
       
        {
            EMITransactionInstallment objEMITransaction = new EMITransactionInstallment()
            {               
                CardNumber = ACardNumber,            
              
                EMIAuthNo = AEMIAuthNo,
                InstallmentNo = AInstallmentNo,
                TotalPrincipalAmt = ATotalPrincipalAmt,
                TotalInterestAmt  = ATotalInterestAmt,
                IsChecked  = AIsChecked,  
                IsPosted  = AIsChecked   ,
                DueDate = ADueDate ,
                TotalPrepaymentInterestAmount = ATotalPrepaymentInterestAmount ,
                TotalPrepaymentPrincipalAmount = ATotalPrepaymentPrincipalAmount
               
            };

            return objEMITransaction;
        }

    }
}