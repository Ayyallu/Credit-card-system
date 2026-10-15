using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.DataTranfer;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.DataTranfer.LookUpInfo;


namespace CC_Ingenio.Manager.Contracts
{
    public interface IMasterManager
    {
        #region Currency

        List<CurrencyDTO> FindCurrency(int APageIndex, int APageCount);
        CurrencyDTO FindByCurrencyCode(string ACurrencyCode,string AOrganizationCode, bool AIsFromApprovalQueue = false);
        void DeleteCurrency(string ACurrencyCode);
        void UpdateCurrencyDetails(string ACurrencyCode,string AOrganizationCode, CurrencyDTO ACurrencyDTO);
        void SaveCurrencyDetails(CurrencyDTO ACurrencyDTO);
        List<CurrencyDTO> FindAll(int APageNo, CurrencyDTO AFilter);
        
        #endregion

        #region Organization

        List<OrganizationDTO> FindOrganization(int APageIndex, int APageCount);
        OrganizationDTO FindByOrganizationCode(string AOrganizationCode);
        void DeleteOrganization(string AOrganizationCode);
        void UpdateOrganizationDetails(string AOrganizationCode, OrganizationDTO AOrganizationDTO);
        void SaveOrganizationDetails(OrganizationDTO AOrganizationDTO);
        List<OrganizationDTO> FindAll(int APageNo, OrganizationDTO AFilter);

        #endregion

        #region Franchise

        List<FranchiseDTO> FindFranchise(int APageIndex, int APageCount);
        FranchiseDTO FindByFranchiseCode(string AFranchiseCode);
        void DeleteFranchise(string AFranchiseCode);
        void UpdateFranchiseDetails(string AFranchiseCode, FranchiseDTO AFranchiseDTO);
        void SaveFranchiseDetails(FranchiseDTO AFranchiseDTO);
        List<FranchiseDTO> FindAll(int APageNo, FranchiseDTO AFilter);

        #endregion

        #region Sourcing

        void SaveSourcingDetails(SourcingDTO ASourcingDTO);
        void UpdateSourcingDetails(string ASourcingChannelCode,string AOrganizationCode, SourcingDTO ASourcingDTO);
        void DeleteSourcing(string ASourcingChannelCode);

        List<SourcingDTO> FindAll(int APageNo, SourcingDTO AFilter);
        SourcingDTO FindBySourcingChannelCode(string ASourcingChannelCode,  string AOrganizationCode);
        List<SourcingDTO> FindSourcing(int APageIndex, int APageCount);
        #endregion

        #region Company

        void SaveCompanyDetails(CompanyDTO ACompanyDTO);
        void UpdateCompanyDetails(string ACompany,string AOrganizationCode, CompanyDTO ACompanyDTO);
        void DeleteCompany(string ACompanyCode);

        List<CompanyDTO> FindAll(int APageNo, CompanyDTO AFilter);
        CompanyDTO FindByCompanyCode(string ACompanyCode,string AOrganizationCode);
        List<CompanyDTO> FindCompany(int APageIndex, int APageCount);
        #endregion

        #region BillingParameter

        void SaveBillingParameterDetails(BillingParameterDTO ABillingParameterDTO);
        void UpdateBillingParameterDetails(string ABillingParameter, BillingParameterDTO ABillingParameterDTO);
        void DeleteBillingParameter(string ACardTypeCode);

        List<BillingParameterDTO> FindAll(int APageNo, BillingParameterDTO AFilter);
        BillingParameterDTO FindByCardTypeCode(string ACardTypeCode);
        List<BillingParameterDTO> FindBillingParameter(int APageIndex, int APageCount);
        #endregion

        #region City

        List<CityDTO> FindCity(int APageIndex, int APageCount);
        CityDTO FindByCityCode(string ACityCode,string AOrganizationCode);
        void DeleteCity(string ACityCode);
        void UpdateCityDetails(string ACityCode,string AOrganizationCode, CityDTO ACityDTO);
        void SaveCityDetails(CityDTO ACityDTO);
        List<CityDTO> FindAll(int APageNo, CityDTO AFilter);
        CityNewDTO GetCityRegionByCommuna(string ACommunaCode, string AType);

        #endregion

        #region Credit Segment
        void SaveCreditSegmentDetails(CreditSegmentDTO ACreditSegmentDTO);
        void UpdateCreditSegmentDetails(string ACardSegmentCode,string AOrganizationCode, CreditSegmentDTO ACreditSegmentDTO);
        void DeleteCreditSegment(string ACardSegmentCode);

