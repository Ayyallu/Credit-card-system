using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using CT=CC_Ingenio.Core.ContactRepo.ContactAggregate;
using CC_Ingenio.Core.ContactRepo.AddressAggregate;
using CC_Ingenio.Core.ContactRepo.ContactAddressAggregate;

using CC_Ingenio.Core.MasterRepo.MasterAggregate;

using CC_Ingenio.Data.Contracts;
using CC_Ingenio.Core.LookUpRepo;
using CC_Ingenio.Core.ApprovalRepo.ApprovalAggregate;

using CC_Ingenio.Core.UserRepo.UserAggregate;
using CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate;
using CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate;
using CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate;
using CC_Ingenio.Core.BillingRepo.BillingAggregate;
using System.Collections;
using CC_Ingenio.Common;



namespace CC_Ingenio.Data
{
    public class UnitOfWork : DbContext, IQueryableUnitOfWork
    {

        #region Constructor
        public UnitOfWork()
        {
            Database.SetCommandTimeout(1500);
        }

        // Used by tests/DI to supply provider options instead of the legacy
        // appSettings-driven connection string built in OnConfiguring.
        public UnitOfWork(DbContextOptions<UnitOfWork> options)
            : base(options)
        {
            Database.SetCommandTimeout(1500);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Legacy EF5 had ProxyCreationEnabled/LazyLoadingEnabled = true.
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(BuildConnectionString());
            }
        }
        #endregion

        #region IDbSet members
        
        #region Masters

        #region Currency
        DbSet<Currency> dbCurrency;
        public DbSet<Currency> Currency
        {
            get
            {
                if (dbCurrency == null)
                    dbCurrency = base.Set<Currency>();

                return dbCurrency;
            }
        }
        #endregion

        #region Country
        DbSet<Country> dbCountry;
        public DbSet<Country> Country
        {
            get
            {
                if (dbCountry == null)
                    dbCountry = base.Set<Country>();

                return dbCountry;
            }
        }
        #endregion

        #region SystemMaintenanceConfiguration
        DbSet<SystemMaintenanceConfiguration> dbSystemMaintenanceConfiguration;
        public DbSet<SystemMaintenanceConfiguration> SystemMaintenanceConfiguration
        {
            get
            {
                if (dbSystemMaintenanceConfiguration == null)
                    dbSystemMaintenanceConfiguration = base.Set<SystemMaintenanceConfiguration>();

                return dbSystemMaintenanceConfiguration;
            }
        }
        #endregion

        #region Sourcing
        DbSet<Sourcing> dbSourcing;
        public DbSet<Sourcing> Sourcing
        {
            get 
            {
                if (dbSourcing == null)
                {
                    dbSourcing = base.Set<Sourcing>();
                }

                return dbSourcing;            
            }
        }
        #endregion

        #region Merchant
        DbSet<Merchant> dbMerchant;
        public DbSet<Merchant> Merchant
        {
            get
            {
                if (dbMerchant == null)
                    dbMerchant = base.Set<Merchant>();

                return dbMerchant;
            }
        }
        #endregion

        #region MCC
        DbSet<MCC > dbMCC;
        public DbSet<MCC> MCC
        {
            get
            {
                if (dbMCC == null)
                    dbMCC = base.Set<MCC>();

                return dbMCC;
            }
        }
        #endregion

        #region CardEmbossing
        DbSet<CardEmbossing> dbCardEmbossing;
        public DbSet<CardEmbossing> CardEmbossing
        {
            get
            {
                if (dbCardEmbossing == null)
                    dbCardEmbossing = base.Set<CardEmbossing>();

                return dbCardEmbossing;
            }
        }
        #endregion
        
        #region ForceAuthorization
        DbSet<ForceAuthorization> dbForceAuthorization;
        public DbSet<ForceAuthorization> ForceAuthorization
        {
            get
            {
                if (dbForceAuthorization == null)
                    dbForceAuthorization = base.Set<ForceAuthorization>();

                return dbForceAuthorization;
            }
        }
        #endregion

        #region Company
        DbSet<Company> dbCompany;
        public DbSet<Company> Company
        {
            get 
            {
                if (dbCompany == null)
                {
                    dbCompany = base.Set<Company>();
                }
                return dbCompany;
            }
        }
        #endregion

        #region BillingParameter
        DbSet<BillingParameter> dbBillingParameter;
        public DbSet<BillingParameter> BillingParameter
        {
            get
            {
                if (dbBillingParameter == null)
                {
                    dbBillingParameter = base.Set<BillingParameter>();
                }
                return dbBillingParameter;
            }
        }
        #endregion
        
        #region ManualTransaction
        DbSet<ManualTransaction> dbManualTransaction;
        public DbSet<ManualTransaction > ManualTransaction
        {
            get
            {
                if (dbManualTransaction == null)
                {
                    dbManualTransaction = base.Set<ManualTransaction>();
                }
                return ManualTransaction;
            }
        }
        #endregion

        #region Organization
        DbSet<Organization> dbOrganization;
        public DbSet<Organization> Organization
        {
            get
            {
                if (dbOrganization == null)
                    dbOrganization = base.Set<Organization>();

                return dbOrganization;
            }
        }
        #endregion

        #region Franchise
        DbSet<Franchise> dbFranchise;
        public DbSet<Franchise> Franchise
        {
            get
            {
                if (dbFranchise == null)
                    dbFranchise = base.Set<Franchise>();

                return dbFranchise;
            }
        }
        #endregion

        #region City
        DbSet<City> dbCity;
        public DbSet<City> City
        {
            get
            {
                if (dbCity == null)
                    dbCity = base.Set<City>();

                return dbCity;
            }
        }
        #endregion

