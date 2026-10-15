using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Web.Http.Controllers;

using CC_Ingenio.Data;
using CC_Ingenio.Data.Contracts;

using CC_Ingenio.Core.ContactRepo.ContactAggregate;
using CC_Ingenio.Core.ContactRepo.AddressAggregate;
using CC_Ingenio.Core.ContactRepo.ContactAddressAggregate;

using CC_Ingenio.Core.MasterRepo.MasterAggregate;
using CC_Ingenio.Core.LookUpRepo;

using CC_Ingenio.Manager;
using CC_Ingenio.Manager.Contracts;

using CC_Ingenio.Repository.ContactRepo;
using CC_Ingenio.Repository.MasterRepo;
using CC_Ingenio.Repository.LookUpRepo;
using CC_Ingenio.Repository.AppovalRepo;

using CC_Ingenio.Core.ApprovalRepo.ApprovalAggregate;
using CC_Ingenio.Core.UserRepo.UserAggregate;
using CC_Ingenio.Repository.UserRepo;
using CC_Ingenio.Repository.CardBookingRepo;
using CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate;
using CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate;
using CC_Ingenio.Repository.AuthorizationRepo;
using CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate;
using CC_Ingenio.Repository.CustomerServiceRepo;

using CC_Ingenio.Core.BillingRepo.BillingAggregate;
using CC_Ingenio.Repository.BillingRepo;
using System.Configuration;