        List<CreditSegmentDTO> FindAll(int APageNo, CreditSegmentDTO AFilter);
        CreditSegmentDTO FindByCardSegmentCode(string ACardSegmentCode,string AOrganizationCode);
        List<CreditSegmentDTO> FindCreditSegment(int APageIndex, int APageCount);
        #endregion

        #region Credit SubSegment
        void SaveCreditSubSegmentDetails(CreditSubSegmentDTO ACreditSubSegmentDTO);
        void UpdateCreditSubSegmentDetails(CreditSubSegmentDTO ACreditSubSegmentDTO);
        void DeleteCreditSubSegment(string ACreditSegmentCode, int ACreditSubSegmentCode);
        List<CreditSubSegmentDTO> FindCreditSubSegment(int APageIndex, int APageCount);
        CreditSubSegmentDTO FindByCreditSubSegmentCode(string ACreditSegmentCode, int ACreditSubSegmentCode);
        List<CreditSubSegmentDTO> FindAll(int APageNo, CreditSubSegmentDTO ACreditSubSegmentDTO);
        #endregion

        #region Designation
        void SaveDesignationDetails(DesignationDTO ADesignationDTO);
        void UpdateDesignationDetails(string ADesignationCode, string AOrganizationCode, DesignationDTO ADesignationDTO);
        void DeleteDesignation(string ADesignationCode);

        List<DesignationDTO> FindAll(int APageNo, DesignationDTO AFilter);
        DesignationDTO FindByDesignationCode(string ADesignationCode, string AOrganizationCode);
        List<DesignationDTO> FindDesignation(int APageIndex, int APageCount);
        #endregion

        #region SystemParameter

        void SaveSystemParameterDetails(SystemParameterDTO ASystemParameterDTO);
        void UpdateSystemParameterDetails(int AParameterId, SystemParameterDTO ASystemParameterDTO);
        void DeleteSystemParameter(int AParameterId);

        List<SystemParameterDTO> FindAll(int APageNo, SystemParameterDTO AFilter);
        SystemParameterDTO FindByParameterId(int AParameterId);
        List<SystemParameterDTO> FindSystemParameter(int APageIndex, int APageCount);
        SystemParameterDTO FindBySystemParameterName(string AParamName);

        #endregion

        #region Customer Segment
        void SaveCustomerSegmentDetails(CustomerSegmentDTO ACustomerSegmentDTO);
        void UpdateCustomerSegmentDetails(string ASegmentCode, string AOrganizationCode,CustomerSegmentDTO ACustomerSegmentDTO);
        void DeleteCustomerSegment(string ASegmentCode);

        List<CustomerSegmentDTO> FindAll(int APageNo, CustomerSegmentDTO AFilter);
        CustomerSegmentDTO FindBySegmentCode(string ASegmentCode ,string AOrganizationCode);
        List<CustomerSegmentDTO> FindCustomerSegment(int APageIndex, int APageCount);
        bool CheckIfDefaultCustomerSegmentExists(string ACustomerSegmentCode);
        #endregion

        #region Customer SubSegment
        void SaveCustomerSubSegmentDetails(CustomerSubSegmentDTO ACustomerSubSegmentDTO);
        void UpdateCustomerSubSegmentDetails(string ASubSegmentCode,string AOrganizationCode, CustomerSubSegmentDTO ACustomerSubSegmentDTO);
        void DeleteCustomerSubSegment(string ASubSegmentCode);

        List<CustomerSubSegmentDTO> FindAll(int APageNo, CustomerSubSegmentDTO AFilter);
        CustomerSubSegmentDTO FindBySubSegmentCode(string ASubSegmentCode,string AOrganizationCode);
        List<CustomerSubSegmentDTO> FindCustomerSubSegment(int APageIndex, int APageCount);
        bool CheckIfDefaultCustomerSubSegmentExists(string ACustomerSubSegmentCode);
        #endregion

        #region Card Status
        void SaveCardStatusDetails(CardStatusDTO ACardStatusDTO);
        void UpdateCardStatusDetails(string ACardStatusCode, CardStatusDTO ACardStatusDTO);
        void DeleteCardStatus(string ACardStatusCode);