        #region ApprovalQueue
        DbSet<ApprovalQueue> dbApprovalQueue;
        public DbSet<ApprovalQueue> ApprovalQueue
        {
            get
            {
                if (dbApprovalQueue == null)
                    dbApprovalQueue = base.Set<ApprovalQueue>();
                return dbApprovalQueue;
            }    
        }
        #endregion

        #region CityCategory
        DbSet<CityCategory> dbCityCategory;
        public DbSet<CityCategory> CityCategory
        {
            get
            {
                if (dbCityCategory == null)
                    dbCityCategory = base.Set<CityCategory>();

                return dbCityCategory;
            }
        }
        #endregion

        #region Region
        DbSet<Region> dbRegion;
        public DbSet<Region> Region
        {
            get
            {
                if (dbRegion == null)
                    dbRegion = base.Set<Region>();

                return dbRegion;
            }
        }
        #endregion

        #region Hub
        DbSet<Hub> dbHub;
        public DbSet<Hub> Hub
        {
            get
            {
                if (dbHub == null)
                    dbHub = base.Set<Hub>();

                return dbHub;
            }
        }
        #endregion

        #region Spoke
        DbSet<Spoke> dbSpoke;
        public DbSet<Spoke> Spoke
        {
            get
            {
                if (dbSpoke == null)
                    dbSpoke = base.Set<Spoke>();

                return dbSpoke;
            }
        }
        #endregion

        #region Communa
        DbSet<Communa> dbCommuna;
        public DbSet<Communa> Communa
        {
            get
            {
                if (dbCommuna == null)
                    dbCommuna = base.Set<Communa>();

                return dbCommuna;
            }
        }
        #endregion

        #region CompanyCategory
        DbSet<CompanyCategory> dbCompanyCategory;
        public DbSet<CompanyCategory> CompanyCategory
        {
            get
            {
                if (dbCompanyCategory == null)
                {
                    dbCompanyCategory = base.Set<CompanyCategory>();
                }
                return dbCompanyCategory;
            }
        }
        #endregion

        #region CreditSegment
        DbSet<CreditSegment> dbCreditSegment;
        public DbSet<CreditSegment> CreditSegment
        {
            get
            {
                if (dbCreditSegment == null)
                {
                    dbCreditSegment = base.Set<CreditSegment>();
                }
                return dbCreditSegment;
            }
        }
        #endregion

        #region CreditSubSegment
        DbSet<CreditSubSegment> dbCreditSubSegment;
        public DbSet<CreditSubSegment> CreditSubSegment
        {
            get
            {
                if (dbCreditSubSegment == null)
                {
                    dbCreditSubSegment = base.Set<CreditSubSegment>();
                }
                return dbCreditSubSegment;
            }
        }
        #endregion

        #region Designation
        DbSet<Designation> dbDesignation;
        public DbSet<Designation> Designation
        {
            get
            {
                if (dbDesignation == null)
                {
                    dbDesignation = base.Set<Designation>();
                }
                return dbDesignation;
            }
        }
        #endregion

        #region LegalEntity
        DbSet<LegalEntity> dbLegalEntity;
        public DbSet<LegalEntity> LegalEntity
        {
            get
            {
                if (dbLegalEntity == null)
                {
                    dbLegalEntity = base.Set<LegalEntity>();
                }
                return dbLegalEntity;
            }
        }
        #endregion

        #region CustomerSegment
        DbSet<CustomerSegment> dbCustomerSegment;
        public DbSet<CustomerSegment> CustomerSegment
        {
            get
            {
                if (dbCustomerSegment == null)
                {
                    dbCustomerSegment = base.Set<CustomerSegment>();
                }
                return dbCustomerSegment;
            }
        }
        #endregion

        #region CustomerSubSegment
        DbSet<CustomerSubSegment> dbCustomerSubSegment;
        public DbSet<CustomerSubSegment> CustomerSubSegment
        {
            get
            {
                if (dbCustomerSubSegment == null)
                {
                    dbCustomerSubSegment = base.Set<CustomerSubSegment>();
                }
                return dbCustomerSubSegment;
            }
        }
        #endregion

        #region SystemParameter
        DbSet<SystemParameter> dbSystemParameter;
        public DbSet<SystemParameter> SystemParameter
        {
            get
            {
                if (dbSystemParameter == null)
                {
                    dbSystemParameter = base.Set<SystemParameter>();
                }
                return dbSystemParameter;
            }
        }
        #endregion

        #region CardStatus
        DbSet<CardStatus> dbCardStatus;
        public DbSet<CardStatus> CardStatus
        {
            get
            {
                if (dbCardStatus == null)
                    dbCardStatus = base.Set<CardStatus>();

                return dbCardStatus;
            }
        }
        #endregion

        #region Contract
        DbSet<Contract> dbContract;
        public DbSet<Contract> Contract
        {
            get
            {
                if (dbContract == null)
                    dbContract = base.Set<Contract>();

                return dbContract;
            }
        }
        #endregion

        #region General
        DbSet<General> dbGeneral;
        public DbSet<General> General
        {
            get
            {
                if (dbGeneral == null)
                    dbGeneral = base.Set<General>();

                return dbGeneral;
            }
        }
        #endregion

        #region ContractTemplate
        DbSet<ContractTemplate> dbContractTemplate;
        public DbSet<ContractTemplate> ContractTemplate
        {
            get
            {
                if (dbContractTemplate == null)
                    dbContractTemplate = base.Set<ContractTemplate>();
                
                return dbContractTemplate;
            }
        }
        #endregion

