using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using CT=CC_Ingenio.Core.ContactRepo.ContactAggregate;
using CC_Ingenio.Core.ContactRepo.AddressAggregate;
using CC_Ingenio.Core.ContactRepo.ContactAddressAggregate;

using CC_Ingenio.Core.MasterRepo.MasterAggregate;

using CC_Ingenio.Data.Contracts;
using CC_Ingenio.Data.EntityMapping;
using System.Data.Entity.ModelConfiguration.Conventions;
using CC_Ingenio.Data.EntityMapping.Master;
using CC_Ingenio.Core.LookUpRepo;
using CC_Ingenio.Core.ApprovalRepo.ApprovalAggregate;

using CC_Ingenio.Core.UserRepo.UserAggregate;
using CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate;
using System.Configuration;
using GemCryptor;
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
            : base(BuildConnectionString())
        {
            this.Configuration.ProxyCreationEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 1500;
        }
        #endregion

        #region IDbSet members
        
        #region Masters

        #region Currency
        IDbSet<Currency> dbCurrency;
        public IDbSet<Currency> Currency
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
        IDbSet<Country> dbCountry;
        public IDbSet<Country> Country
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
        IDbSet<SystemMaintenanceConfiguration> dbSystemMaintenanceConfiguration;
        public IDbSet<SystemMaintenanceConfiguration> SystemMaintenanceConfiguration
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
        IDbSet<Sourcing> dbSourcing;
        public IDbSet<Sourcing> Sourcing
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
        IDbSet<Merchant> dbMerchant;
        public IDbSet<Merchant> Merchant
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
        IDbSet<MCC > dbMCC;
        public IDbSet<MCC> MCC
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
        IDbSet<CardEmbossing> dbCardEmbossing;
        public IDbSet<CardEmbossing> CardEmbossing
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
        IDbSet<ForceAuthorization> dbForceAuthorization;
        public IDbSet<ForceAuthorization> ForceAuthorization
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
        IDbSet<Company> dbCompany;
        public IDbSet<Company> Company
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
        IDbSet<BillingParameter> dbBillingParameter;
        public IDbSet<BillingParameter> BillingParameter
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
        IDbSet<ManualTransaction> dbManualTransaction;
        public IDbSet<ManualTransaction > ManualTransaction
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
        IDbSet<Organization> dbOrganization;
        public IDbSet<Organization> Organization
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
        IDbSet<Franchise> dbFranchise;
        public IDbSet<Franchise> Franchise
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
        IDbSet<City> dbCity;
        public IDbSet<City> City
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
        IDbSet<ApprovalQueue> dbApprovalQueue;
        public IDbSet<ApprovalQueue> ApprovalQueue
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
        IDbSet<CityCategory> dbCityCategory;
        public IDbSet<CityCategory> CityCategory
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
        IDbSet<Region> dbRegion;
        public IDbSet<Region> Region
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
        IDbSet<Hub> dbHub;
        public IDbSet<Hub> Hub
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
        IDbSet<Spoke> dbSpoke;
        public IDbSet<Spoke> Spoke
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
        IDbSet<Communa> dbCommuna;
        public IDbSet<Communa> Communa
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
        IDbSet<CompanyCategory> dbCompanyCategory;
        public IDbSet<CompanyCategory> CompanyCategory
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
        IDbSet<CreditSegment> dbCreditSegment;
        public IDbSet<CreditSegment> CreditSegment
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
        IDbSet<CreditSubSegment> dbCreditSubSegment;
        public IDbSet<CreditSubSegment> CreditSubSegment
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
        IDbSet<Designation> dbDesignation;
        public IDbSet<Designation> Designation
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
        IDbSet<LegalEntity> dbLegalEntity;
        public IDbSet<LegalEntity> LegalEntity
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
        IDbSet<CustomerSegment> dbCustomerSegment;
        public IDbSet<CustomerSegment> CustomerSegment
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
        IDbSet<CustomerSubSegment> dbCustomerSubSegment;
        public IDbSet<CustomerSubSegment> CustomerSubSegment
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
        IDbSet<SystemParameter> dbSystemParameter;
        public IDbSet<SystemParameter> SystemParameter
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
        IDbSet<CardStatus> dbCardStatus;
        public IDbSet<CardStatus> CardStatus
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
        IDbSet<Contract> dbContract;
        public IDbSet<Contract> Contract
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
        IDbSet<General> dbGeneral;
        public IDbSet<General> General
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
        IDbSet<ContractTemplate> dbContractTemplate;
        public IDbSet<ContractTemplate> ContractTemplate
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
        IDbSet<Product> dbProduct;
        public IDbSet<Product> Product
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
        IDbSet<ProductType> dbProductType;
        public IDbSet<ProductType> ProductType
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
        IDbSet<Transaction> dbTransaction;
        public IDbSet<Transaction> Transaction
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
        IDbSet<Position> dbPosition;
        public IDbSet<Position> Position
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
        IDbSet<BoostMaintenance> dbBoostMaintenance;
        public IDbSet<BoostMaintenance> BoostMaintenance
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
        IDbSet<Fee> dbFee;
        public IDbSet<Fee> Fee
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
        IDbSet<FeeType> dbFeeType;
        public IDbSet<FeeType> FeeType
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
        IDbSet<BlockCode> dbBlockCode;
        public IDbSet<BlockCode> BlockCode
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
        IDbSet<CalendarDateMaintenance> dbCalendarDateMaintenance;
        public IDbSet<CalendarDateMaintenance> CalendarDateMaintenance
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
        IDbSet<Branch> dbBranch;
        public IDbSet<Branch> Branch
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

        IDbSet<StatementInterest> dbStatementInterest;
        public IDbSet<StatementInterest> StatementInterest
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
        IDbSet<Customer> dbCustomer;
        public IDbSet<Customer> Customer
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
        IDbSet<FinancialActivity> dbFinancialActivity;
        public IDbSet<FinancialActivity> FinancialActivity
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
        IDbSet<FinancialAccount> dbFinancialAccount;
        public IDbSet<FinancialAccount> FinancialAccount
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
        IDbSet<TransactionSubCode> dbTransactionSubCode;
        public IDbSet<TransactionSubCode> TransactionSubCode
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
        IDbSet<BillingCycleParameter> dbBillingCycleParameter;
        public IDbSet<BillingCycleParameter> BillingCycleParameter
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
        IDbSet<SBIFInterest> dbSBIFInterest;
        public IDbSet<SBIFInterest> SBIFInterest
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
        IDbSet<InterestMaster> dbInterestMaster;
        public IDbSet<InterestMaster> InterestMaster
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
        IDbSet<Commission> dbCommission;
        public IDbSet<Commission> Commission
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
        IDbSet<CardMaster> dbCardMaster;
        public IDbSet<CardMaster> CardMaster
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
        IDbSet<CommissionCharge> dbCommissionCharge;
        public IDbSet<CommissionCharge> CommissionCharge
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
        IDbSet<BlockCodeForCommissionExclusion> dbBlockCodeForCommissionExclusion;
        public IDbSet<BlockCodeForCommissionExclusion> BlockCodeForCommissionExclusion
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
        IDbSet<DiscountOnMgmtCollection> dbDiscountOnMgmtCollection;
        public IDbSet<DiscountOnMgmtCollection> DiscountOnMgmtCollection
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
        IDbSet<CollectionCharge> dbCollectionCharge;
        public IDbSet<CollectionCharge> CollectionCharge
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
        IDbSet<InsuranceCharge> dbInsuranceCharge;
        public IDbSet<InsuranceCharge> InsuranceCharge
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
        IDbSet<StatementGrouping> dbStatementGrouping;
        public IDbSet<StatementGrouping> StatementGrouping
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
        IDbSet<PaymentAllocationType> dbPaymentAllocationType;
        public IDbSet<PaymentAllocationType> PaymentAllocationType
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
        IDbSet<PlanMaster> dbPlanMaster;
        public IDbSet<PlanMaster> PlanMaster
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
        IDbSet<CardCreditSegment> dbCardCreditSegment;
        public IDbSet<CardCreditSegment> CardCreditSegment
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
        IDbSet<DiscountPercentage> dbDiscountPercentage;
        public IDbSet<DiscountPercentage> DiscountPercentage
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
        IDbSet<PaymentHoliday> dbPaymentHoliday;
        public IDbSet<PaymentHoliday> PaymentHoliday
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
        IDbSet<CT.Contact> dbContact;
        public IDbSet<CT.Contact> Contact
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
        IDbSet<AlertType> dbAlertType;
        public IDbSet<AlertType> AlertType
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
        IDbSet<UserCulture> dbUserCulture;
        public IDbSet<UserCulture> UserCulture
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
        IDbSet<Address> dbAddress;
        public IDbSet<Address> Address
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
        IDbSet<ContactAddress> dbContactAddress;
        public IDbSet<ContactAddress> ContactAddress
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
        IDbSet<ApprovalContext> dbApprovalContext;
        public IDbSet<ApprovalContext> ApprovalContext
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
        IDbSet<FirstBilling> dbFirstBilling;
        public IDbSet<FirstBilling> FirstBilling
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
        IDbSet<InterestCalculation> dbInterestCalculation;
        public IDbSet<InterestCalculation> InterestCalculation
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
        IDbSet<ProductCategory> dbProductCategory;
        public IDbSet<ProductCategory> ProductCategory
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
        IDbSet<MemberType> dbMemberType;
        public IDbSet<MemberType> MemberType
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
        IDbSet<BillingCycle> dbBillingCycle;
        public IDbSet<BillingCycle> BillingCycle
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
        IDbSet<DefaultAffinity> dbDefaultAffinity;
        public IDbSet<DefaultAffinity> DefaultAffinity
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
        IDbSet<InterestMethod> dbInterestMethod;
        public IDbSet<InterestMethod> InterestMethod
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
        IDbSet<ProductClass> dbProductClass;
        public IDbSet<ProductClass> ProductClass
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
        IDbSet<ProductMemberType> dbProductMemberType;
        public IDbSet<ProductMemberType> ProductMemberType
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
        IDbSet<ProductStatus> dbProductStatus;
        public IDbSet<ProductStatus> ProductStatus
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
        IDbSet<ProductSubCategory> dbProductSubCategory;
        public IDbSet<ProductSubCategory> ProductSubCategory
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
        IDbSet<Service> dbService;
        public IDbSet<Service> Service
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
        IDbSet<Decision> dbDecision;
        public IDbSet<Decision> Decision
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
        IDbSet<DecisionReason> dbDecisionReason;
        public IDbSet<DecisionReason> DecisionReason
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
        IDbSet<Bank> dbBank;
        public IDbSet<Bank> Bank
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
        IDbSet<CardType> dbCardType;
        public IDbSet<CardType> CardType
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
        IDbSet<ContractType> dbContractType;
        public IDbSet<ContractType> ContractType
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
        IDbSet<Network> dbNetwork;
        public IDbSet<Network> Network
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
        IDbSet<FeeTypes> dbFeeTypes;
        public IDbSet<FeeTypes> FeeTypes
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
        IDbSet<Education> dbEducation;
        public IDbSet<Education> Education
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
        IDbSet<Profession> dbProfession;
        public IDbSet<Profession> Profession
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
        IDbSet<MaritialStatus> dbMaritialStatus;
        public IDbSet<MaritialStatus> MaritialStatus
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
        public IDbSet<User> User { get; set; }
        public IDbSet<UserGroup> UserGroup { get; set; }        
        public IDbSet<UserEntity> UserEntity { get; set; }
        public IDbSet<UserAllowedBranch> UserAllowedBranch { get; set; }
        public IDbSet<UserGroupMembers> UserGroupMembers { get; set; }
        #endregion

        #region Authorization

        #region BillingCycleView
        IDbSet<BillingCycleView> dbBillingCycleView;
        public IDbSet<BillingCycleView> BillingCycleView
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
        IDbSet<Adjustment> dbAdjustment;
        public IDbSet<Adjustment> Adjustment
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
        IDbSet<EMITransaction> dbEMITransaction;
        public IDbSet<EMITransaction> EMITransaction
        {
            get
            {
                if (dbEMITransaction == null)
                    dbEMITransaction = base.Set<EMITransaction>();

                return dbEMITransaction;
            }
        }
        IDbSet<EMIAuthorization> dbEMIAuthorization;
        public IDbSet<EMIAuthorization> EMIAuthorization
        {
            get
            {
                if (dbEMIAuthorization == null)
                    dbEMIAuthorization = base.Set<EMIAuthorization>();

                return dbEMIAuthorization;
            }
        }
        IDbSet<EMITransactionInstallment> dbEMITransactionInstallment;
        public IDbSet<EMITransactionInstallment> EMITransactionInstallment
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
        public IDbSet<NotepadEntry> NotepadEntry { get; set; }
        #endregion

        #region UpdatedCustomerAddress
        public IDbSet<UpdatedCustomerAddress> UpdatedCustomerAddress { get; set; }
        #endregion

        #region AlertMessage
        public IDbSet<AlertMessage> AlertMessage { get; set; }
        #endregion

        #region Block Card Or Segment

        public IDbSet<BlockSegmentCard> BlockSegmentCard { get; set; }
        #endregion

        #region ClosureReason
        IDbSet<ClosureReason> dbClosureReason;
        public IDbSet<ClosureReason> ClosureReason
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
        IDbSet<CardReactivateReason> dbReactivationReason;
        public IDbSet<CardReactivateReason> CardReactivationReason
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
        IDbSet<CardClosureHistory> dbCardClosureHistory;
        public IDbSet<CardClosureHistory> CardClosureHistory
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
        IDbSet<CardReactivation> dbCardReactivation;
        public IDbSet<CardReactivation> CardReactivation
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
        public IDbSet<CardClosure> CardClosure { get; set; }
        #endregion

        #region LostStolenCardBlock
        IDbSet<LostStolenCardBlock> dbLostStolenCardBlock;
        public IDbSet<LostStolenCardBlock> LostStolenCardBlock
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
        IDbSet<LostStolenReissueRequest> dbLostStolenReissueRequest;
        public IDbSet<LostStolenReissueRequest> LostStolenReissueRequest
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
        IDbSet<ChangeCreditLimit> dbChangeCreditLimit;
        public IDbSet<ChangeCreditLimit> ChangeCreditLimit
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
        IDbSet<SwapProduct> dbSwapProduct;
        public IDbSet<SwapProduct> SwapProduct
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
        IDbSet<OnlinePayment> dbOnlinePayment;
        public IDbSet<OnlinePayment> OnlinePayment
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
        IDbSet<Refinancing> dbRefinancing;
        public IDbSet<Refinancing> Refinancing
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
        IDbSet<Renegotiation> dbRenegotiation;
        public IDbSet<Renegotiation> Renegotiation
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
        IDbSet<UserGroupPrivilege> dbUsergroupPrivilege;
        public IDbSet<UserGroupPrivilege> UsergroupPrivilege
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
        IDbSet<StatementGeneration> dbStatementGeneration;
        public IDbSet<StatementGeneration> StatementGeneration
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
        IDbSet<ApprovalQueueContext> dbApprovalQueueContext;
        public IDbSet<ApprovalQueueContext> ApprovalQueueContext
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
        IDbSet<ChangeBusinessDate> dbChangeBusinessDate;
        public IDbSet<ChangeBusinessDate> ChangeBusinessDate
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
        IDbSet<DirectSalesAgent> dbDirectSalesAgent;
        public IDbSet<DirectSalesAgent> DirectSalesAgent
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
        IDbSet<UFRateSettings> dbUFRateSettings;
        public IDbSet<UFRateSettings> UFRateSettings
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
        IDbSet<SaleId> dbSaleId;
        public IDbSet<SaleId> SaleId
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
        IDbSet<TransactionCheckFrequency> dbTransactionCheckFrequency;
        public IDbSet<TransactionCheckFrequency> TransactionCheckFrequency
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
        IDbSet<MerchantDiscount> dbMerchantDiscount;
        public IDbSet<MerchantDiscount> MerchantDiscount
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
        IDbSet<AdjustmentFinancialAccount> dbAdjustmentFinancialAccount;
        public IDbSet<AdjustmentFinancialAccount> AdjustmentFinancialAccount
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
            base.Entry<T>(item).State = System.Data.EntityState.Unchanged;
        }

        public void SetModified<T>(T item)
            where T : class
        {
            //this operation also attach item in object state manager
            base.Entry<T>(item).State = System.Data.EntityState.Modified;
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
                              .ForEach(entry => entry.State = System.Data.EntityState.Unchanged);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters) where TEntity : class
        {
            return base.Database.SqlQuery<TEntity>(sqlQuery, parameters);            
        }

        public IEnumerable ExecuteSqlQuery(Type elementType, string sqlQuery, params object[] parameters)
        {
            return base.Database.SqlQuery(elementType, sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return base.Database.ExecuteSqlCommand(sqlCommand, parameters);
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
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            /*modelBuilder.Configurations.Add(new AddressMapping());
            modelBuilder.Configurations.Add(new ContactAddressMapping());
            modelBuilder.Configurations.Add(new ContactMapping());

           modelBuilder.Configurations.Add(new CurrencyMapping());*/

            /*modelBuilder.Entity<ApprovalQueue>()
                .HasRequired(x => x.ApprovalContext)
                .WithMany()
                .HasForeignKey(x => x.ContextCode);*/

            //modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            //modelBuilder.Conventions.Add(new DecimalPropertyConvention(15, 6));

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
