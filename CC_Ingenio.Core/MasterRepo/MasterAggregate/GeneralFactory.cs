using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class GeneralFactory
    {
        public static General CreateGeneral(string AKeyName, string AValue1, string AValue2, DateTime AEffectiveFromDate,
                        DateTime AEffectiveToDate, string ACreatedBy, string ALastUpdatedBy,
                        string AApprovalStatus,string AOrganizationCode, string AApprovedBy = null)
        {
            General objGeneral = new General()
            {
                KeyName = AKeyName,
                OrganizationCode = AOrganizationCode ,
                Value1 = AValue1,
                Value2 = AValue2,
                EffectiveFromDate = AEffectiveFromDate,
                EffectiveToDate = AEffectiveToDate,
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objGeneral;
        }
    }
}