        #region Product
        DbSet<Product> dbProduct;
        public DbSet<Product> Product
        {
            get
            {
                if (dbProduct == null)
                    dbProduct = base.Set<Product>();

                return dbProduct;
            }
        }
        #endregion

        #region ProductType
        DbSet<ProductType> dbProductType;
        public DbSet<ProductType> ProductType
        {
            get
            {
                if (dbProductType == null)
                    dbProductType = base.Set<ProductType>();

                return dbProductType;
            }
        }
        #endregion

        #region Transaction
        DbSet<Transaction> dbTransaction;
        public DbSet<Transaction> Transaction
        {
            get
            {
                if (dbTransaction == null)
                    dbTransaction = base.Set<Transaction>();

                return dbTransaction;
            }
        }
        #endregion

        #region Position
        DbSet<Position> dbPosition;
        public DbSet<Position> Position
        {
            get
            {
                if (dbPosition == null)
                    dbPosition = base.Set<Position>();

                return dbPosition;
            }
        }
        #endregion
        
        #region BoostMaintenance
        DbSet<BoostMaintenance> dbBoostMaintenance;
        public DbSet<BoostMaintenance> BoostMaintenance
        {
            get
            {
                if (dbBoostMaintenance == null)
                    dbBoostMaintenance = base.Set<BoostMaintenance>();

                return dbBoostMaintenance;
            }
        }
        #endregion

        #region Fee
        DbSet<Fee> dbFee;
        public DbSet<Fee> Fee
        {
            get
            {
                if (dbFee == null)
                    dbFee = base.Set<Fee>();

                return dbFee;
            }
        }
        #endregion

        #region FeeType
        DbSet<FeeType> dbFeeType;
        public DbSet<FeeType> FeeType
        {
            get
            {
                if (dbFeeType == null)
                    dbFeeType = base.Set<FeeType>();

                return dbFeeType;
            }
        }
        #endregion

        #region BlockCode
        DbSet<BlockCode> dbBlockCode;
        public DbSet<BlockCode> BlockCode
        {
            get
            {
                if (dbBlockCode == null)
                    dbBlockCode = base.Set<BlockCode>();

                return dbBlockCode;
            }
        }
        #endregion

        #region CalendarDateMaintenance
        DbSet<CalendarDateMaintenance> dbCalendarDateMaintenance;
        public DbSet<CalendarDateMaintenance> CalendarDateMaintenance
        {
            get
            {
                if (dbCalendarDateMaintenance == null)
                    dbCalendarDateMaintenance = base.Set<CalendarDateMaintenance>();

                return dbCalendarDateMaintenance;
            }
        }
        #endregion

        #region Branch
        DbSet<Branch> dbBranch;
        public DbSet<Branch> Branch
        {
            get
            {
                if (dbBranch == null)
                    dbBranch = base.Set<Branch>();

                return dbBranch;
            }
        }
        #endregion

        #region StatementInterest

        DbSet<StatementInterest> dbStatementInterest;
        public DbSet<StatementInterest> StatementInterest
        {
            get
            {
                if (dbStatementInterest == null)
                    dbStatementInterest = base.Set<StatementInterest>();

                return dbStatementInterest;
            }
        }


        #endregion

        #region Customer
        DbSet<Customer> dbCustomer;
        public DbSet<Customer> Customer
        {
            get
            {
                if (dbCustomer == null)
                    dbCustomer = base.Set<Customer>();

                return dbCustomer;
            }
        }
        #endregion

        #region FinancialActivity
        DbSet<FinancialActivity> dbFinancialActivity;
        public DbSet<FinancialActivity> FinancialActivity
        {
            get
            {
                if (dbFinancialActivity == null)
                    dbFinancialActivity = base.Set<FinancialActivity>();

                return dbFinancialActivity;
            }
        }
        #endregion

        #region FinancialAccount
        DbSet<FinancialAccount> dbFinancialAccount;
        public DbSet<FinancialAccount> FinancialAccount
        {
            get
            {
                if (dbFinancialAccount == null)
                    dbFinancialAccount = base.Set<FinancialAccount>();

                return dbFinancialAccount;
            }
        }
        #endregion

        #region TransactionSubCode
        DbSet<TransactionSubCode> dbTransactionSubCode;
        public DbSet<TransactionSubCode> TransactionSubCode
        {
            get
            {
                if (dbTransactionSubCode == null)
                    dbTransactionSubCode = base.Set<TransactionSubCode>();

                return dbTransactionSubCode;
            }
        }
        #endregion

        #region BillingCycleParameter
        DbSet<BillingCycleParameter> dbBillingCycleParameter;
        public DbSet<BillingCycleParameter> BillingCycleParameter
        {
            get
            {
                if (dbBillingCycleParameter == null)
                    dbBillingCycleParameter = base.Set<BillingCycleParameter>();

                return dbBillingCycleParameter;
            }
        }
        #endregion

        #region SBIFInterest
        DbSet<SBIFInterest> dbSBIFInterest;
        public DbSet<SBIFInterest> SBIFInterest
        {
            get
            {
                if (dbSBIFInterest == null)
                    dbSBIFInterest = base.Set<SBIFInterest>();

                return dbSBIFInterest;
            }
        }
        #endregion

        #region InterestMaster
        DbSet<InterestMaster> dbInterestMaster;
        public DbSet<InterestMaster> InterestMaster
        {
            get
            {
                if (dbInterestMaster == null)
                    dbInterestMaster = base.Set<InterestMaster>();

                return dbInterestMaster;
            }
        }
        #endregion

