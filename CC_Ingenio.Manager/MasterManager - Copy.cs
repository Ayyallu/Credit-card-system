using CC_Ingenio.Core.MasterRepo.MasterAggregate;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.Repository.MasterRepo;
using CC_Ingenio.Repository.CardBookingRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Data.SqlClient;
using CC_Ingenio.Common.Utils;
using System.Transactions;
using CC_Ingenio.Repository.LookUpRepo;
using CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate;
using CC_Ingenio.DataTranfer.LookUpInfo;
using CC_Ingenio.Core.LookUpRepo;
using CC_Ingenio_Resources;

namespace CC_Ingenio.Manager
{
    public class MasterManager : IMasterManager
    {

        #region Variables
        CardCreditSegmentRepository objCardCreditSegmentRepository;
        PlanMasterRepository objPlanMasterRepository;
        CurrencyRepository objCurrencyRepository;
        OrganizationRepository objOrganizationRepository;
        FranchiseRepository objFranchiseRepository;
        SourcingRepository objSourcingRepository;
        CompanyRepository objCompanyRepository;
        CityRepository objCityRepository;
        CreditSegmentRepository objCreditSegmentRepository;
        CreditSubSegmentRepository objCreditSubSegmentRepository;
        SystemParameterRepository objSystemParameterRepository;
        DesignationRepository objDesignationRepository;
        CustomerSegmentRepository objCustomerSegmentRepository;
        CustomerSubSegmentRepository objCustomerSubSegmentRepository;
        CardStatusRepository objCardStatusRepository;
        GeneralRepository objGeneralRepository;
        ContractRepository objContractRepository;
        ProductTypeRepository objProductTypeRepository;
        TransactionRepository objTransactionRepository;
        SystemMaintenanceConfigurationRepository objSystemMaintenanceConfigurationRepository;
        BoostMaintenanceRepository objBoostMaintenanceRepository;
        FeeRepository objFeeRepository;
        FeeTypeRepository objFeeTypeRepository;
        ProductRepository objProductRepository;
        BlockCodeRepository objBlockCodeRepository;
        CustomerRepository objCustomerRepository;
        CalendarDateRepository objCalendarDateRepository;
        TransactionSubCodeRepository objTransactionSubCodeRepository;

        CustomerAddressRepository objCustomerAddressRepository;
        CustomerOccupationRepository objCustomerOccupationRepository;
        CustomerCurrentAccountDetailRepository objCustomerCurrentAccountDetailRepository;
        CustomerCreditCardDetailRepository objCustomerCreditCardDetailRepository;
        CustomerCommericalHouseConsumptionRepository objCustomerCommericalHouseConsumptionRepository;
        CustomerBillingDetailRepository objCustomerBillingDetailRepository;
        CustomerDependentRepository objCustomerDependentRepository;
        CustomerDecisionRepository objCustomerDecisionRepository;

        FinancialAccountRepository objFinancialAccountRepository;
        BillingCycleParameterRepository objBillingCycleParameterRepository;
        BillingParameterRepository objBillingParameterRepository;
        SBIFInterestRepository objSBIFInterestRepository;
        SBIFInterestDetailsRepository objSBIFInterestDetailsRepository;
        InterestMasterRepository objInterestMasterRepository;
        CommissionRepository objCommissionRepository;
        CardMasterRepository objCardMasterRepository;
        CommissionChargeRepository objCommissionChargeRepository;
        CommissionChargeDetailsRepository objCommissionChargeDetailsRepository;
        BlockCodeForCommissionExclusionRepository objBlockCodeForCommissionExclusionRepository;
        DiscountOnMgmtCollectionRepository objDiscountOnMgmtCollectionRepository;
        DiscountOnMgmtCollectionDetailsRepository objDiscountOnMgmtCollectionDetailsRepository;
        CollectionChargeRepository objCollectionChargeRepository;
        CollectionChargeDetailsRepository objCollectionChargeDetailsRepository;
        InsuranceChargeRepository objInsuranceChargeRepository;
        StatementGroupingRepository objStatementGroupingRepository;
        PaymentAllocationTypeRepository objPaymentAllocationTypeRepository;

        ProductCategoryRepository objProductCategoryRepository;
        ProductSubCategoryRepository objProductSubCategoryRepository;
        CompanyCategoryRepository objCompanyCategoryRepository;
        LegalEntityRepository objLegalEntityRepository;
        CountryRepository objCountryRepository;
        DecisionRepository objDecisionRepository;

        BranchRepository objBranchRepository;
        CardTypeRepository objCardTypeRepository;
        FinancialActivityRepository objFinancialActivityRepository;
        ContractTypeRepository objContractTypeRepository;
        NetworkRepository objNetworkRepository;
        BillingCycleRepository objBillingCycleRepository;
        DefaultAffinityRepository objDefaultAffinityRepository;
        InterestMethodRepository objInterestMethodRepository;
        ProductClassRepository objProductClassRepository;
        ProductMemberTypeRepository objProductMemberTypeRepository;
        ProductStatusRepository objProductStatusRepository;
        ServiceRepository objServiceRepository;

        CityCategoryRepository objCityCategoryRepository;
        RegionRepository objRegionRepository;
        HubRepository objHubRepository;
        SpokeRepository objSpokeRepository;
        CommunaRepository objCommunaRepository;
        ContractTemplateRepository objContractTemplateRepository;
        DiscountPercentageRepository objDiscountPercentageRepository;
        DiscountPercentageDetailsRepository objDiscountPercentageDetailsRepository;
        InterestMasterDetailsRepository objInterestMasterDetailsRepository;
        PositionRepository objPositionRepository;

        FeeTypesRepository objFeeTypesRepository;
        MerchantsRepository objMerchantsRepository;
        ChangeBusinessDateRepository objChangeBusinessDateRepository;
        DirectSalesAgentRepository objDirectSalesAgentRepository;
        UFRateSettingsRepository objUFRateSettingsRepository;
        SalesIdRepository objSalesIdRepository;
        MerchantDiscountRepository objMerchantDiscountRepository;
        TransactionCheckFrequencyRepository objTransactionCheckFrequencyRepository;
        AdjustmentFinancialAccountRepository objAdjustmentFinancialAccountRepository;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ACurrencyRepository">Repository to be used </param>
        public MasterManager(CurrencyRepository ACurrencyRepository, SourcingRepository ASourcingRepository, CompanyRepository ACompanyRepository,
                OrganizationRepository AOrganizationRepository, FranchiseRepository AFranchiseRepository, CityRepository ACityRepository,
                CreditSegmentRepository ACreditSegmentRepository, SystemParameterRepository ASystemParameterRepository,
                CustomerSegmentRepository ACustomerSegmentRepository, CustomerSubSegmentRepository ACustomerSubSegmentRepository,
                DesignationRepository ADesignationRepository, CardStatusRepository ACardStatusRepository, GeneralRepository AGeneralRepository,
                ContractRepository AContractRepository, ProductTypeRepository AProductTypeRepository, TransactionRepository ATransactionRepository,
                BoostMaintenanceRepository ABoostMaintenanceRepository, FeeRepository AFeeRepository, ProductRepository AProductRepository,
                BlockCodeRepository ABlockCodeRepository, CustomerRepository ACustomerRepository, CustomerAddressRepository ACustomerAddressRepository,
                CustomerOccupationRepository ACustomerOccupationRepository, CustomerCurrentAccountDetailRepository ACustomerCurrentAccountDetailRepository,
                CustomerCreditCardDetailRepository ACustomerCreditCardDetailRepository, CustomerCommericalHouseConsumptionRepository ACustomerCommericalHouseConsumptionRepository,
                CustomerBillingDetailRepository ACustomerBillingDetailRepository, CustomerDependentRepository ACustomerDependentRepository, CustomerDecisionRepository ACustomerDecisionRepository,
                CalendarDateRepository ACalendarDateRepository, TransactionSubCodeRepository ATransactionSubCodeRepository,
                FeeTypeRepository AFeeTypeRepository, FinancialAccountRepository AFinancialAccountRepository,
                BillingCycleParameterRepository ABillingCycleParameterRepository, SBIFInterestRepository ASBIFInterestRepository,
                InterestMasterRepository AInterestMasterRepository, CommissionRepository ACommissionRepository,
                CardMasterRepository ACardMasterRepository, CommissionChargeRepository ACommissionChargeRepository,
                CommissionChargeDetailsRepository ACommissionChargeDetailsRepository, BlockCodeForCommissionExclusionRepository ABlockCodeForCommissionExclusionRepository,
                DiscountOnMgmtCollectionRepository ADiscountOnMgmtCollectionRepository, CollectionChargeRepository ACollectionChargeRepository,
                InsuranceChargeRepository AInsuranceChargeRepository, StatementGroupingRepository AStatementGroupingRepository,
                PaymentAllocationTypeRepository APaymentAllocationTypeRepository, PlanMasterRepository APlanMasterRepository,
                ProductCategoryRepository AProductCategoryRepository, CompanyCategoryRepository ACompanyCategoryRepository,
                BranchRepository ABranchRepository, CardTypeRepository ACardTypeRepository, FinancialActivityRepository AFinancialActivityRepository,
                LegalEntityRepository ALegalEntityRepository, ContractTypeRepository AContractTypeRepository,
                NetworkRepository ANetworkRepository, CountryRepository ACountryRepository, BillingCycleRepository ABillingCycleRepository,
                DefaultAffinityRepository ADefaultAffinityRepository, InterestMethodRepository AInterestMethodRepository,
                ProductClassRepository AProductClassRepository, ProductMemberTypeRepository AProductMemberTypeRepository,
                ProductStatusRepository AProductStatusRepository, ProductSubCategoryRepository AProductSubCategoryRepository,
                ServiceRepository AServiceRepository, CityCategoryRepository ACityCategoryRepository, RegionRepository ARegionRepository,
                HubRepository AHubRepository, SpokeRepository ASpokeRepository, CommunaRepository ACommunaRepository,
                DecisionRepository ADecisionRepository, ContractTemplateRepository AContractTemplateRepository,
                BillingParameterRepository ABillingParameterRepository, SystemMaintenanceConfigurationRepository ASystemMaintenanceConfigurationRepository,
                SBIFInterestDetailsRepository ASBIFInterestDetailsRepository, CardCreditSegmentRepository ACardCreditSegmentRepository,
                CollectionChargeDetailsRepository ACollectionChargeDetailsRepository, DiscountOnMgmtCollectionDetailsRepository ADiscountOnMgmtCollectionDetailsRepository,
                DiscountPercentageRepository ADiscountPercentageRepository, DiscountPercentageDetailsRepository ADiscountPercentageDetailsRepository,
                CreditSubSegmentRepository ACreditSubSegmentRepository, InterestMasterDetailsRepository AInterestMasterDetailsRepository,
                PositionRepository APositionRepository, FeeTypesRepository AFeeTypesRepository, MerchantsRepository AMerchantsRepository, ChangeBusinessDateRepository AChangeBusinessDateRepository,
                DirectSalesAgentRepository ADirectSalesAgentRepository, UFRateSettingsRepository AUFRateSettingsRepository,
                SalesIdRepository ASalesIdRepository, MerchantDiscountRepository AMerchantDiscountRepository, TransactionCheckFrequencyRepository ATransactionCheckFrequencyRepository,
                AdjustmentFinancialAccountRepository AAdjustmentFinancialAccountRepository 
            )
        {
            if (ACurrencyRepository == null)
                throw new ArgumentNullException("ACurrencyRepository");
            if (AOrganizationRepository == null)
                throw new ArgumentNullException("AOrganizationRepository");
            if (AFranchiseRepository == null)
                throw new ArgumentNullException("AOrganizationRepository");
            if (ASourcingRepository == null)
                throw new ArgumentNullException("ASourcingRepository");
            if (ACompanyRepository == null)
                throw new ArgumentNullException("ACompanyRepository");
            if (ACityRepository == null)
                throw new ArgumentNullException("ACityRepository");
            if (ACreditSegmentRepository == null)
                throw new ArgumentNullException("ACreditSegmentRepository");
            if (ACreditSubSegmentRepository == null)
                throw new ArgumentNullException("ACreditSubSegmentRepository");
            if (ASystemParameterRepository == null)
                throw new ArgumentNullException("ASystemParameterRepository");
            if (ACustomerSegmentRepository == null)
                throw new ArgumentNullException("ACustomerSegmentRepository");
            if (ACustomerSubSegmentRepository == null)
                throw new ArgumentNullException("ACustomerSubSegmentRepository");
            if (ADesignationRepository == null)
                throw new ArgumentNullException("ADesignationRepository");
            if (ACardStatusRepository == null)
                throw new ArgumentNullException("ACardStatusRepository");
            if (ACardCreditSegmentRepository == null)
                throw new ArgumentNullException("ACardCreditSegmentRepository");
            if (AGeneralRepository == null)
                throw new ArgumentNullException("AGeneralRepository");
            if (AContractRepository == null)
                throw new ArgumentNullException("AContractRepository");
            if (AProductTypeRepository == null)
                throw new ArgumentNullException("AProductTypeRepository");
            if (ATransactionRepository == null)
                throw new ArgumentNullException("ATransactionRepository");
            if (ABoostMaintenanceRepository == null)
                throw new ArgumentNullException("ABoostMaintenanceRepository");
            if (AFeeRepository == null)
                throw new ArgumentNullException("AFeeRepository");
            if (AProductRepository == null)
                throw new ArgumentNullException("AProductRepository");
            if (ABlockCodeRepository == null)
                throw new ArgumentNullException("ABlockCodeRepository");
            if (ACustomerRepository == null)
                throw new ArgumentNullException("ACustomerRepository");
            if (ACalendarDateRepository == null)
                throw new ArgumentNullException("ACalendarDateRepository");
            if (ATransactionSubCodeRepository == null)
                throw new ArgumentNullException("ATransactionSubCodeRepository");
            if (AFeeTypeRepository == null)
                throw new ArgumentNullException("AFeeTypeRepository");
            if (ACustomerAddressRepository == null)
                throw new ArgumentNullException("ACustomerAddressRepository");
            if (ACustomerOccupationRepository == null)
                throw new ArgumentNullException("ACustomerOccupationRepository");
            if (ACustomerCurrentAccountDetailRepository == null)
                throw new ArgumentNullException("ACustomerCurrentAccountDetailRepository");
            if (ACustomerCreditCardDetailRepository == null)
                throw new ArgumentNullException("ACustomerCreditCardDetailRepository");
            if (ACustomerCommericalHouseConsumptionRepository == null)
                throw new ArgumentNullException("ACustomerCommericalHouseConsumptionRepository");
            if (ACustomerBillingDetailRepository == null)
                throw new ArgumentNullException("ACustomerBillingDetailRepository");
            if (ACustomerDependentRepository == null)
                throw new ArgumentNullException("ACustomerDependentRepository");
            if (ACustomerDecisionRepository == null)
                throw new ArgumentNullException("ACustomerDecisionRepository");
            if (ABillingCycleParameterRepository == null)
                throw new ArgumentNullException("ABillingCycleParameterRepository");
            if (ABillingParameterRepository == null)
                throw new ArgumentNullException("ABillingParameterRepository");
            if (AFinancialAccountRepository == null)
                throw new ArgumentNullException("AFinanacialAccountRepository");
            if (ASBIFInterestRepository == null)
                throw new ArgumentNullException("ASBIFInterestRepository");
            if (AInterestMasterRepository == null)
                throw new ArgumentNullException("AInterestMasterRepository");
            if (ACommissionRepository == null)
                throw new ArgumentNullException("ACommissionRepository");
            if (ACardMasterRepository == null)
                throw new ArgumentNullException("ACardMasterRepository");
            if (ACommissionChargeRepository == null)
                throw new ArgumentNullException("ACommissionChargeRepository");
            if (ACommissionChargeDetailsRepository == null)
                throw new ArgumentNullException("ACommissionChargeDetailsRepository");
            if (ABlockCodeForCommissionExclusionRepository == null)
                throw new ArgumentNullException("ABlockCodeForCommissionExclusionRepository");
            if (ADiscountOnMgmtCollectionRepository == null)
                throw new ArgumentNullException("ADiscountOnMgmtCollectionRepository");
            if (ACollectionChargeRepository == null)
                throw new ArgumentNullException("ACollectionChargeRepository");
            if (AInsuranceChargeRepository == null)
                throw new ArgumentNullException("AInsuranceChargeRepository");
            if (AStatementGroupingRepository == null)
                throw new ArgumentNullException("AStatementGroupingRepository");
            if (APaymentAllocationTypeRepository == null)
                throw new ArgumentNullException("APaymentAllocationTypeRepository");
            if (APlanMasterRepository == null)
                throw new ArgumentNullException("APlanMasterRepository");
            if (AProductCategoryRepository == null)
                throw new ArgumentNullException("AProductCategoryRepository");
            if (ACompanyCategoryRepository == null)
                throw new ArgumentNullException("ACompanyCategoryRepository");
            if (ABranchRepository == null)
                throw new ArgumentNullException("ABranchRepository");
            if (ACardTypeRepository == null)
                throw new ArgumentNullException("ACardTypeRepository");
            if (AFinancialActivityRepository == null)
                throw new ArgumentNullException("AFinancialActivityRepository");
            if (ALegalEntityRepository == null)
                throw new ArgumentNullException("ALegalEntityRepository");
            if (ASystemMaintenanceConfigurationRepository == null)
                throw new ArgumentNullException("ASystemMaintenanceConfigurationRepository");
            if (AContractTypeRepository == null)
                throw new ArgumentNullException("AContractTypeRepository");
            if (ANetworkRepository == null)
                throw new ArgumentNullException("ANetworkRepository");
            if (ACountryRepository == null)
                throw new ArgumentNullException("ACountryRepository");
            if (ABillingCycleRepository == null)
                throw new ArgumentNullException("ABillingCycleRepository");
            if (ADefaultAffinityRepository == null)
                throw new ArgumentNullException("ADefaultAffinityRepository");
            if (AInterestMethodRepository == null)
                throw new ArgumentNullException("AInterestMethodRepository");
            if (AProductClassRepository == null)
                throw new ArgumentNullException("AProductClassRepository");
            if (AProductMemberTypeRepository == null)
                throw new ArgumentNullException("AProductMemberTypeRepository");
            if (AProductStatusRepository == null)
                throw new ArgumentNullException("AProductStatusRepository");
            if (AProductSubCategoryRepository == null)
                throw new ArgumentNullException("AProductSubCategoryRepository");
            if (AServiceRepository == null)
                throw new ArgumentNullException("AServiceRepository");
            if (ACityCategoryRepository == null)
                throw new ArgumentNullException("ACityCategoryRepository");
            if (ARegionRepository == null)
                throw new ArgumentNullException("ARegionRepository");
            if (AHubRepository == null)
                throw new ArgumentNullException("AHubRepository");
            if (ASpokeRepository == null)
                throw new ArgumentNullException("ASpokeRepository");
            if (ACommunaRepository == null)
                throw new ArgumentNullException("ACommunaRepository");
            if (ADecisionRepository == null)
                throw new ArgumentNullException("ADecisionRepository");
            if (AContractTemplateRepository == null)
                throw new ArgumentNullException("AContractTemplateRepository");
            if (ASBIFInterestDetailsRepository == null)
                throw new ArgumentNullException("ASBIFInterestDetailsRepository");
            if (ACollectionChargeDetailsRepository == null)
                throw new ArgumentNullException("ACollectionChargeDetailsRepository");
            if (ADiscountOnMgmtCollectionDetailsRepository == null)
                throw new ArgumentNullException("ADiscountOnMgmtCollectionDetailsRepository");
            if (ADiscountPercentageRepository == null)
                throw new ArgumentNullException("ADiscountPercentageRepository");
            if (ADiscountPercentageDetailsRepository == null)
                throw new ArgumentNullException("ADiscountPercentageDetailsRepository");
            if (AInterestMasterDetailsRepository == null)
                throw new ArgumentNullException("AInterestMasterDetailsRepository");
            if (APositionRepository == null)
                throw new ArgumentNullException("APositionRepository");
            if (AFeeTypesRepository == null)
                throw new ArgumentNullException("AFeeTypesRepository");
            if (AMerchantsRepository == null)
                throw new ArgumentNullException("AMerchantsRepository");
            if (AChangeBusinessDateRepository == null)
                throw new ArgumentNullException("AChangeBusinessDateRepository");
            if (ADirectSalesAgentRepository == null)
                throw new ArgumentNullException("ADirectSalesAgentRepository");
            if (AUFRateSettingsRepository == null)
                throw new ArgumentNullException("AUFRateSettingsRepository");
            if (ASalesIdRepository == null)
                throw new ArgumentNullException("ASalesIdRepository");
            if (AMerchantDiscountRepository == null)
                throw new ArgumentNullException("AMerchantDiscountRepository");
            if (ATransactionCheckFrequencyRepository == null)
                throw new ArgumentNullException("ATransactionCheckFrequencyRepository");
            if (AAdjustmentFinancialAccountRepository == null)
                throw new ArgumentNullException("AAdjustmentFinancialAccountRepository");

            objCurrencyRepository = ACurrencyRepository;
            objOrganizationRepository = AOrganizationRepository;
            objFranchiseRepository = AFranchiseRepository;
            objSourcingRepository = ASourcingRepository;
            objCompanyRepository = ACompanyRepository;
            objCityRepository = ACityRepository;
            objCreditSegmentRepository = ACreditSegmentRepository;
            objCreditSubSegmentRepository = ACreditSubSegmentRepository;
            objSystemParameterRepository = ASystemParameterRepository;
            objCustomerSegmentRepository = ACustomerSegmentRepository;
            objCustomerSubSegmentRepository = ACustomerSubSegmentRepository;
            objDesignationRepository = ADesignationRepository;
            objCardStatusRepository = ACardStatusRepository;
            objGeneralRepository = AGeneralRepository;
            objContractRepository = AContractRepository;
            objProductTypeRepository = AProductTypeRepository;
            objTransactionRepository = ATransactionRepository;
            objBoostMaintenanceRepository = ABoostMaintenanceRepository;
            objFeeRepository = AFeeRepository;
            objFeeTypeRepository = AFeeTypeRepository;
            objProductRepository = AProductRepository;
            objBlockCodeRepository = ABlockCodeRepository;
            objCustomerRepository = ACustomerRepository;
            objCalendarDateRepository = ACalendarDateRepository;
            objTransactionSubCodeRepository = ATransactionSubCodeRepository;
            objPlanMasterRepository = APlanMasterRepository;
            objBillingParameterRepository = ABillingParameterRepository;
            objSystemMaintenanceConfigurationRepository = ASystemMaintenanceConfigurationRepository;
            objCardCreditSegmentRepository = ACardCreditSegmentRepository;

            objCustomerAddressRepository = ACustomerAddressRepository;
            objCustomerOccupationRepository = ACustomerOccupationRepository;
            objCustomerCurrentAccountDetailRepository = ACustomerCurrentAccountDetailRepository;
            objCustomerCreditCardDetailRepository = ACustomerCreditCardDetailRepository;
            objCustomerCommericalHouseConsumptionRepository = ACustomerCommericalHouseConsumptionRepository;
            objCustomerBillingDetailRepository = ACustomerBillingDetailRepository;
            objCustomerDependentRepository = ACustomerDependentRepository;
            objCustomerDecisionRepository = ACustomerDecisionRepository;
            objFinancialAccountRepository = AFinancialAccountRepository;
            objBillingCycleParameterRepository = ABillingCycleParameterRepository;
            objSBIFInterestRepository = ASBIFInterestRepository;
            objInterestMasterRepository = AInterestMasterRepository;
            objCommissionRepository = ACommissionRepository;
            objCardMasterRepository = ACardMasterRepository;
            objCommissionChargeRepository = ACommissionChargeRepository;
            objCommissionChargeDetailsRepository = ACommissionChargeDetailsRepository;
            objBlockCodeForCommissionExclusionRepository = ABlockCodeForCommissionExclusionRepository;
            objDiscountOnMgmtCollectionRepository = ADiscountOnMgmtCollectionRepository;
            objCollectionChargeRepository = ACollectionChargeRepository;
            objInsuranceChargeRepository = AInsuranceChargeRepository;
            objStatementGroupingRepository = AStatementGroupingRepository;
            objPaymentAllocationTypeRepository = APaymentAllocationTypeRepository;

            objProductCategoryRepository = AProductCategoryRepository;
            objCompanyCategoryRepository = ACompanyCategoryRepository;
            objLegalEntityRepository = ALegalEntityRepository;
            objCountryRepository = ACountryRepository;

            objBranchRepository = ABranchRepository;
            objCardTypeRepository = ACardTypeRepository;
            objFinancialActivityRepository = AFinancialActivityRepository;
            objContractTypeRepository = AContractTypeRepository;
            objNetworkRepository = ANetworkRepository;
            objBillingCycleRepository = ABillingCycleRepository;
            objDefaultAffinityRepository = ADefaultAffinityRepository;
            objInterestMethodRepository = AInterestMethodRepository;
            objProductClassRepository = AProductClassRepository;
            objProductMemberTypeRepository = AProductMemberTypeRepository;
            objProductStatusRepository = AProductStatusRepository;
            objProductSubCategoryRepository = AProductSubCategoryRepository;
            objServiceRepository = AServiceRepository;
            objCityCategoryRepository = ACityCategoryRepository;
            objRegionRepository = ARegionRepository;
            objHubRepository = AHubRepository;
            objSpokeRepository = ASpokeRepository;
            objCommunaRepository = ACommunaRepository;
            objDecisionRepository = ADecisionRepository;
            objContractTemplateRepository = AContractTemplateRepository;
            objSBIFInterestDetailsRepository = ASBIFInterestDetailsRepository;
            objCollectionChargeDetailsRepository = ACollectionChargeDetailsRepository;
            objDiscountOnMgmtCollectionDetailsRepository = ADiscountOnMgmtCollectionDetailsRepository;
            objDiscountPercentageRepository = ADiscountPercentageRepository;
            objDiscountPercentageDetailsRepository = ADiscountPercentageDetailsRepository;
            objInterestMasterDetailsRepository = AInterestMasterDetailsRepository;
            objPositionRepository = APositionRepository;
            objFeeTypesRepository = AFeeTypesRepository;
            objMerchantsRepository = AMerchantsRepository;
            objChangeBusinessDateRepository = AChangeBusinessDateRepository;
            objDirectSalesAgentRepository = ADirectSalesAgentRepository;
            objUFRateSettingsRepository = AUFRateSettingsRepository;
            objSalesIdRepository = ASalesIdRepository;
            objMerchantDiscountRepository = AMerchantDiscountRepository;
            objTransactionCheckFrequencyRepository = ATransactionCheckFrequencyRepository;
            objAdjustmentFinancialAccountRepository = AAdjustmentFinancialAccountRepository;
        }
        #endregion

        #region interface implementation

        #region Currency

        /// <summary>
        /// To list all the currencies when the page is accessed via get method
        /// </summary>
        /// <param name="APageIndex">Current Page Index</param>
        /// <param name="APageCount">Total Page Count</param>
        /// <returns>List of all currencies saved in the system</returns>
        public List<CurrencyDTO> FindCurrency(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindCurrency");


            //int iRecordCount = objCurrencyRepository.GetRecordCount();

            var currencies = objCurrencyRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);

            //objCurrencyRepository.UnitOfWork.ReloadAllDataInContext<Currency>();
            if (currencies != null && currencies.Any())
            {
                List<CurrencyDTO> lstCurrencyDTO = new List<CurrencyDTO>();
                foreach (var currency in currencies)
                {
                    lstCurrencyDTO.Add(Conversion.Mapping.CurrencyToCurrencyDTO(currency));
                }
                return lstCurrencyDTO;
            }
            else
                return new List<CurrencyDTO>();
        }

        /// <summary>
        /// To delete a currency based on the currency code
        /// </summary>
        /// <param name="ACurrencyCode">Currency Code to be deleted</param>
        public void DeleteCurrency(string ACurrencyCode)
        {
            var currency = objCurrencyRepository.Get(ACurrencyCode);

            if (currency != null)
            {
                objCurrencyRepository.Remove(currency);
                objCurrencyRepository.UnitOfWork.Commit();
            }
        }

        /// <summary>
        /// To get details of a currency
        /// </summary>
        /// <param name="ACode">Code of the currency to be searched</param>
        /// <returns>Currency details such as code, name  and country</returns>
        public CurrencyDTO FindByCurrencyCode(string ACode, string AOrganizationCode, bool AIsFromApprovalQueue = false)
        {
            var currency = objCurrencyRepository.Get(AOrganizationCode, ACode);

            if (currency != null)
            {
                if (AIsFromApprovalQueue)
                    objCurrencyRepository.Modify(currency);
                objCurrencyRepository.UnitOfWork.RefreshDataInModel(ref currency);
                return Conversion.Mapping.CurrencyToCurrencyDTO(currency);
            }
            else
            {
                return new CurrencyDTO();
            }

        }

        /// <summary>
        /// To save the newly added currency details
        /// </summary>
        /// <param name="ACurrencyDTO">Currency Details</param>

        public void SaveCurrencyDetails(CurrencyDTO ACurrencyDTO)
        {
            if (ACurrencyDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddCurrencyWithNullInformation");

            var newCurrency = CurrencyFactory.CreateCurrency(ACurrencyDTO.CurrencyCode, ACurrencyDTO.CurrencyName,
                ACurrencyDTO.Precision, ACurrencyDTO.UFCLPRate, ACurrencyDTO.CountryCode, ACurrencyDTO.IsBlackListed, ACurrencyDTO.IsActive,
                ACurrencyDTO.CreatedBy, ACurrencyDTO.LastUpdatedBy, ACurrencyDTO.ApprovalStatus, ACurrencyDTO.OrganizationCode);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objCurrencyRepository.Add(newCurrency);
                        objCurrencyRepository.UnitOfWork.Commit();
                        if (ACurrencyDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_Currency", " CurrencyCode = '" + ACurrencyDTO.CurrencyCode + "' and OrganizationCode = '" + ACurrencyDTO.OrganizationCode + "'",
                                "CUR", ACurrencyDTO.LastUpdatedBy,// "/Master/CurrencyAuthorize/" + ACurrencyDTO.CurrencyCode + "/A?", "MT");
                               "/Master/CurrencyAuthorize?ACurrencyCode=" + ACurrencyDTO.CurrencyCode + "&AOrganizationCode=" + ACurrencyDTO.OrganizationCode + "&", "MT");
                            objCurrencyRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);

                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// To update the details of an existing currency
        /// </summary>
        /// <param name="ACurrencyCode">Code of the currency selected for modification</param>
        /// <param name="ACurrencyDTO">Currency Details</param>
        public void UpdateCurrencyDetails(string ACurrencyCode, string AOrganizationCode, CurrencyDTO ACurrencyDTO)
        {
            if (ACurrencyDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");
            }
            Currency currentCurrency = objCurrencyRepository.Get(AOrganizationCode, ACurrencyCode);
            Currency updatedCurrency = new Currency()
            {
                CurrencyCode = ACurrencyCode,
                OrganizationCode = AOrganizationCode,
                CurrencyName = ACurrencyDTO.CurrencyName,
                Precision = ACurrencyDTO.Precision,
                UFCLPRate = ACurrencyDTO.UFCLPRate,
                CountryCode = ACurrencyDTO.CountryCode,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = ACurrencyDTO.LastUpdatedBy,
                ApprovalStatus = ACurrencyDTO.ApprovalStatus,
                CreatedOn = currentCurrency.CreatedOn,
                CreatedBy = currentCurrency.CreatedBy
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objCurrencyRepository.Merge(currentCurrency, updatedCurrency);
                    objCurrencyRepository.UnitOfWork.Commit();
                    if (ACurrencyDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_Currency", " CurrencyCode = '" + ACurrencyDTO.CurrencyCode + "' and OrganizationCode = '" + AOrganizationCode + "'",
                            "CUR", ACurrencyDTO.LastUpdatedBy,// "/Master/CurrencyAuthorize/" + ACurrencyDTO.CurrencyCode + "/A?", "MT");
                           "/Master/CurrencyAuthorize?ACurrencyCode=" + ACurrencyDTO.CurrencyCode + "&AOrganizationCode=" + AOrganizationCode + "&", "MT");
                        objCurrencyRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);

                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// To retrieve currency details based on the search filters
        /// </summary>
        /// <param name="APageNo">Current Page No</param>
        /// <param name="ACurrencyDTO">Currency search filter</param>
        /// <returns>List of currencies that matches the selection criteria</returns>
        public List<CurrencyDTO> FindAll(int APageNo, CurrencyDTO ACurrencyDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "CurrencyCode", FilterValue = ACurrencyDTO.CurrencyCode, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "CurrencyName", FilterValue = ACurrencyDTO.CurrencyName, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "OrganizationCode", FilterValue = Utils .OrganizationCode , Operator = "Equals"}
            };

            Expression<Func<Currency, bool>> expression = Utils.GetExpression<Currency>(lstExp);
            //int iRecordCount = objCurrencyRepository.GetRecordCount(x => x.CurrencyCode.Contains(ACurrencyDTO.CurrencyCode));
            int iRecordCount = objCurrencyRepository.GetRecordCount(expression);

            //var vCurrencyDTO = objCurrencyRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vCurrencyDTO = objCurrencyRepository.GetFiltered(0, iRecordCount, expression);

            if (vCurrencyDTO != null && vCurrencyDTO.Any())
            {
                List<CurrencyDTO> lstCurrencyDTO = new List<CurrencyDTO>();
                foreach (var currency in vCurrencyDTO)
                {
                    lstCurrencyDTO.Add(Conversion.Mapping.CurrencyToCurrencyDTO(currency));
                }
                return lstCurrencyDTO;
            }
            else
                return new List<CurrencyDTO>();

        }

        #endregion

        #region GetDropDownList
        /// <summary>
        /// To populated the drop down field for country
        /// </summary>
        /// <typeparam name="TEntity">Country</typeparam>
        /// <param name="AText">Text to be displayed in the items</param>
        /// <param name="AValue">Value of the items in the list</param>
        /// <param name="ASelected">Selected item in the drop down</param>
        /// <returns>Items that needs to be populated in the drop down</returns>
        private List<SelectListItem> GetDropDownList<TEntity>
           (string AText, string AValue, string ASelected) where TEntity : class
        {

            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = Resources.SelectOne, Value = string.Empty });
            IQueryable<TEntity> result = (IQueryable<TEntity>)objCurrencyRepository.GetAll().AsQueryable();
            var lisData = (from items in result
                           select items).AsEnumerable().Select(m => new SelectListItem
                           {
                               Text = (string)m.GetType().GetProperty(AText).GetValue(m),
                               Value = (string)m.GetType().GetProperty(AValue).GetValue(m),
                               Selected = (ASelected != "") ? ((string)
                                 m.GetType().GetProperty(AValue).GetValue(m) ==
                                 ASelected ? true : false) : false,
                           }).ToList();
            list.AddRange(lisData);
            return list;
        }
        #endregion

        #region Organization


        public List<OrganizationDTO> FindOrganization(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindOrganization");

            int iRecordCount = objOrganizationRepository.GetRecordCount();
            var organizations = objOrganizationRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);
            if (organizations != null && organizations.Any())
            {
                List<OrganizationDTO> lstOrganizationDTO = new List<OrganizationDTO>();
                foreach (var organization in organizations)
                {
                    lstOrganizationDTO.Add(Conversion.Mapping.OrganizationToOrganizationDTO(organization));
                }
                return lstOrganizationDTO;
            }
            else
                return new List<OrganizationDTO>();
        }

        public void DeleteOrganization(string AOrganizationCode)
        {
            var organization = objOrganizationRepository.Get(AOrganizationCode);

            if (organization != null)
            {
                objOrganizationRepository.Remove(organization);
                objOrganizationRepository.UnitOfWork.Commit();
            }
        }

        public OrganizationDTO FindByOrganizationCode(string ACode)
        {
            var organization = objOrganizationRepository.Get(ACode);
            if (organization != null)
            {
                objOrganizationRepository.UnitOfWork.RefreshDataInModel(ref organization);
                return Conversion.Mapping.OrganizationToOrganizationDTO(organization);
            }
            else
            {
                return new OrganizationDTO();
            }

        }

        public void SaveOrganizationDetails(OrganizationDTO AOrganizationDTO)
        {
            if (AOrganizationDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddCurrencyWithNullInformation");

            var newOrganization = OrganizationFactory.CreateOrganization(AOrganizationDTO.OrganizationCode, AOrganizationDTO.OrganizationDescription,
                AOrganizationDTO.EffectiveFromDate, AOrganizationDTO.EffectiveToDate, AOrganizationDTO.OrganizationShortName, AOrganizationDTO.OrganizationLogo,
                AOrganizationDTO.CreatedBy, AOrganizationDTO.LastUpdatedBy, AOrganizationDTO.ApprovalStatus);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objOrganizationRepository.Add(newOrganization);
                        objOrganizationRepository.UnitOfWork.Commit();
                        if (AOrganizationDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_Organization", " OrganizationCode = '" + AOrganizationDTO.OrganizationCode + "'",
                                "ORGSTN", AOrganizationDTO.CreatedBy, "/Master/OrganizationAuthorize/" + AOrganizationDTO.OrganizationCode + "/A?", "MT");
                            objOrganizationRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateOrganizationDetails(string AOrganizationCode, OrganizationDTO AOrganizationDTO)
        {
            if (AOrganizationDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");

            }
            Organization currentOrganization = objOrganizationRepository.Get(AOrganizationCode);
            Organization updatedOrganization = new Organization()
            {
                OrganizationCode = AOrganizationCode,
                OrganizationDescription = AOrganizationDTO.OrganizationDescription,
                EffectiveFromDate = AOrganizationDTO.EffectiveFromDate,
                EffectiveToDate = AOrganizationDTO.EffectiveToDate,
                OrganizationShortName = AOrganizationDTO.OrganizationShortName,
                OrganizationLogo = AOrganizationDTO.OrganizationLogo == null ? "" : AOrganizationDTO.OrganizationLogo,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = AOrganizationDTO.LastUpdatedBy,
                CreatedOn = currentOrganization.CreatedOn,
                CreatedBy = currentOrganization.CreatedBy,
                ApprovalStatus = AOrganizationDTO.ApprovalStatus
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objOrganizationRepository.Merge(currentOrganization, updatedOrganization);
                    objOrganizationRepository.UnitOfWork.Commit();
                    if (AOrganizationDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_Organization", " OrganizationCode = '" + AOrganizationDTO.OrganizationCode + "'",
                            "ORGSTN", AOrganizationDTO.LastUpdatedBy, "/Master/OrganizationAuthorize/" + AOrganizationDTO.OrganizationCode + "/A?", "MT");
                        objOrganizationRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<OrganizationDTO> FindAll(int APageNo, OrganizationDTO AOrganizationDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "OrganizationCode", FilterValue = AOrganizationDTO.OrganizationCode, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "OrganizationDescription", FilterValue = AOrganizationDTO.OrganizationDescription, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "EffectiveFromDate", FilterValue = AOrganizationDTO.EffectiveFromDate, Operator = "GreaterThanOrEqualTo"},
                  new Utils.ExpressionList(){FilterColumn = "EffectiveToDate", FilterValue = AOrganizationDTO.EffectiveToDate, Operator = "LessThanOrEqualTo"}
            };

            Expression<Func<Organization, bool>> expression = Utils.GetExpression<Organization>(lstExp);
            int iRecordCount = objOrganizationRepository.GetRecordCount(expression);

            //var vOrganizationDTO = objOrganizationRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vOrganizationDTO = objOrganizationRepository.GetFiltered(0, iRecordCount, expression);

            if (vOrganizationDTO != null && vOrganizationDTO.Any())
            {
                List<OrganizationDTO> lstOrganizationDTO = new List<OrganizationDTO>();
                foreach (var organization in vOrganizationDTO)
                {
                    lstOrganizationDTO.Add(Conversion.Mapping.OrganizationToOrganizationDTO(organization));
                }
                return lstOrganizationDTO;
            }
            else
                return new List<OrganizationDTO>();
        }


        #endregion

        #region Franchise


        public List<FranchiseDTO> FindFranchise(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindFranchise");

            int iRecordCount = objFranchiseRepository.GetRecordCount();
            var franchises = objFranchiseRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);
            if (franchises != null && franchises.Any())
            {
                List<FranchiseDTO> lstFranchiseDTO = new List<FranchiseDTO>();
                foreach (var franchise in franchises)
                {
                    lstFranchiseDTO.Add(Conversion.Mapping.FranchiseToFranchiseDTO(franchise));
                }
                return lstFranchiseDTO;
            }
            else
                return new List<FranchiseDTO>();
        }

        public void DeleteFranchise(string AFranchiseCode)
        {
            var franchise = objFranchiseRepository.Get(AFranchiseCode);

            if (franchise != null)
            {
                objFranchiseRepository.Remove(franchise);
                objFranchiseRepository.UnitOfWork.Commit();
            }
        }

        public FranchiseDTO FindByFranchiseCode(string ACode)
        {
            var franchise = objFranchiseRepository.Get(ACode, Utils.OrganizationCode);

            if (franchise != null)
            {
                if (franchise.Organization == null)
                {
                    franchise.Organization = objOrganizationRepository.Get(franchise.OrganizationCode);
                }
                objFranchiseRepository.UnitOfWork.RefreshDataInModel(ref franchise);
                return Conversion.Mapping.FranchiseToFranchiseDTO(franchise);
            }
            else
            {
                return new FranchiseDTO();
            }

        }

        public void SaveFranchiseDetails(FranchiseDTO AFranchiseDTO)
        {
            if (AFranchiseDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddCurrencyWithNullInformation");

            var newFranchise = FranchiseFactory.CreateFranchise(AFranchiseDTO.FranchiseCode, AFranchiseDTO.FranchiseName,
                AFranchiseDTO.OrganizationCode, AFranchiseDTO.Allocation, AFranchiseDTO.EffectiveFromDate, AFranchiseDTO.EffectiveToDate,
                AFranchiseDTO.CreatedBy, AFranchiseDTO.LastUpdatedBy, AFranchiseDTO.ApprovalStatus);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objFranchiseRepository.Add(newFranchise);
                        objFranchiseRepository.UnitOfWork.Commit();
                        if (AFranchiseDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_Franchise", " FranchiseCode = '" + AFranchiseDTO.FranchiseCode + "' and OrganizationCode = '" + AFranchiseDTO.OrganizationCode + "'",
                                "FRNCHSE", AFranchiseDTO.LastUpdatedBy, "/Master/FranchiseAuthorize/" + AFranchiseDTO.FranchiseCode + "/A?", "MT");
                            // "/Master/FranchiseAuthorize?AFranchiseCode=" + AFranchiseDTO.FranchiseCode + "&AOrganizationCode=" + AFranchiseDTO.OrganizationCode + "&", "MT");
                            objFranchiseRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateFranchiseDetails(string AFranchiseCode, FranchiseDTO AFranchiseDTO)
        {
            if (AFranchiseDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");

            }
            Franchise currentFranchise = objFranchiseRepository.Get(AFranchiseCode, Utils.OrganizationCode);
            Franchise updatedFranchise = new Franchise()
            {
                FranchiseCode = AFranchiseCode,
                FranchiseName = AFranchiseDTO.FranchiseName,
                OrganizationCode = AFranchiseDTO.OrganizationCode,
                Allocation = AFranchiseDTO.Allocation,
                EffectiveFromDate = AFranchiseDTO.EffectiveFromDate,
                EffectiveToDate = AFranchiseDTO.EffectiveToDate,
                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = AFranchiseDTO.LastUpdatedBy,
                CreatedOn = currentFranchise.CreatedOn,
                CreatedBy = currentFranchise.CreatedBy,
                ApprovalStatus = AFranchiseDTO.ApprovalStatus
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objFranchiseRepository.Merge(currentFranchise, updatedFranchise);
                    objFranchiseRepository.UnitOfWork.Commit();
                    if (AFranchiseDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };


                        paramCollection = GetApprovalQueueProcedureParameters("MT_Franchise", " FranchiseCode = '" + AFranchiseDTO.FranchiseCode + "' and OrganizationCode = '" + AFranchiseDTO.OrganizationCode + "'",
                            "FRNCHSE", AFranchiseDTO.LastUpdatedBy, "/Master/FranchiseAuthorize/" + AFranchiseDTO.FranchiseCode + "/A?", "MT");
                        // "/Master/FranchiseAuthorize?AFranchiseCode=" + AFranchiseDTO.FranchiseCode + "&AOrganizationCode=" + AFranchiseDTO.OrganizationCode + "&", "MT");                      
                        objFranchiseRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<FranchiseDTO> FindAll(int APageNo, FranchiseDTO AFranchiseDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "FranchiseCode", FilterValue = AFranchiseDTO.FranchiseCode, Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "FranchiseName", FilterValue = AFranchiseDTO.FranchiseName, Operator = "Contains"},
                new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode, Operator = "Equals"},
                new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}

            };

            Expression<Func<Franchise, bool>> expression = Utils.GetExpression<Franchise>(lstExp);

            int iRecordCount = objFranchiseRepository.GetRecordCount(expression);

            //var vFranchiseDTO = objFranchiseRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vFranchiseDTO = objFranchiseRepository.GetFiltered(0, iRecordCount, expression);

            if (vFranchiseDTO != null && vFranchiseDTO.Any())
            {
                List<FranchiseDTO> lstFranchiseDTO = new List<FranchiseDTO>();
                foreach (var franchise in vFranchiseDTO)
                {
                    lstFranchiseDTO.Add(Conversion.Mapping.FranchiseToFranchiseDTO(franchise));
                }
                return lstFranchiseDTO;
            }
            else
                return new List<FranchiseDTO>();

        }

        #endregion

        #region Sourcing

        #region SaveSourcingDetails
        public void SaveSourcingDetails(SourcingDTO ASourcingDTO)
        {
            if (ASourcingDTO == null)
            {
                throw new ArgumentException("");
            }

            var newSourcing = SourcingFactory.CreateSourcing(ASourcingDTO.SourcingChannelCode, ASourcingDTO.SourcingChannelName,
                ASourcingDTO.FloorLimit, ASourcingDTO.CeilingLimit, ASourcingDTO.LetterGenerationDelayDays,
                ASourcingDTO.CreatedBy, ASourcingDTO.LastUpdatedBy, ASourcingDTO.ApprovalStatus, ASourcingDTO.OrganizationCode, null);


            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objSourcingRepository.Add(newSourcing);
                        objSourcingRepository.UnitOfWork.Commit();
                        if (ASourcingDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_Sourcing", " SourcingChannelCode = '" + ASourcingDTO.SourcingChannelCode + "' and OrganizationCode = '" + ASourcingDTO.OrganizationCode + "'",
                                    "SRC", ASourcingDTO.LastUpdatedBy, //"/Master/SourcingAuthorize/" + ASourcingDTO.SourcingChannelCode + "/A?", "MT");

                                  "/Master/SourcingAuthorize?ASourcingChannelCode=" + ASourcingDTO.SourcingChannelCode + "&AOrganizationCode=" + ASourcingDTO.OrganizationCode + "&", "MT");

                            objSourcingRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
        #region UpdateSourcingDetails
        public void UpdateSourcingDetails(string ASourcingChannelCode, string AOrganizationCode, SourcingDTO ASourcingDTO)
        {
            if (ASourcingDTO == null)
            {
                throw new ArgumentException("");
            }

            Sourcing currentSourcing = objSourcingRepository.Get(ASourcingChannelCode, AOrganizationCode);
            Sourcing updatedSourcing = new Sourcing()
            {
                SourcingChannelCode = ASourcingChannelCode,
                OrganizationCode = AOrganizationCode,
                SourcingChannelName = ASourcingDTO.SourcingChannelName,
                FloorLimit = ASourcingDTO.FloorLimit,
                CeilingLimit = ASourcingDTO.CeilingLimit,
                LetterGenerationDelayDays = ASourcingDTO.LetterGenerationDelayDays,
                ApprovalStatus = ASourcingDTO.ApprovalStatus,
                CreatedBy = currentSourcing.CreatedBy,
                CreatedOn = currentSourcing.CreatedOn,
                LastUpdatedBy = ASourcingDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objSourcingRepository.Merge(currentSourcing, updatedSourcing);
                    objSourcingRepository.UnitOfWork.Commit();
                    if (ASourcingDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_Sourcing", " SourcingChannelCode = '" + ASourcingDTO.SourcingChannelCode + "' and OrganizationCode = '" + AOrganizationCode + "'",
                                "SRC", ASourcingDTO.LastUpdatedBy, //"/Master/SourcingAuthorize/" + ASourcingDTO.SourcingChannelCode + "/A?", "MT");

                              "/Master/SourcingAuthorize?ASourcingChannelCode=" + ASourcingDTO.SourcingChannelCode + "&AOrganizationCode=" + AOrganizationCode + "&", "MT");

                        objSourcingRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #region DeleteSourcing
        public void DeleteSourcing(string ASourcingChannelCode)
        {
            var sourcing = objSourcingRepository.Get(ASourcingChannelCode);
            if (sourcing != null)
            {
                objSourcingRepository.Remove(sourcing);
                objSourcingRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindSourcing
        public List<SourcingDTO> FindSourcing(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objSourcingRepository.GetRecordCount();
            var sourcing = objSourcingRepository.GetAll();
            if (sourcing != null && sourcing.Any())
            {
                List<SourcingDTO> lstSourcingDTO = new List<SourcingDTO>();
                foreach (var item in sourcing)
                {
                    lstSourcingDTO.Add(Conversion.Mapping.SourcingToSourcingDTO(item));
                }
                return lstSourcingDTO;
            }
            else
                return new List<SourcingDTO>();
        }
        #endregion
        #region FindAll
        public List<SourcingDTO> FindAll(int APageNo, SourcingDTO ASourcingDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "SourcingChannelCode",FilterValue = ASourcingDTO.SourcingChannelCode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "SourcingChannelName",FilterValue = ASourcingDTO.SourcingChannelName,Operator = "Contains"}                      ,
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode ,Operator = "Equals"}              
                    };

            Expression<Func<Sourcing, bool>> expression = Utils.GetExpression<Sourcing>(lstExpression);
            int iRecordCount = objSourcingRepository.GetRecordCount(expression);

            //var vSourcingDTO = objSourcingRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vSourcingDTO = objSourcingRepository.GetFiltered(0, iRecordCount, expression);

            if (vSourcingDTO != null && vSourcingDTO.Any())
            {
                List<SourcingDTO> lstSourcingDTO = new List<SourcingDTO>();
                foreach (var item in vSourcingDTO)
                {
                    lstSourcingDTO.Add(Conversion.Mapping.SourcingToSourcingDTO(item));
                }
                return lstSourcingDTO;
            }
            else
                return new List<SourcingDTO>();

        }
        #endregion
        #region FindBySourcingChannelCode
        public SourcingDTO FindBySourcingChannelCode(string ASourcingChannelCode, string AOrganizationCode)
        {
            Sourcing sourcing = objSourcingRepository.Get(ASourcingChannelCode, AOrganizationCode);
            if (sourcing != null)
            {
                objSourcingRepository.UnitOfWork.RefreshDataInModel(ref sourcing);
                return Conversion.Mapping.SourcingToSourcingDTO(sourcing);
            }
            else
            {
                return new SourcingDTO();
            }
        }
        #endregion
        #endregion

        #region Company
        #region SaveCompanyDetails
        public void SaveCompanyDetails(CompanyDTO ACompanyDTO)
        {
            if (ACompanyDTO == null)
            {
                throw new ArgumentException("");
            }

            var newCompany = CompanyFactory.CreateCompany(ACompanyDTO.CompanyCode, ACompanyDTO.CompanyName,
                ACompanyDTO.CompanyCategoryCode, ACompanyDTO.IsBlackListed, ACompanyDTO.EffectiveFromDate,
                ACompanyDTO.EffectiveToDate, ACompanyDTO.PricingAmount, ACompanyDTO.TML, ACompanyDTO.ProductCategoryCode,
                ACompanyDTO.IsAffinity, ACompanyDTO.CreatedBy, ACompanyDTO.LastUpdatedBy, ACompanyDTO.ApprovalStatus, ACompanyDTO.OrganizationCode, null);

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                        new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objCompanyRepository.Add(newCompany);
                    objCompanyRepository.UnitOfWork.Commit();

                    if (ACompanyDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_Company", " CompanyCode = '" + ACompanyDTO.CompanyCode + "' and OrganizationCode = '" + ACompanyDTO.OrganizationCode + "'",
                            "COMP", ACompanyDTO.LastUpdatedBy, //"/Master/CompanyAuthorize/" + ACompanyDTO.CompanyCode + "/A?", "MT");


            "/Master/CompanyAuthorize?ACompanyCode=" + ACompanyDTO.CompanyCode + "&AOrganizationCode=" + ACompanyDTO.OrganizationCode + "&", "MT");


                        objCompanyRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #region UpdateCompanyDetails
        public void UpdateCompanyDetails(string ACompanyCode, string AOrganizationCode, CompanyDTO ACompanyDTO)
        {
            if (ACompanyDTO == null)
            {
                throw new ArgumentException("");
            }

            Company currentCompany = objCompanyRepository.Get(ACompanyCode, AOrganizationCode);
            Company updatedCompany = new Company()
            {
                CompanyCode = ACompanyCode,
                OrganizationCode = AOrganizationCode,
                CompanyName = ACompanyDTO.CompanyName,
                CompanyCategoryCode = ACompanyDTO.CompanyCategoryCode,
                IsBlackListed = ACompanyDTO.IsBlackListed,
                EffectiveFromDate = ACompanyDTO.EffectiveFromDate,
                EffectiveToDate = ACompanyDTO.EffectiveToDate,
                PricingAmount = ACompanyDTO.PricingAmount,
                TML = ACompanyDTO.TML,
                ProductCategoryCode = ACompanyDTO.ProductCategoryCode,
                IsAffinity = ACompanyDTO.IsAffinity,
                ApprovalStatus = ACompanyDTO.ApprovalStatus,
                CreatedBy = currentCompany.CreatedBy,
                CreatedOn = currentCompany.CreatedOn,
                LastUpdatedBy = ACompanyDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };


            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objCompanyRepository.Merge(currentCompany, updatedCompany);
                    objCompanyRepository.UnitOfWork.Commit();

                    if (ACompanyDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_Company", " CompanyCode = '" + ACompanyDTO.CompanyCode + "' and OrganizationCode = '" + AOrganizationCode + "'",
                            "COMP", ACompanyDTO.LastUpdatedBy, //"/Master/CompanyAuthorize/" + ACompanyDTO.CompanyCode + "/A?", "MT");


            "/Master/CompanyAuthorize?ACompanyCode=" + ACompanyDTO.CompanyCode + "&AOrganizationCode=" + AOrganizationCode + "&", "MT");


                        objCompanyRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #region DeleteCompany
        public void DeleteCompany(string Id)
        {
            var company = objCompanyRepository.Get(Id);
            if (company != null)
            {
                objCompanyRepository.Remove(company);
                objCompanyRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindCompany
        public List<CompanyDTO> FindCompany(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objCompanyRepository.GetRecordCount();
            var company = objCompanyRepository.GetAll();
            if (company != null && company.Any())
            {
                List<CompanyDTO> lstCompanyDTO = new List<CompanyDTO>();
                foreach (var item in company)
                {
                    lstCompanyDTO.Add(Conversion.Mapping.CompanyToCompanyDTO(item));
                }
                return lstCompanyDTO;
            }
            else
                return new List<CompanyDTO>();
        }
        #endregion
        #region FindAll
        public List<CompanyDTO> FindAll(int APageNo, CompanyDTO ACompanyDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "CompanyCode",FilterValue = ACompanyDTO.CompanyCode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "CompanyName",FilterValue = ACompanyDTO.CompanyName,Operator = "Contains"},
                      new Utils.ExpressionList(){ FilterColumn = "ProductCategoryCode", FilterValue = ACompanyDTO.ProductCategoryCode, Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "CompanyCategoryCode", FilterValue = ACompanyDTO.CompanyCategoryCode, Operator = "Contains" },
                       new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils .OrganizationCode ,  Operator = "Equals" }
                    };

            Expression<Func<Company, bool>> expression = Utils.GetExpression<Company>(lstExpression);
            int iRecordCount = objCompanyRepository.GetRecordCount(expression);

            //var vCompanyDTO = objCompanyRepository.GetFiltered(APageNo, iRecordCount, expression);

            var vCompanyDTO = objCompanyRepository.GetFiltered(0, iRecordCount, expression);

            if (vCompanyDTO != null && vCompanyDTO.Any())
            {
                List<CompanyDTO> lstCompanyDTO = new List<CompanyDTO>();
                foreach (var item in vCompanyDTO)
                {
                    lstCompanyDTO.Add(Conversion.Mapping.CompanyToCompanyDTO(item));
                }
                return lstCompanyDTO;
            }
            else
                return new List<CompanyDTO>();

        }
        #endregion
        #region FindByCompanyCode
        public CompanyDTO FindByCompanyCode(string ACompanyCode, string AOrganizationCode)
        {
            Company company = objCompanyRepository.Get(ACompanyCode, AOrganizationCode);

            if (company != null)
            {
                if (company.ProductCategory == null)
                {
                    company.ProductCategory = objProductCategoryRepository.Get(company.ProductCategoryCode);
                }
                if (company.CompanyCategory == null)
                {
                    company.CompanyCategory = objCompanyCategoryRepository.Get(company.CompanyCategoryCode);
                }
                objCompanyRepository.UnitOfWork.RefreshDataInModel(ref company);
                return Conversion.Mapping.CompanyToCompanyDTO(company);
            }
            else
            {
                return new CompanyDTO();
            }
        }
        #endregion
        #endregion

        #region City

        public List<CityDTO> FindCity(int APageindex, int APageCount)
        {
            var cities = objCityRepository.GetAll();
            if (cities != null && cities.Any())
            {
                List<CityDTO> lstCityDTO = new List<CityDTO>();
                foreach (var city in cities)
                {
                    lstCityDTO.Add(Conversion.Mapping.CityToCityDTO(city));
                }
                return lstCityDTO;
            }
            else
                return new List<CityDTO>();
        }

        public void DeleteCity(string ACityCode)
        {
            var city = objCityRepository.Get(ACityCode);
            if (city != null)
            {
                objCityRepository.Remove(city);
                objCityRepository.UnitOfWork.Commit();
            }
        }

        public CityDTO FindByCityCode(string ACityCode, string AOrganizationCode)
        {
            var city = objCityRepository.Get(ACityCode, AOrganizationCode);
            if (city != null)
            {
                if (city.CityCategory == null)
                {
                    city.CityCategory = objCityCategoryRepository.Get(city.CityCategoryCode);
                }
                if (city.Country == null)
                {
                    city.Country = objCountryRepository.Get(city.CountryCode);
                }
                if (city.Region == null)
                {
                    city.Region = objRegionRepository.Get(city.RegionCode);
                }
                if (city.Hub == null)
                {
                    city.Hub = objHubRepository.Get(city.HubCode);
                }
                if (city.Spoke == null)
                {
                    city.Spoke = objSpokeRepository.Get(city.SpokeCode);
                }
                if (city.Communa == null)
                {
                    city.Communa = objCommunaRepository.Get(city.CommunaCode);
                }

                objCityRepository.UnitOfWork.RefreshDataInModel(ref city);
                return Conversion.Mapping.CityToCityDTO(city);
            }
            else
                return new CityDTO();
        }

        public void SaveCityDetails(CityDTO ACityDTO)
        {
            if (ACityDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddCurrencyWithNullInformation");

            var newCity = CityFactory.CreateCity(ACityDTO.CityCode, ACityDTO.CityName, ACityDTO.CountryCode, ACityDTO.RegionCode,
                ACityDTO.CityCategoryCode, ACityDTO.HubCode, ACityDTO.SpokeCode, ACityDTO.CommunaCode,
                ACityDTO.CreatedBy, ACityDTO.LastUpdatedBy, ACityDTO.ApprovalStatus, ACityDTO.OrganizationCode);


            try
            {

                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                        new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objCityRepository.Add(newCity);
                        objCityRepository.UnitOfWork.Commit();

                        if (ACityDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_City", " CityCode = '" + ACityDTO.CityCode + "' and OrganizationCode = '" + ACityDTO.OrganizationCode + "'",
                                "CITY", ACityDTO.LastUpdatedBy, //"/Master/CityAuthorize/" + ACityDTO.CityCode + "/A?", "MT");


                           "/Master/CityAuthorize?ACityCode=" + ACityDTO.CityCode + "&AOrganizationCode=" + ACityDTO.OrganizationCode + "&", "MT");

                            objCityRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                scope.Dispose();

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void UpdateCityDetails(string ACityCode, string AOrganizationCode, CityDTO ACityDTO)
        {
            if (ACityDTO == null)
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");
            City currentCity = objCityRepository.Get(ACityCode, AOrganizationCode);
            City updateCity = new City()
            {
                CityCode = ACityCode,
                OrganizationCode = AOrganizationCode,
                CityName = ACityDTO.CityName,
                CountryCode = ACityDTO.CountryCode,
                RegionCode = ACityDTO.RegionCode,
                CityCategoryCode = ACityDTO.CityCategoryCode,
                HubCode = ACityDTO.HubCode,
                SpokeCode = ACityDTO.SpokeCode,
                CommunaCode = ACityDTO.CommunaCode,
                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = ACityDTO.LastUpdatedBy,
                CreatedOn = currentCity.CreatedOn,
                CreatedBy = currentCity.CreatedBy,
                ApprovalStatus = ACityDTO.ApprovalStatus
            };


            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objCityRepository.Merge(currentCity, updateCity);
                    objCityRepository.UnitOfWork.Commit();

                    if (ACityDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_City", " CityCode = '" + ACityDTO.CityCode + "' and OrganizationCode = '" + AOrganizationCode + "'",
                            "CITY", ACityDTO.LastUpdatedBy, //"/Master/CityAuthorize/" + ACityDTO.CityCode + "/A?", "MT");


                       "/Master/CityAuthorize?ACityCode=" + ACityCode + "&AOrganizationCode=" + AOrganizationCode + "&", "MT");

                        objCityRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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

        public List<CityDTO> FindAll(int APageNo, CityDTO ACityDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "CityCode",FilterValue = ACityDTO.CityCode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "CityName",FilterValue = ACityDTO.CityName,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "CountryCode",FilterValue = ACityDTO.CountryCode,Operator = "Contains" },
                      new Utils.ExpressionList(){ FilterColumn = "RegionCode", FilterValue = ACityDTO.RegionCode, Operator = "Contains"},
                      new Utils.ExpressionList(){ FilterColumn = "CityCategoryCode", FilterValue = ACityDTO.CityCategoryCode, Operator = "Contains"},
                      new Utils.ExpressionList(){ FilterColumn = "HubCode", FilterValue = ACityDTO.HubCode, Operator = "Contains"},
                      new Utils.ExpressionList(){ FilterColumn = "SpokeCode", FilterValue = ACityDTO.SpokeCode, Operator = "Contains"},
                      new Utils.ExpressionList(){ FilterColumn = "CommunaCode", FilterValue = ACityDTO.CommunaCode, Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils .OrganizationCode , 
                                                                                                            Operator = "Equals" }
                      
                    };

            Expression<Func<City, bool>> expression = Utils.GetExpression<City>(lstExpression);
            int iRecordCount = objCityRepository.GetRecordCount(expression);

            //var vCityDTO = objCityRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vCityDTO = objCityRepository.GetFiltered(0, iRecordCount, expression);

            if (vCityDTO != null && vCityDTO.Any())
            {
                List<CityDTO> lstCityDTO = new List<CityDTO>();
                foreach (var city in vCityDTO)
                {
                    lstCityDTO.Add(Conversion.Mapping.CityToCityDTO(city));
                }
                return lstCityDTO;
            }
            else
                return new List<CityDTO>();
        }

        public CityNewDTO GetCityRegionByCommuna(string ACommunaCode, string AType)
        {
            CityNewDTO objCityNewDTO = new CityNewDTO();
            City city = objCityRepository.GetAll().Where(x => x.CommunaCode == ACommunaCode).FirstOrDefault();
            string strCode = "";

            if (city != null)
            {
                objCityNewDTO.CountryCode = city.CountryCode;
                objCityNewDTO.CityCode = city.CityCode;
                objCityNewDTO.CityName = city.CityName;
                objCityNewDTO.CommunaCode = city.CommunaCode;
                objCityNewDTO.CommunaName = city.CommunaName;
                objCityNewDTO.RegionCode = city.RegionCode;
            }

            if (AType == "City")
            {
                strCode = city.CityCode;
            }
            else if (AType == "Region")
            {
                strCode = city.CommunaCode;
            };

            return objCityNewDTO;
        }

        #endregion

        #region Credit Segment

        #region SaveCreditSegmentDetails
        public void SaveCreditSegmentDetails(CreditSegmentDTO ACreditSegmentDTO)
        {
            if (ACreditSegmentDTO == null)
            {
                throw new ArgumentException("");
            }

            var newCreditSegment = CreditSegmentFactory.CreateCreditSegment(ACreditSegmentDTO.CardSegmentCode, ACreditSegmentDTO.CardSegmentName,
                ACreditSegmentDTO.LegalEntityCode, ACreditSegmentDTO.EffectiveFromDate, ACreditSegmentDTO.EffectiveToDate, ACreditSegmentDTO.IsOTBReplenishment,
                ACreditSegmentDTO.CreatedBy, ACreditSegmentDTO.LastUpdatedBy, ACreditSegmentDTO.ApprovalStatus, ACreditSegmentDTO.OrganizationCode, null);

            try
            {

                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                        new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objCreditSegmentRepository.Add(newCreditSegment);
                        objCreditSegmentRepository.UnitOfWork.Commit();
                        if (ACreditSegmentDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_CreditSegment", " CardSegmentCode = '" + ACreditSegmentDTO.CardSegmentCode + "' and OrganizationCode = '" + ACreditSegmentDTO.OrganizationCode + "'",
                                "CRDTSGMT", ACreditSegmentDTO.LastUpdatedBy,// "/Master/CreditSegmentAuthorize/" + ACreditSegmentDTO.CardSegmentCode + "/A?", "MT" );

                             "/Master/CreditSegmentAuthorize?ACardSegmentCode=" + ACreditSegmentDTO.CardSegmentCode + "&AOrganizationCode=" + ACreditSegmentDTO.OrganizationCode + "&", "MT");
                            objCreditSegmentRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #region UpdateCreditSegmentDetails
        public void UpdateCreditSegmentDetails(string ACardSegmentCode, string AOrganizationCode, CreditSegmentDTO ACreditSegmentDTO)
        {
            if (ACreditSegmentDTO == null)
            {
                throw new ArgumentException("");
            }

            CreditSegment currentCreditSegment = objCreditSegmentRepository.Get(ACardSegmentCode, AOrganizationCode);
            CreditSegment updatedCreditSegment = new CreditSegment()
            {
                OrganizationCode = AOrganizationCode,
                CardSegmentCode = ACardSegmentCode,
                CardSegmentName = ACreditSegmentDTO.CardSegmentName,
                LegalEntityCode = ACreditSegmentDTO.LegalEntityCode,
                EffectiveFromDate = ACreditSegmentDTO.EffectiveFromDate,
                EffectiveToDate = ACreditSegmentDTO.EffectiveToDate,
                IsOTBReplenishment = ACreditSegmentDTO.IsOTBReplenishment,
                ApprovalStatus = ACreditSegmentDTO.ApprovalStatus,
                CreatedBy = currentCreditSegment.CreatedBy,
                CreatedOn = currentCreditSegment.CreatedOn,
                LastUpdatedBy = ACreditSegmentDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };


            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                   new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {

                    objCreditSegmentRepository.Merge(currentCreditSegment, updatedCreditSegment);
                    objCreditSegmentRepository.UnitOfWork.Commit();

                    if (ACreditSegmentDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_CreditSegment", " CardSegmentCode = '" + ACardSegmentCode + "' and OrganizationCode = '" + AOrganizationCode + "'",
                            "CRDTSGMT", ACreditSegmentDTO.LastUpdatedBy,// "/Master/CreditSegmentAuthorize/" + ACreditSegmentDTO.CardSegmentCode + "/A?", "MT" );

                         "/Master/CreditSegmentAuthorize?ACardSegmentCode=" + ACardSegmentCode + "&AOrganizationCode=" + AOrganizationCode + "&", "MT");
                        objCreditSegmentRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #endregion
        #region DeleteCreditSegment
        public void DeleteCreditSegment(string ACardSegmentCode)
        {
            var creditSegment = objCreditSegmentRepository.Get(ACardSegmentCode);
            if (creditSegment != null)
            {
                objCreditSegmentRepository.Remove(creditSegment);
                objCreditSegmentRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindCreditSegment
        public List<CreditSegmentDTO> FindCreditSegment(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objCreditSegmentRepository.GetRecordCount();
            var creditSegment = objCreditSegmentRepository.GetAll();
            if (creditSegment != null && creditSegment.Any())
            {
                List<CreditSegmentDTO> lstCreditSegmentDTO = new List<CreditSegmentDTO>();
                foreach (var item in creditSegment)
                {
                    lstCreditSegmentDTO.Add(Conversion.Mapping.CreditSegmentToCreditSegmentDTO(item));
                }
                return lstCreditSegmentDTO;
            }
            else
                return new List<CreditSegmentDTO>();
        }
        #endregion
        #region FindAll
        public List<CreditSegmentDTO> FindAll(int APageNo, CreditSegmentDTO ACreditSegmentDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { 
                        new Utils.ExpressionList() { FilterColumn = "CardSegmentCode",FilterValue = ACreditSegmentDTO.CardSegmentCode, 
                                                                                                            Operator = "Contains" },
                        new Utils.ExpressionList() { FilterColumn = "CardSegmentName",FilterValue = ACreditSegmentDTO.CardSegmentName,
                                                                                                            Operator = "Contains"},
                        new Utils.ExpressionList(){ FilterColumn = "EffectiveFromDate", FilterValue = ACreditSegmentDTO.EffectiveFromDate, 
                                                                                                            Operator = "GreaterThanOrEqualTo"},
                        new Utils.ExpressionList(){ FilterColumn = "EffectiveToDate", FilterValue = ACreditSegmentDTO.EffectiveToDate, 
                                                                                                            Operator = "LessThanOrEqualTo"},
                         new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils .OrganizationCode , 
                                                                                                            Operator = "Equals" }
                    };

            Expression<Func<CreditSegment, bool>> expression = Utils.GetExpression<CreditSegment>(lstExpression);
            int iRecordCount = objCreditSegmentRepository.GetRecordCount(expression);

            //var vCreditSegmentDTO = objCreditSegmentRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vCreditSegmentDTO = objCreditSegmentRepository.GetFiltered(0, iRecordCount, expression);

            if (vCreditSegmentDTO != null && vCreditSegmentDTO.Any())
            {
                List<CreditSegmentDTO> lstCreditSegmentDTO = new List<CreditSegmentDTO>();
                foreach (var item in vCreditSegmentDTO)
                {
                    lstCreditSegmentDTO.Add(Conversion.Mapping.CreditSegmentToCreditSegmentDTO(item));
                }
                return lstCreditSegmentDTO;
            }
            else
                return new List<CreditSegmentDTO>();

        }
        #endregion
        #region FindByCardSegmentCode
        public CreditSegmentDTO FindByCardSegmentCode(string ACardSegmentCode, string AOrganizationCode)
        {
            CreditSegment creditSegment = objCreditSegmentRepository.Get(ACardSegmentCode, AOrganizationCode);
            if (creditSegment != null)
            {
                if (creditSegment.LegalEntity == null)
                {
                    creditSegment.LegalEntity = objLegalEntityRepository.Get(creditSegment.LegalEntityCode);
                }
                objCreditSegmentRepository.UnitOfWork.RefreshDataInModel(ref creditSegment);
                return Conversion.Mapping.CreditSegmentToCreditSegmentDTO(creditSegment);
            }
            else
            {
                return new CreditSegmentDTO();
            }
        }
        #endregion

        #endregion

        #region Credit SubSegment

        #region SaveCreditSubSegmentDetails
        public void SaveCreditSubSegmentDetails(CreditSubSegmentDTO ACreditSubSegmentDTO)
        {
            if (ACreditSubSegmentDTO == null)
            {
                throw new ArgumentException("");
            }

            var newCreditSubSegment = CreditSubSegmentFactory.CreateCreditSubSegment(ACreditSubSegmentDTO.CreditSegmentCode,
                ACreditSubSegmentDTO.CreditSubSegmentCode, ACreditSubSegmentDTO.Description, ACreditSubSegmentDTO.CreatedBy,
                ACreditSubSegmentDTO.LastUpdatedBy, ACreditSubSegmentDTO.ApprovalStatus, null);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objCreditSubSegmentRepository.Add(newCreditSubSegment);
                        objCreditSubSegmentRepository.UnitOfWork.Commit();
                        if (ACreditSubSegmentDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_CreditSubSegment", " CreditSegmentCode = '" + ACreditSubSegmentDTO.CreditSegmentCode + "'" + " AND CreditSubSegmentCode = '" + ACreditSubSegmentDTO.CreditSubSegmentCode + "'",
                                    "CRSBSGMT", ACreditSubSegmentDTO.LastUpdatedBy,
                                    "/Master/CreditSubSegmentAuthorize?ACreditSegmentCode=" + ACreditSubSegmentDTO.CreditSegmentCode + "&ACreditSubSegmentCode=" + ACreditSubSegmentDTO.CreditSubSegmentCode + "&", "MT");
                            objCreditSubSegmentRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType", paramCollection);
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
        #region UpdateCreditSubSegmentDetails
        public void UpdateCreditSubSegmentDetails(CreditSubSegmentDTO ACreditSubSegmentDTO)
        {
            if (ACreditSubSegmentDTO == null)
            {
                throw new ArgumentException("");
            }

            CreditSubSegment currentCreditSubSegment = objCreditSubSegmentRepository.Get(ACreditSubSegmentDTO.CreditSegmentCode, ACreditSubSegmentDTO.CreditSubSegmentCode);
            CreditSubSegment updatedCreditSubSegment = new CreditSubSegment()
            {
                CreditSegmentCode = ACreditSubSegmentDTO.CreditSegmentCode,
                CreditSubSegmentCode = ACreditSubSegmentDTO.CreditSubSegmentCode,
                Description = ACreditSubSegmentDTO.Description,
                ApprovalStatus = ACreditSubSegmentDTO.ApprovalStatus,
                CreatedBy = currentCreditSubSegment.CreatedBy,
                CreatedOn = currentCreditSubSegment.CreatedOn,
                LastUpdatedBy = ACreditSubSegmentDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objCreditSubSegmentRepository.Merge(currentCreditSubSegment, updatedCreditSubSegment);
                    objCreditSubSegmentRepository.UnitOfWork.Commit();
                    if (ACreditSubSegmentDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_CreditSubSegment", " CreditSegmentCode = '" + ACreditSubSegmentDTO.CreditSegmentCode + "'" + " AND CreditSubSegmentCode = '" + ACreditSubSegmentDTO.CreditSubSegmentCode + "'",
                                "CRSBSGMT", ACreditSubSegmentDTO.LastUpdatedBy,
                                "/Master/CreditSubSegmentAuthorize?ACreditSegmentCode=" + ACreditSubSegmentDTO.CreditSegmentCode + "&ACreditSubSegmentCode=" + ACreditSubSegmentDTO.CreditSubSegmentCode + "&", "MT");

                        objCreditSubSegmentRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType", paramCollection);
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
        #endregion
        #region DeleteCreditSubSegment
        public void DeleteCreditSubSegment(string ACreditSegmentCode, int ACreditSubSegmentCode)
        {
            var creditSubSegment = objCreditSubSegmentRepository.Get(ACreditSegmentCode, ACreditSubSegmentCode);
            if (creditSubSegment != null)
            {
                objCreditSubSegmentRepository.Remove(creditSubSegment);
                objCreditSubSegmentRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindCreditSubSegment
        public List<CreditSubSegmentDTO> FindCreditSubSegment(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objCreditSubSegmentRepository.GetRecordCount();
            var creditSubSegment = objCreditSubSegmentRepository.GetAll();
            if (creditSubSegment != null && creditSubSegment.Any())
            {
                List<CreditSubSegmentDTO> lstCreditSubSegmentDTO = new List<CreditSubSegmentDTO>();
                foreach (var item in creditSubSegment)
                {
                    lstCreditSubSegmentDTO.Add(Conversion.Mapping.CreditSubSegmentToCreditSubSegmentDTO(item));
                }
                return lstCreditSubSegmentDTO;
            }
            else
                return new List<CreditSubSegmentDTO>();
        }
        #endregion
        #region FindAll
        public List<CreditSubSegmentDTO> FindAll(int APageNo, CreditSubSegmentDTO ACreditSubSegmentDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { 
                        new Utils.ExpressionList() { FilterColumn = "CreditSegmentCode",
                            FilterValue = ACreditSubSegmentDTO.CreditSegmentCode, 
                            Operator = ACreditSubSegmentDTO.CreditSegmentCode == string.Empty ? "Contains" : "Equals" },

                        new Utils.ExpressionList() { FilterColumn = "CreditSubSegmentCode",
                            FilterValue = ACreditSubSegmentDTO.CreditSubSegmentCode,
                            Operator = ACreditSubSegmentDTO.CreditSubSegmentCode == 0 ? "NotEquals" : "Equals"},

                         new Utils.ExpressionList() { FilterColumn = "OrganizationCode",
                            FilterValue = Utils .OrganizationCode ,
                            Operator = "Equals"}

                    };

            Expression<Func<CreditSubSegment, bool>> expression = Utils.GetExpression<CreditSubSegment>(lstExpression);
            int iRecordCount = objCreditSubSegmentRepository.GetRecordCount(expression);
            List<CreditSubSegmentDTO> lstCreditSubSegmentDTO = new List<CreditSubSegmentDTO>();
            var vCreditSubSegmentDTO = objCreditSubSegmentRepository.GetFiltered(0, iRecordCount, expression);

            if (vCreditSubSegmentDTO != null && vCreditSubSegmentDTO.Any())
            {

                foreach (var item in vCreditSubSegmentDTO)
                {
                    lstCreditSubSegmentDTO.Add(Conversion.Mapping.CreditSubSegmentToCreditSubSegmentDTO(item));
                }
            }

            return lstCreditSubSegmentDTO;

        }
        #endregion
        #region FindByCreditSubSegmentCode
        public CreditSubSegmentDTO FindByCreditSubSegmentCode(string ACreditSegmentCode, int ACreditSubSegmentCode)
        {
            CreditSubSegment creditSubSegment = objCreditSubSegmentRepository.Get(ACreditSegmentCode, ACreditSubSegmentCode);
            if (creditSubSegment != null)
            {
                if (creditSubSegment.objCreditSegment == null)
                {
                    creditSubSegment.objCreditSegment = objCreditSegmentRepository.Get(creditSubSegment.CreditSegmentCode);
                }
                if (creditSubSegment.objSystemParameter == null)
                {
                    creditSubSegment.objSystemParameter = objSystemParameterRepository.Get(creditSubSegment.CreditSubSegmentCode);
                }
                objCreditSegmentRepository.UnitOfWork.RefreshDataInModel(ref creditSubSegment);
                return Conversion.Mapping.CreditSubSegmentToCreditSubSegmentDTO(creditSubSegment);
            }
            else
            {
                return new CreditSubSegmentDTO();
            }
        }
        #endregion

        #endregion

        #region SystemParameter

        #region SaveSystemParameterDetails
        public void SaveSystemParameterDetails(SystemParameterDTO ASystemParameterDTO)
        {
            if (ASystemParameterDTO == null)
            {
                throw new ArgumentException("");
            }

            var newSystemParameter = SystemParameterFactory.CreateSystemParameter(ASystemParameterDTO.ParameterId,
                ASystemParameterDTO.ParameterName, ASystemParameterDTO.ParameterValue, ASystemParameterDTO.Description,
                ASystemParameterDTO.Function, ASystemParameterDTO.DefaultValue, ASystemParameterDTO.LowValue, ASystemParameterDTO.HighValue,
                ASystemParameterDTO.CreatedBy, ASystemParameterDTO.LastUpdatedBy, ASystemParameterDTO.ApprovalStatus, ASystemParameterDTO.OrganizationCode, null);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objSystemParameterRepository.Add(newSystemParameter);
                        objSystemParameterRepository.UnitOfWork.Commit();
                        if (ASystemParameterDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_SystemParameter", " ParameterId = '" + newSystemParameter.ParameterId + "'",
                                "SYSPARM", ASystemParameterDTO.CreatedBy, "/Master/SystemParameterAuthorize/" + newSystemParameter.ParameterId + "/A?", "MT");
                            objProductTypeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UpdateSystemParameterDetails
        public void UpdateSystemParameterDetails(int AParameterId, SystemParameterDTO ASystemParameterDTO)
        {
            if (ASystemParameterDTO == null)
            {
                throw new ArgumentException("");
            }

            SystemParameter currentSystemParameter = objSystemParameterRepository.Get(AParameterId);
            SystemParameter updatedSystemParameter = new SystemParameter()
            {
                ParameterId = AParameterId,
                ParameterName = ASystemParameterDTO.ParameterName,
                OrganizationCode = ASystemParameterDTO.OrganizationCode,
                ParameterValue = ASystemParameterDTO.ParameterValue,
                Description = ASystemParameterDTO.Description,
                DefaultValue = ASystemParameterDTO.DefaultValue,
                FunctionName = ASystemParameterDTO.Function,
                LowValue = ASystemParameterDTO.LowValue,
                HighValue = ASystemParameterDTO.HighValue,
                ApprovalStatus = currentSystemParameter.ApprovalStatus == "A" ? "A" : ASystemParameterDTO.ApprovalStatus,
                CreatedBy = currentSystemParameter.CreatedBy,
                CreatedOn = currentSystemParameter.CreatedOn,
                LastUpdatedBy = ASystemParameterDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };

            if (currentSystemParameter.ApprovalStatus == "A")
            {
                ASystemParameterDTO.ApprovalStatus = "A";
            }

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objSystemParameterRepository.Merge(currentSystemParameter, updatedSystemParameter);
                    objSystemParameterRepository.UnitOfWork.Commit();
                    if (ASystemParameterDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_SystemParameter", " ParameterId = '" + AParameterId + "'",
                                "SYSPARM", ASystemParameterDTO.LastUpdatedBy, "/Master/SystemParameterAuthorize/" + AParameterId + "/A?", "MT");
                        objSystemParameterRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);


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

        #region DeleteSystemParameter
        public void DeleteSystemParameter(int AParameterId)
        {
            var systemParameter = objSystemParameterRepository.Get(AParameterId);
            if (systemParameter != null)
            {
                objSystemParameterRepository.Remove(systemParameter);
                objSystemParameterRepository.UnitOfWork.Commit();
            }
        }
        #endregion

        #region FindSystemParameter
        public List<SystemParameterDTO> FindSystemParameter(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objSystemParameterRepository.GetRecordCount();
            var systemParameter = objSystemParameterRepository.GetAll();
            if (systemParameter != null && systemParameter.Any())
            {
                List<SystemParameterDTO> lstSystemParameterDTO = new List<SystemParameterDTO>();
                foreach (var item in systemParameter)
                {
                    lstSystemParameterDTO.Add(Conversion.Mapping.SystemParameterToSystemParameterDTO(item));
                }
                return lstSystemParameterDTO;
            }
            else
                return new List<SystemParameterDTO>();
        }
        #endregion

        #region FindAll
        public List<SystemParameterDTO> FindAll(int APageNo, SystemParameterDTO ASystemParameterDTO)
        {
            //int iRecordCount = objSystemParameterRepository.GetRecordCount(x => x.ParameterName.Contains(ASystemParameterDTO.ParameterName));

            ////var vSystemParameterDTO = objSystemParameterRepository.GetFiltered(APageNo, iRecordCount, x => x.ParameterName.Contains(ASystemParameterDTO.ParameterName));
            //var vSystemParameterDTO = objSystemParameterRepository.GetFiltered(0, iRecordCount, x => x.ParameterName.Contains(ASystemParameterDTO.ParameterName));
            //if (vSystemParameterDTO != null && vSystemParameterDTO.Any())
            //{
            //    List<SystemParameterDTO> lstSystemParameterDTO = new List<SystemParameterDTO>();
            //    foreach (var item in vSystemParameterDTO)
            //    {
            //        lstSystemParameterDTO.Add(Conversion.Mapping.SystemParameterToSystemParameterDTO(item));
            //    }
            //    return lstSystemParameterDTO;
            //}
            //else
            //    return new List<SystemParameterDTO>();

            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "ParameterName",FilterValue = ASystemParameterDTO .ParameterName, Operator = "Contains" },               
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode ,Operator = "Equals"}              
                    };

            Expression<Func<SystemParameter, bool>> expression = Utils.GetExpression<SystemParameter>(lstExpression);
            int iRecordCount = objSystemParameterRepository.GetRecordCount(expression);

            //var vSourcingDTO = objSourcingRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vSystemParameterDTO = objSystemParameterRepository.GetFiltered(0, iRecordCount, expression);

            if (vSystemParameterDTO != null && vSystemParameterDTO.Any())
            {
                List<SystemParameterDTO> lstSystemParameterDTO = new List<SystemParameterDTO>();
                foreach (var item in vSystemParameterDTO)
                {
                    lstSystemParameterDTO.Add(Conversion.Mapping.SystemParameterToSystemParameterDTO(item));
                }
                return lstSystemParameterDTO;
            }
            else
                return new List<SystemParameterDTO>();

        }
        #endregion

        #region FindByParameterId
        public SystemParameterDTO FindByParameterId(int AParameterId)
        {
            SystemParameter systemParameter = objSystemParameterRepository.Get(AParameterId);
            if (systemParameter != null)
            {
                objSystemParameterRepository.UnitOfWork.RefreshDataInModel(ref systemParameter);
                return Conversion.Mapping.SystemParameterToSystemParameterDTO(systemParameter);
            }
            else
            {
                return new SystemParameterDTO();
            }
        }
        #endregion


        public SystemParameterDTO FindBySystemParameterName(string AParamName)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { 
                        new Utils.ExpressionList() { FilterColumn = "ParameterName",FilterValue = AParamName, Operator = "Equals" },               
                        new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode ,Operator = "Equals"}  ,
                        new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "A", Operator = "Equals"}          
                    };

            Expression<Func<SystemParameter, bool>> expression = Utils.GetExpression<SystemParameter>(lstExpression);
            int iRecordCount = objSystemParameterRepository.GetRecordCount(expression);

            //var vSourcingDTO = objSourcingRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vSystemParameterDTO = objSystemParameterRepository.GetFiltered(0, iRecordCount, expression).FirstOrDefault();

            return Conversion.Mapping.SystemParameterToSystemParameterDTO(vSystemParameterDTO);

        }

        #endregion //** end of System Parameter Region **//

        #region Designation

        public List<DesignationDTO> FindDesignation(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindDesignation");

            int iRecordCount = objDesignationRepository.GetRecordCount();
            var designations = objDesignationRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);
            if (designations != null && designations.Any())
            {
                List<DesignationDTO> lstDesignationDTO = new List<DesignationDTO>();
                foreach (var designation in designations)
                {
                    lstDesignationDTO.Add(Conversion.Mapping.DesignationToDesignationDTO(designation));
                }
                return lstDesignationDTO;
            }
            else
                return new List<DesignationDTO>();
        }

        public void DeleteDesignation(string ADesignationCode)
        {
            var designation = objDesignationRepository.Get(ADesignationCode);

            if (designation != null)
            {
                objDesignationRepository.Remove(designation);
                objDesignationRepository.UnitOfWork.Commit();
            }
        }

        public DesignationDTO FindByDesignationCode(string ACode, string AOrganizationCode)
        {
            var designation = objDesignationRepository.Get(ACode, AOrganizationCode);

            if (designation != null)
            {
                if (designation.Positions == null)
                {
                    designation.Positions = objPositionRepository.Get(designation.Position);
                }
                objDesignationRepository.UnitOfWork.RefreshDataInModel(ref designation);
                return Conversion.Mapping.DesignationToDesignationDTO(designation);
            }
            else
            {
                return new DesignationDTO();
            }

        }

        public void SaveDesignationDetails(DesignationDTO ADesignationDTO)
        {
            if (ADesignationDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddCurrencyWithNullInformation");

            var newDesignation = DesignationFactory.CreateDesignation(ADesignationDTO.DesignationCode, ADesignationDTO.DesignationDescription,
                ADesignationDTO.Position, ADesignationDTO.NTP, ADesignationDTO.HRP, ADesignationDTO.Rank, ADesignationDTO.EffectiveFromDate,
                ADesignationDTO.EffectiveToDate, ADesignationDTO.CreatedBy, ADesignationDTO.LastUpdatedBy, ADesignationDTO.ApprovalStatus, ADesignationDTO.OrganizationCode, null);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objDesignationRepository.Add(newDesignation);
                        objDesignationRepository.UnitOfWork.Commit();
                        if (ADesignationDTO.ApprovalStatus == "P")
                        {
                            //SqlParameter[] paramCollection = new SqlParameter[] { };
                            //paramCollection = GetApprovalQueueProcedureParameters("MT_Designation", " DesignationCode = '" + ADesignationDTO.DesignationCode + "'",
                            //    "DESIG", ADesignationDTO.CreatedBy, "/Master/DesignationAuthorize/" + ADesignationDTO.DesignationCode + "/A?", "MT");
                            //objDesignationRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType", paramCollection);

                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_Designation", " DesignationCode = '" + ADesignationDTO.DesignationCode + "' and OrganizationCode = '" + ADesignationDTO.OrganizationCode + "'",
                                "DESIG", ADesignationDTO.LastUpdatedBy, //"/Master/DesignationAuthorize/" + ADesignationDTO.DesignationCode + "/A?", "MT");
                            "/Master/DesignationAuthorize?ADesignationCode=" + ADesignationDTO.DesignationCode + "&AOrganizationCode=" + ADesignationDTO.OrganizationCode + "&", "MT");
                            objDesignationRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);


                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDesignationDetails(string ADesignationCode, string AOrganizationCode, DesignationDTO ADesignationDTO)
        {
            if (ADesignationDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");
            }
            Designation currentDesignation = objDesignationRepository.Get(ADesignationCode, AOrganizationCode);
            Designation updatedDesignation = new Designation()
            {
                DesignationCode = ADesignationCode,
                OrganizationCode = AOrganizationCode,
                DesignationDescription = ADesignationDTO.DesignationDescription,
                Position = ADesignationDTO.Position,
                NTP = ADesignationDTO.NTP,
                HRP = ADesignationDTO.HRP,
                Rank = ADesignationDTO.Rank,
                EffectiveFromDate = ADesignationDTO.EffectiveFromDate,
                EffectiveToDate = ADesignationDTO.EffectiveToDate,
                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = ADesignationDTO.LastUpdatedBy,
                CreatedOn = currentDesignation.CreatedOn,
                CreatedBy = currentDesignation.CreatedBy,
                ApprovalStatus = ADesignationDTO.ApprovalStatus

            };
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objDesignationRepository.Merge(currentDesignation, updatedDesignation);
                    objDesignationRepository.UnitOfWork.Commit();
                    if (ADesignationDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_Designation", " DesignationCode = '" + ADesignationDTO.DesignationCode + "' and OrganizationCode = '" + AOrganizationCode + "'",
                            "DESIG", ADesignationDTO.LastUpdatedBy, //"/Master/DesignationAuthorize/" + ADesignationDTO.DesignationCode + "/A?", "MT");
                        "/Master/DesignationAuthorize?ADesignationCode=" + ADesignationCode + "&AOrganizationCode=" + AOrganizationCode + "&", "MT");
                        objDesignationRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);



                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<DesignationDTO> FindAll(int APageNo, DesignationDTO ADesignationDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "DesignationCode", FilterValue = ADesignationDTO.DesignationCode, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "DesignationDescription", FilterValue = ADesignationDTO.DesignationDescription, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "Position", FilterValue = ADesignationDTO.Position, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "OrganizationCode", FilterValue = Utils .OrganizationCode , Operator = "Equals"},
                  new Utils.ExpressionList(){FilterColumn = "Rank", FilterValue = ADesignationDTO.Rank, Operator = "Equals"}
            };

            Expression<Func<Designation, bool>> expression = Utils.GetExpression<Designation>(lstExp);
            int iRecordCount = objDesignationRepository.GetRecordCount(expression);

            //var vDesignationDTO = objDesignationRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vDesignationDTO = objDesignationRepository.GetFiltered(0, iRecordCount, expression);

            if (vDesignationDTO != null && vDesignationDTO.Any())
            {
                List<DesignationDTO> lstDesignationDTO = new List<DesignationDTO>();
                foreach (var designation in vDesignationDTO)
                {
                    lstDesignationDTO.Add(Conversion.Mapping.DesignationToDesignationDTO(designation));
                }
                return lstDesignationDTO;
            }
            else
                return new List<DesignationDTO>();
        }

        #endregion

        #region Customer Segment

        #region SaveCustomerSegmentDetails
        public void SaveCustomerSegmentDetails(CustomerSegmentDTO ACustomerSegmentDTO)
        {
            if (ACustomerSegmentDTO == null)
            {
                throw new ArgumentException("");
            }

            var newCustomerSegment = CustomerSegmentFactory.CreateCustomerSegment(ACustomerSegmentDTO.SegmentCode, ACustomerSegmentDTO.SegmentName,
                ACustomerSegmentDTO.SegementDescription, ACustomerSegmentDTO.IsDefault, ACustomerSegmentDTO.CreatedBy,
                ACustomerSegmentDTO.LastUpdatedBy, ACustomerSegmentDTO.ApprovalStatus, ACustomerSegmentDTO.OrganizationCode, null);
            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objCustomerSegmentRepository.Add(newCustomerSegment);
                        objCustomerSegmentRepository.UnitOfWork.Commit();
                        if (ACustomerSegmentDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_CustomerSegment", " SegmentCode = '" + newCustomerSegment.SegmentCode + "' and OrganizationCode = '" + newCustomerSegment.OrganizationCode + "'",
                                "CSTSGMT", ACustomerSegmentDTO.CreatedBy,// "/Master/CustomerSegmentAuthorize/" + newCustomerSegment.SegmentCode + "/A?", "MT");
                                 "/Master/CustomerSegmentAuthorize?ASegmentCode=" + newCustomerSegment.SegmentCode + "&AOrganizationCode=" + newCustomerSegment.OrganizationCode + "&", "MT");
                            objCustomerSegmentRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);


                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region UpdateCustomerSegmentDetails
        public void UpdateCustomerSegmentDetails(string ASegmentCode, string AOrganizationCode, CustomerSegmentDTO ACustomerSegmentDTO)
        {
            if (ACustomerSegmentDTO == null)
            {
                throw new ArgumentException("");
            }

            CustomerSegment currentCustomerSegment = objCustomerSegmentRepository.Get(ASegmentCode, AOrganizationCode);
            CustomerSegment updatedCustomerSegment = new CustomerSegment()
            {
                SegmentCode = ASegmentCode,
                OrganizationCode = AOrganizationCode,
                SegmentName = ACustomerSegmentDTO.SegmentName,
                SegementDescription = ACustomerSegmentDTO.SegementDescription,
                IsDefault = ACustomerSegmentDTO.IsDefault,
                ApprovalStatus = ACustomerSegmentDTO.ApprovalStatus,
                CreatedBy = currentCustomerSegment.CreatedBy,
                CreatedOn = currentCustomerSegment.CreatedOn,
                LastUpdatedBy = ACustomerSegmentDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objCustomerSegmentRepository.Merge(currentCustomerSegment, updatedCustomerSegment);
                    objCustomerSegmentRepository.UnitOfWork.Commit();
                    if (ACustomerSegmentDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_CustomerSegment", " SegmentCode = '" + ASegmentCode + "' and OrganizationCode = '" + AOrganizationCode + "'",
                            "CSTSGMT", ACustomerSegmentDTO.LastUpdatedBy,// "/Master/CustomerSegmentAuthorize/" + newCustomerSegment.SegmentCode + "/A?", "MT");
                             "/Master/CustomerSegmentAuthorize?ASegmentCode=" + ASegmentCode + "&AOrganizationCode=" + AOrganizationCode + "&", "MT");
                        objCustomerSegmentRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #region DeleteCustomerSegment
        public void DeleteCustomerSegment(string ASegmentCode)
        {
            var customerSegment = objCustomerSegmentRepository.Get(ASegmentCode);
            if (customerSegment != null)
            {
                objCustomerSegmentRepository.Remove(customerSegment);
                objCustomerSegmentRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindCustomerSegment
        public List<CustomerSegmentDTO> FindCustomerSegment(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objCustomerSegmentRepository.GetRecordCount();
            var customerSegment = objCustomerSegmentRepository.GetAll();
            if (customerSegment != null && customerSegment.Any())
            {
                List<CustomerSegmentDTO> lstCustomerSegmentDTO = new List<CustomerSegmentDTO>();
                foreach (var item in customerSegment)
                {
                    lstCustomerSegmentDTO.Add(Conversion.Mapping.CustomerSegmentToCustomerSegmentDTO(item));
                }
                return lstCustomerSegmentDTO;
            }
            else
                return new List<CustomerSegmentDTO>();
        }
        #endregion
        #region FindAll
        public List<CustomerSegmentDTO> FindAll(int APageNo, CustomerSegmentDTO ACustomerSegmentDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "SegmentCode",FilterValue = ACustomerSegmentDTO.SegmentCode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "SegmentName",FilterValue = ACustomerSegmentDTO.SegmentName,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils .OrganizationCode ,Operator = "Equals"}
                    };

            Expression<Func<CustomerSegment, bool>> expression = Utils.GetExpression<CustomerSegment>(lstExpression);
            int iRecordCount = objCustomerSegmentRepository.GetRecordCount(expression);

            //var vCustmerSegmentDTO = objCustomerSegmentRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vCustmerSegmentDTO = objCustomerSegmentRepository.GetFiltered(0, iRecordCount, expression);

            if (vCustmerSegmentDTO != null && vCustmerSegmentDTO.Any())
            {
                List<CustomerSegmentDTO> lstCustomerSegmentDTO = new List<CustomerSegmentDTO>();
                foreach (var item in vCustmerSegmentDTO)
                {
                    lstCustomerSegmentDTO.Add(Conversion.Mapping.CustomerSegmentToCustomerSegmentDTO(item));
                }
                return lstCustomerSegmentDTO;
            }
            else
                return new List<CustomerSegmentDTO>();

        }
        #endregion
        #region FindBySegmentCode
        public CustomerSegmentDTO FindBySegmentCode(string ASegmentCode, string AOrganizationCode)
        {
            CustomerSegment customerSegment = objCustomerSegmentRepository.Get(ASegmentCode, AOrganizationCode);
            if (customerSegment != null)
            {
                objCustomerSegmentRepository.UnitOfWork.RefreshDataInModel(ref customerSegment);
                return Conversion.Mapping.CustomerSegmentToCustomerSegmentDTO(customerSegment);
            }
            else
            {
                return new CustomerSegmentDTO();
            }
        }
        #endregion
        #region CheckIfDefaultCustomerSegmentExists
        public bool CheckIfDefaultCustomerSegmentExists(string ACustomerSegmentCode)
        {
            var segment = objCustomerSegmentRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode &&
                x.IsDefault == true &&
                x.SegmentCode != ACustomerSegmentCode &&
                !(x.ApprovalStatus == "DEL" || x.ApprovalStatus == "DIS"));
            if (segment.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #endregion

        #region Customer SubSegment

        #region SaveCustomerSubSegmentDetails
        public void SaveCustomerSubSegmentDetails(CustomerSubSegmentDTO ACustomerSubSegmentDTO)
        {
            if (ACustomerSubSegmentDTO == null)
            {
                throw new ArgumentException("");
            }

            var newCustomerSubSegment = CustomerSubSegmentFactory.CreateSubCustomerSegment(ACustomerSubSegmentDTO.SubSegmentCode,
                ACustomerSubSegmentDTO.SubSegmentName, ACustomerSubSegmentDTO.SubSegmentDesc, ACustomerSubSegmentDTO.IsDefault,
                 ACustomerSubSegmentDTO.CreatedBy, ACustomerSubSegmentDTO.LastUpdatedBy, ACustomerSubSegmentDTO.ApprovalStatus, ACustomerSubSegmentDTO.OrganizationCode, null);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                        new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objCustomerSubSegmentRepository.Add(newCustomerSubSegment);
                        objCustomerSubSegmentRepository.UnitOfWork.Commit();

                        if (ACustomerSubSegmentDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_CustomerSubSegment", " SubSegmentCode = '" + newCustomerSubSegment.SubSegmentCode + "' and OrganizationCode = '" + newCustomerSubSegment.OrganizationCode + "'",
                                "CSTSBSGMT", ACustomerSubSegmentDTO.CreatedBy, //"/Master/CustomerSubSegmentAuthorize/" + newCustomerSubSegment.SubSegmentCode + "/A?", "MT");
                                   "/Master/CustomerSubSegmentAuthorize?ASubSegmentCode=" + newCustomerSubSegment.SubSegmentCode + "&AOrganizationCode=" + newCustomerSubSegment.OrganizationCode + "&", "MT");
                            objCustomerSubSegmentRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);


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
        #region UpdateCustomerSubSegmentDetails
        public void UpdateCustomerSubSegmentDetails(string ASubSegmentCode, string AOrganizationCode, CustomerSubSegmentDTO ACustomerSubSegmentDTO)
        {
            if (ACustomerSubSegmentDTO == null)
            {
                throw new ArgumentException("");
            }

            CustomerSubSegment currentCustomerSubSegment = objCustomerSubSegmentRepository.Get(ASubSegmentCode, AOrganizationCode);
            CustomerSubSegment updatedCustomerSubSegment = new CustomerSubSegment()
            {
                SubSegmentCode = ASubSegmentCode,
                OrganizationCode = AOrganizationCode,
                SubSegmentName = ACustomerSubSegmentDTO.SubSegmentName,
                SubSegmentDesc = ACustomerSubSegmentDTO.SubSegmentDesc,
                IsDefault = ACustomerSubSegmentDTO.IsDefault,
                ApprovalStatus = ACustomerSubSegmentDTO.ApprovalStatus,
                CreatedBy = currentCustomerSubSegment.CreatedBy,
                CreatedOn = currentCustomerSubSegment.CreatedOn,
                LastUpdatedBy = ACustomerSubSegmentDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objCustomerSubSegmentRepository.Merge(currentCustomerSubSegment, updatedCustomerSubSegment);
                    objCustomerSubSegmentRepository.UnitOfWork.Commit();

                    if (ACustomerSubSegmentDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_CustomerSubSegment", " SubSegmentCode = '" + ASubSegmentCode + "' and OrganizationCode = '" + AOrganizationCode + "'",
                            "CSTSBSGMT", ACustomerSubSegmentDTO.LastUpdatedBy, //"/Master/CustomerSubSegmentAuthorize/" + newCustomerSubSegment.SubSegmentCode + "/A?", "MT");
                               "/Master/CustomerSubSegmentAuthorize?ASubSegmentCode=" + ASubSegmentCode + "&AOrganizationCode=" + AOrganizationCode + "&", "MT");
                        objCustomerSubSegmentRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);

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
        #endregion
        #region DeleteCustomerSubSegment
        public void DeleteCustomerSubSegment(string ASubSegmentCode)
        {
            var customerSubSegment = objCustomerSubSegmentRepository.Get(ASubSegmentCode);
            if (customerSubSegment != null)
            {
                objCustomerSubSegmentRepository.Remove(customerSubSegment);
                objCustomerSubSegmentRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindCustomerSubSegment
        public List<CustomerSubSegmentDTO> FindCustomerSubSegment(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objCustomerSubSegmentRepository.GetRecordCount();
            var customerSubSegment = objCustomerSubSegmentRepository.GetAll();
            if (customerSubSegment != null && customerSubSegment.Any())
            {
                List<CustomerSubSegmentDTO> lstCustomerSubSegmentDTO = new List<CustomerSubSegmentDTO>();
                foreach (var item in customerSubSegment)
                {
                    lstCustomerSubSegmentDTO.Add(Conversion.Mapping.CustomerSubSegmentToCustomerSubSegmentDTO(item));
                }
                return lstCustomerSubSegmentDTO;
            }
            else
                return new List<CustomerSubSegmentDTO>();
        }
        #endregion
        #region FindAll
        public List<CustomerSubSegmentDTO> FindAll(int APageNo, CustomerSubSegmentDTO ACustomerSubSegmentDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "SubSegmentCode",FilterValue = ACustomerSubSegmentDTO.SubSegmentCode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "SubSegmentName",FilterValue = ACustomerSubSegmentDTO.SubSegmentName,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode,Operator = "Equals"}
                    };

            Expression<Func<CustomerSubSegment, bool>> expression = Utils.GetExpression<CustomerSubSegment>(lstExpression);
            int iRecordCount = objCustomerSubSegmentRepository.GetRecordCount(expression);

            //var vCustmerSubSegmentDTO = objCustomerSubSegmentRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vCustmerSubSegmentDTO = objCustomerSubSegmentRepository.GetFiltered(0, iRecordCount, expression);

            if (vCustmerSubSegmentDTO != null && vCustmerSubSegmentDTO.Any())
            {
                List<CustomerSubSegmentDTO> lstCustomerSegmentDTO = new List<CustomerSubSegmentDTO>();
                foreach (var item in vCustmerSubSegmentDTO)
                {
                    lstCustomerSegmentDTO.Add(Conversion.Mapping.CustomerSubSegmentToCustomerSubSegmentDTO(item));
                }
                return lstCustomerSegmentDTO;
            }
            else
                return new List<CustomerSubSegmentDTO>();

        }
        #endregion
        #region FindBySubSegmentCode
        public CustomerSubSegmentDTO FindBySubSegmentCode(string ASubSegmentCode, string AOrganizationCode)
        {
            CustomerSubSegment customerSubSegment = objCustomerSubSegmentRepository.Get(ASubSegmentCode, AOrganizationCode);
            if (customerSubSegment != null)
            {
                objCustomerSubSegmentRepository.UnitOfWork.RefreshDataInModel(ref customerSubSegment);
                return Conversion.Mapping.CustomerSubSegmentToCustomerSubSegmentDTO(customerSubSegment);
            }
            else
            {
                return new CustomerSubSegmentDTO();
            }
        }
        #endregion
        #region CheckIfDefaultCustomerSubSegmentExists
        public bool CheckIfDefaultCustomerSubSegmentExists(string ACustomerSubSegmentCode)
        {
            var subSegment = objCustomerSubSegmentRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode &&
                x.IsDefault == true &&
                x.SubSegmentCode != ACustomerSubSegmentCode &&
                !(x.ApprovalStatus == "DEL" || x.ApprovalStatus == "DIS"));
            if (subSegment.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #endregion

        #region Card Status

        public List<CardStatusDTO> FindCardStatus(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindCardStatus");

            int iRecordCount = objCardStatusRepository.GetRecordCount();
            var cards = objCardStatusRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);
            if (cards != null && cards.Any())
            {
                List<CardStatusDTO> lstCardStatusDTO = new List<CardStatusDTO>();
                foreach (var card in cards)
                {
                    lstCardStatusDTO.Add(Conversion.Mapping.CardStatusToCardStatusDTO(card));
                }
                return lstCardStatusDTO;
            }
            else
                return new List<CardStatusDTO>();
        }

        public void DeleteCardStatus(string ACardStatusCode)
        {
            var cardstatus = objCardStatusRepository.Get(ACardStatusCode);

            if (cardstatus != null)
            {
                objCardStatusRepository.Remove(cardstatus);
                objCardStatusRepository.UnitOfWork.Commit();
            }
        }

        public CardStatusDTO FindByCardStatusCode(string ACode)
        {
            var cardstatus = objCardStatusRepository.Get(ACode, Utils.OrganizationCode);

            if (cardstatus != null)
            {
                if (cardstatus.Decision == null)
                {
                    cardstatus.Decision = objDecisionRepository.Get(cardstatus.DecisionCode);
                }

                objCardStatusRepository.UnitOfWork.RefreshDataInModel(ref cardstatus);
                return Conversion.Mapping.CardStatusToCardStatusDTO(cardstatus);
            }
            else
            {
                return new CardStatusDTO();
            }

        }

        public void SaveCardStatusDetails(CardStatusDTO ACardStatusDTO)
        {
            if (ACardStatusDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddCurrencyWithNullInformation");

            var newCardStatus = CardStatusFactory.CreateCardStatus(ACardStatusDTO.CardStatusCode, ACardStatusDTO.OrganizationCode, ACardStatusDTO.DecisionCode,
                ACardStatusDTO.Narration, ACardStatusDTO.CreatedBy, ACardStatusDTO.LastUpdatedBy, ACardStatusDTO.ApprovalStatus);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objCardStatusRepository.Add(newCardStatus);
                        objCardStatusRepository.UnitOfWork.Commit();
                        if (ACardStatusDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_CardStatus", " CardStatusCode = '" + newCardStatus.CardStatusCode + "'",
                                "CRDSTS", ACardStatusDTO.CreatedBy, "/Master/CardStatusAuthorize/" + newCardStatus.CardStatusCode + "/A?", "MT");
                            objCardStatusRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value," +
                                " @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCardStatusDetails(string ACardStatusCode, CardStatusDTO ACardStatusDTO)
        {
            if (ACardStatusDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");
            }
            CardStatus currentCardStatus = objCardStatusRepository.Get(ACardStatusCode, Utils.OrganizationCode);
            CardStatus updatedCardStatus = new CardStatus()
            {
                CardStatusCode = ACardStatusCode,
                OrganizationCode = ACardStatusDTO.OrganizationCode,
                DecisionCode = ACardStatusDTO.DecisionCode,
                Narration = ACardStatusDTO.Narration,
                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = ACardStatusDTO.LastUpdatedBy,
                ApprovalStatus = ACardStatusDTO.ApprovalStatus,
                CreatedOn = currentCardStatus.CreatedOn,
                CreatedBy = currentCardStatus.CreatedBy
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objCardStatusRepository.Merge(currentCardStatus, updatedCardStatus);
                    objCardStatusRepository.UnitOfWork.Commit();
                    if (ACardStatusDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_CardStatus", " CardStatusCode = '" + ACardStatusCode + "'",
                                "CRDSTS", ACardStatusDTO.LastUpdatedBy, "/Master/CardStatusAuthorize/" + ACardStatusCode + "/A?", "MT");
                        objCardStatusRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value," +
                            " @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<CardStatusDTO> FindAll(int APageNo, CardStatusDTO ACardStatusDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "CardStatusCode", FilterValue = ACardStatusDTO.CardStatusCode, Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "DecisionCode", FilterValue = ACardStatusDTO.DecisionCode, Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "Narration", FilterValue = ACardStatusDTO.Narration, Operator = "Contains"},
                new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode, Operator = "Equals"},
                new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
            };

            Expression<Func<CardStatus, bool>> expression = Utils.GetExpression<CardStatus>(lstExp);
            int iRecordCount = objCardStatusRepository.GetRecordCount(expression);
            var vCardStatusDTO = objCardStatusRepository.GetFiltered(0, iRecordCount, expression);

            if (vCardStatusDTO != null && vCardStatusDTO.Any())
            {
                List<CardStatusDTO> lstCardStatusDTO = new List<CardStatusDTO>();
                foreach (var cardstatus in vCardStatusDTO)
                {
                    lstCardStatusDTO.Add(Conversion.Mapping.CardStatusToCardStatusDTO(cardstatus));
                }
                return lstCardStatusDTO;
            }
            else
                return new List<CardStatusDTO>();

        }

        #endregion

        #region General

        #region FindGeneral
        public List<GeneralDTO> FindGeneral(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindGeneral");

            int iRecordCount = objGeneralRepository.GetRecordCount();
            var generalAll = objGeneralRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);
            if (generalAll != null && generalAll.Any())
            {
                List<GeneralDTO> lstGeneralDTO = new List<GeneralDTO>();
                foreach (var general in generalAll)
                {
                    lstGeneralDTO.Add(Conversion.Mapping.GeneralToGeneralDTO(general));
                }
                return lstGeneralDTO;
            }
            else
                return new List<GeneralDTO>();
        }
        #endregion

        #region DeleteGeneral
        public void DeleteGeneral(string AGeneralKeyName)
        {
            var general = objGeneralRepository.Get(AGeneralKeyName);

            if (general != null)
            {
                objGeneralRepository.Remove(general);
                objGeneralRepository.UnitOfWork.Commit();
            }
        }
        #endregion

        #region FindByGeneralKeyName
        public GeneralDTO FindByGeneralKeyName(string AGeneralKeyName, string AOrganizationCode)
        {
            var general = objGeneralRepository.Get(AGeneralKeyName, AOrganizationCode);

            if (general != null)
            {
                objGeneralRepository.UnitOfWork.RefreshDataInModel(ref general);
                return Conversion.Mapping.GeneralToGeneralDTO(general);
            }
            else
            {
                return new GeneralDTO();
            }
        }
        #endregion

        #region SaveGeneralDetails
        public void SaveGeneralDetails(GeneralDTO AGeneralDTO)
        {
            if (AGeneralDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddCurrencyWithNullInformation");

            var newGeneral = GeneralFactory.CreateGeneral(AGeneralDTO.KeyName, AGeneralDTO.Value1,
                AGeneralDTO.Value2, AGeneralDTO.EffectiveFromDate, AGeneralDTO.EffectiveToDate, AGeneralDTO.CreatedBy,
                AGeneralDTO.LastUpdatedBy, AGeneralDTO.ApprovalStatus, AGeneralDTO.OrganizationCode);
            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objGeneralRepository.Add(newGeneral);
                        objGeneralRepository.UnitOfWork.Commit();
                        if (AGeneralDTO.ApprovalStatus == "P")
                        {

                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_General", " KeyName = '" + AGeneralDTO.KeyName + "' and OrganizationCode = '" + AGeneralDTO.OrganizationCode + "'",
                                "GENRL", AGeneralDTO.CreatedBy, //"/Master/GeneralAuthorize/" + AGeneralDTO.KeyName + "/A?", "MT");
                                 "/Master/GeneralAuthorize?AKeyName=" + AGeneralDTO.KeyName + "&AOrganizationCode=" + AGeneralDTO.OrganizationCode + "&", "MT");
                            objGeneralRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);


                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UpdateGeneralDetails
        public void UpdateGeneralDetails(string AGeneralKeyName, string AOrganizationCode, GeneralDTO AGeneralDTO)
        {
            if (AGeneralDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");
            }
            General currentGeneral = objGeneralRepository.Get(AGeneralKeyName, AOrganizationCode);
            General updatedGeneral = new General()
            {
                KeyName = AGeneralKeyName,
                OrganizationCode = AOrganizationCode,
                Value1 = AGeneralDTO.Value1,
                Value2 = AGeneralDTO.Value2,
                EffectiveFromDate = AGeneralDTO.EffectiveFromDate,
                EffectiveToDate = AGeneralDTO.EffectiveToDate,
                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = AGeneralDTO.LastUpdatedBy,
                CreatedOn = currentGeneral.CreatedOn,
                CreatedBy = currentGeneral.CreatedBy,
                ApprovalStatus = AGeneralDTO.ApprovalStatus
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objGeneralRepository.Merge(currentGeneral, updatedGeneral);
                    objGeneralRepository.UnitOfWork.Commit();
                    if (AGeneralDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_General", " KeyName = '" + AGeneralDTO.KeyName + "' and OrganizationCode = '" + AOrganizationCode + "'",
                            "GENRL", AGeneralDTO.LastUpdatedBy, //"/Master/GeneralAuthorize/" + AGeneralDTO.KeyName + "/A?", "MT");
                             "/Master/GeneralAuthorize?AKeyName=" + AGeneralDTO.KeyName + "&AOrganizationCode=" + AOrganizationCode + "&", "MT");
                        objGeneralRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);


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
        public List<GeneralDTO> FindAll(int APageNo, GeneralDTO AGeneralDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "KeyName", FilterValue = AGeneralDTO.KeyName, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "Value1", FilterValue = AGeneralDTO.Value1, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "Value2", FilterValue = AGeneralDTO.Value2, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "OrganizationCode", FilterValue = Utils.OrganizationCode , Operator = "Equals"}
            };

            Expression<Func<General, bool>> expression = Utils.GetExpression<General>(lstExp);

            int iRecordCount = objGeneralRepository.GetRecordCount(expression);

            //var vGeneralDTO = objGeneralRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vGeneralDTO = objGeneralRepository.GetFiltered(0, iRecordCount, expression);

            if (vGeneralDTO != null && vGeneralDTO.Any())
            {
                List<GeneralDTO> lstGeneralDTO = new List<GeneralDTO>();
                foreach (var general in vGeneralDTO)
                {
                    lstGeneralDTO.Add(Conversion.Mapping.GeneralToGeneralDTO(general));
                }
                return lstGeneralDTO;
            }
            else
                return new List<GeneralDTO>();
        }
        #endregion

        #endregion

        #region Contract

        #region SaveContractDetails
        public void SaveContractDetails(ContractDTO AContractDTO)
        {
            if (AContractDTO == null)
            {
                throw new ArgumentException("");
            }

            var newContract = ContractFactory.CreateContract(AContractDTO.ContractCode, AContractDTO.ContractDescription,
                AContractDTO.ContractTemplateCode, AContractDTO.EffectiveFromDate, AContractDTO.EffectiveToDate,//AContractDTO.ProductCode, 
                AContractDTO.NotaryName, AContractDTO.NotarySignDate, AContractDTO.Repertoire, AContractDTO.CreatedBy, AContractDTO.LastUpdatedBy, AContractDTO.ApprovalStatus, AContractDTO.OrganizationCode, null);

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                        new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objContractRepository.Add(newContract);
                    objContractRepository.UnitOfWork.Commit();

                    if (AContractDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_Contract", " ContractCode = '" + AContractDTO.ContractCode + "' and OrganizationCode = '" + AContractDTO.OrganizationCode + "'",
                            "CNT", AContractDTO.LastUpdatedBy, //"/Master/ContractAuthorize/" + AContractDTO.ContractCode + "/A?", "MT");
                      "/Master/ContractAuthorize?AContractCode=" + AContractDTO.ContractCode + "&AOrganizationCode=" + AContractDTO.OrganizationCode + "&", "MT");
                        objContractRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #endregion
        #region UpdateContractDetails
        public void UpdateContractDetails(string AContractCode, string AOrganizationCode, ContractDTO AContractDTO)
        {
            if (AContractDTO == null)
            {
                throw new ArgumentException("");
            }

            Contract currentContract = objContractRepository.Get(AContractCode, AOrganizationCode);
            Contract updatedContract = new Contract()
            {
                ContractCode = AContractCode,
                OrganizationCode = AOrganizationCode,
                ContractDescription = AContractDTO.ContractDescription,
                //ProductCode = AContractDTO.ProductCode,
                ContractTemplateCode = AContractDTO.ContractTemplateCode,
                EffectiveFromDate = AContractDTO.EffectiveFromDate,
                EffectiveToDate = AContractDTO.EffectiveToDate,
                NotaryName = AContractDTO.NotaryName,
                NotarySignDate = AContractDTO.NotarySignDate,
                Repertoire = AContractDTO.Repertoire,
                ApprovalStatus = AContractDTO.ApprovalStatus,
                CreatedBy = currentContract.CreatedBy,
                CreatedOn = currentContract.CreatedOn,
                LastUpdatedBy = AContractDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };


            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objContractRepository.Merge(currentContract, updatedContract);
                    objContractRepository.UnitOfWork.Commit();

                    if (AContractDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_Contract", " ContractCode = '" + AContractDTO.ContractCode + "' and OrganizationCode = '" + AOrganizationCode + "'",
                            "CNT", AContractDTO.LastUpdatedBy, //"/Master/ContractAuthorize/" + AContractDTO.ContractCode + "/A?", "MT");
                      "/Master/ContractAuthorize?AContractCode=" + AContractDTO.ContractCode + "&AOrganizationCode=" + AOrganizationCode + "&", "MT");

                        objContractRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #region DeleteContract
        public void DeleteContract(string AContractCode)
        {
            var contract = objContractRepository.Get(AContractCode);
            if (contract != null)
            {
                objContractRepository.Remove(contract);
                objContractRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindContract
        public List<ContractDTO> FindContract(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objContractRepository.GetRecordCount();
            var contract = objContractRepository.GetAll();
            if (contract != null && contract.Any())
            {
                List<ContractDTO> lstContractDTO = new List<ContractDTO>();
                foreach (var item in contract)
                {
                    lstContractDTO.Add(Conversion.Mapping.ContractToContractDTO(item));
                }
                return lstContractDTO;
            }
            else
                return new List<ContractDTO>();
        }
        #endregion
        #region FindAll
        public List<ContractDTO> FindAll(int APageNo, ContractDTO AContractDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "ContractCode",FilterValue = AContractDTO.ContractCode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "ContractDescription",FilterValue = AContractDTO.ContractDescription,Operator = "Contains"},
                       new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils .OrganizationCode ,  Operator = "Equals" }
                    };

            Expression<Func<Contract, bool>> expression = Utils.GetExpression<Contract>(lstExpression);
            int iRecordCount = objContractRepository.GetRecordCount(expression);

            //var vContractDTO = objContractRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vContractDTO = objContractRepository.GetFiltered(0, iRecordCount, expression);

            if (vContractDTO != null && vContractDTO.Any())
            {
                List<ContractDTO> lstContractDTO = new List<ContractDTO>();
                foreach (var item in vContractDTO)
                {
                    lstContractDTO.Add(Conversion.Mapping.ContractToContractDTO(item));
                }
                return lstContractDTO;
            }
            else
            {
                return new List<ContractDTO>(); ;
            }
        }
        #endregion
        #region FindByContractCode
        public ContractDTO FindByContractCode(string AContractCode, string AOrganizationCode)
        {
            Contract contract = objContractRepository.Get(AContractCode, AOrganizationCode);
            if (contract != null)
            {
                objContractRepository.UnitOfWork.RefreshDataInModel(ref contract);
                //if (contract.Product == null)
                //{
                //    contract.Product = objProductRepository.Get(AOrganizationCode , contract.ProductCode);
                //}
                if (contract.ContractTemplate == null)
                {
                    contract.ContractTemplate = objContractTemplateRepository.Get(contract.ContractTemplateCode, contract.OrganizationCode);
                }

                return Conversion.Mapping.ContractToContractDTO(contract);
            }
            else
            {
                return new ContractDTO();
            }
        }
        #endregion
        #endregion

        #region Product Type

        #region SaveProductTypeDetails
        public void SaveProductTypeDetails(ProductTypeDTO AProductTypeDTO)
        {
            if (AProductTypeDTO == null)
            {
                throw new ArgumentException("");
            }

            var newProductType = ProductTypeFactory.CreateProductType(AProductTypeDTO.ProductTypeCode, AProductTypeDTO.Description,
                                            AProductTypeDTO.CreatedBy, AProductTypeDTO.LastUpdatedBy, AProductTypeDTO.ApprovalStatus, AProductTypeDTO.OrganizationCode, null);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                        new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objProductTypeRepository.Add(newProductType);
                        objProductTypeRepository.UnitOfWork.Commit();
                        if (AProductTypeDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_ProductType", " ProductTypeCode = '" + AProductTypeDTO.ProductTypeCode +
                            "' and OrganizationCode = '" + AProductTypeDTO.OrganizationCode + "'",
                                    "PDTTYPE", AProductTypeDTO.LastUpdatedBy, //"/Master/ProductTypeAuthorize/" + AProductTypeDTO.ProductTypeCode + "/A?", "MT");
                                            "/Master/ProductTypeAuthorize?AProductTypeCode=" + AProductTypeDTO.ProductTypeCode + "&AOrganizationCode=" + AProductTypeDTO.OrganizationCode + "&", "MT");
                            objProductTypeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
        #region UpdateProductTypeDetails
        public void UpdateProductTypeDetails(string AProductTypeCode, string AOrganizationCode, ProductTypeDTO AProductTypeDTO)
        {
            if (AProductTypeDTO == null)
            {
                throw new ArgumentException("");
            }

            ProductType currentProductType = objProductTypeRepository.Get(AProductTypeCode, AOrganizationCode);
            ProductType updatedProductType = new ProductType()
            {
                ProductTypeCode = AProductTypeCode,
                OrganizationCode = AOrganizationCode,
                Description = AProductTypeDTO.Description,
                ApprovalStatus = AProductTypeDTO.ApprovalStatus,
                CreatedBy = currentProductType.CreatedBy,
                CreatedOn = currentProductType.CreatedOn,
                LastUpdatedBy = AProductTypeDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objProductTypeRepository.Merge(currentProductType, updatedProductType);
                    objProductTypeRepository.UnitOfWork.Commit();
                    if (AProductTypeDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_ProductType", " ProductTypeCode = '" + AProductTypeDTO.ProductTypeCode +
                        "' and OrganizationCode = '" + AOrganizationCode + "'",
                                "PDTTYPE", AProductTypeDTO.LastUpdatedBy, //"/Master/ProductTypeAuthorize/" + AProductTypeDTO.ProductTypeCode + "/A?", "MT");
                                        "/Master/ProductTypeAuthorize?AProductTypeCode=" + AProductTypeDTO.ProductTypeCode + "&AOrganizationCode=" + AOrganizationCode + "&", "MT");
                        objProductTypeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);

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
        #region DeleteProductType
        public void DeleteProductType(string AProductTypeCode)
        {
            var productType = objProductTypeRepository.Get(AProductTypeCode);
            if (productType != null)
            {
                objProductTypeRepository.Remove(productType);
                objProductTypeRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindProductType
        public List<ProductTypeDTO> FindProductType(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objProductTypeRepository.GetRecordCount();
            var productType = objProductTypeRepository.GetAll();
            if (productType != null && productType.Any())
            {
                List<ProductTypeDTO> lstProductTypeDTO = new List<ProductTypeDTO>();
                foreach (var item in productType)
                {
                    lstProductTypeDTO.Add(Conversion.Mapping.ProductTypeToProductTypeDTO(item));
                }
                return lstProductTypeDTO;
            }
            else
                return new List<ProductTypeDTO>();
        }
        #endregion
        #region FindAll
        public List<ProductTypeDTO> FindAll(int APageNo, ProductTypeDTO AProductTypeDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "ProductTypeCode",FilterValue = AProductTypeDTO.ProductTypeCode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "Description",FilterValue = AProductTypeDTO.Description,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils .OrganizationCode ,Operator = "Equals"}
                    };

            Expression<Func<ProductType, bool>> expression = Utils.GetExpression<ProductType>(lstExpression);
            int iRecordCount = objProductTypeRepository.GetRecordCount(expression);

            //var vProductTypeDTO = objProductTypeRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vProductTypeDTO = objProductTypeRepository.GetFiltered(0, iRecordCount, expression);

            if (vProductTypeDTO != null && vProductTypeDTO.Any())
            {
                List<ProductTypeDTO> lstProductTypeDTO = new List<ProductTypeDTO>();
                foreach (var item in vProductTypeDTO)
                {
                    lstProductTypeDTO.Add(Conversion.Mapping.ProductTypeToProductTypeDTO(item));
                }
                return lstProductTypeDTO;
            }
            else
            {
                return new List<ProductTypeDTO>();
            }
        }
        #endregion
        #region FindByProductTypeCode
        public ProductTypeDTO FindByProductTypeCode(string AProductTypeCode, string AOrganizationCode)
        {
            ProductType productType = objProductTypeRepository.Get(AProductTypeCode, AOrganizationCode);
            if (productType != null)
            {
                objProductTypeRepository.UnitOfWork.RefreshDataInModel(ref productType);
                return Conversion.Mapping.ProductTypeToProductTypeDTO(productType);
            }
            else
            {
                return new ProductTypeDTO();
            }
        }
        #endregion
        #endregion

        #region Transaction

        #region SaveTransactionDetails

        public void SaveTransactionDetails(TransactionDTO ATransactionDTO)
        {
            if (ATransactionDTO == null)
            {
                throw new ArgumentException("");
            }

            var newTransaction = TransactionFactory.CreateTransaction(ATransactionDTO.TransactionCode, ATransactionDTO.Description,
                                   ATransactionDTO.OrganizationCode, ATransactionDTO.CreditSegment, ATransactionDTO.InstallmentRequired,
                                   ATransactionDTO.InterestWith, ATransactionDTO.DeferredPayment, ATransactionDTO.FeesRequired,
                                   ATransactionDTO.TaxesRequired, ATransactionDTO.InstallmentRelated, ATransactionDTO.CapitalizeInterest,
                                   ATransactionDTO.CreatedBy, ATransactionDTO.LastUpdatedBy, ATransactionDTO.ApprovalStatus, null);

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objTransactionRepository.Add(newTransaction);
                    objTransactionRepository.UnitOfWork.Commit();
                    if (ATransactionDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_Transaction", " TransactionCode = '" + ATransactionDTO.TransactionCode + "'",
                            "TRANSCTN", ATransactionDTO.CreatedBy, "/Master/TransactionAuthorize/" + ATransactionDTO.TransactionCode + "/A?", "MT");
                        objTransactionRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
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
        #region UpdateTransactionDetails

        public void UpdateTransactionDetails(string ATransactionCode, TransactionDTO ATransactionDTO)
        {
            if (ATransactionDTO == null)
            {
                throw new ArgumentException("");
            }

            CC_Ingenio.Core.MasterRepo.MasterAggregate.Transaction currentTransaction = objTransactionRepository.Get(Utils.OrganizationCode, ATransactionCode);
            CC_Ingenio.Core.MasterRepo.MasterAggregate.Transaction updatedTransaction = new CC_Ingenio.Core.MasterRepo.MasterAggregate.Transaction()
            {
                TransactionCode = ATransactionCode,
                Description = ATransactionDTO.Description,
                OrganizationCode = currentTransaction.OrganizationCode,
                CreditSegment = ATransactionDTO.CreditSegment,
                InstallmentRequired = ATransactionDTO.InstallmentRequired,
                InterestWith = ATransactionDTO.InterestWith,
                DeferredPayment = ATransactionDTO.DeferredPayment,
                FeesRequired = ATransactionDTO.FeesRequired,
                TaxesRequired = ATransactionDTO.TaxesRequired,
                InstallmentRelated = ATransactionDTO.InstallmentRelated,
                CapitalizeInterest = ATransactionDTO.CapitalizeInterest,

                ApprovalStatus = ATransactionDTO.ApprovalStatus,
                CreatedBy = currentTransaction.CreatedBy,
                CreatedOn = currentTransaction.CreatedOn,
                LastUpdatedBy = ATransactionDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objTransactionRepository.Merge(currentTransaction, updatedTransaction);
                    objTransactionRepository.UnitOfWork.Commit();
                    if (ATransactionDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_Transaction", " TransactionCode = '" + ATransactionDTO.TransactionCode + "'",
                            "TRANSCTN", ATransactionDTO.LastUpdatedBy, "/Master/TransactionAuthorize/" + ATransactionDTO.TransactionCode + "/A?", "MT");
                        objTransactionRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
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
        #region DeleteTransaction
        public void DeleteTransaction(string ATransactionCode)
        {
            var transaction = objTransactionRepository.Get(Utils.OrganizationCode, ATransactionCode);
            if (transaction != null)
            {
                objTransactionRepository.Remove(transaction);
                objTransactionRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindTransaction
        public List<TransactionDTO> FindTransaction(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objTransactionRepository.GetRecordCount();
            var productType = objTransactionRepository.GetAll();

            objTransactionRepository.UnitOfWork.ReloadAllDataInContext<CC_Ingenio.Core.MasterRepo.MasterAggregate.Transaction>();

            if (productType != null && productType.Any())
            {
                List<TransactionDTO> lstTransactionDTO = new List<TransactionDTO>();
                foreach (var item in productType)
                {
                    lstTransactionDTO.Add(Conversion.Mapping.TransactionToTransactionDTO(item));
                }
                return lstTransactionDTO;
            }
            else
                return new List<TransactionDTO>();
        }
        #endregion
        #region FindAll
        public List<TransactionDTO> FindAll(int APageNo, TransactionDTO ATransactionDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "TransactionCode",FilterValue = ATransactionDTO.TransactionCode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "Description",FilterValue = ATransactionDTO.Description,Operator = "Contains"}, 
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode, Operator = "Contains"}
                    };

            Expression<Func<CC_Ingenio.Core.MasterRepo.MasterAggregate.Transaction, bool>> expression = Utils.GetExpression<CC_Ingenio.Core.MasterRepo.MasterAggregate.Transaction>(lstExpression);
            int iRecordCount = objTransactionRepository.GetRecordCount(expression);

            //var vTransactionDTO = objTransactionRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vTransactionDTO = objTransactionRepository.GetFiltered(0, iRecordCount, expression);

            if (vTransactionDTO != null && vTransactionDTO.Any())
            {
                List<TransactionDTO> lstTransactionDTO = new List<TransactionDTO>();
                foreach (var item in vTransactionDTO)
                {
                    lstTransactionDTO.Add(Conversion.Mapping.TransactionToTransactionDTO(item));
                }
                return lstTransactionDTO;
            }
            else
            {
                return new List<TransactionDTO>();
            }
        }
        #endregion
        #region FindByTransactionCode
        public TransactionDTO FindByTransactionCode(string ATransactionCode)
        {
            CC_Ingenio.Core.MasterRepo.MasterAggregate.Transaction transaction = objTransactionRepository.Get(Utils.OrganizationCode, ATransactionCode);
            if (transaction != null)
            {
                objTransactionRepository.UnitOfWork.RefreshDataInModel(ref transaction);
                return Conversion.Mapping.TransactionToTransactionDTO(transaction);
            }
            else
            {
                return new TransactionDTO();
            }
        }
        #endregion
        #endregion

        #region System Maintenance Configuration

        #region SaveSystemMaintenanceConfigurationDetails
        public void SaveSystemMaintenanceConfigurationDetails(SystemMaintenanceConfigurationDTO ASystemMaintenanceConfigurationDTO)
        {
            if (ASystemMaintenanceConfigurationDTO == null)
            {
                throw new ArgumentException("");
            }

            var newSystemMaintenanceConfiguration = SystemMaintenanceConfigurationFactory.CreateSystemMaintenanceConfiguration(ASystemMaintenanceConfigurationDTO.BaseCurrency, ASystemMaintenanceConfigurationDTO.WeeklyGlobalCount,
                                   ASystemMaintenanceConfigurationDTO.WeeklySetUp, ASystemMaintenanceConfigurationDTO.CashLimit, ASystemMaintenanceConfigurationDTO.CreditLimit,
                                   ASystemMaintenanceConfigurationDTO.DefaultBoost, ASystemMaintenanceConfigurationDTO.ReissueDamaged, ASystemMaintenanceConfigurationDTO.ClosedDamaged,
                                   ASystemMaintenanceConfigurationDTO.DefaultMerchantChargeType, ASystemMaintenanceConfigurationDTO.DefaultChargeTypeDescrption,
                                   ASystemMaintenanceConfigurationDTO.MinimumDueMerchantCode, ASystemMaintenanceConfigurationDTO.NumberOfSegments,
                                   ASystemMaintenanceConfigurationDTO.CreatedBy, ASystemMaintenanceConfigurationDTO.LastUpdatedBy, ASystemMaintenanceConfigurationDTO.ApprovalStatus, null);

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
              new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objSystemMaintenanceConfigurationRepository.Add(newSystemMaintenanceConfiguration);
                    objSystemMaintenanceConfigurationRepository.UnitOfWork.Commit();

                    if (ASystemMaintenanceConfigurationDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_SystemMaintenanceConfiguration", " BaseCurrency = '" + ASystemMaintenanceConfigurationDTO.BaseCurrency + "'",
                            "MTNCNFG", ASystemMaintenanceConfigurationDTO.LastUpdatedBy, "/Master/SystemMaintenanceConfigurationAuthorize/" + ASystemMaintenanceConfigurationDTO.BaseCurrency + "/A?", "MT");
                        objSystemMaintenanceConfigurationRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType", paramCollection);
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
        #region UpdateSystemMaintenanceConfigurationDetails
        public void UpdateSystemMaintenanceConfigurationDetails(string ABaseCurrency, SystemMaintenanceConfigurationDTO ASystemMaintenanceConfigurationDTO)
        {
            if (ASystemMaintenanceConfigurationDTO == null)
            {
                throw new ArgumentException("");
            }

            CC_Ingenio.Core.MasterRepo.MasterAggregate.SystemMaintenanceConfiguration currentSystemMaintenanceConfiguration = objSystemMaintenanceConfigurationRepository.Get(ABaseCurrency);
            CC_Ingenio.Core.MasterRepo.MasterAggregate.SystemMaintenanceConfiguration updatedSystemMaintenanceConfiguration = new CC_Ingenio.Core.MasterRepo.MasterAggregate.SystemMaintenanceConfiguration()
            {
                BaseCurrency = ABaseCurrency,

                ApprovalStatus = ASystemMaintenanceConfigurationDTO.ApprovalStatus,
                CreatedBy = currentSystemMaintenanceConfiguration.CreatedBy,
                CreatedOn = currentSystemMaintenanceConfiguration.CreatedOn,
                LastUpdatedBy = ASystemMaintenanceConfigurationDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
             new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objSystemMaintenanceConfigurationRepository.Merge(currentSystemMaintenanceConfiguration, updatedSystemMaintenanceConfiguration);
                    objSystemMaintenanceConfigurationRepository.UnitOfWork.Commit();

                    if (ASystemMaintenanceConfigurationDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_SystemMaintenanceConfiguration", " BaseCurrency = '" + ASystemMaintenanceConfigurationDTO.BaseCurrency + "'",
                            "MTNCNFG", ASystemMaintenanceConfigurationDTO.LastUpdatedBy, "/Master/SystemMaintenanceConfigurationAuthorize/" + ASystemMaintenanceConfigurationDTO.BaseCurrency + "/A?", "MT");
                        objSystemMaintenanceConfigurationRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType", paramCollection);
                    }

                    //    SqlParameter[] paramCollection = new SqlParameter[]{ 
                    //    new SqlParameter(parameterName: "@P_TableName",dbType:System.Data.SqlDbType.VarChar ),
                    //    new SqlParameter(parameterName: "@P_ColumnName", dbType:System.Data.SqlDbType.VarChar ),
                    //    new SqlParameter(parameterName: "@P_Value", dbType:System.Data.SqlDbType.VarChar ),
                    //    new SqlParameter(parameterName: "@P_WhereClause",dbType:System.Data.SqlDbType.VarChar ),
                    //    new SqlParameter(parameterName: "@P_ContextCode",dbType:System.Data.SqlDbType.VarChar ),
                    //    new SqlParameter(parameterName: "@P_LoginName",dbType:System.Data.SqlDbType.VarChar ),
                    //    new SqlParameter(parameterName: "@P_ReturnUrl",dbType:System.Data.SqlDbType.VarChar ),
                    //    new SqlParameter(parameterName: "@P_TransactionType",dbType:System.Data.SqlDbType.VarChar ),
                    //};

                    //    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TableName")).First()).Value = "MT_SystemMaintenanceConfiguration";
                    //    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ColumnName")).First()).Value = "ApprovalStatus";
                    //    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Value")).First()).Value = "P";
                    //    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_WhereClause")).First()).Value = " BaseCurrency = '" + ASystemMaintenanceConfigurationDTO.BaseCurrency + "'";
                    //    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ContextCode")).First()).Value = "SYSMTNCNFIG";
                    //    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_LoginName")).First()).Value = ASystemMaintenanceConfigurationDTO.LastUpdatedBy;
                    //    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ReturnUrl")).First()).Value = "/Master/SystemMaintenanceConfigurationAuthorize/" + ASystemMaintenanceConfigurationDTO.BaseCurrency + "/A?";
                    //    ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TransactionType")).First()).Value = "MT";
                    //    objSystemMaintenanceConfigurationRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType", paramCollection);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion
        #region DeleteSystemMaintenanceConfiguration
        public void DeleteSystemMaintenanceConfiguration(string ABaseCurrency)
        {
            var systemMaintenanceConfiguration = objSystemMaintenanceConfigurationRepository.Get(ABaseCurrency);
            if (systemMaintenanceConfiguration != null)
            {
                objSystemMaintenanceConfigurationRepository.Remove(systemMaintenanceConfiguration);
                objSystemMaintenanceConfigurationRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindSystemMaintenanceConfiguration
        public List<SystemMaintenanceConfigurationDTO> FindSystemMaintenanceConfiguration(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objSystemMaintenanceConfigurationRepository.GetRecordCount();
            var productType = objSystemMaintenanceConfigurationRepository.GetAll();

            objSystemMaintenanceConfigurationRepository.UnitOfWork.ReloadAllDataInContext<CC_Ingenio.Core.MasterRepo.MasterAggregate.SystemMaintenanceConfiguration>();

            if (productType != null && productType.Any())
            {
                List<SystemMaintenanceConfigurationDTO> lstSystemMaintenanceConfigurationDTO = new List<SystemMaintenanceConfigurationDTO>();
                foreach (var item in productType)
                {
                    lstSystemMaintenanceConfigurationDTO.Add(Conversion.Mapping.SystemMaintenanceConfigurationToSystemMaintenanceConfigurationDTO(item));
                }
                return lstSystemMaintenanceConfigurationDTO;
            }
            else
                return new List<SystemMaintenanceConfigurationDTO>();
        }
        #endregion
        #region FindAll
        public List<SystemMaintenanceConfigurationDTO> FindAll(int APageNo, SystemMaintenanceConfigurationDTO ASystemMaintenanceConfigurationDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "BaseCurrency",FilterValue = ASystemMaintenanceConfigurationDTO.BaseCurrency, Operator = "Contains" },
                      
                    };

            Expression<Func<CC_Ingenio.Core.MasterRepo.MasterAggregate.SystemMaintenanceConfiguration, bool>> expression = Utils.GetExpression<CC_Ingenio.Core.MasterRepo.MasterAggregate.SystemMaintenanceConfiguration>(lstExpression);
            int iRecordCount = objSystemMaintenanceConfigurationRepository.GetRecordCount(expression);

            //var vTransactionDTO = objTransactionRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vSystemMaintenanceConfigurationDTO = objSystemMaintenanceConfigurationRepository.GetFiltered(0, iRecordCount, expression);

            if (vSystemMaintenanceConfigurationDTO != null && vSystemMaintenanceConfigurationDTO.Any())
            {
                List<SystemMaintenanceConfigurationDTO> lstSystemMaintenanceConfigurationDTO = new List<SystemMaintenanceConfigurationDTO>();
                foreach (var item in vSystemMaintenanceConfigurationDTO)
                {
                    lstSystemMaintenanceConfigurationDTO.Add(Conversion.Mapping.SystemMaintenanceConfigurationToSystemMaintenanceConfigurationDTO(item));
                }
                return lstSystemMaintenanceConfigurationDTO;
            }
            else
            {
                return new List<SystemMaintenanceConfigurationDTO>();
            }
        }
        #endregion
        #region FindByBaseCurrency
        public SystemMaintenanceConfigurationDTO FindByBaseCurrency(string ABaseCurrency)
        {
            CC_Ingenio.Core.MasterRepo.MasterAggregate.SystemMaintenanceConfiguration systemMaintenanceConfiguration = objSystemMaintenanceConfigurationRepository.Get(ABaseCurrency);
            if (systemMaintenanceConfiguration != null)
            {
                objSystemMaintenanceConfigurationRepository.UnitOfWork.RefreshDataInModel(ref systemMaintenanceConfiguration);
                return Conversion.Mapping.SystemMaintenanceConfigurationToSystemMaintenanceConfigurationDTO(systemMaintenanceConfiguration);
            }
            else
            {
                return new SystemMaintenanceConfigurationDTO();
            }
        }
        #endregion
        #endregion

        #region BoostMaintenance

        #region SaveBoostMaintenanceDetails
        public void SaveBoostMaintenanceDetails(BoostMaintenanceDTO ABoostMaintenanceDTO)
        {
            if (ABoostMaintenanceDTO == null)
            {
                throw new ArgumentException("");
            }

            var newBoostMaintenance = BoostMaintenanceFactory.CreateBoostMaintenance(ABoostMaintenanceDTO.BoostId, ABoostMaintenanceDTO.OrganizationCode, ABoostMaintenanceDTO.ProductCode,
                                                ABoostMaintenanceDTO.TransactionCode, ABoostMaintenanceDTO.Percentage, ABoostMaintenanceDTO.CreatedBy,
                                                ABoostMaintenanceDTO.LastUpdatedBy, ABoostMaintenanceDTO.ApprovalStatus, null);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objBoostMaintenanceRepository.Add(newBoostMaintenance);
                        objBoostMaintenanceRepository.UnitOfWork.Commit();
                        if (ABoostMaintenanceDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_BoostMaintenance", " BoostId = '" + newBoostMaintenance.BoostId + "'",
                                "BOOST", ABoostMaintenanceDTO.CreatedBy, "/Master/BoostMaintenanceAuthorize/" + newBoostMaintenance.BoostId + "/A?", "MT");

                            objBoostMaintenanceRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName," +
                                " @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
        #region UpdateBoostMaintenanceDetails
        public void UpdateBoostMaintenanceDetails(int ABoostId, BoostMaintenanceDTO ABoostMaintenanceDTO)
        {
            if (ABoostMaintenanceDTO == null)
            {
                throw new ArgumentException("");
            }

            BoostMaintenance currentBoostMaintenance = objBoostMaintenanceRepository.Get(ABoostId);
            BoostMaintenance updatedBoostMaintenance = new BoostMaintenance()
            {
                BoostId = ABoostId,
                OrganizationCode = ABoostMaintenanceDTO.OrganizationCode,
                ProductCode = ABoostMaintenanceDTO.ProductCode,
                TransactionCode = ABoostMaintenanceDTO.TransactionCode,
                Percentage = ABoostMaintenanceDTO.Percentage,
                ApprovalStatus = ABoostMaintenanceDTO.ApprovalStatus,
                CreatedBy = currentBoostMaintenance.CreatedBy,
                CreatedOn = currentBoostMaintenance.CreatedOn,
                LastUpdatedBy = ABoostMaintenanceDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objBoostMaintenanceRepository.Merge(currentBoostMaintenance, updatedBoostMaintenance);
                    objBoostMaintenanceRepository.UnitOfWork.Commit();
                    if (ABoostMaintenanceDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_BoostMaintenance", " BoostId = '" + ABoostId + "'",
                                "BOOST", ABoostMaintenanceDTO.LastUpdatedBy, "/Master/BoostMaintenanceAuthorize/" + ABoostId + "/A?", "MT");
                        objBoostMaintenanceRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName," +
                            " @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #region DeleteBoostMaintenance
        public void DeleteBoostMaintenance(int ABoostId)
        {
            var boostMaintenance = objBoostMaintenanceRepository.Get(ABoostId);
            if (boostMaintenance != null)
            {
                objBoostMaintenanceRepository.Remove(boostMaintenance);
                objBoostMaintenanceRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindBoostMaintenance
        public List<BoostMaintenanceDTO> FindBoostMaintenance(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objBoostMaintenanceRepository.GetRecordCount();
            var boostMaintenance = objBoostMaintenanceRepository.GetAll();
            if (boostMaintenance != null && boostMaintenance.Any())
            {
                List<BoostMaintenanceDTO> lstBoostMaintenanceDTO = new List<BoostMaintenanceDTO>();
                foreach (var item in boostMaintenance)
                {
                    lstBoostMaintenanceDTO.Add(Conversion.Mapping.BoostMaintenanceToBoostMaintenanceDTO(item));
                }
                return lstBoostMaintenanceDTO;
            }
            else
                return new List<BoostMaintenanceDTO>();
        }
        #endregion
        #region FindAll
        public List<BoostMaintenanceDTO> FindAll(int APageNo, BoostMaintenanceDTO ABoostMaintenanceDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { 
                      new Utils.ExpressionList() { FilterColumn = "TransactionCode",FilterValue = ABoostMaintenanceDTO.TransactionCode,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "ProductCode", FilterValue = ABoostMaintenanceDTO.ProductCode,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode, Operator = "Equals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
                    };

            Expression<Func<CC_Ingenio.Core.MasterRepo.MasterAggregate.BoostMaintenance, bool>> expression = Utils.GetExpression<BoostMaintenance>(lstExpression);
            int iRecordCount = objBoostMaintenanceRepository.GetRecordCount(expression);

            var vTransactionDTO = objBoostMaintenanceRepository.GetFiltered(0, iRecordCount, expression);

            if (vTransactionDTO != null && vTransactionDTO.Any())
            {
                List<BoostMaintenanceDTO> lstBoostMaintenanceDTO = new List<BoostMaintenanceDTO>();
                foreach (var item in vTransactionDTO)
                {
                    lstBoostMaintenanceDTO.Add(Conversion.Mapping.BoostMaintenanceToBoostMaintenanceDTO(item));
                }
                return lstBoostMaintenanceDTO;
            }
            else
            {
                return new List<BoostMaintenanceDTO>();
            }
        }
        #endregion
        #region FindByBoostId
        public BoostMaintenanceDTO FindByBoostId(int ABoostId)
        {
            BoostMaintenance boostMaintenance = objBoostMaintenanceRepository.Get(ABoostId);
            if (boostMaintenance != null)
            {
                if (boostMaintenance.Product == null)
                {
                    boostMaintenance.Product = objProductRepository.Get(boostMaintenance.OrganizationCode, boostMaintenance.ProductCode);
                }
                if (boostMaintenance.Transaction == null)
                {
                    boostMaintenance.Transaction = objTransactionRepository.Get(boostMaintenance.OrganizationCode, boostMaintenance.TransactionCode);
                }

                objBoostMaintenanceRepository.UnitOfWork.RefreshDataInModel(ref boostMaintenance);
                return Conversion.Mapping.BoostMaintenanceToBoostMaintenanceDTO(boostMaintenance);
            }
            else
            {
                return new BoostMaintenanceDTO();
            }
        }
        #endregion
        #endregion

        #region Fee

        #region SaveFeeDetails
        public void SaveFeeDetails(FeeDTO AFeeDTO)
        {
            if (AFeeDTO == null)
            {
                throw new ArgumentException("");
            }

            var newFee = FeeFactory.CreateFee(AFeeDTO.FeeCode, AFeeDTO.ProductCode, AFeeDTO.ProductMemberTypeCode, AFeeDTO.CustomerSegmentCode,
                AFeeDTO.CustomerSubSegmentCode, AFeeDTO.ContractCode, AFeeDTO.EffectiveFromDate, AFeeDTO.EffectiveToDate, AFeeDTO.CurrencyCode,
                AFeeDTO.CreatedBy, AFeeDTO.LastUpdatedBy, AFeeDTO.ApprovalStatus, AFeeDTO.OrganizationCode);

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
            new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objFeeRepository.Add(newFee);
                    objFeeRepository.UnitOfWork.Commit();

                    #region Fee Type

                    AFeeDTO.FeeTypeDTO.ForEach(x => objFeeTypeRepository.Add(
                        FeeTypeFactory.CreateFeeType(newFee.FeeCode, x.FeeTypeCode, x.FeeAmount, x.MonthsFrom, x.ReasonCode, x.FirstYearAmount,
                               x.RenwalFee1, x.RenwalFee2, x.RenwalFee3, x.RenwalFee4, x.RenwalFee5, x.RenwalFee6,
                               x.AdjustCode, x.Period, x.DiscountRatePercent, x.FlatFee, x.MinFee, x.Flag, Utils.OrganizationCode)));
                    objFeeTypeRepository.UnitOfWork.Commit();

                    #endregion

                    if (AFeeDTO.ApprovalStatus == "P")
                    {

                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_Fee", " FeeCode = '" + AFeeDTO.FeeCode + "' and OrganizationCode = '" + AFeeDTO.OrganizationCode + "'",
                            "FEE", AFeeDTO.LastUpdatedBy,// "/Master/FeeAuthorize?AFeeCode=" + AFeeDTO.FeeCode + "&", "MT");
                             "/Master/FeeAuthorize?AFeeCode=" + AFeeDTO.FeeCode + "&AOrganizationCode=" + AFeeDTO.OrganizationCode + "&", "MT");
                        objFeeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);


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
        #region UpdateFeeDetails
        public void UpdateFeeDetails(string AFeeCode, string AOrganizationCode, FeeDTO AFeeDTO)
        {
            if (AFeeDTO == null)
            {
                throw new ArgumentException("");
            }

            Fee currentFee = objFeeRepository.Get(AFeeCode, Utils.OrganizationCode);
            Fee updatedFee = new Fee()
            {
                FeeCode = AFeeCode,
                OrganizationCode = AOrganizationCode,
                ProductCode = AFeeDTO.ProductCode,
                ProductMemberTypeCode = AFeeDTO.ProductMemberTypeCode,
                CustomerSegmentCode = AFeeDTO.CustomerSegmentCode,
                CustomerSubSegmentCode = AFeeDTO.CustomerSubSegmentCode,
                ContractCode = AFeeDTO.ContractCode,
                EffectiveFromDate = AFeeDTO.EffectiveFromDate,
                EffectiveToDate = AFeeDTO.EffectiveToDate,
                CurrencyCode = AFeeDTO.CurrencyCode,

                ApprovalStatus = AFeeDTO.ApprovalStatus,
                CreatedBy = currentFee.CreatedBy,
                CreatedOn = currentFee.CreatedOn,
                LastUpdatedBy = AFeeDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };

            #region Update Fee type - child collection
            List<FeeType> lstFeeTypeUpdated = new List<FeeType>();
            foreach (var item in AFeeDTO.FeeTypeDTO)
            {
                var currentFeeType = objFeeTypeRepository.Get(item.FeeCode, item.FeeTypeCode, Utils.OrganizationCode);
                var updatedFeeType = new FeeType()
                {
                    FeeCode = AFeeCode,
                    FeeTypeCode = item.FeeTypeCode,
                    OrganizationCode = Utils.OrganizationCode,
                    AdjustCode = item.AdjustCode,
                    DiscountRatePercent = item.DiscountRatePercent,
                    FeeAmount = item.FeeAmount,
                    FirstYearAmount = item.FirstYearAmount,
                    Flag = item.Flag,
                    FlatFee = item.FlatFee,
                    MinFee = item.MinFee,
                    MonthsFrom = item.MonthsFrom,
                    Period = item.Period,
                    ReasonCode = item.ReasonCode,
                    RenwalFee1 = item.RenwalFee1,
                    RenwalFee2 = item.RenwalFee2,
                    RenwalFee3 = item.RenwalFee3,
                    RenwalFee4 = item.RenwalFee4,
                    RenwalFee5 = item.RenwalFee5,
                    RenwalFee6 = item.RenwalFee6
                };
                //Save New Record
                if (currentFeeType == null && updatedFeeType != null)
                {
                    objFeeTypeRepository.Add(updatedFeeType);
                }
                //Update Record
                else
                {
                    objFeeTypeRepository.Merge(currentFeeType, updatedFeeType);
                }
                if (updatedFeeType != null)
                {
                    lstFeeTypeUpdated.Add(updatedFeeType);
                }
            }
            //Delete Record
            if (lstFeeTypeUpdated.Count > 0)
            {
                var feeTypeToDelete = currentFee.FeeType
                     .Where(x => !string.IsNullOrWhiteSpace(x.FeeCode) && !string.IsNullOrWhiteSpace(x.FeeTypeCode))
                     .Select(x => new { x.FeeCode, x.FeeTypeCode, x.OrganizationCode }).Except(lstFeeTypeUpdated
                                                     .Where(x => !string.IsNullOrWhiteSpace(x.FeeCode) && !string.IsNullOrWhiteSpace(x.FeeTypeCode))
                                                     .Select(x => new { x.FeeCode, x.FeeTypeCode, x.OrganizationCode }));

                if (feeTypeToDelete.Any())
                {
                    feeTypeToDelete.ToList().ForEach(x => objFeeTypeRepository.Remove(objFeeTypeRepository.Get(x.FeeCode, x.FeeTypeCode, x.OrganizationCode)));
                }
            }
            else
            {
                if (currentFee.FeeType.Count > 0)
                {
                    currentFee.FeeType.ToList().ForEach(x => objFeeTypeRepository.Remove(x));
                }
            }
            #endregion

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objFeeRepository.Merge(currentFee, updatedFee);
                    objFeeTypeRepository.UnitOfWork.Commit();
                    objFeeRepository.UnitOfWork.Commit();

                    if (AFeeDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_Fee", " FeeCode = '" + AFeeDTO.FeeCode + "' and OrganizationCode = '" + Utils.OrganizationCode + "'",
                            "FEE", AFeeDTO.LastUpdatedBy,// "/Master/FeeAuthorize?AFeeCode=" + AFeeDTO.FeeCode + "&", "MT");
                             "/Master/FeeAuthorize?AFeeCode=" + AFeeDTO.FeeCode + "&AOrganizationCode=" + Utils.OrganizationCode + "&", "MT");
                        objFeeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);

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
        #region DeleteFee
        public void DeleteFee(string AFeeCode)
        {
            var fee = objFeeRepository.Get(AFeeCode);
            if (fee != null)
            {
                objFeeRepository.Remove(fee);
                objFeeRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindFee
        public List<FeeDTO> FindFee(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objFeeRepository.GetRecordCount();
            var fee = objFeeRepository.GetAll().Where(x => (x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS"));
            if (fee != null && fee.Any())
            {
                List<FeeDTO> lstFeeDTO = new List<FeeDTO>();
                foreach (var item in fee)
                {
                    lstFeeDTO.Add(Conversion.Mapping.FeeToFeeDTO(item));
                }
                return lstFeeDTO;
            }
            else
                return new List<FeeDTO>();
        }
        #endregion
        #region FindAll
        public List<FeeDTO> FindAll(int APageNo, FeeDTO AFeeDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { 
                        new Utils.ExpressionList() { FilterColumn = "FeeCode",FilterValue = AFeeDTO.FeeCode, Operator = "Contains" },
                        new Utils.ExpressionList() { FilterColumn = "ProductCode",FilterValue = AFeeDTO.ProductCode, 
                            Operator = (AFeeDTO.ProductCode == string.Empty ? "Contains" : "Equals") },
                        new Utils.ExpressionList() { FilterColumn = "CustomerSegmentCode",FilterValue = AFeeDTO.CustomerSegmentCode, 
                            Operator = (AFeeDTO.CustomerSegmentCode == string.Empty ? "Contains" : "Equals") },
                        new Utils.ExpressionList() { FilterColumn = "CustomerSubSegmentCode",FilterValue = AFeeDTO.CustomerSubSegmentCode, 
                            Operator = (AFeeDTO.CustomerSubSegmentCode == string.Empty ? "Contains" : "Equals")},
                        new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                        new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"},
                        new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode , Operator = "Equals"}
                    };

            Expression<Func<Fee, bool>> expression = Utils.GetExpression<Fee>(lstExpression);
            int iRecordCount = objFeeRepository.GetRecordCount(expression);

            //var vFeeDTO = objFeeRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vFeeDTO = objFeeRepository.GetFiltered(0, iRecordCount, expression);

            if (vFeeDTO != null && vFeeDTO.Any())
            {
                List<FeeDTO> lstFeeDTO = new List<FeeDTO>();
                foreach (var item in vFeeDTO)
                {
                    lstFeeDTO.Add(Conversion.Mapping.FeeToFeeDTO(item));
                }
                return lstFeeDTO;
            }
            else
            {
                return new List<FeeDTO>(); ;
            }
        }
        #endregion
        #region FindByFeeCode
        public FeeDTO FindByFeeCode(string AFeeCode, string AOrganizationCode)
        {

            Fee objFee = objFeeRepository.Get(AFeeCode, Utils.OrganizationCode);
            if (objFee != null)
            {

                if (objFee.Product == null)
                {
                    objFee.Product = objProductRepository.Get(objFee.ProductCode);
                }
                if (objFee.CustomerSegment == null)
                {
                    objFee.CustomerSegment = objCustomerSegmentRepository.Get(objFee.CustomerSegmentCode, Utils.OrganizationCode);
                }
                if (objFee.CustomerSubSegment == null)
                {
                    objFee.CustomerSubSegment = objCustomerSubSegmentRepository.Get(objFee.CustomerSubSegmentCode, Utils.OrganizationCode);
                }
                if (objFee.Contract == null)
                {
                    objFee.Contract = objContractRepository.Get(objFee.ContractCode, objFee.OrganizationCode);
                }

                objFeeRepository.UnitOfWork.RefreshDataInModel(ref objFee);
                return Conversion.Mapping.FeeToFeeDTO(objFee);
            }
            else
            {
                return new FeeDTO();
            }
        }
        #endregion
        #endregion

        #region Product

        #region SaveProductDetails
        public void SaveProductDetails(ProductDTO AProductDTO)
        {
            if (AProductDTO == null)
            {
                throw new ArgumentException("");
            }

            SystemParameter objSystemParameter = objSystemParameterRepository.Get(AProductDTO.BillCycleSysParamId);

            AProductDTO.BillingCycleCode = objSystemParameter.DefaultValue;

            var newProduct = ProductFactory.CreateProduct(AProductDTO.ProductCode, AProductDTO.ProductName,
                                AProductDTO.ProductCategoryCode, AProductDTO.ProductFranchiseCode, AProductDTO.ProductSubCategoryCode, AProductDTO.ProductClassCode,
                                AProductDTO.CardNoPattern, AProductDTO.CardNoLength,
                                AProductDTO.RevolverChargeFlag, AProductDTO.MinimumCreditLine, AProductDTO.MaximumCreditLine, AProductDTO.CashLimit,
                                AProductDTO.CurrencyCode, AProductDTO.ProductMemberTypeCode, AProductDTO.BinValue, AProductDTO.SubBinValue,
                                AProductDTO.ProductExpiryInMonths, AProductDTO.ProductStatusCode,
                                AProductDTO.ServiceCode, AProductDTO.PaymentAllocationType, AProductDTO.InterestMethodCode,
                                AProductDTO.CapitalizedInterest, AProductDTO.MinimumConsumptionAmount, AProductDTO.BillCycleSysParamId, AProductDTO.BillingCycleCode,
                                AProductDTO.MaximumAddOnCount, AProductDTO.CanEmbossPhoto,
                                AProductDTO.CanEmbossSignature, AProductDTO.CanEmbossLogo, AProductDTO.ImageRefNo,
                                AProductDTO.NumberofAffinity, AProductDTO.InterestDiffFlag,
                                AProductDTO.InternalScore, AProductDTO.ExternalScore, AProductDTO.CPVFlag, AProductDTO.CombinedScore,
                                AProductDTO.ExternalScoreDefinitionId, AProductDTO.MinimumCombinedScore, AProductDTO.MinimumCardAdvance,
                                AProductDTO.MaximumCardAdvance, AProductDTO.InternalScoreDefinitionId, AProductDTO.ScoreFlag,
                                AProductDTO.RenewalScoreFlag, AProductDTO.CPPLine, AProductDTO.VerificationNeeded, AProductDTO.IsPreviewNeeded,
                                AProductDTO.MasterVerificationNeeded, AProductDTO.DeDuplicationOption,
                                AProductDTO.InterestWaiverFlag, AProductDTO.BackgroundImageEmbossFlag, AProductDTO.ProductClassIndicator, AProductDTO.AutoApprovalFlag, 
                                AProductDTO.ContractCode, AProductDTO.LegacyProductCode,
                                AProductDTO.CreatedBy, AProductDTO.LastUpdatedBy, AProductDTO.ApprovalStatus, AProductDTO.OrganizationCode, null);
            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objProductRepository.Add(newProduct);
                        objProductRepository.UnitOfWork.Commit();
                        if (AProductDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_Product", " ProductCode = '" + newProduct.ProductCode + "' and OrganizationCode = '" + newProduct.OrganizationCode + "'",
                                "PDT", AProductDTO.LastUpdatedBy,// "/Master/ProductAuthorize/" + AProductCode + "/A?", "MT");
                                   "/Master/ProductAuthorize?AProductCode=" + newProduct.ProductCode + "&AOrganizationCode=" + newProduct.OrganizationCode + "&", "MT");

                            objProductRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);


                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region UpdateProductDetails
        public void UpdateProductDetails(string AProductCode, string AOrganizationCode, ProductDTO AProductDTO)
        {
            if (AProductDTO == null)
            {
                throw new ArgumentException("");
            }

            SystemParameter objSystemParameter = objSystemParameterRepository.Get(AProductDTO.BillCycleSysParamId);

            AProductDTO.BillingCycleCode = objSystemParameter.DefaultValue;

            Product currentProduct = objProductRepository.Get(AOrganizationCode, AProductCode);
            Product updatedProduct = new Product()
            {
                ProductCode = AProductCode,
                ProductName = AProductDTO.ProductName,
                OrganizationCode = AOrganizationCode,

                ProductCategoryCode = AProductDTO.ProductCategoryCode,
                ProductSubCategoryCode = AProductDTO.ProductSubCategoryCode,
                ProductClassCode = AProductDTO.ProductClassCode,
                CardNoPattern = AProductDTO.CardNoPattern,
                CardNoLength = AProductDTO.CardNoLength,
                ProductFranchise = AProductDTO.ProductFranchiseCode,
                RevolverChargeFlag = AProductDTO.RevolverChargeFlag,
                MinimumCreditLine = AProductDTO.MinimumCreditLine,
                MaximumCreditLine = AProductDTO.MaximumCreditLine,
                CashLimit = AProductDTO.CashLimit,
                CurrencyCode = AProductDTO.CurrencyCode,
                ProductMemberTypeCode = AProductDTO.ProductMemberTypeCode,
                BinValue = AProductDTO.BinValue,
                SubBinValue = AProductDTO.SubBinValue,
                ProductExpiryInMonths = AProductDTO.ProductExpiryInMonths,
                ProductStatusCode = AProductDTO.ProductStatusCode,
                ServiceCode = AProductDTO.ServiceCode,
                PaymentAllocationType = AProductDTO.PaymentAllocationType,
                InterestMethodCode = AProductDTO.InterestMethodCode,
                CapitalizedInterest = AProductDTO.CapitalizedInterest,
                MinimumConsumptionAmount = AProductDTO.MinimumConsumptionAmount,
                BillCycleSysParamId = AProductDTO.BillCycleSysParamId,
                BillingCycleCode = AProductDTO.BillingCycleCode,
                MaximumAddOnCount = AProductDTO.MaximumAddOnCount,
                CanEmbossPhoto = AProductDTO.CanEmbossPhoto,
                CanEmbossSignature = AProductDTO.CanEmbossSignature,
                CanEmbossLogo = AProductDTO.CanEmbossLogo,
                ImageRefNo = AProductDTO.ImageRefNo,
                NumberofAffinity = AProductDTO.NumberofAffinity,
                InterestDiffFlag = AProductDTO.InterestDiffFlag,
                InternalScore = AProductDTO.InternalScore,
                ExternalScore = AProductDTO.ExternalScore,
                CPVFlag = AProductDTO.CPVFlag,
                CombinedScore = AProductDTO.CombinedScore,
                ExternalScoreDefinitionId = AProductDTO.ExternalScoreDefinitionId,
                MinimumCombinedScore = AProductDTO.MinimumCombinedScore,
                MinimumCardAdvance = AProductDTO.MinimumCardAdvance,
                MaximumCardAdvance = AProductDTO.MaximumCardAdvance,
                InternalScoreDefinitionId = AProductDTO.InternalScoreDefinitionId,
                ScoreFlag = AProductDTO.ScoreFlag,
                RenewalScoreFlag = AProductDTO.RenewalScoreFlag,
                CPPLine = AProductDTO.CPPLine,
                VerificationNeeded = AProductDTO.VerificationNeeded,
                IsPreviewNeeded = AProductDTO.IsPreviewNeeded,
                MasterVerificationNeeded = AProductDTO.MasterVerificationNeeded,
                DeDuplicationOption = AProductDTO.DeDuplicationOption,
                InterestWaiverFlag = AProductDTO.InterestWaiverFlag,
                BackgroundImageEmbossFlag = AProductDTO.BackgroundImageEmbossFlag,
                ProductClassIndicator = AProductDTO.ProductClassIndicator,
                AutoApprovalFlag = AProductDTO.AutoApprovalFlag,
                ContractCode = AProductDTO.ContractCode,
                LegacyProductCode = AProductDTO.LegacyProductCode,

                CreatedBy = currentProduct.CreatedBy,
                CreatedOn = currentProduct.CreatedOn,
                LastUpdatedBy = AProductDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate,
                ApprovalStatus = AProductDTO.ApprovalStatus
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objProductRepository.Merge(currentProduct, updatedProduct);
                    objProductRepository.UnitOfWork.Commit();

                    if (AProductDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_Product", " ProductCode = '" + AProductCode + "' and OrganizationCode = '" + AOrganizationCode + "'",
                            "PDT", AProductDTO.LastUpdatedBy,// "/Master/ProductAuthorize/" + AProductCode + "/A?", "MT");
                               "/Master/ProductAuthorize?AProductCode=" + AProductCode + "&AOrganizationCode=" + AOrganizationCode + "&", "MT");

                        objProductRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);



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
        #region DeleteProduct
        public void DeleteProduct(string AProductCode)
        {
            var product = objProductRepository.Get(AProductCode);
            if (product != null)
            {
                objProductRepository.Remove(product);
                objProductRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindProduct
        public List<ProductDTO> FindProduct(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objProductRepository.GetRecordCount();
            var product = objProductRepository.GetAll();
            if (product != null && product.Any())
            {
                List<ProductDTO> lstProductDTO = new List<ProductDTO>();
                foreach (var item in product)
                {
                    lstProductDTO.Add(Conversion.Mapping.ProductToProductDTO(item));
                }
                return lstProductDTO;
            }
            else
                return new List<ProductDTO>();
        }
        #endregion
        #region FindAll
        public List<ProductDTO> FindAll(int APageNo, ProductDTO AProductDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "ProductCode",FilterValue = AProductDTO.ProductCode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "ProductName",FilterValue = AProductDTO.ProductName,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "ProductCategoryCode",FilterValue = AProductDTO.ProductCategoryCode,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "ProductSubCategoryCode",FilterValue = AProductDTO.ProductSubCategoryCode,Operator = "Contains"},
                       new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils .OrganizationCode ,Operator = "Equals"}

                    };

            Expression<Func<Product, bool>> expression = Utils.GetExpression<Product>(lstExpression);
            int iRecordCount = objProductRepository.GetRecordCount(expression);

            //var vProductDTO = objProductRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vProductDTO = objProductRepository.GetFiltered(0, iRecordCount, expression);

            if (vProductDTO != null && vProductDTO.Any())
            {
                List<ProductDTO> lstProductDTO = new List<ProductDTO>();
                foreach (var item in vProductDTO)
                {
                    lstProductDTO.Add(Conversion.Mapping.ProductToProductDTO(item));
                }
                return lstProductDTO;
            }
            else
            {
                return new List<ProductDTO>();
            }
        }
        #endregion
        #region FindByProductCode
        public ProductDTO FindByProductCode(string AProductCode, string AOrganizationCode)
        {

            try
            {
                Product product1 = objProductRepository.Get(AOrganizationCode, AProductCode);
            }
            catch { }
            Product product = objProductRepository.Get(AOrganizationCode, AProductCode);
            if (product != null)
            {
                objProductRepository.UnitOfWork.RefreshDataInModel(ref product);

                if (product.Currency == null)
                {
                    product.Currency = objCurrencyRepository.Get(AOrganizationCode, product.CurrencyCode);
                }
                if (product.BillingCycle == null)
                {
                    product.BillingCycle = objSystemParameterRepository.Get(product.BillCycleSysParamId);
                }
                //if (product.DefaultAffinity == null)
                //{
                //    product.DefaultAffinity = objDefaultAffinityRepository.Get(product.DefaultAffinityCode);
                //}
                if (product.InterestMethod == null)
                {
                    product.InterestMethod = objInterestMethodRepository.Get(product.InterestMethodCode);
                }
                if (product.ProductClass == null)
                {
                    product.ProductClass = objProductClassRepository.Get(product.ProductClassCode);
                }
                //if (product.ProductMemberType == null)
                //{
                //    product.ProductMemberType = objProductMemberTypeRepository.Get(product.ProductMemberTypeCode);
                //}
                if (product.ProductCategory == null)
                {
                    product.ProductCategory = objProductCategoryRepository.Get(product.ProductCategoryCode);
                }
                if (product.ProductSubCategory == null)
                {
                    product.ProductSubCategory = objProductSubCategoryRepository.Get(product.ProductSubCategoryCode);
                }
                if (product.Service == null)
                {
                    product.Service = objServiceRepository.Get(product.ServiceCode);
                }

                return Conversion.Mapping.ProductToProductDTO(product);
            }

            else
            {
                return new ProductDTO();
            }


        }
        #endregion
        #region GetProductFirstChar
        public string GetProductFirstChar()
        {
            string strProductFirstDigit = "";


            SqlParameter[] paramCollection = new SqlParameter[]
                { 
                     new SqlParameter(parameterName: "@P_OrgCode",dbType:System.Data.SqlDbType.VarChar ),
                    new SqlParameter() { ParameterName = "@P_FirstDigit", SqlDbType = System.Data.SqlDbType.VarChar, Size = 9, Direction = System.Data.ParameterDirection.Output},
                };

            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_OrgCode")).First()).Value = Utils.OrganizationCode;

            objProductRepository.UnitOfWork.ExecuteCommand("PR_GetProductFirstDigit @P_OrgCode, @P_FirstDigit OUTPUT", paramCollection);

            strProductFirstDigit = paramCollection[1].Value.ToString();

            return strProductFirstDigit;

        }
        #endregion
        #region GetProductCurrency
        public string GetProductCurrency(string AProductCode)
        {
            return objProductRepository.Get(Utils.OrganizationCode, AProductCode).CurrencyCode;
        }
        #endregion
        #region GetCurrencyPrecision
        public string GetCurrencyPrecision(string ACurrencyCode)
        {
            return Convert.ToString(objCurrencyRepository.Get(Utils.OrganizationCode, ACurrencyCode).Precision);
        }
        #endregion

        #endregion

        #region Block Code

        #region SaveBlockCodeDetails
        public void SaveBlockCodeDetails(BlockCodeDTO ABlockCodeDTO)
        {
            if (ABlockCodeDTO == null)
            {
                throw new ArgumentException("");
            }

            var newBlockCode = BlockCodeFactory.CreateBlockCode(ABlockCodeDTO.Code, ABlockCodeDTO.OrganizationCode, ABlockCodeDTO.Description, ABlockCodeDTO.ShortDescription,
                                ABlockCodeDTO.IsAccountBlocked, ABlockCodeDTO.IsAccountBlockedForFinancialReasons, ABlockCodeDTO.IsAccountBlockedForNonFinancialReasons,
                                ABlockCodeDTO.FinancialHierarchy, ABlockCodeDTO.NonFinancialHierarchy, ABlockCodeDTO.IsCardBlocked, ABlockCodeDTO.CardBlockHierarchy,
                                ABlockCodeDTO.IsPermanantBlock, ABlockCodeDTO.PlasticRecoveryNeeded, ABlockCodeDTO.InstallmentClose, ABlockCodeDTO.Unblock,
                            ABlockCodeDTO.CreatedBy, ABlockCodeDTO.LastUpdatedBy, ABlockCodeDTO.ApprovalStatus, null);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objBlockCodeRepository.Add(newBlockCode);
                        objBlockCodeRepository.UnitOfWork.Commit();
                        if (ABlockCodeDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_BlockCode", " Code = '" + newBlockCode.Code + "'",
                                "BLKCDE", ABlockCodeDTO.CreatedBy, "/Master/BlockCodeAuthorize/" + newBlockCode.Code + "/A?", "MT");

                            objBlockCodeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value,"
                             + "@P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
        #region UpdateBlockCodeDetails
        public void UpdateBlockCodeDetails(string ACode, BlockCodeDTO ABlockCodeDTO)
        {
            if (ABlockCodeDTO == null)
            {
                throw new ArgumentException("");
            }

            BlockCode currentBlockCode = objBlockCodeRepository.Get(ACode, Utils.OrganizationCode);
            BlockCode updatedBlockCode = new BlockCode()
            {
                Code = ACode,
                OrganizationCode = ABlockCodeDTO.OrganizationCode,
                Description = ABlockCodeDTO.Description,
                ShortDescription = ABlockCodeDTO.ShortDescription,
                IsAccountBlocked = ABlockCodeDTO.IsAccountBlocked,
                IsAccountBlockedForFinancialReasons = ABlockCodeDTO.IsAccountBlockedForFinancialReasons,
                IsAccountBlockedForNonFinancialReasons = ABlockCodeDTO.IsAccountBlockedForNonFinancialReasons,
                FinancialHierarchy = ABlockCodeDTO.FinancialHierarchy,
                NonFinancialHierarchy = ABlockCodeDTO.NonFinancialHierarchy,
                IsCardBlocked = ABlockCodeDTO.IsCardBlocked,
                CardBlockHierarchy = ABlockCodeDTO.CardBlockHierarchy,
                IsPermanantBlock = ABlockCodeDTO.IsPermanantBlock,
                PlasticRecoveryNeeded = ABlockCodeDTO.PlasticRecoveryNeeded,
                InstallmentClose = ABlockCodeDTO.InstallmentClose,
                Unblock = ABlockCodeDTO.Unblock,

                CreatedBy = currentBlockCode.CreatedBy,
                CreatedOn = currentBlockCode.CreatedOn,
                LastUpdatedBy = ABlockCodeDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate,
                ApprovalStatus = ABlockCodeDTO.ApprovalStatus
            };
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objBlockCodeRepository.Merge(currentBlockCode, updatedBlockCode);
                    objBlockCodeRepository.UnitOfWork.Commit();
                    if (ABlockCodeDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_BlockCode", " Code = '" + ACode + "'",
                            "BLKCDE", ABlockCodeDTO.LastUpdatedBy, "/Master/BlockCodeAuthorize/" + ACode + "/A?", "MT");
                        objBlockCodeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName," +
                            " @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #region DeleteBlockCode
        public void DeleteBlockCode(string ACode)
        {
            var blockCode = objBlockCodeRepository.Get(ACode, Utils.OrganizationCode);
            if (blockCode != null)
            {
                objBlockCodeRepository.Remove(blockCode);
                objBlockCodeRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindBlockCode
        public List<BlockCodeDTO> FindBlockCode(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objBlockCodeRepository.GetRecordCount();
            var blockCode = objBlockCodeRepository.GetAll();
            if (blockCode != null && blockCode.Any())
            {
                List<BlockCodeDTO> lstBlockCodeDTO = new List<BlockCodeDTO>();
                foreach (var item in blockCode)
                {
                    lstBlockCodeDTO.Add(Conversion.Mapping.BlockCodeToBlockCodeDTO(item));
                }
                return lstBlockCodeDTO;
            }
            else
                return new List<BlockCodeDTO>();
        }
        #endregion
        #region FindAll
        public List<BlockCodeDTO> FindAll(int APageNo, BlockCodeDTO ABlockCodeDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "Code",FilterValue = ABlockCodeDTO.Code, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "Description",FilterValue = ABlockCodeDTO.Description,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "ShortDescription",FilterValue = ABlockCodeDTO.ShortDescription,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode,Operator = "Equals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
                    };

            Expression<Func<BlockCode, bool>> expression = Utils.GetExpression<BlockCode>(lstExpression);
            int iRecordCount = objBlockCodeRepository.GetRecordCount(expression);

            //var vBlockCodeDTO = objBlockCodeRepository.GetFiltered(APageNo, iRecordCount, expression);

            var vBlockCodeDTO = objBlockCodeRepository.GetFiltered(0, iRecordCount, expression);

            if (vBlockCodeDTO != null && vBlockCodeDTO.Any())
            {
                List<BlockCodeDTO> lstBlockCodeDTO = new List<BlockCodeDTO>();
                foreach (var item in vBlockCodeDTO)
                {
                    lstBlockCodeDTO.Add(Conversion.Mapping.BlockCodeToBlockCodeDTO(item));
                }
                return lstBlockCodeDTO;
            }
            else
            {
                return new List<BlockCodeDTO>();
            }
        }
        #endregion
        #region FindByBlockCode
        public BlockCodeDTO FindByBlockCode(string ACode)
        {
            BlockCode blockCode = objBlockCodeRepository.Get(ACode, Utils.OrganizationCode);
            if (blockCode != null)
            {
                objBlockCodeRepository.UnitOfWork.RefreshDataInModel(ref blockCode);
                return Conversion.Mapping.BlockCodeToBlockCodeDTO(blockCode);
            }
            else
            {
                return new BlockCodeDTO();
            }
        }
        #endregion

        #endregion

        #region Customer

        #region SaveCustomerDetails
        public void SaveCustomerDetails(CustomerDTO ACustomerDTO)
        {
            if (ACustomerDTO == null)
            {
                throw new ArgumentException("");
            }

            var newCustomer = CustomerFactory.CreateCustomer(ACustomerDTO.CustomerId, ACustomerDTO.ParentCustomerId, ACustomerDTO.BranchCode, ACustomerDTO.CampaignSource,
                ACustomerDTO.ChannelCode, ACustomerDTO.ApplicationReferenceNo, ACustomerDTO.OrganizationCode, ACustomerDTO.ProductMemberTypeCode,
                ACustomerDTO.ProductTypeCode, ACustomerDTO.ProductCategoryCode, ACustomerDTO.ProductCode, ACustomerDTO.RUT, ACustomerDTO.SerialNo,
                ACustomerDTO.ExpiryDate, ACustomerDTO.CorporateRUT, ACustomerDTO.SalesId, ACustomerDTO.PhysicalApplicationRefNo, ACustomerDTO.PhysicalApplicationRefDate,
                ACustomerDTO.ExternalCivilRegistryResult, ACustomerDTO.InputterId, ACustomerDTO.InputterDate, ACustomerDTO.CreditScore,
                ACustomerDTO.CustomerType, ACustomerDTO.Name, ACustomerDTO.PaternalSurname, ACustomerDTO.MaternalSurname, ACustomerDTO.Nationality,
                ACustomerDTO.DateOfBirth, ACustomerDTO.Gender, ACustomerDTO.MaritalStatus, ACustomerDTO.NumberofDependents, ACustomerDTO.Education,
                ACustomerDTO.Profession, ACustomerDTO.Housing, ACustomerDTO.EmailID, ACustomerDTO.LegacyCardNumber, ACustomerDTO.IsActive, ACustomerDTO.IsBlackListed,
                ACustomerDTO.CreatedBy, ACustomerDTO.ApprovalStatus, ACustomerDTO.LastUpdatedBy);

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
            new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    if (ACustomerDTO.AutoApprove && ACustomerDTO.ApprovalStatus == "P")
                    {
                        newCustomer.ApprovalStatus = "A";
                        newCustomer.ApprovedBy = ACustomerDTO.CreatedBy;
                        newCustomer.ApprovedOn = Utils.BusinessDate.Add(Utils.BusinessDate.TimeOfDay);
                        newCustomer.ApprovedOn = new DateTime(Utils.BusinessDate.Year, Utils.BusinessDate.Month, Utils.BusinessDate.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    }

                    objCustomerRepository.Add(newCustomer);
                    objCustomerRepository.UnitOfWork.Commit();

                    #region Customer Address

                    ACustomerDTO.CustomerAddressDTO.ForEach(x => objCustomerAddressRepository.Add(
                        CustomerAddressFactory.CreateCustomerAddress(x.AddressId, x.AddressTypeCode, newCustomer.CustomerId,
                        x.TypeOfResidenceClearance, x.RegionCode, x.CityCode, x.CommunaCode, x.Street, x.Number, x.Block,
                        x.ComplementOfAddress, x.Town, x.Landmark, x.PostalCode, x.POBox, x.Phone, x.Mobile, x.Fax)));
                    objCustomerAddressRepository.UnitOfWork.Commit();

                    #endregion

                    #region Customer Occupation

                    ACustomerDTO.CustomerOccupationDTO.ForEach(x => objCustomerOccupationRepository.Add(
                        CustomerOccupationFactory.CreateCustomerOccupation(x.OccupationId, newCustomer.CustomerId, x.WorkType,
                        x.TotalMonthlyIncome, x.CompanyRUT, x.CompanyName, x.DateOfJoining, x.Post, x.IsIncomeProofAvailable, x.IncomeType)));
                    objCustomerOccupationRepository.UnitOfWork.Commit();

                    #endregion

                    #region Customer Current Account Detail

                    ACustomerDTO.CustomerCurrentAccountDetailDTO.ForEach(x => objCustomerCurrentAccountDetailRepository.Add(
                        CustomerCurrentAccountDetailFactory.CreateCustomerCurrentAccountDetail(x.AccountId, newCustomer.CustomerId,
                        x.BankCode, x.AccountNumber, x.BankType)));
                    objCustomerCurrentAccountDetailRepository.UnitOfWork.Commit();

                    #endregion

                    #region Customer Card Detail

                    ACustomerDTO.CustomerCreditCardDetailDTO.ForEach(x => objCustomerCreditCardDetailRepository.Add(
                        CustomerCreditCardDetailFactory.CreateCustomerCreditCardDetail(x.CreditCardId, newCustomer.CustomerId,
                        x.CardType, x.CardBrand, x.BankInstitutionId, x.CardNumber, x.EffectiveToDate, x.CreditLimit)));
                    objCustomerCreditCardDetailRepository.UnitOfWork.Commit();

                    #endregion

                    #region Customer Commerical House Consumption

                    CustomerCommericalHouseConsumption customerCommericalHouseConsumptionNew = new CustomerCommericalHouseConsumption();
                    ACustomerDTO.CustomerCommericalHouseConsumptionDTO.ForEach(x => objCustomerCommericalHouseConsumptionRepository.Add(
                        CustomerCommericalHouseConsumptionFactory.CreateCustomerCommericalHouseConsumption(
                        x.CommericalId, newCustomer.CustomerId, x.Type, x.Name, x.CardNumber, x.AccountStatus)));
                    objCustomerCommericalHouseConsumptionRepository.UnitOfWork.Commit();

                    #endregion

                    #region Customer Billing Detail

                    ACustomerDTO.CustomerBillingDetailDTO.ForEach(x => objCustomerBillingDetailRepository.Add(
                        CustomerBillingDetailFactory.CreateCustomerBillingDetail(x.BillingDetailId, newCustomer.CustomerId,
                        x.IsApplicationSignedOffByCustomer, x.SendAccountStatementTo, x.AccountNo, x.BranchCode)));//, x.IsECSRequried, x.PaymentDate, x.ECSFor, x.BankCode

                    objCustomerBillingDetailRepository.UnitOfWork.Commit();

                    #endregion

                    #region Customer Dependent

                    ACustomerDTO.CustomerDependentDTO.ForEach(x => objCustomerDependentRepository.Add(
                        CustomerDependentFactory.CreateCustomerDependent(x.DependentID, newCustomer.CustomerId, x.AgeGroup, x.NoOfChildren)));
                    objCustomerDependentRepository.UnitOfWork.Commit();

                    #endregion

                    #region Customer Decision

                    ACustomerDTO.CustomerDecisionDTO.ForEach(x => objCustomerDecisionRepository.Add(
                        CustomerDecisionFactory.CreateCustomerDecision(x.DecisionID, newCustomer.CustomerId, x.DecisionCode,
                        x.ReasonCode, x.Remarks, x.ContractCode, x.Unemployment, x.IsContractAcceptedByCustomer)));
                    objCustomerDecisionRepository.UnitOfWork.Commit();

                    #endregion

                    if (ACustomerDTO.ApprovalStatus == "P")
                    {
                        if (!ACustomerDTO.AutoApprove)
                        {
                            //check for violation
                            bool autoApproveActualvalue = objProductRepository.Get(ACustomerDTO.OrganizationCode, ACustomerDTO.ProductCode).AutoApprovalFlag;
                            bool isCreditScoreViolated = ValidateCreditScore(ACustomerDTO.ProductCode, ACustomerDTO.CreditScore);
                            bool isHRP = ValidateCustomerDesignationForHRP(ACustomerDTO.CustomerOccupationDTO.FirstOrDefault().Post);
                            string ContextCode = string.Empty;
                            if ((isCreditScoreViolated || isHRP) && autoApproveActualvalue)
                            {
                                ContextCode = "CUSTV";
                            }
                            if (!autoApproveActualvalue)
                            {
                                ContextCode = "CUST";
                            }
                            //-------------------
                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_Customer", " CustomerId = '" + newCustomer.CustomerId + "'",
                                ContextCode, ACustomerDTO.LastUpdatedBy, "/Master/CustomerAuthorize?ACustomerId=" + newCustomer.CustomerId + "&", "MT");
                            objCustomerRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value," +
                                " @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                    }
                    SqlParameter[] paramApplicationRefNo = new SqlParameter[] { 
                            new SqlParameter(){ ParameterName = "@P_CustomerId", SqlDbType = System.Data.SqlDbType.Int, Value = newCustomer.CustomerId, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ ParameterName = "@P_OrganizationCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = ACustomerDTO.OrganizationCode, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ParameterName = "@R_ApplicationRefNo", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Output, Size = 25}
                        };

                    objCustomerRepository.UnitOfWork.ExecuteCommand("PR_UpdateCustomerApplicationReferenceNo @P_CustomerId,@P_OrganizationCode,@R_ApplicationRefNo OUTPUT", paramApplicationRefNo);
                    newCustomer.ApplicationReferenceNo = paramApplicationRefNo[2].Value.ToString();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion
        #region UpdateCustomerDetails
        public void UpdateCustomerDetails(int ACustomerId, CustomerDTO ACustomerDTO)
        {
            if (ACustomerDTO == null)
            {
                throw new ArgumentException("");
            }

            Customer currentCustomer = objCustomerRepository.Get(ACustomerId);

            Customer updatedCustomer = new Customer()
            {
                CustomerId = ACustomerId,
                ParentCustomerId = ACustomerDTO.ParentCustomerId,
                BranchCode = ACustomerDTO.BranchCode,
                CampaignSource = ACustomerDTO.CampaignSource,
                ChannelCode = ACustomerDTO.ChannelCode,
                ApplicationReferenceNo = ACustomerDTO.ApplicationReferenceNo,
                OrganizationCode = ACustomerDTO.OrganizationCode,
                ProductMemberTypeCode = ACustomerDTO.ProductMemberTypeCode,
                ProductTypeCode = ACustomerDTO.ProductTypeCode,
                ProductCategoryCode = ACustomerDTO.ProductCategoryCode,
                ProductCode = ACustomerDTO.ProductCode,
                SalesId = ACustomerDTO.SalesId,
                RUT = ACustomerDTO.RUT.Contains("-") ? ACustomerDTO.RUT.Replace("-", string.Empty) : ACustomerDTO.RUT,
                SerialNo = ACustomerDTO.SerialNo,
                ExpiryDate = ACustomerDTO.ExpiryDate,
                CorporateRUT = ACustomerDTO.CorporateRUT,
                PhysicalApplicationRefNo = ACustomerDTO.PhysicalApplicationRefNo,
                PhysicalApplicationRefDate = ACustomerDTO.PhysicalApplicationRefDate,
                ExternalCivilRegistryResult = ACustomerDTO.ExternalCivilRegistryResult,
                InputterId = ACustomerDTO.InputterId,
                InputterDate = ACustomerDTO.InputterDate,
                CreditScore = ACustomerDTO.CreditScore,

                CustomerType = ACustomerDTO.CustomerType,
                Name = ACustomerDTO.Name,
                PaternalSurname = ACustomerDTO.PaternalSurname,
                MaternalSurname = ACustomerDTO.MaternalSurname,
                Nationality = ACustomerDTO.Nationality,
                DateOfBirth = ACustomerDTO.DateOfBirth,
                Gender = ACustomerDTO.Gender,
                MaritalStatus = ACustomerDTO.MaritalStatus,
                NumberofDependents = ACustomerDTO.NumberofDependents,
                Education = ACustomerDTO.Education,
                Profession = ACustomerDTO.Profession,
                Housing = ACustomerDTO.Housing,
                EmailID = ACustomerDTO.EmailID,
                IsActive = ACustomerDTO.IsActive,
                IsBlackListed = ACustomerDTO.IsBlackListed,
                ApprovalStatus = ACustomerDTO.ApprovalStatus,
                CreatedBy = currentCustomer.CreatedBy,
                CreatedOn = currentCustomer.CreatedOn,
                LastUpdatedBy = ACustomerDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate,
                LegacyCardNumber = ACustomerDTO.LegacyCardNumber
            };

            #region update customer occupation child collection
            foreach (var item in ACustomerDTO.CustomerOccupationDTO)
            {
                var currenctCustomerOccupation = objCustomerOccupationRepository.Get(item.OccupationId);
                var updatedCustomerOccupation = new CustomerOccupation()
                {
                    CustomerId = ACustomerId,
                    OccupationId = item.OccupationId,
                    CompanyRUT = item.CompanyRUT,
                    CompanyName = item.CompanyName,
                    DateOfJoining = item.DateOfJoining,
                    WorkType = item.WorkType,
                    Post = item.Post,
                    IncomeType = item.IncomeType,
                    TotalMonthlyIncome = item.TotalMonthlyIncome,
                    IsIncomeProofAvailable = item.IsIncomeProofAvailable
                };
                if (currenctCustomerOccupation == null && updatedCustomerOccupation != null)
                {
                    objCustomerOccupationRepository.Add(updatedCustomerOccupation);
                }
                else
                {
                    objCustomerOccupationRepository.Merge(currenctCustomerOccupation, updatedCustomerOccupation);
                }
            }
            #endregion

            #region Update commercial house consumption child collection
            foreach (var item in ACustomerDTO.CustomerCommericalHouseConsumptionDTO)
            {
                var currentCommericalHouseConsumption = objCustomerCommericalHouseConsumptionRepository.Get(item.CommericalId);
                var updatedCommericalHouseConsumption = new CustomerCommericalHouseConsumption()
                {
                    CommericalId = item.CommericalId,
                    CustomerId = ACustomerId,
                    Type = item.Type,
                    Name = item.Name,
                    CardNumber = item.CardNumber,
                    AccountStatus = item.AccountStatus
                };
                if (currentCommericalHouseConsumption == null && updatedCommericalHouseConsumption != null)
                {
                    objCustomerCommericalHouseConsumptionRepository.Add(updatedCommericalHouseConsumption);
                }
                else
                {
                    objCustomerCommericalHouseConsumptionRepository.Merge(currentCommericalHouseConsumption, updatedCommericalHouseConsumption);
                }
            }
            #endregion

            #region Update customer billing detail child collection
            foreach (var item in ACustomerDTO.CustomerBillingDetailDTO)
            {
                var currentBillingDetail = objCustomerBillingDetailRepository.Get(item.BillingDetailId);
                var updatedBillingDetail = new CustomerBillingDetail()
                {
                    BillingDetailId = item.BillingDetailId,
                    CustomerId = ACustomerId,
                    IsApplicationSignedOffByCustomer = item.IsApplicationSignedOffByCustomer,
                    SendAccountStatementTo = item.SendAccountStatementTo,
                    AccountNo = item.AccountNo,
                    BranchCode = item.BranchCode
                    //IsECSRequried = x.IsECSRequried,
                    //PaymentDate = x.PaymentDate,
                    //ECSFor = x.ECSFor,
                    //BankCode = x.BankCode
                };
                if (currentBillingDetail == null && updatedBillingDetail != null)
                {
                    objCustomerBillingDetailRepository.Add(updatedBillingDetail);
                }
                else
                {
                    objCustomerBillingDetailRepository.Merge(currentBillingDetail, updatedBillingDetail);
                }
            }
            #endregion

            #region Update customer address child collection
            List<CustomerAddress> lstCustomerAddressUpdated = new List<CustomerAddress>();
            foreach (var item in ACustomerDTO.CustomerAddressDTO)
            {
                var currentAddressDetail = objCustomerAddressRepository.Get(item.AddressId);
                var updatedAddressDetail = new CustomerAddress()
                {
                    AddressId = item.AddressId,
                    AddressTypeCode = item.AddressTypeCode,
                    CustomerId = ACustomerId,
                    TypeOfResidenceClearance = item.TypeOfResidenceClearance,
                    RegionCode = item.RegionCode,
                    CityCode = item.CityCode,
                    CommunaCode = item.CommunaCode,
                    Street = item.Street,
                    Number = item.Number,
                    Block = item.Block,
                    ComplementOfAddress = item.ComplementOfAddress,
                    Town = item.Town,
                    Landmark = item.Landmark,
                    PostalCode = item.PostalCode,
                    POBox = item.POBox,
                    Phone = item.Phone,
                    Mobile = item.Mobile,
                    Fax = item.Fax
                };
                //Save New Record
                if (currentAddressDetail == null && updatedAddressDetail != null)
                {
                    objCustomerAddressRepository.Add(updatedAddressDetail);
                }
                //Update Record
                else
                {
                    objCustomerAddressRepository.Merge(currentAddressDetail, updatedAddressDetail);
                }

                if (updatedAddressDetail != null)
                {
                    lstCustomerAddressUpdated.Add(updatedAddressDetail);
                }
            }
            //Delete Record
            if (lstCustomerAddressUpdated.Count > 0)
            {
                List<CustomerAddress> lstCustomerAddressAlreayExisting = new List<CustomerAddress>();
                lstCustomerAddressAlreayExisting = currentCustomer.CustomerAddress;

                var customerAddressToDelete = lstCustomerAddressAlreayExisting
                    .Where(x => x.AddressId != 0)
                    .Select(x => x.AddressId).Except(lstCustomerAddressUpdated
                                                        .Where(x => x.AddressId != 0)
                                                        .Select(x => x.AddressId));
                if (customerAddressToDelete.Any())
                {
                    customerAddressToDelete.ToList().ForEach(x => objCustomerAddressRepository.Remove(objCustomerAddressRepository.Get(x)));
                }
            }
            else
            {
                if (currentCustomer.CustomerAddress.Count > 0)
                {
                    currentCustomer.CustomerAddress.ToList().ForEach(x => objCustomerAddressRepository.Remove(x));
                }
            }
            #endregion

            #region Update customer credit card child collection
            List<CustomerCreditCardDetail> lstCustomerCreditCardDetailUpdated = new List<CustomerCreditCardDetail>();
            foreach (var item in ACustomerDTO.CustomerCreditCardDetailDTO)
            {
                var currentCreditCardDetail = objCustomerCreditCardDetailRepository.Get(item.CreditCardId);
                var updatedCreditCardDetail = new CustomerCreditCardDetail()
                {
                    CreditCardId = item.CreditCardId,
                    CustomerId = ACustomerId,
                    CardType = item.CardType,
                    CardBrand = item.CardBrand,
                    BankInstitutionId = item.BankInstitutionId,
                    CardNumber = item.CardNumber,
                    EffectiveToDate = item.EffectiveToDate,
                    CreditLimit = item.CreditLimit
                };
                //Save new record
                if (currentCreditCardDetail == null && updatedCreditCardDetail != null)
                {
                    objCustomerCreditCardDetailRepository.Add(updatedCreditCardDetail);
                }
                //Update record
                else
                {
                    objCustomerCreditCardDetailRepository.Merge(currentCreditCardDetail, updatedCreditCardDetail);
                }

                if (updatedCreditCardDetail != null)
                {
                    lstCustomerCreditCardDetailUpdated.Add(updatedCreditCardDetail);
                }
            }
            //Delete Record
            if (lstCustomerCreditCardDetailUpdated.Count > 0)
            {
                List<CustomerCreditCardDetail> lstCustomerCreditCardDetailAlreayExisting = new List<CustomerCreditCardDetail>();
                lstCustomerCreditCardDetailAlreayExisting = currentCustomer.CustomerCreditCardDetail;

                var customerCreditCardIDToDelete = lstCustomerCreditCardDetailAlreayExisting
                    .Where(x => x.CreditCardId != 0)
                    .Select(x => x.CreditCardId).Except(lstCustomerCreditCardDetailUpdated
                                                        .Where(x => x.CreditCardId != 0)
                                                        .Select(x => x.CreditCardId));
                if (customerCreditCardIDToDelete.Any())
                {
                    customerCreditCardIDToDelete.ToList().ForEach(x => objCustomerCreditCardDetailRepository.Remove(objCustomerCreditCardDetailRepository.Get(x)));
                }
            }
            else
            {
                if (currentCustomer.CustomerCreditCardDetail.Count > 0)
                {
                    currentCustomer.CustomerCreditCardDetail.ToList().ForEach(x => objCustomerCreditCardDetailRepository.Remove(x));
                }
            }
            #endregion

            #region Update customer current account child collection
            List<CustomerCurrentAccountDetail> lstCustomerCurrentAccountUpdated = new List<CustomerCurrentAccountDetail>();
            foreach (var item in ACustomerDTO.CustomerCurrentAccountDetailDTO)
            {
                var currentCurrentAccountDetail = objCustomerCurrentAccountDetailRepository.Get(item.AccountId);
                var updatedCurrentAccountDetail = new CustomerCurrentAccountDetail()
                {
                    AccountId = item.AccountId,
                    CustomerId = ACustomerId,
                    AccountNumber = item.AccountNumber,
                    BankType = item.BankType,
                    BankCode = item.BankCode
                };
                //Save New Record
                if (currentCurrentAccountDetail == null && updatedCurrentAccountDetail != null)
                {
                    objCustomerCurrentAccountDetailRepository.Add(updatedCurrentAccountDetail);
                }
                //else if (currentCurrentAccountDetail != null && updatedCurrentAccountDetail == null)
                //{
                //    objCustomerCurrentAccountDetailRepository.Remove(currentCurrentAccountDetail);
                //}
                //Update Record
                else
                {
                    objCustomerCurrentAccountDetailRepository.Merge(currentCurrentAccountDetail, updatedCurrentAccountDetail);
                }

                if (updatedCurrentAccountDetail != null)
                {
                    lstCustomerCurrentAccountUpdated.Add(updatedCurrentAccountDetail);
                }
            }
            //Delete Record
            if (lstCustomerCurrentAccountUpdated.Count > 0)
            {
                var accountIDToDelete = currentCustomer.CustomerCurrentAccountDetail
                     .Where(x => x.AccountId != 0)
                     .Select(x => x.AccountId).Except(lstCustomerCurrentAccountUpdated
                                                     .Where(x => x.AccountId != 0)
                                                     .Select(x => x.AccountId));

                if (accountIDToDelete.Any())
                {
                    accountIDToDelete.ToList().ForEach(x => objCustomerCurrentAccountDetailRepository.Remove(objCustomerCurrentAccountDetailRepository.Get(x)));
                }
            }
            else
            {
                if (currentCustomer.CustomerCurrentAccountDetail.Count > 0)
                {
                    currentCustomer.CustomerCurrentAccountDetail.ToList().ForEach(x => objCustomerCurrentAccountDetailRepository.Remove(x));
                }
            }
            #endregion

            //if (currentCustomer.ApprovalStatus.Equals("A"))
            //{
            #region Update customer decision child collection
            foreach (var item in ACustomerDTO.CustomerDecisionDTO)
            {
                var currentCustomerDecisionDetail = objCustomerDecisionRepository.Get(item.DecisionID);
                var updatedCustomerDecisionDetail = new CustomerDecision()
                {
                    DecisionID = item.DecisionID,
                    CustomerId = ACustomerId,
                    DecisionCode = item.DecisionCode,
                    ReasonCode = item.ReasonCode,
                    Remarks = item.Remarks,
                    ContractCode = item.ContractCode,
                    Unemployment = item.Unemployment,
                    IsContractAcceptedByCustomer = item.IsContractAcceptedByCustomer,
                    CreditLimit = item.CreditLimit
                };
                if (currentCustomerDecisionDetail == null && updatedCustomerDecisionDetail != null)
                {
                    objCustomerDecisionRepository.Add(updatedCustomerDecisionDetail);
                }
                else
                {
                    objCustomerDecisionRepository.Merge(currentCustomerDecisionDetail, updatedCustomerDecisionDetail);
                }
            }
            #endregion
            //}

            #region Update customer dependent child collection
            List<CustomerDependent> lstCustomerDependentToUpdate = new List<CustomerDependent>();
            foreach (var item in ACustomerDTO.CustomerDependentDTO)
            {
                var currentCustomerDependentDetail = objCustomerDependentRepository.Get(item.DependentID);
                var updatedCustomerDependentDetail = new CustomerDependent()
                {
                    DependentID = item.DependentID,
                    CustomerId = ACustomerId,
                    NoOfChildren = item.NoOfChildren,
                    AgeGroup = item.AgeGroup
                };
                //Save newly added record
                if (currentCustomerDependentDetail == null && updatedCustomerDependentDetail != null)
                {
                    objCustomerDependentRepository.Add(updatedCustomerDependentDetail);
                }
                //Update record
                else
                {
                    objCustomerDependentRepository.Merge(currentCustomerDependentDetail, updatedCustomerDependentDetail);
                }

                if (updatedCustomerDependentDetail != null)
                {
                    lstCustomerDependentToUpdate.Add(updatedCustomerDependentDetail);
                }
            }
            //Delete record
            if (lstCustomerDependentToUpdate.Count > 0)
            {
                List<CustomerDependent> lstDependentToDelete = new List<CustomerDependent>();

                var customerDependentAlreadyExisting = currentCustomer.CustomerDependent;
                var dependentIDToDelete = customerDependentAlreadyExisting.Where(x => x.DependentID != 0).Select(x => x.DependentID).Except(lstCustomerDependentToUpdate.Where(x => x.DependentID != 0).Select(x => x.DependentID));

                if (dependentIDToDelete.Any())
                {
                    dependentIDToDelete.ToList().ForEach(x => lstDependentToDelete.Add(objCustomerDependentRepository.Get(x)));
                    lstDependentToDelete.ToList().ForEach(x => objCustomerDependentRepository.Remove(x));
                }
            }
            else
            {
                if (currentCustomer.CustomerDependent.Count > 0)
                {
                    currentCustomer.CustomerDependent.ToList().ForEach(x => objCustomerDependentRepository.Remove(x));
                }
            }
            #endregion

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
            new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    if (ACustomerDTO.AutoApprove)
                    {
                        updatedCustomer.ApprovalStatus = "A";
                        updatedCustomer.ApprovedBy = ACustomerDTO.LastUpdatedBy;
                        updatedCustomer.ApprovedOn = Utils.BusinessDate.Add(DateTime.Now.TimeOfDay);
                    }

                    objCustomerRepository.Merge(currentCustomer, updatedCustomer);

                    objCustomerOccupationRepository.UnitOfWork.Commit();
                    objCustomerCommericalHouseConsumptionRepository.UnitOfWork.Commit();
                    objCustomerBillingDetailRepository.UnitOfWork.Commit();
                    objCustomerDecisionRepository.UnitOfWork.Commit();
                    objCustomerAddressRepository.UnitOfWork.Commit();
                    objCustomerCreditCardDetailRepository.UnitOfWork.Commit();
                    objCustomerCurrentAccountDetailRepository.UnitOfWork.Commit();
                    objCustomerDependentRepository.UnitOfWork.Commit();
                    objCustomerRepository.UnitOfWork.Commit();
                    if (ACustomerDTO.ApprovalStatus == "P")
                    {
                        if (!ACustomerDTO.AutoApprove)
                        {
                            //check for violation
                            bool autoApproveActualvalue = objProductRepository.Get(ACustomerDTO.OrganizationCode, ACustomerDTO.ProductCode).AutoApprovalFlag;
                            bool isCreditScoreViolated = ValidateCreditScore(ACustomerDTO.ProductCode, ACustomerDTO.CreditScore);
                            bool isHRP = ValidateCustomerDesignationForHRP(ACustomerDTO.CustomerOccupationDTO.FirstOrDefault().Post);
                            string ContextCode = string.Empty;
                            if ((isCreditScoreViolated || isHRP) && autoApproveActualvalue)
                            {
                                ContextCode = "CUSTV";
                            }
                            if (!autoApproveActualvalue)
                            {
                                ContextCode = "CUST";
                            }
                            //-------------------
                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_Customer", " CustomerId = '" + ACustomerId + "'",
                                "CUST", ACustomerDTO.LastUpdatedBy, "/Master/CustomerAuthorize?ACustomerId=" + ACustomerId + "&", "MT");
                            objCustomerRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value," +
                                " @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
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
        #region DeleteCustomer
        public void DeleteCustomer(int ACustomerId)
        {
            var customer = objCustomerRepository.Get(ACustomerId);
            if (customer != null)
            {
                objCustomerRepository.Remove(customer);
                objCustomerRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindCustomer
        public List<CustomerDTO> FindCustomer(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objCustomerRepository.GetRecordCount();
            var customer = objCustomerRepository.GetAll();
            if (customer != null && customer.Any())
            {
                List<CustomerDTO> lstCustomerDTO = new List<CustomerDTO>();
                foreach (var item in customer)
                {
                    lstCustomerDTO.Add(Conversion.Mapping.CustomerToCustomerDTO(item));
                }
                return lstCustomerDTO;
            }
            else
                return new List<CustomerDTO>();
        }
        #endregion
        #region FindAll
        public List<CustomerDTO> FindAll(int APageNo, CustomerDTO ACustomerDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "RUT",FilterValue = ACustomerDTO.RUT, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "CustomerType",FilterValue = ACustomerDTO.CustomerType,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "Name", FilterValue = ACustomerDTO.Name,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode,Operator = "Equals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "UR", Operator = "NotEquals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "PR", Operator = "NotEquals"}
                    };

            Expression<Func<Customer, bool>> expression = Utils.GetExpression<Customer>(lstExpression);
            int iRecordCount = objCustomerRepository.GetRecordCount(expression);
            var vCustomerDTO = objCustomerRepository.GetFiltered(0, iRecordCount, expression);

            if (vCustomerDTO != null && vCustomerDTO.Any())
            {
                List<CustomerDTO> lstCustomerDTO = new List<CustomerDTO>();
                foreach (var item in vCustomerDTO)
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
        #region FindByCustomerId
        public CustomerDTO FindByCustomerId(int ACustomerId)
        {

            CustomerDTO objCustomerDTO = new CustomerDTO();

            var customerResult = from customer in objCustomerRepository.AllMatching(x => x.CustomerId == ACustomerId)
                                 join parentCustomer in objCustomerRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode)
                                 on customer.ParentCustomerId equals parentCustomer.CustomerId into result
                                 from parentCustomer in result.DefaultIfEmpty()
                                 select new { customer, ParentCustomerName = parentCustomer == null ? string.Empty : string.Concat(parentCustomer.Name, " ", parentCustomer.PaternalSurname) };

            if (customerResult.Any())
            {
                if (customerResult.FirstOrDefault().customer != null)
                {
                    Customer customer = ((Customer)customerResult.FirstOrDefault().customer);
                    //objCustomerRepository.UnitOfWork.RefreshDataInModel(ref customer);

                    if (customer.Product == null)
                    {
                        customer.Product = objProductRepository.Get(customer.OrganizationCode, customer.ProductCode);
                    }
                    if (customer.Country == null)
                    {
                        customer.Country = objCountryRepository.Get(customer.Nationality);
                    }
                    if (customer.Branch == null)
                    {
                        customer.Branch = objBranchRepository.Get(customer.BranchCode, customer.OrganizationCode);
                    }
                    if (customer.ProductMemberType == null)
                    {
                        customer.ProductMemberType = objProductMemberTypeRepository.Get(customer.ProductMemberTypeCode);
                    }
                    objCustomerDTO = Conversion.Mapping.CustomerToCustomerDTO(customer);
                    objCustomerDTO.ParentCustomerName = customerResult.FirstOrDefault().ParentCustomerName;
                }
            }
            return objCustomerDTO;
        }
        #endregion
        #region ValidateCustomerDesignationForHRP
        public bool ValidateCustomerDesignationForHRP(string ADesignationCode)
        {
            bool isHRP = false;
            var result = objDesignationRepository.Get(ADesignationCode, Utils.OrganizationCode);
            if (result != null)
            {
                isHRP = result.HRP;
            }
            return isHRP;
        }
        #endregion
        #region ValidateCreditScore
        public bool ValidateCreditScore(string AProductCode, decimal ACreditScore)
        {
            bool isCreditScoreViolated = false;
            var product = objProductRepository.Get(Utils.OrganizationCode, AProductCode);
            if (product != null)
            {
                isCreditScoreViolated = ACreditScore < product.CombinedScore;
            }
            return isCreditScoreViolated;
        }
        #endregion
        #region GetParentCustomers
        public List<RUTCardNumberSearchDTO> GetParentCustomers(string ARUT, string ACardNumber, string ACustomerName)
        {
            List<RUTCardNumberSearchDTO> objRUTCardNumberSearchDTO = new List<RUTCardNumberSearchDTO>();

            SqlParameter[] objSqlParameter = new SqlParameter[] { 
                new SqlParameter(){ ParameterName = "OrganizationCode", Value = Utils.OrganizationCode, SqlDbType = System.Data.SqlDbType.VarChar },
                new SqlParameter(){ ParameterName = "RUT", Value = ARUT == null ? "" : ARUT.Replace("-", "") , SqlDbType = System.Data.SqlDbType.VarChar },
                new SqlParameter(){ ParameterName ="CardNumber", Value = ACardNumber, SqlDbType = System.Data.SqlDbType.VarChar },
                new SqlParameter(){ ParameterName = "CustomerName", Value = ACustomerName, SqlDbType = System.Data.SqlDbType.VarChar }
            };

            objRUTCardNumberSearchDTO = objCardMasterRepository.UnitOfWork.ExecuteQuery<RUTCardNumberSearchDTO>("PR_GetParentCustomers @OrganizationCode,@RUT,@CardNumber,@CustomerName", objSqlParameter).ToList();
            return objRUTCardNumberSearchDTO;
        }
        #endregion
       
        #region GetAutoApprovalPropertyOfProduct
        public bool GetAutoApprovalPropertyOfProduct(string AProductCode)
        {
            return objProductRepository.Get(Utils.OrganizationCode, AProductCode).AutoApprovalFlag;
        }
        #endregion
        #endregion

        #region CustomerUnderwriting
        #region ListCustomersToUnderwrite
        public List<CustomerDTO> ListCustomersToUnderwrite(CustomerDTO ACustomerDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "RUT", FilterValue = ACustomerDTO.RUT, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "CustomerType", FilterValue = ACustomerDTO.CustomerType, Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "Name", FilterValue = ACustomerDTO.Name, Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode", FilterValue = Utils.OrganizationCode, Operator = "Equals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus", FilterValue = "UR", Operator = "Equals"}                      
                    };

            Expression<Func<Customer, bool>> expression = Utils.GetExpression<Customer>(lstExpression);
            int iRecordCount = objCustomerRepository.GetRecordCount(expression);
            var vCustomerDTO = objCustomerRepository.GetFiltered(0, iRecordCount, expression);

            if (vCustomerDTO != null && vCustomerDTO.Any())
            {
                List<CustomerDTO> lstCustomerDTO = new List<CustomerDTO>();
                foreach (var item in vCustomerDTO)
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

        #region AuthorizationByUnderwriter
        public int AuthorizationByUnderwriter(int ACustomerId, int ACreditScore, decimal ACreditLimit, string ARemarks, string AApprovalStatus, string AApprovedBy, string AReasonCode, string AReasonDescription)
        {
            int retValue;

            SqlParameter[] objSqlParameter = new SqlParameter[] { 
                new SqlParameter() { ParameterName = "@CustomerId", Value = ACustomerId, SqlDbType = System.Data.SqlDbType.Int },                
                new SqlParameter() { ParameterName = "@CreditScore", Value = ACreditScore, SqlDbType = System.Data.SqlDbType.Int },
                new SqlParameter() { ParameterName = "@CreditLimit", Value = ACreditLimit, SqlDbType = System.Data.SqlDbType.Decimal },
                new SqlParameter() { ParameterName = "@Remarks", Value = ARemarks, SqlDbType = System.Data.SqlDbType.VarChar},
                new SqlParameter() { ParameterName = "@ApprovalStatus", Value = AApprovalStatus, SqlDbType = System.Data.SqlDbType.VarChar },
                new SqlParameter() { ParameterName = "@ApprovedBy", Value = AApprovedBy, SqlDbType = System.Data.SqlDbType.VarChar },
                new SqlParameter() { ParameterName = "@ReasonCode", Value = AReasonCode, SqlDbType = System.Data.SqlDbType.VarChar },
                new SqlParameter() { ParameterName = "@ReasonDescription", Value = AReasonDescription, SqlDbType = System.Data.SqlDbType.VarChar },
                new SqlParameter() { ParameterName = "@OrganizationType", Value = Utils.OrganizationTypeCode, SqlDbType = System.Data.SqlDbType.VarChar }
            };

            retValue = objCustomerRepository.UnitOfWork.ExecuteCommand("PR_AuthorizeCustomer @CustomerId,@CreditScore,@CreditLimit,@Remarks,@ApprovalStatus,@ApprovedBy,@ReasonCode,@ReasonDescription,@OrganizationType", objSqlParameter);
            return retValue;
        }
        #endregion
        #endregion

        #region CalendarDateMaintenance

        #region SaveCalendarDateMaintenanceDetails
        public void SaveCalendarDateMaintenanceDetails(CalendarDateMaintenanceDTO ACalendarDateMaintenanceDTO)
        {
            if (ACalendarDateMaintenanceDTO == null)
            {
                throw new ArgumentException("");
            }

            var newCalendarDateMaintenance = CalendarDateMaintenanceFactory.CreateCalendarDateMaintenance(ACalendarDateMaintenanceDTO.Id,
                                ACalendarDateMaintenanceDTO.OrganizationCode, ACalendarDateMaintenanceDTO.Type, ACalendarDateMaintenanceDTO.Date,
                                ACalendarDateMaintenanceDTO.Description, ACalendarDateMaintenanceDTO.CreatedBy,
                                ACalendarDateMaintenanceDTO.LastUpdatedBy, ACalendarDateMaintenanceDTO.ApprovalStatus, null);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objCalendarDateRepository.Add(newCalendarDateMaintenance);
                        objCalendarDateRepository.UnitOfWork.Commit();
                        if (ACalendarDateMaintenanceDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_CalendarDateMaintenance", " Id = '" + newCalendarDateMaintenance.Id + "'",
                                "CLNDR", ACalendarDateMaintenanceDTO.CreatedBy, "/Master/CalendarDateMaintenanceAuthorize/" + newCalendarDateMaintenance.Id + "/A?", "MT");

                            objCalendarDateRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
        #region UpdateCalendarDateMaintenanceDetails
        public void UpdateCalendarDateMaintenanceDetails(int AId, CalendarDateMaintenanceDTO ACalendarDateMaintenanceDTO)
        {
            if (ACalendarDateMaintenanceDTO == null)
            {
                throw new ArgumentException("");
            }

            CalendarDateMaintenance currentCalendarDateMaintenance = objCalendarDateRepository.Get(AId);
            CalendarDateMaintenance updatedCalendarDateMaintenance = new CalendarDateMaintenance()
            {
                Id = AId,
                OrganizationCode = ACalendarDateMaintenanceDTO.OrganizationCode,
                Type = ACalendarDateMaintenanceDTO.Type,
                Date = ACalendarDateMaintenanceDTO.Date,
                Description = ACalendarDateMaintenanceDTO.Description,

                CreatedBy = currentCalendarDateMaintenance.CreatedBy,
                CreatedOn = currentCalendarDateMaintenance.CreatedOn,
                LastUpdatedBy = ACalendarDateMaintenanceDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate,
                ApprovalStatus = ACalendarDateMaintenanceDTO.ApprovalStatus
            };
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objCalendarDateRepository.Merge(currentCalendarDateMaintenance, updatedCalendarDateMaintenance);
                    objCalendarDateRepository.UnitOfWork.Commit();
                    if (ACalendarDateMaintenanceDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_CalendarDateMaintenance", " Id = '" + AId + "'",
                                "CLNDR", ACalendarDateMaintenanceDTO.LastUpdatedBy, "/Master/CalendarDateMaintenanceAuthorize/" + AId + "/A?", "MT");
                        objCalendarDateRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #region DeleteCalendarDateMaintenance
        public void DeleteCalendarDateMaintenance(int AId)
        {
            var calendarDateMaintenanceId = objCalendarDateRepository.Get(AId);
            if (calendarDateMaintenanceId != null)
            {
                objCalendarDateRepository.Remove(calendarDateMaintenanceId);
                objCalendarDateRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindCalendarDateMaintenance
        public List<CalendarDateMaintenanceDTO> FindCalendarDateMaintenance(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objCalendarDateRepository.GetRecordCount();
            var calendarDateMaintenance = objCalendarDateRepository.GetAll();
            if (calendarDateMaintenance != null && calendarDateMaintenance.Any())
            {
                List<CalendarDateMaintenanceDTO> lstCalendarDateMaintenanceDTO = new List<CalendarDateMaintenanceDTO>();
                foreach (var item in calendarDateMaintenance)
                {
                    lstCalendarDateMaintenanceDTO.Add(Conversion.Mapping.CalendarDateMaintenanceToCalendarDateMaintenanceDTO(item));
                }
                return lstCalendarDateMaintenanceDTO;
            }
            else
                return new List<CalendarDateMaintenanceDTO>();
        }
        #endregion
        #region FindAll
        public List<CalendarDateMaintenanceDTO> FindAll(int APageNo, CalendarDateMaintenanceDTO ACalendarDateMaintenanceDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "Type",FilterValue = ACalendarDateMaintenanceDTO.Type, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "Description",FilterValue = ACalendarDateMaintenanceDTO.Description,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode,Operator = "Equals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
                    };

            Expression<Func<CalendarDateMaintenance, bool>> expression = Utils.GetExpression<CalendarDateMaintenance>(lstExpression);
            int iRecordCount = objCalendarDateRepository.GetRecordCount(expression);

            //var vCalendarDateDTO = objCalendarDateRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vCalendarDateDTO = objCalendarDateRepository.GetFiltered(0, iRecordCount, expression);

            if (vCalendarDateDTO != null && vCalendarDateDTO.Any())
            {
                List<CalendarDateMaintenanceDTO> lstCalendarDateMaintenanceDTO = new List<CalendarDateMaintenanceDTO>();
                foreach (var item in vCalendarDateDTO)
                {
                    lstCalendarDateMaintenanceDTO.Add(Conversion.Mapping.CalendarDateMaintenanceToCalendarDateMaintenanceDTO(item));
                }
                return lstCalendarDateMaintenanceDTO;
            }
            else
            {
                return new List<CalendarDateMaintenanceDTO>();
            }
        }
        #endregion
        #region FindByCalendarDateMaintenance
        public CalendarDateMaintenanceDTO FindByCalendarDateMaintenance(int AId)
        {
            CalendarDateMaintenance calendarDateMaintenance = objCalendarDateRepository.Get(AId);
            if (calendarDateMaintenance != null)
            {
                objCalendarDateRepository.UnitOfWork.RefreshDataInModel(ref calendarDateMaintenance);
                return Conversion.Mapping.CalendarDateMaintenanceToCalendarDateMaintenanceDTO(calendarDateMaintenance);
            }
            else
            {
                return new CalendarDateMaintenanceDTO();
            }
        }
        #endregion

        #endregion

        #region TransactionSubCode

        #region SaveTransactionSubCodeDetails
        public void SaveTransactionSubCodeDetails(TransactionSubCodeDTO ATransactionSubCodeDTO)
        {
            if (ATransactionSubCodeDTO == null)
            {
                throw new ArgumentException("");
            }

            var newTransactionSubCode = TransactionSubCodeFactory.CreateTransactionSubCode
                (
                    ATransactionSubCodeDTO.TransactionSubCodeId, ATransactionSubCodeDTO.OrganizationCode, ATransactionSubCodeDTO.TransactionCode, ATransactionSubCodeDTO.SubCode,
                    ATransactionSubCodeDTO.Description, ATransactionSubCodeDTO.CreditSubSegmentCode, ATransactionSubCodeDTO.MinDuePercentage,
                    ATransactionSubCodeDTO.IsConsumption, ATransactionSubCodeDTO.IsDebitOrCredit, ATransactionSubCodeDTO.IsIVARequired,
                    ATransactionSubCodeDTO.IncomingTransType, ATransactionSubCodeDTO.FeeType,
                    ATransactionSubCodeDTO.CreatedBy, ATransactionSubCodeDTO.LastUpdatedBy, ATransactionSubCodeDTO.ApprovalStatus, null
                );

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objTransactionSubCodeRepository.Add(newTransactionSubCode);
                        objTransactionSubCodeRepository.UnitOfWork.Commit();
                        if (ATransactionSubCodeDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_TransactionSubCode", " TransactionSubCodeId = '" + newTransactionSubCode.TransactionSubCodeId + "'",
                                "TRNSUBCOD", ATransactionSubCodeDTO.CreatedBy, "/Master/TransactionSubCodeAuthorize/" + newTransactionSubCode.TransactionSubCodeId + "/A?", "MT");
                            objTransactionSubCodeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region UpdateTransactionSubCodeDetails
        public void UpdateTransactionSubCodeDetails(int ATransactionSubCodeId, TransactionSubCodeDTO ATransactionSubCodeDTO)
        {
            if (ATransactionSubCodeDTO == null)
            {
                throw new ArgumentException("");
            }

            TransactionSubCode currentTransactionSubCode = objTransactionSubCodeRepository.Get(ATransactionSubCodeId);
            TransactionSubCode updatedTransactionSubCode = new TransactionSubCode()
            {
                TransactionSubCodeId = ATransactionSubCodeId,
                OrganizationCode = currentTransactionSubCode.OrganizationCode,
                TransactionCode = ATransactionSubCodeDTO.TransactionCode,
                SubCode = ATransactionSubCodeDTO.SubCode,
                Description = ATransactionSubCodeDTO.Description,
                CreditSubSegmentCode = ATransactionSubCodeDTO.CreditSubSegmentCode,
                MinDuePercentage = ATransactionSubCodeDTO.MinDuePercentage,
                IsConsumption = ATransactionSubCodeDTO.IsConsumption,
                IsDebitOrCredit = ATransactionSubCodeDTO.IsDebitOrCredit,
                IsIVARequired = ATransactionSubCodeDTO.IsIVARequired,
                IncomingTransType = ATransactionSubCodeDTO.IncomingTransType,
                FeeType = ATransactionSubCodeDTO.FeeType,

                ApprovalStatus = ATransactionSubCodeDTO.ApprovalStatus,
                CreatedBy = currentTransactionSubCode.CreatedBy,
                CreatedOn = currentTransactionSubCode.CreatedOn,
                LastUpdatedBy = ATransactionSubCodeDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objTransactionSubCodeRepository.Merge(currentTransactionSubCode, updatedTransactionSubCode);
                    objTransactionSubCodeRepository.UnitOfWork.Commit();
                    if (ATransactionSubCodeDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_TransactionSubCode", " TransactionSubCodeId = '" + ATransactionSubCodeId + "'",
                                "TRNSUBCOD", ATransactionSubCodeDTO.LastUpdatedBy, "/Master/TransactionSubCodeAuthorize/" + ATransactionSubCodeId + "/A?", "MT");
                        objTransactionSubCodeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
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

        #region DeleteTransactionSubCode
        public void DeleteTransactionSubCode(int ATransactionSubCodeId)
        {
            var transactionSubCode = objTransactionSubCodeRepository.Get(ATransactionSubCodeId);
            if (transactionSubCode != null)
            {
                objTransactionSubCodeRepository.Remove(transactionSubCode);
                objTransactionSubCodeRepository.UnitOfWork.Commit();
            }
        }
        #endregion

        #region FindTransactionSubCode
        public List<TransactionSubCodeDTO> FindTransactionSubCode(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objTransactionSubCodeRepository.GetRecordCount();
            var transactionSubCode = objTransactionSubCodeRepository.GetAll();
            if (transactionSubCode != null && transactionSubCode.Any())
            {
                List<TransactionSubCodeDTO> lstTransactionSubCodeDTO = new List<TransactionSubCodeDTO>();
                foreach (var item in transactionSubCode)
                {
                    lstTransactionSubCodeDTO.Add(Conversion.Mapping.TransactionSubCodeToTransactionSubCodeDTO(item));
                }
                return lstTransactionSubCodeDTO;
            }
            else
                return new List<TransactionSubCodeDTO>();
        }
        #endregion

        #region FindAll
        public List<TransactionSubCodeDTO> FindAll(int APageNo, TransactionSubCodeDTO ATransactionSubCodeDTO)
        {

            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "SubCode", FilterValue = ATransactionSubCodeDTO.SubCode, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "Description", FilterValue = ATransactionSubCodeDTO.Description, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "TransactionCode", FilterValue = ATransactionSubCodeDTO.TransactionCode, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "IsDebitOrCredit", FilterValue = ATransactionSubCodeDTO.IsDebitOrCredit, Operator = "Contains"}, 
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode, Operator = "Contains"}
            };

            Expression<Func<TransactionSubCode, bool>> expression = Utils.GetExpression<TransactionSubCode>(lstExp);
            int iRecordCount = objTransactionSubCodeRepository.GetRecordCount(expression);

            //var vTransactionSubCodeDTO = objTransactionSubCodeRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vTransactionSubCodeDTO = objTransactionSubCodeRepository.GetFiltered(0, iRecordCount, expression);

            if (vTransactionSubCodeDTO != null && vTransactionSubCodeDTO.Any())
            {
                List<TransactionSubCodeDTO> lstTransactionSubCodeDTO = new List<TransactionSubCodeDTO>();
                foreach (var item in vTransactionSubCodeDTO)
                {
                    lstTransactionSubCodeDTO.Add(Conversion.Mapping.TransactionSubCodeToTransactionSubCodeDTO(item));
                }
                return lstTransactionSubCodeDTO;
            }
            else
                return new List<TransactionSubCodeDTO>();

        }
        #endregion

        #region FindByTransactionSubCodeId
        public TransactionSubCodeDTO FindByTransactionSubCode(int ATransactionSubCodeId)
        {
            TransactionSubCode transactionSubCode = objTransactionSubCodeRepository.Get(ATransactionSubCodeId);
            if (transactionSubCode != null)
            {
                objTransactionSubCodeRepository.UnitOfWork.RefreshDataInModel(ref transactionSubCode);
                if (transactionSubCode.Transaction == null)
                {
                    transactionSubCode.Transaction = objTransactionRepository.Get(Utils.OrganizationCode, transactionSubCode.TransactionCode);
                }
                return Conversion.Mapping.TransactionSubCodeToTransactionSubCodeDTO(transactionSubCode);
            }
            else
            {
                return new TransactionSubCodeDTO();
            }
        }
        #endregion

        #endregion //** end of System Parameter Region **//

        #region FinancialAccount


        public List<FinancialAccountDTO> FindFinancialAccount(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindFinancialAccount");

            int iRecordCount = objFinancialAccountRepository.GetRecordCount();
            var financialAccounts = objFinancialAccountRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);
            if (financialAccounts != null && financialAccounts.Any())
            {
                List<FinancialAccountDTO> lstFinancialAccountDTO = new List<FinancialAccountDTO>();
                foreach (var financialAccount in financialAccounts)
                {
                    lstFinancialAccountDTO.Add(Conversion.Mapping.FinancialAccountToFinancialAccountDTO(financialAccount));
                }
                return lstFinancialAccountDTO;
            }
            else
                return new List<FinancialAccountDTO>();
        }

        public void DeleteFinancialAccount(int AFinancialAccountId)
        {
            var financialAccount = objFinancialAccountRepository.Get(AFinancialAccountId);

            if (financialAccount != null)
            {
                objFinancialAccountRepository.Remove(financialAccount);
                objFinancialAccountRepository.UnitOfWork.Commit();
            }
        }

        public FinancialAccountDTO FindByFinancialAccountId(int AId)
        {
            var financialAccount = objFinancialAccountRepository.Get(AId);
            if (financialAccount != null)
            {
                if (financialAccount.Branch == null)
                {
                    financialAccount.Branch = objBranchRepository.Get(financialAccount.BranchCode, financialAccount.OrganizationCode);
                }
                if (financialAccount.Product == null)
                {
                    financialAccount.Product = objProductRepository.Get(financialAccount.OrganizationCode, financialAccount.ProductCode);
                }
                if (financialAccount.TransactionSubCode == null)
                {
                    financialAccount.TransactionSubCode = objTransactionSubCodeRepository.Get(financialAccount.TransactionSubCodeId);
                }

                objFinancialAccountRepository.UnitOfWork.RefreshDataInModel(ref financialAccount);
                return Conversion.Mapping.FinancialAccountToFinancialAccountDTO(financialAccount);
            }
            else
            {
                return new FinancialAccountDTO();
            }
        }

        public void SaveFinancialAccountDetails(FinancialAccountDTO AFinancialAccountDTO)
        {
            if (AFinancialAccountDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddCurrencyWithNullInformation");

            var newFinancialAccount = FinancialAccountFactory.CreateFinancialAccount(AFinancialAccountDTO.FinancialAccountId, AFinancialAccountDTO.BranchCode, AFinancialAccountDTO.ModuleName,
                AFinancialAccountDTO.ProductCode, AFinancialAccountDTO.TransactionSubCodeId, AFinancialAccountDTO.DebitAccountNr,
                AFinancialAccountDTO.DebitAccountCode, AFinancialAccountDTO.DebitSubTransactionCode, AFinancialAccountDTO.DebitNarration,
                AFinancialAccountDTO.IsDebitConsolidated, AFinancialAccountDTO.CreditAccountNr, AFinancialAccountDTO.CreditAccountCode,
                AFinancialAccountDTO.CreditSubTransactionCode, AFinancialAccountDTO.IsCreditConsolidated, AFinancialAccountDTO.CreditNarration, AFinancialAccountDTO.EffectiveDate, AFinancialAccountDTO.Portfolio,
                AFinancialAccountDTO.CreatedBy, AFinancialAccountDTO.LastUpdatedBy, AFinancialAccountDTO.ApprovalStatus, AFinancialAccountDTO.OrganizationCode);


            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objFinancialAccountRepository.Add(newFinancialAccount);
                        objFinancialAccountRepository.UnitOfWork.Commit();
                        if (AFinancialAccountDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_FinancialAccount", " FinancialAccountId = '" + newFinancialAccount.FinancialAccountId + "'",
                                "FNACNT", AFinancialAccountDTO.CreatedBy, "/Master/FinancialAccountAuthorize/" + newFinancialAccount.FinancialAccountId + "/A?", "MT");

                            objFinancialAccountRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateFinancialAccountDetails(int AFinancialAccountId, FinancialAccountDTO AFinancialAccountDTO)
        {
            if (AFinancialAccountDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");

            }
            FinancialAccount currentFinancialAccount = objFinancialAccountRepository.Get(AFinancialAccountId);
            FinancialAccount updatedFinancialAccount = new FinancialAccount()
            {
                FinancialAccountId = AFinancialAccountId,
                BranchCode = AFinancialAccountDTO.BranchCode,
                ModuleName = AFinancialAccountDTO.ModuleName,
                ProductCode = AFinancialAccountDTO.ProductCode,
                TransactionSubCodeId = AFinancialAccountDTO.TransactionSubCodeId,
                DebitAccountNr = AFinancialAccountDTO.DebitAccountNr,
                DebitAccountCode = AFinancialAccountDTO.DebitAccountCode,
                DebitSubTransactionCode = AFinancialAccountDTO.DebitSubTransactionCode,
                DebitNarration = AFinancialAccountDTO.DebitNarration,
                IsDebitConsolidated = AFinancialAccountDTO.IsDebitConsolidated,
                CreditAccountNr = AFinancialAccountDTO.CreditAccountNr,
                CreditAccountCode = AFinancialAccountDTO.CreditAccountCode,
                CreditSubTransactionCode = AFinancialAccountDTO.CreditSubTransactionCode,
                IsCreditConsolidated = AFinancialAccountDTO.IsCreditConsolidated,
                CreditNarration = AFinancialAccountDTO.CreditNarration,
                OrganizationCode = currentFinancialAccount.OrganizationCode,
                EffectiveDate = AFinancialAccountDTO.EffectiveDate,
                Portfolio = AFinancialAccountDTO.Portfolio,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = AFinancialAccountDTO.LastUpdatedBy,
                CreatedOn = currentFinancialAccount.CreatedOn,
                CreatedBy = currentFinancialAccount.CreatedBy,
                ApprovalStatus = AFinancialAccountDTO.ApprovalStatus
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objFinancialAccountRepository.Merge(currentFinancialAccount, updatedFinancialAccount);
                    objFinancialAccountRepository.UnitOfWork.Commit();
                    if (AFinancialAccountDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_FinancialAccount", " FinancialAccountId = '" + AFinancialAccountId + "'",
                                "FNACNT", AFinancialAccountDTO.LastUpdatedBy, "/Master/FinancialAccountAuthorize/" + AFinancialAccountId + "/A?", "MT");
                        objFinancialAccountRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<FinancialAccountDTO> FindAll(int APageNo, FinancialAccountDTO AFinancialAccountDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "BranchCode", FilterValue = AFinancialAccountDTO.BranchCode, Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "ModuleName", FilterValue = AFinancialAccountDTO.ModuleName, Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "ProductCode", FilterValue = AFinancialAccountDTO.ProductCode, Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "TransactionSubCodeId", FilterValue = AFinancialAccountDTO.TransactionSubCodeId, Operator =  AFinancialAccountDTO.TransactionSubCodeId != 0 ? "Equals" : "NotEquals"},
                new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode,Operator = "Equals"},
                new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
            };

            Expression<Func<FinancialAccount, bool>> expression = Utils.GetExpression<FinancialAccount>(lstExp);
            int iRecordCount = objFinancialAccountRepository.GetRecordCount(expression);
            var vFinancialAccountDTO = objFinancialAccountRepository.GetFiltered(0, iRecordCount, expression);

            if (vFinancialAccountDTO != null && vFinancialAccountDTO.Any())
            {
                List<FinancialAccountDTO> lstFinancialAccountDTO = new List<FinancialAccountDTO>();
                foreach (var financialAccount in vFinancialAccountDTO)
                {
                    lstFinancialAccountDTO.Add(Conversion.Mapping.FinancialAccountToFinancialAccountDTO(financialAccount));
                }
                return lstFinancialAccountDTO;
            }
            else
                return new List<FinancialAccountDTO>();
        }


        #endregion

        #region BillingCycleParameter

        public List<BillingCycleParameterDTO> FindAll(int APageNo, BillingCycleParameterDTO ABillingCycleParameterDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "BillMonth", FilterValue = ABillingCycleParameterDTO.BillMonth, Operator = ABillingCycleParameterDTO.BillMonth == 0 ? "NotEquals" : "Equals"},
                  new Utils.ExpressionList(){FilterColumn = "BillCycleSysParamId", FilterValue = ABillingCycleParameterDTO.BillCycleSysParamId, Operator = ABillingCycleParameterDTO.BillCycleSysParamId == 0 ? "NotEquals" : "Equals"},
                  new Utils.ExpressionList(){FilterColumn = "OrganizationCode", FilterValue = Utils.OrganizationCode, Operator = "Contains"},
            };

            Expression<Func<BillingCycleParameter, bool>> expression = Utils.GetExpression<BillingCycleParameter>(lstExp);
            int iRecordCount = objBillingCycleParameterRepository.GetRecordCount(expression);

            //var vBillingCycleParameterDTO = objBillingCycleParameterRepository.GetFiltered(APageNo, iRecordCount, expression);

            var vBillingCycleParameterDTO = objBillingCycleParameterRepository.GetFiltered(0, iRecordCount, expression);

            if (vBillingCycleParameterDTO != null && vBillingCycleParameterDTO.Any())
            {
                List<BillingCycleParameterDTO> lstBillingCycleParameterDTO = new List<BillingCycleParameterDTO>();
                foreach (var billingCycleParameter in vBillingCycleParameterDTO)
                {
                    lstBillingCycleParameterDTO.Add(Conversion.Mapping.BillingCycleParameterToBillingCycleParameterDTO(billingCycleParameter));
                }
                return lstBillingCycleParameterDTO;
            }
            else
                return new List<BillingCycleParameterDTO>();
        }

        public BillingCycleParameterDTO FindByBillingCycleParameterId(int AId)
        {
            var billingCycleParameter = objBillingCycleParameterRepository.Get(AId);

            if (billingCycleParameter != null)
            {
                if (billingCycleParameter.BillingCycle == null)
                {
                    billingCycleParameter.BillingCycle = objSystemParameterRepository.Get(billingCycleParameter.BillCycleSysParamId);
                }
                objBillingCycleParameterRepository.UnitOfWork.RefreshDataInModel(ref billingCycleParameter);
                return Conversion.Mapping.BillingCycleParameterToBillingCycleParameterDTO(billingCycleParameter);
            }
            else
            {
                return new BillingCycleParameterDTO();
            }
        }

        public List<BillingCycleParameterDTO> FindBillingCycleParameter(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindBillingCycleParameter");

            int iRecordCount = objBillingCycleParameterRepository.GetRecordCount();
            var billingCycleParameters = objBillingCycleParameterRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);
            if (billingCycleParameters != null && billingCycleParameters.Any())
            {
                List<BillingCycleParameterDTO> lstBillingCycleParameterDTO = new List<BillingCycleParameterDTO>();
                foreach (var billingCycleParameter in billingCycleParameters)
                {
                    lstBillingCycleParameterDTO.Add(Conversion.Mapping.BillingCycleParameterToBillingCycleParameterDTO(billingCycleParameter));
                }
                return lstBillingCycleParameterDTO;
            }
            else
                return new List<BillingCycleParameterDTO>();
        }

        public void DeleteBillingCycleParameter(int ABillingCycleParameterId)
        {
            var billingCycleParameter = objBillingCycleParameterRepository.Get(ABillingCycleParameterId);

            if (billingCycleParameter != null)
            {
                objBillingCycleParameterRepository.Remove(billingCycleParameter);
                objBillingCycleParameterRepository.UnitOfWork.Commit();
            }
        }

        public void SaveBillingCycleParameterDetails(BillingCycleParameterDTO ABillingCycleParameterDTO)
        {
            if (ABillingCycleParameterDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddCurrencyWithNullInformation");

            SystemParameter objSystemParameter = objSystemParameterRepository.Get(ABillingCycleParameterDTO.BillCycleSysParamId);

            ABillingCycleParameterDTO.BillingCycleCode = objSystemParameter.DefaultValue;

            var newBillingCycleParameter = BillingCycleParameterFactory.CreateBillingCycleParameter(ABillingCycleParameterDTO.BillingCycleParameterId,
                ABillingCycleParameterDTO.BillMonth, ABillingCycleParameterDTO.BillYear, ABillingCycleParameterDTO.BillCycleSysParamId, ABillingCycleParameterDTO.BillingCycleCode, ABillingCycleParameterDTO.BillDate,
                ABillingCycleParameterDTO.StatementDate, ABillingCycleParameterDTO.PaymentDueDate, ABillingCycleParameterDTO.CreatedBy,
                ABillingCycleParameterDTO.LastUpdatedBy, ABillingCycleParameterDTO.ApprovalStatus, ABillingCycleParameterDTO.OrganizationCode,
                ABillingCycleParameterDTO.BillFrom, ABillingCycleParameterDTO.BillTo
                );

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objBillingCycleParameterRepository.Add(newBillingCycleParameter);
                        objBillingCycleParameterRepository.UnitOfWork.Commit();
                        if (ABillingCycleParameterDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_BillingCycleParameter", " BillingCycleParameterId = '" + newBillingCycleParameter.BillingCycleParameterId + "'",
                                "BLCYCPAR", ABillingCycleParameterDTO.CreatedBy, "/Master/BillingCycleParameterAuthorize/" + newBillingCycleParameter.BillingCycleParameterId + "/A?", "MT");
                            objBillingCycleParameterRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateBillingCycleParameterDetails(int ABillingCycleParameterId, BillingCycleParameterDTO ABillingCycleParameterDTO)
        {
            if (ABillingCycleParameterDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");

            }

            SystemParameter objSystemParameter = objSystemParameterRepository.Get(ABillingCycleParameterDTO.BillCycleSysParamId);

            ABillingCycleParameterDTO.BillingCycleCode = objSystemParameter.DefaultValue;

            BillingCycleParameter currentBillingCycleParameter = objBillingCycleParameterRepository.Get(ABillingCycleParameterId);
            BillingCycleParameter updatedBillingCycleParameter = new BillingCycleParameter()
            {
                BillingCycleParameterId = ABillingCycleParameterId,
                BillMonth = ABillingCycleParameterDTO.BillMonth,
                BillYear = ABillingCycleParameterDTO.BillYear,
                BillCycleSysParamId = ABillingCycleParameterDTO.BillCycleSysParamId,
                BillingCycleCode = ABillingCycleParameterDTO.BillingCycleCode,
                BillDate = ABillingCycleParameterDTO.BillDate,
                StatementDate = ABillingCycleParameterDTO.StatementDate,
                PaymentDueDate = ABillingCycleParameterDTO.PaymentDueDate,
                OrganizationCode = currentBillingCycleParameter.OrganizationCode,
                BillFrom = ABillingCycleParameterDTO.BillFrom,
                BillTo = ABillingCycleParameterDTO.BillTo,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = ABillingCycleParameterDTO.LastUpdatedBy,
                CreatedOn = currentBillingCycleParameter.CreatedOn,
                CreatedBy = currentBillingCycleParameter.CreatedBy,
                ApprovalStatus = currentBillingCycleParameter.ApprovalStatus == "A" ? "A" : ABillingCycleParameterDTO.ApprovalStatus
            };

            if (currentBillingCycleParameter.ApprovalStatus == "A")
            {
                ABillingCycleParameterDTO.ApprovalStatus = "A";
            }

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objBillingCycleParameterRepository.Merge(currentBillingCycleParameter, updatedBillingCycleParameter);
                    objBillingCycleParameterRepository.UnitOfWork.Commit();
                    if (ABillingCycleParameterDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_BillingCycleParameter", " BillingCycleParameterId = '" + ABillingCycleParameterId + "'",
                                "BLCYCPAR", ABillingCycleParameterDTO.LastUpdatedBy, "/Master/BillingCycleParameterAuthorize/" + ABillingCycleParameterId + "/A?", "MT");

                        objBillingCycleParameterRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DateTime GetWorkingDate(int AMonth, int AYear, string AType)
        {
            DateTime dt = new DateTime();
            SqlParameter[] paramCollection = new SqlParameter[] { 
                           
                            new SqlParameter(){ ParameterName = "@P_OrganizationCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = Utils.OrganizationCode, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ ParameterName = "@P_Month", SqlDbType = System.Data.SqlDbType .Int , Value = AMonth, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ ParameterName = "@P_Year", SqlDbType = System.Data.SqlDbType.Int, Value = AYear, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ ParameterName = "@P_Type", SqlDbType = System.Data.SqlDbType.VarChar , Value = AType, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ParameterName = "@P_RetDate", SqlDbType = System.Data.SqlDbType.Date, Direction = System.Data.ParameterDirection.Output, Size = 25}
                        };

            objBillingCycleParameterRepository.UnitOfWork.ExecuteCommand("PR_GetWorkingDay  @P_OrganizationCode, @P_Month, @P_Year,@P_Type, @P_RetDate OUTPUT", paramCollection);
            //var test = objBillingCycleParameterRepository.UnitOfWork.ExecuteSqlQuery(System.Type.GetType("DateTime"), "select dbo.FN_WorkingDay( @P_Date, @P_OrganizationCode)", paramCollection);
            dt = Convert.ToDateTime(paramCollection[4].Value);
            return dt;
        }


        public DateTime GetNextWorkingDate(DateTime ADate)
        {
            DateTime dt = new DateTime();
            SqlParameter[] paramCollection = new SqlParameter[] { 
                            new SqlParameter(){ ParameterName = "@P_Date", SqlDbType = System.Data.SqlDbType.Date, Value = ADate.Date, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ ParameterName = "@P_OrganizationCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = Utils.OrganizationCode, Direction = System.Data.ParameterDirection.Input }
                            , new SqlParameter(){ParameterName = "@P_RetDate", SqlDbType = System.Data.SqlDbType.Date, Direction = System.Data.ParameterDirection.Output, Size = 25}



                        };

            objBillingCycleParameterRepository.UnitOfWork.ExecuteCommand("PR_GetNextWorkingDay @P_Date, @P_OrganizationCode, @P_RetDate OUTPUT", paramCollection);
            //var test = objBillingCycleParameterRepository.UnitOfWork.ExecuteSqlQuery(System.Type.GetType("DateTime"), "select dbo.FN_WorkingDay( @P_Date, @P_OrganizationCode)", paramCollection);
            dt = Convert.ToDateTime(paramCollection[2].Value);
            return dt;
        }

        #endregion

        #region SBIFInterest

        public List<SBIFInterestDTO> FindAll(int APageNo, SBIFInterestDTO ASBIFInterestDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "MaxInterestDefinitionFor", FilterValue = ASBIFInterestDTO.MaxInterestDefinitionFor, Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "OrganizationCode", FilterValue = Utils.OrganizationCode, Operator = "Contains"},
            };

            Expression<Func<SBIFInterest, bool>> expression = Utils.GetExpression<SBIFInterest>(lstExp);
            int iRecordCount = objSBIFInterestRepository.GetRecordCount(expression);

            //var vSBIFInterestDTO = objSBIFInterestRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vSBIFInterestDTO = objSBIFInterestRepository.GetFiltered(0, iRecordCount, expression);

            if (vSBIFInterestDTO != null && vSBIFInterestDTO.Any())
            {
                List<SBIFInterestDTO> lstSBIFInterestDTO = new List<SBIFInterestDTO>();
                foreach (var sbifInterest in vSBIFInterestDTO)
                {
                    lstSBIFInterestDTO.Add(Conversion.Mapping.SBIFInterestToSBIFInterestDTO(sbifInterest));
                }
                return lstSBIFInterestDTO;
            }
            else
                return new List<SBIFInterestDTO>();
        }

        public SBIFInterestDTO FindBySBIFInterestId(int AId)
        {
            var sbifInterest = objSBIFInterestRepository.Get(AId);

            if (sbifInterest != null)
            {
                objSBIFInterestRepository.UnitOfWork.RefreshDataInModel(ref sbifInterest);
                return Conversion.Mapping.SBIFInterestToSBIFInterestDTO(sbifInterest);
            }
            else
            {
                return new SBIFInterestDTO();
            }
        }

        public List<SBIFInterestDTO> FindSBIFInterest(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindSBIFInterest");

            //int iRecordCount = objSBIFInterestRepository.GetRecordCount();
            var sbifInterests = objSBIFInterestRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);
            objSBIFInterestRepository.UnitOfWork.ReloadAllDataInContext<SBIFInterest>();
            if (sbifInterests != null && sbifInterests.Any())
            {
                List<SBIFInterestDTO> lstSBIFInterestDTO = new List<SBIFInterestDTO>();
                foreach (var sbifInterest in sbifInterests)
                {
                    lstSBIFInterestDTO.Add(Conversion.Mapping.SBIFInterestToSBIFInterestDTO(sbifInterest));
                }
                return lstSBIFInterestDTO;
            }
            else
                return new List<SBIFInterestDTO>();
        }

        public void DeleteSBIFInterest(int ASBIFInterestId)
        {
            var sbifInterest = objSBIFInterestRepository.Get(ASBIFInterestId);

            if (sbifInterest != null)
            {
                objSBIFInterestRepository.Remove(sbifInterest);
                objSBIFInterestRepository.UnitOfWork.Commit();
            }
        }

        public void SaveSBIFInterestDetails(SBIFInterestDTO ASBIFInterestDTO)
        {
            if (ASBIFInterestDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddCurrencyWithNullInformation");

            var newSBIFInterest = SBIFInterestFactory.CreateSBIFInterest(ASBIFInterestDTO.SBIFInterestId, ASBIFInterestDTO.EffectiveDate,
                ASBIFInterestDTO.MaxInterestDefinitionFor
                , ASBIFInterestDTO.CreatedBy, ASBIFInterestDTO.LastUpdatedBy, ASBIFInterestDTO.ApprovalStatus, ASBIFInterestDTO.OrganizationCode);

            try
            {

                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {

                        objSBIFInterestRepository.Add(newSBIFInterest);
                        objSBIFInterestRepository.UnitOfWork.Commit();

                        #region SBIFInterestDetailsSave

                        ASBIFInterestDTO.SBIFInterestDetailsDTO.ForEach
                            (
                                x => objSBIFInterestDetailsRepository.Add(SBIFInterestDetailsFactory.CreateSBIFInterestDetails
                                        (
                                            x.SBIFInterestDetailsId,
                                            newSBIFInterest.SBIFInterestId,
                                            x.CreditLimitFrom,
                                            x.CreditLimitTo,
                                            x.SBIFInterestRate
                                        )
                                    )
                            );
                        objSBIFInterestDetailsRepository.UnitOfWork.Commit();

                        #endregion

                        if (ASBIFInterestDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_SBIFInterest", " SBIFInterestId = '" + newSBIFInterest.SBIFInterestId + "'",
                                "SBIFINT", ASBIFInterestDTO.CreatedBy, "/Master/SBIFInterestAuthorize?ASBIFInterestId=" + newSBIFInterest.SBIFInterestId + "&", "MT");

                            objSBIFInterestRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                        }

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public void UpdateSBIFInterestDetails(int ASBIFInterestId, SBIFInterestDTO ASBIFInterestDTO)
        {
            if (ASBIFInterestDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");

            }
            SBIFInterest currentSBIFInterest = objSBIFInterestRepository.Get(ASBIFInterestId);
            SBIFInterest updatedSBIFInterest = new SBIFInterest()
            {
                SBIFInterestId = ASBIFInterestId,
                EffectiveDate = ASBIFInterestDTO.EffectiveDate,
                MaxInterestDefinitionFor = ASBIFInterestDTO.MaxInterestDefinitionFor,
                OrganizationCode = currentSBIFInterest.OrganizationCode,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = ASBIFInterestDTO.LastUpdatedBy,
                CreatedOn = currentSBIFInterest.CreatedOn,
                CreatedBy = currentSBIFInterest.CreatedBy,
                ApprovalStatus = ASBIFInterestDTO.ApprovalStatus
            };


            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {

                    objSBIFInterestRepository.Merge(currentSBIFInterest, updatedSBIFInterest);
                    objSBIFInterestRepository.UnitOfWork.Commit();

                    //Deleting existing child table data 
                    foreach (SBIFInterestDetails s in currentSBIFInterest.SBIFInterestDetails.ToList())
                    {
                        objSBIFInterestDetailsRepository.Remove(s);
                    }
                    objSBIFInterestDetailsRepository.UnitOfWork.Commit();

                    //Inserting new data into child table
                    ASBIFInterestDTO.SBIFInterestDetailsDTO.ForEach
                        (
                            x => objSBIFInterestDetailsRepository.Add(SBIFInterestDetailsFactory.CreateSBIFInterestDetails
                                    (
                                        x.SBIFInterestDetailsId,
                                        x.SBIFInterestId,
                                        x.CreditLimitFrom,
                                        x.CreditLimitTo,
                                        x.SBIFInterestRate
                                    )
                                )
                        );

                    objSBIFInterestDetailsRepository.UnitOfWork.Commit();


                    if (ASBIFInterestDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_SBIFInterest", " SBIFInterestId = '" + ASBIFInterestId + "'",
                            "SBIFINT", ASBIFInterestDTO.LastUpdatedBy, "/Master/SBIFInterestAuthorize?ASBIFInterestId=" + ASBIFInterestId + "&", "MT");

                        objSBIFInterestRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
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

        #region InterestMaster

        public List<InterestMasterDTO> FindAll(int APageNo, InterestMasterDTO AInterestMasterDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "ProductCode", FilterValue = AInterestMasterDTO.ProductCode, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "CustomerSegmentCode", FilterValue = AInterestMasterDTO.CustomerSegmentCode, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "CustomerSubSegmentCode", FilterValue = AInterestMasterDTO.CustomerSubSegmentCode, Operator = "Contains"},
                  new Utils.ExpressionList(){FilterColumn = "OrganizationCode", FilterValue = Utils.OrganizationCode, Operator = "Contains"}
            };

            Expression<Func<InterestMaster, bool>> expression = Utils.GetExpression<InterestMaster>(lstExp);
            int iRecordCount = objInterestMasterRepository.GetRecordCount(expression);

            //var vInterestMasterDTO = objInterestMasterRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vInterestMasterDTO = objInterestMasterRepository.GetFiltered(0, iRecordCount, expression);

            if (vInterestMasterDTO != null && vInterestMasterDTO.Any())
            {
                List<InterestMasterDTO> lstInterestMasterDTO = new List<InterestMasterDTO>();
                foreach (var interestMaster in vInterestMasterDTO)
                {
                    lstInterestMasterDTO.Add(Conversion.Mapping.InterestMasterToInterestMasterDTO(interestMaster));
                }
                return lstInterestMasterDTO;
            }
            else
                return new List<InterestMasterDTO>();
        }

        public InterestMasterDTO FindByInterestMasterId(int AId)
        {

            InterestMaster interestMaster = objInterestMasterRepository.Get(AId);
            if (interestMaster != null)
                objInterestMasterRepository.UnitOfWork.RefreshDataInModel(ref interestMaster);

            if (interestMaster != null)
            {
                if (interestMaster.CustomerSegment == null)
                {
                    interestMaster.CustomerSegment = objCustomerSegmentRepository.Get(interestMaster.CustomerSegmentCode, Utils.OrganizationCode);
                }
                if (interestMaster.CustomerSubSegment == null)
                {
                    interestMaster.CustomerSubSegment = objCustomerSubSegmentRepository.Get(interestMaster.CustomerSubSegmentCode, Utils.OrganizationCode);
                }
                if (interestMaster.Product == null)
                {
                    interestMaster.Product = objProductRepository.Get(Utils.OrganizationCode, interestMaster.ProductCode);
                }

                return Conversion.Mapping.InterestMasterToInterestMasterDTO(interestMaster);
            }
            else
            {
                return new InterestMasterDTO();
            }
        }

        public List<InterestMasterDTO> FindInterestMaster(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindInterestMaster");

            int iRecordCount = objInterestMasterRepository.GetRecordCount();
            var interestMasters = objInterestMasterRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);
            if (interestMasters != null && interestMasters.Any())
            {
                List<InterestMasterDTO> lstInterestMasterDTO = new List<InterestMasterDTO>();
                foreach (var interestMaster in interestMasters)
                {
                    lstInterestMasterDTO.Add(Conversion.Mapping.InterestMasterToInterestMasterDTO(interestMaster));
                }
                return lstInterestMasterDTO;
            }
            else
                return new List<InterestMasterDTO>();
        }

        public void DeleteInterestMaster(int AInterestMasterId)
        {
            var interestMaster = objInterestMasterRepository.Get(AInterestMasterId);

            if (interestMaster != null)
            {
                objInterestMasterRepository.Remove(interestMaster);
                objInterestMasterRepository.UnitOfWork.Commit();
            }
        }

        public void SaveInterestMasterDetails(InterestMasterDTO AInterestMasterDTO)
        {
            if (AInterestMasterDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddCurrencyWithNullInformation");

            var newInterestMaster = InterestMasterFactory.CreateInterestMaster(AInterestMasterDTO.InterestMasterId,
                AInterestMasterDTO.ProductCode, AInterestMasterDTO.CustomerSegmentCode, AInterestMasterDTO.CustomerSubSegmentCode,
                AInterestMasterDTO.EffectiveDate, AInterestMasterDTO.MaxInterestDefinitionFor,
                AInterestMasterDTO.CreatedBy, AInterestMasterDTO.LastUpdatedBy, AInterestMasterDTO.ApprovalStatus, AInterestMasterDTO.OrganizationCode);

            try
            {

                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {

                        objInterestMasterRepository.Add(newInterestMaster);
                        objInterestMasterRepository.UnitOfWork.Commit();

                        #region InterestMasterDetailsSave

                        AInterestMasterDTO.InterestMasterDetailsDTO.ForEach
                            (
                                x => objInterestMasterDetailsRepository.Add(InterestMasterDetailsFactory.CreateInterestMasterDetails
                                        (
                                            x.InterestMasterDetailsId,
                                            newInterestMaster.InterestMasterId,
                                            x.CreditLimitFrom,
                                            x.CreditLimitTo,
                                            x.InterestRatePercent,
                                            x.MaxInterestDefinitionFor
                                        )
                                    )
                            );
                        objInterestMasterDetailsRepository.UnitOfWork.Commit();

                        #endregion

                        if (AInterestMasterDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_InterestMaster", " InterestMasterId = '" + newInterestMaster.InterestMasterId + "'",
                                "INTMSTR", AInterestMasterDTO.CreatedBy, "/Master/InterestMasterAuthorize?AInterestMasterId=" + newInterestMaster.InterestMasterId + "&", "MT");

                            objInterestMasterRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                        }

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateInterestMasterDetails(int AInterestMasterId, InterestMasterDTO AInterestMasterDTO)
        {
            if (AInterestMasterDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");

            }
            InterestMaster currentInterestMaster = objInterestMasterRepository.Get(AInterestMasterId);
            InterestMaster updatedInterestMaster = new InterestMaster()
            {
                InterestMasterId = AInterestMasterId,
                ProductCode = AInterestMasterDTO.ProductCode,
                CustomerSegmentCode = AInterestMasterDTO.CustomerSegmentCode,
                CustomerSubSegmentCode = AInterestMasterDTO.CustomerSubSegmentCode,
                EffectiveDate = AInterestMasterDTO.EffectiveDate,
                MaxInterestDefinitionFor = AInterestMasterDTO.MaxInterestDefinitionFor,
                OrganizationCode = currentInterestMaster.OrganizationCode,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = AInterestMasterDTO.LastUpdatedBy,
                CreatedOn = currentInterestMaster.CreatedOn,
                CreatedBy = currentInterestMaster.CreatedBy,
                ApprovalStatus = AInterestMasterDTO.ApprovalStatus
            };


            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {

                    objInterestMasterRepository.Merge(currentInterestMaster, updatedInterestMaster);
                    objInterestMasterRepository.UnitOfWork.Commit();

                    //Deleting existing child table data 
                    foreach (InterestMasterDetails s in currentInterestMaster.InterestMasterDetails.ToList())
                    {
                        objInterestMasterDetailsRepository.Remove(s);
                    }
                    objInterestMasterDetailsRepository.UnitOfWork.Commit();

                    //Inserting new data into child table
                    AInterestMasterDTO.InterestMasterDetailsDTO.ForEach
                        (
                            x => objInterestMasterDetailsRepository.Add(InterestMasterDetailsFactory.CreateInterestMasterDetails
                                    (
                                        x.InterestMasterDetailsId,
                                        x.InterestMasterId,
                                        x.CreditLimitFrom,
                                        x.CreditLimitTo,
                                        x.InterestRatePercent,
                                        x.MaxInterestDefinitionFor
                                    )
                                )
                        );

                    objInterestMasterDetailsRepository.UnitOfWork.Commit();


                    if (AInterestMasterDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_InterestMaster", " InterestMasterId = '" + AInterestMasterId + "'",
                            "INTMSTR", AInterestMasterDTO.LastUpdatedBy, "/Master/InterestMasterAuthorize?AInterestMasterId=" + AInterestMasterId + "&", "MT");

                        objInterestMasterRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
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

        #region Commission

        #region SaveCommissionDetails
        public void SaveCommissionDetails(CommissionDTO ACommissionDTO)
        {
            if (ACommissionDTO == null)
            {
                throw new ArgumentException("");
            }

            var newCommission = CommissionFactory.CreateCommission(ACommissionDTO.CommissionCode, ACommissionDTO.OrganizationCode,
                ACommissionDTO.Description, ACommissionDTO.CreatedBy, ACommissionDTO.LastUpdatedBy,
                            ACommissionDTO.ApprovalStatus, null);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objCommissionRepository.Add(newCommission);
                        objCommissionRepository.UnitOfWork.Commit();
                        if (ACommissionDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_Commission", " CommissionCode = '" + ACommissionDTO.CommissionCode + "'",
                                "COMSNCDE", ACommissionDTO.CreatedBy, "/Master/CommissionAuthorize/" + ACommissionDTO.CommissionCode + "/A?", "MT");

                            objCommissionRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value," +
                                " @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region UpdateCommissionDetails
        public void UpdateCommissionDetails(string ACode, CommissionDTO ACommissionDTO)
        {
            if (ACommissionDTO == null)
            {
                throw new ArgumentException("");
            }

            Commission currentCommission = objCommissionRepository.Get(ACode, Utils.OrganizationCode);
            Commission updatedCommission = new Commission()
            {
                CommissionCode = ACode,
                OrganizationCode = ACommissionDTO.OrganizationCode,
                Description = ACommissionDTO.Description,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = ACommissionDTO.LastUpdatedBy,
                CreatedOn = currentCommission.CreatedOn,
                CreatedBy = currentCommission.CreatedBy,
                ApprovalStatus = ACommissionDTO.ApprovalStatus
            };
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objCommissionRepository.Merge(currentCommission, updatedCommission);
                    objCommissionRepository.UnitOfWork.Commit();
                    if (ACommissionDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_Commission", " CommissionCode = '" + ACode + "'",
                                "COMSNCDE", ACommissionDTO.LastUpdatedBy, "/Master/CommissionAuthorize/" + ACode + "/A?", "MT");

                        objCommissionRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value," +
                            " @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
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
        #region DeleteCommission
        public void DeleteCommission(string ACode)
        {
            TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew,
              new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            try
            {
                var commission = objCommissionRepository.Get(ACode);

                if (commission != null)
                {

                    using (scope)
                    {
                        objCommissionRepository.Remove(commission);
                        objCommissionRepository.UnitOfWork.Commit();
                        scope.Complete();
                    }

                }
            }
            catch (Exception ex)
            {
                scope.Dispose();
                throw ex;

            }
        }
        #endregion
        #region FindCommission
        public List<CommissionDTO> FindCommission(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objCommissionRepository.GetRecordCount();
            var commission = objCommissionRepository.GetAll();
            if (commission != null && commission.Any())
            {
                List<CommissionDTO> lstCommissionDTO = new List<CommissionDTO>();
                foreach (var item in commission)
                {
                    lstCommissionDTO.Add(Conversion.Mapping.CommissionToCommissionDTO(item));
                }
                return lstCommissionDTO;
            }
            else
                return new List<CommissionDTO>();
        }
        #endregion
        #region FindAll
        public List<CommissionDTO> FindAll(int APageNo, CommissionDTO ACommissionDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "CommissionCode",FilterValue = ACommissionDTO.CommissionCode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "Description",FilterValue = ACommissionDTO.Description,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode,Operator = "Equals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
                    };

            Expression<Func<Commission, bool>> expression = Utils.GetExpression<Commission>(lstExpression);
            int iRecordCount = objCommissionRepository.GetRecordCount(expression);
            var vCommissionDTO = objCommissionRepository.GetFiltered(0, iRecordCount, expression);

            if (vCommissionDTO != null && vCommissionDTO.Any())
            {
                List<CommissionDTO> lstCommissionDTO = new List<CommissionDTO>();
                foreach (var item in vCommissionDTO)
                {
                    lstCommissionDTO.Add(Conversion.Mapping.CommissionToCommissionDTO(item));
                }
                return lstCommissionDTO;
            }
            else
            {
                return new List<CommissionDTO>();
            }
        }
        #endregion
        #region FindByCommission
        public CommissionDTO FindByCommission(string ACode)
        {
            Commission commission = objCommissionRepository.Get(ACode, Utils.OrganizationCode);
            if (commission != null)
            {
                objCommissionRepository.UnitOfWork.RefreshDataInModel(ref commission);
                return Conversion.Mapping.CommissionToCommissionDTO(commission);
            }
            else
            {
                return new CommissionDTO();
            }
        }
        #endregion

        public decimal GetCurrentUFRate(DateTime AEffectiveDate)
        {
            SqlParameter[] paramCollection = new SqlParameter[] { 
                            new SqlParameter(){ ParameterName = "@P_OrganizationCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = Utils.OrganizationCode, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ ParameterName = "@P_Effectivedate", SqlDbType = System.Data.SqlDbType.Date, Value = AEffectiveDate, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ParameterName = "@P_UFRate", SqlDbType = System.Data.SqlDbType.Decimal, Direction = System.Data.ParameterDirection.Output, Size = 25}
                        };

            objCommissionRepository.UnitOfWork.ExecuteCommand("PR_GetCurrentUFRate @P_OrganizationCode, @P_Effectivedate, @P_UFRate OUTPUT", paramCollection);
            decimal dt = Convert.ToDecimal(paramCollection[2].Value);
            return dt;
        }

        #endregion

        #region New Card Application

        public bool CheckIfValidCard(string ARUTNumber)
        {
            bool isvalid = Functions.IsValidRUTNumber(ARUTNumber);
            //Functions.GenerateCardEmbossingFile(ARUTNumber);

            return isvalid;
        }
        public string[] CheckIfAlreadyExistsCard(string ARUTNumber)
        {
            bool isvalid = Functions.IsValidRUTNumber(ARUTNumber);
            string[] retValues = new string[3];
            string cardNumber = string.Empty, customerId = string.Empty, approvalStatus = string.Empty;
            var cardNo = objCardMasterRepository.Get(ARUTNumber);
            if (cardNo != null)
            {
                cardNumber = cardNo.CardNumber;
                retValues[0] = "";
                retValues[1] = "";
                retValues[2] = cardNumber;

            }

            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                // var customerList = ((IQueryable<Customer>)(objCustomerRepository.Get(ARUTNumber))).Select(x => new { CustomerId = x.CustomerId, ApprovalStatus = x.ApprovalStatus });
                var customer = FindCustomerByRut(ARUTNumber);

                retValues[0] = customer.CustomerId.ToString();
                retValues[1] = customer.ApprovalStatus;
                retValues[2] = cardNumber;


                /*customerId = customerList.First().CustomerId;
                approvalStatus = customerList.First().ApprovalStatus;*/
            }



            return retValues;
        }

        public CustomerDTO FindCustomerByRut(string ARutNo)
        {
            CustomerDTO objCustomerDTO = new CustomerDTO();
            try
            {
                var customerResult = from customer in objCustomerRepository.AllMatching(x => x.RUT == ARutNo)
                                     join parentCustomer in objCustomerRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode)
                                     on customer.ParentCustomerId equals parentCustomer.CustomerId into result
                                     from parentCustomer in result.DefaultIfEmpty()
                                     select new { customer, ParentCustomerName = parentCustomer == null ? string.Empty : string.Concat(parentCustomer.Name, " ", parentCustomer.PaternalSurname) };

                if (customerResult.Any())
                {
                    Customer customer = customerResult.FirstOrDefault().customer;
                    objCustomerDTO = Conversion.Mapping.CustomerToCustomerDTO(customer);
                    objCustomerDTO.ParentCustomerName = customerResult.FirstOrDefault().ParentCustomerName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCustomerDTO;
        }
        #endregion

        #region CommissionCharge

        #region SaveCommissionChargeDetails
        public void SaveCommissionChargeDetails(CommissionChargeDTO ACommissionChargeDTO)
        {
            if (ACommissionChargeDTO == null)
            {
                throw new ArgumentException("");
            }

            var newCommissionCharge = CommissionChargeFactory.CreateCommissionCharge(ACommissionChargeDTO.CommissionChargeId,
                            ACommissionChargeDTO.OrganizationCode, ACommissionChargeDTO.ProductCode, ACommissionChargeDTO.ContractCode, ACommissionChargeDTO.CurrencyCode,
                            ACommissionChargeDTO.CreatedBy, ACommissionChargeDTO.LastUpdatedBy, ACommissionChargeDTO.ApprovalStatus, null);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objCommissionChargeRepository.Add(newCommissionCharge);
                        objCommissionChargeRepository.UnitOfWork.Commit();

                        #region CommissionChargeDetailsSave

                        ACommissionChargeDTO.CommissionChargeDetailsDTO.ForEach
                            (
                                x => objCommissionChargeDetailsRepository.Add(CommissionChargeDetailsFactory.CreateCommissionChargeDetails
                                        (
                                            newCommissionCharge.CommissionChargeId,
                                            x.CommissionChargeDetailsId,
                                            x.CommissionCode,
                                            x.CommissionAmount
                                        )
                                    )
                            );
                        objCommissionChargeDetailsRepository.UnitOfWork.Commit();

                        #endregion

                        if (ACommissionChargeDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_CommissionCharge", " CommissionChargeId = '" + newCommissionCharge.CommissionChargeId + "'",
                                "COMSNCHG", ACommissionChargeDTO.CreatedBy, "/Master/CommissionChargeAuthorize?ACommissionChargeId=" + newCommissionCharge.CommissionChargeId + "&", "MT");

                            objCommissionChargeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName," +
                                " @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                        }

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region UpdateCommissionChargeDetails
        public void UpdateCommissionChargeDetails(int AId, CommissionChargeDTO ACommissionChargeDTO)
        {
            if (ACommissionChargeDTO == null)
            {
                throw new ArgumentException("");
            }

            CommissionCharge currentCommissionCharge = objCommissionChargeRepository.Get(AId);
            CommissionCharge updatedCommissionCharge = new CommissionCharge()
            {
                CommissionChargeId = AId,
                OrganizationCode = ACommissionChargeDTO.OrganizationCode,
                ProductCode = ACommissionChargeDTO.ProductCode,
                ContractCode = ACommissionChargeDTO.ContractCode,
                CurrencyCode = ACommissionChargeDTO.CurrencyCode,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = ACommissionChargeDTO.LastUpdatedBy,
                CreatedOn = currentCommissionCharge.CreatedOn,
                CreatedBy = currentCommissionCharge.CreatedBy,
                ApprovalStatus = ACommissionChargeDTO.ApprovalStatus
            };


            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {

                    objCommissionChargeRepository.Merge(currentCommissionCharge, updatedCommissionCharge);
                    objCommissionChargeRepository.UnitOfWork.Commit();

                    //Deleting existing child table data 
                    foreach (CommissionChargeDetails s in currentCommissionCharge.CommissionChargeDetails.ToList())
                    {
                        objCommissionChargeDetailsRepository.Remove(s);
                    }
                    objCommissionChargeDetailsRepository.UnitOfWork.Commit();

                    //Inserting new data into child table
                    ACommissionChargeDTO.CommissionChargeDetailsDTO.ForEach
                        (
                            x => objCommissionChargeDetailsRepository.Add(CommissionChargeDetailsFactory.CreateCommissionChargeDetails
                                    (
                                        x.CommissionChargeId,
                                        x.CommissionChargeDetailsId,
                                        x.CommissionCode,
                                        x.CommissionAmount
                                    )
                                )
                        );

                    objCommissionChargeDetailsRepository.UnitOfWork.Commit();


                    if (ACommissionChargeDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_CommissionCharge", " CommissionChargeId = '" + AId + "'",
                               "COMSNCHG", ACommissionChargeDTO.LastUpdatedBy, "/Master/CommissionChargeAuthorize?ACommissionChargeId=" + AId + "&", "MT");

                        objCommissionChargeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName," +
                            " @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
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
        #region DeleteCommissionCharge
        public void DeleteCommissionCharge(int AId)
        {
            var commissionCharge = objCommissionChargeRepository.Get(AId);
            if (commissionCharge != null)
            {
                //objCommissionChargeRepository.Remove(commissionCharge);
                //objCommissionChargeRepository.UnitOfWork.Commit();

                CommissionCharge currentCommissionCharge = objCommissionChargeRepository.Get(AId);

                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                 new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        //Deleting existing child table data 
                        foreach (CommissionChargeDetails s in currentCommissionCharge.CommissionChargeDetails.ToList())
                        {
                            objCommissionChargeDetailsRepository.Remove(s);
                        }
                        objCommissionChargeDetailsRepository.UnitOfWork.Commit();

                        objCommissionChargeRepository.Remove(commissionCharge);
                        objCommissionChargeRepository.UnitOfWork.Commit();

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
        }
        #endregion

        #region FindCommissionCharge
        public List<CommissionChargeDTO> FindCommissionCharge(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objCommissionChargeRepository.GetRecordCount();
            var commissionCharge = objCommissionChargeRepository.GetAll();
            if (commissionCharge != null && commissionCharge.Any())
            {
                List<CommissionChargeDTO> lstCommissionChargeDTO = new List<CommissionChargeDTO>();
                foreach (var item in commissionCharge)
                {
                    lstCommissionChargeDTO.Add(Conversion.Mapping.CommissionChargeToCommissionChargeDTO(item));
                }
                return lstCommissionChargeDTO;
            }
            else
                return new List<CommissionChargeDTO>();
        }
        #endregion
        #region FindAll
        public List<CommissionChargeDTO> FindAll(int APageNo, CommissionChargeDTO ACommissionChargeDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "ProductCode",FilterValue = ACommissionChargeDTO.ProductCode, Operator = ACommissionChargeDTO.ProductCode == string.Empty ? "Contains" : "Equals" },
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode,Operator = "Equals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
                    };

            Expression<Func<CommissionCharge, bool>> expression = Utils.GetExpression<CommissionCharge>(lstExpression);
            int iRecordCount = objCommissionChargeRepository.GetRecordCount(expression);
            var vCommissionChargeDTO = objCommissionChargeRepository.GetFiltered(0, iRecordCount, expression);

            if (vCommissionChargeDTO != null && vCommissionChargeDTO.Any())
            {
                List<CommissionChargeDTO> lstCommissionChargeDTO = new List<CommissionChargeDTO>();
                foreach (var item in vCommissionChargeDTO)
                {
                    lstCommissionChargeDTO.Add(Conversion.Mapping.CommissionChargeToCommissionChargeDTO(item));
                }
                return lstCommissionChargeDTO;
            }
            else
            {
                return new List<CommissionChargeDTO>();
            }
        }
        #endregion
        #region FindByCommissionCharge
        public CommissionChargeDTO FindByCommissionCharge(int AId)
        {
            CommissionCharge commissionCharge = objCommissionChargeRepository.Get(AId);
            if (commissionCharge != null)
            {
                if (commissionCharge.Product == null)
                {
                    commissionCharge.Product = objProductRepository.Get(commissionCharge.OrganizationCode, commissionCharge.ProductCode);
                }
                if (commissionCharge.Contract == null)
                {
                    commissionCharge.Contract = objContractRepository.Get(commissionCharge.ContractCode, commissionCharge.OrganizationCode);
                }
                objCommissionChargeRepository.UnitOfWork.RefreshDataInModel(ref commissionCharge);
                return Conversion.Mapping.CommissionChargeToCommissionChargeDTO(commissionCharge);
            }
            else
            {
                return new CommissionChargeDTO();
            }
        }
        #endregion

        #endregion

        #region BlockCodeForCommissionExclusion

        #region SaveBlockCodeForCommissionExclusionDetails
        public void SaveBlockCodeForCommissionExclusionDetails(BlockCodeForCommissionExclusionDTO ABlockCodeForCommissionExclusionDTO)
        {
            if (ABlockCodeForCommissionExclusionDTO == null)
            {
                throw new ArgumentException("");
            }
            var newBlockCodeForCommissionExclusion = BlockCodeForCommissionExclusionFactory.CreateBlockCodeForCommissionExclusion(
                ABlockCodeForCommissionExclusionDTO.BlockCodeForCommissionExclusionId, ABlockCodeForCommissionExclusionDTO.BlockCode,
                ABlockCodeForCommissionExclusionDTO.CommissionCode, ABlockCodeForCommissionExclusionDTO.IsIncludeCharges,
                ABlockCodeForCommissionExclusionDTO.OrganizationCode, ABlockCodeForCommissionExclusionDTO.CreatedBy,
                ABlockCodeForCommissionExclusionDTO.LastUpdatedBy, ABlockCodeForCommissionExclusionDTO.ApprovalStatus, null);
            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objBlockCodeForCommissionExclusionRepository.Add(newBlockCodeForCommissionExclusion);
                        objBlockCodeForCommissionExclusionRepository.UnitOfWork.Commit();
                        if (ABlockCodeForCommissionExclusionDTO.ApprovalStatus == "P")
                        {

                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_BlockCodeForCommissionExclusion", " BlockCodeForCommissionExclusionId = '" + newBlockCodeForCommissionExclusion.BlockCodeForCommissionExclusionId + "'",
                                "COMSNXCL", ABlockCodeForCommissionExclusionDTO.CreatedBy, "/Master/BlockCodeForCommissionExclusionAuthorize/" + newBlockCodeForCommissionExclusion.BlockCodeForCommissionExclusionId + "/A?", "MT");

                            objBlockCodeForCommissionExclusionRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl," +
                                " @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region UpdateBlockCodeForCommissionExclusionDetails
        public void UpdateBlockCodeForCommissionExclusionDetails(int ACode, BlockCodeForCommissionExclusionDTO ABlockCodeForCommissionExclusionDTO)
        {
            if (ABlockCodeForCommissionExclusionDTO == null)
            {
                throw new ArgumentException("");
            }

            BlockCodeForCommissionExclusion currentBlockCodeForCommissionExclusion = objBlockCodeForCommissionExclusionRepository.Get(ACode);
            BlockCodeForCommissionExclusion updatedBlockCodeForCommissionExclusion = new BlockCodeForCommissionExclusion()
            {
                BlockCodeForCommissionExclusionId = ACode,
                BlockCode = ABlockCodeForCommissionExclusionDTO.BlockCode,
                CommissionCode = ABlockCodeForCommissionExclusionDTO.CommissionCode,
                IsIncludeCharges = ABlockCodeForCommissionExclusionDTO.IsIncludeCharges,
                OrganizationCode = ABlockCodeForCommissionExclusionDTO.OrganizationCode,
                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = ABlockCodeForCommissionExclusionDTO.LastUpdatedBy,
                CreatedOn = currentBlockCodeForCommissionExclusion.CreatedOn,
                CreatedBy = currentBlockCodeForCommissionExclusion.CreatedBy,
                ApprovalStatus = ABlockCodeForCommissionExclusionDTO.ApprovalStatus
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objBlockCodeForCommissionExclusionRepository.Merge(currentBlockCodeForCommissionExclusion, updatedBlockCodeForCommissionExclusion);
                    objBlockCodeForCommissionExclusionRepository.UnitOfWork.Commit();
                    if (ABlockCodeForCommissionExclusionDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_BlockCodeForCommissionExclusion", " BlockCodeForCommissionExclusionId = '" + ACode + "'",
                            "COMSNXCL", ABlockCodeForCommissionExclusionDTO.LastUpdatedBy, "/Master/BlockCodeForCommissionExclusionAuthorize/" + ACode + "/A?", "MT");

                        objBlockCodeForCommissionExclusionRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value," +
                            " @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #region DeleteBlockCodeForCommissionExclusion
        public void DeleteBlockCodeForCommissionExclusion(int ACode)
        {
            var blockCodeForCommissionExclusion = objBlockCodeForCommissionExclusionRepository.Get(ACode);
            if (blockCodeForCommissionExclusion != null)
            {
                objBlockCodeForCommissionExclusionRepository.Remove(blockCodeForCommissionExclusion);
                objBlockCodeForCommissionExclusionRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindBlockCodeForCommissionExclusion
        public List<BlockCodeForCommissionExclusionDTO> FindBlockCodeForCommissionExclusion(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objBlockCodeForCommissionExclusionRepository.GetRecordCount();
            var blockCodeForCommissionExclusion = objBlockCodeForCommissionExclusionRepository.GetAll();
            if (blockCodeForCommissionExclusion != null && blockCodeForCommissionExclusion.Any())
            {
                List<BlockCodeForCommissionExclusionDTO> lstBlockCodeForCommissionExclusionDTO = new List<BlockCodeForCommissionExclusionDTO>();
                foreach (var item in blockCodeForCommissionExclusion)
                {
                    lstBlockCodeForCommissionExclusionDTO.Add(Conversion.Mapping.BlockCodeForCommissionExclusionToBlockCodeForCommissionExclusionDTO(item));
                }
                return lstBlockCodeForCommissionExclusionDTO;
            }
            else
                return new List<BlockCodeForCommissionExclusionDTO>();
        }
        #endregion
        #region FindAll
        public List<BlockCodeForCommissionExclusionDTO> FindAll(int APageNo, BlockCodeForCommissionExclusionDTO ABlockCodeForCommissionExclusionDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "CommissionCode",FilterValue = ABlockCodeForCommissionExclusionDTO.CommissionCode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "BlockCode",FilterValue = ABlockCodeForCommissionExclusionDTO.BlockCode,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode,Operator = "Equals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
                    };

            Expression<Func<BlockCodeForCommissionExclusion, bool>> expression = Utils.GetExpression<BlockCodeForCommissionExclusion>(lstExpression);
            int iRecordCount = objBlockCodeForCommissionExclusionRepository.GetRecordCount(expression);
            var vBlockCodeForCommissionExclusionDTO = objBlockCodeForCommissionExclusionRepository.GetFiltered(0, iRecordCount, expression);

            if (vBlockCodeForCommissionExclusionDTO != null && vBlockCodeForCommissionExclusionDTO.Any())
            {
                List<BlockCodeForCommissionExclusionDTO> lstBlockCodeForCommissionExclusionDTO = new List<BlockCodeForCommissionExclusionDTO>();
                foreach (var item in vBlockCodeForCommissionExclusionDTO)
                {
                    lstBlockCodeForCommissionExclusionDTO.Add(Conversion.Mapping.BlockCodeForCommissionExclusionToBlockCodeForCommissionExclusionDTO(item));
                }
                return lstBlockCodeForCommissionExclusionDTO;
            }
            else
            {
                return new List<BlockCodeForCommissionExclusionDTO>();
            }
        }
        #endregion
        #region FindByBlockCodeForCommissionExclusionId
        public BlockCodeForCommissionExclusionDTO FindByBlockCodeForCommissionExclusionId(int AId)
        {
            BlockCodeForCommissionExclusion blockCodeForCommissionExclusion = objBlockCodeForCommissionExclusionRepository.Get(AId);
            if (blockCodeForCommissionExclusion != null)
            {
                if (blockCodeForCommissionExclusion.Block == null)
                {
                    blockCodeForCommissionExclusion.Block = objBlockCodeRepository.Get(blockCodeForCommissionExclusion.BlockCode, Utils.OrganizationCode);
                }
                if (blockCodeForCommissionExclusion.Commission == null)
                {
                    blockCodeForCommissionExclusion.Commission = objCommissionRepository.Get(blockCodeForCommissionExclusion.CommissionCode, Utils.OrganizationCode);
                }

                objBlockCodeForCommissionExclusionRepository.UnitOfWork.RefreshDataInModel(ref blockCodeForCommissionExclusion);
                return Conversion.Mapping.BlockCodeForCommissionExclusionToBlockCodeForCommissionExclusionDTO(blockCodeForCommissionExclusion);
            }
            else
            {
                return new BlockCodeForCommissionExclusionDTO();
            }
        }
        #endregion

        #endregion

        #region DiscountOnMgmtCollection

        #region SaveDiscountOnMgmtCollectionDetails
        public void SaveDiscountOnMgmtCollectionDetails(DiscountOnMgmtCollectionDTO ADiscountOnMgmtCollectionDTO)
        {
            if (ADiscountOnMgmtCollectionDTO == null)
            {
                throw new ArgumentException("");
            }

            var newDiscountOnMgmtCollection = DiscountOnMgmtCollectionFactory.CreateDiscountOnMgmtCollection
                (
                    ADiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionId, ADiscountOnMgmtCollectionDTO.OrganizationCode,
                    ADiscountOnMgmtCollectionDTO.ProductCode, ADiscountOnMgmtCollectionDTO.ContractCode,
                    ADiscountOnMgmtCollectionDTO.CommissionCode, ADiscountOnMgmtCollectionDTO.CreatedBy,
                    ADiscountOnMgmtCollectionDTO.LastUpdatedBy, ADiscountOnMgmtCollectionDTO.ApprovalStatus, null
                );

            try
            {

                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {

                        objDiscountOnMgmtCollectionRepository.Add(newDiscountOnMgmtCollection);
                        objDiscountOnMgmtCollectionRepository.UnitOfWork.Commit();


                        #region DiscountOnMgmtCollectionSave

                        ADiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionDetailsDTO.ForEach
                            (
                                x => objDiscountOnMgmtCollectionDetailsRepository.Add(DiscountOnMgmtCollectionDetailsFactory.CreateDiscountOnMgmtCollectionDetails
                                        (
                                            x.DiscountOnMgmtCollectionDetailsId,
                                            newDiscountOnMgmtCollection.DiscountOnMgmtCollectionId,
                                            x.ConsumptionRangeFrom,
                                            x.ConsumptionRangeTo,
                                            x.DiscountPercentage
                                        )
                                    )
                            );
                        objDiscountOnMgmtCollectionDetailsRepository.UnitOfWork.Commit();

                        #endregion

                        if (ADiscountOnMgmtCollectionDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_DiscountOnMgmtCollection", " DiscountOnMgmtCollectionId = '" + newDiscountOnMgmtCollection.DiscountOnMgmtCollectionId + "'",
                                "DSMGMCOL", ADiscountOnMgmtCollectionDTO.CreatedBy, "/Master/DiscountOnMgmtCollectionAuthorize?ADiscountOnMgmtCollectionId=" + newDiscountOnMgmtCollection.DiscountOnMgmtCollectionId + "&", "MT");

                            objDiscountOnMgmtCollectionRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        #endregion
        #region UpdateDiscountOnMgmtCollectionDetails
        public void UpdateDiscountOnMgmtCollectionDetails(int AId, DiscountOnMgmtCollectionDTO ADiscountOnMgmtCollectionDTO)
        {
            if (ADiscountOnMgmtCollectionDTO == null)
            {
                throw new ArgumentException("");
            }

            DiscountOnMgmtCollection currentDiscountOnMgmtCollection = objDiscountOnMgmtCollectionRepository.Get(AId);
            DiscountOnMgmtCollection updatedDiscountOnMgmtCollection = new DiscountOnMgmtCollection()
            {
                DiscountOnMgmtCollectionId = AId,
                ProductCode = ADiscountOnMgmtCollectionDTO.ProductCode,
                OrganizationCode = ADiscountOnMgmtCollectionDTO.OrganizationCode,
                ContractCode = ADiscountOnMgmtCollectionDTO.ContractCode,
                CommissionCode = ADiscountOnMgmtCollectionDTO.CommissionCode,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = ADiscountOnMgmtCollectionDTO.LastUpdatedBy,
                CreatedOn = currentDiscountOnMgmtCollection.CreatedOn,
                CreatedBy = currentDiscountOnMgmtCollection.CreatedBy,
                ApprovalStatus = ADiscountOnMgmtCollectionDTO.ApprovalStatus
            };



            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {

                    objDiscountOnMgmtCollectionRepository.Merge(currentDiscountOnMgmtCollection, updatedDiscountOnMgmtCollection);
                    objDiscountOnMgmtCollectionRepository.UnitOfWork.Commit();

                    //Deleting existing child table data 
                    foreach (DiscountOnMgmtCollectionDetails s in currentDiscountOnMgmtCollection.DiscountOnMgmtCollectionDetails.ToList())
                    {
                        objDiscountOnMgmtCollectionDetailsRepository.Remove(s);
                    }
                    objDiscountOnMgmtCollectionDetailsRepository.UnitOfWork.Commit();

                    //Inserting new data into child table
                    ADiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionDetailsDTO.ForEach
                        (
                            x => objDiscountOnMgmtCollectionDetailsRepository.Add(DiscountOnMgmtCollectionDetailsFactory.CreateDiscountOnMgmtCollectionDetails
                                    (
                                        x.DiscountOnMgmtCollectionDetailsId,
                                        x.DiscountOnMgmtCollectionId,
                                        x.ConsumptionRangeFrom,
                                        x.ConsumptionRangeTo,
                                        x.DiscountPercentage
                                    )
                                )
                        );

                    objDiscountOnMgmtCollectionDetailsRepository.UnitOfWork.Commit();


                    if (ADiscountOnMgmtCollectionDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_DiscountOnMgmtCollection", " DiscountOnMgmtCollectionId = '" + AId + "'",
                            "DSMGMCOL", ADiscountOnMgmtCollectionDTO.LastUpdatedBy, "/Master/DiscountOnMgmtCollectionAuthorize?ADiscountOnMgmtCollectionId=" + AId + "&", "MT");

                        objDiscountOnMgmtCollectionRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #region DeleteDiscountOnMgmtCollection
        public void DeleteDiscountOnMgmtCollection(int AId)
        {
            var discountOnMgmtCollection = objDiscountOnMgmtCollectionRepository.Get(AId);
            if (discountOnMgmtCollection != null)
            {
                objDiscountOnMgmtCollectionRepository.Remove(discountOnMgmtCollection);
                objDiscountOnMgmtCollectionRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindDiscountOnMgmtCollection
        public List<DiscountOnMgmtCollectionDTO> FindDiscountOnMgmtCollection(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objDiscountOnMgmtCollectionRepository.GetRecordCount();
            var discountOnMgmtCollection = objDiscountOnMgmtCollectionRepository.GetAll();
            if (discountOnMgmtCollection != null && discountOnMgmtCollection.Any())
            {
                List<DiscountOnMgmtCollectionDTO> lstDiscountOnMgmtCollectionDTO = new List<DiscountOnMgmtCollectionDTO>();
                foreach (var item in discountOnMgmtCollection)
                {
                    lstDiscountOnMgmtCollectionDTO.Add(Conversion.Mapping.DiscountOnMgmtCollectionToDiscountOnMgmtCollectionDTO(item));
                }
                return lstDiscountOnMgmtCollectionDTO;
            }
            else
                return new List<DiscountOnMgmtCollectionDTO>();
        }
        #endregion
        #region FindAll
        public List<DiscountOnMgmtCollectionDTO> FindAll(int APageNo, DiscountOnMgmtCollectionDTO ADiscountOnMgmtCollectionDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { 
                        new Utils.ExpressionList() { FilterColumn = "ProductCode",FilterValue = ADiscountOnMgmtCollectionDTO.ProductCode,Operator = "Contains"},
                        new Utils.ExpressionList() { FilterColumn = "ContractCode",FilterValue = ADiscountOnMgmtCollectionDTO.ContractCode, Operator = "Contains" },
                        new Utils.ExpressionList() { FilterColumn = "CommissionCode",FilterValue = ADiscountOnMgmtCollectionDTO.CommissionCode, Operator = "Contains" },
                        new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode,Operator = "Equals"},
                        new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                        new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
                    };

            Expression<Func<DiscountOnMgmtCollection, bool>> expression = Utils.GetExpression<DiscountOnMgmtCollection>(lstExpression);
            int iRecordCount = objDiscountOnMgmtCollectionRepository.GetRecordCount(expression);

            //var vDiscountOnMgmtCollectionDTO = objDiscountOnMgmtCollectionRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vDiscountOnMgmtCollectionDTO = objDiscountOnMgmtCollectionRepository.GetFiltered(0, iRecordCount, expression);

            if (vDiscountOnMgmtCollectionDTO != null && vDiscountOnMgmtCollectionDTO.Any())
            {
                List<DiscountOnMgmtCollectionDTO> lstDiscountOnMgmtCollectionDTO = new List<DiscountOnMgmtCollectionDTO>();
                foreach (var item in vDiscountOnMgmtCollectionDTO)
                {
                    lstDiscountOnMgmtCollectionDTO.Add(Conversion.Mapping.DiscountOnMgmtCollectionToDiscountOnMgmtCollectionDTO(item));
                }
                return lstDiscountOnMgmtCollectionDTO;
            }
            else
            {
                return new List<DiscountOnMgmtCollectionDTO>();
            }
        }
        #endregion
        #region FindByDiscountOnMgmtCollectionId
        public DiscountOnMgmtCollectionDTO FindByDiscountOnMgmtCollectionId(int AId)
        {
            DiscountOnMgmtCollection discountOnMgmtCollection = objDiscountOnMgmtCollectionRepository.Get(AId);
            if (discountOnMgmtCollection != null)
            {
                if (discountOnMgmtCollection.Product == null)
                {
                    discountOnMgmtCollection.Product = objProductRepository.Get(discountOnMgmtCollection.OrganizationCode, discountOnMgmtCollection.ProductCode);
                }
                if (discountOnMgmtCollection.Contract == null)
                {
                    discountOnMgmtCollection.Contract = objContractRepository.Get(discountOnMgmtCollection.ContractCode, discountOnMgmtCollection.OrganizationCode);
                }
                if (discountOnMgmtCollection.Commission == null)
                {
                    discountOnMgmtCollection.Commission = objCommissionRepository.Get(discountOnMgmtCollection.CommissionCode, discountOnMgmtCollection.OrganizationCode);
                }

                objDiscountOnMgmtCollectionRepository.UnitOfWork.RefreshDataInModel(ref discountOnMgmtCollection);
                return Conversion.Mapping.DiscountOnMgmtCollectionToDiscountOnMgmtCollectionDTO(discountOnMgmtCollection);
            }
            else
            {
                return new DiscountOnMgmtCollectionDTO();
            }
        }
        #endregion

        #endregion

        #region CollectionCharge

        #region SaveCollectionChargeDetails
        public void SaveCollectionChargeDetails(CollectionChargeDTO ACollectionChargeDTO)
        {
            if (ACollectionChargeDTO == null)
            {
                throw new ArgumentException("");
            }

            var newCollectionCharge = CollectionChargeFactory.CreateCollectionCharge
                (
                    ACollectionChargeDTO.CollectionChargeId, ACollectionChargeDTO.OrganizationCode,
                    ACollectionChargeDTO.ProductCode, ACollectionChargeDTO.EffectiveDate, ACollectionChargeDTO.CurrencyCode,
                    ACollectionChargeDTO.CreatedBy, ACollectionChargeDTO.LastUpdatedBy, ACollectionChargeDTO.ApprovalStatus, null
                );
            try
            {

                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {

                        objCollectionChargeRepository.Add(newCollectionCharge);
                        objCollectionChargeRepository.UnitOfWork.Commit();

                        #region CollectionChargeDetailsSave

                        ACollectionChargeDTO.CollectionChargeDetailsDTO.ForEach
                            (
                                x => objCollectionChargeDetailsRepository.Add(CollectionChargeDetailsFactory.CreateCollectionChargeDetails
                                        (
                                            x.CollectionChargeDetailsId,
                                            newCollectionCharge.CollectionChargeId,
                                            x.DelinquencyAmountFrom,
                                            x.DelinquencyAmountTo,
                                            x.CollectionChargeAmount
                                        )
                                    )
                            );
                        objCollectionChargeDetailsRepository.UnitOfWork.Commit();

                        #endregion

                        if (ACollectionChargeDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_CollectionCharge", " CollectionChargeId = '" + newCollectionCharge.CollectionChargeId + "'",
                                "COLCHRG", ACollectionChargeDTO.CreatedBy, "/Master/CollectionChargeAuthorize?ACollectionChargeId=" + newCollectionCharge.CollectionChargeId + "&", "MT");

                            objCollectionChargeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName," +
                                " @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                        }

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        #endregion
        #region UpdateCollectionChargeDetails
        public void UpdateCollectionChargeDetails(int ACollectionChargeId, CollectionChargeDTO ACollectionChargeDTO)
        {
            if (ACollectionChargeDTO == null)
            {
                throw new ArgumentException("");
            }

            CollectionCharge currentCollectionCharge = objCollectionChargeRepository.Get(ACollectionChargeId);
            CollectionCharge updatedCollectionCharge = new CollectionCharge()
            {
                CollectionChargeId = ACollectionChargeId,
                ProductCode = ACollectionChargeDTO.ProductCode,
                EffectiveDate = ACollectionChargeDTO.EffectiveDate,
                OrganizationCode = ACollectionChargeDTO.OrganizationCode,
                CurrencyCode = ACollectionChargeDTO.CurrencyCode,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = ACollectionChargeDTO.LastUpdatedBy,
                CreatedOn = currentCollectionCharge.CreatedOn,
                CreatedBy = currentCollectionCharge.CreatedBy,
                ApprovalStatus = ACollectionChargeDTO.ApprovalStatus
            };


            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {

                    objCollectionChargeRepository.Merge(currentCollectionCharge, updatedCollectionCharge);
                    objCollectionChargeRepository.UnitOfWork.Commit();

                    //Deleting existing child table data 
                    foreach (CollectionChargeDetails s in currentCollectionCharge.CollectionChargeDetails.ToList())
                    {
                        objCollectionChargeDetailsRepository.Remove(s);
                    }
                    objCollectionChargeDetailsRepository.UnitOfWork.Commit();

                    //Inserting new data into child table
                    ACollectionChargeDTO.CollectionChargeDetailsDTO.ForEach
                        (
                            x => objCollectionChargeDetailsRepository.Add(CollectionChargeDetailsFactory.CreateCollectionChargeDetails
                                    (
                                        x.CollectionChargeDetailsId,
                                        x.CollectionChargeId,
                                        x.DelinquencyAmountFrom,
                                        x.DelinquencyAmountTo,
                                        x.CollectionChargeAmount
                                    )
                                )
                        );

                    objCollectionChargeDetailsRepository.UnitOfWork.Commit();


                    if (ACollectionChargeDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_CollectionCharge", " CollectionChargeId = '" + ACollectionChargeId + "'",
                            "COLCHRG", ACollectionChargeDTO.LastUpdatedBy, "/Master/CollectionChargeAuthorize?ACollectionChargeId=" + ACollectionChargeId + "&", "MT");

                        objDiscountOnMgmtCollectionRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName," +
                            " @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
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
        #region DeleteCollectionCharge
        public void DeleteCollectionCharge(int AId)
        {
            var collectionCharge = objCollectionChargeRepository.Get(AId);
            if (collectionCharge != null)
            {
                objCollectionChargeRepository.Remove(collectionCharge);
                objCollectionChargeRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindCollectionCharge
        public List<CollectionChargeDTO> FindCollectionCharge(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objCollectionChargeRepository.GetRecordCount();
            var collectionCharge = objCollectionChargeRepository.GetAll();
            if (collectionCharge != null && collectionCharge.Any())
            {
                List<CollectionChargeDTO> lstCollectionChargeDTO = new List<CollectionChargeDTO>();
                foreach (var item in collectionCharge)
                {
                    lstCollectionChargeDTO.Add(Conversion.Mapping.CollectionChargeToCollectionChargeDTO(item));
                }
                return lstCollectionChargeDTO;
            }
            else
                return new List<CollectionChargeDTO>();
        }
        #endregion
        #region FindAll
        public List<CollectionChargeDTO> FindAll(int APageNo, CollectionChargeDTO ACollectionChargeDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
            { 
                new Utils.ExpressionList() { FilterColumn = "ProductCode",FilterValue = ACollectionChargeDTO.ProductCode, Operator = "Contains" },
                new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode, Operator = "Equals"},
                new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
            };

            Expression<Func<CollectionCharge, bool>> expression = Utils.GetExpression<CollectionCharge>(lstExpression);
            int iRecordCount = objCollectionChargeRepository.GetRecordCount(expression);
            var vCollectionChargeDTO = objCollectionChargeRepository.GetFiltered(0, iRecordCount, expression);

            if (vCollectionChargeDTO != null && vCollectionChargeDTO.Any())
            {
                List<CollectionChargeDTO> lstCollectionChargeDTO = new List<CollectionChargeDTO>();
                foreach (var item in vCollectionChargeDTO)
                {
                    lstCollectionChargeDTO.Add(Conversion.Mapping.CollectionChargeToCollectionChargeDTO(item));
                }
                return lstCollectionChargeDTO;
            }
            else
            {
                return new List<CollectionChargeDTO>();
            }
        }
        #endregion
        #region FindByCollectionChargeId
        public CollectionChargeDTO FindByCollectionChargeId(int AId)
        {
            CollectionCharge collectionCharge = objCollectionChargeRepository.Get(AId);
            if (collectionCharge != null)
            {
                if (collectionCharge.Product == null)
                {
                    collectionCharge.Product = objProductRepository.Get(collectionCharge.OrganizationCode, collectionCharge.ProductCode);
                }
                objCollectionChargeRepository.UnitOfWork.RefreshDataInModel(ref collectionCharge);
                return Conversion.Mapping.CollectionChargeToCollectionChargeDTO(collectionCharge);
            }
            else
            {
                return new CollectionChargeDTO();
            }
        }
        #endregion

        #endregion

        #region InsuranceCharge

        #region SaveInsuranceChargeDetails
        public void SaveInsuranceChargeDetails(InsuranceChargeDTO AInsuranceChargeDTO)
        {
            if (AInsuranceChargeDTO == null)
            {
                throw new ArgumentException("");
            }

            var newInsuranceCharge = InsuranceChargeFactory.CreateInsuranceCharge(AInsuranceChargeDTO.InsuranceChargeId,
                AInsuranceChargeDTO.OrganizationCode, AInsuranceChargeDTO.ContractCode, AInsuranceChargeDTO.ProductCode, AInsuranceChargeDTO.EffectiveDate,
                AInsuranceChargeDTO.InsuranceType, AInsuranceChargeDTO.InsuranceOption, AInsuranceChargeDTO.InsuranceAmount,
                AInsuranceChargeDTO.EligibilityAgeYears, AInsuranceChargeDTO.EligibilityAgeDays, AInsuranceChargeDTO.MaxCoverAgeYears,
                AInsuranceChargeDTO.MaxCoverAgeDays, AInsuranceChargeDTO.IsGeneralOrSpecial, AInsuranceChargeDTO.CurrencyCode,
                AInsuranceChargeDTO.PolicyCode, AInsuranceChargeDTO.PolicyDetails, AInsuranceChargeDTO.CollectionCharge,
                AInsuranceChargeDTO.CreatedBy, AInsuranceChargeDTO.LastUpdatedBy, AInsuranceChargeDTO.ApprovalStatus, null);

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                 new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objInsuranceChargeRepository.Add(newInsuranceCharge);
                    objInsuranceChargeRepository.UnitOfWork.Commit();

                    if (AInsuranceChargeDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_InsuranceCharge", " InsuranceChargeId = '" + newInsuranceCharge.InsuranceChargeId + "'",
                            "INSCHRG", AInsuranceChargeDTO.CreatedBy, "/Master/InsuranceChargeAuthorize/" + newInsuranceCharge.InsuranceChargeId + "/A?", "MT");
                        objInsuranceChargeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName," +
                            " @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #region UpdateobjInsuranceChargeRepositoryDetails
        public void UpdateInsuranceChargeDetails(int AInsuranceChargeId, InsuranceChargeDTO AInsuranceChargeDTO)
        {
            if (AInsuranceChargeDTO == null)
            {
                throw new ArgumentException("");
            }

            InsuranceCharge currentInsuranceCharge = objInsuranceChargeRepository.Get(AInsuranceChargeId);
            InsuranceCharge updatedInsuranceCharge = new InsuranceCharge()
            {
                InsuranceChargeId = AInsuranceChargeId,
                OrganizationCode = AInsuranceChargeDTO.OrganizationCode,
                ContractCode = AInsuranceChargeDTO.ContractCode,
                ProductCode = AInsuranceChargeDTO.ProductCode,
                EffectiveDate = AInsuranceChargeDTO.EffectiveDate,
                InsuranceType = AInsuranceChargeDTO.InsuranceType,
                InsuranceOption = AInsuranceChargeDTO.InsuranceOption,
                InsuranceAmount = AInsuranceChargeDTO.InsuranceAmount,
                EligibilityAgeYears = AInsuranceChargeDTO.EligibilityAgeYears,
                EligibilityAgeDays = AInsuranceChargeDTO.EligibilityAgeDays,
                MaxCoverAgeYears = AInsuranceChargeDTO.MaxCoverAgeYears,
                MaxCoverAgeDays = AInsuranceChargeDTO.MaxCoverAgeDays,
                IsGeneralOrSpecial = AInsuranceChargeDTO.IsGeneralOrSpecial,
                CurrencyCode = AInsuranceChargeDTO.CurrencyCode,
                PolicyCode = AInsuranceChargeDTO.PolicyCode,
                PolicyDetails = AInsuranceChargeDTO.PolicyDetails,
                CollectionCharge = AInsuranceChargeDTO.CollectionCharge,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = AInsuranceChargeDTO.LastUpdatedBy,
                CreatedOn = currentInsuranceCharge.CreatedOn,
                CreatedBy = currentInsuranceCharge.CreatedBy,
                ApprovalStatus = AInsuranceChargeDTO.ApprovalStatus
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
               new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objInsuranceChargeRepository.Merge(currentInsuranceCharge, updatedInsuranceCharge);
                    objInsuranceChargeRepository.UnitOfWork.Commit();

                    if (AInsuranceChargeDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_InsuranceCharge", " InsuranceChargeId = '" + AInsuranceChargeId + "'",
                            "INSCHRG", AInsuranceChargeDTO.LastUpdatedBy, "/Master/InsuranceChargeAuthorize/" + AInsuranceChargeId + "/A?", "MT");
                        objInsuranceChargeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName," +
                            " @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
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
        #region DeleteInsuranceCharge
        public void DeleteInsuranceCharge(int AInsuranceChargeId)
        {
            var insuranceCharge = objInsuranceChargeRepository.Get(AInsuranceChargeId);
            if (insuranceCharge != null)
            {
                objInsuranceChargeRepository.Remove(insuranceCharge);
                objInsuranceChargeRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindInsuranceCharge
        public List<InsuranceChargeDTO> FindInsuranceCharge(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objInsuranceChargeRepository.GetRecordCount();
            var insuranceCharge = objInsuranceChargeRepository.GetAll();
            if (insuranceCharge != null && insuranceCharge.Any())
            {
                List<InsuranceChargeDTO> lstInsuranceChargeDTO = new List<InsuranceChargeDTO>();
                foreach (var item in insuranceCharge)
                {
                    lstInsuranceChargeDTO.Add(Conversion.Mapping.InsuranceChargeToInsuranceChargeDTO(item));
                }
                return lstInsuranceChargeDTO;
            }
            else
                return new List<InsuranceChargeDTO>();
        }
        #endregion
        #region FindAll
        public List<InsuranceChargeDTO> FindAll(int APageNo, InsuranceChargeDTO AInsuranceChargeDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "InsuranceType",FilterValue = AInsuranceChargeDTO.InsuranceType, Operator = "Equals" },
                      new Utils.ExpressionList() { FilterColumn = "ProductCode",FilterValue = AInsuranceChargeDTO.ProductCode ,Operator = "Equals"},
                      new Utils.ExpressionList() { FilterColumn  = "PolicyCode", FilterValue = AInsuranceChargeDTO.PolicyCode , Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode,Operator = "Equals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
                    };

            Expression<Func<InsuranceCharge, bool>> expression = Utils.GetExpression<InsuranceCharge>(lstExpression);
            int iRecordCount = objInsuranceChargeRepository.GetRecordCount(expression);
            var vInsuranceChargeDTO = objInsuranceChargeRepository.GetFiltered(0, iRecordCount, expression);

            if (vInsuranceChargeDTO != null && vInsuranceChargeDTO.Any())
            {
                List<InsuranceChargeDTO> lstInsuranceChargeDTO = new List<InsuranceChargeDTO>();
                foreach (var item in vInsuranceChargeDTO)
                {
                    lstInsuranceChargeDTO.Add(Conversion.Mapping.InsuranceChargeToInsuranceChargeDTO(item));
                }
                return lstInsuranceChargeDTO;
            }
            else
            {
                return new List<InsuranceChargeDTO>();
            }
        }
        #endregion
        #region FindByInsuranceChargeId
        public InsuranceChargeDTO FindByInsuranceChargeId(int AInsuranceChargeId)
        {
            InsuranceCharge insuranceCharge = objInsuranceChargeRepository.Get(AInsuranceChargeId);
            if (insuranceCharge != null)
            {
                if (insuranceCharge.Contract == null)
                {
                    insuranceCharge.Contract = objContractRepository.Get(insuranceCharge.ContractCode, insuranceCharge.OrganizationCode);
                }
                if (insuranceCharge.Product == null)
                {
                    insuranceCharge.Product = objProductRepository.Get(insuranceCharge.OrganizationCode, insuranceCharge.ProductCode);
                }
                if (insuranceCharge.Currency == null)
                {
                    insuranceCharge.Currency = objCurrencyRepository.Get(insuranceCharge.OrganizationCode, insuranceCharge.CurrencyCode);
                }
                objInsuranceChargeRepository.UnitOfWork.RefreshDataInModel(ref insuranceCharge);
                return Conversion.Mapping.InsuranceChargeToInsuranceChargeDTO(insuranceCharge);
            }
            else
            {
                return new InsuranceChargeDTO();
            }
        }
        #endregion

        #endregion

        #region StatementGrouping

        #region SaveStatementGroupingDetails
        public void SaveStatementGroupingDetails(StatementGroupingDTO AStatementGroupingDTO)
        {
            if (AStatementGroupingDTO == null)
            {
                throw new ArgumentException("");
            }

            var newStatementGrouping = StatementGroupingFactory.CreateStatementGrouping(AStatementGroupingDTO.StatementGroupingId,
                AStatementGroupingDTO.ProductCode, AStatementGroupingDTO.NetworkCode, AStatementGroupingDTO.TransactionCode,
                AStatementGroupingDTO.GroupCode, AStatementGroupingDTO.GroupCodeDescription, AStatementGroupingDTO.SubGroupCode,
                AStatementGroupingDTO.SubGroupCodeDescription, AStatementGroupingDTO.CreatedBy, AStatementGroupingDTO.LastUpdatedBy,
                AStatementGroupingDTO.ApprovalStatus, AStatementGroupingDTO.OrganizationCode, null);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objStatementGroupingRepository.Add(newStatementGrouping);
                        objStatementGroupingRepository.UnitOfWork.Commit();

                        if (AStatementGroupingDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_StatementGrouping",
                                    " StatementGroupingId = '" + newStatementGrouping.StatementGroupingId + "'",
                                    "STMNTGRP", AStatementGroupingDTO.LastUpdatedBy,
                                    "/Master/StatementGroupingAuthorize/" + newStatementGrouping.StatementGroupingId + "/A?", "MT");

                            objStatementGroupingRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region UpdateStatementGroupingDetails
        public void UpdateStatementGroupingDetails(int AStatementGroupingId, StatementGroupingDTO AStatementGroupingDTO)
        {
            if (AStatementGroupingDTO == null)
            {
                throw new ArgumentException("");
            }

            StatementGrouping currentStatementGrouping = objStatementGroupingRepository.Get(AStatementGroupingId);
            StatementGrouping updatedStatementGrouping = new StatementGrouping()
            {
                StatementGroupingId = currentStatementGrouping.StatementGroupingId,
                ProductCode = AStatementGroupingDTO.ProductCode,
                NetworkCode = AStatementGroupingDTO.NetworkCode,
                TransactionCode = AStatementGroupingDTO.TransactionCode,
                GroupCode = AStatementGroupingDTO.GroupCode,
                GroupCodeDescription = AStatementGroupingDTO.GroupCodeDescription,
                SubGroupCode = AStatementGroupingDTO.SubGroupCode,
                SubGroupCodeDescription = AStatementGroupingDTO.SubGroupCodeDescription,
                OrganizationCode = currentStatementGrouping.OrganizationCode,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = AStatementGroupingDTO.LastUpdatedBy,
                CreatedOn = currentStatementGrouping.CreatedOn,
                CreatedBy = currentStatementGrouping.CreatedBy,
                ApprovalStatus = AStatementGroupingDTO.ApprovalStatus
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objStatementGroupingRepository.Merge(currentStatementGrouping, updatedStatementGrouping);
                    objStatementGroupingRepository.UnitOfWork.Commit();

                    if (AStatementGroupingDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_StatementGrouping",
                                " StatementGroupingId = '" + AStatementGroupingId + "'",
                                "STMNTGRP", AStatementGroupingDTO.LastUpdatedBy,
                                "/Master/StatementGroupingAuthorize/" + AStatementGroupingId + "/A?", "MT");

                        objStatementGroupingRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
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
        #region DeleteStatementGrouping
        public void DeleteStatementGrouping(string AProductCode, string ANetworkCode, string ATransactionCode)
        {
            var statementGrouping = objStatementGroupingRepository.Get(AProductCode, ANetworkCode, ATransactionCode);
            if (statementGrouping != null)
            {
                objStatementGroupingRepository.Remove(statementGrouping);
                objStatementGroupingRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindStatementGrouping
        public List<StatementGroupingDTO> FindStatementGrouping(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objStatementGroupingRepository.GetRecordCount();
            var statementGrouping = objStatementGroupingRepository.GetAll();
            if (statementGrouping != null && statementGrouping.Any())
            {
                List<StatementGroupingDTO> lstStatementGroupingDTO = new List<StatementGroupingDTO>();
                foreach (var item in statementGrouping)
                {
                    lstStatementGroupingDTO.Add(Conversion.Mapping.StatementGroupingToStatementGroupingDTO(item));
                }
                return lstStatementGroupingDTO;
            }
            else
                return new List<StatementGroupingDTO>();
        }
        #endregion
        #region FindAll
        public List<StatementGroupingDTO> FindAll(int APageNo, StatementGroupingDTO AStatementGroupingDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "ProductCode",FilterValue = AStatementGroupingDTO.ProductCode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "NetworkCode",FilterValue = AStatementGroupingDTO.NetworkCode ,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn  = "TransactionCode", FilterValue = AStatementGroupingDTO.TransactionCode , Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn  = "GroupCode", FilterValue = AStatementGroupingDTO.GroupCode , Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn  = "GroupCodeDescription", FilterValue = AStatementGroupingDTO.GroupCodeDescription , Operator = "Contains"},
                      new Utils.ExpressionList(){FilterColumn = "OrganizationCode", FilterValue = Utils.OrganizationCode, Operator = "Contains"},
                    };

            Expression<Func<StatementGrouping, bool>> expression = Utils.GetExpression<StatementGrouping>(lstExpression);
            int iRecordCount = objStatementGroupingRepository.GetRecordCount(expression);

            //var vStatementGroupingDTO = objStatementGroupingRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vStatementGroupingDTO = objStatementGroupingRepository.GetFiltered(0, iRecordCount, expression);

            if (vStatementGroupingDTO != null && vStatementGroupingDTO.Any())
            {
                List<StatementGroupingDTO> lstStatementGroupingDTO = new List<StatementGroupingDTO>();
                foreach (var item in vStatementGroupingDTO)
                {
                    lstStatementGroupingDTO.Add(Conversion.Mapping.StatementGroupingToStatementGroupingDTO(item));
                }
                return lstStatementGroupingDTO;
            }
            else
            {
                return new List<StatementGroupingDTO>();
            }
        }
        #endregion
        #region FindByStatementGroupingId
        public StatementGroupingDTO FindByStatementGroupingId(int AId)
        {
            StatementGrouping statementGrouping = objStatementGroupingRepository.Get(AId);
            if (statementGrouping != null)
            {
                objStatementGroupingRepository.UnitOfWork.RefreshDataInModel(ref statementGrouping);

                if (statementGrouping.Product == null)
                {
                    statementGrouping.Product = objProductRepository.Get(Utils.OrganizationCode, statementGrouping.ProductCode);
                }
                if (statementGrouping.Network == null)
                {
                    statementGrouping.Network = objNetworkRepository.Get(statementGrouping.NetworkCode);
                }
                if (statementGrouping.Transaction == null)
                {
                    statementGrouping.Transaction = objTransactionRepository.Get(Utils.OrganizationCode, statementGrouping.TransactionCode);
                }

                return Conversion.Mapping.StatementGroupingToStatementGroupingDTO(statementGrouping);
            }
            else
            {
                return new StatementGroupingDTO();
            }
        }
        #endregion

        #endregion

        #region PaymentAllocationType

        #region SavePaymentAllocationTypeDetails
        public void SavePaymentAllocationTypeDetails(PaymentAllocationTypeDTO APaymentAllocationTypeDTO)
        {
            if (APaymentAllocationTypeDTO == null)
            {
                throw new ArgumentException("");
            }

            var newPaymentAllocationType = PaymentAllocationTypeFactory.CreatePaymentAllocationType(APaymentAllocationTypeDTO.PaymentAllocationTypeId,
                APaymentAllocationTypeDTO.OrganizationCode, APaymentAllocationTypeDTO.CardType, APaymentAllocationTypeDTO.SeqNr,
                APaymentAllocationTypeDTO.PaymentAllocationSequence, APaymentAllocationTypeDTO.CreatedBy, APaymentAllocationTypeDTO.LastUpdatedBy,
                APaymentAllocationTypeDTO.ApprovalStatus, null);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objPaymentAllocationTypeRepository.Add(newPaymentAllocationType);
                        objPaymentAllocationTypeRepository.UnitOfWork.Commit();

                        if (APaymentAllocationTypeDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_PaymentAllocationType", " PaymentAllocationTypeId = '" + newPaymentAllocationType.PaymentAllocationTypeId + "'",
                                "PALOCTYP", APaymentAllocationTypeDTO.CreatedBy, "/Master/PaymentAllocationTypeAuthorize/" + newPaymentAllocationType.PaymentAllocationTypeId + "/A?", "MT");

                            objPaymentAllocationTypeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode," +
                                " @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region UpdatePaymentAllocationTypeDetails
        public void UpdatePaymentAllocationTypeDetails(int APaymentAllocationTypeId, PaymentAllocationTypeDTO APaymentAllocationTypeDTO)
        {
            if (APaymentAllocationTypeDTO == null)
            {
                throw new ArgumentException("");
            }

            PaymentAllocationType currentPaymentAllocationType = objPaymentAllocationTypeRepository.Get(APaymentAllocationTypeId);
            PaymentAllocationType updatedPaymentAllocationType = new PaymentAllocationType()
            {
                PaymentAllocationTypeId = APaymentAllocationTypeId,
                OrganizationCode = APaymentAllocationTypeDTO.OrganizationCode,
                CardType = APaymentAllocationTypeDTO.CardType,
                SeqNr = APaymentAllocationTypeDTO.SeqNr,
                PaymentAllocationSequence = APaymentAllocationTypeDTO.PaymentAllocationSequence,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = APaymentAllocationTypeDTO.LastUpdatedBy,
                CreatedOn = currentPaymentAllocationType.CreatedOn,
                CreatedBy = currentPaymentAllocationType.CreatedBy,
                ApprovalStatus = APaymentAllocationTypeDTO.ApprovalStatus
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objPaymentAllocationTypeRepository.Merge(currentPaymentAllocationType, updatedPaymentAllocationType);
                    objPaymentAllocationTypeRepository.UnitOfWork.Commit();

                    if (APaymentAllocationTypeDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_PaymentAllocationType", " PaymentAllocationTypeId = '" + APaymentAllocationTypeId + "'",
                                "PALOCTYP", APaymentAllocationTypeDTO.LastUpdatedBy, "/Master/PaymentAllocationTypeAuthorize/" + APaymentAllocationTypeId + "/A?", "MT");

                        objPaymentAllocationTypeRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause," +
                            " @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
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
        #region DeletePaymentAllocationType
        public void DeletePaymentAllocationType(int APaymentAllocationTypeId)
        {
            var paymentAllocationType = objPaymentAllocationTypeRepository.Get(APaymentAllocationTypeId);
            if (paymentAllocationType != null)
            {
                objPaymentAllocationTypeRepository.Remove(paymentAllocationType);
                objPaymentAllocationTypeRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindPaymentAllocationType
        public List<PaymentAllocationTypeDTO> FindPaymentAllocationType(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objPaymentAllocationTypeRepository.GetRecordCount();
            var paymentAllocationType = objPaymentAllocationTypeRepository.GetAll();
            if (paymentAllocationType != null && paymentAllocationType.Any())
            {
                List<PaymentAllocationTypeDTO> lstPaymentAllocationTypeDTO = new List<PaymentAllocationTypeDTO>();
                foreach (var item in paymentAllocationType)
                {
                    lstPaymentAllocationTypeDTO.Add(Conversion.Mapping.PaymentAllocationTypeToPaymentAllocationTypeDTO(item));
                }
                return lstPaymentAllocationTypeDTO;
            }
            else
                return new List<PaymentAllocationTypeDTO>();
        }
        #endregion
        #region FindAll
        public List<PaymentAllocationTypeDTO> FindAll(int APageNo, PaymentAllocationTypeDTO APaymentAllocationTypeDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "CardType",FilterValue = APaymentAllocationTypeDTO.CardType, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "SeqNr",FilterValue = APaymentAllocationTypeDTO.SeqNr ,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode,Operator = "Equals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
                    };

            Expression<Func<PaymentAllocationType, bool>> expression = Utils.GetExpression<PaymentAllocationType>(lstExpression);
            int iRecordCount = objPaymentAllocationTypeRepository.GetRecordCount(expression);

            var vPaymentAllocationTypeDTO = objPaymentAllocationTypeRepository.GetFiltered(0, iRecordCount, expression);

            if (vPaymentAllocationTypeDTO != null && vPaymentAllocationTypeDTO.Any())
            {
                List<PaymentAllocationTypeDTO> lstPaymentAllocationTypeDTO = new List<PaymentAllocationTypeDTO>();
                foreach (var item in vPaymentAllocationTypeDTO)
                {
                    lstPaymentAllocationTypeDTO.Add(Conversion.Mapping.PaymentAllocationTypeToPaymentAllocationTypeDTO(item));
                }
                return lstPaymentAllocationTypeDTO;
            }
            else
            {
                return new List<PaymentAllocationTypeDTO>();
            }
        }
        #endregion
        #region FindByPaymentAllocationTypeId
        public PaymentAllocationTypeDTO FindByPaymentAllocationTypeId(int APaymentAllocationTypeId)
        {
            PaymentAllocationType paymentAllocationType = objPaymentAllocationTypeRepository.Get(APaymentAllocationTypeId);
            if (paymentAllocationType != null)
            {
                objPaymentAllocationTypeRepository.UnitOfWork.RefreshDataInModel(ref paymentAllocationType);
                return Conversion.Mapping.PaymentAllocationTypeToPaymentAllocationTypeDTO(paymentAllocationType);
            }
            else
            {
                return new PaymentAllocationTypeDTO();
            }
        }
        #endregion

        #endregion

        #region PlanMaster

        public List<PlanMasterDTO> FindPlanMaster(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindPlanMaster");


            //int iRecordCount = objCurrencyRepository.GetRecordCount();

            var planmasters = objPlanMasterRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.PlanCode, true);

            objPlanMasterRepository.UnitOfWork.ReloadAllDataInContext<PlanMaster>();
            if (planmasters != null && planmasters.Any())
            {
                List<PlanMasterDTO> lstPlanMasterDTO = new List<PlanMasterDTO>();
                foreach (var planmaster in planmasters)
                {
                    lstPlanMasterDTO.Add(Conversion.Mapping.PlanMasterToPlanMasterDTO(planmaster));
                }
                return lstPlanMasterDTO;
            }
            else
                return new List<PlanMasterDTO>();
        }

        public void DeletePlanMaster(int APlanID)
        {
            var planmaster = objPlanMasterRepository.Get(APlanID);

            if (planmaster != null)
            {
                objPlanMasterRepository.Remove(planmaster);
                objPlanMasterRepository.UnitOfWork.Commit();
            }
        }

        public PlanMasterDTO FindByPlanID(int ACode, bool AIsFromApprovalQueue = false)
        {
            var planmaster = objPlanMasterRepository.Get(ACode);

            if (planmaster != null)
            {
                objPlanMasterRepository.UnitOfWork.RefreshDataInModel(ref planmaster);

                if (planmaster.CreditSegment == null)
                {
                    planmaster.CreditSegment = objCreditSegmentRepository.Get(planmaster.CreditSegmentCode, Utils.OrganizationCode);
                }
                if (planmaster.CustomerSegment == null)
                {
                    planmaster.CustomerSegment = objCustomerSegmentRepository.Get(planmaster.CustomerSegmentCode, Utils.OrganizationCode);
                }
                if (planmaster.CustomerSubSegment == null)
                {
                    planmaster.CustomerSubSegment = objCustomerSubSegmentRepository.Get(planmaster.CustomerSubSegmentCode, Utils.OrganizationCode);
                }
                if (planmaster.Product == null)
                {
                    planmaster.Product = objProductRepository.Get(Utils.OrganizationCode, planmaster.ProductCode);
                }
                if (planmaster.Transaction == null)
                {
                    planmaster.Transaction = objTransactionRepository.Get(Utils.OrganizationCode, planmaster.TransactionCode);
                }

                if (AIsFromApprovalQueue)
                    objPlanMasterRepository.Modify(planmaster);

                return Conversion.Mapping.PlanMasterToPlanMasterDTO(planmaster);
            }
            else
            {
                return new PlanMasterDTO();
            }

        }

        public void SavePlanMasterDetails(PlanMasterDTO APlanMasterDTO)
        {
            if (APlanMasterDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddPlanMasterWithNullInformation");

            var newPlanMaster = PlanMasterFactory.CreatePlanMaster(APlanMasterDTO.PlanID, APlanMasterDTO.CreditSegmentCode,
                 APlanMasterDTO.ProductCode, APlanMasterDTO.CustomerSegmentCode, APlanMasterDTO.CustomerSubSegmentCode, APlanMasterDTO.TransactionCode,
                 APlanMasterDTO.CreditLimitFrom, APlanMasterDTO.CreditLimitTo, APlanMasterDTO.TenureTo, APlanMasterDTO.ValidFromDate,
                 APlanMasterDTO.SBIFInterest, APlanMasterDTO.ExpectedInterestRate, APlanMasterDTO.DeferredPayment,
                 APlanMasterDTO.InterestCalculation, APlanMasterDTO.FirstBilling,
                 APlanMasterDTO.CreatedBy, APlanMasterDTO.LastUpdatedBy, null, APlanMasterDTO.OrganizationCode, APlanMasterDTO.ApprovalStatus);

            try
            {

                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objPlanMasterRepository.Add(newPlanMaster);
                        objPlanMasterRepository.UnitOfWork.Commit();
                        if (APlanMasterDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_PlanMaster", " PlanID = '" + newPlanMaster.PlanID + "'",
                                "PLNMSTR", APlanMasterDTO.CreatedBy, "/Master/PlanMasterAuthorize/" + newPlanMaster.PlanID + "/A?", "MT");

                            objPlanMasterRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdatePlanMasterDetails(int APlanID, PlanMasterDTO APlanMasterDTO)
        {
            if (APlanMasterDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdatePlanMasterWithNullInformation");

            }
            PlanMaster currentPlanMaster = objPlanMasterRepository.Get(APlanID);
            PlanMaster updatedPlanMaster = new PlanMaster()
            {
                PlanID = APlanID,
                CreditSegmentCode = APlanMasterDTO.CreditSegmentCode,
                ProductCode = APlanMasterDTO.ProductCode,
                TransactionCode = APlanMasterDTO.TransactionCode,
                CustomerSegmentCode = APlanMasterDTO.CustomerSegmentCode,
                CustomerSubSegmentCode = APlanMasterDTO.CustomerSubSegmentCode,
                ExpectedInterestRate = APlanMasterDTO.ExpectedInterestRate,
                CreditLimitFrom = APlanMasterDTO.CreditLimitFrom,
                CreditLimitTo = APlanMasterDTO.CreditLimitTo,
                FirstBilling = APlanMasterDTO.FirstBilling,
                SBIFInterest = APlanMasterDTO.SBIFInterest,
                //TenureFrom = APlanMasterDTO.TenureFrom,
                TenureTo = APlanMasterDTO.TenureTo,
                ValidFromDate = APlanMasterDTO.ValidFromDate,
                //ValidToDate = APlanMasterDTO.ValidToDate,
                InterestCalculation = APlanMasterDTO.InterestCalculation,
                DeferredPayment = APlanMasterDTO.DeferredPayment,
                OrganizationCode = currentPlanMaster.OrganizationCode,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = APlanMasterDTO.LastUpdatedBy,
                ApprovalStatus = APlanMasterDTO.ApprovalStatus,
                CreatedOn = currentPlanMaster.CreatedOn,
                CreatedBy = currentPlanMaster.CreatedBy

            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objPlanMasterRepository.Merge(currentPlanMaster, updatedPlanMaster);
                    objPlanMasterRepository.UnitOfWork.Commit();

                    if (APlanMasterDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_PlanMaster", " PlanID = '" + APlanID + "'",
                                "PLNMSTR", APlanMasterDTO.LastUpdatedBy, "/Master/PlanMasterAuthorize/" + APlanID + "/A?", "MT");

                        objPlanMasterRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<PlanMasterDTO> FindAll(int APageNo, PlanMasterDTO APlanMasterDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                
             
                     new Utils.ExpressionList() { FilterColumn = "CreditSegmentCode",FilterValue = APlanMasterDTO.CreditSegmentCode, Operator = "Equals" },
                      new Utils.ExpressionList() { FilterColumn = "ProductCode",FilterValue = APlanMasterDTO.ProductCode ,Operator = "Equals"},
                      new Utils.ExpressionList() { FilterColumn = "CustomerSegmentCode",FilterValue = APlanMasterDTO.CustomerSegmentCode, Operator = "Equals" },
                      new Utils.ExpressionList() { FilterColumn = "CustomerSubSegmentCode",FilterValue = APlanMasterDTO.CustomerSubSegmentCode ,Operator = "Equals"},
                      new Utils.ExpressionList(){FilterColumn = "OrganizationCode", FilterValue = Utils.OrganizationCode, Operator = "Contains"}
                    

            };

            Expression<Func<PlanMaster, bool>> expression = Utils.GetExpression<PlanMaster>(lstExp);
            int iRecordCount = objPlanMasterRepository.GetRecordCount(expression);

            //var vPlanMasterDTO = objPlanMasterRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vPlanMasterDTO = objPlanMasterRepository.GetFiltered(0, iRecordCount, expression);

            if (vPlanMasterDTO != null && vPlanMasterDTO.Any())
            {
                List<PlanMasterDTO> lstPlanMasterDTO = new List<PlanMasterDTO>();
                foreach (var planmaster in vPlanMasterDTO)
                {
                    lstPlanMasterDTO.Add(Conversion.Mapping.PlanMasterToPlanMasterDTO(planmaster));
                }
                return lstPlanMasterDTO;
            }
            else
                return new List<PlanMasterDTO>();

        }

        public decimal GetSBIFRate(DateTime AEffectiveDate)
        {
            SqlParameter[] paramCollection = new SqlParameter[] { 
                            new SqlParameter(){ ParameterName = "@P_OrganizationCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = Utils.OrganizationCode, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ ParameterName = "@P_Effectivedate", SqlDbType = System.Data.SqlDbType.Date, Value = AEffectiveDate, Direction = System.Data.ParameterDirection.Input },
                            new SqlParameter(){ ParameterName = "@P_SBIFRate", SqlDbType = System.Data.SqlDbType.Float, Direction = System.Data.ParameterDirection.Output, Size = 25}
                        };

            objPlanMasterRepository.UnitOfWork.ExecuteCommand("PR_GetSBIFRate @P_OrganizationCode, @P_Effectivedate, @P_SBIFRate OUTPUT", paramCollection);
            decimal dt = Convert.ToDecimal(paramCollection[2].Value);
            return dt;
        }

        #endregion

        #region BillingParameter
        #region SaveBillingParameterDetails
        public void SaveBillingParameterDetails(BillingParameterDTO ABillingParameterDTO)
        {
            if (ABillingParameterDTO == null)
            {
                throw new ArgumentException("");
            }

            var newBillingParameter = BillingParameterFactory.CreateBillingParameter(ABillingParameterDTO.ProductCode, ABillingParameterDTO.OrganizationCode, ABillingParameterDTO.MinTotalAmount,
                ABillingParameterDTO.MinTotalDebt, ABillingParameterDTO.ThresholdRevolvingBalance, ABillingParameterDTO.OCLChargePercentage,
                ABillingParameterDTO.MinOCLCharge, ABillingParameterDTO.MaxOCLCharge, ABillingParameterDTO.MinDelinquencyAmount, ABillingParameterDTO.MinCollectionCharges,
                ABillingParameterDTO.MaxCollectionCharges, ABillingParameterDTO.IsOCLIncludedInTotalMinDue, ABillingParameterDTO.DelinquencyBucketCountForWriteOff,
                ABillingParameterDTO.CreatedBy, ABillingParameterDTO.LastUpdatedBy, ABillingParameterDTO.ApprovalStatus, null);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objBillingParameterRepository.Add(newBillingParameter);
                        objBillingParameterRepository.UnitOfWork.Commit();

                        if (ABillingParameterDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_BillingParameter", " ProductCode = '" + newBillingParameter.ProductCode + "'",
                                "BLNGPARAM", ABillingParameterDTO.CreatedBy, "/Master/BillingParameterAuthorize/" + newBillingParameter.ProductCode + "/A?", "MT");

                            objBillingParameterRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
        #region UpdateBillingParameterDetails
        public void UpdateBillingParameterDetails(string AProductCode, BillingParameterDTO ABillingParameterDTO)
        {
            if (ABillingParameterDTO == null)
            {
                throw new ArgumentException("");
            }

            BillingParameter currentBillingParameter = objBillingParameterRepository.Get(Utils.OrganizationCode, AProductCode);
            BillingParameter updatedBillingParameter = new BillingParameter()
            {
                ProductCode = AProductCode,
                OrganizationCode = currentBillingParameter.OrganizationCode,
                MinTotalAmount = ABillingParameterDTO.MinTotalAmount,
                MinTotalDebt = ABillingParameterDTO.MinTotalDebt,
                ThresholdRevolvingBalance = ABillingParameterDTO.ThresholdRevolvingBalance,
                OCLChargePercentage = ABillingParameterDTO.OCLChargePercentage,
                MinOCLCharge = ABillingParameterDTO.MinOCLCharge,
                //PricingAmount = ABillingParameterDTO.PricingAmount,
                MaxOCLCharge = ABillingParameterDTO.MaxOCLCharge,
                MinDelinquencyAmount = ABillingParameterDTO.MinDelinquencyAmount,
                MinCollectionCharges = ABillingParameterDTO.MinCollectionCharges,
                MaxCollectionCharges = ABillingParameterDTO.MaxCollectionCharges,
                IsOCLIncludedInTotalMinDue = ABillingParameterDTO.IsOCLIncludedInTotalMinDue,
                DelinquencyBucketCountForWriteOff = ABillingParameterDTO.DelinquencyBucketCountForWriteOff,
                ApprovalStatus = ABillingParameterDTO.ApprovalStatus,
                CreatedBy = currentBillingParameter.CreatedBy,
                CreatedOn = currentBillingParameter.CreatedOn,
                LastUpdatedBy = ABillingParameterDTO.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objBillingParameterRepository.Merge(currentBillingParameter, updatedBillingParameter);
                    objBillingParameterRepository.UnitOfWork.Commit();

                    if (ABillingParameterDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_BillingParameter", " ProductCode = '" + AProductCode + "'",
                                "BLNGPARAM", ABillingParameterDTO.LastUpdatedBy, "/Master/BillingParameterAuthorize/" + AProductCode + "/A?", "MT");

                        objBillingParameterRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
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
        #region DeleteBillingParameter
        public void DeleteBillingParameter(string Id)
        {
            var billingParameter = objBillingParameterRepository.Get(Utils.OrganizationCode, Id);
            if (billingParameter != null)
            {
                objBillingParameterRepository.Remove(billingParameter);
                objBillingParameterRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindBillingParameter
        public List<BillingParameterDTO> FindBillingParameter(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objBillingParameterRepository.GetRecordCount();
            var billingParameter = objBillingParameterRepository.GetAll();


            if (billingParameter != null && billingParameter.Any())
            {
                List<BillingParameterDTO> lstBillingParameterDTO = new List<BillingParameterDTO>();
                foreach (var item in billingParameter)
                {
                    //if (item.Product == null)
                    //{
                    //    item.Product = objProductRepository.Get(item.ProductCode);
                    //}

                    lstBillingParameterDTO.Add(Conversion.Mapping.BillingParameterToBillingParameterDTO(item));
                }
                return lstBillingParameterDTO;
            }
            else
                return new List<BillingParameterDTO>();
        }
        #endregion
        #region FindAll
        public List<BillingParameterDTO> FindAll(int APageNo, BillingParameterDTO ABillingParameterDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { 
                        new Utils.ExpressionList() { FilterColumn = "ProductCode",FilterValue = ABillingParameterDTO.ProductCode, Operator = "Contains" },
                        new Utils.ExpressionList(){FilterColumn = "OrganizationCode", FilterValue = Utils.OrganizationCode, Operator = "Contains"}
                    };

            Expression<Func<BillingParameter, bool>> expression = Utils.GetExpression<BillingParameter>(lstExpression);
            int iRecordCount = objBillingParameterRepository.GetRecordCount(expression);

            //var vBillingParameterDTO = objBillingParameterRepository.GetFiltered(APageNo, iRecordCount, expression);

            var vBillingParameterDTO = objBillingParameterRepository.GetFiltered(0, iRecordCount, expression);

            if (vBillingParameterDTO != null && vBillingParameterDTO.Any())
            {
                List<BillingParameterDTO> lstBillingParameterDTO = new List<BillingParameterDTO>();
                foreach (var item in vBillingParameterDTO)
                {
                    lstBillingParameterDTO.Add(Conversion.Mapping.BillingParameterToBillingParameterDTO(item));
                }
                return lstBillingParameterDTO;
            }
            else
                return new List<BillingParameterDTO>();

        }
        #endregion
        #region FindByCardTypeCode
        public BillingParameterDTO FindByCardTypeCode(string ACardTypeCode)
        {
            BillingParameter billingParameter = objBillingParameterRepository.Get(Utils.OrganizationCode, ACardTypeCode);


            if (billingParameter != null)
            {
                if (billingParameter.Product == null)
                {
                    billingParameter.Product = objProductRepository.Get(Utils.OrganizationCode, billingParameter.ProductCode);
                }

                objBillingParameterRepository.UnitOfWork.RefreshDataInModel(ref billingParameter);
                return Conversion.Mapping.BillingParameterToBillingParameterDTO(billingParameter);
            }
            else
            {
                return new BillingParameterDTO();
            }
        }
        #endregion
        #endregion

        #region CardCreditSegment

        #region FindCardCreditSegment
        public List<CardCreditSegmentDTO> FindCardCreditSegment(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindPlanMaster");

            var CardCreditSegment = objCardCreditSegmentRepository.GetAll().Where(x => (x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS"));

            objCardCreditSegmentRepository.UnitOfWork.ReloadAllDataInContext<CardCreditSegment>();
            if (CardCreditSegment != null && CardCreditSegment.Any())
            {
                List<CardCreditSegmentDTO> lstCardCreditSegmentDTO = new List<CardCreditSegmentDTO>();
                foreach (var cardCreditSegment in CardCreditSegment)
                {
                    lstCardCreditSegmentDTO.Add(Conversion.Mapping.CardCreditSegmentToCardCreditSegmentDTO(cardCreditSegment));
                }
                return lstCardCreditSegmentDTO;
            }
            else
                return new List<CardCreditSegmentDTO>();
        }
        #endregion
        #region DeleteCardCreditSegment
        public void DeleteCardCreditSegment(string ACardNumber, string ACreditSegmentCode)
        {
            var cardCreditSegment = objCardCreditSegmentRepository.Get(ACreditSegmentCode, ACardNumber, Utils.OrganizationCode);

            if (cardCreditSegment != null)
            {
                objCardCreditSegmentRepository.Remove(cardCreditSegment);
                objCardCreditSegmentRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindByCardNumber
        public CardCreditSegmentDTO FindByCardNumber(string ACardNumber, string ACreditSegmentCode)
        {
            var cardCreditSegment = objCardCreditSegmentRepository.Get(ACreditSegmentCode, ACardNumber, Utils.OrganizationCode);

            if (cardCreditSegment != null)
            {
                objCardCreditSegmentRepository.UnitOfWork.RefreshDataInModel(ref cardCreditSegment);
                if (cardCreditSegment.objCustomerSegment == null)
                {
                    cardCreditSegment.objCustomerSegment = objCustomerSegmentRepository.Get(cardCreditSegment.CustomerSegmentCode, Utils.OrganizationCode);
                }
                if (cardCreditSegment.objCustomerSubSegment == null)
                {
                    cardCreditSegment.objCustomerSubSegment = objCustomerSubSegmentRepository.Get(cardCreditSegment.CustomerSubSegmentCode, Utils.OrganizationCode);
                }
                if (cardCreditSegment.objOrganization == null)
                {
                    cardCreditSegment.objOrganization = objOrganizationRepository.Get(cardCreditSegment.OrganizationCode, Utils.OrganizationCode);
                }
                if (cardCreditSegment.objCreditSegment == null)
                {
                    cardCreditSegment.objCreditSegment = objCreditSegmentRepository.Get(cardCreditSegment.CreditSegmentCode, Utils.OrganizationCode);
                }
                if (cardCreditSegment.objProduct == null)
                {
                    cardCreditSegment.objProduct = objProductRepository.Get(Utils.OrganizationCode, cardCreditSegment.ProductCode);
                }
                return Conversion.Mapping.CardCreditSegmentToCardCreditSegmentDTO(cardCreditSegment);
            }
            else
            {
                return new CardCreditSegmentDTO();
            }

        }
        #endregion
        #region SaveCardCreditSegmentDetails
        public void SaveCardCreditSegmentDetails(CardCreditSegmentDTO ACardCreditSegmentDTO)
        {
            if (ACardCreditSegmentDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddCardCreditSegmentWithNullInformation");

            var newCardCreditSegment = CardCreditSegmentFactory.CreateCardCreditSegment(ACardCreditSegmentDTO.OrganizationCode, ACardCreditSegmentDTO.CardNumber,
                 ACardCreditSegmentDTO.RUT, ACardCreditSegmentDTO.ProductCode, ACardCreditSegmentDTO.CustomerSegmentCode, ACardCreditSegmentDTO.CustomerSubSegmentCode,
                 ACardCreditSegmentDTO.CreditSegmentCode, ACardCreditSegmentDTO.CreditValue, ACardCreditSegmentDTO.OTB, ACardCreditSegmentDTO.ValidFrom, ACardCreditSegmentDTO.ValidTo,
                 ACardCreditSegmentDTO.CreatedBy, ACardCreditSegmentDTO.LastUpdatedBy, ACardCreditSegmentDTO.ApprovalStatus, null);
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objCardCreditSegmentRepository.Add(newCardCreditSegment);
                    objCardCreditSegmentRepository.UnitOfWork.Commit();
                    if (ACardCreditSegmentDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_CardCreditSegment",
                            " CardNumber = '" + ACardCreditSegmentDTO.CardNumber + "' AND CreditSegmentCode = '" + ACardCreditSegmentDTO.CreditSegmentCode + "' ",
                            "CRDCRSG", ACardCreditSegmentDTO.CreatedBy, "/Master/CardCreditSegmentAuthorize?ACardNumber=" + ACardCreditSegmentDTO.CardNumber + "&ACreditSegmentCode=" + ACardCreditSegmentDTO.CreditSegmentCode + "&", "MT");
                        objCardCreditSegmentRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType", paramCollection);
                    }

                    scope.Complete();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion
        #region UpdateCardCreditSegmentDetails
        public void UpdateCardCreditSegmentDetails(string ACardNumber, string ACreditSegmentCode, CardCreditSegmentDTO ACardCreditSegmentDTO)
        {
            if (ACardCreditSegmentDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdatePlanMasterWithNullInformation");
            }

            CardCreditSegment currentCardCreditSegment = objCardCreditSegmentRepository.Get(ACreditSegmentCode, ACardNumber, Utils.OrganizationCode);
            CardCreditSegment updatedCardCreditSegment = new CardCreditSegment()
            {
                CardNumber = ACardNumber,
                OrganizationCode = ACardCreditSegmentDTO.OrganizationCode,
                RUT = ACardCreditSegmentDTO.RUT,
                ProductCode = ACardCreditSegmentDTO.ProductCode,
                CustomerSegmentCode = ACardCreditSegmentDTO.CustomerSegmentCode,
                CustomerSubSegmentCode = ACardCreditSegmentDTO.CustomerSubSegmentCode,
                CreditSegmentCode = ACardCreditSegmentDTO.CreditSegmentCode,
                CreditValue = ACardCreditSegmentDTO.CreditValue,
                OTB = ACardCreditSegmentDTO.CreditValue,
                ValidFrom = ACardCreditSegmentDTO.ValidFrom,
                ValidTo = ACardCreditSegmentDTO.ValidTo,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = ACardCreditSegmentDTO.LastUpdatedBy,
                ApprovalStatus = ACardCreditSegmentDTO.ApprovalStatus,
                CreatedOn = currentCardCreditSegment.CreatedOn,
                CreatedBy = currentCardCreditSegment.CreatedBy

            };
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objCardCreditSegmentRepository.Merge(currentCardCreditSegment, updatedCardCreditSegment);
                    objCardCreditSegmentRepository.UnitOfWork.Commit();
                    if (ACardCreditSegmentDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_CardCreditSegment",
                            " CardNumber = '" + ACardCreditSegmentDTO.CardNumber + "' AND CreditSegmentCode = '" + ACardCreditSegmentDTO.CreditSegmentCode + "' ",
                            "CRDCRSG", ACardCreditSegmentDTO.LastUpdatedBy, "/Master/CardCreditSegmentAuthorize?ACardNumber=" + ACardCreditSegmentDTO.CardNumber + "&ACreditSegmentCode=" + ACardCreditSegmentDTO.CreditSegmentCode + "&", "MT");
                        objCardCreditSegmentRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                    }
                    scope.Complete();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        #endregion
        #region FindAllCardCreditSegments
        public List<CardCreditSegmentDTO> FindAllCardCreditSegments(int APageNo, CardCreditSegmentDTO ACardCreditSegmentDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
             
                new Utils.ExpressionList() { FilterColumn = "RUT",FilterValue = ACardCreditSegmentDTO.RUT, Operator = "Contains" },
                new Utils.ExpressionList() { FilterColumn = "CardNumber",FilterValue = ACardCreditSegmentDTO.CardNumber ,Operator = "Contains"},
                new Utils.ExpressionList() { FilterColumn = "CustomerSegmentCode",
                    FilterValue = ACardCreditSegmentDTO.CustomerSegmentCode, 
                    Operator = ACardCreditSegmentDTO.CustomerSegmentCode == string.Empty ? "Contains" : "Equals" },
                new Utils.ExpressionList() { FilterColumn = "CustomerSubSegmentCode",
                    FilterValue = ACardCreditSegmentDTO.CustomerSubSegmentCode ,
                    Operator = ACardCreditSegmentDTO.CustomerSubSegmentCode == string.Empty ? "Contains" : "Equals" },
                new Utils.ExpressionList() { FilterColumn = "CreditSegmentCode",
                    FilterValue = ACardCreditSegmentDTO.CreditSegmentCode ,
                    Operator = ACardCreditSegmentDTO.CreditSegmentCode == string.Empty ? "Contains" : "Equals" },
                new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"},
                new Utils.ExpressionList(){FilterColumn = "OrganizationCode", FilterValue = Utils.OrganizationCode, Operator = "Contains"}
            };

            Expression<Func<CardCreditSegment, bool>> expression = Utils.GetExpression<CardCreditSegment>(lstExp);
            int iRecordCount = objCardCreditSegmentRepository.GetRecordCount(expression);

            var vCardCreditSegmentDTO = objCardCreditSegmentRepository.GetFiltered(0, iRecordCount, expression);

            if (vCardCreditSegmentDTO != null && vCardCreditSegmentDTO.Any())
            {
                List<CardCreditSegmentDTO> lstCardCreditSegmentDTO = new List<CardCreditSegmentDTO>();
                foreach (var cardCreditSegment in vCardCreditSegmentDTO)
                {
                    lstCardCreditSegmentDTO.Add(Conversion.Mapping.CardCreditSegmentToCardCreditSegmentDTO(cardCreditSegment));
                }
                return lstCardCreditSegmentDTO;
            }
            else
                return new List<CardCreditSegmentDTO>();

        }
        #endregion
        #region GetRUTCardNumberDetails
        public List<RUTCardNumberSearchDTO> GetRUTCardNumberDetails(string ARUT, string ACardNumber, string ACustomerName)
        {
            List<RUTCardNumberSearchDTO> objRUTCardNumberSearchDTO = new List<RUTCardNumberSearchDTO>();

            var result = objCardMasterRepository.AllMatching(x => x.ApprovalStatus == "A" && x.RUT.Contains(ARUT == null ? "" : ARUT.Replace("-", "")) )
            .Join(objCustomerRepository.AllMatching(x => x.ApprovalStatus == "A" && x.RUT.Contains(ARUT == null ? "" : ARUT.Replace("-", ""))
             
             //x.Name.ToLower().Contains(ACustomerName == null ? "" : ACustomerName.ToLower())
             ),
                CardMaster => CardMaster.CustomerId,
                CustomerMaster => CustomerMaster.CustomerId,
                (CardMaster, CustomerMaster) => new
                {
                    CardMaster.CustomerId,
                    CardMaster.RUT,
                    CardMaster.CardNumber,
                    CardMaster.SalesId,
                    AccountNumber = CardMaster.AccountNo,
                    CustomerName = CustomerMaster.Name + " " + CustomerMaster.PaternalSurname + " " + CustomerMaster.MaternalSurname,
                    CardMaster.CustomerSegmentCode,
                    CardMaster.CustomerSubSegmentCode,
                    CardMaster.ProductCode,
                    CardMaster.OrganizationCode,
                    CardMaster.IsBlocked,
                    CardMaster.CardExpiryDate
                })
                .Where(x => (x.RUT.Contains(ARUT == null ? "" : ARUT.Replace("-", "")) &&
                    x.CardNumber.Contains(ACardNumber == null ? "" : ACardNumber) &&
                    x.CustomerName.ToLower().Contains(ACustomerName == null ? "" : ACustomerName.ToLower()) &&
                    x.IsBlocked == false &&
                    x.OrganizationCode == Utils.OrganizationCode 
                    //&& x.CardExpiryDate >= Utils.BusinessDate
                    ));

            if (result.Any())
            {
                result.ToList().ForEach(x => objRUTCardNumberSearchDTO.Add(new RUTCardNumberSearchDTO()
                {
                    CustomerId = x.CustomerId,
                    RUT = x.RUT,
                    CardNumber = x.CardNumber,
                    SalesId = x.SalesId,
                    AccountNumber = x.AccountNumber,
                    CustomerName = x.CustomerName,
                    CustomerSegmentCode = x.CustomerSegmentCode,
                    CustomerSubSegmentCode = x.CustomerSubSegmentCode,
                    ProductCode = x.ProductCode,
                    OrganizationCode = x.OrganizationCode
                }));
            }

            return objRUTCardNumberSearchDTO;
        }
        #endregion


        #region GetAllRUTZoomDetails
        public List<RUTCardNumberSearchDTO> GetAllRUTZoomDetails(string ARUT, string ACardNumber, string ACustomerName)
        {
            List<RUTCardNumberSearchDTO> objRUTCardNumberSearchDTO = new List<RUTCardNumberSearchDTO>();

            SqlParameter[] objSqlParameter = new SqlParameter[] { 
                new SqlParameter(){ ParameterName = "OrganizationCode", Value = Utils.OrganizationCode, SqlDbType = System.Data.SqlDbType.VarChar },
                new SqlParameter(){ ParameterName = "RUT", Value = ARUT == null ? "" : ARUT.Replace("-", "") , SqlDbType = System.Data.SqlDbType.VarChar },
                new SqlParameter(){ ParameterName ="CardNumber", Value = ACardNumber, SqlDbType = System.Data.SqlDbType.VarChar },
                new SqlParameter(){ ParameterName = "CustomerName", Value = ACustomerName, SqlDbType = System.Data.SqlDbType.VarChar }
            };

            objRUTCardNumberSearchDTO = objCardMasterRepository.UnitOfWork.ExecuteQuery<RUTCardNumberSearchDTO>("PR_GetAllRUTDetailsForZoom @OrganizationCode,@RUT,@CardNumber,@CustomerName", objSqlParameter).ToList();
            return objRUTCardNumberSearchDTO;
        }
        #endregion


        #endregion

        #region DiscountPercentage

        #region SaveDiscountPercentageDetails
        public void SaveDiscountPercentageDetails(DiscountPercentageDTO ADiscountPercentageDTO)
        {
            if (ADiscountPercentageDTO == null)
            {
                throw new ArgumentException("");
            }

            var newDiscountPercentage = DiscountPercentageFactory.CreateDiscountPercentage
                (
                    ADiscountPercentageDTO.DiscountPercentageId, ADiscountPercentageDTO.ProductCode, ADiscountPercentageDTO.SegmentCode,
                    ADiscountPercentageDTO.SubSegmentCode, ADiscountPercentageDTO.ValidFrom, ADiscountPercentageDTO.ValidTo,
                    ADiscountPercentageDTO.CreatedBy, ADiscountPercentageDTO.LastUpdatedBy, ADiscountPercentageDTO.ApprovalStatus, ADiscountPercentageDTO.OrganizationCode, null
                );


            try
            {

                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {

                        objDiscountPercentageRepository.Add(newDiscountPercentage);
                        objDiscountPercentageRepository.UnitOfWork.Commit();


                        #region DiscountPercentageDetailsSave

                        ADiscountPercentageDTO.DiscountPercentageDetailsDTO.ForEach
                            (
                                x => objDiscountPercentageDetailsRepository.Add(DiscountPercentageDetailsFactory.CreateDiscountPercentageDetails
                                        (
                                            x.DiscountPercentageDetailsId,
                                            newDiscountPercentage.DiscountPercentageId,
                                            x.PurchaseAmountFrom,
                                            x.PurchaseAmountTo,
                                            x.DPDDaysFrom,
                                            x.DPDDaysTo,
                                            x.DiscountPercentage
                                        )
                                    )
                            );
                        objDiscountPercentageDetailsRepository.UnitOfWork.Commit();

                        #endregion

                        if (ADiscountPercentageDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_DiscountPercentage", " DiscountPercentageId = '" + newDiscountPercentage.DiscountPercentageId + "'",
                                "DSCNTPERC", ADiscountPercentageDTO.CreatedBy, "/Master/DiscountPercentageAuthorize?ADiscountPercentageId=" + newDiscountPercentage.DiscountPercentageId + "&", "MT");

                            objDiscountPercentageRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                        }

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
        #region UpdateDiscountPercentageDetails
        public void UpdateDiscountPercentageDetails(int ADiscountPercentageId, DiscountPercentageDTO ADiscountPercentageDTO)
        {
            if (ADiscountPercentageDTO == null)
            {
                throw new ArgumentException("");
            }

            DiscountPercentage currentDiscountPercentage = objDiscountPercentageRepository.Get(ADiscountPercentageId);
            DiscountPercentage updatedDiscountPercentage = new DiscountPercentage()
            {
                DiscountPercentageId = ADiscountPercentageId,
                ProductCode = ADiscountPercentageDTO.ProductCode,
                SegmentCode = ADiscountPercentageDTO.SegmentCode,
                SubSegmentCode = ADiscountPercentageDTO.SubSegmentCode,
                ValidFrom = ADiscountPercentageDTO.ValidFrom,
                ValidTo = ADiscountPercentageDTO.ValidTo,
                OrganizationCode = currentDiscountPercentage.OrganizationCode,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = ADiscountPercentageDTO.LastUpdatedBy,
                CreatedOn = currentDiscountPercentage.CreatedOn,
                CreatedBy = currentDiscountPercentage.CreatedBy,
                ApprovalStatus = ADiscountPercentageDTO.ApprovalStatus
            };



            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {

                    objDiscountPercentageRepository.Merge(currentDiscountPercentage, updatedDiscountPercentage);
                    objDiscountPercentageRepository.UnitOfWork.Commit();


                    //Deleting existing child table data 
                    foreach (DiscountPercentageDetails s in currentDiscountPercentage.DiscountPercentageDetails.ToList())
                    {
                        objDiscountPercentageDetailsRepository.Remove(s);
                    }
                    objDiscountPercentageDetailsRepository.UnitOfWork.Commit();

                    //Inserting new data into child table
                    ADiscountPercentageDTO.DiscountPercentageDetailsDTO.ForEach
                        (
                            x => objDiscountPercentageDetailsRepository.Add(DiscountPercentageDetailsFactory.CreateDiscountPercentageDetails
                                    (
                                        x.DiscountPercentageDetailsId,
                                        x.DiscountPercentageId,
                                        x.PurchaseAmountFrom,
                                        x.PurchaseAmountTo,
                                        x.DPDDaysFrom,
                                        x.DPDDaysTo,
                                        x.DiscountPercentage
                                    )
                                )
                        );

                    objDiscountPercentageDetailsRepository.UnitOfWork.Commit();


                    if (ADiscountPercentageDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_DiscountPercentage", " DiscountPercentageId = '" + ADiscountPercentageId + "'",
                            "DSCNTPERC", ADiscountPercentageDTO.LastUpdatedBy, "/Master/DiscountPercentageAuthorize?ADiscountPercentageId=" + ADiscountPercentageId + "&", "MT");

                        objDiscountPercentageRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
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
        #region DeleteDiscountPercentage
        public void DeleteDiscountPercentage(int AId)
        {
            var discountPercentage = objDiscountPercentageRepository.Get(AId);
            if (discountPercentage != null)
            {
                objDiscountPercentageRepository.Remove(discountPercentage);
                objDiscountPercentageRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region FindDiscountPercentage
        public List<DiscountPercentageDTO> FindDiscountPercentage(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("");

            int iRecordCount = objDiscountPercentageRepository.GetRecordCount();
            var discountPercentage = objDiscountPercentageRepository.GetAll();
            if (discountPercentage != null && discountPercentage.Any())
            {
                List<DiscountPercentageDTO> lstDiscountPercentageDTO = new List<DiscountPercentageDTO>();
                foreach (var item in discountPercentage)
                {
                    lstDiscountPercentageDTO.Add(Conversion.Mapping.DiscountPercentageToDiscountPercentageDTO(item));
                }
                return lstDiscountPercentageDTO;
            }
            else
                return new List<DiscountPercentageDTO>();
        }
        #endregion
        #region FindAll
        public List<DiscountPercentageDTO> FindAll(int APageNo, DiscountPercentageDTO ADiscountPercentageDTO)
        {

            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { 
                        new Utils.ExpressionList() { FilterColumn = "ProductCode",FilterValue = ADiscountPercentageDTO.ProductCode, Operator = "Contains" },
                        new Utils.ExpressionList() { FilterColumn = "SegmentCode",FilterValue = ADiscountPercentageDTO.SegmentCode, Operator = "Contains" },
                        new Utils.ExpressionList() { FilterColumn = "SubSegmentCode",FilterValue = ADiscountPercentageDTO.SubSegmentCode, Operator = "Contains" },
                        new Utils.ExpressionList(){FilterColumn = "OrganizationCode", FilterValue = Utils.OrganizationCode, Operator = "Contains"}
                        
                    };

            Expression<Func<DiscountPercentage, bool>> expression = Utils.GetExpression<DiscountPercentage>(lstExpression);
            int iRecordCount = objDiscountPercentageRepository.GetRecordCount(expression);

            var vDiscountPercentageDTO = objDiscountPercentageRepository.GetFiltered(0, iRecordCount, expression);

            if (vDiscountPercentageDTO != null && vDiscountPercentageDTO.Any())
            {
                List<DiscountPercentageDTO> lstDiscountPercentageDTO = new List<DiscountPercentageDTO>();
                foreach (var item in vDiscountPercentageDTO)
                {
                    lstDiscountPercentageDTO.Add(Conversion.Mapping.DiscountPercentageToDiscountPercentageDTO(item));
                }
                return lstDiscountPercentageDTO;
            }
            else
            {
                return new List<DiscountPercentageDTO>();
            }
        }
        #endregion
        #region FindByDiscountPercentageId
        public DiscountPercentageDTO FindByDiscountPercentageId(int AId)
        {
            DiscountPercentage discountPercentage = objDiscountPercentageRepository.Get(AId);
            if (discountPercentage != null)
            {
                objDiscountPercentageRepository.UnitOfWork.RefreshDataInModel(ref discountPercentage);

                if (discountPercentage.CustomerSegment == null)
                {
                    discountPercentage.CustomerSegment = objCustomerSegmentRepository.Get(discountPercentage.SegmentCode, Utils.OrganizationCode);
                }
                if (discountPercentage.CustomerSubSegment == null)
                {
                    discountPercentage.CustomerSubSegment = objCustomerSubSegmentRepository.Get(discountPercentage.SubSegmentCode, Utils.OrganizationCode);
                }
                if (discountPercentage.Product == null)
                {
                    discountPercentage.Product = objProductRepository.Get(Utils.OrganizationCode, discountPercentage.ProductCode);
                }

                return Conversion.Mapping.DiscountPercentageToDiscountPercentageDTO(discountPercentage);
            }
            else
            {
                return new DiscountPercentageDTO();
            }
        }
        #endregion

        #endregion

        #region Merchant

        #region FindMerchantsByCode
        public MerchantsDTO FindByMerchantsCode(string AMerchantCode)
        {
            try
            {
                var merchants1 = objMerchantsRepository.Get(AMerchantCode, Utils.OrganizationCode);
            }
            catch { }
            var merchants = objMerchantsRepository.Get(AMerchantCode, Utils.OrganizationCode);
            if (merchants != null)
            {
                objMerchantsRepository.UnitOfWork.RefreshDataInModel(ref merchants);

                if (merchants.Country == null)
                {
                    merchants.Country = objCountryRepository.Get(merchants.CountryCode);
                }

                return Conversion.Mapping.MerchantsToMerchantsDTO(merchants);
            }
            else
                return new MerchantsDTO();
        }
        #endregion

        #region SaveMerchantDetails
        public void SaveMerchantsDetails(MerchantsDTO AMerchantsDTO)
        {
            if (AMerchantsDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddCurrencyWithNullInformation");

            var newMerchant = MerchantsFactory.CreateMerchant(AMerchantsDTO.MerchantCode, AMerchantsDTO.MerchantName, AMerchantsDTO.CountryCode, AMerchantsDTO.RegionCode,
                AMerchantsDTO.CityCode, AMerchantsDTO.CommunaCode, AMerchantsDTO.SettlementInDays, AMerchantsDTO.CommercialUEN, AMerchantsDTO.CommercialUENName,
                AMerchantsDTO.CreatedBy, AMerchantsDTO.LastUpdatedBy, AMerchantsDTO.ApprovalStatus, AMerchantsDTO.OrganizationCode);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objMerchantsRepository.Add(newMerchant);
                        objMerchantsRepository.UnitOfWork.Commit();

                        if (AMerchantsDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_Merchant", " MerchantCode = '" + AMerchantsDTO.MerchantCode + "'",
                                "MERCHANT", AMerchantsDTO.LastUpdatedBy, "/Master/MerchantAuthorize/" + AMerchantsDTO.MerchantCode + "/A?", "MT");
                            objMerchantsRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region UpdateMerchantsDetails

        public void UpdateMerchantsDetails(string AMerchantsCode, MerchantsDTO AMerchantsDTO)
        {
            if (AMerchantsDTO == null)
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");
            try
            {
                Merchants currentMerchant1 = objMerchantsRepository.Get(AMerchantsCode, Utils.OrganizationCode);
            }
            catch { }
            Merchants currentMerchant = objMerchantsRepository.Get(AMerchantsCode, Utils.OrganizationCode);
            Merchants updateMerchant = new Merchants()
            {
                MerchantCode = AMerchantsCode,
                OrganizationCode = Utils.OrganizationCode,
                MerchantName = AMerchantsDTO.MerchantName,
                CountryCode = AMerchantsDTO.CountryCode,
                RegionCode = AMerchantsDTO.RegionCode,
                CityCode = AMerchantsDTO.CityCode,
                CommunaCode = AMerchantsDTO.CommunaCode,
                SettlementInDays = AMerchantsDTO.SettlementInDays,
                CommercialUEN = AMerchantsDTO.CommercialUEN,
                CommercialUENName = AMerchantsDTO.CommercialUENName,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = AMerchantsDTO.LastUpdatedBy,
                CreatedOn = AMerchantsDTO.CreatedOn,
                CreatedBy = AMerchantsDTO.CreatedBy,
                ApprovalStatus = AMerchantsDTO.ApprovalStatus
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
              new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objMerchantsRepository.Merge(currentMerchant, updateMerchant);
                    objMerchantsRepository.UnitOfWork.Commit();

                    if (AMerchantsDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_Merchant", " MerchantCode = '" + AMerchantsDTO.MerchantCode + "'",
                            "MERCHANT", AMerchantsDTO.LastUpdatedBy, "/Master/MerchantAuthorize/" + AMerchantsDTO.MerchantCode + "/A?", "MT");

                        objCityRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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

        #region FindAllMerchants
        public List<MerchantsDTO> FindAll(int APageNo, MerchantsDTO AMerchantDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "MerchantCode",FilterValue = AMerchantDTO.MerchantCode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "MerchantName",FilterValue = AMerchantDTO.MerchantName,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "CountryCode",FilterValue = AMerchantDTO.CountryCode,Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode, Operator = "Equals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
                      
                    };

            Expression<Func<Merchants, bool>> expression = Utils.GetExpression<Merchants>(lstExpression);
            int iRecordCount = objMerchantsRepository.GetRecordCount(expression);

            var vMerchantsDTO = objMerchantsRepository.GetFiltered(0, iRecordCount, expression);

            if (vMerchantsDTO != null && vMerchantsDTO.Any())
            {
                List<MerchantsDTO> lstMerchantsDTO = new List<MerchantsDTO>();
                foreach (var merchant in vMerchantsDTO)
                {
                    lstMerchantsDTO.Add(Conversion.Mapping.MerchantsToMerchantsDTO(merchant));
                }
                return lstMerchantsDTO;
            }
            else
                return new List<MerchantsDTO>();
        }
        #endregion

        #region FindMerchant
        public List<MerchantsDTO> FindMerchant(int APageindex, int APageCount)
        {
            var merchants = objMerchantsRepository.GetAll();
            if (merchants != null && merchants.Any())
            {
                List<MerchantsDTO> lstMerchantDTO = new List<MerchantsDTO>();
                foreach (var merchant in merchants)
                {
                    lstMerchantDTO.Add(Conversion.Mapping.MerchantsToMerchantsDTO(merchant));
                }
                return lstMerchantDTO;
            }
            else
                return new List<MerchantsDTO>();
        }
        #endregion

        #region DeleteMerchant
        public void DeleteMerchant(string AMerchantCode)
        {
            var merchant = objMerchantsRepository.Get(AMerchantCode);
            if (merchant != null)
            {
                objMerchantsRepository.Remove(merchant);
                objMerchantsRepository.UnitOfWork.Commit();
            }
        }
        #endregion

        #endregion

        #region MerchantDiscount

        #region FindByMerchantDiscountId
        public MerchantDiscountDTO FindByMerchantDiscountId(int AMerchantDiscountId)
        {
            var merchantDiscount = objMerchantDiscountRepository.Get(AMerchantDiscountId);
            if (merchantDiscount != null)
            {
                objMerchantDiscountRepository.UnitOfWork.RefreshDataInModel(ref merchantDiscount);
                if (merchantDiscount.ObjMerchant == null)
                {
                    merchantDiscount.ObjMerchant = objMerchantsRepository.Get(merchantDiscount.MerchantCode, merchantDiscount.OrganizationCode);
                }
                if (merchantDiscount.ObjTransactionCheckFrequency == null)
                {
                    merchantDiscount.ObjTransactionCheckFrequency = objTransactionCheckFrequencyRepository.Get(merchantDiscount.TransactionCheckFrequencyCode);
                }

                return Conversion.Mapping.MerchantDiscountToMerchantDiscountDTO(merchantDiscount);
            }
            else
                return new MerchantDiscountDTO();
        }
        #endregion
        #region SaveMerchantDiscountDetails
        public void SaveMerchantDiscountDetails(MerchantDiscountDTO AMerchantDiscountDTO)
        {
            if (AMerchantDiscountDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddMerchantDiscountWithNullInformation");

            var newMerchantDiscount = MerchantDiscountFactory.CreateMerchantDiscount(AMerchantDiscountDTO.OrganizationCode, AMerchantDiscountDTO.MerchantCode,
                AMerchantDiscountDTO.EffectiveFromDate,
                AMerchantDiscountDTO.TransactionCheckFrequencyCode, AMerchantDiscountDTO.DiscountRate,
                AMerchantDiscountDTO.CreatedBy, AMerchantDiscountDTO.LastUpdatedBy, AMerchantDiscountDTO.ApprovalStatus);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objMerchantDiscountRepository.Add(newMerchantDiscount);
                        objMerchantDiscountRepository.UnitOfWork.Commit();

                        if (AMerchantDiscountDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };
                            paramCollection = GetApprovalQueueProcedureParameters("MT_MerchantDiscount", " MerchantDiscountId = '" + newMerchantDiscount.MerchantDiscountId + "'",
                                "MRCHDISC", AMerchantDiscountDTO.CreatedBy, "/Master/MerchantDiscountAuthorize/" + newMerchantDiscount.MerchantDiscountId + "/A?", "MT");
                            objMerchantsRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
        #region UpdateMerchantDiscountDetails

        public void UpdateMerchantDiscountDetails(int AMerchantDiscountId, MerchantDiscountDTO AMerchantDiscountDTO)
        {
            if (AMerchantDiscountDTO == null)
                throw new ArgumentException("Messages.warning_CannotUpdateMerchantDiscountWithNullInformation");

            MerchantDiscount currentMerchantDiscount = objMerchantDiscountRepository.Get(AMerchantDiscountId);
            MerchantDiscount updateMerchantDiscount = new MerchantDiscount()
            {
                MerchantDiscountId = AMerchantDiscountId,
                MerchantCode = AMerchantDiscountDTO.MerchantCode,
                OrganizationCode = Utils.OrganizationCode,
                EffectiveFromDate = AMerchantDiscountDTO.EffectiveFromDate,

                TransactionCheckFrequencyCode = AMerchantDiscountDTO.TransactionCheckFrequencyCode,
                DiscountRate = AMerchantDiscountDTO.DiscountRate,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = AMerchantDiscountDTO.LastUpdatedBy,
                CreatedOn = AMerchantDiscountDTO.CreatedOn,
                CreatedBy = AMerchantDiscountDTO.CreatedBy,
                ApprovalStatus = AMerchantDiscountDTO.ApprovalStatus
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
              new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objMerchantDiscountRepository.Merge(currentMerchantDiscount, updateMerchantDiscount);
                    objMerchantDiscountRepository.UnitOfWork.Commit();

                    if (AMerchantDiscountDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_MerchantDiscount", " MerchantDiscountId = '" + AMerchantDiscountId + "'",
                            "MRCHDISC", AMerchantDiscountDTO.LastUpdatedBy, "/Master/MerchantDiscountAuthorize/" + AMerchantDiscountId + "/A?", "MT");
                        objMerchantsRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #region FindAllMerchantDiscount
        public List<MerchantDiscountDTO> FindAll(int APageNo, MerchantDiscountDTO AMerchantDiscountDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "MerchantCode",FilterValue = AMerchantDiscountDTO.MerchantCode, Operator = "Contains" },                    
                      new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode, Operator = "Equals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                      new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
                      
                    };

            Expression<Func<MerchantDiscount, bool>> expression = Utils.GetExpression<MerchantDiscount>(lstExpression);
            int iRecordCount = objMerchantDiscountRepository.GetRecordCount(expression);

            var vMerchantDiscountDTO = objMerchantDiscountRepository.GetFiltered(0, iRecordCount, expression);

            if (vMerchantDiscountDTO != null && vMerchantDiscountDTO.Any())
            {
                List<MerchantDiscountDTO> lstMerchantDiscountDTO = new List<MerchantDiscountDTO>();
                foreach (var merchantDiscount in vMerchantDiscountDTO)
                {
                    lstMerchantDiscountDTO.Add(Conversion.Mapping.MerchantDiscountToMerchantDiscountDTO(merchantDiscount));
                }
                return lstMerchantDiscountDTO;
            }
            else
                return new List<MerchantDiscountDTO>();
        }
        #endregion
        #region FindMerchantDiscount
        public List<MerchantDiscountDTO> FindMerchantDiscount(int APageindex, int APageCount)
        {
            var merchantDiscount = objMerchantDiscountRepository.GetAll();
            if (merchantDiscount != null && merchantDiscount.Any())
            {
                List<MerchantDiscountDTO> lstMerchantDiscountDTO = new List<MerchantDiscountDTO>();
                foreach (var item in merchantDiscount)
                {
                    lstMerchantDiscountDTO.Add(Conversion.Mapping.MerchantDiscountToMerchantDiscountDTO(item));
                }
                return lstMerchantDiscountDTO;
            }
            else
                return new List<MerchantDiscountDTO>();
        }
        #endregion
        #region DeleteMerchantDiscount
        public void DeleteMerchantDiscount(int AMerchantDiscountId)
        {
            var merchantDiscount = objMerchantDiscountRepository.Get(AMerchantDiscountId);
            if (merchantDiscount != null)
            {
                objMerchantDiscountRepository.Remove(merchantDiscount);
                objMerchantDiscountRepository.UnitOfWork.Commit();
            }
        }
        #endregion
        #region CheckIfExistsValidDiscountForSelectedMerchant
        public bool CheckIfExistsValidDiscountForSelectedMerchant(int AMerchantDiscountId, string AMerchantCode, DateTime AFromDate)
        {
            var varMerchantDet = objMerchantDiscountRepository.AllMatching(x => x.MerchantCode == AMerchantCode
                && (x.ApprovalStatus != "DEL" || x.ApprovalStatus != "DIS")
                && x.EffectiveFromDate == AFromDate
                && x.MerchantDiscountId != AMerchantDiscountId);
            if (varMerchantDet.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #endregion

        #region MerchantTransactions
        public List<MerchantTransactionsDTO> GetMerchantTransactionDetails(string AMerchantCode, string AMerchantName, DateTime ATxnFromDate, DateTime ATxnToDate)
        {
            SqlParameter[] paramCollection = new SqlParameter[]
            { 
            new SqlParameter(){ ParameterName = "@P_MerchantCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = (AMerchantCode == null ? string.Empty : AMerchantCode) , Direction = System.Data.ParameterDirection.Input },
            new SqlParameter(){ ParameterName = "@P_MerchantName", SqlDbType = System.Data.SqlDbType.VarChar, Value = (AMerchantName == null ?string.Empty : AMerchantName), Direction = System.Data.ParameterDirection.Input },

            new SqlParameter(){ ParameterName = "@P_TxnFromDate", SqlDbType = System.Data.SqlDbType.DateTime, Value = ATxnFromDate == null ? DateTime.Parse("01/01/1753") : ATxnFromDate == DateTime.MinValue ? DateTime.Parse("01/01/1753") : ATxnFromDate, Direction = System.Data.ParameterDirection.Input },
            new SqlParameter(){ ParameterName = "@P_TxnToDate", SqlDbType = System.Data.SqlDbType.DateTime, Value = ATxnToDate == null ? Utils.BusinessDate : ATxnToDate == DateTime.MinValue ? Utils.BusinessDate : ATxnToDate, Direction = System.Data.ParameterDirection.Input },           

            };

            List<MerchantTransactionsDTO> objMerchantTransactionList = new List<MerchantTransactionsDTO>();
            objMerchantTransactionList = objMerchantsRepository.UnitOfWork.ExecuteQuery<MerchantTransactionsDTO>("PR_GetMerchantTransactionsList @P_MerchantCode, @P_MerchantName, @P_TxnFromDate,@P_TxnToDate",
                           paramCollection).ToList();

            return objMerchantTransactionList;
        }
        #endregion

        #region GetFeeTypes
        public FeeTypesDTO GetFeeTypes(string AFeeTypeCode)
        {
            FeeTypesDTO objFeeTypesDTO = new FeeTypesDTO();
            FeeTypes objFeeTypes = objFeeTypesRepository.Get(AFeeTypeCode);
            if (objFeeTypes != null)
            {
                objFeeTypesDTO = Conversion.Mapping.ConvertFeeTypesToFeeTypesDTO(objFeeTypes);
            }
            return objFeeTypesDTO;
        }
        #endregion

        #region Branch
        public List<BranchDTO> FindAll(int APageNo, BranchDTO ABranchDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "BranchCode",FilterValue = ABranchDTO.BranchCode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "BranchName",FilterValue = ABranchDTO.BranchName,Operator = "Contains"}
                    };

            Expression<Func<Branch, bool>> expression = Utils.GetExpression<Branch>(lstExpression);
            int iRecordCount = objBranchRepository.GetRecordCount(expression);

            //var vCityDTO = objCityRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vBranchDTO = objBranchRepository.GetFiltered(0, iRecordCount, expression);

            if (vBranchDTO != null && vBranchDTO.Any())
            {
                List<BranchDTO> lstBranchDTO = new List<BranchDTO>();
                foreach (var branch in vBranchDTO)
                {
                    lstBranchDTO.Add(Conversion.Mapping.BranchToBranchDTO(branch));
                }
                return lstBranchDTO;
            }
            else
                return new List<BranchDTO>();
        }
        public List<BranchDTO> FindBranch(int APageindex, int APageCount)
        {
            var Branches = objBranchRepository.GetAll();
            if (Branches != null && Branches.Any())
            {
                List<BranchDTO> lstBranchDTO = new List<BranchDTO>();
                foreach (var branch in Branches)
                {
                    lstBranchDTO.Add(Conversion.Mapping.BranchToBranchDTO(branch));
                }
                return lstBranchDTO;
            }
            else
                return new List<BranchDTO>();
        }

        public BranchDTO FindByBranchCode(string ABranchCode, string AOrganizationCode)
        {
            var Branch = objBranchRepository.Get(ABranchCode, AOrganizationCode);
            if (Branch != null)
            {
                objBranchRepository.UnitOfWork.RefreshDataInModel(ref Branch);

                //if (Branch.City == null)
                //{
                //    Branch.City = objCityRepository.Get(Branch.CityCode, Utils.OrganizationCode);
                //}
                //if (Branch.Country == null)
                //{
                //    Branch.Country = objCountryRepository.Get(Branch.CountryCode);
                //}
                //if (Branch.Region == null)
                //{
                //    Branch.Region = objRegionRepository.Get(Branch.RegionCode);
                //}
                //if (Branch.Communa == null)
                //{
                //    Branch.Communa = objCommunaRepository.Get(Branch.CommunaCode);
                //}

                return Conversion.Mapping.BranchToBranchDTO(Branch);
            }
            else
                return new BranchDTO();
        }

        public void SaveBranchDetails(BranchDTO ABranchDTO)
        {
            if (ABranchDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddBranchWithNullInformation");

            var newBranch = BranchFactory.CreateBranch( ABranchDTO.OrganizationCode, ABranchDTO.BranchCode, ABranchDTO.BranchName, 
                ABranchDTO.CountryCode, ABranchDTO.RegionCode, ABranchDTO.CommunaCode, ABranchDTO.CityCode, ABranchDTO.Address, ABranchDTO.DepId,
                ABranchDTO.CreatedBy, ABranchDTO.LastUpdatedBy, ABranchDTO.ApprovalStatus );
            
            try
            {

                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                        new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objBranchRepository.Add(newBranch);
                        objBranchRepository.UnitOfWork.Commit();

                        if (ABranchDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_Branch", " BranchCode = '" + ABranchDTO.BranchCode + "' and OrganizationCode = '" + ABranchDTO.OrganizationCode + "'",
                                "BRANCH", ABranchDTO.LastUpdatedBy, "/Master/BranchAuthorize?ABranchCode=" + ABranchDTO.BranchCode + "&AOrganizationCode=" + ABranchDTO.OrganizationCode + "&", "MT");

                            objBranchRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                scope.Dispose();

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void UpdateBranchDetails(string ABranchCode, BranchDTO ABranchDTO)
        {
            if (ABranchDTO == null)
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");
            Branch currentBranch = objBranchRepository.Get(ABranchCode, Utils.OrganizationCode);
            Branch updateBranch = new Branch()
            {
                OrganizationCode = Utils.OrganizationCode,
                BranchCode = ABranchCode,
                BranchName = ABranchDTO.BranchName,
                CountryCode = ABranchDTO.CountryCode,
                RegionCode = ABranchDTO.RegionCode,
                CommunaCode = ABranchDTO.CommunaCode,
                CityCode = ABranchDTO.CityCode,
                Address = ABranchDTO.Address,
                DepId = ABranchDTO.DepId,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = ABranchDTO.LastUpdatedBy,
                CreatedOn = currentBranch.CreatedOn,
                CreatedBy = currentBranch.CreatedBy,
                ApprovalStatus = ABranchDTO.ApprovalStatus
            };


            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objBranchRepository.Merge(currentBranch, updateBranch);
                    objBranchRepository.UnitOfWork.Commit();

                    if (ABranchDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
            
                        paramCollection = GetApprovalQueueProcedureParameters("MT_Branch", " BranchCode = '" + ABranchDTO.BranchCode + "' and OrganizationCode = '" + Utils.OrganizationCode + "'",
                            "BRANCH", ABranchDTO.LastUpdatedBy,
                             "/Master/BranchAuthorize?ABranchCode=" + ABranchDTO.BranchCode + "&AOrganizationCode=" + Utils.OrganizationCode + "&", "MT");

                        objBranchRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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

        #endregion

        #region ChangeBusinessDate
        #region FindByBusinessDateOrgCode
        public ChangeBusinessDateDTO FindByBusinessDateOrgCode(string AOrganizationCode)
        {
            ChangeBusinessDate ChangeBusinessDateDTO = objChangeBusinessDateRepository.Get(AOrganizationCode);
            if (ChangeBusinessDateDTO != null)
            {
                objChangeBusinessDateRepository.UnitOfWork.RefreshDataInModel(ref ChangeBusinessDateDTO);
                return Conversion.Mapping.ChangeBusinessDateToChangeBusinessDateDTO(ChangeBusinessDateDTO);
            }
            else
            {
                return new ChangeBusinessDateDTO();
            }
        }
        #endregion
        #endregion

        #region UpdateBusinessDate
        #region UpdateBusinessDate
        public void UpdateBusinessDate(ChangeBusinessDateDTO AChangeBusinessDateDTO)
        {
            SqlParameter[] paramCollection = new SqlParameter[]
            { 
                new SqlParameter(parameterName: "@P_OrganizationCode",dbType:System.Data.SqlDbType.VarChar ),
                new SqlParameter(parameterName: "@P_BusinessDate", dbType:System.Data.SqlDbType.DateTime )
            };

            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_OrganizationCode")).First()).Value = Utils.OrganizationCode;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_BusinessDate")).First()).Value = AChangeBusinessDateDTO.BusinessDate;
            
            objChangeBusinessDateRepository.UnitOfWork.ExecuteCommand("dbo.PR_UpdateBusinessDate @P_OrganizationCode, @P_BusinessDate", paramCollection);

        }
               
        #endregion
        #endregion

        #region IndustrialCustomer - KnockOut
        #region SaveIndustrialCustomerDetails
        public void SaveIndustrialCustomerDetails(int ACustomerId, IndustrialCustomerDTO AIndustrialCustomerDTO)
        {
            if (AIndustrialCustomerDTO == null)
            {
                throw new ArgumentException("");
            }

            System.Data.DataTable dtIndustrialCustomerAddress = new System.Data.DataTable();
            dtIndustrialCustomerAddress.Columns.AddRange(new System.Data.DataColumn[] { 
                new System.Data.DataColumn("AddressId", typeof(int)),
                new System.Data.DataColumn("CustomerId",typeof(int)),
                new System.Data.DataColumn("AddressTypeCode",typeof(string)),
                new System.Data.DataColumn("ReferenceAddressTypeCode", typeof(string)),
                new System.Data.DataColumn("AddressDesc",typeof(string)),
                new System.Data.DataColumn("Village", typeof(string)),
                new System.Data.DataColumn("CommunaCode",typeof(string)),
                new System.Data.DataColumn("CityCode",typeof(string)),
                new System.Data.DataColumn("Phone",typeof(string)),
                new System.Data.DataColumn("Mobile",typeof(string)),
                new System.Data.DataColumn("EmailID",typeof(string)),
                new System.Data.DataColumn("ResidenceType",typeof(string)),
                new System.Data.DataColumn("Other",typeof(string))
            });
        
            if (AIndustrialCustomerDTO.ObjIndustrialCustomerAddressDTO != null)
            {
                foreach (IndustrialCustomerAddressDTO objItem in AIndustrialCustomerDTO.ObjIndustrialCustomerAddressDTO)
                {
                    System.Data.DataRow dRowNew = dtIndustrialCustomerAddress.NewRow();
                    dRowNew["AddressId"] = objItem.AddressId;
                    dRowNew["CustomerId"] = objItem.CustomerId;
                    dRowNew["AddressTypeCode"] = objItem.AddressTypeCode;
                    dRowNew["ReferenceAddressTypeCode"] = objItem.ReferenceAddressTypeCode;
                    dRowNew["AddressDesc"] = objItem.AddressDesc;
                    dRowNew["Village"] = objItem.Village;
                    dRowNew["CommunaCode"] = objItem.CommunaCode;
                    dRowNew["CityCode"] = objItem.CityCode;
                    dRowNew["Phone"] = objItem.Phone;
                    dRowNew["Mobile"] = objItem.Mobile;
                    dRowNew["EmailID"] = objItem.EmailID;
                    dRowNew["ResidenceType"] = objItem.ResidenceType;
                    dRowNew["Other"] = objItem.Other;
                    dtIndustrialCustomerAddress.Rows.Add(dRowNew);
                }
            }

            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter(){ ParameterName = "@P_CustomerId", SqlDbType = System.Data.SqlDbType.Int, Value = AIndustrialCustomerDTO.CustomerId, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_ParentCustomerId", SqlDbType = System.Data.SqlDbType.Int, Value = AIndustrialCustomerDTO.ParentCustomerId, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_OrganizationCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.OrganizationCode, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_RUT", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.RUT, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_SalesId", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.SalesId == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.SalesId, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_ProductMemberTypeCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.ProductMemberTypeCode == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.ProductMemberTypeCode, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_ProductCategoryCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.ProductCategoryCode == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.ProductCategoryCode, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_ProductCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.ProductCode == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.ProductCode, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_BranchCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.BranchCode == null ? DBNull.Value.ToString() :  AIndustrialCustomerDTO.BranchCode, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_NationalityCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.Nationality == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.Nationality, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_LegacyCardNumber", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.LegacyCardNumber == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.LegacyCardNumber, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_CustomerType", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.CustomerType == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.CustomerType, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_Name", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.Name == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.Name, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_PaternalSurname", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.PaternalSurname == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.PaternalSurname, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_MaternalSurname", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.MaternalSurname == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.MaternalSurname, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_DateOfBirth", SqlDbType = System.Data.SqlDbType.DateTime, Value = AIndustrialCustomerDTO.DateOfBirth == null ? DateTime.Parse("01/01/1753") : AIndustrialCustomerDTO.DateOfBirth == DateTime.MinValue ? DateTime.Parse("01/01/1753") : AIndustrialCustomerDTO.DateOfBirth, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_AgeYears", SqlDbType = System.Data.SqlDbType.Int, Value = AIndustrialCustomerDTO.AgeYears, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_AgeMonths", SqlDbType = System.Data.SqlDbType.Int, Value = AIndustrialCustomerDTO.AgeMonths, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_AlcoholLicenseFlag", SqlDbType = System.Data.SqlDbType.Bit, Value = AIndustrialCustomerDTO.AlcoholLicenseFlag, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_CurrentAccountNo", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.CurrentAccountNo ==null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.CurrentAccountNo, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_BankCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.BankCode ==null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.BankCode, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_CreditLineRequestedFlag", SqlDbType = System.Data.SqlDbType.Bit, Value = AIndustrialCustomerDTO.CreditLineRequestedFlag, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_Relevance", SqlDbType = System.Data.SqlDbType.Decimal, Value = AIndustrialCustomerDTO.Relevance, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_GrowthPotentialPercentage", SqlDbType = System.Data.SqlDbType.Decimal, Value = AIndustrialCustomerDTO.GrowthPotentialPercentage, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_PurchaseConsumptionOccasion", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.PurchaseConsumptionOccasion == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.PurchaseConsumptionOccasion, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_CreditBureauDicomFlag", SqlDbType = System.Data.SqlDbType.Bit, Value = AIndustrialCustomerDTO.CreditBureauDicomFlag, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_CreditBureauScore", SqlDbType = System.Data.SqlDbType.Decimal, Value = AIndustrialCustomerDTO.CreditBureauScore, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_CreatedBy", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.CreatedBy == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.CreatedBy, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_LastUpdatedBy", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.LastUpdatedBy == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.LastUpdatedBy, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_ApprovalStatus", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.ApprovalStatus, Direction = System.Data.ParameterDirection.Input },

                new SqlParameter(){ ParameterName = "@P_IndustrialCustomerAddress", SqlDbType= System.Data.SqlDbType.Structured, Value = dtIndustrialCustomerAddress, TypeName = "UD_IndustrialCustomerAddress", Direction = System.Data.ParameterDirection.Input},
                new SqlParameter(){ ParameterName = "@P_IsAutoApprove", SqlDbType = System.Data.SqlDbType.Bit, Value = AIndustrialCustomerDTO.IsAutoApprove, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_NeedToSendToApprovalQueue", SqlDbType = System.Data.SqlDbType.Bit, Value = AIndustrialCustomerDTO.NeedToSendToApprovalQueue, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_IsCustomerForUnderWriting", SqlDbType = System.Data.SqlDbType.Bit, Value = AIndustrialCustomerDTO.IsCustomerForUnderWriting, Direction = System.Data.ParameterDirection.Input },

                new SqlParameter(){ ParameterName = "@P_DecisionCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.FirstOrDefault().DecisionCode  == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.FirstOrDefault().DecisionCode, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_ReasonCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.FirstOrDefault().ReasonCode  == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.FirstOrDefault().ReasonCode, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_Remarks", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.FirstOrDefault().Remarks  == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.FirstOrDefault().Remarks, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_ContractCode", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.FirstOrDefault().ContractCode == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.FirstOrDefault().ContractCode, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_Unemployment", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.FirstOrDefault().Unemployment == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.FirstOrDefault().Unemployment, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_IsContractAcceptedByCustomer", SqlDbType = System.Data.SqlDbType.Bit, Value = AIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.FirstOrDefault().IsContractAcceptedByCustomer, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_CreditLimit", SqlDbType = System.Data.SqlDbType.Decimal, Value = AIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.FirstOrDefault().CreditLimit, Direction = System.Data.ParameterDirection.Input },

                new SqlParameter(){ ParameterName = "@P_BillCycleSysParamId", SqlDbType = System.Data.SqlDbType.Int, Value = AIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.FirstOrDefault().BillCycleSysParamId, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_BillingCycleCode",  SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.FirstOrDefault().BillingCycleCode == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.FirstOrDefault().BillingCycleCode, Direction = System.Data.ParameterDirection.Input }
                    
            };

            objCustomerRepository.UnitOfWork.ExecuteCommand(@"PR_SaveIndustrialCustomer @P_CustomerId,@P_ParentCustomerId, @P_OrganizationCode, 
                                    @P_RUT, @P_SalesId,@P_ProductMemberTypeCode, @P_ProductCategoryCode, @P_ProductCode, 
                                    @P_BranchCode, @P_NationalityCode, @P_LegacyCardNumber, @P_CustomerType, @P_Name, @P_PaternalSurname, 
                                    @P_MaternalSurname, @P_DateOfBirth, @P_AgeYears, @P_AgeMonths, @P_AlcoholLicenseFlag, @P_CurrentAccountNo, 
                                    @P_BankCode, @P_CreditLineRequestedFlag, @P_Relevance, @P_GrowthPotentialPercentage, 
                                    @P_PurchaseConsumptionOccasion, @P_CreditBureauDicomFlag, @P_CreditBureauScore, @P_CreatedBy,
                                    @P_LastUpdatedBy, @P_ApprovalStatus, @P_IndustrialCustomerAddress,
                                    @P_IsAutoApprove,@P_NeedToSendToApprovalQueue,@P_IsCustomerForUnderWriting,@P_DecisionCode,
                                    @P_ReasonCode,@P_Remarks,@P_ContractCode,@P_Unemployment,@P_IsContractAcceptedByCustomer,@P_CreditLimit,
                                    @P_BillCycleSysParamId,@P_BillingCycleCode", parameters);
           
        }
        #endregion

        #region DeleteIndustrialCustomer
		public int DeleteIndustrialCustomer(int ACustomerId, IndustrialCustomerDTO AIndustrialCustomerDTO)
        {
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter(){ ParameterName = "@P_CustomerId", SqlDbType = System.Data.SqlDbType.Int, Value = AIndustrialCustomerDTO.CustomerId, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_LastUpdatedBy", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.LastUpdatedBy == null ? DBNull.Value.ToString() : AIndustrialCustomerDTO.LastUpdatedBy, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_ApprovalStatus", SqlDbType = System.Data.SqlDbType.VarChar, Value = AIndustrialCustomerDTO.ApprovalStatus, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_ReturnValue", SqlDbType = System.Data.SqlDbType.TinyInt, Direction = System.Data.ParameterDirection.Output },
            };
            objCustomerRepository.UnitOfWork.ExecuteCommand(@"PR_DeleteIndustrialCustomer @P_CustomerId,@P_LastUpdatedBy,@P_ApprovalStatus,@P_ReturnValue OUTPUT", parameters);
            int retValue = Convert.ToInt32(parameters[3].Value);
            return retValue;
        }
	    #endregion        

        #region GetIndustrialCustomerByCustomerId
        public IndustrialCustomerDTO GetIndustrialCustomerByCustomerId(int ACustomerId)
        {
            IndustrialCustomerDTO objIndustrialCustomerDTO = new IndustrialCustomerDTO();
            List<IndustrialCustomerAddressDTO> objIndustrialCustomerAddressDTO = new List<IndustrialCustomerAddressDTO>();
            List<IndustrialCustomerDecisionDTO> objIndustrialCustomerDecisionDTO = new List<IndustrialCustomerDecisionDTO>();
            SqlParameter[] parameters1 = new SqlParameter[] { 
                new SqlParameter() { ParameterName = "@P_CustomerId", SqlDbType = System.Data.SqlDbType.VarChar, Size = 25, Value = ACustomerId, Direction = System.Data.ParameterDirection.Input}};
            SqlParameter[] parameters2 = new SqlParameter[] { 
                new SqlParameter() { ParameterName = "@P_CustomerId", SqlDbType = System.Data.SqlDbType.VarChar, Size = 25, Value = ACustomerId, Direction = System.Data.ParameterDirection.Input}};
            SqlParameter[] parameters3 = new SqlParameter[] { 
                new SqlParameter() { ParameterName = "@P_CustomerId", SqlDbType = System.Data.SqlDbType.VarChar, Size = 25, Value = ACustomerId, Direction = System.Data.ParameterDirection.Input}};
            objIndustrialCustomerDTO = objCustomerRepository.UnitOfWork.ExecuteQuery<IndustrialCustomerDTO>("PR_GetIndustrialCustomerByCustomerId @P_CustomerId", parameters1).ToList().FirstOrDefault();
            objIndustrialCustomerAddressDTO = objCustomerRepository.UnitOfWork.ExecuteQuery<IndustrialCustomerAddressDTO>("PR_GetIndustrialCustomerAddressByCustomerId @P_CustomerId", parameters2).ToList();
            objIndustrialCustomerDecisionDTO = objCustomerRepository.UnitOfWork.ExecuteQuery<IndustrialCustomerDecisionDTO>("PR_GetIndustrialCustomerDecisionByCustomerId @P_CustomerId", parameters3).ToList();

            if (objIndustrialCustomerDTO != null)
            {
                objIndustrialCustomerDTO.ApprovalStatusDesc = Utils.GetRecordStatus(objIndustrialCustomerDTO.ApprovalStatus);
                objIndustrialCustomerDTO.ObjIndustrialCustomerAddressDTO = objIndustrialCustomerAddressDTO;
                objIndustrialCustomerDTO.ObjIndustrialCustomerDecisionDTO = objIndustrialCustomerDecisionDTO;
            }

            return objIndustrialCustomerDTO;
        } 
        #endregion

        #region GetDirectSalesAgentDetails
        public DirectSalesAgentDTO GetDirectSalesAgentDetails(string ARUT,string ASalesId)
        {
            DirectSalesAgentDTO objDirectSalesAgentDTO = new DirectSalesAgentDTO();

            ARUT = ARUT.Replace("-", "");

            var result = objSalesIdRepository.AllMatching(x => x.RUT == ARUT && x.SalesId == ASalesId && x.OrganizationCode == Utils.OrganizationCode)
                .Join(objDirectSalesAgentRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode),
                    SalesIds => SalesIds.BranchCode,
                    DSA => DSA.CommercialBranchCode,
                    (SalesIds,DSA) => new { DSA }).FirstOrDefault();

            if (result != null)
            {
                objDirectSalesAgentDTO.DSACode = result.DSA.DSACode;
                objDirectSalesAgentDTO.DSAName = result.DSA.DSAName;
                objDirectSalesAgentDTO.UEN = result.DSA.UEN;
                objDirectSalesAgentDTO.RUT = result.DSA.RUT;
                objDirectSalesAgentDTO.CommercialBranchCode = result.DSA.CommercialBranchCode;
            }
            else 
            {
                objDirectSalesAgentDTO.CommercialBranchCode = objSalesIdRepository.Get(ASalesId,ARUT,Utils.OrganizationCode).BranchCode;
            }
            return objDirectSalesAgentDTO;
        } 
        #endregion

        #region PopulateMtCustomerOnIndustrialCustomerApproval
        public int PopulateMtCustomerOnIndustrialCustomerApproval(string AQueueRefNo)
        {
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter() { ParameterName = "@P_QueueReferenceNo", SqlDbType = System.Data.SqlDbType.VarChar, Size = 25, Value = AQueueRefNo, Direction = System.Data.ParameterDirection.Input}};
            int retValue = objCustomerRepository.UnitOfWork.ExecuteCommand("PR_PopulateMtCustomerOnApproval @P_QueueReferenceNo", parameters);
            return retValue;
        } 
        #endregion

        #region GetCustomersForListing
        public List<CustomerDTO> GetCustomersForListing(string ARUT, string ACustomerType, string AName,string ACustomerStatus, string AOrganizationCode, bool AIsRequestFromUnderWriter)
        {
            List<CustomerDTO> objCustomers = new List<CustomerDTO>();
            try
            {                
                SqlParameter[] parameter = new SqlParameter[]{
                new SqlParameter(){ ParameterName = "@P_RUT", Value = ARUT == null ? DBNull.Value.ToString() : ARUT, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_CustomerType", Value = ACustomerType == null ? DBNull.Value.ToString() : ACustomerType, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_Name", Value = AName == null ? DBNull.Value.ToString() : AName, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_CustomerStatus", Value = ACustomerStatus == null ? DBNull.Value.ToString() : ACustomerStatus, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_OrganizationCode", Value = AOrganizationCode == null ? DBNull.Value.ToString() : AOrganizationCode, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input },
                new SqlParameter(){ ParameterName = "@P_IsRequestFromUnderWriter", Value = AIsRequestFromUnderWriter, SqlDbType = System.Data.SqlDbType.Bit, Direction = System.Data.ParameterDirection.Input }                
                
                };
                objCustomers = objCustomerRepository.UnitOfWork.ExecuteQuery<CustomerDTO>("PR_GetCustomersForListing @P_RUT,@P_CustomerType,@P_Name,@P_CustomerStatus,@P_OrganizationCode,@P_IsRequestFromUnderWriter", parameter).ToList();
                objCustomers.ForEach(x => x.CustomerTypeDesc = (x.CustomerType == "I" ? CC_Ingenio_Resources.Resources.Individual : "Company"));
                objCustomers.ForEach(x => x.ApprovalStatusDesc = Utils.GetRecordStatus(x.ApprovalStatus));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCustomers;
        } 
        #endregion

        #region GetCustomerDetailsIfExists
        public object GetCustomerDetailsIfExists(string ARUT, string ASalesId)
        {
            object objCustomers;
            if (Utils.OrganizationTypeCode == "IND")
            {
                objCustomers = new IndustrialCustomerDTO();
            }
            else
            {
                objCustomers = new CustomerDTO();
            }
            try
            {
                if (Utils.OrganizationTypeCode == "IND")
                {
                    SqlParameter[] parameter = new SqlParameter[]{
                        new SqlParameter(){ ParameterName = "@P_RUT", Value = ARUT == null ? DBNull.Value.ToString() : ARUT, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input },
                        new SqlParameter(){ ParameterName = "@P_SalesId", Value = ASalesId == null ? DBNull.Value.ToString() : ASalesId, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input }
                        };
                    objCustomers = objCustomerRepository.UnitOfWork.ExecuteQuery<IndustrialCustomerDTO>("PR_GetCustomerDetailsIfExists @P_RUT,@P_SalesId", parameter).ToList().FirstOrDefault();
                    if (objCustomers == null)
                    {
                        objCustomers = new IndustrialCustomerDTO();
                    }
                }
                else
                {
                    SqlParameter[] parameter = new SqlParameter[]{
                        new SqlParameter(){ ParameterName = "@P_RUT", Value = ARUT == null ? DBNull.Value.ToString() : ARUT, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input },
                        new SqlParameter(){ ParameterName = "@P_SalesId", Value = ASalesId == null ? DBNull.Value.ToString() : ASalesId, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input }
                        };
                    objCustomers = objCustomerRepository.UnitOfWork.ExecuteQuery<CustomerDTO>("PR_GetCustomerDetailsIfExists @P_RUT,@P_SalesId", parameter).ToList().FirstOrDefault();
                    if (objCustomers == null)
                    {
                        objCustomers = new CustomerDTO();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCustomers;
        }
        #endregion

        #region ValidateCustomerWithoutSalesId
        public CustomerInfo ValidateCustomer(string ARUT,string AOrganizationCode, string AUserName)
        {
            CustomerInfo objCustomerInfo = new CustomerInfo();
            try
            {
                SqlParameter[] parameter = new SqlParameter[]{
                    new SqlParameter(){ ParameterName = "@P_RUT", Value = ARUT == null ? DBNull.Value.ToString() : ARUT, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input },
                    new SqlParameter(){ ParameterName = "@P_OrganizationCode", Value = AOrganizationCode == null ? DBNull.Value.ToString() : AOrganizationCode, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input },
                    new SqlParameter(){ ParameterName = "@P_UserName", Value = AUserName == null ? DBNull.Value.ToString() : AUserName, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input }
                };
                objCustomerInfo = objCustomerRepository.UnitOfWork.ExecuteQuery<CustomerInfo>("PR_ValidateCustomerWithoutSalesId @P_RUT,@P_OrganizationCode,@P_UserName", parameter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCustomerInfo;
        }
        #endregion

        #region ValidateCustomerWithoutSalesId
        public CustomerInfo ValidateCustomerWithoutSalesId(string ARUT,string AProductMemberType, string AParentCustId, string AOrganizationCode)
        {
            CustomerInfo objCustomerInfo = new CustomerInfo();
            try
            {
                SqlParameter[] parameter = new SqlParameter[]{
                    new SqlParameter(){ ParameterName = "@P_RUT", Value = ARUT == null ? DBNull.Value.ToString() : ARUT, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input },
                    new SqlParameter(){ ParameterName = "@P_ProductMemberTypeCode", Value = AProductMemberType == null ? DBNull.Value.ToString() : AProductMemberType, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input },
                    new SqlParameter(){ ParameterName = "@P_ParentCustId", SqlDbType = System.Data.SqlDbType.Int, Value = string.IsNullOrEmpty(AParentCustId) ? 0 : Convert.ToInt32(AParentCustId), Direction = System.Data.ParameterDirection.Input },
                    new SqlParameter(){ ParameterName = "@P_OrganizationCode", Value = AOrganizationCode == null ? DBNull.Value.ToString() : AOrganizationCode, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input }
                };
                objCustomerInfo = objCustomerRepository.UnitOfWork.ExecuteQuery<CustomerInfo>("PR_ValidateCustomerWithoutSalesId @P_RUT,@P_ProductMemberTypeCode,@P_ParentCustId,@P_OrganizationCode", parameter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCustomerInfo;
        } 
        #endregion


        #region ValidateCustomer
        public CustomerInfo ValidateCustomer(string ARUT, string ASalesId, string AOrganizationCode, string AUserName)
        {
            CustomerInfo objCustomerInfo = new CustomerInfo();           
            try
            {               
                SqlParameter[] parameter = new SqlParameter[]{
                    new SqlParameter(){ ParameterName = "@P_RUT", Value = ARUT == null ? DBNull.Value.ToString() : ARUT, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input },
                    new SqlParameter(){ ParameterName = "@P_SelectedSalesId", Value = ASalesId == null ? DBNull.Value.ToString() : ASalesId, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input },
                    new SqlParameter(){ ParameterName = "@P_OrganizationCode", Value = AOrganizationCode == null ? DBNull.Value.ToString() : AOrganizationCode, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input },
                    new SqlParameter(){ ParameterName = "@P_UserName", Value = AUserName == null ? DBNull.Value.ToString() : AUserName, SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input }
                };
                objCustomerInfo = objCustomerRepository.UnitOfWork.ExecuteQuery<CustomerInfo>("PR_ValidateCustomer @P_RUT,@P_SelectedSalesId,@P_OrganizationCode,@P_UserName", parameter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCustomerInfo;
        }        
        #endregion

        #endregion

        #region UFRateSettings

        #region FilterUFRateSettings
        public List<UFRateSettingsDTO> FilterUFRateSettings(int ABillMonth, int ABillYear)
        {
            var ufRateSettings = objUFRateSettingsRepository.GetAll();

            if (ABillMonth != 0 && ABillYear != 0)
            {
                ufRateSettings = ufRateSettings.Where(x => x.EffectiveDate.Month == ABillMonth && x.EffectiveDate.Year == ABillYear);
            }
            
            
            if (ufRateSettings != null && ufRateSettings.Any())
            {
                List<UFRateSettingsDTO> lstUFRateSettingsDTO = new List<UFRateSettingsDTO>();
                foreach (var ufRateSetting in ufRateSettings)
                {
                    lstUFRateSettingsDTO.Add(Conversion.Mapping.UFRateSettingsToUFRateSettingsDTO(ufRateSetting));
                }
                return lstUFRateSettingsDTO;
            }
            else
                return new List<UFRateSettingsDTO>();
        }
        #endregion

        #region FindByUFRateSettingsEffectiveDate
        public UFRateSettingsDTO FindByUFRateSettingsEffectiveDate(DateTime AEffectiveDate)
        {
            UFRateSettings UFRateSettingsDTO = objUFRateSettingsRepository.Get(Utils.OrganizationCode, AEffectiveDate);
            if (UFRateSettingsDTO != null)
            {
                objChangeBusinessDateRepository.UnitOfWork.RefreshDataInModel(ref UFRateSettingsDTO);
                return Conversion.Mapping.UFRateSettingsToUFRateSettingsDTO(UFRateSettingsDTO);
            }
            else
            {
                return new UFRateSettingsDTO();
            }
        }
        #endregion

        #region FindUFRateSettings
        public List<UFRateSettingsDTO> FindUFRateSettings(int APageindex, int APageCount)
        {
            var ufRateSettings = objUFRateSettingsRepository.GetAll();
            if (ufRateSettings != null && ufRateSettings.Any())
            {
                List<UFRateSettingsDTO> lstUFRateSettingsDTO = new List<UFRateSettingsDTO>();
                foreach (var ufRateSetting in ufRateSettings)
                {
                    lstUFRateSettingsDTO.Add(Conversion.Mapping.UFRateSettingsToUFRateSettingsDTO(ufRateSetting));
                }
                return lstUFRateSettingsDTO;
            }
            else
                return new List<UFRateSettingsDTO>();
        }
        #endregion

        #region SaveUFRateSettings
        public void SaveUFRateSettings(UFRateSettingsDTO AUFRateSettingsDTO)
        {
            if (AUFRateSettingsDTO == null)
                throw new ArgumentException("Messages.warning_CannotUFRateSettingsWithNullInformation");

            var newUFRateSettings = UFRateSettingsFactory.CreateUFRateSettings(AUFRateSettingsDTO.OrganizationCode, AUFRateSettingsDTO.EffectiveDate,
                AUFRateSettingsDTO.UFRate, AUFRateSettingsDTO.CreatedBy, AUFRateSettingsDTO.CreatedOn, AUFRateSettingsDTO.LastUpdatedOn, AUFRateSettingsDTO.LastUpdatedBy);

            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objUFRateSettingsRepository.Add(newUFRateSettings);
                        objUFRateSettingsRepository.UnitOfWork.Commit();

                        scope.Complete();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region UpdateUFRateSettings
        public void UpdateUFRateSettings(DateTime AEffectiveDate, UFRateSettingsDTO AUFRateSettingsDTO)
        {
            if (AUFRateSettingsDTO == null)
                throw new ArgumentException("Messages.warning_CannotUpdateUFRateSettingsWithNullInformation");

            UFRateSettings currentUFRateSettings = objUFRateSettingsRepository.Get(Utils.OrganizationCode, AUFRateSettingsDTO.EffectiveDate);
            UFRateSettings updateUFRateSettings = new UFRateSettings()
            {
                OrganizationCode = Utils.OrganizationCode,
                EffectiveDate = AEffectiveDate,
                UFRate = AUFRateSettingsDTO.UFRate,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = AUFRateSettingsDTO.LastUpdatedBy,
                CreatedOn = currentUFRateSettings.CreatedOn,
                CreatedBy = currentUFRateSettings.CreatedBy
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
              new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objUFRateSettingsRepository.Merge(currentUFRateSettings, updateUFRateSettings);
                    objUFRateSettingsRepository.UnitOfWork.Commit();

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        //#region FindAllUFRateSettings
        //public List<UFRateSettingsDTO> FindAll(int APageNo, UFRateSettingsDTO AUFRateSettingsDTO)
        //{
        //    List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
        //            { new Utils.ExpressionList() { FilterColumn = "MerchantCode",FilterValue = AUFRateSettingsDTO.MerchantCode, Operator = "Contains" },
        //              new Utils.ExpressionList() { FilterColumn = "MerchantName",FilterValue = AUFRateSettingsDTO.MerchantName,Operator = "Contains"},
        //              new Utils.ExpressionList() { FilterColumn = "CountryCode",FilterValue = AUFRateSettingsDTO.CountryCode,Operator = "Contains" },
        //              new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode, Operator = "Equals"},
        //              new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
        //              new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
                      
        //            };

        //    Expression<Func<Merchants, bool>> expression = Utils.GetExpression<Merchants>(lstExpression);
        //    int iRecordCount = objMerchantsRepository.GetRecordCount(expression);

        //    var vMerchantsDTO = objMerchantsRepository.GetFiltered(0, iRecordCount, expression);

        //    if (vMerchantsDTO != null && vMerchantsDTO.Any())
        //    {
        //        List<MerchantsDTO> lstMerchantsDTO = new List<MerchantsDTO>();
        //        foreach (var merchant in vMerchantsDTO)
        //        {
        //            lstMerchantsDTO.Add(Conversion.Mapping.MerchantsToMerchantsDTO(merchant));
        //        }
        //        return lstMerchantsDTO;
        //    }
        //    else
        //        return new List<MerchantsDTO>();
        //}
        //#endregion

        #endregion

        #region DirectSalesAgent

        #region FindAll
        public List<DirectSalesAgentDTO> FindAll(int APageNo, DirectSalesAgentDTO ADirectSalesAgentDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "DSACode",FilterValue = ADirectSalesAgentDTO.DSACode, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "DSAName",FilterValue = ADirectSalesAgentDTO.DSAName,Operator = "Contains"}
                    };

            Expression<Func<DirectSalesAgent, bool>> expression = Utils.GetExpression<DirectSalesAgent>(lstExpression);
            int iRecordCount = objDirectSalesAgentRepository.GetRecordCount(expression);

            //var vCityDTO = objCityRepository.GetFiltered(APageNo, iRecordCount, expression);
            var vDirectSalesAgentDTO = objDirectSalesAgentRepository.GetFiltered(0, iRecordCount, expression);

            if (vDirectSalesAgentDTO != null && vDirectSalesAgentDTO.Any())
            {
                List<DirectSalesAgentDTO> lstDirectSalesAgentDTO = new List<DirectSalesAgentDTO>();
                foreach (var directSalesAgent in vDirectSalesAgentDTO)
                {
                    lstDirectSalesAgentDTO.Add(Conversion.Mapping.DirectSalesAgentToDirectSalesAgentDTO(directSalesAgent));
                }
                return lstDirectSalesAgentDTO;
            }
            else
                return new List<DirectSalesAgentDTO>();
        } 
        #endregion

        #region FindDirectSalesAgent
        public List<DirectSalesAgentDTO> FindDirectSalesAgent(int APageindex, int APageCount)
        {
            var directSalesAgents = objDirectSalesAgentRepository.GetAll();
            if (directSalesAgents != null && directSalesAgents.Any())
            {
                List<DirectSalesAgentDTO> lstDirectSalesAgentDTO = new List<DirectSalesAgentDTO>();
                foreach (var directSalesAgent in directSalesAgents)
                {
                    lstDirectSalesAgentDTO.Add(Conversion.Mapping.DirectSalesAgentToDirectSalesAgentDTO(directSalesAgent));
                }
                return lstDirectSalesAgentDTO;
            }
            else
                return new List<DirectSalesAgentDTO>();
        } 
        #endregion

        #region FindDSAByCommercialBranchCode
        public DirectSalesAgentDTO FindDSAByCommercialBranchCode(string ABranchCode)
        {
            var DirectSalesAgent = objDirectSalesAgentRepository.GetAll().Where(x => x.CommercialBranchCode == ABranchCode).FirstOrDefault();
            DirectSalesAgentDTO objDirectSalesAgentDTO = new DirectSalesAgentDTO();
            if(DirectSalesAgent != null)
                objDirectSalesAgentDTO = Conversion.Mapping.DirectSalesAgentToDirectSalesAgentDTO(DirectSalesAgent);
            return objDirectSalesAgentDTO;
        }
        #endregion
        #region FindByDirectSalesAgent
        public DirectSalesAgentDTO FindByDirectSalesAgent(string ADSACode)
        {
            var DirectSalesAgent = objDirectSalesAgentRepository.Get(ADSACode, Utils.OrganizationCode);
            if (DirectSalesAgent != null)
            {
                objDirectSalesAgentRepository.UnitOfWork.RefreshDataInModel(ref DirectSalesAgent);

                return Conversion.Mapping.DirectSalesAgentToDirectSalesAgentDTO(DirectSalesAgent);
            }
            else
                return new DirectSalesAgentDTO();
        } 
        #endregion

        #region SaveDirectSalesAgent
        public void SaveDirectSalesAgent(DirectSalesAgentDTO ADirectSalesAgentDTO)
        {
            if (ADirectSalesAgentDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddDirectSalesAgentWithNullInformation");            

            var newDirectSalesAgent = DirectSalesAgentFactory.CreateDirectSalesAgent(ADirectSalesAgentDTO.OrganizationCode, ADirectSalesAgentDTO.DSACode,
                ADirectSalesAgentDTO.DSAName, ADirectSalesAgentDTO.UEN, ADirectSalesAgentDTO.RUT, ADirectSalesAgentDTO.CommercialBranchCode,
                ADirectSalesAgentDTO.CreatedBy, ADirectSalesAgentDTO.LastUpdatedBy, ADirectSalesAgentDTO.ApprovalStatus);

            try
            {

                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                        new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objDirectSalesAgentRepository.Add(newDirectSalesAgent);
                        objDirectSalesAgentRepository.UnitOfWork.Commit();

                        if (ADirectSalesAgentDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_DirectSalesAgent", " DSACode = '" + ADirectSalesAgentDTO.DSACode + "' and OrganizationCode = '" + ADirectSalesAgentDTO.OrganizationCode + "'",
                                "DSACNTXT", ADirectSalesAgentDTO.LastUpdatedBy, "/Master/DSAAuthorize?ADSACode=" + ADirectSalesAgentDTO.DSACode + "&AOrganizationCode=" + ADirectSalesAgentDTO.OrganizationCode + "&", "MT");

                            objDirectSalesAgentRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                scope.Dispose();

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        } 
        #endregion

        #region UpdateDirectSalesAgent
        public void UpdateDirectSalesAgent(string ADSACode, DirectSalesAgentDTO ADirectSalesAgentDTO)
        {
            if (ADirectSalesAgentDTO == null)
                throw new ArgumentException("Messages.warning_CannotUpdateSalesIdWithNullInformation");
            DirectSalesAgent currentDirectSalesAgent = objDirectSalesAgentRepository.Get(ADSACode, Utils.OrganizationCode);
            DirectSalesAgent updateDirectSalesAgent = new DirectSalesAgent()
            {
                OrganizationCode = Utils.OrganizationCode,
                DSACode = ADSACode,
                DSAName = ADirectSalesAgentDTO.DSAName,
                UEN = ADirectSalesAgentDTO.UEN,
                RUT = ADirectSalesAgentDTO.RUT.Replace("-", ""),
                CommercialBranchCode = ADirectSalesAgentDTO.CommercialBranchCode,

                LastUpdatedOn = Utils.BusinessDate.Date,
                LastUpdatedBy = ADirectSalesAgentDTO.LastUpdatedBy,
                CreatedBy = currentDirectSalesAgent.CreatedBy,
                CreatedOn = currentDirectSalesAgent.CreatedOn,

                ApprovalStatus = ADirectSalesAgentDTO.ApprovalStatus
            };


            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objDirectSalesAgentRepository.Merge(currentDirectSalesAgent, updateDirectSalesAgent);
                    objDirectSalesAgentRepository.UnitOfWork.Commit();

                    if (ADirectSalesAgentDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_DirectSalesAgent", " DSACode = '" + ADSACode + "' and OrganizationCode = '" + currentDirectSalesAgent.OrganizationCode + "'",
                            "DSACNTXT", ADirectSalesAgentDTO.LastUpdatedBy, "/Master/DSAAuthorize?ADSACode=" + ADSACode + "&AOrganizationCode=" + currentDirectSalesAgent.OrganizationCode + "&", "MT");

                        objDirectSalesAgentRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #endregion

        #endregion

        #region Sales Id Master
        #region FindAll
        public List<SalesIdDTO> FindAll(int APageNo, SalesIdDTO ASalesIdDTO)
        {
            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "SalesId",FilterValue = ASalesIdDTO.SalesId, Operator = "Contains" },
                      new Utils.ExpressionList() { FilterColumn = "BranchCode",FilterValue = ASalesIdDTO.BranchCode,Operator = "Contains"},
                      new Utils.ExpressionList() { FilterColumn = "RUT",FilterValue = ASalesIdDTO.RUT == null ? string.Empty : ASalesIdDTO.RUT.Replace("-",string.Empty), Operator = "Contains"}
                    };

            Expression<Func<SaleId, bool>> expression = Utils.GetExpression<SaleId>(lstExpression);
            int iRecordCount = objSalesIdRepository.GetRecordCount(expression);

            var vSalesDTO = objSalesIdRepository.GetFiltered(0, iRecordCount, expression);

            if (vSalesDTO != null && vSalesDTO.Any())
            {
                List<SalesIdDTO> lstSalesIdDTO = new List<SalesIdDTO>();
                foreach (var salesid in vSalesDTO)
                {
                    lstSalesIdDTO.Add(Conversion.Mapping.SalesIdToSalesIdDTO(salesid));
                }
                return lstSalesIdDTO;
            }
            else
                return new List<SalesIdDTO>();
        }
        #endregion
        #region FindSalesId
        public List<SalesIdDTO> FindSalesId(int APageindex, int APageCount)
        {
            var salesId = objSalesIdRepository.GetAll();
            if (salesId != null && salesId.Any())
            {
                List<SalesIdDTO> lstSalesIdDTO = new List<SalesIdDTO>();
                foreach (var saleid in salesId)
                {
                    lstSalesIdDTO.Add(Conversion.Mapping.SalesIdToSalesIdDTO(saleid));
                }
                return lstSalesIdDTO;
            }
            else
                return new List<SalesIdDTO>();
        }
        #endregion
        #region FindBySalesId
        public SalesIdDTO FindBySalesId(string ASalesId, string ARUT, string AOrganizationCode)
        {
            var Sales = objSalesIdRepository.Get(ASalesId, ARUT, AOrganizationCode);
            if (Sales != null)
            {
                objSalesIdRepository.UnitOfWork.RefreshDataInModel(ref Sales);

                return Conversion.Mapping.SalesIdToSalesIdDTO(Sales);
            }
            else
                return new SalesIdDTO();
        }
        #endregion
        #region SaveSalesDetails
        public void SaveSalesDetails(SalesIdDTO ASalesDTO)
        {
            if (ASalesDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddSalesWithNullInformation");
            string RUT = ASalesDTO.RUT.Replace("-", "").ToString();

            var newSalesID = SalesIdFactory.CreateSalesID(ASalesDTO.OrganizationCode, ASalesDTO.SalesId, RUT, ASalesDTO.BranchCode, ASalesDTO.Address,
                ASalesDTO.CommunaCode, ASalesDTO.CityCode,
                ASalesDTO.CreatedBy, ASalesDTO.LastUpdatedBy, ASalesDTO.ApprovalStatus);

            try
            {

                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                        new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objSalesIdRepository.Add(newSalesID);
                        objSalesIdRepository.UnitOfWork.Commit();

                        if (ASalesDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_SalesId", " SalesID = '" + ASalesDTO.SalesId + "' and RUT = '" + ASalesDTO.RUT.Replace("-", "") + "'  and OrganizationCode = '" + Utils.OrganizationCode + "'",
                                "SALEID", ASalesDTO.LastUpdatedBy, "/Master/SalesIDAuthorize?ASalesId=" + ASalesDTO.SalesId + "&ARUT=" + ASalesDTO.RUT.Replace("-", "") + "&AOrganizationCode=" + Utils.OrganizationCode + "&", "MT");

                            objSalesIdRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                scope.Dispose();

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion
        #region UpdateSalesIdDetails
        public void UpdateSalesIdDetails(string ASalesID, string ARUT, SalesIdDTO ASalesIdDTO)
        {
            if (ASalesIdDTO == null)
                throw new ArgumentException("Messages.warning_CannotUpdateSalesIdWithNullInformation");
            SaleId currentSalesId = objSalesIdRepository.Get(ASalesID, ARUT, Utils.OrganizationCode);
            SaleId updateSalesId = new SaleId()
            {
                OrganizationCode = Utils.OrganizationCode,
                SalesId = ASalesID,
                RUT = ARUT.Replace("-", ""),
                BranchCode = ASalesIdDTO.BranchCode,
                CommunaCode = ASalesIdDTO.CommunaCode,
                CityCode = ASalesIdDTO.CityCode,
                Address = ASalesIdDTO.Address,
                LastUpdatedOn = Utils.BusinessDate.Date,
                LastUpdatedBy = ASalesIdDTO.LastUpdatedBy,
                CreatedBy = currentSalesId.CreatedBy,
                CreatedOn = currentSalesId.CreatedOn,

                ApprovalStatus = ASalesIdDTO.ApprovalStatus
            };


            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                  new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objSalesIdRepository.Merge(currentSalesId, updateSalesId);
                    objSalesIdRepository.UnitOfWork.Commit();

                    if (ASalesIdDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };

                        paramCollection = GetApprovalQueueProcedureParameters("MT_SalesId", " SalesID = '" + ASalesIdDTO.SalesId + "' and RUT = '" + ASalesIdDTO.RUT.Replace("-", "") + "'  and OrganizationCode = '" + Utils.OrganizationCode + "'",
                            "SALEID", ASalesIdDTO.LastUpdatedBy, "/Master/SalesIDAuthorize?ASalesId=" + ASalesIdDTO.SalesId + "&ARUT=" + ASalesIdDTO.RUT.Replace("-", "") + "&AOrganizationCode=" + Utils.OrganizationCode + "&", "MT");

                        objSalesIdRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType,@P_OrganizationCode", paramCollection);
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
        #endregion
        #endregion

        #region FinancialAccount


        public List<AdjustmentFinancialAccountDTO> FindAdjustmentFinancialAccount(int APageIndex, int APageCount)
        {
            if (APageIndex < 0 || APageCount <= 0)
                throw new ArgumentException("Messages.warning_InvalidArgumentForFindFinancialAccount");

            int iRecordCount = objAdjustmentFinancialAccountRepository.GetRecordCount();
            var financialAccounts = objAdjustmentFinancialAccountRepository.GetAll(); // <string>(APageIndex, iRecordCount, x => x.CurrencyCode, true);
            if (financialAccounts != null && financialAccounts.Any())
            {
                List<AdjustmentFinancialAccountDTO> lstAdjustmentFinancialAccountDTO = new List<AdjustmentFinancialAccountDTO>();
                foreach (var financialAccount in financialAccounts)
                {
                    lstAdjustmentFinancialAccountDTO.Add(Conversion.Mapping.AdjustmentFinancialAccountToAdjustmentFinancialAccountDTO(financialAccount));
                }
                return lstAdjustmentFinancialAccountDTO;
            }
            else
                return new List<AdjustmentFinancialAccountDTO>();
        }

        public void DeleteAdjustmentFinancialAccount(int AAdjustmentFinancialAccountId)
        {
            var financialAccount = objAdjustmentFinancialAccountRepository.Get(AAdjustmentFinancialAccountId);

            if (financialAccount != null)
            {
                objAdjustmentFinancialAccountRepository.Remove(financialAccount);
                objAdjustmentFinancialAccountRepository.UnitOfWork.Commit();
            }
        }

        public AdjustmentFinancialAccountDTO FindByAdjustmentFinancialAccountId(int AId)
        {
            var financialAccount = objAdjustmentFinancialAccountRepository.Get(AId);
            if (financialAccount != null)
            {
                if (financialAccount.Branch == null)
                {
                    financialAccount.Branch = objBranchRepository.Get(financialAccount.BranchCode, financialAccount.OrganizationCode);
                }
                if (financialAccount.Product == null)
                {
                    financialAccount.Product = objProductRepository.Get(financialAccount.OrganizationCode, financialAccount.ProductCode);
                }
                if (financialAccount.TransactionSubCode == null)
                {
                    financialAccount.TransactionSubCode = objTransactionSubCodeRepository.Get(financialAccount.TransactionSubCodeId);
                }

                objAdjustmentFinancialAccountRepository.UnitOfWork.RefreshDataInModel(ref financialAccount);
                return Conversion.Mapping.AdjustmentFinancialAccountToAdjustmentFinancialAccountDTO(financialAccount);
            }
            else
            {
                return new AdjustmentFinancialAccountDTO();
            }
        }

        public void SaveAdjustmentFinancialAccountDetails(AdjustmentFinancialAccountDTO AAdjustmentFinancialAccountDTO)
        {
            if (AAdjustmentFinancialAccountDTO == null)
                throw new ArgumentException("Messages.warning_CannotAddCurrencyWithNullInformation");

            var newAdjustmentFinancialAccount = AdjustmentFinancialAccountFactory.CreateFinancialAccount(AAdjustmentFinancialAccountDTO.AdjustmentFinancialAccountId, AAdjustmentFinancialAccountDTO.BranchCode, AAdjustmentFinancialAccountDTO.ModuleName,
                AAdjustmentFinancialAccountDTO.ProductCode, AAdjustmentFinancialAccountDTO.TransactionSubCodeId, AAdjustmentFinancialAccountDTO.DebitAccountNr, AAdjustmentFinancialAccountDTO.DebitNarration, AAdjustmentFinancialAccountDTO.AdjustmentCode, AAdjustmentFinancialAccountDTO.AdjustmentFinancialActivityCode,
                AAdjustmentFinancialAccountDTO.CreditAccountNr, AAdjustmentFinancialAccountDTO.CreditNarration, AAdjustmentFinancialAccountDTO.EffectiveDate, 
                AAdjustmentFinancialAccountDTO.CreatedBy, AAdjustmentFinancialAccountDTO.LastUpdatedBy, AAdjustmentFinancialAccountDTO.ApprovalStatus, AAdjustmentFinancialAccountDTO.OrganizationCode);
            
            try
            {
                var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                    new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
                using (scope)
                {
                    try
                    {
                        objAdjustmentFinancialAccountRepository.Add(newAdjustmentFinancialAccount);
                        objAdjustmentFinancialAccountRepository.UnitOfWork.Commit();
                        if (AAdjustmentFinancialAccountDTO.ApprovalStatus == "P")
                        {
                            SqlParameter[] paramCollection = new SqlParameter[] { };

                            paramCollection = GetApprovalQueueProcedureParameters("MT_AdjustmentFinancialAccount", " AdjustmentFinancialAccountId = '" + newAdjustmentFinancialAccount.AdjustmentFinancialAccountId + "'",
                                "ADJFNACNT", AAdjustmentFinancialAccountDTO.CreatedBy, "/Master/AdjustmentFinancialAccountAuthorize/" + newAdjustmentFinancialAccount.AdjustmentFinancialAccountId + "/A?", "MT");

                            objAdjustmentFinancialAccountRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateAdjustmentFinancialAccountDetails(int AAdjustmentFinancialAccountId, AdjustmentFinancialAccountDTO AAdjustmentFinancialAccountDTO)
        {
            if (AAdjustmentFinancialAccountDTO == null)
            {
                throw new ArgumentException("Messages.warning_CannotUpdateCurrencyWithNullInformation");

            }
            AdjustmentFinancialAccount currentAdjustmentFinancialAccount = objAdjustmentFinancialAccountRepository.Get(AAdjustmentFinancialAccountId);
            AdjustmentFinancialAccount updatedAdjustmentFinancialAccount = new AdjustmentFinancialAccount()
            {
                AdjustmentFinancialAccountId = AAdjustmentFinancialAccountId,
                BranchCode = AAdjustmentFinancialAccountDTO.BranchCode,
                ModuleName = AAdjustmentFinancialAccountDTO.ModuleName,
                ProductCode = AAdjustmentFinancialAccountDTO.ProductCode,
                TransactionSubCodeId = AAdjustmentFinancialAccountDTO.TransactionSubCodeId,
                AdjustmentCode = AAdjustmentFinancialAccountDTO.AdjustmentCode,
                AdjustmentFinancialActivityCode = AAdjustmentFinancialAccountDTO.AdjustmentFinancialActivityCode,
                DebitAccountNr = AAdjustmentFinancialAccountDTO.DebitAccountNr,
                DebitNarration = AAdjustmentFinancialAccountDTO.DebitNarration,
                CreditAccountNr = AAdjustmentFinancialAccountDTO.CreditAccountNr,
                CreditNarration = AAdjustmentFinancialAccountDTO.CreditNarration,
                OrganizationCode = currentAdjustmentFinancialAccount.OrganizationCode,
                EffectiveDate = AAdjustmentFinancialAccountDTO.EffectiveDate,

                LastUpdatedOn = Utils.BusinessDate,
                LastUpdatedBy = AAdjustmentFinancialAccountDTO.LastUpdatedBy,
                CreatedOn = currentAdjustmentFinancialAccount.CreatedOn,
                CreatedBy = currentAdjustmentFinancialAccount.CreatedBy,
                ApprovalStatus = AAdjustmentFinancialAccountDTO.ApprovalStatus
            };

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted });
            using (scope)
            {
                try
                {
                    objAdjustmentFinancialAccountRepository.Merge(currentAdjustmentFinancialAccount, updatedAdjustmentFinancialAccount);
                    objAdjustmentFinancialAccountRepository.UnitOfWork.Commit();
                    if (AAdjustmentFinancialAccountDTO.ApprovalStatus == "P")
                    {
                        SqlParameter[] paramCollection = new SqlParameter[] { };
                        paramCollection = GetApprovalQueueProcedureParameters("MT_AdjustmentFinancialAccount", " AdjustmentFinancialAccountId = '" + AAdjustmentFinancialAccountId + "'",
                                "ADJFNACNT", AAdjustmentFinancialAccountDTO.LastUpdatedBy, "/Master/AdjustmentFinancialAccountAuthorize/" + AAdjustmentFinancialAccountId + "/A?", "MT");
                        objFinancialAccountRepository.UnitOfWork.ExecuteCommand("PR_Ct_Approval_Status_Update @P_TableName, @P_ColumnName, @P_Value, @P_WhereClause, @P_ContextCode, @P_ReturnUrl, @P_LoginName, @P_TransactionType, @P_OrganizationCode", paramCollection);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<AdjustmentFinancialAccountDTO> FindAll(int APageNo, AdjustmentFinancialAccountDTO AAdjustmentFinancialAccountDTO)
        {
            List<Utils.ExpressionList> lstExp = new List<Utils.ExpressionList>(){
                new Utils.ExpressionList(){FilterColumn = "BranchCode", FilterValue = AAdjustmentFinancialAccountDTO.BranchCode, Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "ModuleName", FilterValue = AAdjustmentFinancialAccountDTO.ModuleName, Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "ProductCode", FilterValue = AAdjustmentFinancialAccountDTO.ProductCode, Operator = "Contains"},
                new Utils.ExpressionList(){FilterColumn = "TransactionSubCodeId", FilterValue = AAdjustmentFinancialAccountDTO.TransactionSubCodeId, Operator =  AAdjustmentFinancialAccountDTO.TransactionSubCodeId != 0 ? "Equals" : "NotEquals"},
                new Utils.ExpressionList() { FilterColumn = "OrganizationCode",FilterValue = Utils.OrganizationCode,Operator = "Equals"},
                new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DEL", Operator = "NotEquals"},
                new Utils.ExpressionList() { FilterColumn = "ApprovalStatus",FilterValue = "DIS", Operator = "NotEquals"}
            };

            Expression<Func<AdjustmentFinancialAccount, bool>> expression = Utils.GetExpression<AdjustmentFinancialAccount>(lstExp);
            int iRecordCount = objAdjustmentFinancialAccountRepository.GetRecordCount(expression);
            var vFinancialAccountDTO = objAdjustmentFinancialAccountRepository.GetFiltered(0, iRecordCount, expression);

            if (vFinancialAccountDTO != null && vFinancialAccountDTO.Any())
            {
                List<AdjustmentFinancialAccountDTO> lstAdjustmentFinancialAccountDTO = new List<AdjustmentFinancialAccountDTO>();
                foreach (var financialAccount in vFinancialAccountDTO)
                {
                    lstAdjustmentFinancialAccountDTO.Add(Conversion.Mapping.AdjustmentFinancialAccountToAdjustmentFinancialAccountDTO(financialAccount));
                }
                return lstAdjustmentFinancialAccountDTO;
            }
            else
                return new List<AdjustmentFinancialAccountDTO>();
        }


        #endregion

        #endregion

        #region Private Methods
        private void UpdateDetailsInChildCollection(dynamic AUpdatedItem, dynamic ACurrentItem, string ARepositoryName)
        {
            dynamic objRepo = null;
            if (ARepositoryName.Equals("objCustomerOccupationRepository"))
                objRepo = objCustomerOccupationRepository;
            if (ACurrentItem == null && AUpdatedItem != null)
            {
                objRepo.Add(AUpdatedItem);
            }
            else if (ACurrentItem != null && AUpdatedItem == null)
            {
                objRepo.Remove(ACurrentItem);
            }
            else
            {
                objRepo.Merge(ACurrentItem, AUpdatedItem);
            }
        }
        #endregion

        #region ProductDependencyCheck
        public bool ProductDependencyCheck(string Id)
        {

            string[] Context = { "Contract", "Customer", "PlanMaster" };

            List<Utils.ExpressionList> lstExpression = new List<Utils.ExpressionList>() 
                    { new Utils.ExpressionList() { FilterColumn = "ProductCode",FilterValue = Id, Operator = "Equals" }
                    };

            foreach (string context in Context)
            {
                switch (context)
                {
                    case "Contract":
                        {
                            Expression<Func<Contract, bool>> expression = Utils.GetExpression<Contract>(lstExpression);
                            int iRecordCount = objContractRepository.GetRecordCount(expression);
                            if (iRecordCount > 0)
                                return true;
                            break; 
                        }
                    case "Customer":
                        {
                            Expression<Func<Customer, bool>> expression = Utils.GetExpression<Customer>(lstExpression);
                            int iRecordCount = objCustomerRepository.GetRecordCount(expression);
                            if (iRecordCount > 0)
                                return true;
                            break; 
                        }
                    case "PlanMaster":
                        {
                            Expression<Func<PlanMaster, bool>> expression = Utils.GetExpression<PlanMaster>(lstExpression);
                            int iRecordCount = objPlanMasterRepository.GetRecordCount(expression);
                            if (iRecordCount > 0)
                                return true;
                            break; 
                        }
                }
            }

            return false;
        }
        #endregion

        #region GetApprovalQueueProcedureParameters
        private SqlParameter[] GetApprovalQueueProcedureParameters(string ATableName, string AWhereClause, string AContextCode
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
                    new SqlParameter(parameterName: "@P_OrganizationCode",dbType:System.Data.SqlDbType.VarChar ),
                };

            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TableName")).First()).Value = ATableName;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ColumnName")).First()).Value = "ApprovalStatus";
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_Value")).First()).Value = "P";
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_WhereClause")).First()).Value =  AWhereClause;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ContextCode")).First()).Value = AContextCode;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_LoginName")).First()).Value = AUpdatedBy;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_ReturnUrl")).First()).Value = AReturnUrl;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_TransactionType")).First()).Value = ATransactionType;
            ((SqlParameter)paramCollection.Where(x => x.ParameterName.Equals("@P_OrganizationCode")).First()).Value = Utils .OrganizationCode ;

            return paramCollection;
        }
        #endregion
        
      
        
    }
}
