using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CustomerCurrentAccountDetailFactory
    {
        public static CustomerCurrentAccountDetail CreateCustomerCurrentAccountDetail(int AAccountId, int ACustomerId, string ABankCode, string AAccountNumber, string ABankType)
        {
            CustomerCurrentAccountDetail objCustomerCurrentAccountDetail = new CustomerCurrentAccountDetail() { 
                AccountId = AAccountId,
                CustomerId = ACustomerId,
                BankCode = ABankCode,
                AccountNumber = AAccountNumber,
                BankType = ABankType
            };
            return objCustomerCurrentAccountDetail;
        }
    }
}
