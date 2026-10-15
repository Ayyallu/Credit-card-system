using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.DataTranfer.CustomerServiceInfo;
using CC_Ingenio.DataTranfer.AuthorizationInfo;
using CC_Ingenio.Repository.CustomerServiceRepo;
using CC_Ingenio.Repository.MasterRepo;
using CC_Ingenio.Repository.LookUpRepo;
using CC_Ingenio.Core.MasterRepo.MasterAggregate;
using CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate;
using CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate;
using CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate;
using CC_Ingenio.Core.LookUpRepo;
using CC_Ingenio.Common.Utils;
using System.Data;
using Microsoft.Data.SqlClient;
using CC_Ingenio.Repository.AuthorizationRepo;
using CC_Ingenio.Repository.CardBookingRepo;
using CC_Ingenio.Repository.BillingRepo;
using CC_Ingenio.Data;
using System.Transactions;
using CC_Ingenio.Manager.Conversion;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.DataTranfer.CardBookingInfo;
using CC_Ingenio.DataTranfer.BillingInfo;
using System.Linq.Expressions;


namespace CC_Ingenio.Manager
{
    public class CustomerServiceManager : ICustomerServiceManager
    {
        #region Variable
        private readonly IMasterManager tMasterManager;

        CustomerServiceDetailsRepository objCustomerServiceDetailsRepository;
        NotepadEntryRepository objNotepadEntryRepository;

        ManualTransactionRepository objManualTransactionRepository;
        MerchantsRepository objMerchantsRepository;
        CreditSegmentRepository objCreditSegmentRepository;
        AdjustmentRepository objAdjustmentRepository;
        CardCreditSegmentRepository objCardCreditSegmentRepository;
        CardMasterRepository objCardMasterRepository;

        BlockSegmentCardRepository objBlockSegmentCardRepository;
        BlockCodeRepository objBlockCodeRepository;
        EMITransactionRepository objEMITransactionRepository;
        EMITransactionInstallmentRepository objEMITransactionInstallmentRepository;
        EMIAuthorizationRepository objEMIAuthorizationRepository;
        PlanMasterRepository objPlanMasterRepository;

        CardClosureRepository objCardClosureRepository;
        CustomerAddressRepository objCustAddressRepository;
        UpdatedCustomerAddressRepository objUpdtdCustAddressRepository;
        CardReactivationRepository objCardReactivationRepository;
        CardClosureHistoryRepository objCardClosureHistoryRepository;
        AlertMessageRepository objAlertMessageRepository;
        LostStolenCardBlockRepository objLostStolenCardBlockRepository;
        LostStolenReissueRequestRepository objLostStolenReissueRequestRepository;
        StatementGenerationRepository objStatementGenerationRepository;
        TransactionRepository objTransactionRepository;

        CustomerRepository objCustomerRepository;
        ProductMemberTypeRepository objProductMemberTypeRepository;
        ChangeCreditLimitRepository objChangeCreditLimitRepository;
        SwapProductRepository objSwapProductRepository;

        #endregion

        #region Constructor
        public CustomerServiceManager( 
            CustomerServiceDetailsRepository ACustomerServiceDetailsRepository, 
            ManualTransactionRepository AManualTransactionRepository,MerchantsRepository AMerchantsRepository, 
            CreditSegmentRepository ACreditSegmentRepository, AdjustmentRepository AAdjustmentRepository,
            NotepadEntryRepository ANotepadEntryRepository, CardCreditSegmentRepository ACardCreditSegmentRepository,
            CardMasterRepository ACardMasterRepository, BlockSegmentCardRepository ABlockSegmentCardRepository, BlockCodeRepository ABlockCodeRepository, 
            EMITransactionRepository AEMITransactionRepository, EMITransactionInstallmentRepository AEMITransactionInstallmentRepository,
            EMIAuthorizationRepository AEMIAuthorizationRepository, PlanMasterRepository APlanMasterRepository, CardClosureRepository ACardClosureRepository,
            CustomerAddressRepository ACustAddressRepository, UpdatedCustomerAddressRepository AUpdtdCustAddressRepository, CardReactivationRepository ACardReactivationRepository, 
            CardClosureHistoryRepository ACardClosureHistoryRepository, AlertMessageRepository AAlertMessageRepository, IMasterManager AMasterManager,
            LostStolenCardBlockRepository ALostStolenCardBlockRepository, LostStolenReissueRequestRepository ALostStolenReissueRequestRepository,
            StatementGenerationRepository AStatementGenerationRepository, TransactionRepository ATransactionRepository, CustomerRepository ACustomerRepository,
            ProductMemberTypeRepository AProductMemberTypeRepository, ChangeCreditLimitRepository AChangeCreditLimitRepository, SwapProductRepository ASwapProductRepository)
        {
            tMasterManager = AMasterManager;

            if (ACustomerServiceDetailsRepository == null)
                throw new ArgumentNullException("ACustomerServiceDetailsRepository");
            if (AManualTransactionRepository == null)
                throw new ArgumentNullException("AManualTransactionRepository");
            if (AMerchantsRepository == null)
                throw new ArgumentNullException("AMerchantsRepository");
            if (ACreditSegmentRepository == null)
                throw new ArgumentNullException("ACreditSegmentRepository");
            if (AAdjustmentRepository == null)
                throw new ArgumentNullException("AAdjustmentRepository");
            if (ANotepadEntryRepository == null)
                throw new ArgumentNullException("ANotepadEntryRepository");
            if (ACardCreditSegmentRepository == null)
                throw new ArgumentNullException("ACardCreditSegmentRepository");
            if (ACardMasterRepository == null)
                throw new ArgumentNullException("ACardMasterRepository");
            if (ABlockSegmentCardRepository == null)
                throw new ArgumentNullException("ABlockSegmentCardRepository");
            if (ABlockCodeRepository == null)
                throw new ArgumentNullException("ABlockCodeRepository");
            if (AEMITransactionRepository == null)
                throw new ArgumentNullException("AEMITransactionRepository");
            if (AEMITransactionInstallmentRepository == null)
                throw new ArgumentNullException("AEMITransactionInstallmentRepository");
            if(AEMIAuthorizationRepository==null)
                throw new ArgumentNullException("AEMIAuthorizationRepository");
            if (APlanMasterRepository == null)
                throw new ArgumentNullException("APlanMasterRepository");
            if (ACardClosureRepository == null)
                throw new ArgumentNullException("ACardClosureRepository");
            if (ACustAddressRepository == null)
                throw new ArgumentNullException("ACustAddressRepository");
            if (AUpdtdCustAddressRepository == null)
                throw new ArgumentNullException("AUpdtdCustAddressRepository");
            if (ACardReactivationRepository == null)
                throw new ArgumentException("objCardReactivationRepository");
            if (ACardClosureHistoryRepository == null)
                throw new ArgumentException("objCardClosureHistoryRepository");
            if (AAlertMessageRepository == null)
                throw new ArgumentNullException("AAlertMessageRepository");
            if (ALostStolenCardBlockRepository == null)
                throw new ArgumentNullException("ALostStolenCardBlockRepository");
            if (ALostStolenReissueRequestRepository == null)
                throw new ArgumentNullException("ALostStolenReissueRequestRepository");
            if (AStatementGenerationRepository == null)
                throw new ArgumentNullException("AStatementGenerationRepository");
            if (ATransactionRepository == null)
                throw new ArgumentNullException("ATransactionRepository");
            if (ACustomerRepository == null)
                throw new ArgumentNullException("ACustomerRepository");
            if (AProductMemberTypeRepository == null)
                throw new ArgumentNullException("AProductMemberTypeRepository");
            if (AChangeCreditLimitRepository == null)
                throw new ArgumentNullException("AChangeCreditLimitRepository");
            if (ASwapProductRepository == null)
                throw new ArgumentNullException("ASwapProductRepository");

            objCustomerServiceDetailsRepository = ACustomerServiceDetailsRepository;
            objManualTransactionRepository = AManualTransactionRepository;
            objMerchantsRepository = AMerchantsRepository;
            objCreditSegmentRepository = ACreditSegmentRepository;
            objAdjustmentRepository = AAdjustmentRepository;
            objNotepadEntryRepository = ANotepadEntryRepository;
            objCardCreditSegmentRepository = ACardCreditSegmentRepository;
            objCardMasterRepository = ACardMasterRepository;
            objBlockSegmentCardRepository = ABlockSegmentCardRepository;
            objBlockCodeRepository = ABlockCodeRepository;
            objEMITransactionRepository = AEMITransactionRepository;
            objEMITransactionInstallmentRepository = AEMITransactionInstallmentRepository;
            objEMIAuthorizationRepository = AEMIAuthorizationRepository;
            objPlanMasterRepository = APlanMasterRepository;
            objCardClosureRepository = ACardClosureRepository;
            objCustAddressRepository = ACustAddressRepository;
            objUpdtdCustAddressRepository = AUpdtdCustAddressRepository;
            objCardReactivationRepository = ACardReactivationRepository;
            objCardClosureHistoryRepository = ACardClosureHistoryRepository;
            objAlertMessageRepository = AAlertMessageRepository;
            objLostStolenCardBlockRepository = ALostStolenCardBlockRepository;
            objLostStolenReissueRequestRepository = ALostStolenReissueRequestRepository;
            objStatementGenerationRepository = AStatementGenerationRepository;
            objTransactionRepository = ATransactionRepository;
            objCustomerRepository = ACustomerRepository;
            objProductMemberTypeRepository = AProductMemberTypeRepository;
            objChangeCreditLimitRepository = AChangeCreditLimitRepository;
            objSwapProductRepository = ASwapProductRepository;
        }
        #endregion

