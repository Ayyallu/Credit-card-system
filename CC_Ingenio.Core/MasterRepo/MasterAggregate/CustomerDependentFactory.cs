using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CustomerDependentFactory
    {
        public static CustomerDependent CreateCustomerDependent(int ADependentID, int ACustomerId, string AAgeGroup, int ANoOfChildren)
        {
            CustomerDependent objCustomerDependent = new CustomerDependent() { 
                DependentID = ADependentID,
                CustomerId = ACustomerId,
                AgeGroup = AAgeGroup,
                NoOfChildren = ANoOfChildren
            };
            return objCustomerDependent;
        }
    }
}
