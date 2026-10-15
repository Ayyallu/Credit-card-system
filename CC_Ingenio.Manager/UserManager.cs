using System;
using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Repository.MasterRepo;
using CC_Ingenio.Core.UserRepo.UserAggregate;
using CC_Ingenio.Core.MasterRepo.MasterAggregate;
using CC_Ingenio.DataTranfer.UserInfo;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.Repository.UserRepo;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using CC_Ingenio.Common.Logging;
using System.Data;

namespace CC_Ingenio.Manager
{
    public class UserManager : IUserManager
    {
        #region Variables
        UserRepository objUserRepository;
        UserGroupRepository objUserGroupRepository;
        UserAllowedBranchRepository objUserAllowedBranchRepository;
        UserGroupMembersRepository objUserGroupMembersRepository;
        BranchRepository objBranchRepository;
        OrganizationRepository objOrganizationRepository;
        UserGroupPrivilegeRepository objUserGroupPrivilegeRepository;


        #endregion

        #region Constructor
        public UserManager(UserRepository AUserRepository, UserGroupRepository AUserGroupRepository,
                           UserAllowedBranchRepository AUserAllowedBranchRepository, UserGroupMembersRepository AUserGroupMembersRepository,
                           BranchRepository ABranchRepository, OrganizationRepository AOrganizationRepository,
                           UserGroupPrivilegeRepository AUserGroupPrivilegeRepository)
        {
            if (AUserRepository == null)
                throw new ArgumentNullException("AUserRepository");
            if (AUserGroupRepository == null)
                throw new ArgumentNullException("AUsergroupRepository");
            if (AUserAllowedBranchRepository == null)
                throw new ArgumentNullException("AUserAllowedBranchRepository");
            if (AUserGroupMembersRepository == null)
                throw new ArgumentNullException("AUserGroupMembersRepository");
            if (ABranchRepository == null)
                throw new ArgumentNullException("ABranchRepository");
            if (AOrganizationRepository == null)
                throw new ArgumentNullException("AOrganizationRepository");
            objUserRepository = AUserRepository;
            objUserGroupRepository = AUserGroupRepository;
            objUserAllowedBranchRepository = AUserAllowedBranchRepository;
            objUserGroupMembersRepository = AUserGroupMembersRepository;
            objBranchRepository = ABranchRepository;
            objOrganizationRepository = AOrganizationRepository;
            objUserGroupPrivilegeRepository = AUserGroupPrivilegeRepository;
        }
        #endregion

        #region User

