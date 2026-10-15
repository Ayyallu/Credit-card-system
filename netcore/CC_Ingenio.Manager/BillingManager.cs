using CC_Ingenio.Common.Utils;
using CC_Ingenio.Core.BillingRepo.BillingAggregate;
using CC_Ingenio.DataTranfer.BillingInfo;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.Repository.BillingRepo;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using CC_Ingenio.DataTranfer.MasterInfo;


namespace CC_Ingenio.Manager
{
    public class BillingManager : IBillingManager
    {

        #region Variables
        OnlinePaymentRepository objOnlinePaymentRepository;
        IReportGenerator objReportGenerator;
        RefinancingRepository objRefinancingRepository;
        RenegotiationRepository objRenegotiationRepository;
        StatementGenerationRepository objStatementGenerationRepository;
        #endregion

        #region Constructor
        public BillingManager(OnlinePaymentRepository AOnlinePaymentRepository, RefinancingRepository ARefinancingRepository,
            RenegotiationRepository ARenegotiationRepository, StatementGenerationRepository AStatementGenerationRepository,
            IReportGenerator AReportGenerator)
        {
            objReportGenerator = AReportGenerator;
            if (AOnlinePaymentRepository == null)
                throw new ArgumentNullException("AOnlinePaymentRepository");
            if (ARefinancingRepository == null)
                throw new ArgumentNullException("ARefinancingRepository");
            if (ARenegotiationRepository == null)
                throw new ArgumentNullException("ARenegotiationRepository");
            if (AStatementGenerationRepository == null)
                throw new ArgumentNullException("AStatementGenerationRepository");

            objOnlinePaymentRepository = AOnlinePaymentRepository;
            objRefinancingRepository = ARefinancingRepository;
            objRenegotiationRepository = ARenegotiationRepository;
            objStatementGenerationRepository = AStatementGenerationRepository;

        }
        #endregion

        #region OnlinePayment

