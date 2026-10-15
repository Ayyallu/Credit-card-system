using System;

namespace CC_Ingenio.Core.UserRepo.UserAggregate
{
    public static class UserGroupFactory
    {
        public static UserGroup CreateUserGroup(string AGroupCode, string AGroupName,
                        string ACreatedBy, string ALastUpdatedBy, string AApprovalStatus, string AApprovedBy = null)
        {
            UserGroup objUserGroup = new UserGroup()
            {
                GroupCode = AGroupCode,
                GroupName = AGroupName,
                CreatedBy = ACreatedBy,
                LastUpdatedBy = ALastUpdatedBy,
                ApprovedBy = AApprovedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                CreatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus
            };
            return objUserGroup;
        }
    }
}
