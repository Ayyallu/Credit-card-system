using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CommissionChargeDetailsFactory
    {
        public static CommissionChargeDetails CreateCommissionChargeDetails(int ACommissionChargeId, int ACommissionChargeDetailsId,
                        string ACommissionCode, decimal ACommissionAmount)
        {
            CommissionChargeDetails objCommissionChargeDetails = new CommissionChargeDetails() 
            {
                CommissionChargeId = ACommissionChargeId,
                CommissionChargeDetailsId = ACommissionChargeDetailsId,
                CommissionCode = ACommissionCode,
                CommissionAmount = ACommissionAmount
            };
            return objCommissionChargeDetails;
        }
    }
}