        List<CardStatusDTO> FindAll(int APageNo, CardStatusDTO AFilter);
        CardStatusDTO FindByCardStatusCode(string ACardStatusCode);
        List<CardStatusDTO> FindCardStatus(int APageIndex, int APageCount);
        #endregion

        #region General
        void SaveGeneralDetails(GeneralDTO AGeneralDTO);
        void UpdateGeneralDetails(string AGeneralKeyName,string AOrganizationCode, GeneralDTO AGeneralDTO);
        void DeleteGeneral(string AGeneralKeyName);

        List<GeneralDTO> FindAll(int APageNo, GeneralDTO AFilter);
        GeneralDTO FindByGeneralKeyName(string AGeneralKeyName,string AOrganizationCode);
        List<GeneralDTO> FindGeneral(int APageIndex, int APageCount);
        #endregion

        #region Contract
        void SaveContractDetails(ContractDTO AContractDTO);
        void UpdateContractDetails(string AContractCode,string AOrganizationCode, ContractDTO AContractDTO);
        void DeleteContract(string AContractCode);

        List<ContractDTO> FindAll(int APageNo, ContractDTO AFilter);
        ContractDTO FindByContractCode(string AContractCode,string AOrganizationCode);
        List<ContractDTO> FindContract(int APageIndex, int APageCount);
        #endregion

        #region Product Type
        void SaveProductTypeDetails(ProductTypeDTO AProductTypeDTO);
        void UpdateProductTypeDetails(string AProductTypeCode, string  AOrganizationCode,ProductTypeDTO AProductTypeDTO);
        void DeleteProductType(string AProductTypeCode);

        List<ProductTypeDTO> FindAll(int APageNo, ProductTypeDTO AFilter);
        ProductTypeDTO FindByProductTypeCode(string AProductTypeCode,string AOrganizationCode);
        List<ProductTypeDTO> FindProductType(int APageIndex, int APageCount);
        #endregion
        
        #region Transaction
        void SaveTransactionDetails(TransactionDTO ATransactionDTO);
        void UpdateTransactionDetails(string ATransactionCode, TransactionDTO ATransactionDTO);
        void DeleteTransaction(string ATransactionCode);

        List<TransactionDTO> FindAll(int APageNo, TransactionDTO AFilter);
        TransactionDTO FindByTransactionCode(string ATransactionCode);
        List<TransactionDTO> FindTransaction(int APageIndex, int APageCount);
        #endregion

        #region  SystemMaintenanceConfiguration
        void SaveSystemMaintenanceConfigurationDetails( SystemMaintenanceConfigurationDTO ASystemMaintenanceConfigurationDTO);
        void UpdateSystemMaintenanceConfigurationDetails(string ABaseCurrency, SystemMaintenanceConfigurationDTO ASystemMaintenanceConfigurationDTO);
        void DeleteSystemMaintenanceConfiguration(string ABaseCurrency);

        List<SystemMaintenanceConfigurationDTO> FindAll(int APageNo, SystemMaintenanceConfigurationDTO AFilter);
        SystemMaintenanceConfigurationDTO FindByBaseCurrency(string ABaseCurrency);
        List<SystemMaintenanceConfigurationDTO> FindSystemMaintenanceConfiguration(int APageIndex, int APageCount);
        #endregion
        
        #region BoostMaintenance
        void SaveBoostMaintenanceDetails(BoostMaintenanceDTO ABoostMaintenanceDTO);
        void UpdateBoostMaintenanceDetails(int ABoostId, BoostMaintenanceDTO ABoostMaintenanceDTO);
        void DeleteBoostMaintenance(int ABoostId);

        List<BoostMaintenanceDTO> FindAll(int APageNo, BoostMaintenanceDTO AFilter);
        BoostMaintenanceDTO FindByBoostId(int ABoostId);
        List<BoostMaintenanceDTO> FindBoostMaintenance(int APageIndex, int APageCount);
        #endregion

        #region Fee
        void SaveFeeDetails(FeeDTO AFeeDTO);
        void UpdateFeeDetails(string AFeeCode,string AOrganizationCode, FeeDTO AFeeDTO);
        void DeleteFee(string AFeeCode);

        List<FeeDTO> FindAll(int APageNo, FeeDTO AFilter);
        FeeDTO FindByFeeCode(string AFeeCode,string AOrganizationCode);
        List<FeeDTO> FindFee(int APageIndex, int APageCount);
        #endregion