namespace CC_Ingenio.Web.DependencyInjection
{
    public class BootStrapContainer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                                .BasedOn<IController>()
                                .LifestyleTransient());

            container.Register(

                         Component.For<IQueryableUnitOfWork, UnitOfWork>().ImplementedBy<UnitOfWork>(),
                            

                         Component.For<IContactRepository, ContactRepository>().ImplementedBy<ContactRepository>(),
                         Component.For<IAddressRepository, AddressRepository>().ImplementedBy<AddressRepository>(),
                         Component.For<IContactAddressRepository, ContactAddressRepository>().ImplementedBy<ContactAddressRepository>(),
                         Component.For<IContactManager>().ImplementedBy<ContactManager>(),

                         //master
                         Component.For<ICurrencyRepository, CurrencyRepository>().ImplementedBy<CurrencyRepository>(),
                         Component.For<IOrganizationRepository, OrganizationRepository>().ImplementedBy<OrganizationRepository>(),
                         Component.For<IFranchiseRepository, FranchiseRepository>().ImplementedBy<FranchiseRepository>(),
                         Component.For<ISourcingRepository, SourcingRepository>().ImplementedBy<SourcingRepository>(),
                         Component.For<ICompanyRepository, CompanyRepository>().ImplementedBy<CompanyRepository>(),
                         Component.For<ICityRepository, CityRepository>().ImplementedBy<CityRepository>(),
                         Component.For<ICreditSegmentRepository, CreditSegmentRepository>().ImplementedBy<CreditSegmentRepository>(),
                         Component.For<ISystemParameterRepository, SystemParameterRepository>().ImplementedBy<SystemParameterRepository>(),
                         Component.For<IDesignationRepository, DesignationRepository>().ImplementedBy<DesignationRepository>(),
                         Component.For<ICustomerSegmentRepository, CustomerSegmentRepository>().ImplementedBy<CustomerSegmentRepository>(),
                         Component.For<ICustomerSubSegmentRepository, CustomerSubSegmentRepository>().ImplementedBy<CustomerSubSegmentRepository>(),
                         Component.For<ICardStatusRepository, CardStatusRepository>().ImplementedBy<CardStatusRepository>(),
                         Component.For<IGeneralRepository, GeneralRepository>().ImplementedBy<GeneralRepository>(),
                         Component.For<IContractRepository, ContractRepository>().ImplementedBy<ContractRepository>(),
                         Component.For<IProductTypeRepository, ProductTypeRepository>().ImplementedBy<ProductTypeRepository>(),
                         Component.For<ITransactionRepository, TransactionRepository>().ImplementedBy<TransactionRepository>(),
                         Component.For<IBoostMaintenanceRepository, BoostMaintenanceRepository>().ImplementedBy<BoostMaintenanceRepository>(),
                         Component.For<IFeeRepository, FeeRepository>().ImplementedBy<FeeRepository>(),
                         Component.For<IFeeTypeRepository, FeeTypeRepository>().ImplementedBy<FeeTypeRepository>(),
                         Component.For<IProductRepository, ProductRepository>().ImplementedBy<ProductRepository>(),
                         Component.For<IBlockCodeRepository, BlockCodeRepository>().ImplementedBy<BlockCodeRepository>(),
                         Component.For<IMerchantRepository  , MerchantRepository>().ImplementedBy<MerchantRepository >(),
                         Component.For<IMCCRepository, MCCRepository>().ImplementedBy<MCCRepository>(),
                         Component.For<ICardCreditSegmentRepository, CardCreditSegmentRepository>().ImplementedBy<CardCreditSegmentRepository>(),
                         Component.For<IMerchantsRepository, MerchantsRepository>().ImplementedBy<MerchantsRepository>(),
                             
                         Component.For<ICustomerRepository, CustomerRepository>().ImplementedBy<CustomerRepository>(),
                         Component.For<ICalendarDateMaintenanceRepository, CalendarDateRepository>().ImplementedBy<CalendarDateRepository>(),
                         Component.For<ITransactionSubCodeRepository, TransactionSubCodeRepository>().ImplementedBy<TransactionSubCodeRepository>(),
                         Component.For<IBranchRepository, BranchRepository>().ImplementedBy<BranchRepository>(),
                         Component.For<ICustomerAddressRepository,CustomerAddressRepository>().ImplementedBy<CustomerAddressRepository>(),
                         Component.For<ICustomerOccupationRepository, CustomerOccupationRepository>().ImplementedBy<CustomerOccupationRepository>(),
                         Component.For<ICustomerCurrentAccountDetailRepository, CustomerCurrentAccountDetailRepository>().ImplementedBy<CustomerCurrentAccountDetailRepository>(),
                         Component.For<ICustomerCreditCardDetailRepository, CustomerCreditCardDetailRepository>().ImplementedBy<CustomerCreditCardDetailRepository>(),
                         Component.For<ICustomerCommericalHouseConsumptionRepository, CustomerCommericalHouseConsumptionRepository>().ImplementedBy<CustomerCommericalHouseConsumptionRepository>(),
                         Component.For<ICustomerBillingDetailRepository, CustomerBillingDetailRepository>().ImplementedBy<CustomerBillingDetailRepository>(),
                         Component.For<ICustomerDependentRepository, CustomerDependentRepository>().ImplementedBy<CustomerDependentRepository>(),
                         Component.For<ICustomerDecisionRepository, CustomerDecisionRepository>().ImplementedBy<CustomerDecisionRepository>(),
                         Component.For<IFinancialAccountRepository, FinancialAccountRepository>().ImplementedBy<FinancialAccountRepository>(),
                         Component.For<IFinancialActivityRepository, FinancialActivityRepository>().ImplementedBy<FinancialActivityRepository>(),
                         Component.For<IBillingCycleParameterRepository, BillingCycleParameterRepository>().ImplementedBy<BillingCycleParameterRepository>(),
                         Component.For<ISBIFInterestRepository, SBIFInterestRepository>().ImplementedBy<SBIFInterestRepository>(),
                         Component.For<ISBIFInterestDetailsRepository, SBIFInterestDetailsRepository>().ImplementedBy<SBIFInterestDetailsRepository>(),
                         Component.For<IInterestMasterRepository, InterestMasterRepository>().ImplementedBy<InterestMasterRepository>(),
                         Component.For<IInterestMasterDetailsRepository, InterestMasterDetailsRepository>().ImplementedBy<InterestMasterDetailsRepository>(),
                         Component.For<ICommissionRepository, CommissionRepository>().ImplementedBy<CommissionRepository>(),
                         Component.For<ICommissionChargeRepository, CommissionChargeRepository>().ImplementedBy<CommissionChargeRepository>(),
                         Component.For<ICommissionChargeDetailsRepository, CommissionChargeDetailsRepository>().ImplementedBy<CommissionChargeDetailsRepository>(),
                         Component.For<IBlockCodeForCommissionExclusionRepository, BlockCodeForCommissionExclusionRepository>().ImplementedBy<BlockCodeForCommissionExclusionRepository>(),
                         Component.For<IDiscountOnMgmtCollectionRepository, DiscountOnMgmtCollectionRepository>().ImplementedBy<DiscountOnMgmtCollectionRepository>(),
                         Component.For<IDiscountOnMgmtCollectionDetailsRepository, DiscountOnMgmtCollectionDetailsRepository>().ImplementedBy<DiscountOnMgmtCollectionDetailsRepository>(),
                         Component.For<ICollectionChargeRepository, CollectionChargeRepository>().ImplementedBy<CollectionChargeRepository>(),
                         Component.For<ICollectionChargeDetailsRepository, CollectionChargeDetailsRepository>().ImplementedBy<CollectionChargeDetailsRepository>(),
                         Component.For<IInsuranceChargeRepository, InsuranceChargeRepository>().ImplementedBy<InsuranceChargeRepository>(),
                         Component.For<IStatementGroupingRepository, StatementGroupingRepository>().ImplementedBy<StatementGroupingRepository>(),
                         Component.For<IPaymentAllocationTypeRepository, PaymentAllocationTypeRepository>().ImplementedBy<PaymentAllocationTypeRepository>(),
                         Component.For<IPlanMasterRepository, PlanMasterRepository>().ImplementedBy<PlanMasterRepository>(),
                         Component.For<IBillingParameterRepository, BillingParameterRepository>().ImplementedBy<BillingParameterRepository>(),
                         Component.For<ISystemMaintenanceConfigurationRepository, SystemMaintenanceConfigurationRepository>().ImplementedBy<SystemMaintenanceConfigurationRepository>(),
                         Component.For<IDiscountPercentageRepository, DiscountPercentageRepository>().ImplementedBy<DiscountPercentageRepository>(),
                         Component.For<IDiscountPercentageDetailsRepository, DiscountPercentageDetailsRepository>().ImplementedBy<DiscountPercentageDetailsRepository>(),
                         Component.For<ICreditSubSegmentRepository, CreditSubSegmentRepository>().ImplementedBy<CreditSubSegmentRepository>(),
                         Component.For<IChangeBusinessDateRepository, ChangeBusinessDateRepository>().ImplementedBy<ChangeBusinessDateRepository>(),
                         Component.For<IDirectSalesAgentRepository, DirectSalesAgentRepository>().ImplementedBy<DirectSalesAgentRepository>(),
                         Component.For<IUFRateSettingsRepository, UFRateSettingsRepository>().ImplementedBy<UFRateSettingsRepository>(),
                         Component.For<ISalesIdRepository, SalesIdRepository>().ImplementedBy<SalesIdRepository>(),
                         Component.For<IMerchantDiscountRepository, MerchantDiscountRepository>().ImplementedBy<MerchantDiscountRepository>(),
                         Component.For<IAdjustmentFinancialAccountRepository, AdjustmentFinancialAccountRepository>().ImplementedBy<AdjustmentFinancialAccountRepository>(),

                         Component.For<IMasterManager>().ImplementedBy<MasterManager>(),
                         
                         //lookup
                         Component.For<ICountryRepository, CountryRepository>().ImplementedBy<CountryRepository>(),
                         Component.For<IRegionRepository, RegionRepository>().ImplementedBy<RegionRepository>(),
                         Component.For<ICityCategoryRepository, CityCategoryRepository>().ImplementedBy<CityCategoryRepository>(),
                         Component.For<IHubRepository, HubRepository>().ImplementedBy<HubRepository>(),
                         Component.For<ISpokeRepository, SpokeRepository>().ImplementedBy<SpokeRepository>(),
                         Component.For<ICommunaRepository, CommunaRepository>().ImplementedBy<CommunaRepository>(),

                         Component.For<IAlertTypeRepository,AlertTypeRepository>().ImplementedBy<AlertTypeRepository>(),
                         
                         Component.For<ICompanyCategoryRepository, CompanyCategoryRepository>().ImplementedBy<CompanyCategoryRepository>(),
                         Component.For<ILegalEntityRepository, LegalEntityRepository>().ImplementedBy<LegalEntityRepository>(),
                         Component.For<IContractTemplateRepository, ContractTemplateRepository>().ImplementedBy<ContractTemplateRepository>(),
                         Component.For<IApprovalContextRepository, ApprovalContextRepository>().ImplementedBy<ApprovalContextRepository>(),
                         Component.For<IProductCategoryRepository,ProductCategoryRepository>().ImplementedBy<ProductCategoryRepository>(),
                         Component.For<IMemberTypeRepository, MemberTypeRepository>().ImplementedBy<MemberTypeRepository>(),

                         Component.For<IDefaultAffinityRepository, DefaultAffinityRepository>().ImplementedBy<DefaultAffinityRepository>(),
                         Component.For<IBillingCycleRepository, BillingCycleRepository>().ImplementedBy<BillingCycleRepository>(),
                         Component.For<IInterestMethodRepository, InterestMethodRepository>().ImplementedBy<InterestMethodRepository>(),
                         Component.For<IServiceRepository, ServiceRepository>().ImplementedBy<ServiceRepository>(),
                         Component.For<IProductStatusRepository, ProductStatusRepository>().ImplementedBy<ProductStatusRepository>(),
                         Component.For<IProductMemberTypeRepository, ProductMemberTypeRepository>().ImplementedBy<ProductMemberTypeRepository>(),
                         Component.For<IProductClassRepository, ProductClassRepository>().ImplementedBy<ProductClassRepository>(),
                         Component.For<IProductSubCategoryRepository, ProductSubCategoryRepository>().ImplementedBy<ProductSubCategoryRepository>(),

                         Component.For<IDecisionRepository,DecisionRepository>().ImplementedBy<DecisionRepository>(),
                         Component.For<IDecisionReasonRepository,DecisionReasonRepository>().ImplementedBy<DecisionReasonRepository>(),
                         Component.For<IBankRepository, BankRepository>().ImplementedBy<BankRepository>(),
                         Component.For<ICardTypeRepository, CardTypeRepository>().ImplementedBy<CardTypeRepository>(),
                         Component.For<IContractTypeRepository, ContractTypeRepository>().ImplementedBy<ContractTypeRepository>(),
                         Component.For<INetworkRepository, NetworkRepository>().ImplementedBy<NetworkRepository>(),
                         Component.For<IPositionRepository, PositionRepository>().ImplementedBy<PositionRepository>(),
                         Component.For<IFirstBillingRepository, FirstBillingRepository>().ImplementedBy<FirstBillingRepository>(),
                         Component.For<IInterestCalculationRepository, InterestCalculationRepository>().ImplementedBy<InterestCalculationRepository>(),
                         Component.For<IFeeTypesRepository, FeeTypesRepository>().ImplementedBy<FeeTypesRepository>(),
                         Component.For<IEducationRepository, EducationRepository>().ImplementedBy<EducationRepository>(),
                         Component.For<IProfessionRepository, ProfessionRepository>().ImplementedBy<ProfessionRepository>(),
                         Component.For<IMaritialStatusRepository, MaritialStatusRepository>().ImplementedBy<MaritialStatusRepository>(),
                         Component.For<ILookUpManager>().ImplementedBy<LookUpManager>(),
                         Component.For<IClosureReasonRepository, ClosureReasonRepository>().ImplementedBy<ClosureReasonRepository>(),
                         Component.For<IReactivationReasonRepository, CardReactivationReasonRepository>().ImplementedBy<CardReactivationReasonRepository>(),
                         Component.For<IApprovalQueueContextRepository, ApprovalQueueContextRepository>().ImplementedBy<ApprovalQueueContextRepository>(),
                         Component.For<IUserEntityRepository, UserEntityRepository>().ImplementedBy<UserEntityRepository>(),
                         Component.For<IUserCultureRepository, UserCultureRepository>().ImplementedBy<UserCultureRepository>(),
                         Component.For<ITransactionCheckFrequency, TransactionCheckFrequencyRepository>().ImplementedBy<TransactionCheckFrequencyRepository>(),
                         //approval queue
                         Component.For<IApprovalQueueRepository, ApprovalQueueRepository>().ImplementedBy<ApprovalQueueRepository>(),
                         Component.For<IApprovalQueueManager>().ImplementedBy<ApprovalQueueManager>(),

                         //user
                         Component.For<IUserRepository, UserRepository>().ImplementedBy<UserRepository>(),
                         Component.For<IUserGroupRepository, UserGroupRepository>().ImplementedBy<UserGroupRepository>(),
                         Component.For<IUserAllowedBranchRepository, UserAllowedBranchRepository>().ImplementedBy<UserAllowedBranchRepository>(),
                         Component.For<IUserManager>().ImplementedBy<UserManager>(),
                         Component.For<IUserGroupMembersRepository,UserGroupMembersRepository>().ImplementedBy<UserGroupMembersRepository>(),
                         Component.For<IUserGroupPrivilegeRepository, UserGroupPrivilegeRepository>().ImplementedBy<UserGroupPrivilegeRepository>(),
                              

                         
                         //card booking
                         Component.For<ICardEmbossingRepository, CardEmbossingRepository>().ImplementedBy<CardEmbossingRepository>(),
                         Component.For<ICardMasterRepository, CardMasterRepository>().ImplementedBy<CardMasterRepository>(),
                         Component.For<ICardBookingManager>().ImplementedBy<CardBookingManager>(),
                         Component.For<IAuthorizationManager >().ImplementedBy<AuthorizationManager >(),
                         Component.For<IForceAuthorizationRepository, AuthorizationRepository>().ImplementedBy<AuthorizationRepository>(),
                         Component.For<IManualTransactionRepository, ManualTransactionRepository>().ImplementedBy<ManualTransactionRepository>(),
                         Component.For<IEMIAuthorizationRepository, EMIAuthorizationRepository>().ImplementedBy<EMIAuthorizationRepository>(),
                         Component.For<IEMITransactionRepository, EMITransactionRepository>().ImplementedBy<EMITransactionRepository>(),
                         Component.For<IEMITransactionInstallmentRepository, EMITransactionInstallmentRepository>().ImplementedBy<EMITransactionInstallmentRepository>(),
                         Component.For<IAuthorizationCancellationRepository, AuthorizationCancellationRepository>().ImplementedBy<AuthorizationCancellationRepository>(),
                          Component.For<IPaymentHolidayRepository, PaymentHolidayRepository>().ImplementedBy<PaymentHolidayRepository>(),

                         //Transaction Adjustment
                         Component.For<IBillingCycleViewRepository, BillingCycleViewRepository>().ImplementedBy<BillingCycleViewRepository>(),
                         Component.For<IAdjustmentRepository, AdjustmentRepository>().ImplementedBy<AdjustmentRepository>(),

                         //Customer Service
                         Component.For<ICustomerServiceDetailsRepository, CustomerServiceDetailsRepository>().ImplementedBy<CustomerServiceDetailsRepository>(),
                         Component.For<INotepadEntryRepository, NotepadEntryRepository>().ImplementedBy<NotepadEntryRepository>(),
                         Component.For < IBlockSegmentCardRepository,BlockSegmentCardRepository>().ImplementedBy<BlockSegmentCardRepository>(),
                         Component.For<ICustomerServiceManager, CustomerServiceManager>().ImplementedBy<CustomerServiceManager>(),
                         Component.For<ICardClosureRepository, CardClosureRepository>().ImplementedBy<CardClosureRepository>(),
                         Component.For<IUpdatedCustomerAddressRepository,UpdatedCustomerAddressRepository>().ImplementedBy<UpdatedCustomerAddressRepository>(),
                         Component.For<ICardReactivationRepository, CardReactivationRepository>().ImplementedBy<CardReactivationRepository>(),
                         Component.For<ICardClosureHistoryRepository, CardClosureHistoryRepository>().ImplementedBy<CardClosureHistoryRepository>(),
                         Component.For<IAlertMessageRepository,AlertMessageRepository>().ImplementedBy<AlertMessageRepository>(),
                         Component.For<ILostStolenCardBlockRepository, LostStolenCardBlockRepository>().ImplementedBy<LostStolenCardBlockRepository>(),
                         Component.For<ILostStolenReissueRequestRepository, LostStolenReissueRequestRepository>().ImplementedBy<LostStolenReissueRequestRepository>(),
                         Component.For<IChangeCreditLimitRepository, ChangeCreditLimitRepository>().ImplementedBy<ChangeCreditLimitRepository>(),
                         Component.For<ISwapProductRepository, SwapProductRepository>().ImplementedBy<SwapProductRepository>(),

                         //Billing
                         Component.For<IBillingManager, BillingManager>().ImplementedBy<BillingManager>(),
                         Component.For<IOnlinePaymentRepository, OnlinePaymentRepository>().ImplementedBy<OnlinePaymentRepository>(),
                         Component.For<IRefinancingRepository, RefinancingRepository>().ImplementedBy<RefinancingRepository>(),
                         Component.For<IRenegotiationRepository, RenegotiationRepository>().ImplementedBy<RenegotiationRepository>(),
                         Component.For<IStatementGenerationRepository, StatementGenerationRepository>().ImplementedBy<StatementGenerationRepository>(),
                         
                         //StatementInterest
                         Component.For<IStatementInterestRepository,StatementInterestRepository>().ImplementedBy<StatementInterestRepository>(),

                         Types.FromThisAssembly().BasedOn<IHttpController>().LifestyleTransient()

                         );

        }


    }
    
}