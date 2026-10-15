using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
  public class StatementInterestFactory
    {
      public static StatementInterest CreateStatementInterest(string AOrganizationCode, int? ABillingMonth, int?  ABillingYear,
                decimal AInstallmentInterest, decimal ARevolvingInterest, decimal ARevolvingCAE, decimal AInstallmentCAE,
                string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus)
      {
          StatementInterest objStatementInterest = new StatementInterest()
          {
              OrganizationCode = AOrganizationCode,
              BillingMonth = ABillingMonth,
              BillingYear = ABillingYear,
              InstallmentInterest = AInstallmentInterest,
              RevolvingInterest = ARevolvingInterest,
              RevolvingCAE = ARevolvingCAE,
              InstallmentCAE = AInstallmentCAE,
              CreatedBy = ACreatedBy,
              LastUpdatedBy = ALastUpdatedBy,
              ApprovalStatus = AApprovalStatus
          };
          return objStatementInterest;
      }
    }
}