        #region Product
        void SaveProductDetails(ProductDTO AProductDTO);
        void UpdateProductDetails(string AProductCode,string AOrganizationCode, ProductDTO AProductDTO);
        ///Sheeba:11-9-2015
        void UpdateProductMaximumCreditLine(string AProductCode, string AOrganizationCode, decimal MaximumCreditLine);

        void DeleteProduct(string AProductCode);

        List<ProductDTO> FindAll(int APageNo, ProductDTO AFilter);
        ProductDTO FindByProductCode(string AProductCode,string AOrganizationCode);
        List<ProductDTO> FindProduct(int APageIndex, int APageCount);
        string GetProductFirstChar();
        string GetProductCurrency(string AProductCode);
        string GetCurrencyPrecision(string ACurrencyCode);
        #endregion

        #region Block Code
        void SaveBlockCodeDetails(BlockCodeDTO ABlockCodeDTO);
        void UpdateBlockCodeDetails(string ABlockCode, BlockCodeDTO ABlockCodeDTO);
        void DeleteBlockCode(string ABlockCode);

        List<BlockCodeDTO> FindAll(int APageNo, BlockCodeDTO AFilter);
        BlockCodeDTO FindByBlockCode(string ABlockCode);
        List<BlockCodeDTO> FindBlockCode(int APageIndex, int APageCount);
        #endregion

        #region Customer
        void SaveCustomerDetails(CustomerDTO ACustomerDTO);
        void UpdateCustomerDetails(int ACustomerId, CustomerDTO ACustomerDTO);
        void DeleteCustomer(int ACustomerId);

        List<CustomerDTO> FindAll(int APageNo, CustomerDTO AFilter);
        CustomerDTO FindByCustomerId(int ACustomerId);
        List<CustomerDTO> FindCustomer(int APageIndex, int APageCount);
        bool ValidateCustomerDesignationForHRP(string ADesignationCode);
        bool ValidateCreditScore(string AProductCode, decimal ACreditScore);
        List<RUTCardNumberSearchDTO> GetParentCustomers(string ARUT, string ACardNumber, string ACustomerName);       
        bool GetAutoApprovalPropertyOfProduct(string AProductCode);
        #endregion

        #region CustomerUnderwriting
        List<CustomerDTO> ListCustomersToUnderwrite(CustomerDTO ACustomerDTO);
        int AuthorizationByUnderwriter(int ACustomerId, int ACreditScore, decimal ACreditLimit, string ARemarks, string AApprovalStatus, string AApprovedBy, string AReasonCode, string AReasonDescription);
        #endregion

        #region Industrial Customer
        void SaveIndustrialCustomerDetails(int ACustomerId, IndustrialCustomerDTO AIndustrialCustomerDTO);
        int DeleteIndustrialCustomer(int ACustomerId, IndustrialCustomerDTO AIndustrialCustomerDTO);
        IndustrialCustomerDTO GetIndustrialCustomerByCustomerId(int ACustomerId);
        int PopulateMtCustomerOnIndustrialCustomerApproval(string AQueueRefNo);
        //List<CustomerDTO> GetCustomersForListing(string ARUT, string ACustomerType, string AName, string AOrganizationCode, bool AIsRequestFromUnderWriter);
        List<CustomerDTO> GetCustomersForListing(string ARUT, string ACustomerType, string AName, string ACustomerStatus, string AOrganizationCode, bool AIsRequestFromUnderWriter);
        object GetCustomerDetailsIfExists(string ARUT, string ASalesId);
        CustomerInfo ValidateCustomer(string ARUT, string ASalesId, string AOrganizationCode, string AUserName);
        CustomerInfo ValidateCustomerWithoutSalesId(string ARUT, string AProductMemberType, string AParentCustId, string AOrganizationCode);
        #endregion

        #region CalendarDateMaintenance
        void SaveCalendarDateMaintenanceDetails(CalendarDateMaintenanceDTO ACalendarDateMaintenanceDTO);
        void UpdateCalendarDateMaintenanceDetails(int AId, CalendarDateMaintenanceDTO ACalendarDateMaintenanceDTO);
        void DeleteCalendarDateMaintenance(int AId);

        List<CalendarDateMaintenanceDTO> FindAll(int APageNo, CalendarDateMaintenanceDTO AFilter);
        CalendarDateMaintenanceDTO FindByCalendarDateMaintenance(int AId);
        List<CalendarDateMaintenanceDTO> FindCalendarDateMaintenance(int APageIndex, int APageCount);
        #endregion

