using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class PlanMasterFactory
    {
    public static PlanMaster CreatePlanMaster(int APlanID,
            string ACreditSegmentCode, 
            string AProductCode,
            string ACustomerSegmentCode, 
            string ACustomerSubSegmentCode,
            string ATransactionCode , 
            decimal ACreditLimitFrom, 
	        decimal ACreditLimitTo ,
            //decimal ATenureFrom,
            decimal ATenureTo,
	        DateTime AValidFromDate ,
            //DateTime AValidToDate,
	        decimal ASBIFInterest,
	        decimal AExpectedInterestRate,
            decimal ADeferredPayment,
            string AInterestCalculation,
            string AFirstBilling,
            string ACreatedBy ,
            string ALastUpdatedBy, 
            string AApprovedBy,     
            string AOrganizationCode,
            string AApprovalStatus 
           )
        
    {
         PlanMaster objPlanMaster = new PlanMaster()
         {
            PlanID = APlanID,
            CreditSegmentCode = ACreditSegmentCode,
            ProductCode = AProductCode,
            CustomerSegmentCode = ACustomerSegmentCode,
            CustomerSubSegmentCode = ACustomerSubSegmentCode,
            TransactionCode = ATransactionCode,
            CreditLimitFrom = ACreditLimitFrom ,
	        CreditLimitTo = ACreditLimitTo,
            //TenureFrom = ATenureFrom,
	        TenureTo = ATenureTo,
	        ValidFromDate = AValidFromDate,
            //ValidToDate = AValidToDate,
	        SBIFInterest = ASBIFInterest,
	        ExpectedInterestRate = AExpectedInterestRate, 
	        DeferredPayment = ADeferredPayment, 
	        InterestCalculation = AInterestCalculation,
	        FirstBilling = AFirstBilling, 
            OrganizationCode = AOrganizationCode,

            CreatedBy = ACreatedBy,
            CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
            ApprovedBy = AApprovedBy, 
            ApprovedOn = null, 
            LastUpdatedBy = ALastUpdatedBy, 
            LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
            ApprovalStatus = AApprovalStatus
           
         };

         return objPlanMaster;
    }

    }

}