        #region Commission
        DbSet<Commission> dbCommission;
        public DbSet<Commission> Commission
        {
            get
            {
                if (dbCommission == null)
                    dbCommission = base.Set<Commission>();

                return dbCommission;
            }
        }
        #endregion

        #region CardMaster
        DbSet<CardMaster> dbCardMaster;
        public DbSet<CardMaster> CardMaster
        {
            get 
            {
                if (dbCardMaster == null)
                    dbCardMaster = base.Set<CardMaster>();
                return dbCardMaster;
            }
        }
        #endregion

        #region CommissionCharge
        DbSet<CommissionCharge> dbCommissionCharge;
        public DbSet<CommissionCharge> CommissionCharge
        {
            get
            {
                if (dbCommissionCharge == null)
                    dbCommissionCharge = base.Set<CommissionCharge>();

                return dbCommissionCharge;
            }
        }
        #endregion
        
        #region BlockCodeForCommissionExclusion
        DbSet<BlockCodeForCommissionExclusion> dbBlockCodeForCommissionExclusion;
        public DbSet<BlockCodeForCommissionExclusion> BlockCodeForCommissionExclusion
        {
            get
            {
                if (dbBlockCodeForCommissionExclusion == null)
                    dbBlockCodeForCommissionExclusion = base.Set<BlockCodeForCommissionExclusion>();

                return dbBlockCodeForCommissionExclusion;
            }
        }
        #endregion

        #region DiscountOnMgmtCollection
        DbSet<DiscountOnMgmtCollection> dbDiscountOnMgmtCollection;
        public DbSet<DiscountOnMgmtCollection> DiscountOnMgmtCollection
        {
            get
            {
                if (dbDiscountOnMgmtCollection == null)
                    dbDiscountOnMgmtCollection = base.Set<DiscountOnMgmtCollection>();

                return dbDiscountOnMgmtCollection;
            }
        }
        #endregion

        #region CollectionCharge
        DbSet<CollectionCharge> dbCollectionCharge;
        public DbSet<CollectionCharge> CollectionCharge
        {
            get
            {
                if (dbCollectionCharge == null)
                    dbCollectionCharge = base.Set<CollectionCharge>();

                return dbCollectionCharge;
            }
        }
        #endregion

        #region InsuranceCharge
        DbSet<InsuranceCharge> dbInsuranceCharge;
        public DbSet<InsuranceCharge> InsuranceCharge
        {
            get
            {
                if (dbInsuranceCharge == null)
                    dbInsuranceCharge = base.Set<InsuranceCharge>();

                return dbInsuranceCharge;
            }
        }
        #endregion

        #region StatementGrouping
        DbSet<StatementGrouping> dbStatementGrouping;
        public DbSet<StatementGrouping> StatementGrouping
        {
            get
            {
                if (dbStatementGrouping == null)
                    dbStatementGrouping = base.Set<StatementGrouping>();

                return dbStatementGrouping;
            }
        }
        #endregion

        #region PaymentAllocationType
        DbSet<PaymentAllocationType> dbPaymentAllocationType;
        public DbSet<PaymentAllocationType> PaymentAllocationType
        {
            get
            {
                if (dbPaymentAllocationType == null)
                    dbPaymentAllocationType = base.Set<PaymentAllocationType>();

                return dbPaymentAllocationType;
            }
        }
        #endregion

        #region PlanMaster
        DbSet<PlanMaster> dbPlanMaster;
        public DbSet<PlanMaster> PlanMaster
        {
            get
            {
                if (dbPlanMaster == null)
                    dbPlanMaster = base.Set<PlanMaster>();

                return dbPlanMaster;
            }
        }
        #endregion

        #region CardCreditSegment
        DbSet<CardCreditSegment> dbCardCreditSegment;
        public DbSet<CardCreditSegment> CardCreditSegment
        {
            get
            {
                if (dbCardCreditSegment == null)
                    dbCardCreditSegment = base.Set<CardCreditSegment>();

                return dbCardCreditSegment;
            }
        }
        #endregion

        #region DiscountPercentage
        DbSet<DiscountPercentage> dbDiscountPercentage;
        public DbSet<DiscountPercentage> DiscountPercentage
        {
            get
            {
                if (dbDiscountPercentage == null)
                    dbDiscountPercentage = base.Set<DiscountPercentage>();

                return dbDiscountPercentage;
            }
        }
        #endregion

        #region PaymentHoliday
        DbSet<PaymentHoliday> dbPaymentHoliday;
        public DbSet<PaymentHoliday> PaymentHoliday
        {
            get
            {
                if (dbPaymentHoliday == null)
                    dbPaymentHoliday = base.Set<PaymentHoliday>();

                return dbPaymentHoliday;
            }
        }

        #endregion
        
        #region Lookup

        #region Contact
        DbSet<CT.Contact> dbContact;
        public DbSet<CT.Contact> Contact
        {
            get
            {
                if (dbContact == null)
                    dbContact = base.Set<CT.Contact>();

                return dbContact;
            }
        }
        #endregion
      
        #region AlertType
        DbSet<AlertType> dbAlertType;
        public DbSet<AlertType> AlertType
        {
            get
            {
                if (dbAlertType == null)
                    dbAlertType = base.Set<AlertType>();

                return dbAlertType;
            }
        }
        #endregion

        #region UserCulture
        DbSet<UserCulture> dbUserCulture;
        public DbSet<UserCulture> UserCulture
        {
            get
            {
                if (dbUserCulture == null)
                    dbUserCulture = base.Set<UserCulture>();

                return dbUserCulture;
            }
        }
        #endregion

