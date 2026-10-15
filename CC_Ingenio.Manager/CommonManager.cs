using System;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;
using CC_Ingenio.DataTranfer.UserInfo;
using CC_Ingenio.Common.Logging;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.Manager
{
    public class CommonManager 
    {
        HttpContext CurrentContext;

        #region Constructor
        public CommonManager(HttpContext AContext)
        {
            CurrentContext = AContext;
        }
        #endregion
        
        #region GetApprovalQueueProcedureParameters
        public static SqlParameter[] GetApprovalQueueProcedureParameters(string ATableName, string AWhereClause, string AContextCode
                                                        , string AUpdatedBy, string AReturnUrl, string ATransactionType)
        {
            SqlParameter[] paramCollection = new SqlParameter[]{ 
                    new SqlParameter(parameterName: "@P_TableName",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_ColumnName", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_Value", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_WhereClause",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_ContextCode",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_LoginName",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_ReturnUrl",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_TransactionType",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_OrganizationCode",dbType:System.Data.SqlDbType.VarChar )
                };

            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TableName")).First()).Value = ATableName;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ColumnName")).First()).Value = "ApprovalStatus";
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Value")).First()).Value = "P";
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_WhereClause")).First()).Value = AWhereClause;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ContextCode")).First()).Value = AContextCode;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_LoginName")).First()).Value = AUpdatedBy;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ReturnUrl")).First()).Value = AReturnUrl;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TransactionType")).First()).Value = ATransactionType;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_OrganizationCode")).First()).Value = Utils.OrganizationCode;

            return paramCollection;
        }
        #endregion
        
        [NonAction]
        public string GetLoggedUserName()
        {
            try
            {
                return ((LoginUserDTO)CurrentContext.Session["LOGGED_IN_USER"]).Username;
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
            }
            return "";
        }

        [NonAction]
        public List<String> GetUserPrivilegesForEntity (string AEntityName)
        {
            
            List<UserPrivilegeDTO> objPrivilegesDTO = (List<UserPrivilegeDTO>)((LoginUserDTO)CurrentContext.Session["LOGGED_IN_USER"]).UserPrivileges;
            int[] arrActionValue =((objPrivilegesDTO.Where(x => x.EntityCode.Equals(AEntityName)).Select(y => y.ActionValue)).ToArray()) ;

            if (arrActionValue.Length == 0)
            {
                //
                return null;
            }

            int intActionValue = arrActionValue[0];

            List<String> lstPrivileges = new List<string>();

            int iLoop = 5;

            while (intActionValue > 0)
            {
                switch (iLoop)
                {
                    case 5: if (intActionValue % 2 > 0) lstPrivileges.Add("View");
                        break;
                    case 4: if (intActionValue % 2 > 0) lstPrivileges.Add("Authorize");
                        break;
                    case 3: if (intActionValue % 2 > 0) lstPrivileges.Add("Delete");
                        break;
                    case 2: if (intActionValue % 2 > 0) lstPrivileges.Add("Update");
                        break;
                    case 1: if (intActionValue % 2 > 0) lstPrivileges.Add("Add");
                        break;
                }

                intActionValue = intActionValue / 2;
                iLoop -= 1;
            }
            return lstPrivileges;
        }

        public String GetAttachmentType(string AType)
        {
            if ((AType == "pdf") || (AType == ".pdf"))
            {
                return "application/pdf";
            }
            else if (AType == ".docx")
            {
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            }
            else if (AType == ".doc")
            {
                return "application/msword";
            }
            else if ((AType == ".jpg") || (AType == ".jpeg"))
            {
                return "image/jpeg";
            }
            else if (AType == ".png")
            {
                return "image/png";
            }
            else if (AType == ".gif")
            {
                return "image/gif";
            }
            else if (AType == ".pptx")
            {
                return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
            }
            else
            {
                return "text/plain";
            }
        }
    }
}
