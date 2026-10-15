using CC_Ingenio.Common.Utils;
using CC_Ingenio.Core.MasterRepo.MasterAggregate;
using CC_Ingenio.Core.UserRepo.UserAggregate;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.Repository.MasterRepo;
using CC_Ingenio.Repository.UserRepo;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate;
using CC_Ingenio.DataTranfer.CardBookingInfo;
using CC_Ingenio.Repository.CardBookingRepo;
using System.Transactions;

namespace CC_Ingenio.Manager
{
    public class CardBookingManager : ICardBookingManager
    {
        #region Variables
        CustomerRepository objCustomerRepository;
        UserAllowedBranchRepository objUserAllowedBranchRepository;
        CardMasterRepository objCardMasterRepository;
        CardEmbossingRepository objCardEmbossingRepository;

        OrganizationRepository objOrganizationRepository;
        ProductRepository objProductRepository;
        CustomerSegmentRepository objCustomerSegmentRepository;
        CustomerSubSegmentRepository objCustomerSubSegmentRepository;
        PaymentHolidayRepository objPaymentHolidayRepository;
        #endregion

        #region Constructor
        public CardBookingManager(CustomerRepository ACustomerRepository, UserAllowedBranchRepository AUserAllowedBranchRepository,
            CardMasterRepository ACardMasterRepository, CardEmbossingRepository ACardEmbossingRepository,
            OrganizationRepository AOrganizationRepository, ProductRepository AProductRepository,
            CustomerSegmentRepository ACustomerSegmentRepository, CustomerSubSegmentRepository ACustomerSubSegmentRepository, PaymentHolidayRepository APaymentHolidayRepository)
        {
            if (ACustomerRepository == null)
                throw new ArgumentNullException("ACustomerRepository");
            if (AUserAllowedBranchRepository == null)
                throw new ArgumentNullException("AUserAllowedBranchRepository");
            if (ACardMasterRepository == null)
                throw new ArgumentNullException("ACardMasterRepository");
            if (ACardEmbossingRepository == null)
                throw new ArgumentNullException("ACardEmbossingRepository");
            if (AOrganizationRepository == null)
                throw new ArgumentNullException("AOrganizationRepository");
            if (AProductRepository == null)
                throw new ArgumentNullException("AProductRepository");
            if (ACustomerSegmentRepository == null)
                throw new ArgumentNullException("ACustomerSegmentRepository");
            if (ACustomerSubSegmentRepository == null)
                throw new ArgumentNullException("ACustomerSubSegmentRepository");
            if (APaymentHolidayRepository == null)
                throw new ArgumentNullException("APaymentHolidayRepository");

            objCustomerRepository = ACustomerRepository;
            objUserAllowedBranchRepository = AUserAllowedBranchRepository;
            objCardMasterRepository = ACardMasterRepository;
            objCardEmbossingRepository = ACardEmbossingRepository;

            objOrganizationRepository = AOrganizationRepository;
            objProductRepository = AProductRepository;
            objCustomerSegmentRepository = ACustomerSegmentRepository;
            objCustomerSubSegmentRepository = ACustomerSubSegmentRepository;
            objPaymentHolidayRepository = APaymentHolidayRepository;
        }
        #endregion

        #region CardMaster

        #region FindCustomerForCardGeneration
        public List<CustomerDTO> FindCustomerForCardGeneration(int APageNo, CustomerDTO ACustomerDTO)
        {
            
            List<Utils.ExpressionList> expressionList = new List<Utils.ExpressionList>() { 
                new Utils.ExpressionList(){ FilterColumn = "RUT", FilterValue = ACustomerDTO.RUT, Operator = "Contains" },
                new Utils.ExpressionList(){ FilterColumn = "BranchCode", FilterValue = ACustomerDTO.BranchCode, Operator = "Contains"},
                new Utils.ExpressionList(){ FilterColumn = "IsActive", FilterValue = true, Operator = "Equals"},
                new Utils.ExpressionList(){ FilterColumn = "IsBlackListed", FilterValue = false, Operator = "Equals"},
                new Utils.ExpressionList(){ FilterColumn = "IsCardGenerated", FilterValue = false, Operator = "Equals"},
                new Utils.ExpressionList(){ FilterColumn = "ApprovalStatus", FilterValue = "A", Operator = "Equals"},
                new Utils.ExpressionList(){ FilterColumn = "OrganizationCode", FilterValue = Utils.OrganizationCode, Operator = "Equals"},
            };

            Expression<Func<Customer, bool>> expression = Utils.GetExpression<Customer>(expressionList);
            int recordCount = objCustomerRepository.GetRecordCount(expression);

            var vTransactionDTO = objCustomerRepository.GetFiltered(APageNo, recordCount, expression);
            if (vTransactionDTO != null && vTransactionDTO.Any())
            {
                List<CustomerDTO> lstCustomerDTO = new List<CustomerDTO>();
                foreach (var item in vTransactionDTO)
                {
                    lstCustomerDTO.Add(Conversion.Mapping.CustomerToCustomerDTO(item));
                }
                return lstCustomerDTO;
            }
            else
            {
                return new List<CustomerDTO>();
            }
        } 
        