        public List<UserDTO> FindUser(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindUser");

            var users = objUserRepository.GetAll().Where(x => x.ApprovalStatus != "DEL").AsEnumerable();

            // objUserRepository.UnitOfWork.ReloadAllDataInContext<User>();
            if (users != null && users.Any())
            {
                List<UserDTO> lstUserDTO = new List<UserDTO>();
                foreach (var currency in users)
                {
                    lstUserDTO.Add(Conversion.Mapping.UserToUserDTO(currency));
                }
                return lstUserDTO;
            }
            else
                return new List<UserDTO>();
        }
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        public List<UserDTO> GetAllUsers()
        {
            var users = objUserRepository.GetAll();
            if (users != null && users.Any())
            {
                List<UserDTO> lstUserDTO = new List<UserDTO>();
                foreach (var usr in users)
                {
                    lstUserDTO.Add(Conversion.Mapping.UserToUserDTO(usr));
                }
                return lstUserDTO;
            }
            else
                return new List<UserDTO>();
        }
        public UserDTO FindByUsername(string AUsername, bool AIsFromApprovalQueue = false)
        {
            var user = objUserRepository.Get(AUsername);

            if (user != null)
            {
                if (AIsFromApprovalQueue)
                    objUserRepository.Modify(user);
                return Conversion.Mapping.UserToUserDTO(user);
            }
            else
            {
                return new UserDTO();
            }
        }
        public void DeleteUser(string AUsername)
        {
            var user = objUserRepository.Get(AUsername);

            if (user != null)
            {
                objUserRepository.Remove(user);
                objUserRepository.UnitOfWork.Commit();
            }

        }
        public void UpdateUserDetails(string AUsername, UserDTO AUserDTO)
        {
            if (AUserDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdateUserWithNullInformation");

            }
            User currentUser = objUserRepository.Get(AUsername);
            User updatedUser = new User()
            {
                Username = AUsername,
                Password = AUserDTO.Password,
                FullName = AUserDTO.FullName
            };

            objUserRepository.Merge(currentUser, updatedUser);
            objUserRepository.UnitOfWork.Commit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AUserDTO"></param>
        public void SaveUserDetails(UserDTO AUserDTO)
        {
            if (AUserDTO == null)
                throw new ArgumentException("Messages.warning_CannotSaveUserWithNullInformation");

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    DataTable dtGroupTable = new DataTable();
                    dtGroupTable.Columns.Add("Column1", typeof(int));
                    dtGroupTable.Columns.Add("Column2", typeof(string));
                    dtGroupTable.Columns.Add("Column3", typeof(string));
                    dtGroupTable.Columns.Add("Column4", typeof(string));
                    if (AUserDTO.UserGroupMembers != null)
                    {
                        foreach (string str in AUserDTO.UserGroupMembers)
                        {
                            dtGroupTable.Rows.Add(null, str, null, null);
                        }
                    }

                    DataTable dtBranchTable = new DataTable();
                    dtBranchTable.Columns.Add("Column1", typeof(int));
                    dtBranchTable.Columns.Add("Column2", typeof(string));
                    dtBranchTable.Columns.Add("Column3", typeof(string));
                    dtBranchTable.Columns.Add("Column4", typeof(string));
                    if (AUserDTO.UserAllowedBranchForSave != null)
                    {
                        foreach (string str in AUserDTO.UserAllowedBranchForSave)
                        {
                            if (str.IndexOf('-') > 0)
                            {
                                dtBranchTable.Rows.Add(null, str.Substring(0,str.IndexOf('-')), null, null);
                            }
                            else
                            {
                                dtBranchTable.Rows.Add(null, str, null, null);
                            }
                           
                        }
                    }

                    SqlParameter[] paramCollection = new SqlParameter[]{ 
                    new SqlParameter(parameterName: "@P_UserName",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_FullName", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_Password", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_Email", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_Active",dbType:System.Data.SqlDbType.Bit ),
                    new SqlParameter(parameterName: "@P_Culture",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(){ ParameterName="@P_GroupList", SqlDbType= SqlDbType.Structured, TypeName = "UD_General_TableType", Direction = ParameterDirection.Input},
                    new SqlParameter(){ ParameterName="@P_BranchList", SqlDbType= SqlDbType.Structured, TypeName = "UD_General_TableType", Direction = ParameterDirection.Input},
                    new SqlParameter(parameterName: "@P_CreatedBy",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_OrganizationCode",dbType:System.Data.SqlDbType.VarChar )
         
                };

                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_UserName")).First()).Value = AUserDTO.Username;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_FullName")).First()).Value = AUserDTO.FullName;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Password")).First()).Value = CC_Ingenio.Common.Utils.Utils.Helper.EncryptPassword(AUserDTO.Password);
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Email")).First()).Value = (AUserDTO.Email != null) ? AUserDTO.Email : string.Empty;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Active")).First()).Value = AUserDTO.Active;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Culture")).First()).Value = AUserDTO.CultureCode;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_GroupList")).First()).Value = dtGroupTable;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_BranchList")).First()).Value = dtBranchTable;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CreatedBy")).First()).Value = AUserDTO.CreatedBy;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_OrganizationCode")).First()).Value = AUserDTO.OrganizationCode;


                    objUserRepository.UnitOfWork.ExecuteCommand(@"PR_UserDetailsSave @P_UserName, @P_FullName, 
                        @P_Password, @P_Email, @P_Active,@P_Culture, @P_GroupList, @P_BranchList, @P_CreatedBy,@P_OrganizationCode", paramCollection);

                    scope.Complete();
                }
                catch (Exception)
                {

                }
            }
        }


        public List<UserDTO> FindAll(int APageNo, UserDTO AUserDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "Username", FilterValue = AUserDTO.Username, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "FullName", FilterValue = AUserDTO.FullName, Operator = "Contains"}
            };

            Expression<Func<User, bool>> expression = Utils.GetExpression<User>(lstExp);
            int iRecordCount = objUserRepository.GetRecordCount(x => x.Username.Contains(AUserDTO.Username));

            var vUserDTO = objUserRepository.GetFiltered(APageNo, iRecordCount, expression);
            if (vUserDTO != null && vUserDTO.Any())
            {
                List<UserDTO> lstUserDTO = new List<UserDTO>();
                foreach (var user in vUserDTO)
                {
                    lstUserDTO.Add(Conversion.Mapping.UserToUserDTO(user));
                }
                return lstUserDTO;
            }
            else
                return new List<UserDTO>();

        }
        #endregion

        #region User Group

        public List<UserGroupDTO> FindUserGroup(int APageindex, int APageCount)
        {
            var UGs = objUserGroupRepository.GetAll();
            if (UGs != null && UGs.Any())
            {
                List<UserGroupDTO> lstUGDTO = new List<UserGroupDTO>();
                foreach (var UG in UGs)
                {
                    lstUGDTO.Add(Conversion.Mapping.UserGroupToUserGroupDTO(UG));
                }
                return lstUGDTO;
            }
            else
                return new List<UserGroupDTO>();
        }

        public UserGroupDTO FindByUserGroup(string AGroupCode)
        {
            var userGroup = objUserGroupRepository.Get(AGroupCode);
            if (userGroup != null)
            {
                objUserGroupRepository.UnitOfWork.RefreshDataInModel(ref userGroup);
                return Conversion.Mapping.UserGroupToUserGroupDTO(userGroup);
            }
            else
                return new UserGroupDTO();
        }

        public List<UserGroupDTO> FindAll(int APageNo, UserGroupDTO AUGDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "GroupCode",FilterValue = AUGDTO.GroupCode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "GroupName",FilterValue = AUGDTO.GroupName,Operator = "Contains"}                      
                    };

            Expression<Func<UserGroup, bool>> expression = Utils.GetExpression<UserGroup>(lstExpression);
            int iRecordCount = objUserGroupRepository.GetRecordCount(expression);

            var vUGDTO = objUserGroupRepository.GetFiltered(0, iRecordCount, expression);

            if (vUGDTO != null && vUGDTO.Any())
            {
                List<UserGroupDTO> lstUGDTO = new List<UserGroupDTO>();
                foreach (var UG in vUGDTO)
                {
                    lstUGDTO.Add(Conversion.Mapping.UserGroupToUserGroupDTO(UG));
                }
                return lstUGDTO;
            }
            else
                return new List<UserGroupDTO>();
        }

        public void SaveUserGroupDetails(UserGroupDTO AUGDTO)
        {
            if (AUGDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddCurrencyWithNullInformation");

            var newUG = UserGroupFactory.CreateUserGroup(AUGDTO.GroupCode, AUGDTO.GroupName,
                AUGDTO.CreatedBy, AUGDTO.LastUpdatedBy, AUGDTO.ApprovalStatus);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                        new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objUserGroupRepository.Add(newUG);
                        objUserGroupRepository.UnitOfWork.Commit();                     
                        scope.Complete();
                    }
                    catch (SqlException)
                    {

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                scope.Dispose();

            }
            catch (SqlException)
            {

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void UpdateUserGroupDetails(string AGroupCode, UserGroupDTO AUGDTO)
        {
            if (AUGDTO == null)
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");

            UserGroup currentUG = objUserGroupRepository.Get(AGroupCode);
            UserGroup updateUG = new UserGroup()
            {
                GroupCode = AGroupCode,
                GroupName = AUGDTO.GroupName,
                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = AUGDTO.LastUpdatedBy,
                CreatedOn = currentUG.CreatedOn,
                CreatedBy = currentUG.CreatedBy,
                ApprovalStatus = AUGDTO.ApprovalStatus
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objUserGroupRepository.Merge(currentUG, updateUG);
                    objUserGroupRepository.UnitOfWork.Commit();  
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            scope.Dispose();
        }

        #endregion

        #region Login

        public LoginStatus ValidateUser(string AUsername, string APassword, string AOrganizationCode, ref string ADBUserName, ref string AUserCulture)
        {
            APassword = CC_Ingenio.Common.Utils.Utils.Helper.EncryptPassword(APassword);
            string str;
            str = CC_Ingenio.Common.Utils.Utils.Helper.EncryptPassword1("15032016");
            int PrivCount = 0;

            //var query = objUserRepository.GetAll().Where(x => x.Username.Equals(AUsername) && x.Password.Equals(APassword))
            //    .Join(objUserGroupMembersRepository.GetAll(), r => r.Username, p => p.UserName, (r, p) => new { r.Username, p.GroupCode })
            //    .Join(objUserGroupPrivilegeRepository.GetAll().Where(x => x.ActionValue != null), a => a.GroupCode, b => b.GroupCode, (a, b) => new { a.Username, b.OrganizationCode })
            //    .GroupBy(g => new { Id = g.OrganizationCode, Name = g.Username })
            //    .Select(g => new { Id = g.Key.Id, Name = g.Key.Name, Count = g.Count() });
            
            SqlParameter[] paramCollection = new SqlParameter[] { 
                            new SqlParameter(){ ParameterName = "@P_UserName", SqlDbType = System.Data.SqlDbType.VarChar, Value = AUsername, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ ParameterName = "@P_Password", SqlDbType = System.Data.SqlDbType.VarChar, Value = APassword, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ ParameterName = "@P_OrganizationCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = AOrganizationCode, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ ParameterName = "@P_UserPrivCount", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output, Size = 5},
                            new SqlParameter(){ ParameterName = "@P_DBUserName", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Output, Size = 50},
                            new SqlParameter(){ ParameterName = "@P_UserCulture", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Output, Size = 50}
                        };

            objUserRepository.UnitOfWork.ExecuteCommand("PR_ValidateUserOnLogin @P_UserName, @P_Password, @P_OrganizationCode, @P_UserPrivCount OUTPUT, @P_DBUserName OUTPUT, @P_UserCulture OUTPUT", paramCollection);
            //var test = objBillingCycleParameterRepository.UnitOfWork.ExecuteSqlQuery(System.Type.GetType("DateTime"), "select dbo.FN_WorkingDay( @P_Date, @P_OrganizationCode)", paramCollection);

            if (!string.IsNullOrEmpty(Convert.ToString(paramCollection[3].Value)))
                PrivCount = Convert.ToInt32(paramCollection[3].Value);

            if (!string.IsNullOrEmpty(Convert.ToString(paramCollection[4].Value)))
                ADBUserName = Convert.ToString(paramCollection[4].Value);
            
            if (!string.IsNullOrEmpty(Convert.ToString(paramCollection[5].Value)))
                AUserCulture = Convert.ToString(paramCollection[5].Value);
            
            if (PrivCount > 0)
            {
                //AOrganizationCode = validuser.Select(x => x.orgcode).FirstOrDefault();
               
                return LoginStatus.Success;
            }
            else
            {
                return LoginStatus.Fail;
            }

        }

        public List<UserEntity> LoadLoginUserDetails(string AUsername, string AOrganizationCode, System.Web.HttpContext AContext)
        {
            List<UserEntity> objResult;
            SqlParameter[] paramCollection = new SqlParameter[]
            { 
                new SqlParameter(parameterName: "@P_UserName",dbType:SqlDbType.VarChar ),
                new SqlParameter(parameterName: "@P_OrganizationCode",dbType:SqlDbType.VarChar )
            };

            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_UserName")).First()).Value = AUsername;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_OrganizationCode")).First()).Value = AOrganizationCode;

            objResult = objUserRepository.UnitOfWork.ExecuteQuery<UserEntity>("dbo.PR_UserMenuGet @P_UserName, @P_OrganizationCode",
                paramCollection).ToList();

            if (objResult != null)
            {
                List<UserPrivilegeDTO> objPrivileges = new List<UserPrivilegeDTO>();
                foreach (var UE in objResult)
                {
                    if (UE.ActionValue > 0)
                        objPrivileges.Add(Conversion.Mapping.UserEntityToUserPrivilegeDTO(UE));
                }

                LoginUserDTO objLUDTO = new LoginUserDTO();
                objLUDTO.Username = AUsername;
                objLUDTO.UserPrivileges = objPrivileges;
                objLUDTO.OrganizationCode = AOrganizationCode;
                Utils.OrganizationCode = AOrganizationCode;
                List<OrganizationDTO> objOrganization = new List<OrganizationDTO>();

                AContext.Session["LOGGED_IN_USER"] = objLUDTO;
            }
            return objResult;

        }

        public DateTime GetBusinessDate()
        {
            DateTime dt = new DateTime();
            SqlParameter[] paramCollection = new SqlParameter[] { 
                            new SqlParameter(){ ParameterName = "@P_OrganizationCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = Utils.OrganizationCode, Direction = System.Data.ParameterDirection.Input }
                            , new SqlParameter(){ParameterName = "@P_RetDate", SqlDbType = System.Data.SqlDbType.Date, Direction = System.Data.ParameterDirection.Output, Size = 25}
                        };

            objUserRepository.UnitOfWork.ExecuteCommand("PR_GetBusinessDate @P_OrganizationCode, @P_RetDate OUTPUT", paramCollection);
            dt = Convert.ToDateTime(paramCollection[1].Value);
            return dt;
        }

        public decimal GetCurrentUFRate()
        {
            SqlParameter[] paramCollection = new SqlParameter[] { 
                            new SqlParameter(){ ParameterName = "@P_OrganizationCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = Utils.OrganizationCode, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ ParameterName = "@P_Effectivedate", SqlDbType = System.Data.SqlDbType.Date, Value = Utils.BusinessDate.Date, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ParameterName = "@P_UFRate", SqlDbType = System.Data.SqlDbType.Float, Direction = System.Data.ParameterDirection.Output, Size = 25}
                        };

            objUserRepository.UnitOfWork.ExecuteCommand("PR_GetCurrentUFRate @P_OrganizationCode, @P_Effectivedate, @P_UFRate OUTPUT", paramCollection);
            decimal currentUFRate = Convert.ToDecimal(paramCollection[2].Value);
            return currentUFRate;
        }

        #endregion

        #region Login Audit
        public int SaveLoginAudit(string AUsername, string AStatus, string ANodeName, int ALoginAuditID)
        {
            SqlParameter[] paramCollection = new SqlParameter[]
            { 
                new SqlParameter(parameterName: "@P_UserName",dbType:SqlDbType.VarChar ),
                new SqlParameter(parameterName: "@P_NodeName", dbType:SqlDbType.VarChar ),
                new SqlParameter(parameterName: "@P_Flag", dbType:SqlDbType.Char ),
                new SqlParameter(){ParameterName = "@P_LoginAuditId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.InputOutput, Size = 5}
            };

            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_UserName")).First()).Value = AUsername;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_NodeName")).First()).Value = ANodeName;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Flag")).First()).Value = AStatus[0];
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_LoginAuditId")).First()).Value = ALoginAuditID;

            objUserRepository.UnitOfWork.ExecuteCommand("dbo.[PR_UserLoginAuditSave] @P_UserName, @P_NodeName, @P_Flag, @P_LoginAuditId OUTPUT",
                paramCollection);
            return Int16.Parse(paramCollection[3].Value.ToString());
        }

        public List<LoginAuditDTO> GetLoginAuditDetails(string AUserName, string ALoginStatus, DateTime ALoginFrom, DateTime ALoginTo, string ANodeName)
        {
            List<LoginAuditDTO> objResult;
            SqlParameter[] paramCollection = new SqlParameter[] 
            { 
                new SqlParameter() { ParameterName = "@P_UserName", SqlDbType = SqlDbType.VarChar, Size = 15, 
                    Value = (AUserName == null ? "%" : AUserName), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_LoginStatus", SqlDbType = SqlDbType.VarChar, Size = 5, 
                    Value = (ALoginStatus == null ? "%" : ALoginStatus), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_LoginFrom", SqlDbType = SqlDbType.DateTime, 
                    Value = (ALoginFrom == null ? DateTime.Parse("2000-01-01") : ALoginFrom), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_LoginTo", SqlDbType = SqlDbType.DateTime, 
                    Value = (ALoginTo == null ? DateTime.Parse("3000-01-01") : ALoginTo), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_NodeName", SqlDbType = SqlDbType.VarChar, Size = 15, 
                    Value = (ANodeName == null ? "%" : ANodeName), Direction = ParameterDirection.Input}
            };
            objResult = objUserRepository.UnitOfWork.ExecuteQuery<LoginAuditDTO>("dbo.[PR_UserLoginAuditGet] @P_UserName, @P_LoginStatus, @P_LoginFrom, @P_LoginTo, @P_NodeName",
                paramCollection).ToList();

            if (objResult == null)
            {
                objResult = new List<LoginAuditDTO>();
            }
            return objResult;

        }

        #endregion

        #region Change Password
        public void ChangePassword(string AUsername, string APassword, string AModifiedBy)
        {
            SqlParameter[] paramCollection = new SqlParameter[]
            { 
                new SqlParameter(parameterName: "@P_UserName",dbType:SqlDbType.VarChar ),
                new SqlParameter(parameterName: "@P_EncryptedPassword", dbType:SqlDbType.VarChar ),
                new SqlParameter(parameterName: "@P_ModifiedBy", dbType:SqlDbType.VarChar )
            };

            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_UserName")).First()).Value = AUsername;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_EncryptedPassword")).First()).Value = CC_Ingenio.Common.Utils.Utils.Helper.EncryptPassword(APassword);
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ModifiedBy")).First()).Value = AModifiedBy;

            objUserRepository.UnitOfWork.ExecuteCommand("dbo.[PR_UserSavePassword] @P_UserName, @P_EncryptedPassword, @P_ModifiedBy",
                paramCollection);
        }

        public bool ValidateOldPassword(string AUserName, string AOldPassword)
        {
            //var user = objUserRepository.Get(AUserName);

            UserDTO userDTO = GetUserDetails(AUserName);

            if (AOldPassword == userDTO.Password)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        
        #endregion

        #region UserDetails

       /// <summary>
       /// 
       /// </summary>
       /// <param name="Organizationcode"></param>
       /// <returns></returns>
        public List<OrganizationDTO> GetOrganization(string Organizationcode)
        {
            List<OrganizationDTO> objOrganizationDTOList = new List<OrganizationDTO>();
            var result = objOrganizationRepository.AllMatching(x => x.OrganizationCode == Organizationcode).ToList();
            if (result.Any())
            {
                result.ToList().ForEach(x => objOrganizationDTOList.Add(Conversion.Mapping.OrganizationToOrganizationDTO(x)));
            }
            return objOrganizationDTOList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<UserDTO> GetUserDetails()
        {
            List<UserDTO> objUserDTOList = new List<UserDTO>();
            var result = objUserRepository.GetAll().Where(x => x.ApprovalStatus != "DEL").AsEnumerable();
            if (result.Any())
            {
                result.ToList().ForEach(x => objUserDTOList.Add(Conversion.Mapping.UserToUserDTO(x)));
            }
            return objUserDTOList;
        }
        /// <summary>
        /// Returns user data from 
        /// </summary>
        /// <param name="AUserName"></param>
        /// <returns></returns>
        public UserDTO GetUserDetails(string AUserName)
        {
            UserDTO objUserDTO = new UserDTO();
            User objUser = objUserRepository.AllMatching(x => x.Username == AUserName).FirstOrDefault();
            if (objUser != null)
            {
                objUserDTO = Conversion.Mapping.UserToUserDTO(objUser);
            }

            var result = objUserAllowedBranchRepository.AllMatching(x => x.Username == AUserName).ToList();        
            List<SelectListItem> objusrAllowedBranch = new List<SelectListItem>();
            if (result.Any())
            {
                List<UserAllowedBranch> lst = result.ToList();
                foreach (UserAllowedBranch bc in lst)
                {

                    SelectListItem selItem = objBranchRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode &&
                          x.ApprovalStatus.Equals("A") && x.BranchCode == bc.BranchCode).AsEnumerable()
                          .Select(x => new SelectListItem { Value = x.BranchCode, Text = x.BranchCode + '-' + x.BranchName }).FirstOrDefault();
                    if(selItem != null)
                        objusrAllowedBranch.Add(selItem);

                }
            }           
            objUserDTO.UserAllowedBranch = objusrAllowedBranch;
            objUserDTO.UserGroupMembers = new List<string>();
            var result1 = objUserGroupMembersRepository.AllMatching(x => x.UserName == AUserName).ToList();
            if (result1.Any())
            {
                result1.ToList().ForEach(x => objUserDTO.UserGroupMembers.Add(x.GroupCode));
            }
           
            return objUserDTO;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BranchDTO> GetAllBranches()
        {
            List<BranchDTO> objBranchDTO = new List<BranchDTO>();
            var result = objBranchRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable();
            if (result.Any())
            {
                result.ToList().ForEach(x => objBranchDTO.Add(Conversion.Mapping.BranchToBranchDTO(x)));
            }
            return objBranchDTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUserDTO"></param>
        public bool DeleteUser(string AUserName, string ALoggedUser)
        {
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
               new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            bool isDeleted = false;

            using (scope)
            {
                try
                {
                    SqlParameter[] paramCollection = new SqlParameter[]{ 
                    new SqlParameter(parameterName: "@P_UserCode",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_CreatedBy",dbType:System.Data.SqlDbType.VarChar)
                      };
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_UserCode")).First()).Value = AUserName;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CreatedBy")).First()).Value = ALoggedUser;

                    objUserRepository.UnitOfWork.ExecuteCommand(@"PR_UserDetailsDelete @P_UserCode, @P_CreatedBy", paramCollection);
                    scope.Complete();
                    isDeleted = true;
                }
                catch
                {
                    isDeleted = false;
                }
            }
            return isDeleted;
        }
     
        
        #endregion

        #region UserGroupPrivilege
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<UserGroupDTO> GetAllUserGroups()
        {
            List<UserGroupDTO> objUserGroupDTO = new List<UserGroupDTO>();
            var result = objUserGroupRepository.GetAll();
            if (result.Any())
            {
                result.ToList().ForEach(x => objUserGroupDTO.Add(Conversion.Mapping.UserGroupToUserGroupDTO(x)));
            }
            return objUserGroupDTO;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<OrganizationDTO> GetAllOrganizations()
        {
            List<OrganizationDTO> objUserGroupDTO = new List<OrganizationDTO>();
            var result = objOrganizationRepository.GetAll();
            if (result.Any())
            {
                result.ToList().ForEach(x => objUserGroupDTO.Add(Conversion.Mapping.OrganizationToOrganizationDTO(x)));
            }
            return objUserGroupDTO;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AOrgCode"></param>
        /// <param name="AGroupCode"></param>
        /// <returns></returns>
        public List<UserPrivilegeDTO> GetUserGroupPrivilegesList(string AOrgCode, string AGroupCode,string AEntityCode)
        {
            List<UserPrivilegeDTO> objResult;
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter() { ParameterName = "@P_OrganizationCode", SqlDbType = SqlDbType.VarChar, Size = 5, Value = (AOrgCode == null ? string.Empty : AOrgCode), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_GroupCode", SqlDbType = SqlDbType.VarChar, Size = 5, Value =  (AGroupCode == null ? string.Empty : AGroupCode), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_EntityCode", SqlDbType = SqlDbType.VarChar, Size = 50, Value =  (AEntityCode == null ? string.Empty : AEntityCode), Direction = ParameterDirection.Input}
             };
            objResult = objUserGroupPrivilegeRepository.UnitOfWork.ExecuteQuery<UserPrivilegeDTO>("PR_UserGroupStandardPrivilegesGet @P_OrganizationCode,@P_GroupCode,@P_EntityCode", parameters).ToList();
            if (objResult == null)
            {
                objResult = new List<UserPrivilegeDTO>();
            }
            return objResult;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AOrgCode"></param>
        /// <param name="AGroupCode"></param>
        /// <param name="AUsrPrivilegeList"></param>
        /// <param name="loggedUser"></param>
        public void SaveUpdatedUserGroupPrivileges(string AOrgCode, string AGroupCode, List<UserPrivilegeDTO> AUsrPrivilegeList, string loggedUser)
        {

            DataTable dtGroupTable = new DataTable();
            dtGroupTable.Columns.Add("EntityCode", typeof(string));
            dtGroupTable.Columns.Add("EffActionValue", typeof(int));
            dtGroupTable.Columns.Add("EffSpecialAction", typeof(string));
           

            foreach (UserPrivilegeDTO objUsrprivDTO in AUsrPrivilegeList)
            {
                int x = (objUsrprivDTO.AddPrivilege == true ? 1 : 0);
                objUsrprivDTO.ActionValue = Convert.ToInt32((objUsrprivDTO.AddPrivilege == true ? "1" : "0")
                                                          + (objUsrprivDTO.UpdatePrivilege == true ? "1" : "0")
                                                          + (objUsrprivDTO.DeletePrivilege == true ? "1" : "0")
                                                          + (objUsrprivDTO.AuthorizePrivilege == true ? "1" : "0")
                                                          + (objUsrprivDTO.ViewPrivilege == true ? "1" : "0"), 2);

                dtGroupTable.Rows.Add(objUsrprivDTO.EntityCode, objUsrprivDTO.ActionValue, null);
            }
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            
            using (scope)
            {
                try
                {
                    SqlParameter[] paramCollection = new SqlParameter[]{ 
                    new SqlParameter(parameterName: "@P_OrganizationCode",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_GroupCode", dbType:System.Data.SqlDbType.VarChar ),  
                    new SqlParameter(){ ParameterName="@P_GroupList", SqlDbType= SqlDbType.Structured, TypeName = "UDT_Privilege", Direction = ParameterDirection.Input},
                    new SqlParameter(parameterName: "@P_CreatedBy",dbType:System.Data.SqlDbType.VarChar)
                      };
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_OrganizationCode")).First()).Value = AOrgCode;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_GroupCode")).First()).Value = AGroupCode;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_GroupList")).First()).Value = dtGroupTable;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CreatedBy")).First()).Value = loggedUser;

                    objUserRepository.UnitOfWork.ExecuteCommand(@"PR_UserGroupPrivilegeSave @P_OrganizationCode, @P_GroupCode, 
                                                         @P_GroupList, @P_CreatedBy", paramCollection);


                   scope.Complete();
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AUsrPrivilegeList"></param>
        /// <param name="loggedUser"></param>
        /// <param name="AQueueRefNo"></param>
        public void UpdateUserGroupPrivilegesAfterApproval(List<UserPrivilegeDTO> AUsrPrivilegeList, string loggedUser,string AQueueRefNo)
        {

            DataTable dtGroupTable = new DataTable();
            dtGroupTable.Columns.Add("EntityCode", typeof(string));
            dtGroupTable.Columns.Add("EffActionValue", typeof(int));
            dtGroupTable.Columns.Add("EffSpecialAction", typeof(string));


            foreach (UserPrivilegeDTO objUsrprivDTO in AUsrPrivilegeList)
            {                
                objUsrprivDTO.ActionValue = Convert.ToInt32((objUsrprivDTO.AddPrivilege == true ? "1" : "0")
                                                          + (objUsrprivDTO.UpdatePrivilege == true ? "1" : "0")
                                                          + (objUsrprivDTO.DeletePrivilege == true ? "1" : "0")
                                                          + (objUsrprivDTO.AuthorizePrivilege == true ? "1" : "0")
                                                          + (objUsrprivDTO.ViewPrivilege == true ? "1" : "0"), 2);

                dtGroupTable.Rows.Add(objUsrprivDTO.EntityCode, objUsrprivDTO.ActionValue, null);
            }
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });

            using (scope)
            {
                try
                {
                    SqlParameter[] paramCollection = new SqlParameter[]{ 
                   
                    new SqlParameter(){ ParameterName="@P_GroupList", SqlDbType= SqlDbType.Structured, TypeName = "UDT_Privilege", Direction = ParameterDirection.Input},
                    new SqlParameter(parameterName: "@P_CreatedBy",dbType:System.Data.SqlDbType.VarChar),
                     new SqlParameter(parameterName: "@P_QueueRefNo",dbType:System.Data.SqlDbType.VarChar),
                      };
                 
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_GroupList")).First()).Value = dtGroupTable;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CreatedBy")).First()).Value = loggedUser;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_QueueRefNo")).First()).Value = AQueueRefNo;

                    objUserRepository.UnitOfWork.ExecuteCommand(@"PR_UserGroupPrivilegeUpdateAfterApproval 
                                                         @P_GroupList, @P_CreatedBy,@P_QueueRefNo", paramCollection);

                    scope.Complete();
                }
                catch
                {
                }
            }
        }        

        #endregion

        #region Userprivileges
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AOrgCode"></param>
        /// <param name="AUserName"></param>
        /// <param name="AEntityCode"></param>
        /// <returns></returns>
        public List<UserPrivilegeDTO> GetUserPrivilegesList(string AOrgCode, string AUserName, string AEntityCode)
        {
            List<UserPrivilegeDTO> objResult;
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter() { ParameterName = "@P_OrganizationCode", SqlDbType = SqlDbType.VarChar, Size = 5, Value = (AOrgCode == null ? string.Empty : AOrgCode), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_UserName", SqlDbType = SqlDbType.VarChar, Size = 15, Value =  (AUserName == null ? string.Empty : AUserName), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_EntityCode", SqlDbType = SqlDbType.VarChar, Size = 50, Value =  (AEntityCode == null ? string.Empty : AEntityCode), Direction = ParameterDirection.Input}
             };
            objResult = objUserGroupPrivilegeRepository.UnitOfWork.ExecuteQuery<UserPrivilegeDTO>("PR_UserPrivilegesStandardGet @P_OrganizationCode,@P_UserName,@P_EntityCode", parameters).ToList();
            if (objResult == null)
            {
                objResult = new List<UserPrivilegeDTO>();
            }
            return objResult;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AOrgCode"></param>
        /// <param name="AUserName"></param>
        /// <param name="AUsrPrivilegeList"></param>
        /// <param name="loggedUser"></param>
        public void SaveUpdatedUserPrivileges(string AOrgCode, string AUserName, List<UserPrivilegeDTO> AUsrPrivilegeList, string loggedUser)
        {

            DataTable dtUserTable = new DataTable();
            dtUserTable.Columns.Add("EntityCode", typeof(string));
            dtUserTable.Columns.Add("EffActionValue", typeof(int));
            dtUserTable.Columns.Add("EffSpecialAction", typeof(string));


            foreach (UserPrivilegeDTO objUsrprivDTO in AUsrPrivilegeList)
            {
                //Save only the changed rows.Update/Insert USR_UserPrivilegeStandard Table
                if (objUsrprivDTO.IsChanged)
                {
                    objUsrprivDTO.ActionValue = Convert.ToInt32((objUsrprivDTO.AddPrivilege == true ? "1" : "0")
                                                              + (objUsrprivDTO.UpdatePrivilege == true ? "1" : "0")
                                                              + (objUsrprivDTO.DeletePrivilege == true ? "1" : "0")
                                                              + (objUsrprivDTO.AuthorizePrivilege == true ? "1" : "0")
                                                              + (objUsrprivDTO.ViewPrivilege == true ? "1" : "0"), 2);

                    dtUserTable.Rows.Add(objUsrprivDTO.EntityCode, objUsrprivDTO.ActionValue, null);
                }
            }
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });

            using (scope)
            {
                try
                {
                    SqlParameter[] paramCollection = new SqlParameter[]{ 
                    new SqlParameter(parameterName: "@P_OrganizationCode",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_UserName", dbType:System.Data.SqlDbType.VarChar ),  
                    new SqlParameter(){ ParameterName="@P_UserPrivList", SqlDbType= SqlDbType.Structured, TypeName = "UDT_Privilege", Direction = ParameterDirection.Input},
                    new SqlParameter(parameterName: "@P_CreatedBy",dbType:System.Data.SqlDbType.VarChar)
                      };
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_OrganizationCode")).First()).Value = AOrgCode;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_UserName")).First()).Value = AUserName;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_UserPrivList")).First()).Value = dtUserTable;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CreatedBy")).First()).Value = loggedUser;

                    objUserRepository.UnitOfWork.ExecuteCommand(@"PR_UserPrivilegesSave @P_OrganizationCode, @P_UserName, 
                                                         @P_UserPrivList, @P_CreatedBy", paramCollection);


                    scope.Complete();
                }
                catch
                {
                }
            }
        }

        public void UpdateUserPrivilegesAfterApproval(List<UserPrivilegeDTO> AUsrPrivilegeList, string loggedUser, string AQueueRefNo)
        {

            DataTable dtUserTable = new DataTable();
            dtUserTable.Columns.Add("EntityCode", typeof(string));
            dtUserTable.Columns.Add("EffActionValue", typeof(int));
            dtUserTable.Columns.Add("EffSpecialAction", typeof(string));


            foreach (UserPrivilegeDTO objUsrprivDTO in AUsrPrivilegeList)
            {
                objUsrprivDTO.ActionValue = Convert.ToInt32((objUsrprivDTO.AddPrivilege == true ? "1" : "0")
                                                          + (objUsrprivDTO.UpdatePrivilege == true ? "1" : "0")
                                                          + (objUsrprivDTO.DeletePrivilege == true ? "1" : "0")
                                                          + (objUsrprivDTO.AuthorizePrivilege == true ? "1" : "0")
                                                          + (objUsrprivDTO.ViewPrivilege == true ? "1" : "0"), 2);

                dtUserTable.Rows.Add(objUsrprivDTO.EntityCode, objUsrprivDTO.ActionValue, null);
            }
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });

            using (scope)
            {
                try
                {
                    SqlParameter[] paramCollection = new SqlParameter[]{ 
                   
                    new SqlParameter(){ ParameterName="@P_UserPrivList", SqlDbType= SqlDbType.Structured, TypeName = "UDT_Privilege", Direction = ParameterDirection.Input},
                    new SqlParameter(parameterName: "@P_CreatedBy",dbType:System.Data.SqlDbType.VarChar),
                     new SqlParameter(parameterName: "@P_QueueRefNo",dbType:System.Data.SqlDbType.VarChar),
                      };

                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_UserPrivList")).First()).Value = dtUserTable;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CreatedBy")).First()).Value = loggedUser;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_QueueRefNo")).First()).Value = AQueueRefNo;

                    objUserRepository.UnitOfWork.ExecuteCommand(@"PR_UserPrivilegeUpdateAfterApproval 
                                                         @P_UserPrivList, @P_CreatedBy,@P_QueueRefNo", paramCollection);

                    scope.Complete();
                }
                catch
                {
                }
            }
        }        
        #endregion

    }
}