        #region Validate Customer
        public bool ValidateCustomer(string ACardNumber, string ARUT)
        {
            var result = objCardMasterRepository.AllMatching(x => (x.CardNumber == ACardNumber || x.RUT == ARUT) && x.OrganizationCode == Utils.OrganizationCode);
            if (result.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        } 
        #endregion

        #region GetCustomersForCustomerService
        public List<RUTCardNumberSearchDTO> GetCustomersForCustomerService(string ARUT, string ACardNumber, string ACustomerName, string ASalesId)
        {
            List<RUTCardNumberSearchDTO> objRUTCardNumberSearchDTO = new List<RUTCardNumberSearchDTO>();

            var result = objCardMasterRepository.AllMatching(x => x.ApprovalStatus == "A")
            .Join(objCustomerRepository.GetAll(),
                CardMaster => CardMaster.CustomerId,
                CustomerMaster => CustomerMaster.CustomerId,
                (CardMaster, CustomerMaster) => new
                {
                    CardMaster.CustomerId,
                    CardMaster.RUT,
                    CardMaster.CardNumber,
                    AccountNumber = CardMaster.AccountNo,
                    CustomerName = CustomerMaster.Name + " " + CustomerMaster.PaternalSurname + " " + CustomerMaster.MaternalSurname,
                    CardMaster.CustomerSegmentCode,
                    CardMaster.CustomerSubSegmentCode,
                    CardMaster.ProductCode,
                    CardMaster.OrganizationCode,
                    CardMaster.IsBlocked,
                    CardMaster.CardExpiryDate,
                    SalesId = CardMaster.SalesId == null ? "" : CardMaster.SalesId,
                    CardMaster.CardMemberType
                })
                .Join(objProductMemberTypeRepository.GetAll(),
                    CardMast => CardMast.CardMemberType,
                    ProductMemType => ProductMemType.ProductMemberTypeCode,
                    (CardMast, ProductMemType) => new 
                    { 
                        CardMast, 
                        ProductMemType.ProductMemberTypeCode, 
                        ProductMemberTypeDesc = ProductMemType.Description 
                    })
                .Where(x =>
                    (x.CardMast.RUT.Contains(ARUT == null ? "" : ARUT.Replace("-", "")) &&
                    x.CardMast.CardNumber.Contains(ACardNumber == null ? "" : ACardNumber) &&
                    x.CardMast.CustomerName.ToLower().Contains(ACustomerName == null ? "" : ACustomerName.ToLower()) &&
                    x.CardMast.SalesId.Contains(ASalesId == null ? "" : ASalesId) &&
                    //x.CardMast.IsBlocked == false &&
                    x.CardMast.OrganizationCode == Utils.OrganizationCode 
                    //x.CardMast.CardExpiryDate >= Utils.BusinessDate
                    ));

            if (result.Any())
            {
                result.ToList().ForEach(x => objRUTCardNumberSearchDTO.Add(new RUTCardNumberSearchDTO()
                {
                    CustomerId = x.CardMast.CustomerId,
                    RUT = x.CardMast.RUT,
                    CardNumber = x.CardMast.CardNumber,
                    AccountNumber = x.CardMast.AccountNumber,
                    CustomerName = x.CardMast.CustomerName,
                    CustomerSegmentCode = x.CardMast.CustomerSegmentCode,
                    CustomerSubSegmentCode = x.CardMast.CustomerSubSegmentCode,
                    ProductCode = x.CardMast.ProductCode,
                    OrganizationCode = x.CardMast.OrganizationCode,
                    SalesId = x.CardMast.SalesId,
                    ProductMemberTypeCode = x.ProductMemberTypeCode,
                    ProductMemberTypeDesc = x.ProductMemberTypeDesc
                }));
            }

            return objRUTCardNumberSearchDTO;
        }
        #endregion

        #region GetCustomerServiceDetails
        public CustomerServiceDetailsDTO GetCustomerServiceDetails(string ACardNumber,string ARUT)
        {
            string searchType = string.Empty,searchCriteria = string.Empty;

            if (!string.IsNullOrWhiteSpace(ACardNumber))
            {
                searchType = "CardNumber";
                searchCriteria = ACardNumber;
            }
            else 
            {
                searchType = "RUT";
                searchCriteria = ARUT;
            }

            CustomerServiceDetailsDTO objCustomerServiceDetailsDTO;
            
            var result = objCustomerServiceDetailsRepository.UnitOfWork.ExecuteQuery<CustomerServiceDetails>("PR_GetDetailsForCustomerService @P_SearchType,@P_SearchCriteria", 
                new SqlParameter("P_SearchType", searchType), new SqlParameter("P_SearchCriteria", searchCriteria)).FirstOrDefault();

            if (result == null)
            {
                objCustomerServiceDetailsDTO = new CustomerServiceDetailsDTO();
            }
            else
            {
                objCustomerServiceDetailsDTO = CC_Ingenio.Manager.Conversion.Mapping.CustomerServiceDetailsToCustomerServiceDetailsDTO(result);
            }
            return objCustomerServiceDetailsDTO;
        }
        #endregion

        #region GetUnMatchedAuthorizationDetails
        public List<UnMatchedAuthorizationDTO> GetUnMatchedAuthorizationDetails(string ACardNumber)
        {
            List<UnMatchedAuthorizationDTO> objUnMatchedAuthorizationList = new List<UnMatchedAuthorizationDTO>();

            var result = objManualTransactionRepository.GetFiltered(0, 10, Utils.GetExpression<ManualTransaction>(new List<Utils.ExpressionList>() { 
                 new Utils.ExpressionList() { FilterColumn = "CardNumber", FilterValue = ACardNumber, Operator = "Equals" },
                 new Utils.ExpressionList(){ FilterColumn="IsPosted", FilterValue = false, Operator = "Equals"}}))
                .Join(objMerchantsRepository.GetAll(),
                    manualTrans => new { manualTrans.MerchantCode, manualTrans.OrganizationCode },
                    merchant => new { MerchantCode = merchant.MerchantCode, OrganizationCode = merchant.OrganizationCode },
                    (manualTrans, merchant) => new
                     {
                         manualTrans.CardNumber,
                         manualTrans.SegmentCode,
                         merchant.MerchantName,
                         manualTrans.Amount,
                         manualTrans.TxnDate,
                         manualTrans.TxnTime,
                         manualTrans.OrganizationCode
                     })
                 .Join(objCreditSegmentRepository.GetAll(),
                     manualTrans => new { manualTrans.SegmentCode, manualTrans.OrganizationCode },
                     segment => new { SegmentCode = segment.CardSegmentCode, OrganizationCode = segment .OrganizationCode },
                     (manualTrans, segment) => new { manualTrans, SegmentName = segment.CardSegmentName })
                 .Select(x => x);

            if (result.Any())
            {
                result.ToList().ForEach(x => objUnMatchedAuthorizationList.Add(new UnMatchedAuthorizationDTO()
                {
                    CardNumber = x.manualTrans.CardNumber,
                    SegmentName = x.SegmentName,
                    MerchantName = x.manualTrans.MerchantName,
                    Amount = x.manualTrans.Amount,
                    TxnDate = x.manualTrans.TxnDate,
                    TxnTime = x.manualTrans.TxnTime
                }));
            }

            return objUnMatchedAuthorizationList;
        }
        #endregion

        #region GetUnbilledTransactionDetails
        public List<UnbilledTransactionDetailsDTO> GetUnbilledTransactionDetails(string ACardNumber)
        {
            List<UnbilledTransactionDetailsDTO> objUnbilledTransactionList = new List<UnbilledTransactionDetailsDTO>();

            objUnbilledTransactionList = objCustomerServiceDetailsRepository.UnitOfWork.ExecuteQuery<UnbilledTransactionDetailsDTO>("PR_UnbilledTransactionDetailsGet @P_CardNumber", 
                            new SqlParameter("P_CardNumber", ACardNumber)).ToList();

            return objUnbilledTransactionList;
        }
        #endregion

        #region GetCustomerTransactionDetails
        public List<CustomerTransactionDetailsDTO> GetCustomerTransactionDetails(string ACardNumber, DateTime AFromDate, DateTime AToDate, int ATransType)
        {
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter() { ParameterName = "@P_CardNumber", SqlDbType = SqlDbType.VarChar, Size = 16, Value = (ACardNumber == null ? string.Empty : ACardNumber), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_FromDate", SqlDbType = System.Data.SqlDbType.Date, Value = AFromDate.Date, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter() { ParameterName = "@P_ToDate", SqlDbType = System.Data.SqlDbType.Date, Value = AToDate.Date, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter() { ParameterName = "@P_TransType", SqlDbType = SqlDbType.Int, Value = ATransType, Direction = ParameterDirection.Input}
             } ;

            List<CustomerTransactionDetailsDTO> objCustomerTransactionList = new List<CustomerTransactionDetailsDTO>();

            objCustomerTransactionList = objCustomerServiceDetailsRepository.UnitOfWork.ExecuteQuery<CustomerTransactionDetailsDTO>("PR_GetCustomerTransactionsList @P_CardNumber, @P_FromDate, @P_ToDate,@P_TransType",
                           parameters).ToList();

            return objCustomerTransactionList;
        }
        #endregion

        #region GetDisputedTransactionsList
        public List<CustomerTransactionDetailsDTO> GetDisputedTransactionsList(string ACardNumber)
        {
            List<CustomerTransactionDetailsDTO> objDisputedTransactionList = new List<CustomerTransactionDetailsDTO>();

            objDisputedTransactionList = objCustomerServiceDetailsRepository.UnitOfWork.ExecuteQuery<CustomerTransactionDetailsDTO>("PR_DisputedTransactionsGet @P_CardNumber",
                            new SqlParameter("P_CardNumber", ACardNumber)).ToList();

            return objDisputedTransactionList;
        }
        #endregion
       
        #region Alert Message
        /// <summary>
        /// Get the alert message for a cardno and alert type.
        /// </summary>
        /// <param name="ACardNumber"></param>
        /// <param name="AAlertType"></param>
        /// <returns></returns>
        public AlertMessageDTO GetAlertMessage(string ACardNumber, string AAlertType)
        {
            AlertMessageDTO objAlertMessageDTO = new AlertMessageDTO();
            var result = objAlertMessageRepository.AllMatching(x => x.CardNumber == ACardNumber && x.AlertType == AAlertType);
            if (result.Any())
            {
                objAlertMessageDTO.AlertId = result.FirstOrDefault().AlertId;
                objAlertMessageDTO.CardNumber = result.FirstOrDefault().CardNumber;
                objAlertMessageDTO.AlertType = result.FirstOrDefault().AlertType;
                objAlertMessageDTO.IsEmailAlert = result.FirstOrDefault().IsEmailAlert;
                objAlertMessageDTO.IsMobileAlert = result.FirstOrDefault().IsMobileAlert;
                objAlertMessageDTO.MobileNo = result.FirstOrDefault().MobileNo;
                objAlertMessageDTO.EmailId = result.FirstOrDefault().EmailId;
                objAlertMessageDTO.IsCharged = result.FirstOrDefault().IsCharged;
                objAlertMessageDTO.Message = result.FirstOrDefault().Message;
            }
            return objAlertMessageDTO;
        }
        /// <summary>
        /// Save the alert message
        /// </summary>
        /// <param name="AAlertMessageDTO"></param>
        public void SaveAlertMessage(AlertMessageDTO AAlertMessageDTO)
        {
            if (AAlertMessageDTO == null)
            {
                throw new ArgumentNullException("");
            }
            AlertMessage alertMsgObj = new AlertMessage();
            alertMsgObj = AlertMessageFactory.CreateAlertMessage(AAlertMessageDTO.AlertId,AAlertMessageDTO.CardNumber,AAlertMessageDTO.AlertType,
                AAlertMessageDTO.Message,AAlertMessageDTO.IsEmailAlert,AAlertMessageDTO.IsMobileAlert,AAlertMessageDTO.MobileNo,AAlertMessageDTO.EmailId,AAlertMessageDTO.IsCharged);
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted });
            using (scope)
            {
                try
                {
                        AlertMessage objExistingAlertMessage = objAlertMessageRepository.AllMatching(x => (x.CardNumber == AAlertMessageDTO.CardNumber && x.AlertType == AAlertMessageDTO.AlertType)).FirstOrDefault();
                        if (objExistingAlertMessage == null)
                        {
                            objAlertMessageRepository.Add(alertMsgObj);
                            objAlertMessageRepository.UnitOfWork.Commit();
                        }
                        else
                        {
                            objExistingAlertMessage = objAlertMessageRepository.Get(objExistingAlertMessage.AlertId);
                            alertMsgObj.AlertId = objExistingAlertMessage.AlertId;
                            objAlertMessageRepository.Merge(objExistingAlertMessage, alertMsgObj);
                            objAlertMessageRepository.UnitOfWork.Commit();
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

        #region StatementImage
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ACardNumber"></param>
        /// <param name="ABillMonth"></param>
        /// <param name="ABillYear"></param>
        /// <returns></returns>
        public bool CheckBilledStatementCreated(string ACardNumber, int ABillMonth, int ABillYear)
        {
            GeneratedStatementDTO objGeneratedStatementDTO = new GeneratedStatementDTO();
            var result = objStatementGenerationRepository.AllMatching(x => x.CardNumber == ACardNumber && x.BillMonth == ABillMonth && x.BillYear == ABillYear);
            if (result.Any())            
                return true;            
            else
                return false;
        }

        public string CheckStatementFileExists(string ACardNumber, int ABillMonth, int ABillYear)
        {
            string strFileName = "";
            SqlParameter[] paramCollection = new SqlParameter[] { 
                           
                            new SqlParameter(){ ParameterName = "@P_OrganizationCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = Utils.OrganizationCode, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ ParameterName = "@P_CardNumber", SqlDbType = System.Data.SqlDbType.VarChar, Value = ACardNumber, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ ParameterName = "@P_BillMonth", SqlDbType = System.Data.SqlDbType .Int , Value = ABillMonth, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ ParameterName = "@P_BillYear", SqlDbType = System.Data.SqlDbType.Int, Value = ABillYear, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ParameterName = "@P_FileName", SqlDbType = System.Data.SqlDbType.NVarChar, Direction = System.Data.ParameterDirection.Output, Size = 250}
                        };

            objStatementGenerationRepository.UnitOfWork.ExecuteCommand("PR_CheckStatementFileExistsInFolder  @P_OrganizationCode, @P_CardNumber, @P_BillMonth, @P_BillYear, @P_FileName OUTPUT", paramCollection);
            strFileName = Convert.ToString(paramCollection[4].Value);

            return strFileName;
        }

        #endregion

        #region DocumentsUpload
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ACardNumber"></param>
        /// <param name="ACustomerId"></param>
        /// <param name="ADocName"></param>
        /// <param name="ADocPath"></param>
        /// <returns></returns>
        public bool SaveUploadedDocumentInfo(string ACardNumber, string ACustomerId, string ADocName, string ALoggedUser)
        {
            if (ADocName == null || ACardNumber == null || ACustomerId == null)
                throw new ArgumentNullException("");
            try
            {
                SqlParameter[] paramCollection = new SqlParameter[]
                { 
                    new SqlParameter(parameterName: "@P_DocName",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_CustomerId",dbType:System.Data.SqlDbType.Int),
                    new SqlParameter(parameterName: "@P_Cardnumber",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_LoggedUser",dbType:System.Data.SqlDbType.VarChar)
                   
                };

                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_DocName")).First()).Value = ADocName;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CustomerId")).First()).Value = Convert.ToInt32(ACustomerId);
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Cardnumber")).First()).Value = ACardNumber;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_LoggedUser")).First()).Value = ALoggedUser;

                objCustomerServiceDetailsRepository.UnitOfWork.ExecuteCommand("PR_CustomerDocsAdd @P_DocName,@P_CustomerId,@P_Cardnumber,@P_LoggedUser", paramCollection);
                return true;
            }

            catch (Exception ex)
            {
                return false;
                throw ex;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ACardNumber"></param>
        /// <param name="ACustomerId"></param>
        /// <returns></returns>
        public List<UploadDocumentDTO> GetUploadedDocumentList(string ACardNumber, string ACustomerId)
        {
            List<UploadDocumentDTO> objUploadedDocList = new List<UploadDocumentDTO>();

            SqlParameter[] paramsCollection = new SqlParameter[]
            { 
                new SqlParameter(){ ParameterName = "@P_CustomerId", Value = (object)ACustomerId ?? DBNull.Value, SqlDbType = SqlDbType.Int },
                new SqlParameter(){ ParameterName = "@P_Cardnumber", Value = (object)ACardNumber ?? DBNull.Value ,SqlDbType = SqlDbType.VarChar}
                
            };

            objUploadedDocList = objCustomerServiceDetailsRepository.UnitOfWork.ExecuteQuery<UploadDocumentDTO>("PR_CustomerDocsGet @P_CustomerId, @P_Cardnumber",
                           paramsCollection).ToList();


            return objUploadedDocList;
        }

        #endregion

        #region GetCardClosureDetails
        public CardClosureDTO GetCardClosureDetails(string ACardNumber, int ACustomerId)
        {
            CardClosureDTO objCardClosureDTO = new CardClosureDTO();

            var result = objCardClosureRepository.AllMatching(x => x.CustomerId == ACustomerId && x.CardNumber == ACardNumber && x.IsActive == true)
                .Join(objCardMasterRepository.GetAll(), Closure => Closure.CardNumber,
                CardMast => CardMast.CardNumber,
                (Closure, CardMast) => new { Closure, CardMast.CreditOTB }).Select(x => x).ToList();

            if (result.Any())
            {
                objCardClosureDTO = Mapping.CardClosureToCardClosureDTO((CardClosure)result.FirstOrDefault().Closure);
                objCardClosureDTO.CurrentBalance = result.FirstOrDefault().CreditOTB;
                objCardClosureDTO.IsActive = result.FirstOrDefault().Closure.IsActive;
            }
            else
            {

                decimal CreditLimit = objCardMasterRepository.Get(ACardNumber).CreditLimit;
                decimal CurrBalance = CreditLimit - (objCardMasterRepository.Get(ACardNumber).CreditOTB);

                objCardClosureDTO.CurrentBalance = CurrBalance;
                objCardClosureDTO.CustomerId = ACustomerId;
                objCardClosureDTO.CardNumber = ACardNumber;
                objCardClosureDTO.ClosuredDate = Utils.BusinessDate;
            }

            return objCardClosureDTO;
              //blocksegmentcard => blocksegmentcard.BlockCode,
              //  blockcode=> blockcode.Code,
              //  (blocksegmentcard, blockcode) => new
              //  {
              //      blockcode.Description,
              //      blocksegmentcard.BlockCode 
              //  }).Select(x => x);


            // SqlParameter[] paramCollection = new SqlParameter[]
            //{ 
            //    new SqlParameter(){ ParameterName = "@P_CardNumber", Value = (object)ACardNumber?? DBNull.Value, SqlDbType = SqlDbType.DateTime },
            //    new SqlParameter(){ ParameterName = "@P_Customer_ID", Value = (object) ACustomerid ?? DBNull.Value ,SqlDbType = SqlDbType.VarChar }
            //};

           // lstBillingCycleViewDTO = objBillingCycleViewRepository.UnitOfWork.ExecuteQuery<BillingCycleViewDTO>("PR_GetTransactionsForAdjustment @ChargeSlipDate, @CardNumber", paramCollection).ToList();


            // objCardClosureList = objCustomerServiceDetailsRepository.UnitOfWork.ExecuteQuery<CardClosureDTO>("PR_GetCardClosureDetails @P_CardNumber, @P_Customer_ID", paramCollection).ToList();
            
        }
        #endregion

        #region GetCardReactivationDetails
        public CardReactivationDTO GetCardReactivationDetails(string ACardNumber, int ACustomerId)
        {
            CardReactivationDTO objCardReactivationDTO = new CardReactivationDTO();
    
            //CardReactivation cardReactivation = objCardReactivationRepository.Get(ACardNumber, ACustomerId);
            objCardReactivationDTO.CustomerId = ACustomerId;
            objCardReactivationDTO.CardNumber = ACardNumber;

            var result = objCardClosureRepository.AllMatching(x => x.CustomerId == ACustomerId && x.CardNumber == ACardNumber && x.IsActive == true);
            if (result.Any())
            {

                if (ValidateCardReactivationExists(ACustomerId, ACardNumber) == true)
                {
                    var results = objCardReactivationRepository.AllMatching(x => x.CustomerId == ACustomerId && x.CardNumber == ACardNumber && (x.ApprovalStatus == "P" || x.ApprovalStatus == "R"));
                    if (results.Any())
                    {

                        // objCardReactivationDTO = Mapping.CardReactivationToCardReactivationDTO((CardReactivation)result).FirstOrDefault());;
                        //   objCardReactivationDTO = Conversion.Mapping.CardReactivationToCardReactivationDTO(result).;
                        objCardReactivationDTO.ClosureID = results.FirstOrDefault().ClosureID;
                        objCardReactivationDTO.ReactivationReason = results.FirstOrDefault().ReactivationReasonCode;
                        objCardReactivationDTO.ReactivationRemarks = results.FirstOrDefault().ReactivationRemarks;
                        objCardReactivationDTO.ClosureReasondesc = results.FirstOrDefault().objCardClosure.objClosureReason.ReasonDescription;
                        objCardReactivationDTO.ApprovalStatus = results.FirstOrDefault().ApprovalStatus;
                        objCardReactivationDTO.ApprovalStatusdesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(objCardReactivationDTO.ApprovalStatus);
                    }

                    else
                    {
                        objCardReactivationDTO.ClosureReasondesc = result.FirstOrDefault().objClosureReason.ReasonDescription;                       
                        objCardReactivationDTO.ClosureID = result.FirstOrDefault().ClosureID;
                    }
                }
                else
                {
                    objCardReactivationDTO.ClosureReasondesc = result.FirstOrDefault().objClosureReason.ReasonDescription;
                    objCardReactivationDTO.ClosureID = result.FirstOrDefault().ClosureID;
                }
            }
            //else if (ValidateCardReactivationExists(ACustomerId, ACardNumber) == true)
            //{
            //    var results = objCardReactivationRepository.AllMatching(x => x.CustomerId == ACustomerId && x.CardNumber == ACardNumber);
            //    if (results.Any())
            //    {

            //        // objCardReactivationDTO = Mapping.CardReactivationToCardReactivationDTO((CardReactivation)result).FirstOrDefault());;
            //        //   objCardReactivationDTO = Conversion.Mapping.CardReactivationToCardReactivationDTO(result).;
            //        objCardReactivationDTO.ClosureID = results.FirstOrDefault().ClosureID;
            //        objCardReactivationDTO.ReactivationReason = results.FirstOrDefault().ReactivationReasonCode;
            //        objCardReactivationDTO.ReactivationRemarks = results.FirstOrDefault().ReactivationRemarks;
            //        objCardReactivationDTO.ClosureReasondesc = results.FirstOrDefault().objCardClosure.objClosureReason.ReasonDescription;
            //        objCardReactivationDTO.ApprovalStatus = results.FirstOrDefault().ApprovalStatus;
            //        objCardReactivationDTO.ApprovalStatusdesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(objCardReactivationDTO.ApprovalStatus);
            //    }
            //}
            return objCardReactivationDTO;

        }
        #endregion

        #region GetAdjustmentDetails
        public List<AdjustmentDTO> GetAdjustmentDetails(string ACardNumber,DateTime AFromDate,DateTime AToDate)
        {
            List<AdjustmentDTO> objAdjustmentDTOList = new List<AdjustmentDTO>();
            DateTime toDate = AToDate.AddDays(1);
            var result = objAdjustmentRepository.AllMatching(x => x.CardNumber == ACardNumber
                && (x.CreatedOn >= AFromDate
                    && x.CreatedOn < toDate)).OrderBy(x => x.CreatedOn);
                
            if(result.Any())
            {
                result.ToList().ForEach(x => objAdjustmentDTOList.Add(Conversion.Mapping.AdjustmentToAdjustmentDTO(x)));
            }

            return objAdjustmentDTOList;
        } 
        #endregion

        #region GetPendingAdjustments
        public List<AdjustmentDTO> GetPendingAdjustments(string ACardNumber)
        {
            List<AdjustmentDTO> objAdjustmentDTOList = new List<AdjustmentDTO>();
            var result = objAdjustmentRepository.AllMatching(x => x.CardNumber == ACardNumber && x.ApprovalStatus == "P");
            if (result.Any())
            {
                result.ToList().ForEach(x => objAdjustmentDTOList.Add(Conversion.Mapping.AdjustmentToAdjustmentDTO(x)));
            }

            return objAdjustmentDTOList;
        }
        #endregion

        #region SaveNotepadEntry
        public void SaveNotepadEntry(NotepadEntryDTO ANotepadEntry)
        {
            if (ANotepadEntry == null)
            {
                throw new ArgumentNullException("");
            }

            NotepadEntry newNotepadEntryObj = new NotepadEntry();
            newNotepadEntryObj = NotepadEntryFactory.CreateNotepadEntry(ANotepadEntry.MessageType, ANotepadEntry.MessageDescription,
                ANotepadEntry.CustomerId,ANotepadEntry.CardNumber,ANotepadEntry.CreatedBy, ANotepadEntry.LastUpdatedBy);

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objNotepadEntryRepository.Add(newNotepadEntryObj);
                    objNotepadEntryRepository.UnitOfWork.Commit();
                    scope.Complete();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region GetCardClosureHistoryDetails
        public List<CardClosureHistoryDTO> GetCardClosureHistoryDetails(string ACardNumber, int ACustomerId)
        {

            List<CardClosureHistoryDTO> objCardClosureDTOList = new List<CardClosureHistoryDTO>();

            SqlParameter[] paramsCollection = new SqlParameter[]
            { 
                
                new SqlParameter(){ ParameterName = "@P_CardNumber", Value = (object)ACardNumber ?? DBNull.Value ,SqlDbType = SqlDbType.VarChar},
                new SqlParameter(){ ParameterName = "@P_CustomerID", Value = (object)ACustomerId ?? DBNull.Value, SqlDbType = SqlDbType.Int }
            };

            objCardClosureDTOList = objCardClosureHistoryRepository.UnitOfWork.ExecuteQuery<CardClosureHistoryDTO>("PR_GetCardClosureHistoryDetails @P_CardNumber, @P_CustomerID", paramsCollection).ToList();

            return objCardClosureDTOList;

     
        }
        #endregion

        #region SaveClosureInitiation
        public void SaveClosureInitiation(CardClosureDTO ACardClosureDTO)
        {
            if (ACardClosureDTO == null)
            {
                throw new ArgumentNullException("");
            }

            CardClosure newCardClosureObj = new CardClosure();
            newCardClosureObj = CardClosureFactory.CreateCardClosure(ACardClosureDTO.CustomerId, ACardClosureDTO.CardNumber, ACardClosureDTO.ClosureReasonCode, ACardClosureDTO.ClosuredDate,ACardClosureDTO.IsCardReceived, ACardClosureDTO.IsDocumentReceived, ACardClosureDTO.Remarks,
            ACardClosureDTO.CreatedBy, ACardClosureDTO.LastUpdatedBy);

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    newCardClosureObj.IsActive = true;
                    objCardClosureRepository.Add(newCardClosureObj);
                    objCardClosureRepository.UnitOfWork.Commit();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region SaveCardReactivation
        public void SaveCardReactivation(CardReactivationDTO ACardReactivationDTO)
        {
            if (ACardReactivationDTO == null)
            {
                throw new ArgumentNullException("");
            }

            CardReactivation newCardReactivationObj = new CardReactivation();
            newCardReactivationObj = CardReactivationFactory.CreateCardReactivation(ACardReactivationDTO.CustomerId, ACardReactivationDTO.CardNumber, ACardReactivationDTO.ClosureID,
                ACardReactivationDTO.ReactivationReason, ACardReactivationDTO.ReactivationRemarks,
            ACardReactivationDTO.CreatedBy, ACardReactivationDTO.LastUpdatedBy, ACardReactivationDTO.ApprovalStatus);
            newCardReactivationObj.ApprovalStatus = "P";
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {

                    if (string.IsNullOrEmpty(ACardReactivationDTO.ApprovalStatus))
                    {
                        objCardReactivationRepository.Add(newCardReactivationObj);
                        objCardReactivationRepository.UnitOfWork.Commit();

                        ACardReactivationDTO.ApprovalStatusdesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACardReactivationDTO.ApprovalStatus);
                    }
                    else if (ACardReactivationDTO.ApprovalStatus == "R")
                    {
                        CardReactivation objCardReactivationexisting = objCardReactivationRepository.AllMatching(x => (x.CustomerId == ACardReactivationDTO.CustomerId && x.CardNumber == ACardReactivationDTO.CardNumber && x.ApprovalStatus == "R")).FirstOrDefault();

                        if (objCardReactivationexisting != null)
                            objCardReactivationexisting = objCardReactivationRepository.Get(objCardReactivationexisting.ReactivationID);
                        newCardReactivationObj.ReactivationID = objCardReactivationexisting.ReactivationID;

                        objCardReactivationRepository.Merge(objCardReactivationexisting, newCardReactivationObj);
                        objCardReactivationRepository.UnitOfWork.Commit();
                    }
                   

                    SqlParameter[] paramCollection = new SqlParameter[] { };
                    paramCollection = CommonManager.GetApprovalQueueProcedureParameters("CS_Card_Reactivation", " ReactivationID = '" + newCardReactivationObj.ReactivationID + "'",
                        "RACT", newCardReactivationObj.LastUpdatedBy, "/CustomerService/CardReactivationAuthorize/" + newCardReactivationObj.ReactivationID + "/A?", "CS");
                    objUpdtdCustAddressRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType", paramCollection);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region SaveChangedCustomerAddress
        public void SaveChangedCustomerAddress(UpdatedCustomerAddressDTO AUpdtdCustomerAddress)
        {
            if (AUpdtdCustomerAddress == null)
            {
                throw new ArgumentNullException("");
            }           
            UpdatedCustomerAddress custAddressObj = new UpdatedCustomerAddress();
            custAddressObj = UpdatedCustomerAddressFactory.CreateUpdatedCustomerAddress(AUpdtdCustomerAddress.CustomerId, AUpdtdCustomerAddress.CardNumber, AUpdtdCustomerAddress.AddressTypeCode,
                AUpdtdCustomerAddress.TypeOfResidenceClearance, AUpdtdCustomerAddress.CountryCode, AUpdtdCustomerAddress.RegionCode, AUpdtdCustomerAddress.CityCode, AUpdtdCustomerAddress.CommunaCode,
                AUpdtdCustomerAddress.Street, AUpdtdCustomerAddress.Number, AUpdtdCustomerAddress.Block, AUpdtdCustomerAddress.ComplementOfAddress, AUpdtdCustomerAddress.Town,
                AUpdtdCustomerAddress.Landmark, AUpdtdCustomerAddress.PostalCode, AUpdtdCustomerAddress.POBox, AUpdtdCustomerAddress.Phone, AUpdtdCustomerAddress.Mobile,
                AUpdtdCustomerAddress.Fax, AUpdtdCustomerAddress.UpdatedBy, AUpdtdCustomerAddress.UpdatedOn, AUpdtdCustomerAddress.ApprovedBy, AUpdtdCustomerAddress.ApprovedOn, AUpdtdCustomerAddress.ApprovalStatus);
            custAddressObj.ApprovalStatus = "P";
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted });
            using (scope)
            {
                try
                {
                    if (string.IsNullOrEmpty(AUpdtdCustomerAddress.ApprovalStatus))
                    {
                        objUpdtdCustAddressRepository.Add(custAddressObj);
                        objUpdtdCustAddressRepository.UnitOfWork.Commit();
                    }
                    else if (AUpdtdCustomerAddress.ApprovalStatus == "R")
                    {
                        UpdatedCustomerAddress objExistingCustAddress = objUpdtdCustAddressRepository.AllMatching(x => (x.CustomerId == AUpdtdCustomerAddress.CustomerId && x.AddressTypeCode == AUpdtdCustomerAddress.AddressTypeCode && x.ApprovalStatus == AUpdtdCustomerAddress.ApprovalStatus)).FirstOrDefault();
                        if (objExistingCustAddress != null)
                            objExistingCustAddress = objUpdtdCustAddressRepository.Get(objExistingCustAddress.AddressId);
                        custAddressObj.AddressId = objExistingCustAddress.AddressId;
                        objUpdtdCustAddressRepository.Merge(objExistingCustAddress, custAddressObj);
                        objUpdtdCustAddressRepository.UnitOfWork.Commit();
                    }
                    SqlParameter[] paramCollection = new SqlParameter[] { };

                    paramCollection = CommonManager.GetApprovalQueueProcedureParameters("CS_CustomerAddressUpdate", " AddressId = '" + custAddressObj.AddressId + "'",
                        "ADDR", custAddressObj.LastUpdatedBy, "/CustomerService/AddressAuthorize/" + custAddressObj.AddressId + "/A?", "CS");
                    objUpdtdCustAddressRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }           

        }
        #endregion

        #region GetNotepadDetails
        public List<NotepadEntryDTO> GetNotepadDetails(int ACustomerId, string ACardNumber, string AStatus)
        {
            List<NotepadEntryDTO> objNotepadEntryDTOList = new List<NotepadEntryDTO>();
            var result = objNotepadEntryRepository.AllMatching(x => x.CustomerId == ACustomerId && x.CardNumber == ACardNumber && x.MessageStatus == AStatus);
            if (result.Any())
            {
                result.ToList().ForEach(x => objNotepadEntryDTOList.Add(Mapping.NotepadEntryToNotepadEntryDTO(x)));
            }
            return objNotepadEntryDTOList;
        }
        #endregion

        #region GetCardClosureInitiation
        public List<CardClosureDTO> GetCardClosureInitiation(int ACustomerId, string ACardNumber)
        {
            List<CardClosureDTO> objCardClosureDTOList = new List<CardClosureDTO>();
            var result = objCardClosureRepository.AllMatching(x => x.CustomerId == ACustomerId && x.CardNumber == ACardNumber );
            if (result.Any())
            {
                result.ToList().ForEach(x => objCardClosureDTOList.Add(Mapping.CardClosureToCardClosureDTO(x)));
            }
            return objCardClosureDTOList;
        }
        #endregion

        #region GetUpdatedCustomerAddressForApproval
        public UpdatedCustomerAddressDTO GetUpdatedCustomerAddressForApproval(int AAddressId)
        {
            UpdatedCustomerAddressDTO objUpdtdCustAddressDTO = new UpdatedCustomerAddressDTO();
            if (objUpdtdCustAddressRepository.Get(AAddressId) != null)
            {
                var result = objUpdtdCustAddressRepository.AllMatching(x => x.AddressId == AAddressId);
                if (result.Any())
                {
                    objUpdtdCustAddressDTO.CardNumber = result.FirstOrDefault().CardNumber;
                    objUpdtdCustAddressDTO.CustomerId = result.FirstOrDefault().CustomerId;
                    objUpdtdCustAddressDTO.AddressTypeCode = result.FirstOrDefault().AddressTypeCode;
                    objUpdtdCustAddressDTO.ApprovalStatus = result.FirstOrDefault().ApprovalStatus;
                }
            }
            return objUpdtdCustAddressDTO;
        }
        #endregion

        #region GetCardReactivationIDForApproval
        public CardReactivationDTO GetCardReactivationIDForApproval(int AReactivationID)
        {
            CardReactivationDTO objCardReactivationDTO = new CardReactivationDTO();

            if (objCardReactivationRepository.Get(AReactivationID) != null)
            {
                var result = objCardReactivationRepository.AllMatching(x => x.ReactivationID == AReactivationID);
                if (result.Any())
                {
                    objCardReactivationDTO.CardNumber = result.FirstOrDefault().CardNumber;
                    objCardReactivationDTO.CustomerId = result.FirstOrDefault().CustomerId;
                    objCardReactivationDTO.ApprovalStatus = result.FirstOrDefault().ApprovalStatus;
                    
                }
            }
            return objCardReactivationDTO;
        }
        #endregion

        #region SaveDisputedTransactionsList
        public bool SaveDisputedTransactionsList(List<DisputedTransactionsDTO> ACustTransactionsLsTable)
        {
            if (ACustTransactionsLsTable == null)
                throw new ArgumentException("Messages.warning_CannotSaveUserWithNullInformation");

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    DataTable objDisputedTransTable = new DataTable();
                    objDisputedTransTable.Columns.Add("AuthNo", typeof(string));
                    objDisputedTransTable.Columns.Add("Disputed", typeof(int));
                    foreach (DisputedTransactionsDTO objDisputedTrans in ACustTransactionsLsTable)
                    {
                        objDisputedTransTable.Rows.Add(objDisputedTrans.AuthCode, objDisputedTrans.IsDisputed == true ? 1 : 0);
                    }

                    SqlParameter[] paramCollection = new SqlParameter[]{                    
                    new SqlParameter(){ ParameterName="@P_TransactionList", SqlDbType= SqlDbType.Structured, TypeName = "UD_DisputedTransactions", Direction = ParameterDirection.Input},
               
                 };

                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TransactionList")).First()).Value = objDisputedTransTable;
                    
                    objTransactionRepository.UnitOfWork.ExecuteCommand(@"PR_DisputedTransactionsSave @P_TransactionList", paramCollection);

                    scope.Complete();
                }
                catch (Exception)
                {

                }
            }
            return true;
        }
        #endregion

        #region CardReactivationApproveInsert
        public bool CardReactivationApproveInsert(string AId)
        {
            //CardReactivationDTO objCardReactivationDTO = new CardReactivationDTO();
           
            CardReactivation objCardReactivation =objCardReactivationRepository.AllMatching(x => (x.QueueReferenceNo == AId)).FirstOrDefault();
            //bool isUpdated = false;
            //CardClosure objCardClosure = new  CardClosure
            //{

            //    CustomerId = objCardReactivation.CustomerId,
            //    CardNumber = objCardReactivation.CardNumber,
            //    IsActive = false
            //};
            //CardClosure objExistingCardClosure = objCardClosureRepository.AllMatching(x => (x.CustomerId == objCardClosure.CustomerId && x.CardNumber == objCardClosure.CardNumber && x.IsActive==true)).FirstOrDefault();
            //if (objExistingCardClosure != null)
            //{
            //    objCardClosure.ClosureID = objExistingCardClosure.ClosureID;
            //    objExistingCardClosure = objCardClosureRepository.Get(objExistingCardClosure.ClosureID);
            //    objCardClosureRepository.Merge(objExistingCardClosure, objCardClosure);
            //    objCustAddressRepository.UnitOfWork.Commit();
            //    isUpdated = true;
            //}
            //return isUpdated;

            if (objCardReactivation == null)
                throw new ArgumentException("");
            try
            {
                bool isUpdated = false;
                SqlParameter[] paramCollection = new SqlParameter[]
                { 
                    new SqlParameter(parameterName: "@P_Cardnumber",dbType:System.Data.SqlDbType.VarChar),
                    new SqlParameter(parameterName: "@P_CustomerId",dbType:System.Data.SqlDbType.Int),
                };

                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Cardnumber")).First()).Value = objCardReactivation.CardNumber;
                ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CustomerId")).First()).Value = objCardReactivation.CustomerId;

                objCardReactivationRepository.UnitOfWork.ExecuteCommand("PR_UpdateCardReactivationAfterApproval @P_Cardnumber,@P_CustomerId", paramCollection);
                isUpdated = true;            
                return isUpdated;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ValidateCardReactivationExists
        public bool ValidateCardReactivationExists(int ACustomerId, string ACardNumber)
        {
            var result = objCardReactivationRepository.AllMatching(x => x.CardNumber == ACardNumber || x.CustomerId == ACustomerId);
            if (result.Any())
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region GetRenegotiationDetails
        public CardMasterDTO GetRenegotiationDetails(string ACardNumber)
        {
            CardMasterDTO  objCardMasterDTO = new CardMasterDTO();

            CardMaster objCardMaster = objCardMasterRepository.AllMatching(x => x.CardNumber == ACardNumber).FirstOrDefault();

            if (objCardMaster != null)
             {
                 return Conversion.Mapping.CardMasterToCardMasterDTO(objCardMaster);
             }
            return objCardMasterDTO;
        }
        #endregion

        #region GetRefinanceDetails
        public CardMasterDTO GetRefinanceDetails(string ACardNumber)
        {
            CardMasterDTO objCardMasterDTO = new CardMasterDTO();
            CardMaster ObjCardMaster = objCardMasterRepository.AllMatching(x => x.CardNumber == ACardNumber).FirstOrDefault();
            if (ObjCardMaster != null)
            {
                return Conversion.Mapping.CardMasterToCardMasterDTO(ObjCardMaster);
            }
            return objCardMasterDTO;
        }
        #endregion

        #region GetAvailableSegmentDetails
        public List<CardCreditSegmentDTO> GetAvailableSegmentDetails(string ARUT, string ACardNumber)
        {
            List<CardCreditSegmentDTO> objCardCreditSegmentDTOList = new List<CardCreditSegmentDTO>();
            var result = objCardCreditSegmentRepository.AllMatching(x => (x.RUT == ARUT && x.CardNumber == ACardNumber));
            if (result.Any())
            {
                result.ToList().ForEach(x => objCardCreditSegmentDTOList.Add(Mapping.CardCreditSegmentToCardCreditSegmentDTO(x)));
            }
            return objCardCreditSegmentDTOList;
        }
        #endregion

        #region  GetCustomerAddress
        public UpdatedCustomerAddressDTO GetCustomerAddress(int ACustomerId, string AAddressTypeCode, string ACardNumber, string ACountryCode)
        {
            UpdatedCustomerAddressDTO objUpdtdCustAddressDTO = new UpdatedCustomerAddressDTO();
            UpdatedCustomerAddress objUpdtdCustAddress = objUpdtdCustAddressRepository.AllMatching(x => (x.CustomerId == ACustomerId && x.AddressTypeCode == AAddressTypeCode && (x.ApprovalStatus == "P" || x.ApprovalStatus == "R"))).FirstOrDefault();
            if (objUpdtdCustAddress == null)
            {
                CustomerAddressDTO objCustAddressDTO = new CustomerAddressDTO();
                CustomerAddress objCustAddress = objCustAddressRepository.AllMatching(x => (x.CustomerId == ACustomerId && x.AddressTypeCode == AAddressTypeCode)).FirstOrDefault();
                if (objCustAddress != null)
                {
                    UpdatedCustomerAddressDTO objUpdtdCustAddrDTO = Conversion.Mapping.CustomerAddressToUpdtdCustomerAddressDTO(objCustAddress);
                    objUpdtdCustAddrDTO.CustomerId = ACustomerId;
                    objUpdtdCustAddrDTO.CardNumber = ACardNumber;
                    objUpdtdCustAddrDTO.CountryCode = ACountryCode;
                    objUpdtdCustAddrDTO.AddressTypeCode = AAddressTypeCode;
                    return objUpdtdCustAddrDTO;
                }
                objUpdtdCustAddressDTO.CustomerId = ACustomerId;
                objUpdtdCustAddressDTO.CardNumber = ACardNumber;
                objUpdtdCustAddressDTO.CountryCode = ACountryCode;
                objUpdtdCustAddressDTO.AddressTypeCode = AAddressTypeCode;
                return objUpdtdCustAddressDTO;
            }
            else
            {                
                return Conversion.Mapping.UpdtdCustomerAddressToUpdtdCustomerAddressDTO(objUpdtdCustAddress);
            }
        }
        #endregion

        #region UpdateChangedCustomerAddress
        public bool UpdateChangedCustomerAddress(string AQueueRefNo)
        {            
            UpdatedCustomerAddress objUpdtCustAddress = objUpdtdCustAddressRepository.AllMatching(x => (x.QueueReferenceNo == AQueueRefNo)).FirstOrDefault();
            bool isUpdated = false;
            CustomerAddress objCurrentCustAddr = new CustomerAddress
            {                
                AddressTypeCode = objUpdtCustAddress.AddressTypeCode,
                CustomerId = objUpdtCustAddress.CustomerId,
                TypeOfResidenceClearance = objUpdtCustAddress.TypeOfResidenceClearance,
                RegionCode = objUpdtCustAddress.RegionCode,
                CityCode = objUpdtCustAddress.CityCode,
                CommunaCode = objUpdtCustAddress.CommunaCode,
                Street = objUpdtCustAddress.Street,
                Number = objUpdtCustAddress.Number,
                Block = objUpdtCustAddress.Block,
                ComplementOfAddress = objUpdtCustAddress.ComplementOfAddress,
                Town = objUpdtCustAddress.Town,
                Landmark = objUpdtCustAddress.Landmark,
                PostalCode = objUpdtCustAddress.PostalCode,
                POBox = objUpdtCustAddress.POBox,
                Phone = objUpdtCustAddress.Phone,
                Mobile = objUpdtCustAddress.Mobile,
                Fax = objUpdtCustAddress.Fax
            };
           CustomerAddress objExistingCustAddress = objCustAddressRepository.AllMatching(x => (x.CustomerId == objUpdtCustAddress.CustomerId && x.AddressTypeCode == objUpdtCustAddress.AddressTypeCode)).FirstOrDefault();
           if (objExistingCustAddress != null)
               objExistingCustAddress = objCustAddressRepository.Get(objExistingCustAddress.AddressId);
            if (objExistingCustAddress == null)
            {
                //Insert a new record in Master table - MT_CustomerAddress      
                objCustAddressRepository.Add(objCurrentCustAddr);
                objCustAddressRepository.UnitOfWork.Commit();
                isUpdated= true;
            }
            else
            {
                //Updating existing record in Master table - MT_CustomerAddress
                objCurrentCustAddr.AddressId = objExistingCustAddress.AddressId;
                objCustAddressRepository.Merge(objExistingCustAddress, objCurrentCustAddr);
                objCustAddressRepository.UnitOfWork.Commit();
                isUpdated= true;
            }
            return isUpdated;
        }
        #endregion

        #region Billing Cycle Change

        public List<BillingCycleDTO> GetValidBillingCycle(string ACardNumber)
        {
            List<BillingCycleDTO> objResult;
            SqlParameter[] paramCollection = new SqlParameter[] 
            { 
                new SqlParameter() { ParameterName = "@P_CardNumber", SqlDbType = SqlDbType.VarChar, Size = 16, 
                    Value = (ACardNumber == null ? "%" : ACardNumber), Direction = ParameterDirection.Input}

            };
            try
            {
                objResult = objCustomerServiceDetailsRepository.UnitOfWork.ExecuteQuery<BillingCycleDTO>("dbo.[PR_BillingCycleGet] @P_CardNumber",
                        paramCollection).ToList();

                if (objResult == null)
                {
                    objResult = new List<BillingCycleDTO>();
                }

                return objResult;
            }
            catch (Exception)
            { return null; }
        }

        public BillingCycleChangeDTO GetBillingCycleStatus(string ACardNumber, int AUpdateId)
        {
            SqlParameter[] paramCollection = new SqlParameter[] 
            { 
                new SqlParameter() { ParameterName = "@P_CardNumber", SqlDbType = SqlDbType.VarChar, Size = 16, 
                    Value = (ACardNumber == null ? "%" : ACardNumber), Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_UpdateId", SqlDbType = SqlDbType.Int, Size = 1, 
                    Value = AUpdateId, Direction = ParameterDirection.Input}
            };
            try
            {
                List<BillingCycleChangeDTO> objResult;
                objResult = objCustomerServiceDetailsRepository.UnitOfWork.ExecuteQuery<BillingCycleChangeDTO>(
                    "dbo.[PR_BillingCycleGetStatus] @P_CardNumber, @P_UpdateId", paramCollection).ToList();
                if ((objResult == null) || (objResult.Count == 0))
                {
                    return new BillingCycleChangeDTO();
                }
                else
                    return objResult[0];
            }
            catch (Exception)
            { return null; }
        }

        /// <summary>
        /// The changed billing cycle is saved to MT_CardMaster table if approved and to approval table if pending
        /// </summary>
        public void BillingCycleSave(BillingCycleChangeDTO ABillingCycleChangeDTO)
        {
            if (ABillingCycleChangeDTO == null)
            {
                throw new ArgumentNullException("");
            }           
 
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted });

            using (scope)
            {
                try
                {
                    SqlParameter[] paramCollection = new SqlParameter[] { };
                    String strApprovalParams = "";

                    if (ABillingCycleChangeDTO.ApprovalStatus != "A") //Add approval queue parameters in all cases other than 'Approved'
                    {
                        paramCollection = CommonManager.GetApprovalQueueProcedureParameters("CS_BillingCycleUpdate", "",
                            "BLGCYCCHG", ABillingCycleChangeDTO.UpdatedBy, "/CustomerService/FillBillingCycle/", "CS");
                        strApprovalParams = "@P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, "
                            + " @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode, ";
                    }

                    List<SqlParameter> lstParam =  new List<SqlParameter>();
                    lstParam = paramCollection.ToList();
                    lstParam.Add(new SqlParameter(parameterName: "@P_CustomerId", dbType: SqlDbType.VarChar));
                    lstParam.Add(new SqlParameter(parameterName: "@P_CardNumber", dbType: SqlDbType.VarChar));
                    lstParam.Add(new SqlParameter(parameterName: "@P_BillingCycleCode", dbType: SqlDbType.VarChar));
                    lstParam.Add(new SqlParameter(parameterName: "@P_ApprovalStatus", dbType: SqlDbType.Char));
                    lstParam.Add(new SqlParameter(parameterName: "@P_UpdateId", dbType: SqlDbType.Char));
                    lstParam.Add(new SqlParameter(parameterName: "@P_UpdatedBy", dbType: SqlDbType.Char));
                    paramCollection = lstParam.ToArray();

                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CustomerId")).First()).Value = ABillingCycleChangeDTO.CustomerId;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CardNumber")).First()).Value = ABillingCycleChangeDTO.CardNumber;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_BillingCycleCode")).First()).Value = ABillingCycleChangeDTO.BillingCycleCode;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ApprovalStatus")).First()).Value = ABillingCycleChangeDTO.ApprovalStatus;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_UpdateId")).First()).Value = ABillingCycleChangeDTO.UpdateId;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_UpdatedBy")).First()).Value = ABillingCycleChangeDTO.UpdatedBy;

                    objTransactionRepository.UnitOfWork.ExecuteCommand("dbo.PR_BillingCycleSave " + strApprovalParams + 
                        " @P_CustomerId, @P_CardNumber, @P_BillingCycleCode, @P_ApprovalStatus, @P_UpdateId, @P_UpdatedBy",
                        paramCollection);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }           

        }
        #endregion

        #region Block/Unblock Card/Segment
        public List<CardCreditSegmentDTO> GetCardCreditSegmentByCardNumber(string ACardNumber)
        {
            List<CardCreditSegmentDTO> objCardCreditSegmentDTOList = new List<CardCreditSegmentDTO>();
            var result=objCardCreditSegmentRepository.AllMatching(x=>(x.CardNumber==ACardNumber && x.ApprovalStatus =="A"));
            if (result.Any())
            {
                result.ToList().ForEach(x => objCardCreditSegmentDTOList.Add(Mapping.CardCreditSegmentToCardCreditSegmentDTO(x)));
            }
            return objCardCreditSegmentDTOList;
        }

        public void BlockCardOrSegment(BlockSegmentCardDTO ABlockSegmentCard)
        {
            if (ABlockSegmentCard == null)
            {
                throw new ArgumentNullException("");
            }
            if (ABlockSegmentCard.SelectedBlockCodeList.Count>0) // if block codes exists
            {
                foreach (string BlockCodeItems in ABlockSegmentCard.SelectedBlockCodeList) // Currently restricted to only one SelectedBlockCode
                {
                    BlockSegmentCard newBlockSegmentCardObj = new BlockSegmentCard();
                    newBlockSegmentCardObj = BlockSegmentCardFactory.CreateBlockSegmentCard(ABlockSegmentCard.CardNumber, ABlockSegmentCard.AccountNumber,
                        ABlockSegmentCard.BlockedType, ABlockSegmentCard.CreditSegmentCode, BlockCodeItems, ABlockSegmentCard.IsBlocked,
                        ABlockSegmentCard.CreatedBy, ABlockSegmentCard.LastUpdatedBy,ABlockSegmentCard.ApprovalStatus);
                    var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                          new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
                    using (scope)
                    {
                        try
                        {
                            objBlockSegmentCardRepository.Add(newBlockSegmentCardObj);
                            objBlockSegmentCardRepository.UnitOfWork.Commit();

                            string AStatusCode = BlockCodeItems; // Currently restricted to only one SelectedBlockCode
                            
                            SqlParameter[] paramCollection = new SqlParameter[] 
                            { 
                                new SqlParameter(){ ParameterName = "@P_OrganizationCode", SqlDbType = System.Data.SqlDbType.VarChar, 
                                        Value = Utils.OrganizationCode, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter(){ ParameterName = "@P_CardNumber", SqlDbType = System.Data.SqlDbType.VarChar, 
                                        Value = ABlockSegmentCard.CardNumber, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter(){ ParameterName = "@P_SegmentCode", SqlDbType = System.Data.SqlDbType.VarChar, 
                                        Value = ABlockSegmentCard.CreditSegmentCode == null ? string.Empty : ABlockSegmentCard.CreditSegmentCode , Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter(){ ParameterName = "@P_BlockFlag", SqlDbType = System.Data.SqlDbType.Bit , 
                                        Value = true, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter(){ ParameterName = "@P_BlockType", SqlDbType = System.Data.SqlDbType.Char, 
                                        Value = "C", Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter(){ ParameterName = "@P_BlockStatusCode", SqlDbType = System.Data.SqlDbType.VarChar, 
                                        Value = AStatusCode, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter(){ ParameterName = "@P_LoginName", SqlDbType = System.Data.SqlDbType.VarChar, 
                                        Value = ABlockSegmentCard.CreatedBy , Direction = System.Data.ParameterDirection.Input }
                            };

                            objCardMasterRepository.UnitOfWork.ExecuteCommand("PR_UpdateCardBlockStatus  @P_OrganizationCode, @P_CardNumber, @P_SegmentCode, @P_BlockFlag, @P_BlockType, @P_BlockStatusCode,@P_LoginName ", paramCollection);
                            objCardMasterRepository.UnitOfWork.Commit();
                            
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }

        public List<BlockSegmentCardDTO> GetBlockcodesByCardNumber(string ACardNumber,string ABlockType,string ASegment)
        {
            List<BlockSegmentCardDTO> objBlocKCodesList = new List<BlockSegmentCardDTO>();
            if (ASegment != null)
            {
                var result = objBlockSegmentCardRepository.AllMatching(x => (x.CardNumber == ACardNumber && x.ApprovalStatus == "A" && x.BlockedType == ABlockType && x.CreditSegmentCode == ASegment && x.IsBlocked==true ));
                if (result.Any())
                {
                    result.ToList().ForEach(x => objBlocKCodesList.Add(Mapping.BlockSegmentCardtoBlockSegmentCardDTO(x)));
                }
            }
            else
            {
                var result = objBlockSegmentCardRepository.AllMatching(x => (x.CardNumber == ACardNumber && x.ApprovalStatus == "A" && x.BlockedType == ABlockType && x.IsBlocked == true));
                if (result.Any())
                {
                    result.ToList().ForEach(x => objBlocKCodesList.Add(Mapping.BlockSegmentCardtoBlockSegmentCardDTO(x)));
                }
            }
            return objBlocKCodesList;
        }

        public List<BlockCodeDTO> GetBlockedCodes(string ACardNumber, string ABlockType, string ASegment)
        {
            
            List<BlockCodeDTO> objBlocKCodesList = new List<BlockCodeDTO>();
             var result= objBlockSegmentCardRepository.GetFiltered(0, 10, Utils.GetExpression<BlockSegmentCard>(new List<Utils.ExpressionList>() { 
                 new Utils.ExpressionList() { FilterColumn = "CardNumber", FilterValue = ACardNumber, Operator = "Equals" },
                 new Utils.ExpressionList(){ FilterColumn="BlockedType", FilterValue = ABlockType, Operator = "Equals"},
                 new Utils.ExpressionList(){ FilterColumn="IsBlocked", FilterValue = true, Operator = "Equals"},
                 new Utils.ExpressionList(){ FilterColumn="ApprovalStatus", FilterValue = "A", Operator = "Equals"}}))
            .Join(objBlockCodeRepository .GetAll(),
                blocksegmentcard => blocksegmentcard.BlockCode,
                blockcode=> blockcode.Code,
                (blocksegmentcard, blockcode) => new
                {
                    blockcode.Description,
                    blocksegmentcard.BlockCode 
                }).Select(x => x);
             if (result.Any())
             {
                 result.ToList().ForEach(x => objBlocKCodesList.Add(new BlockCodeDTO()
                 {
                  Code=x.BlockCode,
                  Description=x.Description
                    
                 }));
             }
            
        return objBlocKCodesList;
        }

        public CardMaster GetCardDetails(string ACardNumber)
        {
            CardMaster objCardMaster = new CardMaster();

            //var result = objCardMasterRepository.GetFiltered(0, 10, Utils.GetExpression<CardMaster>(new List<Utils.ExpressionList>() { 
            //     new Utils.ExpressionList() { FilterColumn = "CardNumber", FilterValue = ACardNumber, Operator = "Equals" }
            //}));
            objCardMaster = objCardMasterRepository.Get(ACardNumber);

            objCardMasterRepository.UnitOfWork.RefreshDataInModel(ref objCardMaster);

            return objCardMaster;
        }

        public List<CreditSegmentDTO> GetBlockedSegments(string ACardNumber, string ABlockType, string ASegment)
         {

             List<CreditSegmentDTO> objCreditSegment = new List<CreditSegmentDTO>();
             var result = objBlockSegmentCardRepository.GetFiltered(0, 10, Utils.GetExpression<BlockSegmentCard>(new List<Utils.ExpressionList>() { 
                 new Utils.ExpressionList() { FilterColumn = "CardNumber", FilterValue = ACardNumber, Operator = "Equals" },
                 new Utils.ExpressionList(){ FilterColumn="BlockedType", FilterValue = ABlockType, Operator = "Equals"},
                 new Utils.ExpressionList(){ FilterColumn="IsBlocked", FilterValue = true, Operator = "Equals"},
                 new Utils.ExpressionList(){ FilterColumn="ApprovalStatus", FilterValue = "A", Operator = "Equals"}}))
            .Join(objCreditSegmentRepository.GetAll(),
                blocksegmentcard => blocksegmentcard.CreditSegmentCode,
               segmentcode => segmentcode.CardSegmentCode,
                (blocksegmentcard, segmentcode) => new
                {
                    segmentcode.CardSegmentCode,
                    segmentcode.CardSegmentName
                }).Select(x => x);
             if (result.Any())
             {
                 result.ToList().ForEach(x => objCreditSegment.Add(new CreditSegmentDTO()
                 {
                     CardSegmentCode = x.CardSegmentCode,
                     CardSegmentName = x.CardSegmentName

                 }));
             }

             return objCreditSegment;
         }
         public void UnblockCardSegment(BlockSegmentCardDTO ABlockSegmentCard)
         {
             if (ABlockSegmentCard == null)
             {
                 throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");
             }
             if (ABlockSegmentCard.SelectedBlockCodeList.Count > 0) // if block codes exists
             {
                 foreach (string BlockCodeItems in ABlockSegmentCard.SelectedBlockCodeList)
                 {

                     List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "CardNumber", FilterValue = ABlockSegmentCard.CardNumber, Operator = "Equals"},
                  new Utils.ExpressionList(){FilterColumn = "BlockedType", FilterValue = ABlockSegmentCard.BlockedType, Operator = "Equals"},
                  new Utils.ExpressionList(){FilterColumn = "BlockCode", FilterValue =BlockCodeItems, Operator = "Equals"},
                  new Utils.ExpressionList(){FilterColumn = "IsBlocked", FilterValue = true, Operator = "Equals"},
                  new Utils.ExpressionList(){ FilterColumn="ApprovalStatus", FilterValue = "A", Operator = "Equals"}
            };

                     Expression<Func<BlockSegmentCard, bool>> expression = Utils.GetExpression<BlockSegmentCard>(lstExp);
                     int iRecordCount = objBlockSegmentCardRepository.GetRecordCount(expression);
                     var vFinancialAccountDTO  = objBlockSegmentCardRepository.GetFiltered(0, iRecordCount, expression);
                     var ABlockSegmentCardId = vFinancialAccountDTO.Select(x => x.BlockSegmentCardId);
                     int Acode = vFinancialAccountDTO.First(a => a.BlockSegmentCardId == a.BlockSegmentCardId).BlockSegmentCardId;
                   
                     BlockSegmentCard currentSegmentCard = objBlockSegmentCardRepository.Get(Acode);
                     
                     BlockSegmentCard updatedSegmentCard = new BlockSegmentCard()
                     {
                         BlockSegmentCardId=Acode,
                         CardNumber=currentSegmentCard.CardNumber,
                         AccountNumber=currentSegmentCard.AccountNumber,
                         BlockedType=currentSegmentCard.BlockedType,
                         CreditSegmentCode=currentSegmentCard.CreditSegmentCode,
                         BlockCode=currentSegmentCard.BlockCode,
                         IsBlocked = false,
                         CreatedOn = currentSegmentCard.CreatedOn,
                         CreatedBy = currentSegmentCard.CreatedBy,
                         ApprovalStatus = ABlockSegmentCard.ApprovalStatus,
                         LastUpdatedOn = Utils.BusinessDate,
                         LastUpdatedBy = ABlockSegmentCard.LastUpdatedBy
                        
                         
                     };
                     var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                     new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
                     using (scope)
                     {
                         try
                         {
                             objBlockSegmentCardRepository.Merge(currentSegmentCard, updatedSegmentCard);
                             objBlockSegmentCardRepository.UnitOfWork.Commit();


                             SqlParameter[] paramCollection = new SqlParameter[] 
                             { 
                                 new SqlParameter(){ ParameterName = "@P_OrganizationCode", SqlDbType = System.Data.SqlDbType.VarChar, 
                                         Value = Utils.OrganizationCode, Direction = System.Data.ParameterDirection.Input },
                                 new SqlParameter(){ ParameterName = "@P_CardNumber", SqlDbType = System.Data.SqlDbType.VarChar, 
                                         Value = ABlockSegmentCard.CardNumber, Direction = System.Data.ParameterDirection.Input },
                                new SqlParameter(){ ParameterName = "@P_SegmentCode", SqlDbType = System.Data.SqlDbType.VarChar, 
                                        Value = ABlockSegmentCard.CreditSegmentCode == null ? string.Empty : ABlockSegmentCard.CreditSegmentCode , Direction = System.Data.ParameterDirection.Input },
                                 new SqlParameter(){ ParameterName = "@P_BlockFlag", SqlDbType = System.Data.SqlDbType.Bit , 
                                         Value = false, Direction = System.Data.ParameterDirection.Input },
                                 new SqlParameter(){ ParameterName = "@P_BlockType", SqlDbType = System.Data.SqlDbType.Char, 
                                         Value = "C", Direction = System.Data.ParameterDirection.Input },
                                 new SqlParameter(){ ParameterName = "@P_BlockStatusCode", SqlDbType = System.Data.SqlDbType.VarChar, 
                                         Value = BlockCodeItems, Direction = System.Data.ParameterDirection.Input },
                                 new SqlParameter(){ ParameterName = "@P_LoginName", SqlDbType = System.Data.SqlDbType.VarChar, 
                                         Value = ABlockSegmentCard.LastUpdatedBy, Direction = System.Data.ParameterDirection.Input }
                             };

                             objCardMasterRepository.UnitOfWork.ExecuteCommand("PR_UpdateCardBlockStatus  @P_OrganizationCode, @P_CardNumber, @P_SegmentCode, @P_BlockFlag, @P_BlockType, @P_BlockStatusCode,@P_LoginName ", paramCollection);
                             objCardMasterRepository.UnitOfWork.Commit();


                             scope.Complete();
                         }
                         catch (Exception ex)
                         {
                             throw ex;
                         }
                     }
                 }
             }

         }

        #endregion

        #region Blocked Card Details
         public List<BlockedCardDetailsDTO> GetBlockedCardDetails(string ACardNumber)
         {
             List<BlockedCardDetailsDTO> objBlockedCardDetails = new List<BlockedCardDetailsDTO>();
             var result = objBlockSegmentCardRepository.AllMatching(Utils.GetExpression<BlockSegmentCard>(new List<Utils.ExpressionList>() { 
                 new Utils.ExpressionList() { FilterColumn = "CardNumber", FilterValue = ACardNumber, Operator = "Equals" },
                 new Utils.ExpressionList(){ FilterColumn="IsBlocked", FilterValue = true, Operator = "Equals"},
                 new Utils.ExpressionList(){ FilterColumn="ApprovalStatus", FilterValue = "A", Operator = "Equals"}}))
              .Join(objBlockCodeRepository.GetAll(),
              blockcode => blockcode.BlockCode,
              blockedcard => blockedcard.Code,
              (blockcode, blockedcard) => new
              {
                  blockcode.BlockCode,
                  blockcode.CreditSegmentCode,
                  blockcode.IsBlocked,
                  blockcode.CreatedOn,
                  blockedcard.ShortDescription,
                  blockedcard.IsAccountBlocked,
                  blockedcard.IsAccountBlockedForFinancialReasons,
                  blockedcard.FinancialHierarchy,
                  blockedcard.IsAccountBlockedForNonFinancialReasons,
                  blockedcard.NonFinancialHierarchy,

              }) .Select(x => x);

            if (result.Any())
            {
                result.ToList().ForEach(x => objBlockedCardDetails.Add(new BlockedCardDetailsDTO()
                {
                    Code =x.BlockCode,
                    ShortDescription = x.IsBlocked == true ? "Blocked" : "UnBlocked",
                    CreatedOn = x.CreatedOn,
                    Description = x.ShortDescription,
                    IsAccountBlocked = x.IsAccountBlocked,
                    IsAccountBlockedForFinancialReasons = x.IsAccountBlockedForFinancialReasons,
                    FinancialHierarchy = x.FinancialHierarchy,
                    IsAccountBlockedForNonFinancialReasons = x.IsAccountBlockedForNonFinancialReasons,
                    NonFinancialHierarchy = x.NonFinancialHierarchy,
                    Segment = getSegmentname(x.CreditSegmentCode)
                }));
            }
             return objBlockedCardDetails;
         }

         public string getSegmentname(string segmentCode)
         {
             string result = null;
             if (segmentCode != null)
             {
                 List<CreditSegment> obj = new List<CreditSegment>();
                 obj = objCreditSegmentRepository.AllMatching(Utils.GetExpression<CreditSegment>(new List<Utils.ExpressionList>() { 
                 new Utils.ExpressionList() { FilterColumn = "CardSegmentCode", FilterValue = segmentCode, Operator = "Equals" }
                })).ToList();
                 result = obj[0].CardSegmentName;
             }
             return result;
         }
        #endregion

        # region Line Utilization

         public List<LineUtilizationDTO> GetLineUtilizationDetails(string ACardNumber, string ASegment)
         {

             List<LineUtilizationDTO> objCreditSegment = new List<LineUtilizationDTO>();
             if (ASegment == "")
             {
                 var result = objCardCreditSegmentRepository.AllMatching(Utils.GetExpression<CardCreditSegment>(new List<Utils.ExpressionList>() { 
                 new Utils.ExpressionList() { FilterColumn = "CardNumber", FilterValue = ACardNumber, Operator = "Equals" } }))
                    .Join(objCreditSegmentRepository.GetAll(),
                    cardcreditsegment => new { cardcreditsegment.CreditSegmentCode, cardcreditsegment.OrganizationCode },
                    segmentcode => new { CreditSegmentCode = segmentcode.CardSegmentCode, OrganizationCode = segmentcode.OrganizationCode },
                    (cardcreditsegment, segmentcode) => new
                    {
                        cardcreditsegment.CreditValue,
                        cardcreditsegment.OTB,
                        segmentcode.CardSegmentCode,
                        segmentcode.CardSegmentName,
                        segmentcode.OrganizationCode
                    }).Where(x => x.OrganizationCode == Utils.OrganizationCode).Select(x => x);

                 if (result.Any())
                 {
                     result.ToList().ForEach(x => objCreditSegment.Add(new LineUtilizationDTO()
                     {
                         Segment = x.CardSegmentName,
                         Limit = x.CreditValue,
                         UtilizedAmount = (x.CreditValue - x.OTB),
                         UtilizedPercentage = ((x.CreditValue - x.OTB) / x.CreditValue) * 100,

                     }));
                 }
             }
             else
             {
                 var result = objCardCreditSegmentRepository.AllMatching(Utils.GetExpression<CardCreditSegment>(new List<Utils.ExpressionList>() { 
                 new Utils.ExpressionList() { FilterColumn = "CardNumber", FilterValue = ACardNumber, Operator = "Equals" },
                 new Utils.ExpressionList() { FilterColumn = "CreditSegmentCode", FilterValue = ASegment, Operator = "Equals" } }))
                   .Join(objCreditSegmentRepository.GetAll(),
                   cardcreditsegment => cardcreditsegment.CreditSegmentCode,
                   segmentcode => segmentcode.CardSegmentCode,
                   (cardcreditsegment, segmentcode) => new
                   {
                       cardcreditsegment.CreditValue,
                       cardcreditsegment.OTB,
                       segmentcode.CardSegmentCode,
                       segmentcode.CardSegmentName,
                       segmentcode.OrganizationCode
                   }).Where(x => x.OrganizationCode == Utils.OrganizationCode).Select(x => x);

                 if (result.Any())
                 {
                     result.ToList().ForEach(x => objCreditSegment.Add(new LineUtilizationDTO()
                     {
                         Segment = x.CardSegmentName,
                         Limit = x.CreditValue,
                         UtilizedAmount = (x.CreditValue - x.OTB),
                         UtilizedPercentage = ((x.CreditValue - x.OTB) / x.CreditValue) * 100,

                     }));
                 }
             }

             return objCreditSegment;
         }
        #endregion
        
        #region Installment Schedule
         public List<InstallmentScheduleDTO> GetInstallmentSchedule1(string ACardNumber)
         {

             List<InstallmentScheduleDTO> objInstallmentScheduleDTO = new List<InstallmentScheduleDTO>();
             var result = objEMITransactionRepository.AllMatching(Utils.GetExpression<EMITransaction>(new List<Utils.ExpressionList>() { 
                 new Utils.ExpressionList() { FilterColumn = "CardNumber", FilterValue = ACardNumber, Operator = "Equals" } }))
                   .Select(x => x);
             if (result.Any())
             {
                 result.ToList().ForEach(x => objInstallmentScheduleDTO.Add(new InstallmentScheduleDTO()
                 {
                   EMISlNo=x.EMISlno,
                   CardNumber = x.CardNumber,
                   InstallmentNumber=x.EMISlno,
                   BookDate=x.LoanBookedDate,
                   Tenure=x.TotalInstallment,
                   RateOfInterest=x.RateOfInterest,
                   Status =  (x.Status == "U" ? "Unbilled" : "Billed"),
                   TransactionCode=x.TransactionCode,
                   AuthNo=x.EMIAuthNo

                 }));
             }
             return objInstallmentScheduleDTO;
         }

         public List<InstallmentScheduleDTO> GetInstallmentSchedule(string ACardNumber)
         {
             //decimal sbifrate = GetSBIFRates(250000);
             List<InstallmentScheduleDTO> objInstallmentScheduleDTO = new List<InstallmentScheduleDTO>();
             var result = objEMITransactionRepository.AllMatching(Utils.GetExpression<EMITransaction>(new List<Utils.ExpressionList>() { 
                 new Utils.ExpressionList() { FilterColumn = "CardNumber", FilterValue = ACardNumber, Operator = "Equals" } })).Select(x => x);
                 
                   
             if (result.Any())
             {
                 result.ToList().ForEach(x => objInstallmentScheduleDTO.Add(new InstallmentScheduleDTO()
                 {
                     EMISlNo = x.EMISlno,
                     CardNumber = x.CardNumber,
                     InstallmentNumber = x.EMISlno,
                     BookDate = x.LoanBookedDate,
                     Tenure = x.TotalInstallment,
                     //RateOfInterest = (x.RateOfInterest / 100) * GetSBIFRates(x.LoanAmount),
                     RateOfInterest = x.RateOfInterest * (x.SBIFInterestRate/100),
                     Status = (x.Status == "U" ? "Unbilled" : "Billed"),
                     TransactionCode = (x.TransactionCode + " - " + objTransactionRepository.Get(Utils.OrganizationCode,x.TransactionCode).Description),
                     AuthNo = x.EMIAuthNo,
                     InstallmentAmount = x.LoanAmount,
                     CreditSegment = getSegmentnameByPlanId(x.PlanId)

                 }));
             }
             return objInstallmentScheduleDTO;
         }


         public string getSegmentnameByPlanId(int Planid)
         {
             string result = null;
             if (Planid != null)
             {
                var obj = objPlanMasterRepository.AllMatching(Utils.GetExpression<PlanMaster>(new List<Utils.ExpressionList>() { new Utils.ExpressionList()
                 { FilterColumn = "PlanId", FilterValue = Planid, Operator = "Equals" } }))
                 .Join(objCreditSegmentRepository.GetAll(),
                 planmaster => planmaster.CreditSegmentCode,
                 creditseg => creditseg.CardSegmentCode,
                 (planmaster, creditseg) => new
                 {

                     creditseg.CardSegmentName
                 }).Select(x => x).ToList();
                 if(obj.Count>0)
                result = obj[0].CardSegmentName;
               
             }
             return result;
         }

         public List<InstallmentScheduleDetailsDTO> GetInstallmentScheduleDetails(string id)
         {
             List<InstallmentScheduleDetailsDTO> objInstallmentDetailsDTO = new List<InstallmentScheduleDetailsDTO>();
             var result = objEMITransactionInstallmentRepository.AllMatching(Utils.GetExpression<EMITransactionInstallment>(new List<Utils.ExpressionList>() { 
                 new Utils.ExpressionList() { FilterColumn = "EMIAuthNo", FilterValue = id, Operator = "Equals" } })).Select(x => x);
                
             if (result.Any())
             {
                 result.ToList().ForEach(x => objInstallmentDetailsDTO.Add(new InstallmentScheduleDetailsDTO()
                 {
                    InstallmentNo = x.InstallmentNo,
                    PrincipalAmount=x.TotalPrincipalAmt,
                    InterestAmount=x.TotalInterestAmt,
                    TotalAmount = x.TotalPrincipalAmt + x.TotalInterestAmt,
                    Status=x.Status,
                    DueOn=x.DueDate
                 }));
             }
            
             return objInstallmentDetailsDTO;
         }
        #endregion

        #region LostCardBlock/Reissue

        public void SaveLostStolenCardBlock(LostStolenCardBlockDTO ALostStolenCardBlockDTO)
        {
            if (ALostStolenCardBlockDTO == null)
            {
                throw new ArgumentNullException("");
            }

            LostStolenCardBlock newLostStolenCardBlock = new LostStolenCardBlock();
            newLostStolenCardBlock = LostStolenCardBlockFactory.CreateLostStolenCardBlock(ALostStolenCardBlockDTO.RUT,  
                ALostStolenCardBlockDTO.CardNumber, ALostStolenCardBlockDTO.IsBlocked,
            ALostStolenCardBlockDTO.CreatedBy, ALostStolenCardBlockDTO.CreatedOn, ALostStolenCardBlockDTO.LastUpdatedBy, 
            ALostStolenCardBlockDTO.LastUpdatedOn);
                       
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {

                    objLostStolenCardBlockRepository.Add(newLostStolenCardBlock);
                    objLostStolenCardBlockRepository.UnitOfWork.Commit();
                    
                    SqlParameter[] paramCollection = new SqlParameter[]
                    { 
                        new SqlParameter(parameterName: "@P_CardNumber",dbType:System.Data.SqlDbType.VarChar),
                        new SqlParameter(parameterName: "@P_Type",dbType:System.Data.SqlDbType.VarChar)
                    };

                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CardNumber")).First()).Value = ALostStolenCardBlockDTO.CardNumber;
                    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Type")).First()).Value = "B";

                    objLostStolenCardBlockRepository.UnitOfWork.ExecuteCommand("PR_UpdateCardMasterLostStolenBlockReissueStatus @P_CardNumber, @P_Type ", paramCollection);
                    
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void SaveLostStolenCardReissueRequest(LostStolenReissueRequestDTO ALostStolenReissueRequestDTO)
        {
            if (ALostStolenReissueRequestDTO == null)
            {
                throw new ArgumentNullException("");
            }

            LostStolenReissueRequest newLostStolenReissueRequestObj = new LostStolenReissueRequest();
            newLostStolenReissueRequestObj = LostStolenReissueRequestFactory.CreateLostStolenReissueRequest(
                    ALostStolenReissueRequestDTO.RUT, ALostStolenReissueRequestDTO.RUTVerificationDigit, ALostStolenReissueRequestDTO.CardNumber,
                    ALostStolenReissueRequestDTO.SerialNumber, ALostStolenReissueRequestDTO.ExpiryDate, ALostStolenReissueRequestDTO.NewContractCode,
                    ALostStolenReissueRequestDTO.CreatedBy, ALostStolenReissueRequestDTO.CreatedOn, ALostStolenReissueRequestDTO.LastUpdatedBy, ALostStolenReissueRequestDTO.LastUpdatedOn,
                    ALostStolenReissueRequestDTO.ApprovalStatus);

            newLostStolenReissueRequestObj.ApprovalStatus = "P";

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {

                    if (string.IsNullOrEmpty(ALostStolenReissueRequestDTO.ApprovalStatus))
                    {
                        objLostStolenReissueRequestRepository.Add(newLostStolenReissueRequestObj);
                        objLostStolenReissueRequestRepository.UnitOfWork.Commit();

                        ALostStolenReissueRequestDTO.ApprovalStatusdesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ALostStolenReissueRequestDTO.ApprovalStatus);
                    }
                    else if (ALostStolenReissueRequestDTO.ApprovalStatus == "R")
                    {
                        LostStolenReissueRequest objLostStolenReissueRequestexisting = objLostStolenReissueRequestRepository.AllMatching(x => (x.LostStolenCardReissueRequestId == ALostStolenReissueRequestDTO.LostStolenCardReissueRequestId && x.RUT == ALostStolenReissueRequestDTO.RUT && x.ApprovalStatus == "R")).FirstOrDefault();

                        if (objLostStolenReissueRequestexisting != null)
                            objLostStolenReissueRequestexisting = objLostStolenReissueRequestRepository.Get(objLostStolenReissueRequestexisting.LostStolenCardReissueRequestId);
                        newLostStolenReissueRequestObj.LostStolenCardReissueRequestId = objLostStolenReissueRequestexisting.LostStolenCardReissueRequestId;

                        objLostStolenReissueRequestRepository.Merge(objLostStolenReissueRequestexisting, newLostStolenReissueRequestObj);
                        objCardReactivationRepository.UnitOfWork.Commit();
                    }
                    
                    SqlParameter[] paramCollection = new SqlParameter[] { };

                    paramCollection = CommonManager.GetApprovalQueueProcedureParameters("CS_LostStolenCardReissueRequest", " LostStolenCardReissueRequestId = '" + newLostStolenReissueRequestObj.LostStolenCardReissueRequestId + "'",
                        "LSTCRDREQ", newLostStolenReissueRequestObj.LastUpdatedBy, "/CustomerService/LostStolenCardReissueRequestAuthorize/" + newLostStolenReissueRequestObj.LostStolenCardReissueRequestId + "/A?", "CS");

                    objUpdtdCustAddressRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }

        public LostStolenReissueRequestDTO GetLostStolenReissueRequestDTOForApproval(int ALostStolenReissueRequestID)
        {
            LostStolenReissueRequestDTO objLostStolenReissueRequestDTO = new LostStolenReissueRequestDTO();

            if (objLostStolenReissueRequestRepository.Get(ALostStolenReissueRequestID) != null)
            {
                var result = objLostStolenReissueRequestRepository.AllMatching(x => x.LostStolenCardReissueRequestId == ALostStolenReissueRequestID);
                if (result.Any())
                {
                    objLostStolenReissueRequestDTO.CardNumber = result.FirstOrDefault().CardNumber;
                    objLostStolenReissueRequestDTO.RUT = result.FirstOrDefault().RUT;
                    objLostStolenReissueRequestDTO.SerialNumber = result.FirstOrDefault().SerialNumber;
                    objLostStolenReissueRequestDTO.RUTVerificationDigit = result.FirstOrDefault().RUTVerificationDigit;
                    objLostStolenReissueRequestDTO.NewContractCode = result.FirstOrDefault().NewContractCode;
                    objLostStolenReissueRequestDTO.ExpiryDate = result.FirstOrDefault().ExpiryDate;
                    objLostStolenReissueRequestDTO.ApprovalStatus = result.FirstOrDefault().ApprovalStatus;
                    objLostStolenReissueRequestDTO.ApprovalStatusdesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(result.FirstOrDefault().ApprovalStatus);

                }
            }
            return objLostStolenReissueRequestDTO;
        }

        public string GetLostStolenReissueRequestCardNumber(string AQRefID)
        {

            var LostStolenReissueRequest = objLostStolenReissueRequestRepository.GetAll().Where(x => x.QueueReferenceNo == AQRefID).FirstOrDefault();
            return LostStolenReissueRequest.CardNumber;
        }

        public LostStolenReissueRequestDTO GetLostStolenReissueRequestDTOForApproval(string ACardNumber)
        {
            LostStolenReissueRequestDTO objLostStolenReissueRequestDTO = new LostStolenReissueRequestDTO();

            var result = objLostStolenReissueRequestRepository.AllMatching(Utils.GetExpression<LostStolenReissueRequest>(new List<Utils.ExpressionList>() { 
                 new Utils.ExpressionList() { FilterColumn = "CardNumber", FilterValue = ACardNumber, Operator = "Equals" }}));

            if (result.Any())
            {
                objLostStolenReissueRequestDTO.CardNumber = result.FirstOrDefault().CardNumber;
                objLostStolenReissueRequestDTO.RUT = result.FirstOrDefault().RUT;
                objLostStolenReissueRequestDTO.SerialNumber = result.FirstOrDefault().SerialNumber;
                objLostStolenReissueRequestDTO.RUTVerificationDigit = result.FirstOrDefault().RUTVerificationDigit;
                objLostStolenReissueRequestDTO.NewContractCode = result.FirstOrDefault().NewContractCode;
                objLostStolenReissueRequestDTO.ExpiryDate = result.FirstOrDefault().ExpiryDate;
                objLostStolenReissueRequestDTO.ApprovalStatus = result.FirstOrDefault().ApprovalStatus;
                objLostStolenReissueRequestDTO.ApprovalStatusdesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(result.FirstOrDefault().ApprovalStatus);

            }
            else
            {
                objLostStolenReissueRequestDTO = null;
            }

            return objLostStolenReissueRequestDTO;
        }

        public void UpdateCardMasterLostStolenReissueReqStatus(string ACardNumber)
        {

            SqlParameter[] paramCollection = new SqlParameter[]
                    { 
                        new SqlParameter(parameterName: "@P_CardNumber",dbType:System.Data.SqlDbType.VarChar),
                        new SqlParameter(parameterName: "@P_Type",dbType:System.Data.SqlDbType.VarChar)
                    };

            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CardNumber")).First()).Value = ACardNumber;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Type")).First()).Value = "R";


            objLostStolenCardBlockRepository.UnitOfWork.ExecuteCommand("PR_UpdateCardMasterLostStolenBlockReissueStatus @P_CardNumber, @P_Type ", paramCollection);
        }

        #endregion

        #region GetConversionTableList
        public List<ConversionTableDTO> GetConversionTableList(string ACardNumber)
        {
     
            List<ConversionTableDTO> objConversionTableList = new List<ConversionTableDTO>();

            objConversionTableList = objCustomerServiceDetailsRepository.UnitOfWork.ExecuteQuery<ConversionTableDTO>("PR_ConversionTableList @P_OrganizationCode, @P_CardNumber",
                            new SqlParameter("@P_OrganizationCode", Utils.OrganizationCode), new SqlParameter("@P_CardNumber", ACardNumber)).ToList();

            return objConversionTableList;

        }
        #endregion

        #region CardActivateSave
        public bool CardActivateSave(string ACardNumber, string ALoggedInUser)
        {
            try
            {
                string AStatusCode = "00";
                CardMaster objExistingCardMaster = objCardMasterRepository.Get(ACardNumber);
                CardMaster objUpdatedCardMaster = objExistingCardMaster;

                objUpdatedCardMaster.CardStatusCode = AStatusCode;
                objUpdatedCardMaster.BlockStatusCode = AStatusCode;
                objUpdatedCardMaster.CardActivatedDate = Utils.BusinessDate;
                objUpdatedCardMaster.LastUpdatedBy = ALoggedInUser;
                objUpdatedCardMaster.LastUpdatedOn = Utils.BusinessDate;          

               // objCardMasterRepository.Merge(objExistingCardMaster, objUpdatedCardMaster);
               // objCardMasterRepository.UnitOfWork.Commit();

                SqlParameter[] objSqlParameter = new SqlParameter[] { 
                new SqlParameter(){ ParameterName = "@P_Cardnumber", Value = ACardNumber, SqlDbType = System.Data.SqlDbType.VarChar },
                new SqlParameter(){ ParameterName ="@p_loginname", Value = ALoggedInUser, SqlDbType = System.Data.SqlDbType.VarChar }
              
            };

                //objCustomerRepository.UnitOfWork.ExecuteCommand("PR_UpdateCustomerApplicationReferenceNo @P_CustomerId,@P_OrganizationCode,@R_ApplicationRefNo OUTPUT", paramApplicationRefNo);
                objCardMasterRepository.UnitOfWork.ExecuteCommand("PR_CardActivation @P_Cardnumber,@p_loginname", objSqlParameter);
                objCardMasterRepository.UnitOfWork.Commit();

            }
            catch 
            {
                return false;
            }            
            //if (objCardReactivation == null)
            //    throw new ArgumentException("");
            //try
            //{
            //    bool isUpdated = false;
            //    SqlParameter[] paramCollection = new SqlParameter[]
            //    { 
            //        new SqlParameter(parameterName: "@P_Cardnumber",dbType:System.Data.SqlDbType.VarChar),
            //        new SqlParameter(parameterName: "@P_CustomerId",dbType:System.Data.SqlDbType.Int),
            //    };

            //    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Cardnumber")).First()).Value = objCardReactivation.CardNumber;
            //    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_CustomerId")).First()).Value = objCardReactivation.CustomerId;

            //    objCardReactivationRepository.UnitOfWork.ExecuteCommand("PR_UpdateCardReactivationAfterApproval @P_Cardnumber,@P_CustomerId", paramCollection);
            //    isUpdated = true;
            //    return isUpdated;
            //}

            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return true;
        }
        #endregion

        #region ChangeCreditLimit
        public void SaveChangedCreditLimit(ChangeCreditLimitDTO AChangeCreditLimitDTO)
        {
            if (AChangeCreditLimitDTO == null)
            {
                throw new ArgumentNullException("");
            }

            ChangeCreditLimit objChangeCreditLimit = new ChangeCreditLimit();
            objChangeCreditLimit = ChangeCreditLimitFactory.CreateChangeCreditLimit(
                AChangeCreditLimitDTO.CustomerId, AChangeCreditLimitDTO.CardNumber, AChangeCreditLimitDTO.NewCreditLimit, 
                AChangeCreditLimitDTO.CreatedBy, AChangeCreditLimitDTO.LastUpdatedBy, AChangeCreditLimitDTO.ApprovalStatus
                );

            objChangeCreditLimit.ApprovalStatus = "P";
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {

                    objChangeCreditLimitRepository.Add(objChangeCreditLimit);
                    objChangeCreditLimitRepository.UnitOfWork.Commit();
                    
                    SqlParameter[] paramCollection = new SqlParameter[] { };

                    paramCollection = CommonManager.GetApprovalQueueProcedureParameters("CS_ChangeCreditLimit", " ChangeCreditLimitID = '" + objChangeCreditLimit.ChangeCreditLimitID + "'",
                        "CRLMTCHG", objChangeCreditLimit.LastUpdatedBy, "/CustomerService/ChangeCreditLimitAuthorize/" + objChangeCreditLimit.ChangeCreditLimitID + "/A?", "CS");

                    objChangeCreditLimitRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public ChangeCreditLimitDTO GetChangeCreditLimitForApproval(int AChangeCreditLimitID)
        {
            ChangeCreditLimitDTO objChangeCreditLimitDTO = new ChangeCreditLimitDTO();

            if (objChangeCreditLimitRepository.Get(AChangeCreditLimitID) != null)
            {
                var result = objChangeCreditLimitRepository.AllMatching(x => x.ChangeCreditLimitID == AChangeCreditLimitID);
                if (result.Any())
                {
                    objChangeCreditLimitDTO.CardNumber = result.FirstOrDefault().CardNumber;
                    objChangeCreditLimitDTO.CustomerId = result.FirstOrDefault().CustomerId;
                    objChangeCreditLimitDTO.NewCreditLimit = result.FirstOrDefault().CreditLimit;
                    objChangeCreditLimitDTO.ApprovalStatus = result.FirstOrDefault().ApprovalStatus;

                }
            }
            return objChangeCreditLimitDTO;
        }

        public ChangeCreditLimitDTO GetChangeCreditLimitDetails(string AQRefID)
        {
            var changeCreditLimitDTO = objChangeCreditLimitRepository.GetAll().Where(x => x.QueueReferenceNo == AQRefID).FirstOrDefault();
            
            ChangeCreditLimitDTO objChangeCreditLimitDTO = new ChangeCreditLimitDTO();

            if (objChangeCreditLimitRepository.Get(changeCreditLimitDTO.ChangeCreditLimitID) != null)
            {
                objChangeCreditLimitDTO.CardNumber = changeCreditLimitDTO.CardNumber;
                objChangeCreditLimitDTO.CustomerId = changeCreditLimitDTO.CustomerId;
                objChangeCreditLimitDTO.NewCreditLimit = changeCreditLimitDTO.CreditLimit;
                objChangeCreditLimitDTO.ApprovalStatus = changeCreditLimitDTO.ApprovalStatus;
            }
            return objChangeCreditLimitDTO;
        }

        public ChangeCreditLimitDTO GetChangeCreditLimitDetailsByCardNumber(string ACardNumber)
        {
            var changeCreditLimitDTO = objChangeCreditLimitRepository.GetAll().Where(x => x.CardNumber == ACardNumber && x.ApprovalStatus == "P").FirstOrDefault();

            ChangeCreditLimitDTO objChangeCreditLimitDTO = new ChangeCreditLimitDTO();

            if (changeCreditLimitDTO != null)
            {
                objChangeCreditLimitDTO.CardNumber = changeCreditLimitDTO.CardNumber;
                objChangeCreditLimitDTO.CustomerId = changeCreditLimitDTO.CustomerId;
                objChangeCreditLimitDTO.NewCreditLimit = changeCreditLimitDTO.CreditLimit;
                objChangeCreditLimitDTO.ApprovalStatus = changeCreditLimitDTO.ApprovalStatus;
            }
            return objChangeCreditLimitDTO;
        }

        public void UpdateCardMasterCreditLimit(string ACardNumber, decimal ACreditLimit, string ALoggedInUser)
        {
            /*
            CardMaster objExistingCardMaster = objCardMasterRepository.Get(ACardNumber);
            CardMaster objUpdatedCardMaster = objExistingCardMaster;

            objUpdatedCardMaster.CreditLimit = ACreditLimit;
            objUpdatedCardMaster.CreditOTB = ACreditLimit - objExistingCardMaster.UtilizedOTB;
            objUpdatedCardMaster.LastUpdatedBy = ALoggedInUser;
            objUpdatedCardMaster.LastUpdatedOn = Utils.BusinessDate;

            objCardMasterRepository.Merge(objExistingCardMaster, objUpdatedCardMaster);
            objCardMasterRepository.UnitOfWork.Commit();
             * */

            SqlParameter[] paramCollection = new SqlParameter[] 
            { 
                new SqlParameter() { ParameterName = "@P_CardNumber", SqlDbType = SqlDbType.VarChar, Size = 25, Value = ACardNumber, Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_NewCreditLimit", SqlDbType = SqlDbType.Decimal, Size = 15, Value = ACreditLimit, Direction = ParameterDirection.Input },
                new SqlParameter() { ParameterName = "@P_UpdatedBy", SqlDbType = SqlDbType.VarChar, Size = 15, Value = ALoggedInUser, Direction = ParameterDirection.Input },
            };
            try
            {
                objSwapProductRepository.UnitOfWork.ExecuteCommand("PR_UpdateCreditLimit @P_CardNumber,@P_NewCreditLimit,@P_UpdatedBy", paramCollection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Common Methods
        public decimal GetSBIFRates(decimal Amount)
         {
             decimal SBIFRate;

             SqlParameter[] paramCollection = new SqlParameter[]
                { 
                    new SqlParameter(parameterName: "@P_AMOUNT",dbType:System.Data.SqlDbType.Decimal),
                     new SqlParameter(parameterName: "@P_OrganizationCode",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter(){ParameterName = "@P_SBIFRate", SqlDbType = System.Data.SqlDbType.Money, Direction = System.Data.ParameterDirection.Output, Size = 5}
                };

             ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_AMOUNT")).First()).Value = Amount;
             ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_OrganizationCode")).First()).Value = Utils.OrganizationCode ;

             objEMITransactionRepository.UnitOfWork.ExecuteCommand("PR_GetSBIFInterestRate @P_AMOUNT,@P_OrganizationCode, @P_SBIFRate OUTPUT", paramCollection);

             SBIFRate = Convert.ToDecimal(paramCollection[2].Value);

             return SBIFRate;
         }
        #endregion

        #region GetUnbilledTransactionDetailsToList
        public List<UnbilledTransactionsDTO> GetUnbilledTransactionDetailsToList(string ACardNumber, string ATransactionType)
        {
            List<UnbilledTransactionsDTO> objUnbilledTrans;

            SqlParameter[] paramCollection = new SqlParameter[] 
            { 
                new SqlParameter() { ParameterName = "@P_CardNumber", SqlDbType = SqlDbType.VarChar, Size = 16, Value = ACardNumber, Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_TransactionType", SqlDbType = SqlDbType.VarChar, Size = 25, Value = ATransactionType, Direction = ParameterDirection.Input }
            };
            try
            {
                objUnbilledTrans = objCustomerServiceDetailsRepository.UnitOfWork.ExecuteQuery<UnbilledTransactionsDTO>("PR_GetUnbilledTransactionDetailsOfDiffTransType @P_CardNumber,@P_TransactionType", paramCollection).ToList();
                if (objUnbilledTrans == null)
                {
                    objUnbilledTrans = new List<UnbilledTransactionsDTO>();
                }
            }
            catch (Exception)
            {
                return null;
            }
            return objUnbilledTrans;
        } 
        #endregion

        #region Swap Product
        #region SaveSwapProduct
        public void SaveSwapProduct(SwapProductDTO ASwapProductDTO)
        {
            if (ASwapProductDTO == null)
            {
                throw new ArgumentNullException("");
            }

            SwapProduct objSwapProduct = new SwapProduct();
            objSwapProduct = SwapProductFactory.CreateSwapProduct(
                ASwapProductDTO.NewProductCode, ASwapProductDTO.NewCreditLimit, ASwapProductDTO.CustomerId, ASwapProductDTO.CardNumber,
                ASwapProductDTO.CreatedBy, ASwapProductDTO.CreatedOn, ASwapProductDTO.LastUpdatedBy, ASwapProductDTO.LastUpdatedOn, ASwapProductDTO.ApprovalStatus);

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objSwapProductRepository.Add(objSwapProduct);
                    objSwapProductRepository.UnitOfWork.Commit();

                    SqlParameter[] paramCollection = new SqlParameter[] { };

                    paramCollection = CommonManager.GetApprovalQueueProcedureParameters("CS_SwapProduct", " SwapProductID = '" + objSwapProduct.SwapProductID + "'",
                        "SWPPDT", objSwapProduct.LastUpdatedBy, "/CustomerService/SwapProductAuthorize/" + objSwapProduct.SwapProductID + "/A?", "CS");

                    objSwapProductRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        } 
        #endregion

        #region SelectSwapProductByCardNumber
        public SwapProductDTO SelectSwapProductByCardNumber(string ACardNumber, int ACustomerId)
        {
            SwapProductDTO objSwapProductDTO;

            var result = objSwapProductRepository.GetAll().Where(x => x.CustomerId == ACustomerId && x.CardNumber == ACardNumber && x.ApprovalStatus == "P").FirstOrDefault();

            if (result != null)
            {
                objSwapProductDTO = new SwapProductDTO()
                {
                    CustomerId = result.CustomerId,
                    CardNumber = result.CardNumber,
                    NewProductCode = result.NewProductCode,
                    NewCreditLimit = result.NewCreditLimit,
                    ApprovalStatus = result.ApprovalStatus
                };
            }
            else
            {
                objSwapProductDTO = new SwapProductDTO()
                {
                    NewProductCode = string.Empty
                };
            }
            return objSwapProductDTO;
        } 
        #endregion

        #region GetSwapProductById
        public SwapProductDTO GetSwapProductById(int ASwapProductId)
        {
            SwapProductDTO objSwapProductDTO = new SwapProductDTO();
            var result = objSwapProductRepository.Get(ASwapProductId);
            if (result != null)
            {
                objSwapProductDTO.CardNumber = result.CardNumber;
                objSwapProductDTO.CustomerId = result.CustomerId;
                objSwapProductDTO.NewProductCode = result.NewProductCode;
                objSwapProductDTO.NewCreditLimit = result.NewCreditLimit;
                objSwapProductDTO.ApprovalStatus = result.ApprovalStatus;

            }

            return objSwapProductDTO;
        }        
        #endregion

        #region SwapProductOnApproval
        public void SwapProductOnApproval(string AQueueRefNo, string AUserName)
        {
            SqlParameter[] paramCollection = new SqlParameter[] 
            { 
                new SqlParameter() { ParameterName = "@P_QueueReferenceNo", SqlDbType = SqlDbType.VarChar, Size = 25, Value = AQueueRefNo, Direction = ParameterDirection.Input},
                new SqlParameter() { ParameterName = "@P_UserName", SqlDbType = SqlDbType.VarChar, Size = 15, Value = AUserName, Direction = ParameterDirection.Input }
            };
            try
            {
                objSwapProductRepository.UnitOfWork.ExecuteCommand("PR_SwapProductAfterApproval @P_QueueReferenceNo,@P_UserName", paramCollection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion
        #endregion

        #region GetFinancialSummaryDetails
        public List<FinancialSummaryDTO> GetFinancialSummaryDetails(string ACardNumber)
        {
            List<FinancialSummaryDTO> objFinancialSummary;

            SqlParameter[] paramCollection = new SqlParameter[] 
            { 
                new SqlParameter() { ParameterName = "@P_CardNumber", SqlDbType = SqlDbType.VarChar, Size = 16, Value = ACardNumber, Direction = ParameterDirection.Input},
            };
            try
            {
                objFinancialSummary = objCustomerServiceDetailsRepository.UnitOfWork.ExecuteQuery<FinancialSummaryDTO>("PR_GetFinancialSummaryDetails @P_CardNumber ", paramCollection).ToList();
                if (objFinancialSummary == null)
                {
                    objFinancialSummary = new List<FinancialSummaryDTO>();
                }
            }
            catch (Exception)
            {
                return null;
            }
            return objFinancialSummary;
        }
        #endregion

        #region GetUnbilledTransactionDetails
        public List<AdjustmentDTO> GetAdjustmentDetails(string ACardNumber)
        {
            List<AdjustmentDTO> objAdjustmentList = new List<AdjustmentDTO>();

            objAdjustmentList = objCustomerServiceDetailsRepository.UnitOfWork.ExecuteQuery<AdjustmentDTO>("PR_GetAdjustmentDetails @P_CardNumber",
                            new SqlParameter("P_CardNumber", ACardNumber)).ToList();

            return objAdjustmentList;
        }
        #endregion

        #region GetOverDueSummaryDetails
        public List<OverDueSummaryDTO> GetOverDueSummaryDetails(string ACardNumber)
        {
            List<OverDueSummaryDTO> objOverDueSummary;

            SqlParameter[] paramCollection = new SqlParameter[] 
            { 
                new SqlParameter() { ParameterName = "@P_CardNumber", SqlDbType = SqlDbType.VarChar, Size = 16, Value = ACardNumber, Direction = ParameterDirection.Input},
            };
            try
            {
                objOverDueSummary = objCustomerServiceDetailsRepository.UnitOfWork.ExecuteQuery<OverDueSummaryDTO>("PR_GetOverDueSummaryDetails @P_CardNumber ", paramCollection).ToList();
                if (objOverDueSummary == null)
                {
                    objOverDueSummary = new List<OverDueSummaryDTO>();
                }
            }
            catch (Exception)
            {
                return null;
            }
            return objOverDueSummary;
        }
        #endregion


        #region GetOverDueSummaryDetails
        public List<UserActivationBlockedDetails> GetUserActivationBlockedDetails(string ACardNumber)
        {
            List<UserActivationBlockedDetails> objOverDueSummary;

            SqlParameter[] paramCollection = new SqlParameter[] 
            { 
                new SqlParameter() { ParameterName = "@P_CardNumber", SqlDbType = SqlDbType.VarChar, Size = 16, Value = ACardNumber, Direction = ParameterDirection.Input},
            };
            try
            {
                objOverDueSummary = objCustomerServiceDetailsRepository.UnitOfWork.ExecuteQuery<UserActivationBlockedDetails>("PR_CardMakerDetails @P_CardNumber ", paramCollection).ToList();
                if (objOverDueSummary == null)
                {
                    objOverDueSummary = new List<UserActivationBlockedDetails>();
                }
            }
            catch (Exception)
            {
                return null;
            }
            return objOverDueSummary;
        }
        #endregion
    }
}
