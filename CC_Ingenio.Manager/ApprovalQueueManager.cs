using CC_Ingenio.Common.Utils;
using CC_Ingenio.Core.ApprovalRepo.ApprovalAggregate;
using CC_Ingenio.DataTranfer.ApprovalQueueInfo;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.Repository.AppovalRepo;
using CC_Ingenio.Repository.LookUpRepo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CC_Ingenio.Manager
{
    public class ApprovalQueueManager : IApprovalQueueManager
    {
        #region Variables

        ApprovalQueueRepository objApprovalQueueRepository;
        ApprovalQueueContextRepository objApprovalQueueContextRepository;

        #endregion

        #region Constructor
        public ApprovalQueueManager(ApprovalQueueRepository AApprovalQueueRepository, ApprovalQueueContextRepository AApprovalQueueContextRepository)
        {
            if (AApprovalQueueRepository == null)
                throw new ArgumentNullException("AApprovalQueueRepository");
            if (AApprovalQueueRepository == null)
                throw new ArgumentNullException("AApprovalQueueContextRepository");

            objApprovalQueueRepository = AApprovalQueueRepository;
            objApprovalQueueContextRepository = AApprovalQueueContextRepository;
        }
        #endregion

        #region Public Methods

        public string AuthorizeItems(ApprovalQueueDTO AApprovalQueueDTO, string ATableContext)
        {
            if (AApprovalQueueDTO == null || String.IsNullOrWhiteSpace(ATableContext))
                throw new ArgumentException("Messages.warning_CannotAddApprovalQueueDetailsWithNullInformation");
            string QueueRefNo = "";
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    SqlParameter[] paramCollection = new SqlParameter[]
                    { 
                        new SqlParameter(parameterName: "@P_RequestId",dbType:System.Data.SqlDbType.VarChar ),
                        new SqlParameter(parameterName: "@P_ReasonCode", dbType:System.Data.SqlDbType.VarChar ),
                        new SqlParameter(parameterName: "@P_ReasonDescription", dbType:System.Data.SqlDbType.VarChar ),
                        new SqlParameter(parameterName: "@P_ApprovedBy",dbType:System.Data.SqlDbType.VarChar ),
                        new SqlParameter(parameterName: "@P_ApprovalStatus",dbType:System.Data.SqlDbType.VarChar ),
                        new SqlParameter(parameterName: "@P_TableName",dbType:System.Data.SqlDbType.VarChar ),
                        new SqlParameter(){ParameterName = "@P_QueueRefNo", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Output, Size = 25}
                    };

                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_RequestId")).First()).Value = AApprovalQueueDTO.RequestId;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ReasonCode")).First()).Value = AApprovalQueueDTO.ContextCode;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ReasonDescription")).First()).Value = AApprovalQueueDTO.ApprovalDescription;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ApprovedBy")).First()).Value =  AApprovalQueueDTO.ApprovedBy;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ApprovalStatus")).First()).Value = AApprovalQueueDTO.ApprovalStatus;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TableName")).First()).Value = ATableContext;

                    objApprovalQueueRepository.UnitOfWork.ExecuteCommand("PR_CtApprovalQueueAuthorize @P_RequestId, @P_ReasonCode, @P_ReasonDescription, @P_ApprovedBy, @P_ApprovalStatus, @P_TableName, @P_QueueRefNo OUTPUT", paramCollection);
                    // objCurrencyRepository.UnitOfWork.ExecuteCommand(" exec PR_Ct_Approval_Status_Update {0}, {1}, {2}, {3}", "MT_Currency", "IsActive", "1");
                    QueueRefNo = paramCollection[6].Value.ToString();
                    scope.Complete();
                  
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return QueueRefNo;
            }
            //objApprovalQueueRepository.UnitOfWork.RefreshDataInModel(objApprovalQueueRepository.Get(AApprovalQueueDTO.RequestId));
            
        }

        public string AuthorizeItemsAll(string AContextCode, string AReasonCode, string AReasonDescription, string AApprovedBy, string AApprovalStatus, DataTable ARequestIds)
        {
            if (ARequestIds == null || String.IsNullOrWhiteSpace(AReasonCode))
                throw new ArgumentException("Messages.warning_CannotAddApprovalQueueDetailsAllWithNullInformation");
            string QueueRefNo = "";
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {

                    SqlParameter[] parameters = new SqlParameter[] { 
                        new SqlParameter(){ParameterName = "@P_ReasonCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = AReasonCode },
                        new SqlParameter(){ParameterName = "@P_ReasonDescription", SqlDbType = System.Data.SqlDbType.VarChar, Value = AReasonDescription },
                        new SqlParameter(){ParameterName = "@P_ContextCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = AContextCode },
                        new SqlParameter(){ParameterName = "@P_ApprovedBy", SqlDbType = System.Data.SqlDbType.VarChar, Value = AApprovedBy },
                        new SqlParameter(){ParameterName = "@P_ApprovalStatus", SqlDbType = System.Data.SqlDbType.VarChar, Value = AApprovalStatus },
                        new SqlParameter(){ParameterName = "@P_BusinessDate", SqlDbType = System.Data.SqlDbType.DateTime, Value = Utils.BusinessDate },
                            new SqlParameter(){ 
                                ParameterName="@P_ReqIDs", SqlDbType= SqlDbType.Structured, Value = ARequestIds, 
                                TypeName = "UD_ApproveAll", Direction = ParameterDirection.Input}
                    };
                    objApprovalQueueRepository.UnitOfWork.ExecuteCommand("PR_CtApprovalQueueAuthorizeAll @P_ReasonCode, @P_ReasonDescription, @P_ContextCode, @P_ApprovedBy, @P_ApprovalStatus, @P_BusinessDate, @P_ReqIDs", parameters);
           
                    scope.Complete();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return QueueRefNo;
            }
            //objApprovalQueueRepository.UnitOfWork.RefreshDataInModel(objApprovalQueueRepository.Get(AApprovalQueueDTO.RequestId));

        }

        public string GetEntityFromContext(string AContextCode)
        {
            return objApprovalQueueContextRepository.GetAll().Where(x => x.ContextCode == AContextCode).Select(x => x.EntityCode).FirstOrDefault();
        }
        #endregion

        #region Implemented methods
        public List<ApprovalQueueDTO> GetPendingItems(int APageNo, ApprovalQueueDTO AApprovalQueueFilter)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "ContextCode", FilterValue = AApprovalQueueFilter.ContextCode, Operator = "Equals"},
                new Utils.ExpressionList(){FilterColumn = "RequestedBy", FilterValue = AApprovalQueueFilter.RequestedBy, Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "ApprovalStatus", FilterValue = "P", Operator = "Equals"},
                new Utils.ExpressionList(){FilterColumn = "RequestedOn", FilterValue = AApprovalQueueFilter.RequestedOn, Operator = "GreaterThanOrEqualTo"},
                new Utils.ExpressionList(){FilterColumn = "RequestedOn", FilterValue = AApprovalQueueFilter.RequestedOnTo.AddDays(1), Operator = "LessThanOrEqualTo"},
                  new Utils.ExpressionList(){FilterColumn = "OrganizationCode", FilterValue = AApprovalQueueFilter.OrganizationCode, Operator = "Contains"}
            };

            Expression<Func<ApprovalQueue, bool>> expression = Utils.GetExpression<ApprovalQueue>(lstExp);
            int iRecordCount = objApprovalQueueRepository.GetRecordCount(expression);
                        
            //var vApprovalQueueDTO = objApprovalQueueRepository.GetFiltered(APageNo, Utils.GRD_PAGE_SIZE, expression);

            var vApprovalQueueDTO = objApprovalQueueRepository.GetFiltered(APageNo, iRecordCount, expression);

            if (vApprovalQueueDTO != null && vApprovalQueueDTO.Any())
            {
                List<ApprovalQueueDTO> lstApprovalQueueDTO = new List<ApprovalQueueDTO>();
                foreach (var currency in vApprovalQueueDTO)
                {
                    lstApprovalQueueDTO.Add(Conversion.Mapping.ApprovalQueueToApprovalQueueDTO(currency));
                }
                return lstApprovalQueueDTO;
            }
            else
                return new List<ApprovalQueueDTO>();

        }

        public int GetPendingItemsRecordCount(ApprovalQueueDTO AApprovalQueueFilter)
        {
            int iRetValue = 0;
            try
            {
                List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "ContextCode", FilterValue = AApprovalQueueFilter.ContextCode, Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "RequestedBy", FilterValue = AApprovalQueueFilter.RequestedBy, Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "ApprovalStatus", FilterValue = "P", Operator = "Equals"},
                new Utils.ExpressionList(){FilterColumn = "RequestedOn", FilterValue = AApprovalQueueFilter.RequestedOn, Operator = "GreaterThanOrEqualTo"},
                new Utils.ExpressionList(){FilterColumn = "RequestedOn", FilterValue = AApprovalQueueFilter.RequestedOnTo.AddDays(1), Operator = "LessThanOrEqualTo"}
             
            };

                Expression<Func<ApprovalQueue, bool>> expression = Utils.GetExpression<ApprovalQueue>(lstExp);

                iRetValue = objApprovalQueueRepository.GetRecordCount(expression);
            }
            catch(Exception ex)
            {
                throw ex;

            }
            return iRetValue;

        }

        #endregion
    }
}
