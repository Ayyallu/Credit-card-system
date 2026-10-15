using CC_Ingenio.Common.Utils;
using CC_Ingenio.Core.MasterRepo.MasterAggregate;
using CC_Ingenio.Core.UserRepo.UserAggregate;
using CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate;
using CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate;
using CC_Ingenio.Core.LookUpRepo;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.Repository.MasterRepo;
using CC_Ingenio.Repository.UserRepo;
using CC_Ingenio.Repository.CardBookingRepo;
using CC_Ingenio.Repository.LookUpRepo;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.DataTranfer.AuthorizationInfo ;
using CC_Ingenio.Repository.AuthorizationRepo;
using System.Transactions;
using System.Configuration;


namespace CC_Ingenio.Manager
{
    public class AuthorizationManager : IAuthorizationManager 
    {

        #region Variables

        CustomerRepository objCustomerRepository;
        //UserAllowedBranchRepository objUserAllowedBranchRepository;
        AuthorizationRepository objAuthorizationRepository;
        ManualTransactionRepository objManualTransactionRepository;
        BillingCycleViewRepository objBillingCycleViewRepository;
        AdjustmentRepository objAdjustmentRepository;
        
        CardMasterRepository objCardMasterRepository;
        CityRepository objCityRepository;
        CreditSegmentRepository objCreditSegmentRepository;

        EMITransactionInstallmentRepository objEMITransactionRepository;
        EMIAuthorizationRepository objEMIAuthorizationRepository;
        EMITransactionRepository objEMITranRepository;

        AuthorizationCancellationRepository objAuthorizationCancellationRepository;
        SystemParameterRepository objSystemParameterRepository;
        MerchantsRepository objMerchantMasterRepository;
        #endregion

        #region Constructor
        public AuthorizationManager(AuthorizationRepository AAuthorizationRepository,
            ManualTransactionRepository AManualTransactionRepository, BillingCycleViewRepository ABillingCycleViewRepository,
            AdjustmentRepository AAdjustmentRepository, CardMasterRepository ACardMasterRepository, CityRepository ACityRepository, 
            CreditSegmentRepository ACreditSegmentRepository, CustomerRepository ACustomerRepository, 
            EMITransactionInstallmentRepository AEMITransactionInstallmentRepository,EMIAuthorizationRepository AEMIAuthorizationRepository,
            AuthorizationCancellationRepository AAuthorizationCancellationRepository,EMITransactionRepository AEMITransactionRepository, 
            SystemParameterRepository ASystemparameterRepository,MerchantsRepository AMerchantMasterRepository)
        {
            if (AAuthorizationRepository == null)
                throw new ArgumentNullException("AAuthorizationRepository");
            if (AManualTransactionRepository == null)
                throw new ArgumentNullException("AManualTransactionRepository");
            if (ABillingCycleViewRepository == null)
                throw new ArgumentNullException("ABillingCycleViewRepository");
            if (AAdjustmentRepository == null)
                throw new ArgumentNullException("AAdjustmentRepository");
            if (ACardMasterRepository == null)
                throw new ArgumentNullException("ACardMasterRepository");
            if (ACityRepository == null)
                throw new ArgumentNullException("ACityRepository");
            if (ACreditSegmentRepository == null)
                throw new ArgumentNullException("ACreditSegmentRepository");
            if (ACustomerRepository == null)
                throw new ArgumentNullException("ACustomerRepository");
            if (AEMIAuthorizationRepository == null)
                throw new ArgumentNullException("AEMIAuthorizationRepository");
            if (AEMITransactionInstallmentRepository == null)
                throw new ArgumentNullException("AEMITransactionInstallmentRepository");
            if (AAuthorizationCancellationRepository == null)
                throw new ArgumentNullException("AAuthorizationCancellationRepository");
            if (AEMITransactionRepository == null)
                throw new ArgumentNullException("AEMITransactionRepository");
            if (ASystemparameterRepository == null)
                throw new ArgumentNullException("ASystemparameterRepository");
            if (AMerchantMasterRepository == null)
                throw new ArgumentNullException("AMerchantMasterRepository");

            objAuthorizationRepository = AAuthorizationRepository;
            objManualTransactionRepository = AManualTransactionRepository;
            objBillingCycleViewRepository = ABillingCycleViewRepository;
            objAdjustmentRepository = AAdjustmentRepository;

            objCardMasterRepository = ACardMasterRepository;
            objCityRepository = ACityRepository;
            objCreditSegmentRepository = ACreditSegmentRepository;
            objCustomerRepository = ACustomerRepository;

            objAuthorizationCancellationRepository = AAuthorizationCancellationRepository;
            objEMIAuthorizationRepository = AEMIAuthorizationRepository;
            objEMITransactionRepository = AEMITransactionInstallmentRepository;
            objEMITranRepository = AEMITransactionRepository;
            objSystemParameterRepository = ASystemparameterRepository;
            objMerchantMasterRepository = AMerchantMasterRepository;
        }
        #endregion

        #region CardEmbossing

        public string[] ForceAuthorizationData(List<ForceAuthorizationDTO> AlstData,string user)
        {
            string[] retValues = new string[7];
            DataTable dtType = new DataTable();
            dtType.Columns.Add("IsPosted");
            dtType.Columns.Add("TransactionNo");
            dtType.Columns.Add("LoginName");

            foreach (var item in AlstData)
            {
                if (item.IsChecked )
                {
                  
                    DataRow drCrType = dtType.NewRow();
                    drCrType["IsPosted"] = 1;
                    drCrType["TransactionNo"] = item.AuthCode;
                    drCrType["LoginName"] = user;
                    dtType.Rows.Add(drCrType);
                  
                  
                }

            }

               
                SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter(){ ParameterName="@P_CardNumber", SqlDbType= SqlDbType.Structured, Value = dtType, TypeName = "UD_Transactionupdate", Direction = ParameterDirection.Input}
               
            };
                objAuthorizationRepository.UnitOfWork.ExecuteCommand("PR_UpdateTransaction @P_CardNumber", parameters);
           

              return retValues;
        }