        #region SaveOnlinepaymentDetails
        public void SaveOnlinePaymentDetails(OnlinePaymentDTO AOnlinePaymentDTO)
        {
            //if (AOnlinePaymentDTO == null)
            //    throw new ArgumentException("Messages.warning_CannotAddOnlinePaymentWithNullInformation");

            //var newOnlinePayment = OnlinePaymentFactory.CreateOnlinePayment(
            //    AOnlinePaymentDTO.CardNumber, AOnlinePaymentDTO.RUT, AOnlinePaymentDTO.PaymentModeCode, AOnlinePaymentDTO.ChequeDate,
            //    AOnlinePaymentDTO.ChequeNo, AOnlinePaymentDTO.PaymentCurrency, AOnlinePaymentDTO.PaymentAmount, 
            //    AOnlinePaymentDTO.CreatedBy, AOnlinePaymentDTO.LastUpdatedBy, AOnlinePaymentDTO.ApprovalStatus, AOnlinePaymentDTO.OrganizationCode);

            //try
            //{
            //    var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
            //        new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            //    using (scope)
            //    {
            //        try
            //        {
            //            objOnlinePaymentRepository.Add(newOnlinePayment);
            //            objOnlinePaymentRepository.UnitOfWork.Commit();
            //            scope.Complete();
            //        }
            //        catch (Exception ex)
            //        {
            //            throw ex;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            if (AOnlinePaymentDTO == null)
            {
                throw new ArgumentException("");
            }

            try
            {
                SqlParameter[] paramCollection = new SqlParameter[]{ 
                    new SqlParameter(parameterName: "@P_AuthCode",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_CardNumber", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_IncomingTransTypeId", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_Amount",dbType:System.Data.SqlDbType.Decimal ),
                    new SqlParameter(parameterName: "@P_PaymentMode",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_TransactionDate",dbType:System.Data.SqlDbType.DateTime ),
                    new SqlParameter(parameterName: "@P_TxnTime",dbType:System.Data.SqlDbType.VarChar )
                };

                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_AuthCode")).First()).Value = AOnlinePaymentDTO.ChequeNo == null ? "" : AOnlinePaymentDTO.ChequeNo;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CardNumber")).First()).Value = AOnlinePaymentDTO.CardNumber;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_IncomingTransTypeId")).First()).Value = "PYC";
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Amount")).First()).Value = AOnlinePaymentDTO.PaymentAmount;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_PaymentMode")).First()).Value = AOnlinePaymentDTO.PaymentModeCode;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TransactionDate")).First()).Value = AOnlinePaymentDTO.ChequeDate;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TxnTime")).First()).Value = AOnlinePaymentDTO.TxnTime == null ? "" : AOnlinePaymentDTO.TxnTime;

                objOnlinePaymentRepository.UnitOfWork.ExecuteCommand("PR_TransactionPulling1 @P_AuthCode, @P_CardNumber, @P_IncomingTransTypeId, @P_Amount, @P_PaymentMode, @P_TransactionDate, @P_TxnTime", paramCollection);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
        #region FindOnlinePayment
        public List<OnlinePaymentDTO> FindOnlinePayment(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindOnlinePayment");

            int iRecordCount = objOnlinePaymentRepository.GetRecordCount();
            var OnlinePayments = objOnlinePaymentRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);
            if (OnlinePayments != null && OnlinePayments.Any())
            {
                List<OnlinePaymentDTO> lstOnlinePaymentDTO = new List<OnlinePaymentDTO>();
                foreach (var OnlinePayment in OnlinePayments)
                {
                    lstOnlinePaymentDTO.Add(Conversion.Mapping.OnlinePaymentToOnlinePaymentDTO(OnlinePayment));
                }
                return lstOnlinePaymentDTO;
            }
            else
                return new List<OnlinePaymentDTO>();

        }
        #endregion
        #region FindAll
        public List<OnlinePaymentDTO> FindAll(int APageNo, OnlinePaymentDTO AOnlinePaymentDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "ChequeDate", FilterValue = AOnlinePaymentDTO.ChequeDate.Date, Operator = "GreaterThanOrEqualTo"},
                new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = AOnlinePaymentDTO.CardNumber, Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "RUT", FilterValue = AOnlinePaymentDTO.RUT, Operator = "Contains"},
                new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode ,Operator = "Contains"}              
            };

            Expression<Func<OnlinePayment, bool>> expression = Utils.GetExpression<OnlinePayment>(lstExp);
            int iRecordCount = objOnlinePaymentRepository.GetRecordCount(expression);

            var vOnlinePaymentDTO = objOnlinePaymentRepository.GetFiltered(0, iRecordCount, expression);

            if (vOnlinePaymentDTO != null && vOnlinePaymentDTO.Any())
            {
                List<OnlinePaymentDTO> lstOnlinePaymentDTO = new List<OnlinePaymentDTO>();
                foreach (var OnlinePayment in vOnlinePaymentDTO)
                {
                    lstOnlinePaymentDTO.Add(Conversion.Mapping.OnlinePaymentToOnlinePaymentDTO(OnlinePayment));
                }
                return lstOnlinePaymentDTO;
            }
            else
                return new List<OnlinePaymentDTO>();
        }
        #endregion
        #region FindByOnlinePaymentId
        public OnlinePaymentDTO FindByOnlinePaymentId(int AOnlinePaymentId)
        {
            OnlinePayment OnlinePaymentDTO = objOnlinePaymentRepository.Get(AOnlinePaymentId);
            if (OnlinePaymentDTO != null)
            {

                objOnlinePaymentRepository.UnitOfWork.RefreshDataInModel(ref OnlinePaymentDTO);
                return Conversion.Mapping.OnlinePaymentToOnlinePaymentDTO(OnlinePaymentDTO);
            }
            else
            {
                return new OnlinePaymentDTO();
            }
        }
        #endregion

        #endregion

        #region Refinancing

        #region FindAll
        public List<RefinancingDTO> FindAll(int APageNo, RefinancingDTO ARefinancingDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = ARefinancingDTO.CardNumber, Operator = "Contains"}
            };

            Expression<Func<Refinancing, bool>> expression = Utils.GetExpression<Refinancing>(lstExp);
            int iRecordCount = objRefinancingRepository.GetRecordCount(expression);

            var vRefinancingDTO = objRefinancingRepository.GetFiltered(0, iRecordCount, expression);

            if (vRefinancingDTO != null && vRefinancingDTO.Any())
            {
                List<RefinancingDTO> lstRefinancingDTO = new List<RefinancingDTO>();
                foreach (var OnlinePayment in vRefinancingDTO)
                {
                    lstRefinancingDTO.Add(Conversion.Mapping.RefinancingToRefinancingDTO(OnlinePayment));
                }
                return lstRefinancingDTO;
            }
            else
                return new List<RefinancingDTO>();
        }
        #endregion
        #region FindByRefinancingCardNo
        public RefinancingDTO FindByRefinancingCardNo(string ACardNo)
        {
            Refinancing RefinancingDTO = objRefinancingRepository.Get(ACardNo);
            if (RefinancingDTO != null)
            {

                objRefinancingRepository.UnitOfWork.RefreshDataInModel(ref RefinancingDTO);
                return Conversion.Mapping.RefinancingToRefinancingDTO(RefinancingDTO);
            }
            else
            {
                return new RefinancingDTO();
            }
        }
        #endregion
        #region FindRefinancing
        public List<RefinancingDTO> FindRefinancing(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindRefinancing");

            int iRecordCount = objRefinancingRepository.GetRecordCount();
            var Refinancings = objRefinancingRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);
            if (Refinancings != null && Refinancings.Any())
            {
                List<RefinancingDTO> lstRefinancingDTO = new List<RefinancingDTO>();
                foreach (var Refinancing in Refinancings)
                {
                    lstRefinancingDTO.Add(Conversion.Mapping.RefinancingToRefinancingDTO(Refinancing));
                }
                return lstRefinancingDTO;
            }
            else
                return new List<RefinancingDTO>();

        }
        #endregion
        #region SaveRefinancingDetails

        public void SaveRefinancingDetails(RefinancingDTO ARefinancingDTO)
        {

            if (ARefinancingDTO == null)
            {
                throw new ArgumentException("");
            }

            try
            {

                SqlParameter[] paramCollection = new SqlParameter[]{ 
                    new SqlParameter(parameterName: "@P_CardNumber",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_Transactioncode", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_TotalInstallment", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_ExpectedInterestRate",dbType:System.Data.SqlDbType.Decimal ),
                    new SqlParameter(parameterName: "@P_LoanAmount",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_DownPaymentAmount",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_DiscountAmount",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_LoanBookedDate",dbType:System.Data.SqlDbType.DateTime ),
                    new SqlParameter(parameterName: "@P_CreatedBy",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_InvokeType",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_OrganizationCode",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_TotalAmount",dbType:System.Data.SqlDbType.VarChar )
                };

                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CardNumber")).First()).Value = ARefinancingDTO.CardNumber;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Transactioncode")).First()).Value = ARefinancingDTO.TransactionCode;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TotalInstallment")).First()).Value = ARefinancingDTO.Tenure;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ExpectedInterestRate")).First()).Value = ARefinancingDTO.InterestPercentage;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_LoanAmount")).First()).Value = ARefinancingDTO.NetRefinancedAmount;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_DownPaymentAmount")).First()).Value = ARefinancingDTO.DownPayment;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_DiscountAmount")).First()).Value = ARefinancingDTO.DiscountAmount;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_LoanBookedDate")).First()).Value = Utils.BusinessDate;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CreatedBy")).First()).Value = ARefinancingDTO.CreatedBy;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_InvokeType")).First()).Value = "RF";
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_OrganizationCode")).First()).Value = Utils.OrganizationCode;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TotalAmount")).First()).Value = ARefinancingDTO.MinDueAmount;

                objRefinancingRepository.UnitOfWork.ExecuteCommand("PR_ReFinancingToEMISave @P_CardNumber, @P_Transactioncode, @P_TotalInstallment, @P_ExpectedInterestRate, @P_LoanAmount, @P_DownPaymentAmount, @P_DiscountAmount, @P_LoanBookedDate,  @P_CreatedBy, @P_InvokeType, @P_OrganizationCode,@P_TotalAmount", paramCollection);


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region GetRefinancingDetails
        public List<RefinancingDTO> GetRefinancingDetails(string ACardNumber, string ARUT)
        {
            List<RefinancingDTO> objResult;
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter() { ParameterName = "@P_CardNumber", SqlDbType = SqlDbType.VarChar, Size = 16, Value = (ACardNumber == null ? string.Empty : ACardNumber), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_RUT", SqlDbType = SqlDbType.VarChar, Size = 11, Value = (ARUT == null ? string.Empty : ARUT.Replace("-",string.Empty)), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_OrganizationCode", SqlDbType = SqlDbType.VarChar, Size = 5, Value = Utils .OrganizationCode , Direction = ParameterDirection.Input}
             };
            objResult = objRefinancingRepository.UnitOfWork.ExecuteQuery<RefinancingDTO>("PR_GetRefinanceDetails @P_CardNumber, @P_RUT, @P_OrganizationCode", parameters).ToList();//, @TYPE 
            if (objResult == null)
            {
                objResult = new List<RefinancingDTO>();
            }
            return objResult;
        }
        #endregion

        #endregion

        #region Renegotiation

        #region FindAll
        public List<RenegotiationDTO> FindAll(int APageNo, RenegotiationDTO ARenegotiationDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = ARenegotiationDTO.CardNumber, Operator = "Contains"}
            };

            Expression<Func<Renegotiation, bool>> expression = Utils.GetExpression<Renegotiation>(lstExp);
            int iRecordCount = objRenegotiationRepository.GetRecordCount(expression);

            var vRenegotiationDTO = objRenegotiationRepository.GetFiltered(0, iRecordCount, expression);

            if (vRenegotiationDTO != null && vRenegotiationDTO.Any())
            {
                List<RenegotiationDTO> lstRenegotiationDTO = new List<RenegotiationDTO>();
                foreach (var Renegotiation in vRenegotiationDTO)
                {
                    lstRenegotiationDTO.Add(Conversion.Mapping.RenegotiationToRenegotiationDTO(Renegotiation));
                }
                return lstRenegotiationDTO;
            }
            else
                return new List<RenegotiationDTO>();
        }
        #endregion
        #region FindByRenegotiationCardNo
        public RenegotiationDTO FindByRenegotiationCardNo(string ACardNo)
        {
            Renegotiation RenegotiationDTO = objRenegotiationRepository.Get(ACardNo);
            if (RenegotiationDTO != null)
            {

                objRenegotiationRepository.UnitOfWork.RefreshDataInModel(ref RenegotiationDTO);
                return Conversion.Mapping.RenegotiationToRenegotiationDTO(RenegotiationDTO);
            }
            else
            {
                return new RenegotiationDTO();
            }
        }
        #endregion
        #region FindRenegotiation
        public List<RenegotiationDTO> FindRenegotiation(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindRefinancing");

            int iRecordCount = objRenegotiationRepository.GetRecordCount();
            var Renegotiations = objRenegotiationRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);
            if (Renegotiations != null && Renegotiations.Any())
            {
                List<RenegotiationDTO> lstRenegotiationDTO = new List<RenegotiationDTO>();
                foreach (var Renegotiation in Renegotiations)
                {
                    lstRenegotiationDTO.Add(Conversion.Mapping.RenegotiationToRenegotiationDTO(Renegotiation));
                }
                return lstRenegotiationDTO;
            }
            else
                return new List<RenegotiationDTO>();

        }
        #endregion
        #region SaveRenegotiationDetails
        public void SaveRenegotiationDetails(RenegotiationDTO ARenegotiationDTO)
        {

            if (ARenegotiationDTO == null)
            {
                throw new ArgumentException("");
            }

            try
            {

                SqlParameter[] paramCollection = new SqlParameter[]{ 
                    new SqlParameter(parameterName: "@P_CardNumber",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_Transactioncode", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_TotalInstallment", dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_ExpectedInterestRate",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_LoanAmount",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_DownPaymentAmount",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_DiscountAmount",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_LoanBookedDate",dbType:System.Data.SqlDbType.DateTime ),
                    new SqlParameter(parameterName: "@P_CreatedBy",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_InvokeType",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_OrganizationCode",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(parameterName: "@P_TotalAmount",dbType:System.Data.SqlDbType.VarChar )
                };

                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CardNumber")).First()).Value = ARenegotiationDTO.CardNumber;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Transactioncode")).First()).Value = ARenegotiationDTO.TransactionCode;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TotalInstallment")).First()).Value = ARenegotiationDTO.Tenure;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ExpectedInterestRate")).First()).Value = ARenegotiationDTO.InterestPercentage;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_LoanAmount")).First()).Value = ARenegotiationDTO.NetRenegotiatedAmount;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_DownPaymentAmount")).First()).Value = ARenegotiationDTO.DownPayment;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_DiscountAmount")).First()).Value = ARenegotiationDTO.DiscountAmount;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_LoanBookedDate")).First()).Value = Utils.BusinessDate;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CreatedBy")).First()).Value = ARenegotiationDTO.CreatedBy;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_InvokeType")).First()).Value = "RG";
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_OrganizationCode")).First()).Value = Utils.OrganizationCode;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TotalAmount")).First()).Value = ARenegotiationDTO.TotalOS;


                objRefinancingRepository.UnitOfWork.ExecuteCommand("PR_ReFinancingToEMISave @P_CardNumber, @P_Transactioncode, @P_TotalInstallment, @P_ExpectedInterestRate, @P_LoanAmount, @P_DownPaymentAmount, @P_DiscountAmount, @P_LoanBookedDate,  @P_CreatedBy, @P_InvokeType, @P_OrganizationCode,@P_TotalAmount", paramCollection);


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region GetRenegotiationDetails
        public List<RenegotiationDTO> GetRenegotiationDetails(string ACardNumber , string ARUT)
        {
            List<RenegotiationDTO> objResult;

            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter() { ParameterName = "@P_CardNumber", SqlDbType = SqlDbType.VarChar, Size = 16, Value = (ACardNumber == null ? string.Empty : ACardNumber), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_RUT", SqlDbType = SqlDbType.VarChar, Size = 16, Value = (ARUT == null ? string.Empty : ARUT), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_OrganizationCode", SqlDbType = SqlDbType.VarChar, Size = 5, Value =  Utils.OrganizationCode  , Direction = ParameterDirection.Input}
             };
            objResult = objRenegotiationRepository.UnitOfWork.ExecuteQuery<RenegotiationDTO>("PR_GetRenegotiationDetails @P_CardNumber,@P_RUT,@P_OrganizationCode", parameters).ToList();//, @TYPE 
            if (objResult == null)
            {
                objResult = new List<RenegotiationDTO>();
            }
            return objResult;
        }
        #endregion

        #endregion

        #region CreateDynamicBillingTables
        public void CreateDynamicBillingTables(string ABillingCycleCode)
        {

            if (ABillingCycleCode == null)
            {
                throw new ArgumentException("");
            }

            try
            {

                SqlParameter[] paramCollection = new SqlParameter[]
                { 
                    new SqlParameter(parameterName: "@P_CycleCode",dbType:System.Data.SqlDbType.VarChar),
                };

                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CycleCode")).First()).Value = ABillingCycleCode;

                objRefinancingRepository.UnitOfWork.ExecuteCommand("PR_PrepareDynamicBillingTable @P_CycleCode", paramCollection);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void ImportFromExcel(string filepath)
        {
            string ssqlconnectionstring = Utils.BuildConnectionString();
            string ssqltable = "[TR_TransactionPulling]";
            string sclearsql = "delete from " + ssqltable;

            // Read the workbook first (the legacy code emptied the table before
            // opening the file, so an unreadable upload left it empty).
            DataTable pulledTransactions = TransactionPullingWorkbookReader.Read(filepath);

            using (SqlConnection sqlconn = new SqlConnection(ssqlconnectionstring))
            {
                sqlconn.Open();
                using (SqlCommand sqlcmd = new SqlCommand(sclearsql, sqlconn))
                {
                    sqlcmd.ExecuteNonQuery();
                }
            }

            using (SqlBulkCopy sqlBulk = new SqlBulkCopy(ssqlconnectionstring))
            {
                sqlBulk.DestinationTableName = ssqltable;
                sqlBulk.WriteToServer(pulledTransactions);
            }
        }

        public void ExecMovementtoOverdue(string Acardnumber, string Aloginname)
        {
            if (Acardnumber == null)
            {
                throw new ArgumentException("");
            }



            SqlParameter[] paramCollection = new SqlParameter[]
                { 
                    new SqlParameter(parameterName: "@pcardnumber",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@p_loginname",dbType:System.Data.SqlDbType.VarChar),
                };

            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@pcardnumber")).First()).Value = Acardnumber;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@p_loginname")).First()).Value = Aloginname;

            objRefinancingRepository.UnitOfWork.ExecuteCommand("pr_movementtoOverdue @pcardnumber, @p_loginname", paramCollection);


        }


        // Movement To Insurance
        public void ExecMovementtoInsurance(string Acardnumber, string Aloginname)
        {
            if (Acardnumber == null)
            {
                throw new ArgumentException("");
            }



            SqlParameter[] paramCollection = new SqlParameter[]
                { 
                    new SqlParameter(parameterName: "@pcardnumber",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@p_loginname",dbType:System.Data.SqlDbType.VarChar),
                };

            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@pcardnumber")).First()).Value = Acardnumber;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@p_loginname")).First()).Value = Aloginname;

            objRefinancingRepository.UnitOfWork.ExecuteCommand("PR_movementtoInsurance @pcardnumber, @p_loginname", paramCollection);


        }


        public void ExecMovementtoNormal(string Acardnumber, string Aloginname)
        {
            if (Acardnumber == null)
            {
                throw new ArgumentException("");
            }

            SqlParameter[] paramCollection = new SqlParameter[]
                { 
                    new SqlParameter(parameterName: "@pcardnumber",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@p_loginname",dbType:System.Data.SqlDbType.VarChar),
                };

            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@pcardnumber")).First()).Value = Acardnumber;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@p_loginname")).First()).Value = Aloginname;

            objRefinancingRepository.UnitOfWork.ExecuteCommand("PR_MovementtoNormal @pcardnumber, @p_loginname", paramCollection);
        }


        public void ExecBillingProcedureTestingProcs(DateTime ABillDate, string AType)
        {
            if (AType == null)
            {
                throw new ArgumentException("");
            }

            try
            {

                SqlParameter[] paramCollection = new SqlParameter[]
                { 
                    new SqlParameter(parameterName: "@P_BillDate",dbType:System.Data.SqlDbType.DateTime),
                    new SqlParameter(parameterName: "@P_Type",dbType:System.Data.SqlDbType.VarChar),
                };

                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_BillDate")).First()).Value = ABillDate;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Type")).First()).Value = AType;

                objRefinancingRepository.UnitOfWork.ExecuteCommand("PR_BillingTestingProcExec @P_BillDate, @P_Type", paramCollection);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region BillingCycleSummaryInsertion
        public void BillingCycleSummaryInsertion(string ACardNumber)
        {
            if (ACardNumber == null)
                throw new ArgumentException("");
            try
            {

                SqlParameter[] paramCollection = new SqlParameter[]
                { 
                    new SqlParameter(parameterName: "@P_Cardnumber",dbType:System.Data.SqlDbType.VarChar),
                };

                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Cardnumber")).First()).Value = ACardNumber;

                objRefinancingRepository.UnitOfWork.ExecuteCommand("PR_BillingCycleSummaryInsertion @P_Cardnumber", paramCollection);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Statement Generation

        #region FindAll
        public List<StatementGenerationDTO> FindAll(int APageNo, StatementGenerationDTO AStatementGenerationDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = AStatementGenerationDTO.CardNumber, Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "RUT", FilterValue = AStatementGenerationDTO.RUT == null ? string.Empty : AStatementGenerationDTO.RUT.Replace("-",string.Empty), Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "BillMonth", FilterValue = AStatementGenerationDTO.BillMonth, Operator = "Equals" },
                new Utils.ExpressionList(){FilterColumn = "BillYear", FilterValue = AStatementGenerationDTO.BillYear, Operator = "Equals"}
            };

            Expression<Func<StatementGeneration, bool>> expression = Utils.GetExpression<StatementGeneration>(lstExp);
            int iRecordCount = objStatementGenerationRepository.GetRecordCount(expression);

            var vStatementGenerationDTO = objStatementGenerationRepository.GetFiltered(0, iRecordCount, expression).OrderBy(s => s.CardNumber).OrderByDescending(s => s.BillDate);

            if (vStatementGenerationDTO != null && vStatementGenerationDTO.Any())
            {
                List<StatementGenerationDTO> lstStatementGenerationDTO = new List<StatementGenerationDTO>();
                foreach (var StatementGeneration in vStatementGenerationDTO)
                {
                    lstStatementGenerationDTO.Add(Conversion.Mapping.StatementGenerationToStatementGenerationDTO(StatementGeneration));
                }
                return lstStatementGenerationDTO;
            }
            else
                return new List<StatementGenerationDTO>();
        }
        #endregion
        #region FindStatementGeneration
        public List<StatementGenerationDTO> FindStatementGeneration(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindRefinancing");

            int iRecordCount = objStatementGenerationRepository.GetRecordCount();
            var Statements = objStatementGenerationRepository.GetAll().OrderBy(s => s.CardNumber).OrderByDescending(s => s.BillDate); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);
            if (Statements != null && Statements.Any())
            {
                List<StatementGenerationDTO> lstStatementGenerationDTO = new List<StatementGenerationDTO>();
                foreach (var Statement in Statements)
                {
                    lstStatementGenerationDTO.Add(Conversion.Mapping.StatementGenerationToStatementGenerationDTO(Statement));
                }
                return lstStatementGenerationDTO;
            }
            else
                return new List<StatementGenerationDTO>();

        }
        #endregion
        #region FindByStatementGenerationCardNo
        public StatementGenerationDTO FindByStatementGenerationCardNo(string ACardNo)
        {
            StatementGeneration StatementGenerationDTO = objStatementGenerationRepository.Get(ACardNo);
            if (StatementGenerationDTO != null)
            {

                objStatementGenerationRepository.UnitOfWork.RefreshDataInModel(ref StatementGenerationDTO);
                return Conversion.Mapping.StatementGenerationToStatementGenerationDTO(StatementGenerationDTO);
            }
            else
            {
                return new StatementGenerationDTO();
            }
        }
        #endregion


        public string GenerateReport(string Cardnumber, string path)
        {

            IReportGenerator objRpt = objReportGenerator;

            string str = path;

            return objRpt.PrintReportWithParamNewWindow("SampleReport", "@P_CardNumber",//,@P_BRANCHCODE,@P_DOCTYPE,@P_VENDORCODE,@P_FROM,@P_TO,@P_COM_FROM,@P_COM_TO,@P_LOGINNAME,@P_MODE",
                        Cardnumber, //entity + "," + branch + "," + doctype + "," + Vendor + "," + dtInvFrom + "," + dtInvTo + "," + dtInvCompFrom + "," + dtInvCompTo + "," + loginname + "," + mode,
                        str, "test");

        }

        public void UpdateStatementGenerationFileName(string ACardNumber, int? ABillMonth, int? ABillYear, string AFileName)
        {
            if (string.IsNullOrEmpty(ACardNumber) || string.IsNullOrEmpty(AFileName))
                throw new ArgumentException("");
            try
            {

                SqlParameter[] paramCollection = new SqlParameter[]
                { 
                    new SqlParameter(parameterName: "@P_CardNumber",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_BillMonth",dbType:System.Data.SqlDbType.Int),
                    new SqlParameter(parameterName: "@P_BillYear",dbType:System.Data.SqlDbType.Int),
                    new SqlParameter(parameterName: "@P_FileName",dbType:System.Data.SqlDbType.VarChar),
                };

                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CardNumber")).First()).Value = ACardNumber;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_BillMonth")).First()).Value = ABillMonth;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_BillYear")).First()).Value = ABillYear;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_FileName")).First()).Value = AFileName;

                objStatementGenerationRepository.UnitOfWork.ExecuteCommand("PR_UpdateStatementGenerationFileName @P_CardNumber, @P_BillMonth, @P_BillYear,  @P_FileName ", paramCollection);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<GeneratedStatementDTO> GetGeneratedStatementList(string ACardNumber, string ARUT, string ABillMonth, string ABillYear)
        {
            List<GeneratedStatementDTO> objResult;
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter() { ParameterName = "@P_CardNumber", SqlDbType = SqlDbType.VarChar, Size = 16, Value = (ACardNumber == null ? string.Empty : ACardNumber), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_RUT", SqlDbType = SqlDbType.VarChar, Size = 11, Value = (ARUT == null ? string.Empty : ARUT.Replace("-",string.Empty)), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_BillMonth", SqlDbType = SqlDbType.VarChar, Size = 2, Value = (ABillMonth == null ? string.Empty : ABillMonth), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_BillYear", SqlDbType = SqlDbType.VarChar, Size = 4, Value = (ABillYear == null ? string.Empty : ABillYear), Direction = ParameterDirection.Input},
             };
            objResult = objStatementGenerationRepository.UnitOfWork.ExecuteQuery<GeneratedStatementDTO>("PR_GetGeneratedStatementList @P_CardNumber, @P_RUT, @P_BillMonth, @P_BillYear", parameters).ToList();
            if (objResult == null)
            {
                objResult = new List<GeneratedStatementDTO>();
            }
            return objResult;
        }


        #endregion

        #region GetParentCustomersForTransaction
        public List<RUTCardNumberSearchDTO> GetParentCustomersForTransaction(string ARUT, string ACardNumber, string ACustomerName)
        {
            List<RUTCardNumberSearchDTO> objRUTCardNumberSearchDTO = new List<RUTCardNumberSearchDTO>();

            SqlParameter[] objSqlParameter = new SqlParameter[] { 
                new SqlParameter(){ ParameterName = "OrganizationCode", Value = Utils.OrganizationCode, SqlDbType = System.Data.SqlDbType.VarChar },
                new SqlParameter(){ ParameterName = "RUT", Value = ARUT == null ? "" : ARUT.Replace("-", "") , SqlDbType = System.Data.SqlDbType.VarChar },
                new SqlParameter(){ ParameterName ="CardNumber", Value = ACardNumber, SqlDbType = System.Data.SqlDbType.VarChar },
                new SqlParameter(){ ParameterName = "CustomerName", Value = ACustomerName, SqlDbType = System.Data.SqlDbType.VarChar }
            };

            objRUTCardNumberSearchDTO = objOnlinePaymentRepository.UnitOfWork.ExecuteQuery<RUTCardNumberSearchDTO>("PR_GetParentCustomersForTransaction @OrganizationCode,@RUT,@CardNumber,@CustomerName", objSqlParameter).ToList();
            return objRUTCardNumberSearchDTO;
        }
        #endregion

    }
}
