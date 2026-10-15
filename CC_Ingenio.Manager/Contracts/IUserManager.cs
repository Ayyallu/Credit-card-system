using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Core.UserRepo.UserAggregate;
using CC_Ingenio.DataTranfer.MasterInfo;

namespace CC_Ingenio.Manager.Contracts
{
    public interface IUserManager
    {
        #region Currency
        List<UserDTO> FindUser(int APageIndex, int APageCount);
        UserDTO FindByUsername(string AUsername, bool AIsFromApprovalQueue = false);
        void DeleteUser(string AUsername);
        void UpdateUserDetails(string AUsername, UserDTO AUserDTO);
        void SaveUserDetails(UserDTO AUserDTO);
        List<UserDTO> FindAll(int APageNo, UserDTO AFilter);
        LoginStatus ValidateUser(string AUsername, string APassword, string AOrganizationCode, ref string ArefDbUserName, ref string ArefUserCulture);
        DateTime GetBusinessDate();

        List<UserGroupDTO> FindUserGroup(int APageindex, int APageCount);
        UserGroupDTO FindByUserGroup(string AGroupCode);
        List<UserGroupDTO> FindAll(int APageNo, UserGroupDTO AUGDTO);
        void SaveUserGroupDetails(UserGroupDTO AUGDTO);
        void UpdateUserGroupDetails(string AGroupCode, UserGroupDTO AUGDTO);

        List<UserEntity> LoadLoginUserDetails(string AUsername, string AOrganizationCode, System.Web.HttpContext AContext);
        int SaveLoginAudit(string AUsername, string AStatus, string ANodeName, int ALoginAuditID);
        List<LoginAuditDTO> GetLoginAuditDetails(string AUserName, string ALoginStatus, DateTime ALoginFrom, DateTime ALoginTo, string ANodeName);
        decimal GetCurrentUFRate();
        #endregion

        void ChangePassword(string AUsername, string APassword, string AModifiedBy);

        List<UserDTO> GetUserDetails();
        UserDTO GetUserDetails(string AUserName);
        List<BranchDTO> GetAllBranches();
        List<UserGroupDTO> GetAllUserGroups();
        List<OrganizationDTO> GetAllOrganizations();
        List<UserPrivilegeDTO> GetUserGroupPrivilegesList(string AOrgCode, string AGroupCode, string AEntityCode);
        List<UserPrivilegeDTO> GetUserPrivilegesList(string AOrgCode, string AUserName, string AEntityCode);
        void SaveUpdatedUserGroupPrivileges(string AOrgCode, string AGroupCode, List<UserPrivilegeDTO> AUsrPrivilegeList, string loggedUser);
        void SaveUpdatedUserPrivileges(string AOrgCode, string AUserName, List<UserPrivilegeDTO> AUsrPrivilegeList, string loggedUser);
        void UpdateUserGroupPrivilegesAfterApproval(List<UserPrivilegeDTO> AUsrPrivilegeList, string loggedUser, string AQueueRefNo);
        void UpdateUserPrivilegesAfterApproval(List<UserPrivilegeDTO> AUsrPrivilegeList, string loggedUser, string AQueueRefNo);
        bool ValidateOldPassword(string AUserName, string AOldPassword);
        List<UserDTO> GetAllUsers();
        bool DeleteUser(string AUserName, string ALoggedUser);
    }
}
 