        #region Address
        DbSet<Address> dbAddress;
        public DbSet<Address> Address
        {
            get
            {
                if (dbAddress == null)
                    dbAddress = base.Set<Address>();

                return dbAddress;
            }
        }
        #endregion

        #region ContactAddress
        DbSet<ContactAddress> dbContactAddress;
        public DbSet<ContactAddress> ContactAddress
        {
            get
            {
                if (dbContactAddress == null)
                    dbContactAddress = base.Set<ContactAddress>();

                return dbContactAddress;
            }
        }
        #endregion

        #region ApprovalContext
        DbSet<ApprovalContext> dbApprovalContext;
        public DbSet<ApprovalContext> ApprovalContext
        {
            get
            {
                if (dbApprovalContext == null)
                    dbApprovalContext = base.Set<ApprovalContext>();

                return dbApprovalContext;
            }
        }
        #endregion

        #region FirstBilling
        DbSet<FirstBilling> dbFirstBilling;
        public DbSet<FirstBilling> FirstBilling
        {
            get
            {
                if (dbFirstBilling == null)
                    dbFirstBilling = base.Set<FirstBilling>();

                return dbFirstBilling;
            }
        }
        #endregion
        
        #region InterestCalculation
        DbSet<InterestCalculation> dbInterestCalculation;
        public DbSet<InterestCalculation> InterestCalculation
        {
            get
            {
                if (dbInterestCalculation == null)
                    dbInterestCalculation = base.Set<InterestCalculation>();

                return dbInterestCalculation;
            }
        }
        #endregion

        #region ProductCategory
        DbSet<ProductCategory> dbProductCategory;
        public DbSet<ProductCategory> ProductCategory
        {
            get
            {
                if (dbProductCategory == null)
                    dbProductCategory = base.Set<ProductCategory>();

                return dbProductCategory;
            }
        }
        #endregion

        #region MemberType
        DbSet<MemberType> dbMemberType;
        public DbSet<MemberType> MemberType
        {
            get
            {
                if (dbMemberType == null)
                    dbMemberType = base.Set<MemberType>();

                return dbMemberType;
            }
        }
        #endregion

        #region BillingCycle
        DbSet<BillingCycle> dbBillingCycle;
        public DbSet<BillingCycle> BillingCycle
        {
            get
            {
                if (dbBillingCycle == null)
                    dbBillingCycle = base.Set<BillingCycle>();

                return dbBillingCycle;
            }
        }
        #endregion

        #region DefaultAffinity
        DbSet<DefaultAffinity> dbDefaultAffinity;
        public DbSet<DefaultAffinity> DefaultAffinity
        {
            get
            {
                if (dbDefaultAffinity == null)
                    dbDefaultAffinity = base.Set<DefaultAffinity>();

                return dbDefaultAffinity;
            }
        }
        #endregion

        #region InterestMethod
        DbSet<InterestMethod> dbInterestMethod;
        public DbSet<InterestMethod> InterestMethod
        {
            get
            {
                if (dbInterestMethod == null)
                    dbInterestMethod = base.Set<InterestMethod>();

                return dbInterestMethod;
            }
        }
        #endregion

        #region ProductClass
        DbSet<ProductClass> dbProductClass;
        public DbSet<ProductClass> ProductClass
        {
            get
            {
                if (dbProductClass == null)
                    dbProductClass = base.Set<ProductClass>();

                return dbProductClass;
            }
        }
        #endregion

        #region ProductMemberType
        DbSet<ProductMemberType> dbProductMemberType;
        public DbSet<ProductMemberType> ProductMemberType
        {
            get
            {
                if (dbProductMemberType == null)
                    dbProductMemberType = base.Set<ProductMemberType>();

                return dbProductMemberType;
            }
        }
        #endregion

        #region ProductStatus
        DbSet<ProductStatus> dbProductStatus;
        public DbSet<ProductStatus> ProductStatus
        {
            get
            {
                if (dbProductStatus == null)
                    dbProductStatus = base.Set<ProductStatus>();

                return dbProductStatus;
            }
        }
        #endregion

        #region ProductSubCategory
        DbSet<ProductSubCategory> dbProductSubCategory;
        public DbSet<ProductSubCategory> ProductSubCategory
        {
            get
            {
                if (dbProductSubCategory == null)
                    dbProductSubCategory = base.Set<ProductSubCategory>();

                return dbProductSubCategory;
            }
        }
        #endregion

        #region Service
        DbSet<Service> dbService;
        public DbSet<Service> Service
        {
            get
            {
                if (dbService == null)
                    dbService = base.Set<Service>();

                return dbService;
            }
        }
        #endregion

        #region Decision
        DbSet<Decision> dbDecision;
        public DbSet<Decision> Decision
        {
            get 
            {
                if (dbDecision == null)
                    dbDecision = base.Set<Decision>();

                return dbDecision;
            }
        }
        #endregion

        #region DecisionReason
        DbSet<DecisionReason> dbDecisionReason;
        public DbSet<DecisionReason> DecisionReason
        {
            get
            {
                if (dbDecisionReason == null)
                    dbDecisionReason = base.Set<DecisionReason>();

                return dbDecisionReason;
            }
        }
        #endregion

        #region Bank
        DbSet<Bank> dbBank;
        public DbSet<Bank> Bank
        {
            get
            {
                if (dbBank == null)
                    dbBank = base.Set<Bank>();

                return dbBank;
            }
        }
        #endregion

        #region CardType
        DbSet<CardType> dbCardType;
        public DbSet<CardType> CardType
        {
            get
            {
                if (dbCardType == null)
                    dbCardType = base.Set<CardType>();

                return dbCardType;
            }
        }
        #endregion

