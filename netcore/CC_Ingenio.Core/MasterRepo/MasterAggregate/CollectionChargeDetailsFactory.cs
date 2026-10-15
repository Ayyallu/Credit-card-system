using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CollectionChargeDetailsFactory
    {
        public static CollectionChargeDetails CreateCollectionChargeDetails(int ACollectionChargeDetailsId, int ACollectionChargeId,
                        decimal ADelinquencyAmountFrom, decimal ADelinquencyAmountTo, decimal ACollectionChargeAmount)
        {
            CollectionChargeDetails objCollectionChargeDetails = new CollectionChargeDetails()
            {
                CollectionChargeDetailsId = ACollectionChargeDetailsId,
                CollectionChargeId = ACollectionChargeId,
                DelinquencyAmountFrom = ADelinquencyAmountFrom,
                DelinquencyAmountTo = ADelinquencyAmountTo,
                CollectionChargeAmount = ACollectionChargeAmount
            };
            return objCollectionChargeDetails;
        }
    }
}
