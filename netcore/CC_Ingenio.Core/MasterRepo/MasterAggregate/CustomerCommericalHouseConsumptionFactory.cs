using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CustomerCommericalHouseConsumptionFactory
    {
        public static CustomerCommericalHouseConsumption CreateCustomerCommericalHouseConsumption(int ACommericalId, int ACustomerId, 
            string AType, string AName, string ACardNumber, string AAccountStatus)
        {
            CustomerCommericalHouseConsumption objCustomerCommericalHouseConsumption = new CustomerCommericalHouseConsumption() { 
                CommericalId = ACommericalId,
                CustomerId = ACustomerId,
                Type = AType,
                Name = AName,
                CardNumber = ACardNumber,
                AccountStatus = AAccountStatus
            };
            return objCustomerCommericalHouseConsumption;
        }
    }
}
