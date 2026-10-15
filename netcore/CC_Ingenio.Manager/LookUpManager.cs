using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CC_Ingenio.Core.LookUpRepo;
using CC_Ingenio.DataTranfer.LookUpInfo;
using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.Repository.LookUpRepo;
using System.Web.Mvc;
using CC_Ingenio.Repository.MasterRepo;
using CC_Ingenio.Repository.UserRepo;
using CC_Ingenio.Common.Utils ;

using CC_Ingenio.Core.UserRepo;
using CC_Ingenio.DataTranfer.UserInfo;
using CC_Ingenio_Resources;

namespace CC_Ingenio.Manager
{
    public class LookUpManager : ILookUpManager
    {

        #region Sub Class
		public class ListItem
        {
            public string Code { get; set; }
            public string  Description { get; set; }
        }
 
	    #endregion

        #region Variables
        CountryRepository objLookUpRepository;
        OrganizationRepository objOrganizationLookUpRepository;
        RegionRepository objRegionLookUpRepository;
        CityCategoryRepository objCityCategoryLookUpRepository;
        HubRepository objHubLookUpRepository;
        SpokeRepository objSpokeLookUpRepository;
        CommunaRepository objCommunaLookUpRepository;
        CompanyCategoryRepository objCompanyCategoryRepository;
        LegalEntityRepository objLegalEntityRepository;
        ContractTemplateRepository objContractTemplateRepository;
        ApprovalContextRepository objApprovalContextRepository;
        ProductTypeRepository objProductTypeRepository;
        TransactionRepository objTransactionRepository;
        TransactionSubCodeRepository objTransactionSubCodeRepository;
        ProductRepository objProductRepository;
        ProductCategoryRepository objProductCategoryRepository;
        CustomerSegmentRepository objCustomerSegmentRepository;
        CustomerSubSegmentRepository objCustomerSubSegmentRepository;
        ContractRepository objContractRepository;
        MemberTypeRepository objMemberTypeRepository;
        SystemParameterRepository objSystemParameterRepository;

        DefaultAffinityRepository objDefaultAffinityRepository;
        BillingCycleRepository objBillingCycleRepository;
        InterestMethodRepository objInterestMethodRepository;
        ServiceRepository objServiceRepository;
        ProductStatusRepository objProductStatusRepository;
        ProductMemberTypeRepository objProductMemberTypeRepository;
        ProductClassRepository objProductClassRepository;
        ProductSubCategoryRepository objProductSubCategoryRepository;

        CurrencyRepository objCurrencyRepository;
        DecisionRepository objDecisionRepository;
        DecisionReasonRepository objDecisionReasonRepository;
        BankRepository objBankRepository;
        BranchRepository objBranchRepository;
        PositionRepository objPositionRepository;

        DesignationRepository objDesignationRepository;
        CardTypeRepository objCardTypeRepository;
        CardStatusRepository objCardStatusRepository;
        CityRepository objCityRepository;
        FinancialActivityRepository objFinancialActivityRepository;
        ContractTypeRepository objContractTypeRepository;
        CommissionRepository objCommissionRepository;
        BlockCodeRepository objBlockCodeRepository;
        NetworkRepository objNetworkRepository;
        UserAllowedBranchRepository objUserAllowedBranchRepository;

        MerchantsRepository objMerchantsRepository;
        MCCRepository objMCCRepository;
        CreditSegmentRepository objCreditSegmentRepository;
        //CreditSubSegmentRepository objCreditSubSegmentRepository;
        PlanMasterRepository objPlanMasterRepository;
        FranchiseRepository objFranchiseRepository;
        FirstBillingRepository objFirstBillingRepository;
        InterestCalculationRepository objInterestCalculationRepository;
        SourcingRepository objSourcingRepository; 
        FeeTypesRepository objFeeTypesRepository;

        EducationRepository objEducationRepository;
        ProfessionRepository objProfessionRepository;
        MaritialStatusRepository objMaritialStatusRepository;
        ClosureReasonRepository objClosureReasonRepository;
        CardReactivationReasonRepository objCardReactivationReasonRepository;
        AlertTypeRepository objAlertTypeRepository;
        DirectSalesAgentRepository objDirectSalesAgentRepository;
        SalesIdRepository objSalesIdRepository;
        TransactionCheckFrequencyRepository objTransactionCheckFrequencyRepository;
        //user

        UserRepository objUserRepository;
        UserGroupRepository objUserGroupRepository;
        UserGroupMembersRepository objUserGroupMembersRepository;
        UserGroupPrivilegeRepository objUserGroupPrivilegeRepository;

        ApprovalQueueContextRepository objApprovalQueueContextRepository;
        UserEntityRepository objUserEntityRepository;
        UserCultureRepository objUserCultureRepository;


        List<ListItem> objListItem;
        ILoginSession objLoginSession;
        #endregion

