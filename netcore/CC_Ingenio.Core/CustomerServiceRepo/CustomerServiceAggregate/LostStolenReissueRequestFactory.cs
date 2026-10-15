using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
    public class LostStolenReissueRequestFactory
    {
        public static LostStolenReissueRequest CreateLostStolenReissueRequest(string ARUT, string ARUTVerificationDigit, string ACardNumber,
                string ASerialNumber, DateTime AExpiryDate, string ANewContractCode,
                string ACreatedBy, DateTime ACreatedOn, string ALastUpdatedBy, DateTime ALastUpdatedOn, string AApprovalStatus )
        {
            LostStolenReissueRequest objLostStolenReissueRequest = new LostStolenReissueRequest() 
            {
                RUT = ARUT,
                RUTVerificationDigit = ARUTVerificationDigit,
                CardNumber = ACardNumber,
                SerialNumber = ASerialNumber,
                ExpiryDate = AExpiryDate,
                NewContractCode = ANewContractCode,

                CreatedBy = ACreatedBy,
                CreatedOn = ACreatedOn,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = ALastUpdatedOn,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objLostStolenReissueRequest;
        }
    }
}