        #region ContractType
        DbSet<ContractType> dbContractType;
        public DbSet<ContractType> ContractType
        {
            get
            {
                if (dbContractType == null)
                    dbContractType = base.Set<ContractType>();

                return dbContractType;
            }
        }
        #endregion

        #region Network
        DbSet<Network> dbNetwork;
        public DbSet<Network> Network
        {
            get
            {
                if (dbNetwork == null)
                    dbNetwork = base.Set<Network>();

                return dbNetwork;
            }
        }
        #endregion

        #region FeeTypes
        DbSet<FeeTypes> dbFeeTypes;
        public DbSet<FeeTypes> FeeTypes
        {
            get
            {
                if (dbFeeTypes == null)
                    dbFeeTypes = base.Set<FeeTypes>();

                return dbFeeTypes;
            }
        }
        #endregion

        #region Education
        DbSet<Education> dbEducation;
        public DbSet<Education> Education
        {
            get
            {
                if (dbEducation == null)
                    dbEducation = base.Set<Education>();

                return dbEducation;
            }
        }
        #endregion

        #region Profession
        DbSet<Profession> dbProfession;
        public DbSet<Profession> Profession
        {
            get
            {
                if (dbProfession == null)
                    dbProfession = base.Set<Profession>();

                return dbProfession;
            }
        }
        #endregion

        #region MaritialStatus
        DbSet<MaritialStatus> dbMaritialStatus;
        public DbSet<MaritialStatus> MaritialStatus
        {
            get
            {
                if (dbMaritialStatus == null)
                    dbMaritialStatus = base.Set<MaritialStatus>();

                return dbMaritialStatus;
            }
        }
        #endregion

        #endregion

        #region User
        public DbSet<User> User { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }        
        public DbSet<UserEntity> UserEntity { get; set; }
        public DbSet<UserAllowedBranch> UserAllowedBranch { get; set; }
        public DbSet<UserGroupMembers> UserGroupMembers { get; set; }
        #endregion

        #region Authorization

        #region BillingCycleView
        DbSet<BillingCycleView> dbBillingCycleView;
        public DbSet<BillingCycleView> BillingCycleView
        {
            get
            {
                if (dbBillingCycleView == null)
                    dbBillingCycleView = base.Set<BillingCycleView>();

                return dbBillingCycleView;
            }
        }
        #endregion
        #region Adjustment
        DbSet<Adjustment> dbAdjustment;
        public DbSet<Adjustment> Adjustment
        {
            get
            {
                if (dbAdjustment == null)
                    dbAdjustment = base.Set<Adjustment>();

                return dbAdjustment;
            }
        }
        #endregion
        #region AuthCancellation
        public DbSet<AuthorizationCancellation> dbAuthorizationCancellation { get; set; }
        #endregion
        #region EMI
        DbSet<EMITransaction> dbEMITransaction;
        public DbSet<EMITransaction> EMITransaction
        {
            get
            {
                if (dbEMITransaction == null)
                    dbEMITransaction = base.Set<EMITransaction>();

                return dbEMITransaction;
            }
        }
        DbSet<EMIAuthorization> dbEMIAuthorization;
        public DbSet<EMIAuthorization> EMIAuthorization
        {
            get
            {
                if (dbEMIAuthorization == null)
                    dbEMIAuthorization = base.Set<EMIAuthorization>();

                return dbEMIAuthorization;
            }
        }
        DbSet<EMITransactionInstallment> dbEMITransactionInstallment;
        public DbSet<EMITransactionInstallment> EMITransactionInstallment
        {
            get
            {
                if (dbEMITransactionInstallment == null)
                    dbEMITransactionInstallment = base.Set<EMITransactionInstallment>();

                return dbEMITransactionInstallment;
            }
        }
        #endregion

        #endregion

        #region CustomerService

        #region NotepadEntry
        public DbSet<NotepadEntry> NotepadEntry { get; set; }
        #endregion

        #region UpdatedCustomerAddress
        public DbSet<UpdatedCustomerAddress> UpdatedCustomerAddress { get; set; }
        #endregion

        #region AlertMessage
        public DbSet<AlertMessage> AlertMessage { get; set; }
        #endregion

        #region Block Card Or Segment

        public DbSet<BlockSegmentCard> BlockSegmentCard { get; set; }
        #endregion

        #region ClosureReason
        DbSet<ClosureReason> dbClosureReason;
        public DbSet<ClosureReason> ClosureReason
        {
            get
            {
                if (dbClosureReason == null)
                    dbClosureReason = base.Set<ClosureReason>();

                return dbClosureReason;
            }
        }
        #endregion

        #region CardReactivationReason
        DbSet<CardReactivateReason> dbReactivationReason;
        public DbSet<CardReactivateReason> CardReactivationReason
        {
            get
            {
                if (dbReactivationReason == null)
                    dbReactivationReason = base.Set<CardReactivateReason>();

                return dbReactivationReason;
            }
        }
        #endregion

        #region CardClosureHistory
        DbSet<CardClosureHistory> dbCardClosureHistory;
        public DbSet<CardClosureHistory> CardClosureHistory
        {
            get
            {
                if (dbCardClosureHistory == null)
                    dbCardClosureHistory = base.Set<CardClosureHistory>();

                return dbCardClosureHistory;
            }
        }
        #endregion

        #region CardReactivation
        DbSet<CardReactivation> dbCardReactivation;
        public DbSet<CardReactivation> CardReactivation
        {
            get
            {
                if (dbCardReactivation == null)
                    dbCardReactivation = base.Set<CardReactivation>();

                return dbCardReactivation;
            }
        }
        #endregion