        #region Constructor
        public LookUpManager(CountryRepository ALookUpRepository, OrganizationRepository AOrganizationRepository,
            RegionRepository ARegionRepository, CityCategoryRepository ACityCategoryRepository, HubRepository AHubRepository,
            SpokeRepository ASpokeRepository, CommunaRepository ACommunaRepository, CompanyCategoryRepository ACompanyCategoryRepository,
            LegalEntityRepository ALegalEntityRepository, ContractTemplateRepository AContractTemplateRepository,
            ApprovalContextRepository AApprovalContextRepository, ProductTypeRepository AProductTypeRepository,
            TransactionRepository ATransactionRepository, ProductRepository AProductRepository,
            ProductCategoryRepository AProductCategoryRepository, CustomerSegmentRepository ACustomerSegmentRepository,
            CustomerSubSegmentRepository ACustomerSubSegmentRepository, ContractRepository AContractRepository,
            MemberTypeRepository AMemberTypeRepository, DefaultAffinityRepository ADefaultAffinityRepository,
            BillingCycleRepository ABillingCycleRepository, InterestMethodRepository AInterestMethodRepository,
            ServiceRepository AServiceRepository, ProductStatusRepository AProductStatusRepository,
            ProductMemberTypeRepository AProductMemberTypeRepository, ProductClassRepository AProductClassRepository,
            ProductSubCategoryRepository AProductSubCategoryRepository, CurrencyRepository ACurrencyRepository,
            DecisionRepository ADecisionRepository, DecisionReasonRepository ADecisionReasonRepository,
            BankRepository ABankRepository, BranchRepository ABranchRepository, DesignationRepository ADesignationRepository,
            CardTypeRepository ACardTypeRepository, CityRepository ACityRepository, FinancialActivityRepository AFinancialActivityRepository,
            ContractTypeRepository AContractTypeRepository, CommissionRepository ACommissionRepository, BlockCodeRepository ABlockCodeRepository,
            NetworkRepository ANetworkRepository, UserAllowedBranchRepository AUserAllowedBranchRepository, CardStatusRepository ACardStatusRepository,
            MerchantsRepository AMerchantsRepository, MCCRepository AMCCRepository, CreditSegmentRepository ACreditSegmentRepository,
            PositionRepository APositionRepository, PlanMasterRepository APlanMasterRepository,FranchiseRepository AFranchiseRepository,
            SourcingRepository ASourcingRepository, FirstBillingRepository AFirstBillingRepository,
            InterestCalculationRepository AInterestCalculationRepository, CreditSubSegmentRepository ACreditSubSegmentRepository,
            SystemParameterRepository ASystemParameterRepository, FeeTypesRepository AFeeTypesRepository, TransactionSubCodeRepository ATransactionSubCodeRepository,
            EducationRepository AEducationRepository, ProfessionRepository AProfessionRepository, MaritialStatusRepository AMaritialStatusRepository ,
            ClosureReasonRepository AClosureReasonRepository, CardReactivationReasonRepository ACardReactivationReasonRepository, AlertTypeRepository AAlertTypeRepository,
            UserRepository AUserRepository, UserGroupMembersRepository AUserGroupMembersRepository, UserGroupPrivilegeRepository AUserGroupPrivilegeRepository,
            UserGroupRepository AUserGroupRepository, ApprovalQueueContextRepository AApprovalQueueContextRepository, UserEntityRepository AUserEntityRepository,
            UserCultureRepository AUserCultureRepository, DirectSalesAgentRepository ADirectSalesAgentRepository, SalesIdRepository ASalesIdRepository,
            TransactionCheckFrequencyRepository ATransactionCheckFrequencyRepository,
            ILoginSession ALoginSession
            )
        {
            objLoginSession = ALoginSession;

            if (APlanMasterRepository == null)
                throw new NullReferenceException("APlanMasterRepository");
            objPlanMasterRepository = APlanMasterRepository;

            if (APositionRepository == null)
                throw new NullReferenceException("APositionRepository");
            objPositionRepository = APositionRepository;

            if (AMerchantsRepository == null)
                throw new NullReferenceException("AMerchantsRepository");
            objMerchantsRepository = AMerchantsRepository;

            if (AMCCRepository == null)
                throw new NullReferenceException("AMCCRepository");
            objMCCRepository = AMCCRepository;

            if (ACreditSegmentRepository == null)
                throw new NullReferenceException("ACreditSegmentRepository");
            objCreditSegmentRepository = ACreditSegmentRepository;
            if (ACreditSubSegmentRepository == null)
                throw new NullReferenceException("ACreditSubSegmentRepository");
            objCreditSegmentRepository = ACreditSegmentRepository;

            if (ALookUpRepository == null)
                throw new NullReferenceException("ALookUpRepository");
            objLookUpRepository = ALookUpRepository;

            if (AOrganizationRepository == null)
                throw new NullReferenceException("AOrganizationRepository");
            objOrganizationLookUpRepository = AOrganizationRepository;

            if (ARegionRepository == null)
                throw new NullReferenceException("ARegionRepository");
            objRegionLookUpRepository = ARegionRepository;

            if (ACityCategoryRepository == null)
                throw new NullReferenceException("ACityCategoryRepository");
            objCityCategoryLookUpRepository = ACityCategoryRepository;

            if (AHubRepository == null)
                throw new NullReferenceException("AHubRepository");
            objHubLookUpRepository = AHubRepository;

            if (ASpokeRepository == null)
                throw new NullReferenceException("ASpokeRepository");
            objSpokeLookUpRepository = ASpokeRepository;

            if (ACommunaRepository == null)
                throw new NullReferenceException("ACommunaRepository");
            objCommunaLookUpRepository = ACommunaRepository;

           if (ACompanyCategoryRepository == null)
                throw new NullReferenceException("ACompanyCategoryRepository");
           objCompanyCategoryRepository = ACompanyCategoryRepository;

           if (ALegalEntityRepository == null)
               throw new NullReferenceException("ALegalEntityRepository");
           objLegalEntityRepository = ALegalEntityRepository;

           if (AContractTemplateRepository == null)
               throw new NullReferenceException("AContractTemplateRepository");
           objContractTemplateRepository = AContractTemplateRepository;

             if (AApprovalContextRepository == null)
               throw new NullReferenceException("AApprovalContextRepository");
             objApprovalContextRepository = AApprovalContextRepository;

            if (AProductTypeRepository == null)
                throw new NullReferenceException("AProductTypeRepository");
            objProductTypeRepository = AProductTypeRepository;

            if (ATransactionRepository == null)
                throw new NullReferenceException("ATransactionRepository");
            objTransactionRepository = ATransactionRepository;

            if (AProductRepository == null)
                throw new NullReferenceException("AProductRepository");
            objProductRepository = AProductRepository;

            if (AProductCategoryRepository == null)
                throw new NullReferenceException("AProductCategoryRepository");
            objProductCategoryRepository = AProductCategoryRepository;

            if (ACustomerSegmentRepository == null)
                throw new NullReferenceException("ACustomerSegmentRepository");
            objCustomerSegmentRepository = ACustomerSegmentRepository;

            if (ACustomerSubSegmentRepository == null)
                throw new NullReferenceException("ACustomerSubSegmentRepository");
            objCustomerSubSegmentRepository = ACustomerSubSegmentRepository;

            if (AContractRepository == null)
                throw new NullReferenceException("AContractRepository");
            objContractRepository = AContractRepository;

            if (AMemberTypeRepository == null)
                throw new NullReferenceException("AMemberTypeRepository");
            objMemberTypeRepository = AMemberTypeRepository;

            if (ADefaultAffinityRepository == null)
                throw new NullReferenceException("ADefaultAffinityRepository");
            objDefaultAffinityRepository = ADefaultAffinityRepository;

            if (ABillingCycleRepository == null)
                throw new NullReferenceException("ABillingCycleRepository");
            objBillingCycleRepository = ABillingCycleRepository;

            if (AInterestMethodRepository == null)
                throw new NullReferenceException("AInterestMethodRepository");
            objInterestMethodRepository = AInterestMethodRepository;

            if (AServiceRepository == null)
                throw new NullReferenceException("AServiceRepository");
            objServiceRepository = AServiceRepository;

            if (AProductStatusRepository == null)
                throw new NullReferenceException("AProductStatusRepository");
            objProductStatusRepository = AProductStatusRepository;

            if (AProductMemberTypeRepository == null)
                throw new NullReferenceException("AProductMemberTypeRepository");
            objProductMemberTypeRepository = AProductMemberTypeRepository;

            if (AProductClassRepository == null)
                throw new NullReferenceException("AProductClassRepository");
            objProductClassRepository = AProductClassRepository;

            if (AProductSubCategoryRepository == null)
                throw new NullReferenceException("AProductSubCategoryRepository");
            objProductSubCategoryRepository = AProductSubCategoryRepository;

            if (ACurrencyRepository == null)
                throw new NullReferenceException("ACurrencyRepository");
            objCurrencyRepository = ACurrencyRepository;

            if (ADecisionRepository == null)
                throw new NullReferenceException("ADecisionRepository");
            objDecisionRepository = ADecisionRepository;

            if (ADecisionReasonRepository == null)
                throw new NullReferenceException("ADecisionReasonRepository");
            objDecisionReasonRepository = ADecisionReasonRepository;

            if (ABankRepository == null)
                throw new NullReferenceException("ABankRepository");
            objBankRepository = ABankRepository;

            if (ABranchRepository == null)
                throw new NullReferenceException("ABranchRepository");
            objBranchRepository = ABranchRepository;

            if (ADesignationRepository == null)
                throw new NullReferenceException("ADesignationRepository");
            objDesignationRepository = ADesignationRepository;

            if (ACardTypeRepository == null)
                throw new NullReferenceException("ACardTypeRepository");
            objCardTypeRepository  = ACardTypeRepository;

            if (ACityRepository == null)
                throw new NullReferenceException("ACityRepository");
            objCityRepository = ACityRepository;

            if (AFinancialActivityRepository == null)
                throw new NullReferenceException("AFinancialAccountRepository");
            objFinancialActivityRepository = AFinancialActivityRepository;

            if (AContractTypeRepository == null)
                throw new NullReferenceException("AContractTypeRepository");
            objContractTypeRepository = AContractTypeRepository;

            if (ACommissionRepository == null)
                throw new NullReferenceException("ACommissionRepository");
            objCommissionRepository = ACommissionRepository;

            if (ABlockCodeRepository == null)
                throw new NullReferenceException("ABlockCodeRepository");
            objBlockCodeRepository = ABlockCodeRepository;

            if (ANetworkRepository == null)
                throw new NullReferenceException("ANetworkRepository");
            objNetworkRepository = ANetworkRepository;

            if (AUserAllowedBranchRepository == null)
                throw new NullReferenceException("AUserAllowedBranchRepository");
            objUserAllowedBranchRepository = AUserAllowedBranchRepository;

            if (ACardStatusRepository == null)
                throw new NullReferenceException("ACardStatusRepository");
            objCardStatusRepository = ACardStatusRepository;

            if (AFranchiseRepository == null)
                throw new NullReferenceException("AFranchiseRepository");
            objFranchiseRepository = AFranchiseRepository;

            if (ASourcingRepository == null)
                throw new NullReferenceException("ASourcingRepository");
            objSourcingRepository = ASourcingRepository;

            if (AFirstBillingRepository == null)
                throw new NullReferenceException("AFirstBillingRepository");
            objFirstBillingRepository = AFirstBillingRepository;

            if (AInterestCalculationRepository == null)
                throw new NullReferenceException("AInterestCalculationRepository");
            objInterestCalculationRepository = AInterestCalculationRepository;

            if (ASystemParameterRepository == null)
                throw new NullReferenceException("ASystemParameterRepository");
            objSystemParameterRepository = ASystemParameterRepository;

            if (AFeeTypesRepository == null)
                throw new NullReferenceException("AFeeTypesRepository");
            objFeeTypesRepository = AFeeTypesRepository;

            if (ATransactionSubCodeRepository == null)
                throw new NullReferenceException("ATransactionSubCodeRepository");
            objTransactionSubCodeRepository = ATransactionSubCodeRepository;

            if (AEducationRepository == null)
                throw new NullReferenceException("AEducationRepository");
            objEducationRepository = AEducationRepository;

            if (AProfessionRepository == null)
                throw new NullReferenceException("AProfessionRepository");
            objProfessionRepository = AProfessionRepository;

            if (AMaritialStatusRepository == null)
                throw new NullReferenceException("AMaritialStatusRepository");
            objMaritialStatusRepository = AMaritialStatusRepository;

            if (AClosureReasonRepository == null)
                throw new NullReferenceException("AClosureReasonRepository");
            objClosureReasonRepository = AClosureReasonRepository;

            if (ACardReactivationReasonRepository == null)
                throw new NullReferenceException("ACardReactivationReasonRepository");
            objCardReactivationReasonRepository = ACardReactivationReasonRepository;

            if (AAlertTypeRepository == null)
                throw new NullReferenceException("AAlertTypeREository");
            objAlertTypeRepository = AAlertTypeRepository;

            if (AUserRepository == null)
                throw new NullReferenceException("AUserRepository");
            objUserRepository = AUserRepository;

            if (AUserGroupMembersRepository == null)
                throw new NullReferenceException("AUserGroupMembersRepository");
            objUserGroupMembersRepository = AUserGroupMembersRepository;

            if (AUserGroupPrivilegeRepository == null)
                throw new NullReferenceException("AUserGroupPrivilegeRepository");
            objUserGroupPrivilegeRepository = AUserGroupPrivilegeRepository;

            if (AUserGroupRepository == null)
                throw new NullReferenceException("AUserGroupRepository");
            objUserGroupRepository = AUserGroupRepository;

            if (AApprovalQueueContextRepository == null)
                throw new NullReferenceException("AApprovalQueueContextRepository");
            objApprovalQueueContextRepository = AApprovalQueueContextRepository;

            if (AUserEntityRepository == null)
                throw new NullReferenceException("AUserEntityRepository");
            objUserEntityRepository = AUserEntityRepository;

            if (AUserCultureRepository == null)
                throw new NullReferenceException("AUserCultureRepository");
            objUserCultureRepository = AUserCultureRepository;

            if (ADirectSalesAgentRepository == null)
                throw new NullReferenceException("ADirectSalesAgentRepository");
            objDirectSalesAgentRepository = ADirectSalesAgentRepository;

            if (ASalesIdRepository == null)
                throw new NullReferenceException("ASalesIdRepository");
            objSalesIdRepository = ASalesIdRepository;

            if (ATransactionCheckFrequencyRepository == null)
                throw new NullReferenceException("ATransactionCheckFrequencyRepository");
            objTransactionCheckFrequencyRepository = ATransactionCheckFrequencyRepository;
        }


