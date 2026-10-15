using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CalendarDateMaintenanceFactory
    {
        public static CalendarDateMaintenance CreateCalendarDateMaintenance(int AId, string AOrganizationCode, string AType, DateTime ADate, string ADescription,
                         string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            CalendarDateMaintenance objCalendarDateMaintenance = new CalendarDateMaintenance()
            {
                Id = AId,
                OrganizationCode = AOrganizationCode,
                Type = AType,
                Date = ADate,
                Description = ADescription,

                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };

            return objCalendarDateMaintenance;
        }
    }
}