        #region CardClosureInitiation
        public DbSet<CardClosure> CardClosure { get; set; }
        #endregion

        #region LostStolenCardBlock
        DbSet<LostStolenCardBlock> dbLostStolenCardBlock;
        public DbSet<LostStolenCardBlock> LostStolenCardBlock
        {
            get
            {
                if (dbLostStolenCardBlock == null)
                    dbLostStolenCardBlock = base.Set<LostStolenCardBlock>();

                return dbLostStolenCardBlock;
            }
        }
        #endregion

        #region LostStolenReissueRequest
        DbSet<LostStolenReissueRequest> dbLostStolenReissueRequest;
        public DbSet<LostStolenReissueRequest> LostStolenReissueRequest
        {
            get
            {
                if (dbLostStolenReissueRequest == null)
                    dbLostStolenReissueRequest = base.Set<LostStolenReissueRequest>();

                return dbLostStolenReissueRequest;
            }
        }
        #endregion

        #region ChangeCreditLimit
        DbSet<ChangeCreditLimit> dbChangeCreditLimit;
        public DbSet<ChangeCreditLimit> ChangeCreditLimit
        {
            get
            {
                if (dbChangeCreditLimit == null)
                    dbChangeCreditLimit = base.Set<ChangeCreditLimit>();

                return dbChangeCreditLimit;
            }
        }
        #endregion

        #region SwapProduct
        DbSet<SwapProduct> dbSwapProduct;
        public DbSet<SwapProduct> SwapProduct
        {
            get
            {
                if (dbSwapProduct == null)
                    dbSwapProduct = base.Set<SwapProduct>();

                return dbSwapProduct;
            }
        }
        #endregion

        #endregion

        #region OnlinePayment
        DbSet<OnlinePayment> dbOnlinePayment;
        public DbSet<OnlinePayment> OnlinePayment
        {
            get
            {
                if (dbOnlinePayment == null)
                    dbOnlinePayment = base.Set<OnlinePayment>();

                return dbOnlinePayment;
            }
        }
        #endregion

        #region Refinancing
        DbSet<Refinancing> dbRefinancing;
        public DbSet<Refinancing> Refinancing
        {
            get
            {
                if (dbRefinancing == null)
                    dbRefinancing = base.Set<Refinancing>();

                return dbRefinancing;
            }
        }
        #endregion

        #region Renegotiation
        DbSet<Renegotiation> dbRenegotiation;
        public DbSet<Renegotiation> Renegotiation
        {
            get
            {
                if (dbRenegotiation == null)
                    dbRenegotiation = base.Set<Renegotiation>();

                return dbRenegotiation;
            }
        }
        #endregion

        #region UserGroupPrivilegeStandard
        DbSet<UserGroupPrivilege> dbUsergroupPrivilege;
        public DbSet<UserGroupPrivilege> UsergroupPrivilege
        {
            get
            {
                if (dbUsergroupPrivilege == null)
                    dbUsergroupPrivilege = base.Set<UserGroupPrivilege>();

                return dbUsergroupPrivilege;
            }
        }
        #endregion

        #region Statement Generation
        DbSet<StatementGeneration> dbStatementGeneration;
        public DbSet<StatementGeneration> StatementGeneration
        {
            get
            {
                if (dbStatementGeneration == null)
                    dbStatementGeneration = base.Set<StatementGeneration>();

                return dbStatementGeneration;
            }
        }
        #endregion

        #region ApprovalQueueContext
        DbSet<ApprovalQueueContext> dbApprovalQueueContext;
        public DbSet<ApprovalQueueContext> ApprovalQueueContext
        {
            get
            {
                if (dbApprovalQueueContext == null)
                    dbApprovalQueueContext = base.Set<ApprovalQueueContext>();

                return dbApprovalQueueContext;
            }
        }
        #endregion
        
        #region ChangeBusinessDate
        DbSet<ChangeBusinessDate> dbChangeBusinessDate;
        public DbSet<ChangeBusinessDate> ChangeBusinessDate
        {
            get
            {
                if (dbChangeBusinessDate == null)
                    dbChangeBusinessDate = base.Set<ChangeBusinessDate>();

                return dbChangeBusinessDate;
            }
        }
        #endregion

        #region DirectSalesAgent
        DbSet<DirectSalesAgent> dbDirectSalesAgent;
        public DbSet<DirectSalesAgent> DirectSalesAgent
        {
            get
            {
                if (dbDirectSalesAgent == null)
                    dbDirectSalesAgent = base.Set<DirectSalesAgent>();

                return dbDirectSalesAgent;
            }
        }
        #endregion

        #region UFRateSettings
        DbSet<UFRateSettings> dbUFRateSettings;
        public DbSet<UFRateSettings> UFRateSettings
        {
            get
            {
                if (dbUFRateSettings == null)
                    dbUFRateSettings = base.Set<UFRateSettings>();

                return dbUFRateSettings;
            }
        }
        #endregion

        #region SaleId
        DbSet<SaleId> dbSaleId;
        public DbSet<SaleId> SaleId
        {
            get
            {
                if (dbSaleId == null)
                    dbSaleId = base.Set<SaleId>();

                return dbSaleId;
            }
        }
        #endregion

        #region TransactionCheckFrequency
        DbSet<TransactionCheckFrequency> dbTransactionCheckFrequency;
        public DbSet<TransactionCheckFrequency> TransactionCheckFrequency
        {
            get
            {
                if (dbTransactionCheckFrequency == null)
                    dbTransactionCheckFrequency = base.Set<TransactionCheckFrequency>();

                return dbTransactionCheckFrequency;
            }
        }
        #endregion

