using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CustomerBillingDetailFactory
    {
        public static CustomerBillingDetail CreateCustomerBillingDetail(int ABillingDetailId, int ACustomerId, bool AIsApplicationSignedOffByCustomer,
            string ASendAccountStatementTo, string AAccountNo, string ABranchCode)// bool AIsECSRequried, DateTime APaymentDate, string AECSFor, string ABankCode
        {
            CustomerBillingDetail objCustomerBillingDetail = new CustomerBillingDetail() { 
            
                BillingDetailId = ABillingDetailId,
                CustomerId = ACustomerId,
                IsApplicationSignedOffByCustomer = AIsApplicationSignedOffByCustomer,
                SendAccountStatementTo = ASendAccountStatementTo,
                AccountNo = AAccountNo,
                BranchCode = ABranchCode
                //IsECSRequried = AIsECSRequried,
                //PaymentDate = APaymentDate,
                //ECSFor = AECSFor,
                //BankCode = ABankCode
            };
            return objCustomerBillingDetail;
        }
    }
}
