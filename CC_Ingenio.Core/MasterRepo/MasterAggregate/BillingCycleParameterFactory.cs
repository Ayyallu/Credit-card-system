using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public static class BillingCycleParameterFactory
    {
        public static BillingCycleParameter CreateBillingCycleParameter(int ABillingCycleParameterId, int ABillMonth, int ABillYear,
            int ABillCycleSysParamId, string ABillingCycleCode, DateTime ABillDate, DateTime AStatementDate, DateTime APaymentDueDate,
            string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AOrganizationCode, 
            DateTime? ABillFrom, DateTime? ABillTo, string AApprovedBy = null)
        {
            BillingCycleParameter objBillingCycleParameter = new BillingCycleParameter() 
            {
                BillingCycleParameterId = ABillingCycleParameterId,
                BillMonth = ABillMonth,
                BillYear = ABillYear,
                BillCycleSysParamId = ABillCycleSysParamId,
                BillingCycleCode = ABillingCycleCode,
                BillDate = ABillDate,
                StatementDate = AStatementDate,
                PaymentDueDate = APaymentDueDate,
                OrganizationCode = AOrganizationCode,
                BillFrom = ABillFrom,
                BillTo = ABillTo,

                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objBillingCycleParameter;
        }
    }
}