        #region TransactionSubCode
        void SaveTransactionSubCodeDetails(TransactionSubCodeDTO ATransactionSubCodeDTO);
        void UpdateTransactionSubCodeDetails(int ATransactionSubCodeId, TransactionSubCodeDTO ATransactionSubCodeDTO);
        void DeleteTransactionSubCode(int ATransactionSubCodeId);

        List<TransactionSubCodeDTO> FindAll(int APageNo, TransactionSubCodeDTO AFilter);
        TransactionSubCodeDTO FindByTransactionSubCode(int ATransactionSubCodeId);
        List<TransactionSubCodeDTO> FindTransactionSubCode(int APageIndex, int APageCount);
        #endregion

        #region FinancialAccount
        void SaveFinancialAccountDetails(FinancialAccountDTO AFinancialAccountDTO);
        void UpdateFinancialAccountDetails(int AFinancialAccountId, FinancialAccountDTO AFinancialAccountDTO);
        void DeleteFinancialAccount(int AFinancialAccountId);

        List<FinancialAccountDTO> FindAll(int APageNo, FinancialAccountDTO AFilter);
        FinancialAccountDTO FindByFinancialAccountId(int AFinancialAccountId);
        List<FinancialAccountDTO> FindFinancialAccount(int APageIndex, int APageCount);
        #endregion

        #region BillingCycleParameter

        List<BillingCycleParameterDTO> FindAll(int APageNo, BillingCycleParameterDTO AFilter);
        BillingCycleParameterDTO FindByBillingCycleParameterId(int ABillingCycleParameterId);
        List<BillingCycleParameterDTO> FindBillingCycleParameter(int APageIndex, int APageCount);
        void SaveBillingCycleParameterDetails(BillingCycleParameterDTO ABillingCycleParameterDTO);
        void UpdateBillingCycleParameterDetails(int ABillingCycleParameterId, BillingCycleParameterDTO ABillingCycleParameterDTO);
        void DeleteBillingCycleParameter(int ABillingCycleParameterId);
        DateTime GetWorkingDate(int Month, int Year, string Type);
        DateTime GetNextWorkingDate(DateTime ADate);

        #endregion

        #region SBIFInterest

        List<SBIFInterestDTO> FindAll(int APageNo, SBIFInterestDTO AFilter);
        SBIFInterestDTO FindBySBIFInterestId(int ASBIFInterestId);
        List<SBIFInterestDTO> FindSBIFInterest(int APageIndex, int APageCount);
        void SaveSBIFInterestDetails(SBIFInterestDTO ASBIFInterestDTO);
        void UpdateSBIFInterestDetails(int ASBIFInterestId, SBIFInterestDTO ASBIFInterestDTO);
        void DeleteSBIFInterest(int ASBIFInterestId);

        #endregion

        #region InterestMaster

        List<InterestMasterDTO> FindAll(int APageNo, InterestMasterDTO AFilter);
        InterestMasterDTO FindByInterestMasterId(int AInterestMasterId);
        List<InterestMasterDTO> FindInterestMaster(int APageIndex, int APageCount);
        void SaveInterestMasterDetails(InterestMasterDTO AInterestMasterDTO);
        void UpdateInterestMasterDetails(int AInterestMasterId, InterestMasterDTO AInterestMasterDTO);
        void DeleteInterestMaster(int AInterestMasterId);

        #endregion

        #region Commission
        void SaveCommissionDetails(CommissionDTO ACommissionDTO);
        void UpdateCommissionDetails(string ACommissionCode, CommissionDTO ACommissionDTO);
        void DeleteCommission(string ACommissionCode);

        List<CommissionDTO> FindAll(int APageNo, CommissionDTO AFilter);
        CommissionDTO FindByCommission(string ACommissionCode);
        List<CommissionDTO> FindCommission(int APageIndex, int APageCount);
        decimal GetCurrentUFRate(DateTime AEffectiveDate);
        #endregion

        #region New Card Application
        string[] CheckIfAlreadyExistsCard(string ARUTNumber);
        bool CheckIfValidCard(string ARUTNumber);
        CustomerDTO FindCustomerByRut(string ARutNo);
        #endregion

        #region CommissionCharge
        void SaveCommissionChargeDetails(CommissionChargeDTO ACommissionChargeDTO);
        void UpdateCommissionChargeDetails(int ACommissionChargeId, CommissionChargeDTO ACommissionChargeDTO);
        void DeleteCommissionCharge(int ACommissionChargeId);

