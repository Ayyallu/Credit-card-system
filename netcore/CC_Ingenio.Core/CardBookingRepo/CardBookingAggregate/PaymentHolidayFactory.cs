using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate
{
    public static class PaymentHolidayFactory
    {
        public static PaymentHoliday CreatePaymentHoliday( string ACardNumber, string AProductCode,string ARUT,
            DateTime AFromDate, DateTime AToDate, string AApprovalStatus,
            string ACreatedBy, string ALastUpdatedBy,string AOrganizationCode, string AApprovedBy = null)
       
        {
            PaymentHoliday objPaymentHoliday = new PaymentHoliday()
            {               
                CardNumber = ACardNumber,
                ProductCode = AProductCode,                
                FromDate = AFromDate,
                ToDate = AToDate,
                ApprovalStatus = AApprovalStatus,
                OrganizationCode = AOrganizationCode,
                RUT=ARUT,

                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
               
            };

            return objPaymentHoliday;
        }

    }
}