        #region Implemented methods
        public List<ForceAuthorizationDTO> GetPendingAuthorizationItems(int APageNo, ForceAuthorizationDTO AForceAuthorizationDTO, List<ForceAuthorizationDTO> AList,string AType)
        {
           
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>();
            if (AType != null && AType != "")
            {
                 lstExp = new List<Utils.ExpressionList>()
                 {
                     new Utils.ExpressionList(){FilterColumn = "AccountNo", FilterValue = AForceAuthorizationDTO.AccountNo, Operator = "Contains"},
                     new Utils.ExpressionList(){FilterColumn = "BatchNo", FilterValue = AForceAuthorizationDTO.BatchNo, Operator = "Contains"},
                     new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = AForceAuthorizationDTO.CardNumber, Operator = "Contains"},               
                     new Utils.ExpressionList(){FilterColumn = "IsManualEntry", FilterValue = AForceAuthorizationDTO.IsManualEntry , Operator = "Equals"},
                     new Utils.ExpressionList(){FilterColumn = "IsPosted", FilterValue = false , Operator = "Equals"},
                    new Utils.ExpressionList(){FilterColumn = "OrganizationCode", FilterValue = Utils.OrganizationCode  , Operator = "Equals"},
                 };
            }
            else
            {
                 lstExp = new List<Utils.ExpressionList>()
                 {
                    new Utils.ExpressionList(){FilterColumn = "AccountNo", FilterValue = AForceAuthorizationDTO.AccountNo, Operator = "Contains"},
                    new Utils.ExpressionList(){FilterColumn = "BatchNo", FilterValue = AForceAuthorizationDTO.BatchNo, Operator = "Contains"},
                    new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = AForceAuthorizationDTO.CardNumber, Operator = "Contains"},               
                    new Utils.ExpressionList(){FilterColumn = "IsPosted", FilterValue = false , Operator = "Equals"},   
                    new Utils.ExpressionList(){FilterColumn = "OrganizationCode", FilterValue = Utils.OrganizationCode , Operator = "Equals"},
                 };
            }

            Expression<Func<ForceAuthorization, bool>> expression = Utils.GetExpression<ForceAuthorization>(lstExp);

            int iRecordCount = objAuthorizationRepository.GetRecordCount(expression);
            var vApprovalQueueDTO = objAuthorizationRepository.GetFiltered(APageNo, iRecordCount, expression);
            
            if (vApprovalQueueDTO != null && vApprovalQueueDTO.Any())
            {
                List<ForceAuthorizationDTO> lstForceAuthorizationDTO;
                if (AList == null)
                {
                    lstForceAuthorizationDTO = new List<ForceAuthorizationDTO>();

                    foreach (var variable in vApprovalQueueDTO)
                    {
                        lstForceAuthorizationDTO.Add(Conversion.Mapping.ForceAuthorizationToForceAuthorizationDTO(variable));
                    }
                }

                else
                    lstForceAuthorizationDTO = AList;

                return lstForceAuthorizationDTO;
            }
            else
                return new List<ForceAuthorizationDTO>();

        }

        public int GetPendingAuthorizationItemsRecordCount(ForceAuthorizationDTO AForceAuthorizationDTO,string AType)
        {
            int iRetValue = 0;
            try
            {

                if (AType != null && AType != "")
                {
                    List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                 new Utils.ExpressionList(){FilterColumn = "AccountNo", FilterValue = AForceAuthorizationDTO.AccountNo, Operator = "Contains"},
                 new Utils.ExpressionList(){FilterColumn = "BatchNo", FilterValue = AForceAuthorizationDTO.BatchNo, Operator = "Contains"},
                 new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = AForceAuthorizationDTO.CardNumber, Operator = "Contains"},               
                 new Utils.ExpressionList(){FilterColumn = "IsManualEntry", FilterValue = AForceAuthorizationDTO.IsManualEntry , Operator = "Equals"},
             
             

            };
                    Expression<Func<ForceAuthorization, bool>> expression = Utils.GetExpression<ForceAuthorization>(lstExp);
                    iRetValue = objAuthorizationRepository.GetRecordCount(expression);
                }
                else
                {
                    List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                    new Utils.ExpressionList(){FilterColumn = "AccountNo", FilterValue = AForceAuthorizationDTO.AccountNo, Operator = "Contains"},
                    new Utils.ExpressionList(){FilterColumn = "BatchNo", FilterValue = AForceAuthorizationDTO.BatchNo, Operator = "Contains"},
                    new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = AForceAuthorizationDTO.CardNumber, Operator = "Contains"},               
                                
             
            };
                    Expression<Func<ForceAuthorization, bool>> expression = Utils.GetExpression<ForceAuthorization>(lstExp);
                    iRetValue = objAuthorizationRepository.GetRecordCount(expression);
                }



              

                
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return iRetValue;

        }

        #endregion
                     
        #endregion 

        #region ManualTransaction

        #region SaveManualTransactionDetails
        public void SaveManualTransactionDetails(ManualTransactionDTO AManualTransactionDTO)
        {
            if (AManualTransactionDTO == null)
            {
                throw new ArgumentException("");
            }

            var newManualTransaction = ManualTransactionFactory.CreateManualTransaction(AManualTransactionDTO.BatchNo,
                AManualTransactionDTO.CardNumber, AManualTransactionDTO.RUT, AManualTransactionDTO.TxnDate, AManualTransactionDTO.Time,
                AManualTransactionDTO.AuthCode, AManualTransactionDTO.OrganizationCode, AManualTransactionDTO.MerchantCode, AManualTransactionDTO.IncomingTransType, AManualTransactionDTO.MCCCode,
                AManualTransactionDTO.SegmentCode, AManualTransactionDTO.TransType, AManualTransactionDTO.Amount, AManualTransactionDTO.IsManualEntry,
                AManualTransactionDTO.PostedDate, AManualTransactionDTO.IsPosted, 
                AManualTransactionDTO.AuthCancellationStatus,AManualTransactionDTO.ReferenceAuthCode,AManualTransactionDTO.AuthCancellationRemarks,
                AManualTransactionDTO.CreatedBy, AManualTransactionDTO.LastUpdatedBy, AManualTransactionDTO.ApprovalStatus,AManualTransactionDTO .InvoiceNo ,
                AManualTransactionDTO .OrderNo ,AManualTransactionDTO .SalesId , AManualTransactionDTO .Remarks ,
                AManualTransactionDTO .CreditNote ,AManualTransactionDTO .BalanceAmount ,null);

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = System .Transactions . IsolationLevel.ReadUncommitted });
            using (scope)
            {
               
                    SqlParameter[] paramCollection = new SqlParameter[]{ 
                    new SqlParameter(parameterName: "@P_AuthCode",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_CardNumber", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_MerchantCode", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_SegmentCode", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_MCCCode",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_Amount",dbType:System.Data.SqlDbType.Decimal ),
                    new SqlParameter(parameterName: "@P_IncomingTransTypeId",dbType:System.Data.SqlDbType.Int ),
                    new SqlParameter(parameterName: "@p_TxnDate",dbType:System.Data.SqlDbType.DateTime2 ),
                    new SqlParameter(parameterName: "@P_TxnTime",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@p_TxnType",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_ApprovalStatus",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_CreatedBy",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(){ ParameterName = "@P_AuthCancellationStatus", Value = (object)AManualTransactionDTO.AuthCancellationStatus ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_ReferenceAuthCode", Value = (object)AManualTransactionDTO.ReferenceAuthCode ?? DBNull.Value ,SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_AuthCancellationRemarks", Value = (object)AManualTransactionDTO.AuthCancellationRemarks ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_OrganizationCode", Value = (object)AManualTransactionDTO.OrganizationCode ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                     new SqlParameter(){ ParameterName = "@P_InvoiceNo", Value = (object)AManualTransactionDTO.InvoiceNo ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_SalesId", Value = (object)AManualTransactionDTO.SalesId ?? DBNull.Value ,SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_OrderNo", Value = (object)AManualTransactionDTO.OrderNo ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_Remarks", Value = (object)AManualTransactionDTO.Remarks ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                     new SqlParameter(){ ParameterName = "@P_CreditNote", Value = (object)AManualTransactionDTO.CreditNote ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_BalanceAmount", Value = (object)AManualTransactionDTO.BalanceAmount ?? 0, SqlDbType = SqlDbType.Decimal },

                };

                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_AuthCode")).First()).Value = AManualTransactionDTO.AuthCode == null ? "0" : AManualTransactionDTO.AuthCode;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CardNumber")).First()).Value = AManualTransactionDTO.CardNumber;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_MerchantCode")).First()).Value = AManualTransactionDTO.MerchantCode == null ? "" : AManualTransactionDTO.MerchantCode;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_SegmentCode")).First()).Value = AManualTransactionDTO.SegmentCode == null ? "" : AManualTransactionDTO.SegmentCode;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_MCCCode")).First()).Value = AManualTransactionDTO.MCCCode == null ? "" : AManualTransactionDTO.MCCCode;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Amount")).First()).Value = AManualTransactionDTO.Amount;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_IncomingTransTypeId")).First()).Value = AManualTransactionDTO.IncomingTransType;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@p_TxnDate")).First()).Value = AManualTransactionDTO.TxnDate;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TxnTime")).First()).Value = AManualTransactionDTO.Time == null ? "" : AManualTransactionDTO.Time;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@p_TxnType")).First()).Value = AManualTransactionDTO.TransType;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ApprovalStatus")).First()).Value = AManualTransactionDTO.ApprovalStatus;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ReferenceAuthCode")).First()).Value = AManualTransactionDTO.ReferenceAuthCode == null ? "" : AManualTransactionDTO.ReferenceAuthCode;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CreatedBy")).First()).Value = AManualTransactionDTO.CreatedBy;
                    
                    objManualTransactionRepository.UnitOfWork.ExecuteCommand(@"PR_ManualTransactionSave @P_AuthCode, @P_CardNumber, 
                        @P_MerchantCode, @P_SegmentCode, @P_MCCCode, @P_Amount, @P_IncomingTransTypeId, @p_TxnDate,@P_TxnTime, @p_TxnType, @P_ApprovalStatus, 
                        @P_CreatedBy, @P_AuthCancellationStatus,@P_ReferenceAuthCode,@P_AuthCancellationRemarks,@P_OrganizationCode,@P_InvoiceNo,@P_SalesId,@P_OrderNo,@P_Remarks,@P_CreditNote,@P_BalanceAmount", paramCollection);
                    scope.Complete();
            }
        }
        #endregion

        #region UpdateManualTransactionDetails
        public void UpdateManualTransactionDetails(string AAuthCode, ManualTransactionDTO AManualTransactionDTO)
        {
            if (AManualTransactionDTO == null)
            {
                throw new ArgumentException("");
            }

            ManualTransaction currentManualTransaction = objManualTransactionRepository.Get(AAuthCode);
            ManualTransaction updatedManualTransaction = new ManualTransaction()
            {
                BatchNo = AManualTransactionDTO.BatchNo,
                CardNumber = AManualTransactionDTO.CardNumber,
                RUT = AManualTransactionDTO.RUT,
                TxnDate = AManualTransactionDTO.TxnDate,
                AuthCode = AAuthCode,
                OrganizationCode = AManualTransactionDTO.OrganizationCode,
                IncomingTransType = AManualTransactionDTO.IncomingTransType,
                MerchantCode = AManualTransactionDTO.MerchantCode,
                MCCCode = AManualTransactionDTO.MCCCode,
                SegmentCode = AManualTransactionDTO.SegmentCode,
                TransType = AManualTransactionDTO.TransType,
                Amount = AManualTransactionDTO.Amount,
                IsManualEntry = AManualTransactionDTO.IsManualEntry,
                PostedDate = AManualTransactionDTO.PostedDate,
                IsPosted = AManualTransactionDTO.IsPosted,
                ApprovalStatus = AManualTransactionDTO.ApprovalStatus,
                CreatedBy = currentManualTransaction.CreatedBy,
                CreatedOn = currentManualTransaction.CreatedOn,
                LastUpdatedBy = AManualTransactionDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
               new TransactionOptions() { IsolationLevel = System .Transactions .IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objManualTransactionRepository.Merge(currentManualTransaction, updatedManualTransaction);
                    objManualTransactionRepository.UnitOfWork.Commit();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region DeleteManualTransaction
        public void DeleteManualTransaction(string No)
        {
            var manualTransaction = objManualTransactionRepository.Get(No);
            if (manualTransaction != null)  
            {
                objManualTransactionRepository.Remove(manualTransaction);
                objManualTransactionRepository.UnitOfWork.Commit();
            }
        }
        #endregion

        #region FindManualTransaction
        public List<ManualTransactionDTO> FindManualTransaction(int APageIndex, int APageCount, int Amode)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { 
                      new Utils.ExpressionList() 
                            { FilterColumn = "IsManualEntry",FilterValue = true, Operator = "Equals" },
                     new Utils.ExpressionList() 
                            { FilterColumn = "IsBilled",FilterValue = false, Operator = "Equals" },
                     new Utils.ExpressionList() 
                            { FilterColumn = "CreditNote",FilterValue = "" , Operator =( Amode == 0?"Equals":"NotEquals") },                           
                      new Utils.ExpressionList() 
                            { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode, Operator = "Equals" },
                    };

            Expression<Func<ManualTransaction, bool>> expression = Utils.GetExpression<ManualTransaction>(lstExpression);
            int iRecordCount = objManualTransactionRepository.GetRecordCount(expression);

            var manualTransaction = objManualTransactionRepository.GetFiltered(0, iRecordCount, expression);
            if (manualTransaction != null && manualTransaction.Any())
            {
                List<ManualTransactionDTO> lstManualTransactionDTO = new List<ManualTransactionDTO>();
                foreach (var item in manualTransaction)
                {
                    lstManualTransactionDTO.Add(Conversion.Mapping.ManualTransactionToManualTransactionDTO(item));
                }
                return lstManualTransactionDTO;
            }
            else
                return new List<ManualTransactionDTO>();
        }
        #endregion

        #region FindAll
        public List<ManualTransactionDTO> FindAll(int APageNo, ManualTransactionDTO AManualTransactionDTO, int Amode)
        {
            
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { 
                        new Utils.ExpressionList() 
                            { FilterColumn = "AuthCode",FilterValue = AManualTransactionDTO.AuthCode, Operator = "Contains" },
                          new Utils.ExpressionList() 
                            { FilterColumn = "InvoiceNo",FilterValue = AManualTransactionDTO.InvoiceNo, Operator = "Contains" },
                             new Utils.ExpressionList() 
                            { FilterColumn = "CreditNote",FilterValue = AManualTransactionDTO.CreditNote, Operator = "Contains" },
                        new Utils.ExpressionList() 
                            { FilterColumn = "RUT",FilterValue = AManualTransactionDTO.RUT, Operator = "Contains" },
                        new Utils.ExpressionList() 
                            { FilterColumn = "CardNumber",FilterValue = AManualTransactionDTO.CardNumber, Operator = "Contains" },
                        new Utils.ExpressionList() 
                            { FilterColumn = "MerchantCode",FilterValue = AManualTransactionDTO.MerchantCode, Operator = "Contains" },
                        new Utils.ExpressionList() 
                            { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode, Operator = "Contains" },
                        new Utils.ExpressionList() 
                            { FilterColumn = "CreditNote",FilterValue = "" , Operator = (Amode == 0?"Equals":"NotEquals") },
                        new Utils.ExpressionList() 
                            { FilterColumn = "IsBilled",FilterValue = false , Operator = "Equals" },
                           
                    };

            Expression<Func<ManualTransaction, bool>> expression = Utils.GetExpression<ManualTransaction>(lstExpression);
            int iRecordCount = objManualTransactionRepository.GetRecordCount(expression);


            var vManualTransactionDTO = objManualTransactionRepository.GetFiltered(0, iRecordCount, expression);

            if (vManualTransactionDTO != null && vManualTransactionDTO.Any())
            {
                List<ManualTransactionDTO> lstManualTransactionDTO = new List<ManualTransactionDTO>();
                foreach (var item in vManualTransactionDTO)
                {
                    lstManualTransactionDTO.Add(Conversion.Mapping.ManualTransactionToManualTransactionDTO(item));
                }
                return lstManualTransactionDTO;
            }
            else
                return new List<ManualTransactionDTO>();
        }
        #endregion

        #region FindByTransactionNo
        public ManualTransactionDTO FindByTransactionNo(string AAuthCode,string ATransactionCode,string AtransactionSubcode, DateTime? APostedDate, bool AApprover)
        {
            ManualTransaction manualTransaction = objManualTransactionRepository.Get(AAuthCode, ATransactionCode, AtransactionSubcode, APostedDate);
            if (manualTransaction != null)
            {
                if (manualTransaction.objSystemParameter == null)
                {
                    manualTransaction.objSystemParameter = objSystemParameterRepository.Get(manualTransaction.IncomingTransType);
                }
                if (manualTransaction.objMerchants == null)
                {
                    manualTransaction.objMerchants = objMerchantMasterRepository.Get(manualTransaction.MerchantCode, Utils.OrganizationCode);
                }
                return Conversion.Mapping.ManualTransactionToManualTransactionDTO(manualTransaction);
            }
            else
            {
                return new ManualTransactionDTO();
            }
        }
        #endregion

        #region FindByTransactionNo
        public ReversalTransactionDTO FindByReversalTransactionNo(string AAuthCode, string ATransactionCode, string ATransactionSubcode, DateTime? APostedDate, bool AApprover)
        {
            ManualTransaction manualTransaction = objManualTransactionRepository.Get(AAuthCode, ATransactionCode,ATransactionSubcode, APostedDate);
            if (manualTransaction != null)
            {
                if (manualTransaction.objSystemParameter == null)
                {
                    manualTransaction.objSystemParameter = objSystemParameterRepository.Get(manualTransaction.IncomingTransType);
                }
                if (manualTransaction.objMerchants == null)
                {
                    manualTransaction.objMerchants = objMerchantMasterRepository.Get(manualTransaction.MerchantCode, Utils.OrganizationCode);
                }
                return Conversion.Mapping.ReversalTransactionToReversalTransactionDTO(manualTransaction);
            }
            else
            {
                return new ReversalTransactionDTO();
            }
        }
        #endregion

        #region FindByTransactionNo
        public ManualTransactionDTO FindByInvoiceNo(string CardNumber, string InvoiceNo, string AMode)
        {

            ManualTransactionDTO objManualTransactionDTO = new ManualTransactionDTO();
            try
            {
                List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                { 
                    new Utils.ExpressionList() { FilterColumn = "CardNumber",FilterValue = CardNumber , Operator = "Equal" }     ,
                    new Utils.ExpressionList() { FilterColumn = "InvoiceNo",FilterValue = InvoiceNo, Operator = "Equal" }   ,
                     new Utils.ExpressionList() { FilterColumn = "TransType",FilterValue = AMode, Operator = "Equal" }   
                };

                Expression<Func<ManualTransaction, bool>> expression = Utils.GetExpression<ManualTransaction>(lstExpression);


                var vTransactionDTO = objManualTransactionRepository .GetFiltered(0, 1, expression);
                if (vTransactionDTO.Count() != 0)
                {
                    objManualTransactionDTO = Conversion.Mapping.ManualTransactionToManualTransactionDTO(vTransactionDTO.SingleOrDefault());
                }
                else
                {
                    
                }

                return objManualTransactionDTO;

            }
            catch (Exception ex)
            {

                throw ex;
              
            }


        }
        #endregion

        #endregion

        #region AuthorizationLog
        #region FindAuthorizationLog
        public List<ManualTransactionDTO> FindAuthorizationLog(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objManualTransactionRepository.GetRecordCount();
            var manualTransaction = objManualTransactionRepository.GetAll();
            if (manualTransaction != null && manualTransaction.Any())
            {
                List<ManualTransactionDTO> lstManualTransactionDTO = new List<ManualTransactionDTO>();
                foreach (var item in manualTransaction)
                {
                    lstManualTransactionDTO.Add(Conversion.Mapping.ManualTransactionToManualTransactionDTO(item));
                }
                return lstManualTransactionDTO;
            }
            else
                return new List<ManualTransactionDTO>();
        }
        #endregion

        #region FindAllAuthorizationLog
        public List<ManualTransactionDTO> FindAllAuthorizationLog(int APageNo, ManualTransactionDTO AManualTransactionDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { 
                        new Utils.ExpressionList() { FilterColumn = "AuthCode",FilterValue = AManualTransactionDTO.AuthCode, Operator = "Contains" },
                        new Utils.ExpressionList() { FilterColumn = "AccountNo",FilterValue = AManualTransactionDTO.AccountNo, Operator = "Contains" },
                        new Utils.ExpressionList() { FilterColumn = "CardNumber",FilterValue = AManualTransactionDTO.CardNumber, Operator = "Contains" },
                        new Utils.ExpressionList() { FilterColumn = "Amount",FilterValue = AManualTransactionDTO.Amount, Operator = "Equals" },
                        new Utils.ExpressionList() { FilterColumn = "Time",FilterValue = AManualTransactionDTO.Time, Operator = "GreaterThanOrEqualTo" },
                       new Utils.ExpressionList() { FilterColumn = "TxnDate",FilterValue = AManualTransactionDTO.TxnDate, Operator = "GreaterThanOrEqualTo" },
                      
                    };

            Expression<Func<ManualTransaction, bool>> expression = Utils.GetExpression<ManualTransaction>(lstExpression);
            int iRecordCount = objManualTransactionRepository.GetRecordCount(expression);




            var vManualTransactionDTO = objManualTransactionRepository.GetFiltered(0, iRecordCount, expression);

            if (vManualTransactionDTO != null && vManualTransactionDTO.Any())
            {
                List<ManualTransactionDTO> lstManualTransactionDTO = new List<ManualTransactionDTO>();
                foreach (var item in vManualTransactionDTO)
                {
                    lstManualTransactionDTO.Add(Conversion.Mapping.ManualTransactionToManualTransactionDTO(item));
                }
                return lstManualTransactionDTO;
            }
            else
                return new List<ManualTransactionDTO>();

        }
        #endregion
        #endregion

        #region EMITransaction

        #region SaveManualTransactionDetails

        public EMIAuthorizationDTO  SaveEMITransactionDetails(EMITransactionDTO AManualTransactionDTO)
        {
            EMIAuthorizationDTO mccCodeList = new EMIAuthorizationDTO();
            if (AManualTransactionDTO == null)
            {
                throw new ArgumentException("");
            }


   
                try
                {


                    SqlParameter[] paramCollection = new SqlParameter[]{ 
                    new SqlParameter(parameterName: "@P_EMIAuthNo",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_CardNumber", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_MerchantCode", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_PlanID",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_TotalInstallment",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_LoanAmount",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@p_LoanBookedDate",dbType:System.Data.SqlDbType.DateTime ),
                    new SqlParameter(parameterName: "@P_CreatedBy",dbType:System.Data.SqlDbType.VarChar ),
                };

                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_EMIAuthNo")).First()).Value = AManualTransactionDTO.AuthCode == null ? "0" : AManualTransactionDTO.AuthCode;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CardNumber")).First()).Value = AManualTransactionDTO.CardNumber;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_MerchantCode")).First()).Value = AManualTransactionDTO.MerchantCode;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_PlanID")).First()).Value = AManualTransactionDTO.PlanId;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TotalInstallment")).First()).Value = AManualTransactionDTO.TotalInstallment;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_LoanAmount")).First()).Value = AManualTransactionDTO.LoanAmount;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@p_LoanBookedDate")).First()).Value = AManualTransactionDTO.LoanBookedDate;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CreatedBy")).First()).Value = AManualTransactionDTO.CreatedBy;


                 //   objManualTransactionRepository.UnitOfWork.ExecuteCommand("PR_EMITransactionSave @P_EMIAuthNo, @P_CardNumber, @P_MerchantCode, @P_PlanID, @P_TotalInstallment, @P_LoanAmount,@p_LoanBookedDate,  @P_CreatedBy", paramCollection);

                  
                      
                 EMIAuthorization    objEMIAuthorization = objEMIAuthorizationRepository
              .UnitOfWork.ExecuteQuery<EMIAuthorization>("PR_EMITransactionSave @P_EMIAuthNo, @P_CardNumber, @P_MerchantCode, @P_PlanID, @P_TotalInstallment, @P_LoanAmount,@p_LoanBookedDate,  @P_CreatedBy", paramCollection).FirstOrDefault();
                  
                    if (objManualTransactionRepository != null)
                    {
                        return Conversion.Mapping.EMIAuthorizationToEMIAuthorizationDTO(objEMIAuthorization); ;
                    }

                    return mccCodeList;
                   
                }
                catch (Exception ex)
                {
                    return mccCodeList;
                    throw ex;
                }
            
        }

        #endregion

        #region FindAll
        public List<EMITransactionDTO > FindAll(int APageNo, EMITransactionDTO  AManualTransactionDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { 
                        new Utils.ExpressionList() { FilterColumn = "EMIAuthNo",FilterValue = AManualTransactionDTO.AuthCode, Operator = "Contains" },
                      
                    };

            Expression<Func<EMITransaction, bool>> expression = Utils.GetExpression<EMITransaction>(lstExpression);
            int iRecordCount = objEMITranRepository .GetRecordCount(expression);



            var vManualTransactionDTO = objEMITranRepository.GetFiltered(0, iRecordCount, expression);

            if (vManualTransactionDTO != null && vManualTransactionDTO.Any())
            {
                List<EMITransactionDTO> lstManualTransactionDTO = new List<EMITransactionDTO>();
                foreach (var item in vManualTransactionDTO)
                {
                    lstManualTransactionDTO.Add(Conversion.Mapping.EMITransactionToEMITransactionDTO (item));
                }
                return lstManualTransactionDTO;
            }
            else
                return new List<EMITransactionDTO>();

        }

        public List<EMITransactionDTO> FindEMITransaction(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objEMITranRepository .GetRecordCount();
            var manualTransaction = objEMITranRepository.GetAll();
            if (manualTransaction != null && manualTransaction.Any())
            {
                List<EMITransactionDTO> lstManualTransactionDTO = new List<EMITransactionDTO>();
                foreach (var item in manualTransaction)
                {
                    lstManualTransactionDTO.Add(Conversion.Mapping.EMITransactionToEMITransactionDTO (item));
                }
                return lstManualTransactionDTO;
            }
            else
                return new List<EMITransactionDTO >();
        }
        #endregion
      
        #endregion
        
        #region BillingCycleView

        #region FindBillingCycleView
        public List<BillingCycleViewDTO> FindBillingCycleView(int APageIndex, int APageCount)
        {

            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objBillingCycleViewRepository.GetRecordCount();
            var BillingCycleView = objBillingCycleViewRepository.GetAll();
            return new List<BillingCycleViewDTO>();

        }
        #endregion

        #region FindAll
        public List<BillingCycleViewDTO> FindAll(int APageNo, BillingCycleViewDTO ABillingCycleViewDTO)
        {
            
            List<BillingCycleViewDTO> lstBillingCycleViewDTO = new List<BillingCycleViewDTO>();

            SqlParameter[] paramCollection = new SqlParameter[]
            { 
                new SqlParameter(){ ParameterName = "@ChargeSlipDate", Value = (object)ABillingCycleViewDTO.ChargeSlipDate ?? DBNull.Value, SqlDbType = SqlDbType.DateTime },
                new SqlParameter(){ ParameterName = "@CardNumber", Value = (object)ABillingCycleViewDTO.CardNumber ?? DBNull.Value ,SqlDbType = SqlDbType.VarChar }
            };

            lstBillingCycleViewDTO = objBillingCycleViewRepository.UnitOfWork.ExecuteQuery<BillingCycleViewDTO>("PR_GetTransactionsForAdjustment @ChargeSlipDate, @CardNumber", paramCollection).ToList();
            
            return lstBillingCycleViewDTO;
                        
        }
        #endregion

        #endregion

        #region Adjustment

        #region FindAdjustment
        public List<AdjustmentDTO> FindAdjustment(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objAdjustmentRepository.GetRecordCount();
            var Adjustment = objAdjustmentRepository.GetAll();
            if (Adjustment != null && Adjustment.Any())
            {
                List<AdjustmentDTO> lstAdjustmentDTO = new List<AdjustmentDTO>();
                foreach (var item in Adjustment)
                {
                    lstAdjustmentDTO.Add(Conversion.Mapping.AdjustmentToAdjustmentDTO(item));
                }
                return lstAdjustmentDTO;
            }
            else
                return new List<AdjustmentDTO>();
        }
        #endregion

        #region FindByAdjustmentCode
        public AdjustmentDTO FindByAdjustmentCode(string AAdjustmentCode)
        {
            Adjustment Adjustment = objAdjustmentRepository.Get(AAdjustmentCode);
            if (Adjustment != null)
            {
                return Conversion.Mapping.AdjustmentToAdjustmentDTO(Adjustment);
            }
            else
            {
                return new AdjustmentDTO();
            }
        }
        #endregion

        #region Save Adjustment Details
        public void SaveAdjustmentDetails(AdjustmentDTO AAdjustmentDTO)
        {
            if (AAdjustmentDTO == null)
            {
                throw new ArgumentException("");
            }

            var newAdjustment = AdjustmentFactory.CreateAdjustment(AAdjustmentDTO.AdjustmentCode, AAdjustmentDTO.AdjustmentNarration,
                AAdjustmentDTO.AuthCode, AAdjustmentDTO.CardNumber, 
                AAdjustmentDTO.ActualAmount,AAdjustmentDTO.AdjustedAmount, AAdjustmentDTO.CloseFlag,
                AAdjustmentDTO.CreatedBy, AAdjustmentDTO.LastUpdatedBy, "P", null);

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objAdjustmentRepository.Add(newAdjustment);

                    SqlParameter[] paramCollection = new SqlParameter[]{ 
                    new SqlParameter(parameterName: "@P_TableName",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_ColumnName", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_Value", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_WhereClause",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_ContextCode",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_LoginName",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_ReturnUrl",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_TransactionType",dbType:System.Data.SqlDbType.VarChar ),
                };

                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TableName")).First()).Value = "TR_Adjustment";
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ColumnName")).First()).Value = "ApprovalStatus";
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Value")).First()).Value = "P";
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_WhereClause")).First()).Value = "AdjustmentCode = '" + AAdjustmentDTO.AdjustmentCode + "'";
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ContextCode")).First()).Value = "ADJSTMNT";
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_LoginName")).First()).Value = AAdjustmentDTO.CreatedBy;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ReturnUrl")).First()).Value = "/Authorization/AdjustmentAuthorize/" + AAdjustmentDTO.AdjustmentCode + "/A?";
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TransactionType")).First()).Value = "TR";


                    objAdjustmentRepository.UnitOfWork.Commit();
                    objAdjustmentRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType", paramCollection);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #endregion

        #region Authorization Cancellation
        
        #region GetTransactionDetails

        public AuthorizationCancellationDTO GetTransactionDetails(string AAuthCode)
        {
            AuthorizationCancellationDTO objAuthorizationCancellationDTO = new AuthorizationCancellationDTO();

            AuthorizationCancellation objAuthorizationCancellation = objAuthorizationCancellationRepository
                .UnitOfWork.ExecuteQuery<AuthorizationCancellation>("PR_GetTransactionForAuthCancellation @P_AuthCode", new SqlParameter("P_AuthCode", AAuthCode)).ToList().FirstOrDefault();

            if (objAuthorizationCancellation != null)
            {
                return Conversion.Mapping.AuthorizationCancellationToAuthorizationCancellationDTO(objAuthorizationCancellation);
            }
            #region Old
            //List<ManualTransaction> objTransaction = objManualTransactionRepository.GetFiltered(0, 1, Utils.GetExpression<ManualTransaction>(new List<Utils.ExpressionList>() 
            //        { new Utils.ExpressionList() { FilterColumn = "AuthCode",FilterValue = AAuthCode, Operator = "Equals" }})).ToList();
            //List<CardMaster> objCardMaster = objCardMasterRepository.GetFiltered(0, 1, Utils.GetExpression<CardMaster>(new List<Utils.ExpressionList>(){
            //    new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = objTransaction.FirstOrDefault().CardNumber, Operator = "Equals"}})).ToList();

            //List<Customer> objCustomer = objCustomerRepository.GetFiltered(0, 1, Utils.GetExpression<Customer>(new List<Utils.ExpressionList>(){
            //    new Utils.ExpressionList(){FilterColumn = "CustomerId", FilterValue = objCardMaster.FirstOrDefault().CustomerId, Operator="Equals" }})).ToList();

            //List<Merchant> objMerchant = objMerchantRepository.GetFiltered(0, 1, Utils.GetExpression<Merchant>(new List<Utils.ExpressionList>() { 
            //    new Utils.ExpressionList(){FilterColumn = "MerchantCode", FilterValue = objTransaction.FirstOrDefault().MerchantCode, Operator = "Equals"}})).ToList();

            //var customerAddress = objCustomer.FirstOrDefault().CustomerAddress.Find(x => x.AddressTypeCode == objCardMaster.FirstOrDefault().AddressTypeCode);

            //List<City> objCity = objCityRepository.GetFiltered(0,1,Utils.GetExpression<City>(new List<Utils.ExpressionList>(){ 
            //    new Utils.ExpressionList(){ FilterColumn = "CityCode", FilterValue = customerAddress.CityCode, Operator="Equals" }})).ToList();

            //List<CreditSegment> objCreditSegment = objCreditSegmentRepository.GetFiltered(0,1,Utils.GetExpression<CreditSegment>(new List<Utils.ExpressionList>(){ 
            //    new Utils.ExpressionList(){ FilterColumn = "CardSegmentCode", FilterValue = objTransaction.FirstOrDefault().SegmentCode, Operator = "Equals" }})).ToList();

            //objAuthorizationCancellationDTO.MerchantCode = objMerchant.FirstOrDefault().MerchantCode;
            //objAuthorizationCancellationDTO.MerchantName = objMerchant.FirstOrDefault().MerchantName;
            //objAuthorizationCancellationDTO.CardNumber = objTransaction.FirstOrDefault().CardNumber;
            //objAuthorizationCancellationDTO.CardExpiryDate = objCardMaster.FirstOrDefault().CardExpiryDate;
            //objAuthorizationCancellationDTO.RUT = objCustomer.FirstOrDefault().RUT;
            //objAuthorizationCancellationDTO.CustomerName = objCustomer.FirstOrDefault().Name;

            //objAuthorizationCancellationDTO.Address = customerAddress.Block + customerAddress.Street + customerAddress.Town;
            //objAuthorizationCancellationDTO.City = objCity.FirstOrDefault().CityName;
            //objAuthorizationCancellationDTO.PostalCode = customerAddress.PostalCode;
            //objAuthorizationCancellationDTO.Phone = customerAddress.Phone;

            //objAuthorizationCancellationDTO.AuthCode = objTransaction.FirstOrDefault().AuthCode;
            //objAuthorizationCancellationDTO.Amount = objTransaction.FirstOrDefault().Amount;
            //objAuthorizationCancellationDTO.TransactionCode = objTransaction.FirstOrDefault().TransactionCode;
            //objAuthorizationCancellationDTO.TransactionDate = objTransaction.FirstOrDefault().TxnDate;
            //objAuthorizationCancellationDTO.TransactionType = objTransaction.FirstOrDefault().TransType;
            //objAuthorizationCancellationDTO.SegmentName = objCreditSegment.FirstOrDefault().CardSegmentName;
            //objAuthorizationCancellationDTO.ManualTransaction = Conversion.Mapping.ManualTransactionToManualTransactionDTO(objTransaction.FirstOrDefault());
            #endregion
            return objAuthorizationCancellationDTO;
        }
        #endregion

        #region EMIAuthorizationData

        public string[] EMIAuthorizationData(List<EMITransactionInstallmentDTO> AlstData, string user,int Type)
        {
            string[] retValues = new string[7];
            DataTable dtType = new DataTable();
            dtType.Columns.Add("IsPosted");
            dtType.Columns.Add("EMIAuthNo");
            dtType.Columns.Add("InstallmentNo");
            dtType.Columns.Add("TotalPrincipalAmount");
            dtType.Columns.Add("TotalInterestAmount");

            dtType.Columns.Add("LoginName");

            foreach (var item in AlstData)
            {
                if (item.IsChecked)
                {

                    DataRow drCrType = dtType.NewRow();
                    drCrType["IsPosted"] = 1;
                    drCrType["EMIAuthNo"] = item.EMIAuthNo;
                    drCrType["InstallmentNo"] = item.InstallmentNo;
                    if (Type == 0)
                    {
                        drCrType["TotalPrincipalAmount"] = item.TotalPrepaymentPrincipalAmount;
                        drCrType["TotalInterestAmount"] = item.TotalPrepaymentInterestAmount;
                    }
                    else
                    {
                        drCrType["TotalPrincipalAmount"] = item.TotalPrincipalAmt;
                        drCrType["TotalInterestAmount"] = item.TotalInterestAmt;
                    }
                    drCrType["LoginName"] = user;
                    dtType.Rows.Add(drCrType);


                }

            }

            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter(){ ParameterName="@P_CardNumber", SqlDbType= SqlDbType.Structured, Value = dtType, TypeName = "UD_EMITranIntUpdate", Direction = ParameterDirection.Input},
                new SqlParameter(){ ParameterName="@P_Mode", SqlDbType = SqlDbType.Int , Size = 15, Value = Type, Direction = ParameterDirection.Input}
            };
          
                objAuthorizationRepository.UnitOfWork.ExecuteCommand("PR_UpdateEMIInstallmentTransaction @P_CardNumber,@P_Mode", parameters);
        


            return retValues;
        }

        #endregion
        
        #region Implemented methods
        public List<EMITransactionInstallmentDTO> GetPendingEMIItems(int APageNo, EMITransactionInstallmentDTO AForceAuthorizationDTO, List<EMITransactionInstallmentDTO> AList, string AType)
        {

            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>();
            if (AType != null && AType != "")
            {
                lstExp = new List<Utils.ExpressionList>(){
                 new Utils.ExpressionList(){FilterColumn = "EMIAuthNo", FilterValue = AForceAuthorizationDTO.EMIAuthNo, Operator = "Contains"},             
                
            };

            }
            else
            {
                lstExp = new List<Utils.ExpressionList>(){
                    new Utils.ExpressionList(){FilterColumn = "EMIAuthNo", FilterValue = AForceAuthorizationDTO.EMIAuthNo, Operator = "Contains"},           
                         
                                
             
            };

            }

            Expression<Func<EMITransactionInstallment, bool>> expression = Utils.GetExpression<EMITransactionInstallment>(lstExp);

            int iRecordCount = objEMITransactionRepository.GetRecordCount(expression);
            var vApprovalQueueDTO = objEMITransactionRepository.GetFiltered(APageNo, iRecordCount, expression);

          



            if (vApprovalQueueDTO != null && vApprovalQueueDTO.Any())
            {
                List<EMITransactionInstallmentDTO> lstForceAuthorizationDTO;
                if (AList == null)
                {
                    lstForceAuthorizationDTO = new List<EMITransactionInstallmentDTO>();

                    foreach (var variable in vApprovalQueueDTO)
                    {
                        lstForceAuthorizationDTO.Add(Conversion.Mapping.EMITransactionInstallmentToEMITransactionInstallmentDTO(variable));
                    }
                }

                else
                    lstForceAuthorizationDTO = AList;

                return lstForceAuthorizationDTO;
            }
            else
                return new List<EMITransactionInstallmentDTO>();

        }

        public int GetPendingEMIItemsRecordCount(EMITransactionInstallmentDTO AForceAuthorizationDTO, string AType)
        {
            int iRetValue = 0;
            try
            {

                if (AType != null && AType != "")
                {
                    List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                 new Utils.ExpressionList(){FilterColumn = "EMIAuthNo", FilterValue = AForceAuthorizationDTO.EMIAuthNo , Operator = "Contains"},
              
             

            };
                    Expression<Func<EMITransactionInstallment, bool>> expression = Utils.GetExpression<EMITransactionInstallment>(lstExp);
                    iRetValue = objEMITransactionRepository.GetRecordCount(expression);
                }
                else
                {
                    List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                    new Utils.ExpressionList(){FilterColumn = "EMIAuthNo", FilterValue = AForceAuthorizationDTO.EMIAuthNo, Operator = "Contains"},
                  
             
            };
                    Expression<Func<EMITransactionInstallment, bool>> expression = Utils.GetExpression<EMITransactionInstallment>(lstExp);
                    iRetValue = objEMITransactionRepository.GetRecordCount(expression);
                }






            }
            catch (Exception ex)
            {
                throw ex;

            }
            return iRetValue;

        }

        #endregion
        
        #region EMIAuthorizationData

        public string[] EMIMasterData(List<EMIAuthorizationDTO> AlstData, string user)
        {
            string[] retValues = new string[7];
            DataTable dtType = new DataTable();
            dtType.Columns.Add("IsPosted");
            dtType.Columns.Add("TransactionNo");
            dtType.Columns.Add("LoginName");

            foreach (var item in AlstData)
            {
                if (item.IsChecked)
                {

                    DataRow drCrType = dtType.NewRow();
                    drCrType["IsPosted"] = 1;
                    drCrType["EMIAuthNo"] = item.EMIAuthNo;
                    drCrType["LoginName"] = user;
                    dtType.Rows.Add(drCrType);



                }

            }

            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter(){ ParameterName="@P_CardNumber", SqlDbType= SqlDbType.Structured, Value = dtType, TypeName = "UD_Transactionupdate", Direction = ParameterDirection.Input}
               // new SqlParameter(){ ParameterName="@P_CreatedBy", SqlDbType = SqlDbType.VarChar, Size = 15, Value = AUserName, Direction = ParameterDirection.Input}
            };
            objAuthorizationRepository.UnitOfWork.ExecuteCommand("PR_UpdateTransaction @P_CardNumber", parameters);


            return retValues;
        }

        #endregion
        
        #region Implemented methods
        public List<EMIAuthorizationDTO> GetPendingEMIMasterItems(int APageNo, EMIAuthorizationDTO AForceAuthorizationDTO, List<EMIAuthorizationDTO> AList, string AType)
        {

            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>();
            if (AType != null && AType != "")
            {
                lstExp = new List<Utils.ExpressionList>(){
                    new Utils.ExpressionList(){FilterColumn = "RUT", FilterValue = AForceAuthorizationDTO.RUT, Operator = "Contains"},
                    new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = AForceAuthorizationDTO.CardNumber, Operator = "Contains"},     

            };

            }
            else
            {
                lstExp = new List<Utils.ExpressionList>(){
                    new Utils.ExpressionList(){FilterColumn = "RUT", FilterValue = AForceAuthorizationDTO.RUT, Operator = "Contains"},
                    new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = AForceAuthorizationDTO.CardNumber, Operator = "Contains"},      
                                
             
            };

            }

            Expression<Func<EMIAuthorization, bool>> expression = Utils.GetExpression<EMIAuthorization>(lstExp);
            int iRecordCount = objEMIAuthorizationRepository.GetRecordCount(expression);
            var vApprovalQueueDTO = objEMIAuthorizationRepository.GetFiltered(APageNo, iRecordCount, expression);



            if (vApprovalQueueDTO != null && vApprovalQueueDTO.Any())
            {
                List<EMIAuthorizationDTO> lstForceAuthorizationDTO;
                if (AList == null)
                {
                    lstForceAuthorizationDTO = new List<EMIAuthorizationDTO>();

                    foreach (var variable in vApprovalQueueDTO)
                    {
                        lstForceAuthorizationDTO.Add(Conversion.Mapping.EMIAuthorizationToEMIAuthorizationDTO(variable));
                    }
                }

                else
                    lstForceAuthorizationDTO = AList;

                return lstForceAuthorizationDTO;
            }
            else
                return new List<EMIAuthorizationDTO>();

        }

        public int GetPendingEMIIMasterItemsRecordCount(EMIAuthorizationDTO AForceAuthorizationDTO, string AType)
        {
            int iRetValue = 0;
            try
            {

                if (AType != null && AType != "")
                {
                    List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                 new Utils.ExpressionList(){FilterColumn = "RUT", FilterValue = AForceAuthorizationDTO.RUT , Operator = "Contains"},
                 new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = AForceAuthorizationDTO.CardNumber , Operator = "Contains"},
              
             

            };
                    Expression<Func<EMIAuthorization, bool>> expression = Utils.GetExpression<EMIAuthorization>(lstExp);
                    iRetValue = objEMIAuthorizationRepository.GetRecordCount(expression);
                }
                else
                {
                    List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                    new Utils.ExpressionList(){FilterColumn = "RUT", FilterValue = AForceAuthorizationDTO.RUT, Operator = "Contains"},
                    new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = AForceAuthorizationDTO.CardNumber, Operator = "Contains"},                            
                                
             
            };
                    Expression<Func<EMIAuthorization, bool>> expression = Utils.GetExpression<EMIAuthorization>(lstExp);
                    iRetValue = objEMIAuthorizationRepository.GetRecordCount(expression);
                }






            }
            catch (Exception ex)
            {
                throw ex;

            }
            return iRetValue;

        }

        #endregion
        
        #endregion

        #region EMIAuthorizationData

        public string[] EMIPreClosureData(List<EMIAuthorizationDTO > AlstData, string user)
        {
            string[] retValues = new string[7];
            DataTable dtType = new DataTable();
            dtType.Columns.Add("IsPosted");
            dtType.Columns.Add("EMIAuthNo");
            dtType.Columns.Add("InstallmentNo");
            dtType.Columns.Add("TotalPrincipalAmount");
            dtType.Columns.Add("TotalInterestAmount");

            dtType.Columns.Add("LoginName");

            foreach (var item in AlstData)
            {
                    DataRow drCrType = dtType.NewRow();
                    drCrType["IsPosted"] = 1;
                    drCrType["EMIAuthNo"] = item.EMIAuthNo;
                    drCrType["InstallmentNo"] = 0;                   
                    
                    drCrType["TotalPrincipalAmount"] = item.AdjPrincipalAmount;
                    drCrType["TotalInterestAmount"] = item.AdjPrincipalAmount;
                    drCrType["LoginName"] = user;
                    dtType.Rows.Add(drCrType);


               

            }

            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter(){ ParameterName="@P_CardNumber", SqlDbType= SqlDbType.Structured, Value = dtType, TypeName = "UD_EMITranIntUpdate", Direction = ParameterDirection.Input}
               // new SqlParameter(){ ParameterName="@P_CreatedBy", SqlDbType = SqlDbType.VarChar, Size = 15, Value = AUserName, Direction = ParameterDirection.Input}
            };
            objAuthorizationRepository.UnitOfWork.ExecuteCommand("PR_UpdateEMIPreClosureTransaction @P_CardNumber", parameters);


            return retValues;
        }

        #endregion

        #region Implemented methods
        public List<EMIAuthorizationDTO> GetPendingEMIPreClosureItems(int APageNo, EMIAuthorizationDTO AForceAuthorizationDTO, List<EMIAuthorizationDTO> AList, string AType)
        {

            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>();
            if (AType != null && AType != "")
            {
                lstExp = new List<Utils.ExpressionList>(){
                 new Utils.ExpressionList(){FilterColumn = "EMIAuthNo", FilterValue = AForceAuthorizationDTO.EMIAuthNo, Operator = "Contains"},             
                        };
            }
            else
            {
                lstExp = new List<Utils.ExpressionList>(){
                    new Utils.ExpressionList(){FilterColumn = "EMIAuthNo", FilterValue = AForceAuthorizationDTO.EMIAuthNo, Operator = "Contains"},             
                         
                  };

            }

            Expression<Func<EMIAuthorization, bool>> expression = Utils.GetExpression<EMIAuthorization>(lstExp);
            int iRecordCount = objEMIAuthorizationRepository.GetRecordCount(expression);
            var vApprovalQueueDTO = objEMIAuthorizationRepository.GetFiltered(APageNo, iRecordCount, expression);



            if (vApprovalQueueDTO != null && vApprovalQueueDTO.Any())
            {
                List<EMIAuthorizationDTO> lstForceAuthorizationDTO;
                if (AList == null)
                {
                    lstForceAuthorizationDTO = new List<EMIAuthorizationDTO>();

                    foreach (var variable in vApprovalQueueDTO)
                    {
                        lstForceAuthorizationDTO.Add(Conversion.Mapping.EMIAuthorizationToEMIAuthorizationDTO (variable));
                    }
                }

                else
                    lstForceAuthorizationDTO = AList;

                return lstForceAuthorizationDTO;
            }
            else
                return new List<EMIAuthorizationDTO>();

        }

        public int GetPendingEMIIPreClosureItemsRecordCount(EMIAuthorizationDTO AForceAuthorizationDTO, string AType)
        {
            int iRetValue = 0;
            try
            {

                if (AType != null && AType != "")
                {
                    List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                 new Utils.ExpressionList(){FilterColumn = "EMIAuthNo", FilterValue = AForceAuthorizationDTO.EMIAuthNo , Operator = "Contains"},
              
             

            };
                    Expression<Func<EMIAuthorization, bool>> expression = Utils.GetExpression<EMIAuthorization>(lstExp);
                    iRetValue = objEMIAuthorizationRepository .GetRecordCount(expression);
                }
                else
                {
                    List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                    new Utils.ExpressionList(){FilterColumn = "EMIAuthNo", FilterValue = AForceAuthorizationDTO.EMIAuthNo, Operator = "Contains"},
                  
             
            };
                    Expression<Func<EMIAuthorization, bool>> expression = Utils.GetExpression<EMIAuthorization>(lstExp);
                    iRetValue = objEMIAuthorizationRepository.GetRecordCount(expression);
                }


            }
            catch (Exception ex)
            {
                throw ex;

            }
            return iRetValue;

        }

        #endregion

        #region SaveManualTransactionDetails
        public void SaveReversalTransactionDetails(ReversalTransactionDTO AManualTransactionDTO)
        {
            if (AManualTransactionDTO == null)
            {
                throw new ArgumentException("");
            }

            var newManualTransaction = ManualTransactionFactory.CreateManualTransaction(AManualTransactionDTO.BatchNo,
                AManualTransactionDTO.CardNumber, AManualTransactionDTO.RUT, AManualTransactionDTO.TxnDate, AManualTransactionDTO.Time,
                AManualTransactionDTO.AuthCode, AManualTransactionDTO.OrganizationCode, AManualTransactionDTO.MerchantCode, AManualTransactionDTO.IncomingTransType, AManualTransactionDTO.MCCCode,
                AManualTransactionDTO.SegmentCode, AManualTransactionDTO.TransType, AManualTransactionDTO.Amount, AManualTransactionDTO.IsManualEntry,
                AManualTransactionDTO.PostedDate, AManualTransactionDTO.IsPosted,
                AManualTransactionDTO.AuthCancellationStatus, AManualTransactionDTO.ReferenceAuthCode, AManualTransactionDTO.AuthCancellationRemarks,
                AManualTransactionDTO.CreatedBy, AManualTransactionDTO.LastUpdatedBy, AManualTransactionDTO.ApprovalStatus, AManualTransactionDTO.InvoiceNo,
                AManualTransactionDTO.OrderNo, AManualTransactionDTO.SalesId, AManualTransactionDTO.Remarks,
                AManualTransactionDTO.CreditNote, AManualTransactionDTO.BalanceAmount, null);

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {

                SqlParameter[] paramCollection = new SqlParameter[]{ 
                    new SqlParameter(parameterName: "@P_AuthCode",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_CardNumber", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_MerchantCode", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_SegmentCode", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_MCCCode",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_Amount",dbType:System.Data.SqlDbType.Decimal ),
                    new SqlParameter(parameterName: "@P_IncomingTransTypeId",dbType:System.Data.SqlDbType.Int ),
                    new SqlParameter(parameterName: "@p_TxnDate",dbType:System.Data.SqlDbType.DateTime2 ),
                    new SqlParameter(parameterName: "@P_TxnTime",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@p_TxnType",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_ApprovalStatus",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_CreatedBy",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(){ ParameterName = "@P_AuthCancellationStatus", Value = (object)AManualTransactionDTO.AuthCancellationStatus ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_ReferenceAuthCode", Value = (object)AManualTransactionDTO.ReferenceAuthCode ?? DBNull.Value ,SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_AuthCancellationRemarks", Value = (object)AManualTransactionDTO.AuthCancellationRemarks ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_OrganizationCode", Value = (object)AManualTransactionDTO.OrganizationCode ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                     new SqlParameter(){ ParameterName = "@P_InvoiceNo", Value = (object)AManualTransactionDTO.InvoiceNo ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_SalesId", Value = (object)AManualTransactionDTO.SalesId ?? DBNull.Value ,SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_OrderNo", Value = (object)AManualTransactionDTO.OrderNo ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_Remarks", Value = (object)AManualTransactionDTO.Remarks ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                     new SqlParameter(){ ParameterName = "@P_CreditNote", Value = (object)AManualTransactionDTO.CreditNote ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_BalanceAmount", Value = (object)AManualTransactionDTO.BalanceAmount ?? 0, SqlDbType = SqlDbType.Decimal },

                };

                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_AuthCode")).First()).Value = AManualTransactionDTO.AuthCode == null ? "0" : AManualTransactionDTO.AuthCode;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CardNumber")).First()).Value = AManualTransactionDTO.CardNumber;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_MerchantCode")).First()).Value = AManualTransactionDTO.MerchantCode == null ? "" : AManualTransactionDTO.MerchantCode;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_SegmentCode")).First()).Value = AManualTransactionDTO.SegmentCode == null ? "" : AManualTransactionDTO.SegmentCode;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_MCCCode")).First()).Value = AManualTransactionDTO.MCCCode == null ? "" : AManualTransactionDTO.MCCCode;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Amount")).First()).Value = AManualTransactionDTO.Amount;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_IncomingTransTypeId")).First()).Value = AManualTransactionDTO.IncomingTransType;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@p_TxnDate")).First()).Value = AManualTransactionDTO.TxnDate;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TxnTime")).First()).Value = AManualTransactionDTO.Time == null ? "" : AManualTransactionDTO.Time;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@p_TxnType")).First()).Value = AManualTransactionDTO.TransType;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ApprovalStatus")).First()).Value = AManualTransactionDTO.ApprovalStatus;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ReferenceAuthCode")).First()).Value = AManualTransactionDTO.ReferenceAuthCode == null ? "" : AManualTransactionDTO.ReferenceAuthCode;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CreatedBy")).First()).Value = AManualTransactionDTO.CreatedBy;

                objManualTransactionRepository.UnitOfWork.ExecuteCommand(@"PR_ManualTransactionSave @P_AuthCode, @P_CardNumber, 
                        @P_MerchantCode, @P_SegmentCode, @P_MCCCode, @P_Amount, @P_IncomingTransTypeId, @p_TxnDate,@P_TxnTime, @p_TxnType, @P_ApprovalStatus, 
                        @P_CreatedBy, @P_AuthCancellationStatus,@P_ReferenceAuthCode,@P_AuthCancellationRemarks,@P_OrganizationCode,@P_InvoiceNo,@P_SalesId,@P_OrderNo,@P_Remarks,@P_CreditNote,@P_BalanceAmount", paramCollection);
                scope.Complete();
            }
        }
        #endregion

        #region Transaction Adjustment
        #region GetTransactionsToAdjust
        public List<TransactionAdjustmentDTO> GetTransactionsToAdjust(string ARUT, string ACardNumber, string ACustomerName, string ATransactionCodeSubCode, string APostedDate, int ABillMonth, int ABillYear)
        {
            List<TransactionAdjustmentDTO> objTransactions = new List<TransactionAdjustmentDTO>();

            SqlParameter[] objParameters = new SqlParameter[] { 
                new SqlParameter(){ ParameterName = "@P_RUT", Value = string.IsNullOrWhiteSpace(ARUT) ? string.Empty : ARUT, SqlDbType = SqlDbType.VarChar, Size = 25 },
                new SqlParameter(){ ParameterName = "@P_CardNumber", Value = string.IsNullOrWhiteSpace(ACardNumber) ? string.Empty : ACardNumber, SqlDbType = SqlDbType.VarChar, Size = 16 },
                new SqlParameter(){ ParameterName = "@P_CustomerName", Value = string.IsNullOrWhiteSpace(ACustomerName) ? string.Empty : ACustomerName, SqlDbType = SqlDbType.VarChar, Size = 50 },
                new SqlParameter(){ ParameterName = "@P_TransactionCodeSubCode", Value = string.IsNullOrWhiteSpace(ATransactionCodeSubCode) ? string.Empty : ATransactionCodeSubCode, SqlDbType = SqlDbType.VarChar, Size = 10 },
                new SqlParameter(){ ParameterName = "@P_PostedDate", Value = string.IsNullOrWhiteSpace(APostedDate) ? DateTime.Parse("01/01/1753") : Convert.ToDateTime(APostedDate), SqlDbType = SqlDbType.DateTime },
                new SqlParameter(){ ParameterName = "@P_BillMonth", Value = ABillMonth, SqlDbType = SqlDbType.Int },
                new SqlParameter(){ ParameterName = "@P_BillYear", Value = ABillYear, SqlDbType = SqlDbType.Int }
            };

            objTransactions = objManualTransactionRepository.UnitOfWork.ExecuteQuery<TransactionAdjustmentDTO>("PR_GetTransactionsToAdjust @P_RUT,@P_CardNumber,@P_CustomerName, @P_TransactionCodeSubCode, @P_PostedDate,@P_BillMonth,@P_BillYear", objParameters).ToList();

            return objTransactions;
        } 
        #endregion
        #region ViewAdjustmentDetails
        public List<TransactionAdjustmentDTO> ViewAdjustmentDetails(string AAuthorizationCode)
        {
            List<TransactionAdjustmentDTO> objAdjustmentDetails = new List<TransactionAdjustmentDTO>();

            SqlParameter objParameter = new SqlParameter() { ParameterName = "@P_AuthorizationCode", Value = string.IsNullOrWhiteSpace(AAuthorizationCode) ? string.Empty : AAuthorizationCode, SqlDbType = SqlDbType.VarChar, Size = 100 };
            objAdjustmentDetails = objManualTransactionRepository.UnitOfWork.ExecuteQuery<TransactionAdjustmentDTO>("SELECT AuthCode AuthorizationCode,ReferenceAuthCode,Amount AdjustedAmount,Remarks, " +
                                    "CreatedOn,CreatedBy FROM TR_Transaction WHERE ReferenceAuthCode = @P_AuthorizationCode AND ISNULL(IsAdjusted,0) = 1", objParameter).ToList();

            return objAdjustmentDetails;
        }
        #endregion
        #region ShowTransactionDetailsToAdjust
        public TransactionAdjustmentDTO ShowTransactionDetailsToAdjust(string AAuthorizationCode ,string ATransactionCode ,string ATransactionSubcode, DateTime ATxnDate)
        {
            TransactionAdjustmentDTO objAdjustmentDetails = new TransactionAdjustmentDTO();

            SqlParameter objParameter = new SqlParameter() { ParameterName = "@P_AuthorizationCode", Value = string.IsNullOrWhiteSpace(AAuthorizationCode) ? string.Empty : AAuthorizationCode, SqlDbType = SqlDbType.VarChar, Size = 100 };
            SqlParameter objParameter1 = new SqlParameter() { ParameterName = "@P_TransactionCode", Value = string.IsNullOrWhiteSpace(ATransactionCode) ? string.Empty : ATransactionCode, SqlDbType = SqlDbType.VarChar, Size = 5 };
            SqlParameter objParameter2 = new SqlParameter() { ParameterName = "@P_TransactionSubCode", Value = string.IsNullOrWhiteSpace(ATransactionSubcode) ? string.Empty : ATransactionSubcode, SqlDbType = SqlDbType.VarChar, Size = 5 };
            SqlParameter objParameter3 = new SqlParameter() { ParameterName = "@P_TxnDate", Value = ATxnDate, SqlDbType = SqlDbType.DateTime };
            objAdjustmentDetails = objManualTransactionRepository.UnitOfWork.ExecuteQuery<TransactionAdjustmentDTO>("PR_ShowTransactionDetailsToAdjust @P_AuthorizationCode,@P_TransactionCode,@P_TransactionSubCode, @P_TxnDate", objParameter,objParameter1 ,objParameter2, objParameter3 ).FirstOrDefault();

            return objAdjustmentDetails;
        }

        #endregion

        #region SaveTransactionAdjustmentDetails
        public void SaveTransactionAdjustmentDetails(ManualTransactionDTO AManualTransactionDTO)
        {
            if (AManualTransactionDTO == null)
            {
                throw new ArgumentException("");
            }

            var newManualTransaction = ManualTransactionFactory.CreateManualTransaction(AManualTransactionDTO.BatchNo,
                AManualTransactionDTO.CardNumber, AManualTransactionDTO.RUT, AManualTransactionDTO.TxnDate, AManualTransactionDTO.Time,
                AManualTransactionDTO.AuthCode, AManualTransactionDTO.OrganizationCode, AManualTransactionDTO.MerchantCode, AManualTransactionDTO.IncomingTransType, AManualTransactionDTO.MCCCode,
                AManualTransactionDTO.SegmentCode, AManualTransactionDTO.TransType, AManualTransactionDTO.Amount, AManualTransactionDTO.IsManualEntry,
                AManualTransactionDTO.PostedDate, AManualTransactionDTO.IsPosted,
                AManualTransactionDTO.AuthCancellationStatus, AManualTransactionDTO.ReferenceAuthCode, AManualTransactionDTO.AuthCancellationRemarks,
                AManualTransactionDTO.CreatedBy, AManualTransactionDTO.LastUpdatedBy, AManualTransactionDTO.ApprovalStatus, AManualTransactionDTO.InvoiceNo,
                AManualTransactionDTO.OrderNo, AManualTransactionDTO.SalesId, AManualTransactionDTO.Remarks,
                AManualTransactionDTO.CreditNote, AManualTransactionDTO.BalanceAmount, null);

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {

                SqlParameter[] paramCollection = new SqlParameter[]{ 
                    new SqlParameter(parameterName: "@P_AuthCode",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_CardNumber", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_MerchantCode", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_SegmentCode", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_MCCCode",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_Amount",dbType:System.Data.SqlDbType.Decimal ),
                    new SqlParameter(parameterName: "@P_IncomingTransTypeId",dbType:System.Data.SqlDbType.Int ),
                    new SqlParameter(parameterName: "@p_TxnDate",dbType:System.Data.SqlDbType.DateTime2 ),
                    new SqlParameter(parameterName: "@P_TxnTime",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@p_TxnType",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_ApprovalStatus",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_CreatedBy",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(){ ParameterName = "@P_AuthCancellationStatus", Value = (object)AManualTransactionDTO.AuthCancellationStatus ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_ReferenceAuthCode", Value = (object)AManualTransactionDTO.ReferenceAuthCode ?? DBNull.Value ,SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_AuthCancellationRemarks", Value = (object)AManualTransactionDTO.AuthCancellationRemarks ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_OrganizationCode", Value = (object)AManualTransactionDTO.OrganizationCode ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_InvoiceNo", Value = (object)AManualTransactionDTO.InvoiceNo ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_SalesId", Value = (object)AManualTransactionDTO.SalesId ?? DBNull.Value ,SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_OrderNo", Value = (object)AManualTransactionDTO.OrderNo ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_Remarks", Value = (object)AManualTransactionDTO.Remarks ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_CreditNote", Value = (object)AManualTransactionDTO.CreditNote ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter(){ ParameterName = "@P_BalanceAmount", Value = (object)AManualTransactionDTO.BalanceAmount ?? 0, SqlDbType = SqlDbType.Decimal },
                    new SqlParameter(){ ParameterName = "@P_TransactionAdjustmentCode", Value = (object)AManualTransactionDTO.TransactionAdjustmentCode ?? DBNull.Value, SqlDbType = SqlDbType.VarChar },
                     new SqlParameter(){ ParameterName = "@P_TransactionSubCodeOrg", Value = (object)AManualTransactionDTO.TransactionSubCode ?? DBNull.Value, SqlDbType = SqlDbType.VarChar }

                };

                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_AuthCode")).First()).Value = AManualTransactionDTO.AuthCode == null ? "0" : AManualTransactionDTO.AuthCode;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CardNumber")).First()).Value = AManualTransactionDTO.CardNumber;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_MerchantCode")).First()).Value = AManualTransactionDTO.MerchantCode == null ? "" : AManualTransactionDTO.MerchantCode;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_SegmentCode")).First()).Value = AManualTransactionDTO.SegmentCode == null ? "" : AManualTransactionDTO.SegmentCode;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_MCCCode")).First()).Value = AManualTransactionDTO.MCCCode == null ? "" : AManualTransactionDTO.MCCCode;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Amount")).First()).Value = AManualTransactionDTO.Amount;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_IncomingTransTypeId")).First()).Value = AManualTransactionDTO.IncomingTransType == null ? 0 : AManualTransactionDTO.IncomingTransType; 
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@p_TxnDate")).First()).Value = AManualTransactionDTO.TxnDate;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TxnTime")).First()).Value = AManualTransactionDTO.Time == null ? "" : AManualTransactionDTO.Time;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@p_TxnType")).First()).Value = AManualTransactionDTO.TransType;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ApprovalStatus")).First()).Value = AManualTransactionDTO.ApprovalStatus;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ReferenceAuthCode")).First()).Value = AManualTransactionDTO.ReferenceAuthCode == null ? "" : AManualTransactionDTO.ReferenceAuthCode;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CreatedBy")).First()).Value = AManualTransactionDTO.CreatedBy;
                //((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TransactionSubCodeOrg")).First()).Value = AManualTransactionDTO.TransactionSubCode;

                objManualTransactionRepository.UnitOfWork.ExecuteCommand(@"PR_TransactionAdjustmentSave @P_AuthCode, @P_CardNumber, 
                        @P_MerchantCode, @P_SegmentCode, @P_MCCCode, @P_Amount, @P_IncomingTransTypeId, @p_TxnDate,@P_TxnTime, @p_TxnType, @P_ApprovalStatus, 
                        @P_CreatedBy, @P_AuthCancellationStatus,@P_ReferenceAuthCode,@P_AuthCancellationRemarks,@P_OrganizationCode,@P_InvoiceNo,@P_SalesId,@P_OrderNo,@P_Remarks,@P_CreditNote,@P_BalanceAmount, @P_TransactionAdjustmentCode,@P_TransactionSubCodeOrg ", paramCollection);
                scope.Complete();
            }
        }
        #endregion
        #endregion
    }
}