        List<CommissionChargeDTO> FindAll(int APageNo, CommissionChargeDTO AFilter);
        CommissionChargeDTO FindByCommissionCharge(int ACommissionChargeId);
        List<CommissionChargeDTO> FindCommissionCharge(int APageIndex, int APageCount);
        #endregion

        #region BlockCodeForCommissionExclusion
        void SaveBlockCodeForCommissionExclusionDetails(BlockCodeForCommissionExclusionDTO ABlockCodeForCommissionExclusionDTO);
        void UpdateBlockCodeForCommissionExclusionDetails(int ABlockCodeForCommissionExclusionId, BlockCodeForCommissionExclusionDTO ABlockCodeForCommissionExclusionDTO);
        void DeleteBlockCodeForCommissionExclusion(int ABlockCodeForCommissionExclusionId);

        List<BlockCodeForCommissionExclusionDTO> FindAll(int APageNo, BlockCodeForCommissionExclusionDTO AFilter);
        BlockCodeForCommissionExclusionDTO FindByBlockCodeForCommissionExclusionId(int ABlockCodeForCommissionExclusionId);
        List<BlockCodeForCommissionExclusionDTO> FindBlockCodeForCommissionExclusion(int APageIndex, int APageCount);
        #endregion

        #region DiscountOnMgmtCollection
        void SaveDiscountOnMgmtCollectionDetails(DiscountOnMgmtCollectionDTO ADiscountOnMgmtCollectionDTO);
        void UpdateDiscountOnMgmtCollectionDetails(int ADiscountOnMgmtCollectionId, DiscountOnMgmtCollectionDTO ADiscountOnMgmtCollectionDTO);
        void DeleteDiscountOnMgmtCollection(int ADiscountOnMgmtCollectionId);

        List<DiscountOnMgmtCollectionDTO> FindAll(int APageNo, DiscountOnMgmtCollectionDTO AFilter);
        DiscountOnMgmtCollectionDTO FindByDiscountOnMgmtCollectionId(int ADiscountOnMgmtCollectionId);
        List<DiscountOnMgmtCollectionDTO> FindDiscountOnMgmtCollection(int APageIndex, int APageCount);
        #endregion

        #region CollectionCharge
        void SaveCollectionChargeDetails(CollectionChargeDTO ACollectionChargeDTO);
        void UpdateCollectionChargeDetails(int ACollectionChargeId, CollectionChargeDTO ACollectionChargeDTO);
        void DeleteCollectionCharge(int CollectionChargeId);

        List<CollectionChargeDTO> FindAll(int APageNo, CollectionChargeDTO AFilter);
        CollectionChargeDTO FindByCollectionChargeId(int ACollectionChargeId);
        List<CollectionChargeDTO> FindCollectionCharge(int APageIndex, int APageCount);
        #endregion

        #region InsuranceCharge
        void SaveInsuranceChargeDetails(InsuranceChargeDTO AInsuranceChargeDTO);
        void UpdateInsuranceChargeDetails(int AInsuranceChargeId, InsuranceChargeDTO AInsuranceChargeDTO);
        void DeleteInsuranceCharge(int AInsuranceChargeId);

        List<InsuranceChargeDTO> FindAll(int APageNo, InsuranceChargeDTO AFilter);
        InsuranceChargeDTO FindByInsuranceChargeId(int AInsuranceChargeId);
        List<InsuranceChargeDTO> FindInsuranceCharge(int APageIndex, int APageCount);
        #endregion

        #region StatementGrouping
        void SaveStatementGroupingDetails(StatementGroupingDTO AStatementGroupingDTO);
        void UpdateStatementGroupingDetails(int AStatementGroupingId, StatementGroupingDTO AStatementGroupingDTO);
        void DeleteStatementGrouping(string AProductTypeCode, string ANetworkCode, string ATransactionCode);

        List<StatementGroupingDTO> FindAll(int APageNo, StatementGroupingDTO AFilter);
        StatementGroupingDTO FindByStatementGroupingId(int AId);
        List<StatementGroupingDTO> FindStatementGrouping(int APageIndex, int APageCount);
        #endregion

        #region PaymentAllocationType
        void SavePaymentAllocationTypeDetails(PaymentAllocationTypeDTO APaymentAllocationTypeDTO);
        void UpdatePaymentAllocationTypeDetails(int APaymentAllocationTypeId, PaymentAllocationTypeDTO APaymentAllocationTypeDTO);
        void DeletePaymentAllocationType(int APaymentAllocationTypeId);

