using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CustomerOccupationFactory
    {
        public static CustomerOccupation CreateCustomerOccupation(int AOccupationId, int ACustomerId,
            string AWorkType, decimal? ATotalMonthlyIncome, string ACompanyRUT, string ACompanyName, DateTime ADateOfJoining,
            string APost, bool AIsIncomeProofAvailable, string AIncomeType)
        {
            CustomerOccupation objCustomerOccupation = new CustomerOccupation() { 
                OccupationId = AOccupationId,
                CustomerId = ACustomerId,
                WorkType = AWorkType,
                TotalMonthlyIncome = ATotalMonthlyIncome,
                CompanyRUT = ACompanyRUT,
                CompanyName = ACompanyName,
                DateOfJoining = ADateOfJoining,
                Post = APost,
                IsIncomeProofAvailable = AIsIncomeProofAvailable,
                IncomeType = AIncomeType
            };
            return objCustomerOccupation;
        }
    }
}
