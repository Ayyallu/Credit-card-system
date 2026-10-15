using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CustomerDecisionFactory
    {
        public static CustomerDecision CreateCustomerDecision(int ADecisionID, int ACustomerID,string ADecisionCode,string AReasonCode,
            string ARemarks, string AContractCode, string AUnemployement, bool AIsContractAcceptedByCustomer)
        {
            CustomerDecision objCustomerDecision = new CustomerDecision() { 
                DecisionID = ADecisionID,
                CustomerId = ACustomerID,
                DecisionCode = ADecisionCode,
                ReasonCode = AReasonCode,
                Remarks = ARemarks,
                ContractCode = AContractCode,
                Unemployment = AUnemployement,
                IsContractAcceptedByCustomer = AIsContractAcceptedByCustomer
            };
            return objCustomerDecision;
        }
    }
}