        #endregion

        #region ListCustomers
        public List<CustomerDTO> ListCustomers(string AUserName)
        {
            var customerList = objCustomerRepository.GetAll().AsEnumerable()

                .Join(objUserAllowedBranchRepository.GetAll(),
                    customer => customer.BranchCode,
                    userBranch => userBranch.BranchCode,
                    (customer, userBranch) => new { customer, userBranch.Username })

                .Where(x => x.Username.ToLower() == AUserName.ToLower() 
                    && !x.customer.IsBlackListed 
                    && x.customer.IsActive == true
                    && x.customer.ApprovalStatus.Equals("A") 
                    && !x.customer.IsCardGenerated
                    && x.customer.OrganizationCode == Utils.OrganizationCode)
                
                .Select(x => x.customer);

            if (customerList != null && customerList.Any())
            {
                List<CustomerDTO> customers = new List<CustomerDTO>();
                customerList.ToList().ForEach(customer => customers.Add(Conversion.Mapping.CustomerToCustomerDTO(customer)));
                return customers;
            }
            else
            {
                return new List<CustomerDTO>();
            }
        } 
        #endregion

        #region PopulateCardMaster
        public int PopulateCardMaster(DataTable ASelectedCustomers,string AUserName)
        {
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter(){ ParameterName="@P_Customers", SqlDbType= SqlDbType.Structured, Value = ASelectedCustomers, TypeName = "UD_General_TableType", Direction = ParameterDirection.Input},
                new SqlParameter(){ ParameterName="@P_CreatedBy", SqlDbType = SqlDbType.VarChar, Size = 15, Value = AUserName, Direction = ParameterDirection.Input}
            };
            return objCardMasterRepository.UnitOfWork.ExecuteCommand("PR_MtCardMasterSave @P_Customers,@P_CreatedBy", parameters);
        } 
        #endregion