        #endregion
        
        #region Implemeted methods
        public List<SelectListItem> GetAll<TEntity>(string ASelected,string AContext, bool ASelectOneNeeded = true) where TEntity : class
        {
            
            //IQueryable<TEntity> result = (IQueryable<TEntity>)objLookUpRepository.GetAll().AsQueryable();
            //if(typeof(TEntity) == typeof(CountryRepository))
            if(AContext.Equals("Country"))
            {
                objListItem = objLookUpRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.CountryCode, Description = x.CountryName }).OrderBy(o => o.Description).ToList();
                             
            }
            else if (AContext.Equals("Organization"))
            {
                objListItem = objOrganizationLookUpRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.OrganizationCode, Description = x.OrganizationCode + " - " + x.OrganizationDescription }).ToList();

            }
            else if (AContext.Equals("OrganizationShortName"))
            {
                objListItem = objOrganizationLookUpRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.OrganizationCode, Description =  x.OrganizationShortName }).ToList();

            }
            else if (AContext.Equals("OrganizationForLogin"))
            {
                objListItem = objOrganizationLookUpRepository.GetAll().Where(x => x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.OrganizationCode, Description = x.OrganizationCode + " - " + x.OrganizationDescription }).ToList();
                
            }
            else if (AContext.Equals("Organizationwithoutcode"))
            {
                objListItem = objOrganizationLookUpRepository.GetAll().Where(x => x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.OrganizationCode, Description =  x.OrganizationShortName  }).ToList();

            }
            else if (AContext.Equals("UserTaggedOrganization"))
            {
                objListItem = new List<ListItem>();
                if (System.Security.Principal.GenericPrincipal.Current.Identity.Name != Utils.CNST_ADMIN)
                {
                    var queryUserTaggedOrganization = objUserRepository.GetAll().Where(x => x.Username.Equals(ASelected))
                                .Join(objUserGroupMembersRepository.GetAll(), r => r.Username, p => p.UserName, (r, p) => new
                                {
                                    r.Username,
                                    p.GroupCode
                                })
                                .Join(objUserGroupPrivilegeRepository.GetAll().Where(x => x.ActionValue != null), a => a.GroupCode, b => b.GroupCode, (a, b) => new
                                {
                                    a.Username,
                                    b.OrganizationCode,
                                })
                                .Join(objOrganizationLookUpRepository.GetAll().Where(x => x.ApprovalStatus == "A"), c => c.OrganizationCode, d => d.OrganizationCode, (a, b) => new
                                {
                                    a.Username,
                                    a.OrganizationCode,
                                    b.OrganizationDescription
                                })
                                .GroupBy(g => new
                                {
                                    OrgCode = g.OrganizationCode,
                                    Orgname = g.OrganizationDescription,
                                    UserName = g.Username
                                })
                                .Select(g => new
                                {
                                    OrganizationCode = g.Key.OrgCode,
                                    UserName = g.Key.UserName,
                                    OrgDesc = g.Key.Orgname,
                                    Count = g.Count()
                                });

                    queryUserTaggedOrganization.ToList().ForEach(x => objListItem.Add(new ListItem() { Code = x.OrganizationCode, Description = x.OrganizationCode + " - " + x.OrgDesc }));
                }
                else 
                {
                    objListItem = objOrganizationLookUpRepository.GetAll().Where(x => x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.OrganizationCode, Description = x.OrganizationCode + " - " + x.OrganizationDescription }).ToList();
                }
            }
            else if (AContext.Equals("Region"))
            {
                //objListItem = objRegionLookUpRepository.AllMatching(x => x.CountryCode == ASelected).AsEnumerable().Select(x => new ListItem { Code = x.RegionCode, Description = x.RegionCode + "-" + x.RegionName }).OrderBy(o => o.Description).ToList();
                objListItem = objRegionLookUpRepository.GetAll().Select(x => new ListItem { Code = x.RegionCode, Description = x.RegionCode + "-" + x.RegionName }).OrderBy(o => o.Description).ToList();
            }
            else if (AContext.Equals("CityCategory"))
            {
                objListItem = objCityCategoryLookUpRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.CityCategoryCode, Description = x.Description }).ToList();

            }
            else if (AContext.Equals("Hub"))
            {
                objListItem = objHubLookUpRepository.AllMatching(x => x.CountryCode == ASelected).AsEnumerable().Select(x => new ListItem { Code = x.HubCode, Description = x.HubName }).ToList();
            }
            else if (AContext.Equals("Spoke"))
            {
                objListItem = objSpokeLookUpRepository.AllMatching(x => x.CountryCode == ASelected).AsEnumerable().Select(x => new ListItem { Code = x.SpokeCode, Description = x.SpokeName }).ToList();
            }
            else if (AContext.Equals("Communa"))
            {
                objListItem = objCommunaLookUpRepository.AllMatching(x => x.CountryCode == ASelected).AsEnumerable().Select(x => new ListItem { Code = x.CommunaCode, Description = x.CommunaName }).OrderBy(o => o.Description).ToList();
            }

            else if (AContext.Equals("Communabyutils"))
            {
                objListItem = objCommunaLookUpRepository.AllMatching(x => x.CountryCode == Utils.CountryCode ).AsEnumerable().Select(x => new ListItem { Code = x.CommunaCode, Description = x.CommunaName }).OrderBy(o => o.Description).ToList();
            }   

            else if (AContext.Equals("CommunaByCountry"))
            {
                objListItem = new List<ListItem>();
                if (!string.IsNullOrEmpty(ASelected))
                {
                    objListItem = objCityRepository.AllMatching(x => x.CountryCode == ASelected).AsEnumerable().Select(x => new ListItem { Code = x.CommunaCode, Description = x.CommunaName }).OrderBy(o => o.Description).ToList();
                }
            }




            else if (AContext.Equals("CityByCommuna"))
            {
                objListItem = new List<ListItem>();
                if (!string.IsNullOrEmpty(ASelected))
                {
                    objListItem = objCityRepository.AllMatching(x => x.CommunaCode == ASelected).AsEnumerable().Select(x => new ListItem { Code = x.CityCode, Description = x.CityName }).OrderBy(o => o.Description).ToList();
                }
            }
            else if (AContext.Equals("RegionByCommuna"))
            {
                objListItem = new List<ListItem>();
                if (!string.IsNullOrEmpty(ASelected))
                {
                    var region = objCityRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.CommunaCode == ASelected && x.ApprovalStatus.Equals("A")).AsEnumerable().
                        Join(objRegionLookUpRepository.GetAll().AsEnumerable(), x => x.RegionCode, y => y.RegionCode, (x, y) => new
                        {
                            y.RegionCode,
                            y.RegionName
                        }).Select(g => new
                        {
                            Code = g.RegionCode,
                            Description = g.RegionName
                        }
                        );

                    objListItem = new List<ListItem>();
                    region.ToList().ForEach(x => objListItem.Add(new ListItem() { Code = x.Code, Description = x.Description }));
                }
            }
            



            else if (AContext.Equals("CommunaByRegion"))
            {
                //  objListItem = objCommunaLookUpRepository.AllMatching(x => x.RegionCode == ASelected).AsEnumerable().Select(x => new ListItem { Code = x.CommunaCode, Description = x.CommunaName }).ToList();
            }
            else if (AContext.Equals("CompanyCategory"))
            {
                objListItem = objCompanyCategoryRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.CompanyCategoryCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("LegalEntity"))
            {
                objListItem = objLegalEntityRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.LegalEntityCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("ContractTemplate"))
            {
                objListItem = objContractTemplateRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode).AsEnumerable().Select(x => new ListItem { Code = x.ContractTemplateCode, Description = x.ContractTemplateDescription }).ToList();
            }
            else if (AContext.Equals("ReasonContext"))
            {
                objListItem = objApprovalContextRepository.AllMatching(x => x.ReasonCode == ASelected).AsEnumerable().Select(x => new ListItem { Code = x.ReasonContextCode, Description = x.ReasonContextDescription }).ToList();
            }
            else if (AContext.Equals("ApprovalQueueContext"))
            {

                LoginUserDTO loggedInUser = objLoginSession.LoggedInUser;

                List<UserPrivilegeDTO> privilege = loggedInUser.UserPrivileges;

                objListItem = new List<ListItem>();
                
                var newquery = privilege.Select(x => new { x.EntityCode, x.EntityDesc }).
                                Join(objApprovalQueueContextRepository.GetAll(), r => r.EntityCode, p => p.EntityCode, (r, p) => new
                                {
                                    p.ContextCode,
                                    p.ContextDescription
                                })
                                .Select(g => new 
                                { 
                                    Code = g.ContextCode, Description = g.ContextDescription
                                }).OrderBy(x => x.Description);

                
                newquery.ToList().ForEach(x => objListItem.Add(new ListItem() { Code = x.Code, Description = x.Description }));


            }
            else if (AContext.Equals("ProductType"))
            {
                objListItem = objProductTypeRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.ProductTypeCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("Transaction"))
            {
                objListItem = objTransactionRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.TransactionCode, Description = x.TransactionCode + " - " + x.Description }).ToList();
            }
            else if (AContext.Equals("PlanTransaction"))
            {
                objListItem = objTransactionRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.InstallmentRequired == true && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.TransactionCode, Description = x.TransactionCode + " - " + x.Description }).ToList();
            }
            else if (AContext.Equals("Product"))
            {
                objListItem = objProductRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).Select(x => new { x.ProductCode, x.ProductName }).AsEnumerable().Select(x => new ListItem { Code = x.ProductCode, Description = x.ProductCode + " - " + x.ProductName }).ToList();
            }
            else if (AContext.Equals("ProductCategory"))
            {
                objListItem = objProductCategoryRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.ProductCategoryCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("CustomerSegment"))
            {
                objListItem = objCustomerSegmentRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.SegmentCode, Description = x.SegmentName }).ToList();
            }
            else if (AContext.Equals("CustomerSubSegment"))
            {
                objListItem = objCustomerSubSegmentRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.SubSegmentCode, Description = x.SubSegmentName }).ToList();
            }
            else if (AContext.Equals("Contract"))
            {
                objListItem = objContractRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.ContractCode, Description = x.ContractDescription }).ToList();
            }
            else if (AContext.Equals("MemberType"))
            {
                objListItem = objMemberTypeRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.MemberTypeCode, Description = x.MemberTypeName }).ToList();
            }
            else if (AContext.Equals("DefaultAffinity"))
            {
                objListItem = objDefaultAffinityRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.DefaultAffinityCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("BillingCycle"))
            {
                //objListItem = objBillingCycleRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.BillingCycleCode, Description = x.Description }).ToList();
                objListItem = objSystemParameterRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ParameterName == "BILL_CYCLE_CODE").AsEnumerable().Select(x => new ListItem { Code = x.ParameterId.ToString(), Description = (x.DefaultValue + " - " + x.Description) }).ToList();

            }

            else if (AContext.Equals("ProductFranchise"))
            {
                objListItem = objFranchiseRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.FranchiseCode, Description = x.FranchiseName }).ToList();
            }
            else if (AContext.Equals("FirstBilling"))
            {
                objListItem = objFirstBillingRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.Code, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("InterestCalculation"))
            {
                objListItem = objInterestCalculationRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.Code, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("InterestMethod"))
            {
                objListItem = objInterestMethodRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.InterestMethodCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("Service"))
            {
                objListItem = objServiceRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.ServiceCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("ProductStatus"))
            {
                objListItem = objProductStatusRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.ProductStatusCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("ProductMemberType"))
            {
                objListItem = objProductMemberTypeRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.ProductMemberTypeCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("ProductClass"))
            {
                objListItem = objProductClassRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.ProductClassCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("ProductSubCategory"))
            {
                objListItem = objProductSubCategoryRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.ProductSubCategoryCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("Currency"))
            {
                objListItem = objCurrencyRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.CurrencyCode, Description = x.CurrencyName }).ToList();
            }
            else if (AContext.Equals("Nationality"))
            {
                objListItem = objLookUpRepository.GetAll().Select(x => new { x.CountryCode, x.Nationality }).AsEnumerable().Select(x => new ListItem { Code = x.CountryCode, Description = x.Nationality }).ToList();
            }
            else if (AContext.Equals("Decision"))
            {
                objListItem = objDecisionRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.DecisionCode, Description = x.DecisionName }).ToList();
            }
            else if (AContext.Equals("DecisionReason"))
            {
                objListItem = objDecisionReasonRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.ReasonCode, Description = x.ReasonDescription }).ToList();
            }
            else if (AContext.Equals("Bank"))
            {
                objListItem = objBankRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.BankCode, Description = x.BankName }).ToList();
            }
            else if (AContext.Equals("Branch"))
            {
                objListItem = objBranchRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.BranchCode, Description = x.BranchCode + '-' + x.BranchName }).ToList();
            }
            else if (AContext.Equals("Designation"))
            {
                objListItem = objDesignationRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).Select(x => new { x.DesignationCode, x.DesignationDescription }).AsEnumerable().Select(x => new ListItem { Code = x.DesignationCode, Description = x.DesignationDescription }).ToList();
            }
            else if (AContext.Equals("CardType"))
            {
                objListItem = objCardTypeRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.CardTypeCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("CardStatus"))
            {
                objListItem = objCardStatusRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).Select(x => new { x.CardStatusCode, x.Narration }).AsEnumerable().Select(x => new ListItem { Code = x.CardStatusCode, Description = x.Narration }).ToList();
            }
            else if (AContext.Equals("City"))
            {
                objListItem = objCityRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).Select(x => new { x.CityCode, x.CityName }).AsEnumerable().Select(x => new ListItem { Code = x.CityCode, Description = x.CityName }).OrderBy(o => o.Description).ToList();
            }
            else if (AContext.Equals("FinancialActivity"))
            {
                objListItem = objFinancialActivityRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).Select(x => new { x.FinancialActivityCode, x.FinancialActivityName }).AsEnumerable().Select(x => new ListItem { Code = x.FinancialActivityCode, Description = x.FinancialActivityName }).ToList();
            }
            else if (AContext.Equals("ContractType"))
            {
                objListItem = objContractTypeRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.ContractTypeCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("Commission"))
            {
                objListItem = objCommissionRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).Select(x => new { x.CommissionCode, x.Description }).AsEnumerable().Select(x => new ListItem { Code = x.CommissionCode, Description = x.CommissionCode + " - " + x.Description }).ToList();
            }
            else if (AContext.Equals("BlockCode"))
            {
                objListItem = objBlockCodeRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).Select(x => new { x.Code, x.Description }).AsEnumerable().Select(x => new ListItem { Code = x.Code, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("BlockCodeForCardSegmentBlock"))
            {
                objListItem = objBlockCodeRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A") && x.IsInternalBlock == (x.IsInternalBlock == null ? false : false)).Select(x => new { x.Code, x.Description }).AsEnumerable().Select(x => new ListItem { Code = x.Code, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("Network"))
            {
                objListItem = objNetworkRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.NetworkCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("ProductByProductCategory"))
            {
                objListItem = objProductRepository.AllMatching(x => x.ProductCategoryCode == ASelected && x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).Select(x => new { x.ProductCode, x.ProductName }).AsEnumerable().Select(x => new ListItem { Code = x.ProductCode, Description = x.ProductCode + " - " + x.ProductName }).ToList();
            }
            else if (AContext.Equals("Merchant"))
            {
                objListItem = objMerchantsRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).Select(x => new ListItem { Code = x.MerchantCode, Description = x.MerchantCode +" - "+ x.MerchantName }).ToList();
            }
            else if (AContext.Equals("MCC"))
            {
                //objListItem = objMCCRepository.AllMatching(x => x.MCCCode == ASelected).Select(x => new { x.MCCCode, x.MCCName }).AsEnumerable().Select(x => new ListItem { Code = x.MCCCode, Description = x.MCCName }).ToList();\
                objListItem = objMCCRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.MCCCode, Description = x.MCCName }).ToList();
            }
            else if (AContext.Equals("CreditSegment"))
            {
                objListItem = objCreditSegmentRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.CardSegmentCode, Description = x.CardSegmentCode + " - " + x.CardSegmentName }).ToList();
            }
            else if (AContext.Equals("CreditSegmentPromo"))
            {
                objListItem = objCreditSegmentRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A") && (x.CreditSegmentType != "NL" || x.CreditSegmentType == null)).AsEnumerable().Select(x => new ListItem { Code = x.CardSegmentCode, Description = x.CardSegmentCode + " - " + x.CardSegmentName }).ToList();
            }
            else if (AContext.Equals("CreditSubSegment"))
            {
                objListItem = objSystemParameterRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ParameterName == "CRD_SUB_SGMNT" && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.ParameterId.ToString(), Description = (x.DefaultValue + " - " + x.Description) }).ToList();
            }
            else if (AContext.Equals("Position"))
            {
                //objListItem = objMCCRepository.AllMatching(x => x.MCCCode == ASelected).Select(x => new { x.MCCCode, x.MCCName }).AsEnumerable().Select(x => new ListItem { Code = x.MCCCode, Description = x.MCCName }).ToList();\
                objListItem = objPositionRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.Code, Description = x.Name }).ToList();
            }
            else if (AContext.Equals("PlanMaster"))
            {
                //objListItem = objMCCRepository.AllMatching(x => x.MCCCode == ASelected).Select(x => new { x.MCCCode, x.MCCName }).AsEnumerable().Select(x => new ListItem { Code = x.MCCCode, Description = x.MCCName }).ToList();\
                //objListItem = objPlanMasterRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).Select(x => new ListItem { Code = x.PlanID.ToString(), Description = x.CreditSegmentCode }).ToList();

                objListItem = objPlanMasterRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.PlanID.ToString(), Description = x.CreditSegmentCode }).ToList();
            }
            else if (AContext.Equals("UserBranch"))
            {
                var userbranchList = objBranchRepository.GetAll().AsEnumerable()
                   .Join(objUserAllowedBranchRepository.GetAll(),
                   branch => branch.BranchCode,
                   userBranch => userBranch.BranchCode,
                   (branch, userBranch) => new { branchCode = userBranch.BranchCode, branchName = branch.BranchName, userName = userBranch.Username, organizationCode = branch.OrganizationCode })
                   .Where(x => x.userName.ToLower().Equals(ASelected.ToLower()) && x.organizationCode == Utils.OrganizationCode);
                objListItem = new List<ListItem>();
                userbranchList.ToList().ForEach(x => objListItem.Add(new ListItem() { Code = x.branchCode, Description = x.branchName }));
            }
            else if (AContext.Equals("Sourcing"))
            {
                objListItem = objSourcingRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).Select(x => new { x.SourcingChannelCode, x.SourcingChannelName }).AsEnumerable().Select(x => new ListItem { Code = x.SourcingChannelCode, Description = x.SourcingChannelCode + '-' + x.SourcingChannelName }).ToList();
            }
            else if (AContext.Equals("ProductMemberTypeByCode"))
            {
                objListItem = objProductMemberTypeRepository.AllMatching(x => x.ProductMemberTypeCode == ASelected).AsEnumerable().Select(x => new ListItem { Code = x.ProductMemberTypeCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("ProductMemberType"))
            {
                objListItem = objProductMemberTypeRepository.GetAll().Select(x => new { x.ProductMemberTypeCode, x.Description }).AsEnumerable().Select(x => new ListItem { Code = x.ProductMemberTypeCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("CycleCodeByCode"))
            {
                int parameterId = int.Parse(ASelected);
                objListItem = objSystemParameterRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ParameterName == "BILL_CYCLE_CODE" && x.ParameterId == parameterId && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.DefaultValue, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("FeeType"))
            {
                objListItem = objFeeTypesRepository.GetAll().Select(x => new { x.FeeTypeCode, x.FeeTypeDescription }).AsEnumerable().Select(x => new ListItem { Code = x.FeeTypeCode, Description = x.FeeTypeDescription }).ToList();
            }
            else if (AContext.Equals("PaymentMode"))
            {
                objListItem = objSystemParameterRepository.AllMatching(x => x.ParameterName == "PAYMENT_MODE" && x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.ParameterId.ToString(), Description = (x.Description) }).ToList();
            }
            else if (AContext.Equals("TaggedIncomingTransType"))
            {
                var IncomingTransTypeList = objSystemParameterRepository.AllMatching(x => x.ApprovalStatus.Equals("A") && x.OrganizationCode == Utils.OrganizationCode).AsEnumerable()
                   .Join(objTransactionSubCodeRepository.AllMatching(x => x.IncomingTransType > 0 && x.ApprovalStatus.Equals("A") && x.OrganizationCode == Utils.OrganizationCode).AsEnumerable(),
                   SysParam => SysParam.ParameterId,
                   SubCode => SubCode.IncomingTransType,
                   (SysParam, SubCode) => new { ParameterName = SysParam.ParameterName, Code = SysParam.ParameterId, Description = SysParam.DefaultValue + " - " + SysParam.Description })
                   .Where(x => x.ParameterName.Equals("INCMNG_TRNS_TYPE"));

                objListItem = new List<ListItem>();
                IncomingTransTypeList.ToList().ForEach(x => objListItem.Add(new ListItem() { Code = x.Code.ToString(), Description = x.Description }));
                //objListItem = objSystemParameterRepository.AllMatching(x => x.ParameterName == "INCMNG_TRNS_TYPE").AsEnumerable().Select(x => new ListItem { Code = x.ParameterId.ToString(), Description = (x.DefaultValue + " - " + x.Description) }).ToList();
            }
            else if (AContext.Equals("ManTransTaggedIncomingTransType"))
            {
                var IncomingTransTypeList = objSystemParameterRepository.AllMatching(x => x.ApprovalStatus.Equals("A") && x.OrganizationCode == Utils.OrganizationCode  && x.DefaultValue != "PYC" && x.DefaultValue != "PYD").AsEnumerable()
                   .Join(objTransactionSubCodeRepository.AllMatching(x => x.IncomingTransType > 0 && x.ApprovalStatus.Equals("A") && (x.IsDebitOrCredit == "D" || x.IncomingTransType == 173) && x.OrganizationCode == Utils.OrganizationCode).AsEnumerable(),
                   SysParam => SysParam.ParameterId,
                   SubCode => SubCode.IncomingTransType,
                   (SysParam, SubCode) => new { ParameterName = SysParam.ParameterName, Code = SysParam.ParameterId, Description = SysParam.DefaultValue + " - " + SysParam.Description })
                   .Where(x => x.ParameterName.Equals("INCMNG_TRNS_TYPE"));

                objListItem = new List<ListItem>();
                IncomingTransTypeList.ToList().ForEach(x => objListItem.Add(new ListItem() { Code = x.Code.ToString(), Description = x.Description }));
                //objListItem = objSystemParameterRepository.AllMatching(x => x.ParameterName == "INCMNG_TRNS_TYPE").AsEnumerable().Select(x => new ListItem { Code = x.ParameterId.ToString(), Description = (x.DefaultValue + " - " + x.Description) }).ToList();
            }
            else if (AContext.Equals("RevTransTaggedIncomingTransType"))
            {
                var IncomingTransTypeList = objSystemParameterRepository.AllMatching(x => x.ApprovalStatus.Equals("A") && x.OrganizationCode == Utils.OrganizationCode && x.DefaultValue != "PYC" && x.DefaultValue != "CADJ").AsEnumerable()
                   .Join(objTransactionSubCodeRepository.AllMatching(x => x.IncomingTransType > 0 && x.ApprovalStatus.Equals("A") && (x.IsDebitOrCredit == "C" || x.IncomingTransType == 106 )&& x.OrganizationCode == Utils.OrganizationCode).AsEnumerable(),
                   SysParam => SysParam.ParameterId,
                   SubCode => SubCode.IncomingTransType,
                   (SysParam, SubCode) => new { ParameterName = SysParam.ParameterName, Code = SysParam.ParameterId, Description = SysParam.DefaultValue + " - " + SysParam.Description })
                   .Where(x => x.ParameterName.Equals("INCMNG_TRNS_TYPE"));

                objListItem = new List<ListItem>();
                IncomingTransTypeList.ToList().ForEach(x => objListItem.Add(new ListItem() { Code = x.Code.ToString(), Description = x.Description }));
                //objListItem = objSystemParameterRepository.AllMatching(x => x.ParameterName == "INCMNG_TRNS_TYPE").AsEnumerable().Select(x => new ListItem { Code = x.ParameterId.ToString(), Description = (x.DefaultValue + " - " + x.Description) }).ToList();
            }
            else if (AContext.Equals("IncomingTransType"))
            {
                //objListItem = objSystemParameterRepository.AllMatching(x => x.ParameterName == "INCMNG_TRNS_TYPE").AsEnumerable().Select(x => new ListItem { Code = x.ParameterId.ToString(), Description = (x.DefaultValue + " - " + x.Description) }).ToList();

                var IncomingTransTypeList1 = objTransactionSubCodeRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode).AsEnumerable().Select(x => new ListItem { Code = x.IncomingTransType.ToString(), Description = (x.Description) });
                var IncomingTransTypeList2 = objSystemParameterRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ParameterName == "INCMNG_TRNS_TYPE" && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.ParameterId.ToString(), Description = (x.DefaultValue + " - " + x.Description) });

                var newList = IncomingTransTypeList1.Select(c => c.Code);
                var IncomingTransTypeList = IncomingTransTypeList2.Where(f => !newList.Contains(f.Code));

                objListItem = new List<ListItem>();

                IncomingTransTypeList.ToList().ForEach(x => objListItem.Add(new ListItem() { Code = x.Code.ToString(), Description = x.Description }));

            }
            else if (AContext.Equals("IncomingTransTypeById"))
            {
                objListItem = new List<ListItem>();
                if (ASelected != null && ASelected != "")
                {
                    int parameterId = int.Parse(ASelected);
                    objListItem = objSystemParameterRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ParameterName == "INCMNG_TRNS_TYPE" && x.ParameterId == parameterId && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.ParameterId.ToString(), Description = (x.DefaultValue + " - " + x.Description) }).ToList();
                }
            }
            else if (AContext.Equals("Education"))
            {
                objListItem = objEducationRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.EducationCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("Profession"))
            {
                objListItem = objProfessionRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.ProfessionCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("MaritalStatus"))
            {
                objListItem = objMaritialStatusRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.MaritalStatusCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("ClosureReason"))
            {
                objListItem = objClosureReasonRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.ReasonCode, Description = x.ReasonDescription }).ToList();
            }
            else if (AContext.Equals("CardReactivationReason"))
            {
                objListItem = objCardReactivationReasonRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.ReactivationReasonCode, Description = x.ReasonDescription }).ToList();
            }
            else if (AContext.Equals("CityByRegion"))
            {
                objListItem = objCityRepository.AllMatching(x => x.RegionCode == ASelected && x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.CityCode, Description = x.CityName }).OrderBy(o => o.Description).ToList();
            }
            else if (AContext.Equals("ContractByProductCode"))
            {
                //objListItem = objContractRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ProductCode == ASelected && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.ContractCode, Description = x.ContractDescription }).ToList();
                var contract = objProductRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ProductCode == ASelected && x.ApprovalStatus.Equals("A")).AsEnumerable().
                    Join(objContractRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable(), x => x.ContractCode, y => y.ContractCode, (x, y) => new
                    {
                        y.ContractCode,
                        y.ContractDescription
                    }).Select(g => new
                        {
                            Code = g.ContractCode,
                            Description = g.ContractDescription
                        }
                    );

                objListItem = new List<ListItem>();
                contract.ToList().ForEach(x => objListItem.Add(new ListItem() { Code = x.Code, Description = x.Description }));
            }
            else if (AContext.Equals("TransCodeSubCode"))
            {
                objListItem = objTransactionSubCodeRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.TransactionSubCodeId.ToString(), Description = x.TransactionCode + x.SubCode + " - " + x.Description }).ToList();
            }
            else if (AContext.Equals("TransCodeSubCodeForAdjstmntSearch"))
            {
                objListItem = objTransactionSubCodeRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.TransactionCode + x.SubCode, Description = x.TransactionCode + x.SubCode + " - " + x.Description }).ToList();
            }
            else if (AContext.Equals("SysParamTransSubCode"))
            {
                objListItem = objSystemParameterRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ParameterName == "TXN_SUB_CODE" && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.DefaultValue.ToString(), Description = (x.DefaultValue + " - " + x.Description) }).ToList();
            }
            else if (AContext.Equals("SysParamProductFirstDigit"))
            {
                objListItem = objSystemParameterRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ParameterName == "PDT_FIRST_DIGIT" && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.DefaultValue.ToString(), Description = x.DefaultValue }).ToList();
            }
            else if (AContext.Equals("SysParamAvlCashLimit"))
            {
                objListItem = objSystemParameterRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ParameterName == "AVL_CASH_LIMIT" && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.DefaultValue.ToString(), Description = x.DefaultValue }).ToList();
            }
            else if (AContext.Equals("SysParamCardNoLen"))
            {
                objListItem = objSystemParameterRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ParameterName == "CARD_NO_LEN" && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.DefaultValue.ToString(), Description = x.DefaultValue }).ToList();
            }
            else if (AContext.Equals("SysParamProductStatus"))
            {
                if (ASelected != null && ASelected != "")
                {
                    objListItem = objSystemParameterRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ParameterName == "PRODUCT_STATUS" && x.ApprovalStatus.Equals("A") && x.DefaultValue == ASelected).AsEnumerable().Select(x => new ListItem { Code = x.DefaultValue.ToString(), Description = x.Description }).ToList();
                }
                else
                {
                    objListItem = objSystemParameterRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ParameterName == "PRODUCT_STATUS" && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.DefaultValue.ToString(), Description = x.Description }).ToList();
                }
            }
            else if (AContext.Equals("SysParamCardNoPattern"))
            {
                objListItem = objSystemParameterRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ParameterName == "CARD_NO_PATTERN" && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.DefaultValue.ToString(), Description = x.Description }).ToList();
            }
            else if (AContext.Equals("AlertType"))
            {
                objListItem = objAlertTypeRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.AlertCode, Description = x.AlertName }).ToList();
            }
            else if (AContext.Equals("UserGroup"))
            {
                objListItem = objUserGroupRepository.AllMatching(x => x.GroupCode != "ADMIN" && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.GroupCode, Description = x.GroupCode + " - " + x.GroupName }).ToList();
            }
            else if (AContext.Equals("User"))
            {
                objListItem = objUserRepository.AllMatching(x => x.Username != "Administrator"&& x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.Username, Description = x.Username + " - " + x.Username }).ToList();
            }
            else if (AContext.Equals("Culture"))
            {
                objListItem = objUserCultureRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.CultureCode, Description = x.Description }).ToList();
            }
            else if (AContext.Equals("DirectSalesAgent"))
            {
                objListItem = objDirectSalesAgentRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.DSACode, Description = x.DSAName }).ToList();
            }
            else if (AContext.Equals("PlanTransaction"))
            {
                objListItem = objTransactionRepository.GetAll().Where(x => x.OrganizationCode == Utils.OrganizationCode && x.InstallmentRequired == true && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.TransactionCode, Description = x.TransactionCode + " - " + x.Description }).ToList();
            }
            else if (AContext.Equals("SalesIdByRUT"))
            {
                objListItem = objSalesIdRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ApprovalStatus.Equals("A") && x.RUT == ASelected).AsEnumerable().Select(x => new ListItem { Code = x.SalesId.ToString(), Description = x.SalesId }).ToList();
            }
            else if (AContext.Equals("TransactionCheckFrequency"))
            {
                objListItem = objTransactionCheckFrequencyRepository.GetAll().AsEnumerable().Select(x => new ListItem { Code = x.TransactionCheckFrequencyCode, Description = x.TransactionCheckFrequencyDescription }).OrderBy(o => o.Code). ToList();
            }
            else if (AContext.Equals("SysParamDelPortFolio"))
            {
                objListItem = objSystemParameterRepository.AllMatching(x => x.OrganizationCode == Utils.OrganizationCode && x.ParameterName == "DELQ_PORTFOLIO" && x.ApprovalStatus.Equals("A")).AsEnumerable().Select(x => new ListItem { Code = x.ParameterValue.ToString(), Description = x.Description }).ToList();
            }

            List<SelectListItem> list = new List<SelectListItem>();
            if (ASelectOneNeeded)
                list.Add(new SelectListItem { Text = Resources.SelectOne, Value = string.Empty });

            var lisData = (from items in objListItem
                           select items).AsEnumerable().Select(m => new SelectListItem
                           {
                               Text = (string)m.GetType().GetProperty("Description").GetValue(m),
                               Value = (string)m.GetType().GetProperty("Code").GetValue(m),
                               Selected = (ASelected != "") ? ((string)
                                 m.GetType().GetProperty("Code").GetValue(m) ==
                                 ASelected ? true : false) : false,
                           }).ToList();
            list.AddRange(lisData);
                       

            return list;
        }
        #endregion

        #region Private Methods
       
        #endregion

    }
}