        List<PaymentAllocationTypeDTO> FindAll(int APageNo, PaymentAllocationTypeDTO AFilter);
        PaymentAllocationTypeDTO FindByPaymentAllocationTypeId(int APaymentAllocationTypeId);
        List<PaymentAllocationTypeDTO> FindPaymentAllocationType(int APageIndex, int APageCount);
        #endregion

        #region PlanMaster 

        List<PlanMasterDTO> FindPlanMaster(int APageIndex, int APageCount);
        PlanMasterDTO FindByPlanID(int APlanID, bool AIsFromApprovalQueue = false);
        void DeletePlanMaster(int APlanID);
        void UpdatePlanMasterDetails(int APlanID, PlanMasterDTO APlanMasterDTO);
        void SavePlanMasterDetails(PlanMasterDTO APlanMasterDTO);
        List<PlanMasterDTO> FindAll(int APageNo, PlanMasterDTO AFilter);
        decimal GetSBIFRate(DateTime AEffectiveDate);
        #endregion

        #region CardCreditSegment

        List<CardCreditSegmentDTO> FindCardCreditSegment(int APageIndex, int APageCount);
        CardCreditSegmentDTO FindByCardNumber(string ACardNumber, string ACreditSegmentCode);
        void DeleteCardCreditSegment(string ACardNumber, string ACreditSegmentCode);
        void UpdateCardCreditSegmentDetails(string ACardNumber, string ACreditSegmentCode, CardCreditSegmentDTO ACardCreditSegmentDTO);
        void SaveCardCreditSegmentDetails(CardCreditSegmentDTO ACardCreditSegmentDTO);
        List<CardCreditSegmentDTO> FindAllCardCreditSegments(int APageNo, CardCreditSegmentDTO AFilter);
        List<RUTCardNumberSearchDTO> GetRUTCardNumberDetails(string ARUT, string ACardNumber, string ACustomerName);
        #endregion

        #region Dependency Check

        bool ProductDependencyCheck(string Id);

        #endregion

        #region DiscountPercentage
        void SaveDiscountPercentageDetails(DiscountPercentageDTO ADiscountPercentageDTO);
        void UpdateDiscountPercentageDetails(int ADiscountPercentageId, DiscountPercentageDTO ADiscountPercentageDTO);
        void DeleteDiscountPercentage(int ADiscountPercentageId);

        List<DiscountPercentageDTO> FindAll(int APageNo, DiscountPercentageDTO AFilter);
        DiscountPercentageDTO FindByDiscountPercentageId(int ADiscountPercentageId);
        List<DiscountPercentageDTO> FindDiscountPercentage(int APageIndex, int APageCount);
        #endregion

        #region FeeTypes
        FeeTypesDTO GetFeeTypes(string AFeeTypeCode); 
        #endregion

        #region Merchants

        MerchantsDTO FindByMerchantsCode(string AMerchantCode);
        void SaveMerchantsDetails(MerchantsDTO AMerchantsDTO);
        void UpdateMerchantsDetails(string AMerchantsCode, MerchantsDTO AMerchantsDTO);
        List<MerchantsDTO> FindAll(int APageNo, MerchantsDTO AFilter);
        List<MerchantsDTO> FindMerchant(int APageIndex, int APageCount);
        void DeleteMerchant(string AMerchantCode);
     
        #endregion

        #region MerchantDiscount

        MerchantDiscountDTO FindByMerchantDiscountId(int AMerchantDiscountId);
        void SaveMerchantDiscountDetails(MerchantDiscountDTO AMerchantDiscountDTO);
        void UpdateMerchantDiscountDetails(int AMerchantDiscountId, MerchantDiscountDTO AMerchantDiscountDTO);
        List<MerchantDiscountDTO> FindAll(int APageNo, MerchantDiscountDTO AMerchantDiscountDTO);
        List<MerchantDiscountDTO> FindMerchantDiscount(int APageIndex, int APageCount);
        void DeleteMerchantDiscount(int AMerchantDiscountId);
        bool CheckIfExistsValidDiscountForSelectedMerchant(int AMerchantDiscountId, string AMerchantCode, DateTime AFromDate);

        #endregion

        #region MerchatTransactions
        List<MerchantTransactionsDTO> GetMerchantTransactionDetails(string AMerchantCode, string AMerchantName, DateTime ATxnFromDate, DateTime ATxnToDate);
        #endregion

