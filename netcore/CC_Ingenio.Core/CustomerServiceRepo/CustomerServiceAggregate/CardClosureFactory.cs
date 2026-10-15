using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
    public class CardClosureFactory
    {
        public static CardClosure CreateCardClosure(
            int ACustomerId, string ACardNumber, string AClosureReasonCode, DateTime AClosureDate, bool AIsCardReceived, bool AIsDocumentReceived, string ARemarks,
            string ACreatedBy, string ALastUpdatedBy)
        {
            CardClosure objCardClosure = new CardClosure() { 

            CustomerId = ACustomerId,
            CardNumber = ACardNumber,
            ClosureReasonCode = AClosureReasonCode,
            ClosureDate = AClosureDate,
            IsCardReceived = AIsCardReceived,
            IsDocumentReceived = AIsDocumentReceived,
            Remarks = ARemarks,
            CreatedBy = ACreatedBy,
            CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
            LastUpdatedBy = ALastUpdatedBy,
            LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
            ApprovedOn = null,
            };
            return objCardClosure;
        }
    }
}