        #region MerchantDiscount
        DbSet<MerchantDiscount> dbMerchantDiscount;
        public DbSet<MerchantDiscount> MerchantDiscount
        {
            get
            {
                if (dbMerchantDiscount == null)
                    dbMerchantDiscount = base.Set<MerchantDiscount>();

                return dbMerchantDiscount;
            }
        }
        #endregion

        #region AdjustmentFinancialAccount
        DbSet<AdjustmentFinancialAccount> dbAdjustmentFinancialAccount;
        public DbSet<AdjustmentFinancialAccount> AdjustmentFinancialAccount
        {
            get
            {
                if (dbAdjustmentFinancialAccount == null)
                    dbAdjustmentFinancialAccount = base.Set<AdjustmentFinancialAccount>();

                return dbAdjustmentFinancialAccount;
            }
        }
        #endregion

        #endregion

        #region IQueryableUnitOfWork
        public DbSet<TEntity> CreateSet<TEntity>()
               where TEntity : class
        {
            return base.Set<TEntity>();
        }


        public void Attach<T>(T item)
          where T : class
        {
            //attach and set as unchanged
            base.Entry<T>(item).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
        }

        public void SetModified<T>(T item)
            where T : class
        {
            //this operation also attach item in object state manager
            base.Entry<T>(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        public void ApplyCurrentValues<T>(T original, T current)
            where T : class
        {
            //if it is not attached, attach original and set current values
            
            base.Entry<T>(original).CurrentValues.SetValues(current);
        }

        public void Commit()
        {
            try
            {
                base.SaveChanges();
            }
            catch(Exception ex)
            {
                RollbackChanges();
                throw ex;
            }
            
        }

        public void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    base.SaveChanges();

                    saveFailed = false;

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                              .ForEach(entry =>
                              {
                                  entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                              });

                }
            } while (saveFailed);

        }

        public void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            base.ChangeTracker.Entries()
                              .ToList()
                              .ForEach(entry => entry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters) where TEntity : class
        {
            // EF5's Database.SqlQuery<T> returned untracked results for any T.
            // EF Core: mapped entity types go through FromSqlRaw (AsNoTracking to
            // match), anything else (DTOs/scalars) through SqlQueryRaw<T>.
            if (Model.FindEntityType(typeof(TEntity)) != null)
            {
                return base.Set<TEntity>().FromSqlRaw(sqlQuery, parameters).AsNoTracking();
            }

            return base.Database.SqlQueryRaw<TEntity>(sqlQuery, parameters);
        }

        public IEnumerable ExecuteSqlQuery(Type elementType, string sqlQuery, params object[] parameters)
        {
            // EF Core has no non-generic SqlQuery(Type, ...), so dispatch to the
            // generic overload above by reflection.
            var method = typeof(UnitOfWork)
                .GetMethod(nameof(ExecuteQuery))
                .MakeGenericMethod(elementType);
            return (IEnumerable)method.Invoke(this, new object[] { sqlQuery, parameters });
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return base.Database.ExecuteSqlRaw(sqlCommand, parameters);
        }

        public void RefreshDataInModel<T>(ref T item) where T : class
        {
            base.Entry(item).Reload();
        }

        public void ReloadAllDataInContext<T>() where T : class
        {
           base.ChangeTracker.Entries<T>().ToList().ForEach(x => x.Reload());
           // base.dd
        }

        #endregion
        
        #region Overidden Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // EF5 had PluralizingTableNameConvention removed here (table name =
            // CLR type name); LegacyEf5ModelConventions reproduces that, plus the
            // composite-key/composite-FK handling EF Core doesn't do from
            // data annotations.
            LegacyEf5ModelConventions.Apply(modelBuilder);

            modelBuilder.Entity<InsuranceCharge>().Property(q => q.InsuranceAmount).HasPrecision(15, 6);
            modelBuilder.Entity<SBIFInterestDetails>().Property(q => q.CreditLimitFrom).HasPrecision(15, 6);
            modelBuilder.Entity<SBIFInterestDetails>().Property(q => q.CreditLimitTo).HasPrecision(15, 6);
            modelBuilder.Entity<InterestMasterDetails>().Property(q => q.CreditLimitFrom).HasPrecision(15, 6);
            modelBuilder.Entity<InterestMasterDetails>().Property(q => q.CreditLimitTo).HasPrecision(15, 6);
            modelBuilder.Entity<CollectionChargeDetails>().Property(q => q.DelinquencyAmountFrom).HasPrecision(15, 6);
            modelBuilder.Entity<CollectionChargeDetails>().Property(q => q.DelinquencyAmountTo).HasPrecision(15, 6);
            modelBuilder.Entity<CommissionChargeDetails>().Property(q => q.CommissionAmount).HasPrecision(15, 6);
            modelBuilder.Entity<DiscountOnMgmtCollectionDetails>().Property(q => q.ConsumptionRangeFrom).HasPrecision(15, 6);
            modelBuilder.Entity<DiscountOnMgmtCollectionDetails>().Property(q => q.ConsumptionRangeTo).HasPrecision(15, 6);
            modelBuilder.Entity<UFRateSettings>().Property(q => q.UFRate).HasPrecision(15, 6);
        }
        #endregion
        
        #region BuildConnectionString
        private static string BuildConnectionString()
        {
           return  CC_Ingenio.Common.Utils.Utils.BuildConnectionString();
        }
        #endregion

        #endregion

    }

}