        #region Branch
        List<BranchDTO> FindAll(int APageNo, BranchDTO ABranchDTO);
        List<BranchDTO> FindBranch(int APageindex, int APageCount);
        BranchDTO FindByBranchCode(string ABranchCode, string AOrganizationCode);
        void SaveBranchDetails(BranchDTO ABranchDTO);
        void UpdateBranchDetails(string ABranchCode, BranchDTO ABranchDTO);
        #endregion 


        #region StatementInterest

        List<StatementInterestDTO> FindAll(int APageNo, StatementInterestDTO AStatementInterestDTO);
        List<StatementInterestDTO> FindStatmentInterest(int APageindex, int APageCount);
        StatementInterestDTO FindByBillingMonthYear(string AOrganizationCode,int? ABillingMonth,int? ABillingYear);
        void SaveStatementInterestDetails(StatementInterestDTO AStatementInterestDTO);
        void UpdateStatementInterestDetails(int? ABillingMonth, int? ABillingYear, StatementInterestDTO AStatementInterestDTO);

        #endregion

        #region ChangeBusinessDate
        ChangeBusinessDateDTO FindByBusinessDateOrgCode(string AOrganizationCode);
        void UpdateBusinessDate(ChangeBusinessDateDTO AChangeBusinessDateDTO);
        #endregion

        #region DirectSalesAgent
        DirectSalesAgentDTO GetDirectSalesAgentDetails(string ARUT, string ASalesId);
        DirectSalesAgentDTO FindDSAByCommercialBranchCode(string ABranchCode);
        #endregion

        #region UFRateSettings
        List<UFRateSettingsDTO> FilterUFRateSettings(int ABillMonth, int ABillYear);
        UFRateSettingsDTO FindByUFRateSettingsEffectiveDate(DateTime AEffectiveDate);
        List<UFRateSettingsDTO> FindUFRateSettings(int APageindex, int APageCount);
        void SaveUFRateSettings(UFRateSettingsDTO AUFRateSettingsDTO);
        void UpdateUFRateSettings(DateTime AEffectiveDate, UFRateSettingsDTO AUFRateSettingsDTO);

        #endregion
        
        #region SalesId
        List<SalesIdDTO> FindAll(int APageNo, SalesIdDTO ASalesIdDTO);
        List<SalesIdDTO> FindSalesId(int APageindex, int APageCount);
        SalesIdDTO FindBySalesId(string ASalesId, string ARUT, string AOrganizationCode);
        void SaveSalesDetails(SalesIdDTO ASalesIdDTO);
        void UpdateSalesIdDetails(string ASsalesID, string ARUT, SalesIdDTO ASalesIdDTO);
        #endregion 

        #region DirectSalesAgent
        List<DirectSalesAgentDTO> FindAll(int APageNo, DirectSalesAgentDTO ADirectSalesAgentDTO);
        List<DirectSalesAgentDTO> FindDirectSalesAgent(int APageindex, int APageCount);
        DirectSalesAgentDTO FindByDirectSalesAgent(string ADSACode);
        void SaveDirectSalesAgent(DirectSalesAgentDTO ADirectSalesAgentDTO);
        void UpdateDirectSalesAgent(string ADSACode, DirectSalesAgentDTO ADirectSalesAgentDTO);
        #endregion

        #region AdjustmentFinancialAccount
        void SaveAdjustmentFinancialAccountDetails(AdjustmentFinancialAccountDTO AAdjustmentFinancialAccountDTO);
        void UpdateAdjustmentFinancialAccountDetails(int AAdjustmentFinancialAccountId, AdjustmentFinancialAccountDTO AAdjustmentFinancialAccountDTO);
        void DeleteAdjustmentFinancialAccount(int AAdjustmentFinancialAccountId);

        List<AdjustmentFinancialAccountDTO> FindAll(int APageNo, AdjustmentFinancialAccountDTO AFilter);
        AdjustmentFinancialAccountDTO FindByAdjustmentFinancialAccountId(int AAdjustmentFinancialAccountId);
        List<AdjustmentFinancialAccountDTO> FindAdjustmentFinancialAccount(int APageIndex, int APageCount);
        #endregion

        List<RUTCardNumberSearchDTO> GetAllRUTZoomDetails(string ARUT, string ACardNumber, string ACustomerName);

    }
}