        #region FindCardMaster
        public List<CardMasterDTO> FindCardMaster(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindCardMaster");

            int iRecordCount = objCardMasterRepository.GetRecordCount();
            var cardmasters = objCardMasterRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);
            if (cardmasters != null && cardmasters.Any())
            {
                List<CardMasterDTO> lstCardMasterDTO = new List<CardMasterDTO>();
                foreach (var cardmaster in cardmasters)
                {
                    lstCardMasterDTO.Add(Conversion.Mapping.CardMasterToCardMasterDTO(cardmaster));
                }
                return lstCardMasterDTO;
            }
            else
                return new List<CardMasterDTO>();
        } 
        #endregion

        #region DeleteCardMaster
        public void DeleteCardMaster(string ACardNumber)
        {
            var cardmaster = objCardMasterRepository.Get(ACardNumber);

            if (cardmaster != null)
            {
                objCardMasterRepository.Remove(cardmaster);
                objCardMasterRepository.UnitOfWork.Commit();
            }
        } 
        #endregion

        #region FindByCardMasterCardNumber
        public CardMasterDTO FindByCardMasterCardNumber(string ACardNumber)
        {
            var cardmaster = objCardMasterRepository.Get(ACardNumber);
            if (cardmaster != null)
            {
                objCardMasterRepository.UnitOfWork.RefreshDataInModel(ref cardmaster);
                if (cardmaster.objProduct == null)
                {
                    cardmaster.objProduct = objProductRepository.Get(cardmaster.OrganizationCode, cardmaster.ProductCode);
                }
                            
                if (cardmaster.objCustomerSegment == null)
                {
                    cardmaster.objCustomerSegment = objCustomerSegmentRepository.Get(cardmaster.CustomerSegmentCode, cardmaster.OrganizationCode);
                }
                if (cardmaster.CustomerSubSegmentCode == null)
                {
                    cardmaster.objCustomerSubSegment = objCustomerSubSegmentRepository.Get(cardmaster.CustomerSubSegmentCode, cardmaster.OrganizationCode);
                }
                if (cardmaster.objOrganization == null)
                {
                    cardmaster.objOrganization = objOrganizationRepository.Get(cardmaster.OrganizationCode);
                }    
               
                return Conversion.Mapping.CardMasterToCardMasterDTO(cardmaster);
            }
            else
            {
                return new CardMasterDTO();
            }
        } 
        #endregion

        #region UpdateCardMasterDetails
        public void UpdateCardMasterDetails(string ACardNumber, CardMasterDTO ACardMasterDTO)
        {
            if (ACardMasterDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");

            }
            CardMaster currentCardMaster = objCardMasterRepository.Get(ACardNumber);
            CardMaster updatedCardMaster = new CardMaster()
            {
                OrganizationCode = currentCardMaster.OrganizationCode,
                CustomerId = currentCardMaster.CustomerId,
                RUT = currentCardMaster.RUT,
                CardNumber = currentCardMaster.CardNumber,
                ProductCode = currentCardMaster.ProductCode,
                ValidFromDate = currentCardMaster.ValidFromDate,
                CardExpiryDate = currentCardMaster.CardExpiryDate,
                CustomerCardName = currentCardMaster.CustomerCardName,
                CountryCode = currentCardMaster.CountryCode,
                Gender = currentCardMaster.Gender,
                AddressTypeCode = currentCardMaster.AddressTypeCode,
                IsPrinted = currentCardMaster.IsPrinted,

                AccountNo = ACardMasterDTO.AccountNo,
                CardTypeCode = ACardMasterDTO.CardTypeCode,
                CardStatusCode = ACardMasterDTO.CardStatusCode,
                CashOTB = ACardMasterDTO.CashLimit,
                CreditOTB = ACardMasterDTO.CreditLimit,
                DailyCreditLimit = ACardMasterDTO.DailyCreditLimit,
                WeeklyCreditLimit = ACardMasterDTO.WeeklyCreditLimit,
                DailyCashLimit = ACardMasterDTO.DailyCashLimit,
                WeeklyCashLimit = ACardMasterDTO.WeeklyCashLimit,
                RenewalFeeWaiver = ACardMasterDTO.RenewalFeeWaiver,
                IsInterestWaiver = ACardMasterDTO.IsInterestWaiver,
                IsAutoRenewal = ACardMasterDTO.IsAutoRenewal,
                CustomerSegmentCode = ACardMasterDTO.CustomerSegmentCode,
                CustomerSubSegmentCode = ACardMasterDTO.CustomerSubSegmentCode,
                IsVIP = ACardMasterDTO.IsVIP,
                IsCardRenewable = ACardMasterDTO.IsCardRenewable,
                IsCardRenewed = ACardMasterDTO.IsCardRenewed,
                CardMemberType = ACardMasterDTO.CardMemberType,

                CustomerRefinanceCount  = ACardMasterDTO.CustomerRefinanceCount,
	            MaxPermitRefinanceCount  = ACardMasterDTO.MaxPermitRefinanceCount,
	            MaxPermitRenegoCount  = ACardMasterDTO.MaxPermitRenegoCount,
	            MaxDueRenegoCount  = ACardMasterDTO.MaxDueRenegoCount,
	            CustomerDueRenegoCount  = ACardMasterDTO.CustomerDueRenegoCount,
	            MaxOverDueRenegoCount  = ACardMasterDTO.MaxOverDueRenegoCount,
	            CustomerOverDueRenegoCount   = ACardMasterDTO.CustomerOverDueRenegoCount,
	            MaxWriteOffRenegoCount  = ACardMasterDTO.MaxWriteOffRenegoCount,
                CustomerWriteOffRenegoCount = ACardMasterDTO.CustomerWriteOffRenegoCount,
                CreditLimit = ACardMasterDTO.CreditLimit,
                CashLimit = ACardMasterDTO.CashLimit,

                CreatedBy = currentCardMaster.CreatedBy,
                CreatedOn = currentCardMaster.CreatedOn,
                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = ACardMasterDTO.LastUpdatedBy,
                ApprovalStatus = ACardMasterDTO.ApprovalStatus,
                ApprovedOn = null,
            };
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
               new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objCardMasterRepository.Merge(currentCardMaster, updatedCardMaster);
                    objCardMasterRepository.UnitOfWork.Commit();
                    if (ACardMasterDTO.ApprovalStatus == "P")
                    {
                        
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = CommonManager.GetApprovalQueueProcedureParameters("MT_CardMaster", " CardNumber = '" + ACardMasterDTO.CardNumber + "'",
                            "CRDMSTR", ACardMasterDTO.LastUpdatedBy, "/CardBooking/CardMasterAuthorize/" + ACardMasterDTO.CardNumber + "/A?", "MT");

                        objCardMasterRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        } 
        #endregion

        #region FindAll
        public List<CardMasterDTO> FindAll(int APageNo, CardMasterDTO ACardMasterDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                 new Utils.ExpressionList() { FilterColumn = "RUT", FilterValue = ACardMasterDTO.RUT, Operator = "Contains"},
                 new Utils.ExpressionList() { FilterColumn = "CardNumber", FilterValue = ACardMasterDTO.CardNumber, Operator = "Contains"},              
                 new Utils.ExpressionList() { FilterColumn = "ProductCode", FilterValue = ACardMasterDTO.ProductCode, Operator = ACardMasterDTO.ProductCode == string.Empty ? "Contains" : "Equals"},
                 new Utils.ExpressionList() { FilterColumn = "CustomerSegmentCode", FilterValue = ACardMasterDTO.CustomerSegmentCode, Operator = ACardMasterDTO.CustomerSegmentCode == string.Empty ? "Contains" : "Equals"},
                 new Utils.ExpressionList() { FilterColumn = "CustomerSubSegmentCode", FilterValue = ACardMasterDTO.CustomerSubSegmentCode, Operator = ACardMasterDTO.CustomerSubSegmentCode == string.Empty ? "Contains" : "Equals"},
                 new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode,Operator = "Equals"},
                 new Utils.ExpressionList() { FilterColumn = "IsBlocked",FilterValue = false, Operator = "Equals"},
                 new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                 new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
            };

            Expression<Func<CardMaster, bool>> expression = Utils.GetExpression<CardMaster>(lstExp);
            int iRecordCount = objCardMasterRepository.GetRecordCount(expression);

            var vCardMasterDTO = objCardMasterRepository.GetFiltered(0, iRecordCount, expression);

            if (vCardMasterDTO != null && vCardMasterDTO.Any())
            {
                List<CardMasterDTO> lstCardMasterDTO = new List<CardMasterDTO>();
                foreach (var cardmaster in vCardMasterDTO)
                {
                    lstCardMasterDTO.Add(Conversion.Mapping.CardMasterToCardMasterDTO(cardmaster));
                }
                return lstCardMasterDTO;
            }
            else
                return new List<CardMasterDTO>();
        } 
        #endregion

        #endregion

        #region PaymentHoliday

        #region GetPaymentHolidayIDForApproval
        public PaymentHolidayDTO GetPaymentHolidayIDForApproval(int ASlNo)
        {
            PaymentHolidayDTO objPaymentHolidayDTO = new PaymentHolidayDTO();

            if (objPaymentHolidayRepository.Get(ASlNo) != null)
            {
                var result = objPaymentHolidayRepository.AllMatching(x => x.SlNo == ASlNo);
                if (result.Any())
                {
                    objPaymentHolidayDTO.SlNo = ASlNo;
                    objPaymentHolidayDTO.CardNumber = result.FirstOrDefault().CardNumber;
                    objPaymentHolidayDTO.ProductCode = result.FirstOrDefault().ProductCode;
                    objPaymentHolidayDTO.RUT = result.FirstOrDefault().RUT;
                    objPaymentHolidayDTO.FromDate = result.FirstOrDefault().FromDate;
                    objPaymentHolidayDTO.ToDate = result.FirstOrDefault().ToDate;
                    objPaymentHolidayDTO.ApprovalStatus = result.FirstOrDefault().ApprovalStatus;
                    objPaymentHolidayDTO.ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(objPaymentHolidayDTO.ApprovalStatus);
                }
            }
            return objPaymentHolidayDTO;
        } 
        #endregion

        #region FindPaymentHolidayById
        public PaymentHolidayDTO FindPaymentHolidayById(int ASlNo)
        {
            PaymentHoliday paymentholiday = objPaymentHolidayRepository.Get(ASlNo);
            if (paymentholiday != null)
            {
                if (paymentholiday.objProduct == null)
                {
                    paymentholiday.objProduct = objProductRepository.Get(paymentholiday.OrganizationCode,paymentholiday.ProductCode);
                }

                objPaymentHolidayRepository.UnitOfWork.RefreshDataInModel(ref paymentholiday);
                return Conversion.Mapping.PaymentHolidayToPaymentHolidayDTO(paymentholiday);
            }
            else
            {
                return new PaymentHolidayDTO();
            }
        }
        #endregion

        #region SavePaymentHolidayDetails
        public void SavePaymentHolidayDetails(PaymentHolidayDTO APaymentHolidayDTO)
        {
            if (APaymentHolidayDTO == null)
            {
                throw new ArgumentException("");
            }

            var newPaymentHoliday = PaymentHolidayFactory.CreatePaymentHoliday(APaymentHolidayDTO.CardNumber, APaymentHolidayDTO.ProductCode, APaymentHolidayDTO.RUT,
                 APaymentHolidayDTO.FromDate, APaymentHolidayDTO.ToDate, APaymentHolidayDTO.ApprovalStatus,
                APaymentHolidayDTO.CreatedBy, APaymentHolidayDTO.LastUpdatedBy, APaymentHolidayDTO.OrganizationCode, APaymentHolidayDTO.ApprovedBy);
            try
            {

                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                         new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objPaymentHolidayRepository.Add(newPaymentHoliday);
                        objPaymentHolidayRepository.UnitOfWork.Commit();

                        if (APaymentHolidayDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = CommonManager.GetApprovalQueueProcedureParameters("MT_PaymentHoliday", " SlNo = '" + newPaymentHoliday.SlNo + "'",
                                "PYMTH", APaymentHolidayDTO.LastUpdatedBy, "/CardBooking/PaymentHolidayAuthorize/" + newPaymentHoliday.SlNo + "/A?", "MT");

                            objCardMasterRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                scope.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        } 
        #endregion

        #region UpdatePaymentHolidayDetails
        public void UpdatePaymentHolidayDetails(string Id,string ACardNumber, PaymentHolidayDTO APaymentHolidayDTO)
        {
            if (APaymentHolidayDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");

            }
            PaymentHoliday currentPaymentHoliday = objPaymentHolidayRepository.Get(Convert.ToInt32(Id));
            PaymentHoliday updatedPaymentHoliday = new PaymentHoliday()
            {
                SlNo = Convert.ToInt32(Id),
                CardNumber = APaymentHolidayDTO.CardNumber,
                ProductCode = APaymentHolidayDTO.ProductCode,
                RUT=APaymentHolidayDTO.RUT,
                FromDate = APaymentHolidayDTO.FromDate,
                ToDate = APaymentHolidayDTO.ToDate,
                OrganizationCode = APaymentHolidayDTO.OrganizationCode,

                CreatedBy = currentPaymentHoliday.CreatedBy,
                CreatedOn = currentPaymentHoliday.CreatedOn,
                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = APaymentHolidayDTO.LastUpdatedBy,
                ApprovalStatus = APaymentHolidayDTO.ApprovalStatus,
            };
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
               new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {

                    objPaymentHolidayRepository.Merge(currentPaymentHoliday, updatedPaymentHoliday);
                    objPaymentHolidayRepository.UnitOfWork.Commit();
                    if (APaymentHolidayDTO.ApprovalStatus == "P")
                    {

                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = CommonManager.GetApprovalQueueProcedureParameters("MT_PaymentHoliday", " SlNo = '" + APaymentHolidayDTO.SlNo + "'",
                            "PYMTH", APaymentHolidayDTO.LastUpdatedBy, "/CardBooking/PaymentHolidayAuthorize/" + APaymentHolidayDTO.SlNo + "/A?", "MT");
                                                                     
                        objCardMasterRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region DeletePaymentHoliday
        public void DeletePaymentHoliday(string Id, string ACardNumber)
        {
            var paymentholiday = objPaymentHolidayRepository.Get(Convert.ToInt32(Id));

            if (paymentholiday != null)
            {
                objPaymentHolidayRepository.Remove(paymentholiday);
                objPaymentHolidayRepository.UnitOfWork.Commit();
            }
        }
        #endregion

        #region GetPaymentHolidayDetails
        public List<PaymentHolidayDTO> GetPaymentHolidayDetails(string ACardNumber,string AProductCode,string ARUT, DateTime? AFromDate, DateTime? AToDate)
        {
            List<PaymentHolidayDTO> objResult;
            SqlParameter[] parameterss = new SqlParameter[] { 
                new SqlParameter() { ParameterName = "@P_CardNumber", SqlDbType = SqlDbType.VarChar, Size = 15, Value = (ACardNumber == null ? string.Empty : ACardNumber), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_ProductCode", SqlDbType = SqlDbType.VarChar, Size = 5, Value = (AProductCode == null ? string.Empty : AProductCode), Direction = ParameterDirection.Input},
                 new SqlParameter() { ParameterName = "@P_RUT", SqlDbType = SqlDbType.VarChar, Size = 25, Value = (ARUT == null ? string.Empty : ARUT), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_FromDate", SqlDbType = SqlDbType.DateTime, Value = ((AFromDate ==  null) || (AFromDate ==  DateTime.MinValue)) ? Convert.ToDateTime("1900-01-01") : AFromDate, Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_ToDate", SqlDbType = SqlDbType.DateTime, Value = ((AToDate ==  null) || (AToDate ==  DateTime.MinValue)) ? Convert.ToDateTime("9999-09-09") : AToDate, Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_OrganizationCode", SqlDbType = SqlDbType.VarChar, Size = 5, Value = Utils.OrganizationCode, Direction = ParameterDirection.Input}
            };
            objResult = objPaymentHolidayRepository.UnitOfWork.ExecuteQuery<PaymentHolidayDTO>("PR_GetAllPaymentHolidayDetails @P_CardNumber, @P_FromDate, @P_ToDate, @P_OrganizationCode,@P_ProductCode,@P_RUT", parameterss).ToList();
            objResult.ForEach(x => x.ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(x.ApprovalStatus));           
            if (objResult == null)
            {
                objResult = new List<PaymentHolidayDTO>();
            }
            return objResult;
        }
        #endregion

        #endregion

        #region PopulatePaymentAllocationType
        public int PopulatePaymentAllocationType(DataTable ASelectedCustomers, string AUserName)
        {
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter(){ ParameterName="@P_Customers", SqlDbType= SqlDbType.Structured, Value = ASelectedCustomers, TypeName = "UD_General_TableType", Direction = ParameterDirection.Input},
                new SqlParameter(){ ParameterName="@P_CreatedBy", SqlDbType = SqlDbType.VarChar, Size = 15, Value = AUserName, Direction = ParameterDirection.Input},
                new SqlParameter(){ ParameterName="@P_OrganizationCode", SqlDbType = SqlDbType.VarChar, Size = 5, Value = Utils.OrganizationCode, Direction = ParameterDirection.Input}
            };
            return objCardMasterRepository.UnitOfWork.ExecuteCommand("PR_MtPaymentAllocationTypeSave @P_Customers,@P_CreatedBy, @P_OrganizationCode", parameters);
        }
        #endregion

        #region CardEmbossing

    public string GenerateReport(string Cardnumber,string path)
    {

        CC_Ingenio.Common.Utils.ReportGenerator objRpt = new CC_Ingenio.Common.Utils.ReportGenerator();
        //ReportDocument repDoc = new ReportDocument();
        //string str = Server.MapPath("~/Reports/");

        string str = path;

        return objRpt.PrintReportWithParamNewWindow("SampleReport", "@P_CardNumber",//,@P_BRANCHCODE,@P_DOCTYPE,@P_VENDORCODE,@P_FROM,@P_TO,@P_COM_FROM,@P_COM_TO,@P_LOGINNAME,@P_MODE",
                    Cardnumber, //entity + "," + branch + "," + doctype + "," + Vendor + "," + dtInvFrom + "," + dtInvTo + "," + dtInvCompFrom + "," + dtInvCompTo + "," + loginname + "," + mode,
                    str, "test");
                    //ConfigurationSettings.AppSettings["Server"].ToString(),
                    //ConfigurationSettings.AppSettings["DATABASE"].ToString());


    }

    public bool CardEmbossingData( DataTable dtType,string AUserName)
    {
        try
        {

            SqlParameter[] parameters = new SqlParameter[] { 
            new SqlParameter(){ ParameterName="@P_CardNumber", SqlDbType= SqlDbType.Structured, Value = dtType, TypeName = "UD_CardNumberFlag", Direction = ParameterDirection.Input},
                new SqlParameter(){ ParameterName="@P_CreatedBy", SqlDbType = SqlDbType.VarChar, Size = 15, Value = AUserName, Direction = ParameterDirection.Input}
              
        };
            objCardEmbossingRepository.UnitOfWork.ExecuteCommand("PR_UpdateCardNoPrinted @P_CardNumber,@P_CreatedBy", parameters);
            return true;
        }
        catch { return false; }
           

           
    }                

       

        public CardEmbossingDTO FindCardNumberByRut(string ARutNo)
        {
            CardEmbossingDTO objCustomerDTO = new CardEmbossingDTO();
            try
            {
                List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                { new Utils.ExpressionList() { FilterColumn = "RUT",FilterValue = ARutNo, Operator = "Equals" }                    
                };

                Expression<Func<CardEmbossing, bool>> expression = Utils.GetExpression<CardEmbossing>(lstExpression);


                var vTransactionDTO = objCardEmbossingRepository.GetFiltered(0, 1, expression);
                if (vTransactionDTO.Count() != 0)
                {
                    objCustomerDTO = Conversion.Mapping.CardEmbossingToCardEmbossingDTO(vTransactionDTO.SingleOrDefault());
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }



            return objCustomerDTO;

        }

        #region GetEmbossedDetails
        public List<CardEmbossedDTO> GetEmbossedDetails(string ARUT, string ACardNumber, string ACustomerName)
        {
            List<CardEmbossedDTO> objResult;
            SqlParameter[] parameterss = new SqlParameter[] { 
            new SqlParameter() { ParameterName = "@P_CardNumber", SqlDbType = SqlDbType.VarChar, Size = 15, Value = (ACardNumber == null ? string.Empty : ACardNumber), Direction = ParameterDirection.Input},
            new SqlParameter() { ParameterName = "@P_RUT", SqlDbType = SqlDbType.VarChar, Size = 15, Value = (ARUT == null ?  string.Empty : ARUT), Direction = ParameterDirection.Input},
            new SqlParameter() { ParameterName = "@P_CustomerName", SqlDbType = SqlDbType.VarChar, Size = 50, Value = (ACustomerName == null ?  string.Empty : ACustomerName) , Direction = ParameterDirection.Input},
            new SqlParameter() { ParameterName = "@P_OrganizationCode", SqlDbType = SqlDbType.VarChar, Size = 5, Value = Utils.OrganizationCode , Direction = ParameterDirection.Input}
             };
            objResult = objCardEmbossingRepository.UnitOfWork.ExecuteQuery<CardEmbossedDTO>("PR_GetAllCardEmbossedDetails @P_CardNumber,@P_RUT,@P_CustomerName,@P_OrganizationCode", parameterss).ToList();

            if (objResult == null)
            {
                objResult = new List<CardEmbossedDTO>();
            }
            return objResult;
        }
        #endregion

        #region FindCustomerForCardGeneration
        public List<CardEmbossingDTO> FindCardnumberForEmbossing(int APageNo, CardEmbossingDTO ACardEmbossingDTO)
        {

            List<Utils.ExpressionList> expressionList = new List<Utils.ExpressionList>() { 
            new Utils.ExpressionList(){ FilterColumn = "RUT", FilterValue = ACardEmbossingDTO.RUT, Operator = "Contains" },
            new Utils.ExpressionList(){ FilterColumn = "Name", FilterValue = ACardEmbossingDTO.Name , Operator = "Contains"},
            new Utils.ExpressionList(){ FilterColumn = "CardNumber", FilterValue = ACardEmbossingDTO.CardNumber , Operator = "Contains"},
            new Utils.ExpressionList(){ FilterColumn = "OrganizationCode", FilterValue = Utils.OrganizationCode, Operator = "Equals"},
            //,new Utils.ExpressionList(){FilterColumn = "ApprovalStatus", FilterValue = "A", Operator = "Equals"}
        };

            Expression<Func<CardEmbossing, bool>> expression = Utils.GetExpression<CardEmbossing>(expressionList);
            int recordCount = objCardEmbossingRepository.GetRecordCount(expression);

            var vTransactionDTO = objCardEmbossingRepository.GetFiltered(APageNo, recordCount, expression);
            if (vTransactionDTO != null && vTransactionDTO.Any())
            {
                List<CardEmbossingDTO> lstCardEmbossingDTO = new List<CardEmbossingDTO>();
                foreach (var item in vTransactionDTO)
                {
                    lstCardEmbossingDTO.Add(Conversion.Mapping.CardEmbossingToCardEmbossingDTO(item));
                }
                return lstCardEmbossingDTO;
            }
            else
            {
                return new List<CardEmbossingDTO>();
            }
        }
        #endregion

        #region ListCustomers
        public List<CardEmbossingDTO> ListCardNumber(string AUserName)
        {
            var cardnumberList = objCardEmbossingRepository .GetAll().AsEnumerable()
                .Join(objUserAllowedBranchRepository.GetAll(),
                customer => customer.BranchCode,
                userBranch => userBranch.BranchCode,
                (customer, userBranch) => new { customer, userBranch.Username })
                .Where(x => x.Username == AUserName && x.customer.OrganizationCode == Utils.OrganizationCode ).Select(x => x.customer);//&& x.customer.ApprovalStatus.Equals("A"))
            if (cardnumberList != null && cardnumberList.Any())
            {
                List<CardEmbossingDTO> customers = new List<CardEmbossingDTO>();
                cardnumberList.ToList().ForEach(customer => customers.Add(Conversion.Mapping.CardEmbossingToCardEmbossingDTO(customer)));
                return customers;
            }
            else
            {
                return new List<CardEmbossingDTO>();
            }
        }
        #endregion
        #endregion 

        #region Implemented methods
        public List<CardEmbossingDTO> GetPendingCardEmbossingItems(int APageNo, CardEmbossingDTO ACardEmbossingFilter ,List<CardEmbossingDTO > AList)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
            new Utils.ExpressionList(){FilterColumn = "RUT", FilterValue = ACardEmbossingFilter.RUT, Operator = "Contains"},
            new Utils.ExpressionList(){FilterColumn = "Name", FilterValue = ACardEmbossingFilter.Name, Operator = "Contains"},
            new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = ACardEmbossingFilter.CardNumber, Operator = "Contains"}};

            Expression<Func<CardEmbossing, bool>> expression = Utils.GetExpression<CardEmbossing>(lstExp);
            var vEmbossingList = objCardEmbossingRepository .GetFiltered(APageNo, CC_Ingenio .Common .Utils .Utils .GRD_ROWS_PER_PAGE , expression);
            if (vEmbossingList != null && vEmbossingList.Any())
            {
                List<CardEmbossingDTO> lstCardEmbossingDTO = lstCardEmbossingDTO = new List<CardEmbossingDTO>();

                if (AList == null)
                {                    
                    foreach (var variable in vEmbossingList)
                    {
                        lstCardEmbossingDTO.Add(Conversion.Mapping.CardEmbossingToCardEmbossingDTO(variable));
                    }
                }
                else
                {
                    List<CardEmbossingDTO> cardEmbossingList = new List<CardEmbossingDTO>();
                    foreach (var variable in vEmbossingList)
                    {
                        cardEmbossingList.Add(Conversion.Mapping.CardEmbossingToCardEmbossingDTO(variable));
                    }

                    var newRecords = cardEmbossingList.Select(x => new { x.RUT, x.CardNumber }).Except(AList.Select(x => new { x.RUT, x.CardNumber }));
                    if (newRecords.Any())
                    {
                        foreach (var item in newRecords) 
                        {
                            AList.Add(cardEmbossingList.Where(x => x.RUT == item.RUT && x.CardNumber == item.CardNumber).FirstOrDefault());
                        }
                    }

                    lstCardEmbossingDTO = AList;
                }


                //if (AList == null)
                //{
                //    lstCardEmbossingDTO = new List<CardEmbossingDTO>();

                //    foreach (var variable in vEmbossingList)
                //    {
                //        lstCardEmbossingDTO.Add(Conversion.Mapping.CardEmbossingToCardEmbossingDTO(variable));
                //    }
                //}
                //else
                //    lstCardEmbossingDTO = AList;



                return lstCardEmbossingDTO;
            }
            else
                return new List<CardEmbossingDTO>();
        }

        public int GetPendingCardEmbossingItemsRecordCount(CardEmbossingDTO ACardEmbossingFilter)
        {
            int iRetValue = 0;
            try
            {
                List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                    new Utils.ExpressionList(){FilterColumn = "RUT", FilterValue = ACardEmbossingFilter.RUT, Operator = "Contains"},
                    new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = ACardEmbossingFilter.CardNumber, Operator = "Contains"},               
                    new Utils.ExpressionList(){FilterColumn = "Name", FilterValue = ACardEmbossingFilter.Name, Operator = "Contains"}};

                Expression<Func<CardEmbossing, bool>> expression = Utils.GetExpression<CardEmbossing>(lstExp);
                iRetValue = objCardEmbossingRepository.GetRecordCount(expression);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return iRetValue;
        }

        #endregion

        #region New Card Application

        public string[] CheckIfValidCard(string ARUTNumber,string Acheckdigit)
        {
            string[] retValues = new string[2];
            string checkdigit;
            if (Acheckdigit == null || Acheckdigit == "")
            {
                Acheckdigit = Functions.generateCheckDigitMod11(ARUTNumber).CheckDigit;
                ARUTNumber = ARUTNumber + "-" + Acheckdigit;
                retValues[0] = Acheckdigit;
            }
            else
            {
                checkdigit = Functions.generateCheckDigitMod11(ARUTNumber).CheckDigit;
                ARUTNumber = ARUTNumber + "-" + Acheckdigit;
                retValues[0] = checkdigit;
            }
               
            bool isvalid = Functions.IsValidRUTNumber(ARUTNumber);
            //Functions.GenerateCardEmbossingFile(ARUTNumber);
            retValues[1] = isvalid.ToString();

            return retValues;
        }
        #region CheckIfAlreadyExistsCard
        public string[] CheckIfAlreadyExistsCard(string ARUTNumber, string ASalesId)
        {
            string[] retValues = new string[4];
            string cardNumber = string.Empty, customerId = string.Empty, approvalStatus = string.Empty;

            if (Utils.OrganizationTypeCode == "RET")
            {
                cardNumber = FindCardNumberByRutNo(ARUTNumber).CardNumber;
                var customer = FindCustomerByRut(ARUTNumber);

                retValues[0] = customer.CustomerId.ToString();
                retValues[1] = customer.ApprovalStatus;
                retValues[2] = cardNumber;
                retValues[3] = customer.OrganizationCode;
            }
            else if (Utils.OrganizationTypeCode == "IND")
            {
                IndustrialCustomerDTO objResult = new IndustrialCustomerDTO();
                if (!string.IsNullOrEmpty(ASalesId))
                {

                    SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter() { ParameterName = "@P_RUT", SqlDbType = SqlDbType.VarChar, Value = ARUTNumber, Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_SalesId", SqlDbType = SqlDbType.VarChar, Value = ASalesId, Direction = ParameterDirection.Input}
                           };
                    objResult = objCustomerRepository.UnitOfWork.ExecuteQuery<IndustrialCustomerDTO>("PR_GetIndustrialCustomerByRUTSalesId @P_RUT, @P_SalesId", parameters).ToList().FirstOrDefault();
                    if (objResult != null)
                    {
                        retValues[0] = objResult.CustomerId.ToString();
                        retValues[1] = objResult.ApprovalStatus;
                        retValues[2] = "";
                        retValues[3] = objResult.OrganizationCode;
                    }
                }
                else
                {
                    SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter() { ParameterName = "@P_RUT", SqlDbType = SqlDbType.VarChar, Value = ARUTNumber, Direction = ParameterDirection.Input}
              
                    };
                    objResult = objCustomerRepository.UnitOfWork.ExecuteQuery<IndustrialCustomerDTO>("PR_GetIndustrialCustomersWithoutSalesId @P_RUT", parameters).ToList().FirstOrDefault();
                    if (objResult != null)
                    {
                        retValues[0] = objResult.CustomerId.ToString();
                        retValues[1] = objResult.ApprovalStatus;
                        retValues[2] = "";
                        retValues[3] = objResult.OrganizationCode;
                    }
                }
            }
            return retValues;
        }
        #endregion

        public CardMasterDTO  FindCardNumberByRutNo(string ARutNo)
        {
            CardMasterDTO objCardMasterDTO = new CardMasterDTO();
            try
            {
                List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                { new Utils.ExpressionList() { FilterColumn = "RUT",FilterValue = ARutNo, Operator = "Equals" }};

                Expression<Func<CardMaster, bool>> expression = Utils.GetExpression<CardMaster>(lstExpression);
                var cardMaster = objCardMasterRepository.GetFiltered(0, 1, expression);
                if (cardMaster.Count() != 0)
                {
                    objCardMasterDTO = Conversion.Mapping.CardMasterToCardMasterDTO(cardMaster.SingleOrDefault());
                }              
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objCardMasterDTO;
        }

        public CustomerDTO FindCustomerByRut(string ARutNo)
        {
            CustomerDTO objCustomerDTO = new CustomerDTO();
            try
            {
                List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                { new Utils.ExpressionList() { FilterColumn = "RUT", FilterValue = ARutNo, Operator = "Equals" },
                  new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                  new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
                };

                Expression<Func<Customer, bool>> expression = Utils.GetExpression<Customer>(lstExpression);
                var customer = objCustomerRepository.GetFiltered(0, 1, expression);
                if (customer.Count() != 0)
                {
                    objCustomerDTO = Conversion.Mapping.CustomerToCustomerDTO(customer.SingleOrDefault());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objCustomerDTO;
        }
        #endregion
    }
}
