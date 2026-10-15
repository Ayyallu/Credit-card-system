using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CT = CC_Ingenio.Core.ContactRepo.ContactAggregate;
using CC_Ingenio.Core.ContactRepo.AddressAggregate;
using CC_Ingenio.Core.ContactRepo.ContactAddressAggregate;

using CC_Ingenio.Core.MasterRepo.MasterAggregate;
using CC_Ingenio.DataTranfer.MasterInfo;

using CC_Ingenio.DataTranfer.ContactInfo;
using CC_Ingenio.DataTranfer.ApprovalQueueInfo;
using CC_Ingenio.Core.ApprovalRepo.ApprovalAggregate;
using CC_Ingenio.DataTranfer.LookUpInfo;
using CC_Ingenio.Core.LookUpRepo;
using CC_Ingenio.Core.UserRepo.UserAggregate;
using CC_Ingenio.DataTranfer.UserInfo;

using CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate;
using CC_Ingenio.DataTranfer.CardBookingInfo ;


using CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate;
using CC_Ingenio.DataTranfer.AuthorizationInfo;
using CC_Ingenio.DataTranfer.CustomerServiceInfo;
using CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate;
using CC_Ingenio.DataTranfer.BillingInfo;
using CC_Ingenio.Core.BillingRepo.BillingAggregate;
using CC_Ingenio.Common.Utils;


namespace CC_Ingenio.Manager.Conversion
{
    public static class Mapping
    {

        #region ContactDTO
        public static ContactDTO ContactToContactDTO(CT.Contact AContact)
        {

            ContactDTO objContactDTO = new ContactDTO()
            {
                ContactId = AContact.ContactId,
                FirstName = AContact.FirstName,
                HomePhoneNo = AContact.HomePhoneNo,
                LastName = AContact.LastName,
                MobileNo = AContact.MobileNo,
                OfficePhoneNo = AContact.OfficePhoneNo,
                OrganizationName = AContact.OrganizationName,
                SSN = AContact.SSN,
                Type = AContact.Type
            };

            foreach (var contactAddress in AContact.ContactAddress)
            {
                AddressDTO objAddressDTO = AddressToAddressDTO(contactAddress.Address);
                objContactDTO.AddressDTO.Add(objAddressDTO);

            }

            return objContactDTO;
        }

        public static AddressDTO AddressToAddressDTO(Address AAddress)
        {
            AddressDTO objAddressDTO = new AddressDTO();
            if (AAddress != null)
            {
                objAddressDTO.AddressId = AAddress.AddressId;
                objAddressDTO.Line1 = AAddress.Line1;
                objAddressDTO.Line2 = AAddress.Line2;
                objAddressDTO.City = AAddress.City;
                objAddressDTO.State = AAddress.State;
                objAddressDTO.Country = AAddress.Country;


            }

            return objAddressDTO;
        }
        #endregion

        #region CurrencyDTo
        public static CurrencyDTO CurrencyToCurrencyDTO(Currency ACurrency)
        {
            CurrencyDTO objCurrencyDTO = new CurrencyDTO()
            {
                CurrencyCode = ACurrency.CurrencyCode,
                OrganizationCode = ACurrency.OrganizationCode,
                CurrencyName = ACurrency.CurrencyName,
                Precision = ACurrency.Precision,
                UFCLPRate = ACurrency.UFCLPRate,
                CountryCode = ACurrency.CountryCode,
                IsActive = ACurrency.IsActive,
                IsBlackListed = ACurrency.IsBlackListed,

                CreatedBy = ACurrency.CreatedBy,
                CreatedOn = ACurrency.CreatedOn,
                LastUpdatedBy = ACurrency.LastUpdatedBy,
                LastUpdatedOn = ACurrency.LastUpdatedOn,
                ApprovedBy = ACurrency.ApprovedBy,
                ApprovedOn = ACurrency.ApprovedOn,
                ApprovalStatus = ACurrency.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACurrency.ApprovalStatus)

            };
            return objCurrencyDTO;
        }

        #endregion

        #region OrganizationDTO
        public static OrganizationDTO OrganizationToOrganizationDTO(Organization AOrganization)
        {
            OrganizationDTO objOrganizationDTO = new OrganizationDTO()
            {
                OrganizationCode = AOrganization.OrganizationCode,
                OrganizationDescription = AOrganization.OrganizationDescription,
                EffectiveFromDate = AOrganization.EffectiveFromDate,
                EffectiveToDate = AOrganization.EffectiveToDate,
                OrganizationShortName = AOrganization.OrganizationShortName,
                OrganizationLogo = AOrganization.OrganizationLogo,
                OrganizationTypeCode = AOrganization.OrganizationTypeCode,

                CreatedBy = AOrganization.CreatedBy,
                CreatedOn = AOrganization.CreatedOn,
                LastUpdatedBy = AOrganization.LastUpdatedBy,
                LastUpdatedOn = AOrganization.LastUpdatedOn,
                ApprovedBy = AOrganization.ApprovedBy,
                ApprovedOn = AOrganization.ApprovedOn,
                ApprovalStatus = AOrganization.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AOrganization.ApprovalStatus)
            };
            return objOrganizationDTO;
        }
        #endregion

        #region FranchiseDTO
        public static FranchiseDTO FranchiseToFranchiseDTO(Franchise AFranchise)
        {
            FranchiseDTO objFranchiseDTO = new FranchiseDTO()
            {
                FranchiseCode = AFranchise.FranchiseCode,
                FranchiseName = AFranchise.FranchiseName,
                OrganizationCode = AFranchise.OrganizationCode,
                Allocation = AFranchise.Allocation,

                EffectiveFromDate = AFranchise.EffectiveFromDate,
                EffectiveToDate = AFranchise.EffectiveToDate,

                CreatedBy = AFranchise.CreatedBy,
                CreatedOn = AFranchise.CreatedOn,
                LastUpdatedBy = AFranchise.LastUpdatedBy,
                LastUpdatedOn = AFranchise.LastUpdatedOn,
                ApprovedBy = AFranchise.ApprovedBy,
                ApprovedOn = AFranchise.ApprovedOn,
                ApprovalStatus = AFranchise.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AFranchise.ApprovalStatus),
                OrganizationDesc = (AFranchise.OrganizationCode != null) ? AFranchise.Organization.OrganizationDescription : null,
            };
            return objFranchiseDTO;
        }

        #endregion

        #region SourcingDTO
        public static SourcingDTO SourcingToSourcingDTO(Sourcing ASourcing)
        {
            SourcingDTO objSourcingDTO = new SourcingDTO()
            {
                SourcingChannelCode = ASourcing.SourcingChannelCode,
                OrganizationCode = ASourcing.OrganizationCode,
                SourcingChannelName = ASourcing.SourcingChannelName,
                FloorLimit = ASourcing.FloorLimit,
                CeilingLimit = ASourcing.CeilingLimit,
                LetterGenerationDelayDays = ASourcing.LetterGenerationDelayDays,
                CreatedBy = ASourcing.CreatedBy,
                CreatedOn = ASourcing.CreatedOn,
                LastUpdatedBy = ASourcing.LastUpdatedBy,
                LastUpdatedOn = ASourcing.LastUpdatedOn,
                ApprovedBy = ASourcing.ApprovedBy,
                ApprovedOn = ASourcing.ApprovedOn,
                ApprovalStatus = ASourcing.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ASourcing.ApprovalStatus)
            };
            return objSourcingDTO;
        }
        #endregion

        #region ApprovalQueueDTO
        public static ApprovalQueueDTO ApprovalQueueToApprovalQueueDTO(ApprovalQueue AApprovalQueue)
        {
            ApprovalQueueDTO objApprovalQueueDTO = new ApprovalQueueDTO()
            {
                RequestId = AApprovalQueue.RequestId,
                ContextCode = AApprovalQueue.ContextCode,
                ContextDescription = AApprovalQueue.ApprovalContext.ContextDescription,
                ApprovalDescription = AApprovalQueue.ApprovalDescription,
                ReturnURL = AApprovalQueue.ReturnURL,
                RequestedBy = AApprovalQueue.RequestedBy,
                RequestedOn = AApprovalQueue.RequestedOn,
                ApprovedBy = AApprovalQueue.ApprovedBy,
                ApprovedOn = AApprovalQueue.ApprovedOn,
                ApprovalStatus = AApprovalQueue.ApprovalStatus,
                ApproveAllFlag = AApprovalQueue.ApprovalContext.ApproveAllFlag,

            };
            return objApprovalQueueDTO;

        }
        #endregion

        #region SystemParameterDTO
        public static SystemParameterDTO SystemParameterToSystemParameterDTO(SystemParameter ASystemParameter)
        {
            SystemParameterDTO objSystemParameterDTO = new SystemParameterDTO()
            {
                ParameterId = ASystemParameter.ParameterId,
                OrganizationCode = ASystemParameter.OrganizationCode,
                ParameterName = ASystemParameter.ParameterName,
                ParameterValue = ASystemParameter.ParameterValue,
                Description = ASystemParameter.Description,
                DefaultValue = ASystemParameter.DefaultValue,
                Function = ASystemParameter.FunctionName,
                LowValue = ASystemParameter.LowValue,
                HighValue = ASystemParameter.HighValue,
                CreatedBy = ASystemParameter.CreatedBy,
                CreatedOn = ASystemParameter.CreatedOn,
                LastUpdatedBy = ASystemParameter.LastUpdatedBy,
                LastUpdatedOn = ASystemParameter.LastUpdatedOn,
                ApprovedBy = ASystemParameter.ApprovedBy,
                ApprovedOn = ASystemParameter.ApprovedOn,
                ApprovalStatus = ASystemParameter.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ASystemParameter.ApprovalStatus),
                QueueReferenceNo = ASystemParameter.QueueReferenceNo
            };
            return objSystemParameterDTO;
        }

        #endregion

        #region CreditSegmentDTO
        public static CreditSegmentDTO CreditSegmentToCreditSegmentDTO(CreditSegment ACreditSegment)
        {
            CreditSegmentDTO objCreditSegmentDTO = new CreditSegmentDTO()
            {
                OrganizationCode = ACreditSegment.OrganizationCode,
                CardSegmentCode = ACreditSegment.CardSegmentCode,
                CardSegmentName = ACreditSegment.CardSegmentName,
                LegalEntityCode = ACreditSegment.LegalEntityCode,
                EffectiveFromDate = ACreditSegment.EffectiveFromDate,
                EffectiveToDate = ACreditSegment.EffectiveToDate,
                IsOTBReplenishment = ACreditSegment.IsOTBReplenishment,
                CreatedBy = ACreditSegment.CreatedBy,
                CreatedOn = Utils.BusinessDate,
                LastUpdatedBy = ACreditSegment.LastUpdatedBy,
                LastUpdatedOn =Utils.BusinessDate,
                ApprovedBy = ACreditSegment.ApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = ACreditSegment.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACreditSegment.ApprovalStatus),
                LegalEntityTypeDesc = (ACreditSegment.LegalEntityCode != null) ? ACreditSegment.LegalEntity.Description : null
            };
            return objCreditSegmentDTO;
        }
        #endregion

        #region CreditSubSegmentDTO
        public static CreditSubSegmentDTO CreditSubSegmentToCreditSubSegmentDTO(CreditSubSegment ACreditSubSegment)
        {
            CreditSubSegmentDTO objCreditSubSegmentDTO = new CreditSubSegmentDTO()
            {
                CreditSegmentCode = ACreditSubSegment.CreditSegmentCode,
                CreditSubSegmentCode = ACreditSubSegment.CreditSubSegmentCode,
                Description = ACreditSubSegment.Description,
                CreatedBy = ACreditSubSegment.CreatedBy,
                CreatedOn = Utils.BusinessDate,
                LastUpdatedBy = ACreditSubSegment.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate,
                ApprovedBy = ACreditSubSegment.ApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = ACreditSubSegment.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACreditSubSegment.ApprovalStatus),

                CreditSegmentName = ACreditSubSegment.objCreditSegment.CardSegmentName,
                CreditSubSegmentName = ACreditSubSegment.objSystemParameter.DefaultValue + " - " + ACreditSubSegment.objSystemParameter.Description

            };
            return objCreditSubSegmentDTO;
        }
        #endregion

        #region CityDTO
        public static CityDTO CityToCityDTO(City ACity)
        {
            CityDTO objCityDTO = new CityDTO()
            {
                CityCode = ACity.CityCode,
                OrganizationCode = ACity.OrganizationCode,
                CityName = ACity.CityName,
                CountryCode = ACity.CountryCode,
                RegionCode = ACity.RegionCode,
                CityCategoryCode = ACity.CityCategoryCode,
                HubCode = ACity.HubCode,
                SpokeCode = ACity.SpokeCode,
                CommunaCode = ACity.CommunaCode,
                CommunaName = ACity.CommunaName,

                CreatedBy = ACity.CreatedBy,
                CreatedOn = ACity.CreatedOn,
                LastUpdatedBy = ACity.LastUpdatedBy,
                LastUpdatedOn = ACity.LastUpdatedOn,
                ApprovedBy = ACity.ApprovedBy,
                ApprovedOn = ACity.ApprovedOn,
                ApprovalStatus = ACity.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACity.ApprovalStatus),

                CityCategoryDesc = (ACity.CityCategoryCode != null) ? ACity.CityCategory.Description : null,
                CountryDesc = (ACity.CountryCode != null) ? ACity.Country.CountryName : null,
                RegionDesc = (ACity.RegionCode != null) ? ACity.Region.RegionName : null,
                HubDesc = (ACity.HubCode != null) ? ACity.Hub.HubName : null,
                SpokeDesc = (ACity.SpokeCode != null) ? ACity.Spoke.SpokeName : null,
                CommunaDesc = (ACity.CommunaCode != null) ? ACity.Communa.CommunaName : null,

            };
            return objCityDTO;
        }
        #endregion

        #region CompanyDTO
        public static CompanyDTO CompanyToCompanyDTO(Company ACompany)
        {
            CompanyDTO objCompanyDTO = new CompanyDTO()
            {
                CompanyCode = ACompany.CompanyCode,
                OrganizationCode = ACompany.OrganizationCode,
                CompanyName = ACompany.CompanyName,
                CompanyCategoryCode = ACompany.CompanyCategoryCode,
                IsBlackListed = ACompany.IsBlackListed,
                EffectiveFromDate = ACompany.EffectiveFromDate,
                EffectiveToDate = ACompany.EffectiveToDate.Date,
                PricingAmount = ACompany.PricingAmount,
                TML = ACompany.TML,
                ProductCategoryCode = ACompany.ProductCategoryCode,
                IsAffinity = ACompany.IsAffinity,
                CreatedBy = ACompany.CreatedBy,
                CreatedOn = ACompany.CreatedOn,
                LastUpdatedBy = ACompany.LastUpdatedBy,
                LastUpdatedOn = ACompany.LastUpdatedOn,
                ApprovedBy = ACompany.ApprovedBy,
                ApprovedOn = ACompany.ApprovedOn,
                ApprovalStatus = ACompany.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACompany.ApprovalStatus),

                CompanyCategoryDesc = (ACompany.CompanyCategoryCode != null) ? ACompany.CompanyCategory.Description : null,
                ProductCategoryDesc = (ACompany.ProductCategoryCode != null) ? ACompany.ProductCategory.Description : null

            };
            return objCompanyDTO;
        }
        #endregion

        #region DesignationDTO
        public static DesignationDTO DesignationToDesignationDTO(Designation ADesignation)
        {
            DesignationDTO objDesignationDTO = new DesignationDTO()
            {
                DesignationCode = ADesignation.DesignationCode,
                OrganizationCode = ADesignation.OrganizationCode,
                DesignationDescription = ADesignation.DesignationDescription,
                Position = ADesignation.Position,
                NTP = ADesignation.NTP,
                HRP = ADesignation.HRP,
                Rank = ADesignation.Rank,
                EffectiveFromDate = ADesignation.EffectiveFromDate,
                EffectiveToDate = ADesignation.EffectiveToDate,

                CreatedBy = ADesignation.CreatedBy,
                CreatedOn = ADesignation.CreatedOn,
                LastUpdatedBy = ADesignation.LastUpdatedBy,
                LastUpdatedOn = ADesignation.LastUpdatedOn,
                ApprovalStatus = ADesignation.ApprovalStatus,
                ApprovedBy = ADesignation.ApprovedBy,
                ApprovedOn = ADesignation.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ADesignation.ApprovalStatus),
                PositionDesc = (ADesignation.Position != null) ? ADesignation.Positions.Name : null

            };
            return objDesignationDTO;
        }

        #endregion

        #region CustomerSegmentDTO
        public static CustomerSegmentDTO CustomerSegmentToCustomerSegmentDTO(CustomerSegment ACustomerSegment)
        {
            CustomerSegmentDTO objCustomerSegmentDTO = new CustomerSegmentDTO()
            {
                SegmentCode = ACustomerSegment.SegmentCode,
                OrganizationCode = ACustomerSegment.OrganizationCode,
                SegmentName = ACustomerSegment.SegmentName,
                SegementDescription = ACustomerSegment.SegementDescription,
                IsDefault = ACustomerSegment.IsDefault,
                CreatedBy = ACustomerSegment.CreatedBy,
                CreatedOn = Utils.BusinessDate,
                LastUpdatedBy = ACustomerSegment.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate,
                ApprovedBy = ACustomerSegment.ApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = ACustomerSegment.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACustomerSegment.ApprovalStatus)
            };
            return objCustomerSegmentDTO;
        }
        #endregion

        #region CustomerSubSegmentDTO
        public static CustomerSubSegmentDTO CustomerSubSegmentToCustomerSubSegmentDTO(CustomerSubSegment ACustomerSubSegment)
        {
            CustomerSubSegmentDTO objCustomerSubSegmentDTO = new CustomerSubSegmentDTO()
            {
                SubSegmentCode = ACustomerSubSegment.SubSegmentCode,
                OrganizationCode = ACustomerSubSegment.OrganizationCode,
                SubSegmentName = ACustomerSubSegment.SubSegmentName,
                SubSegmentDesc = ACustomerSubSegment.SubSegmentDesc,
                IsDefault = ACustomerSubSegment.IsDefault,
                CreatedBy = ACustomerSubSegment.CreatedBy,
                CreatedOn = Utils.BusinessDate,
                LastUpdatedBy = ACustomerSubSegment.LastUpdatedBy,
                LastUpdatedOn = Utils.BusinessDate,
                ApprovedBy = ACustomerSubSegment.ApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = ACustomerSubSegment.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACustomerSubSegment.ApprovalStatus)
            };
            return objCustomerSubSegmentDTO;
        }
        #endregion

        #region CardStatusDTO
        public static CardStatusDTO CardStatusToCardStatusDTO(CardStatus ACardStatus)
        {
            CardStatusDTO objCardStatusDTO = new CardStatusDTO()
            {
                CardStatusCode = ACardStatus.CardStatusCode,
                OrganizationCode = ACardStatus.OrganizationCode,
                DecisionCode = ACardStatus.DecisionCode,
                Narration = ACardStatus.Narration,

                CreatedBy = ACardStatus.CreatedBy,
                CreatedOn = ACardStatus.CreatedOn,
                LastUpdatedBy = ACardStatus.LastUpdatedBy,
                LastUpdatedOn = ACardStatus.LastUpdatedOn,
                ApprovalStatus = ACardStatus.ApprovalStatus,
                ApprovedBy = ACardStatus.ApprovedBy,
                ApprovedOn = ACardStatus.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACardStatus.ApprovalStatus),
                DecisionDesc = (ACardStatus.DecisionCode != null) ? ACardStatus.Decision.DecisionName : null,
            };
            return objCardStatusDTO;
        }

        #endregion

        #region GeneralDTO
        public static GeneralDTO GeneralToGeneralDTO(General AGeneral)
        {
            GeneralDTO objGeneralDTO = new GeneralDTO()
            {
                KeyName = AGeneral.KeyName,
                OrganizationCode = AGeneral.OrganizationCode,
                Value1 = AGeneral.Value1,
                Value2 = AGeneral.Value2,
                EffectiveFromDate = AGeneral.EffectiveFromDate,
                EffectiveToDate = AGeneral.EffectiveToDate,
                CreatedBy = AGeneral.CreatedBy,
                CreatedOn = AGeneral.CreatedOn,
                LastUpdatedBy = AGeneral.LastUpdatedBy,
                LastUpdatedOn = AGeneral.LastUpdatedOn,
                ApprovalStatus = AGeneral.ApprovalStatus,
                ApprovedBy = AGeneral.ApprovedBy,
                ApprovedOn = AGeneral.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AGeneral.ApprovalStatus)
            };
            return objGeneralDTO;
        }
        #endregion

        #region ContractDTO
        public static ContractDTO ContractToContractDTO(Contract AContract)
        {
            ContractDTO objContractDTO = new ContractDTO()
            {
                ContractCode = AContract.ContractCode,
                OrganizationCode = AContract.OrganizationCode,
                ContractDescription = AContract.ContractDescription,
                //ProductCode = AContract.ProductCode,
                ContractTemplateCode = AContract.ContractTemplateCode,
                EffectiveFromDate = AContract.EffectiveFromDate,
                EffectiveToDate = AContract.EffectiveToDate,
                NotaryName = AContract.NotaryName,
                NotarySignDate = AContract.NotarySignDate,
                Repertoire = AContract.Repertoire,
                CreatedBy = AContract.CreatedBy,
                CreatedOn = AContract.CreatedOn,
                ApprovedBy = AContract.ApprovedBy,
                ApprovedOn = AContract.ApprovedOn,
                LastUpdatedBy = AContract.LastUpdatedBy,
                LastUpdatedOn = AContract.LastUpdatedOn,
                ApprovalStatus = AContract.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AContract.ApprovalStatus),
                //ProductDesc = (AContract.ProductCode != null) ? AContract.ProductCode + " - " + AContract.Product.ProductName : null,                
                ContractTemplateDesc = (AContract.ContractTemplateCode != null) ? AContract.ContractTemplate.ContractTemplateDescription : null
            };
            return objContractDTO;
        }
        #endregion

        #region ProductTypeDTO
        public static ProductTypeDTO ProductTypeToProductTypeDTO(ProductType AProductType)
        {
            ProductTypeDTO objProductTypeDTO = new ProductTypeDTO()
            {
                ProductTypeCode = AProductType.ProductTypeCode,
                OrganizationCode = AProductType.OrganizationCode,
                Description = AProductType.Description,
                CreatedBy = AProductType.CreatedBy,
                CreatedOn = AProductType.CreatedOn,
                ApprovedBy = AProductType.ApprovedBy,
                ApprovedOn = AProductType.ApprovedOn,
                ApprovalStatus = AProductType.ApprovalStatus,
                LastUpdatedBy = AProductType.LastUpdatedBy,
                LastUpdatedOn = AProductType.LastUpdatedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AProductType.ApprovalStatus)
            };
            return objProductTypeDTO;
        }
        #endregion

        #region TransactionDTO
        public static TransactionDTO TransactionToTransactionDTO(CC_Ingenio .Core.MasterRepo.MasterAggregate. Transaction ATransaction)
        {
            TransactionDTO objTransactionDTO = new TransactionDTO()
            {
                TransactionCode = ATransaction.TransactionCode,
                Description = ATransaction.Description,
                OrganizationCode = ATransaction.OrganizationCode,
                CreditSegment = ATransaction.CreditSegment,
                InstallmentRequired = ATransaction.InstallmentRequired,
                InterestWith = ATransaction.InterestWith,
                DeferredPayment = ATransaction.DeferredPayment,
                FeesRequired = ATransaction.FeesRequired,
                TaxesRequired = ATransaction.TaxesRequired,
                InstallmentRelated = ATransaction.InstallmentRelated,
                CapitalizeInterest = ATransaction.CapitalizeInterest,

                CreatedBy = ATransaction.CreatedBy,
                CreatedOn = ATransaction.CreatedOn,
                ApprovedBy = ATransaction.ApprovedBy,
                ApprovedOn = ATransaction.ApprovedOn,
                ApprovalStatus = ATransaction.ApprovalStatus,
                LastUpdatedBy = ATransaction.LastUpdatedBy,
                LastUpdatedOn = ATransaction.LastUpdatedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ATransaction.ApprovalStatus)
            };
            return objTransactionDTO;
        }
        #endregion

        #region SystemMaintenanceConfigurationDTO
        public static SystemMaintenanceConfigurationDTO SystemMaintenanceConfigurationToSystemMaintenanceConfigurationDTO(SystemMaintenanceConfiguration ASystemMaintenanceConfiguration)
        {
            SystemMaintenanceConfigurationDTO objSystemMaintenanceConfigurationDTO = new SystemMaintenanceConfigurationDTO()
            {
                BaseCurrency = ASystemMaintenanceConfiguration.BaseCurrency,
                WeeklyGlobalCount = ASystemMaintenanceConfiguration.WeeklyGlobalCount,
                WeeklySetUp = ASystemMaintenanceConfiguration.WeeklySetUp,
                CashLimit = ASystemMaintenanceConfiguration.CashLimit,
                CreditLimit = ASystemMaintenanceConfiguration.CreditLimit,
                DefaultBoost = ASystemMaintenanceConfiguration.DefaultBoost,
                ReissueDamaged = ASystemMaintenanceConfiguration.ReissueDamaged,
                ClosedDamaged = ASystemMaintenanceConfiguration.ClosedDamaged,
                DefaultMerchantChargeType = ASystemMaintenanceConfiguration.DefaultMerchantChargeType,
                DefaultChargeTypeDescrption = ASystemMaintenanceConfiguration.DefaultChargeTypeDescrption,
                MinimumDueMerchantCode = ASystemMaintenanceConfiguration.MinimumDueMerchantCode,
                NumberOfSegments = ASystemMaintenanceConfiguration.NumberOfSegments,
                CreatedBy = ASystemMaintenanceConfiguration.CreatedBy,
                CreatedOn = ASystemMaintenanceConfiguration.CreatedOn,
                ApprovedBy = ASystemMaintenanceConfiguration.ApprovedBy,
                ApprovedOn = ASystemMaintenanceConfiguration.ApprovedOn,
                ApprovalStatus = ASystemMaintenanceConfiguration.ApprovalStatus,
                LastUpdatedBy = ASystemMaintenanceConfiguration.LastUpdatedBy,
                LastUpdatedOn = ASystemMaintenanceConfiguration.LastUpdatedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ASystemMaintenanceConfiguration.ApprovalStatus)
            };
            return objSystemMaintenanceConfigurationDTO;
        }
        #endregion

        #region BoostMaintenanceDTO
        public static BoostMaintenanceDTO BoostMaintenanceToBoostMaintenanceDTO(BoostMaintenance ABoostMaintenance)
        {
            BoostMaintenanceDTO objBoostMaintenanceDTO = new BoostMaintenanceDTO()
            {
                BoostId = ABoostMaintenance.BoostId,
                OrganizationCode = ABoostMaintenance.OrganizationCode,
                Percentage = ABoostMaintenance.Percentage,
                TransactionCode = ABoostMaintenance.TransactionCode,
                ProductCode = ABoostMaintenance.ProductCode,
                CreatedBy = ABoostMaintenance.CreatedBy,
                CreatedOn = ABoostMaintenance.CreatedOn,
                ApprovedBy = ABoostMaintenance.ApprovedBy,
                ApprovedOn = ABoostMaintenance.ApprovedOn,
                ApprovalStatus = ABoostMaintenance.ApprovalStatus,
                LastUpdatedBy = ABoostMaintenance.LastUpdatedBy,
                LastUpdatedOn = ABoostMaintenance.LastUpdatedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ABoostMaintenance.ApprovalStatus),
                ProductName = (ABoostMaintenance.ProductCode != null) ? ABoostMaintenance.ProductCode + " - " + ABoostMaintenance.Product.ProductName : null,
                TransactionDesc = (ABoostMaintenance.TransactionCode != null) ? ABoostMaintenance.Transaction.Description : null,
            };
            return objBoostMaintenanceDTO;
        }
        #endregion

        #region FeeToFeeDTO
        public static FeeDTO FeeToFeeDTO(Fee AFee)
        {
            FeeDTO objFeeDTO = new FeeDTO()
            {
                FeeCode = AFee.FeeCode,
                OrganizationCode = AFee.OrganizationCode,
                ProductCode = AFee.ProductCode,
                ProductMemberTypeCode = AFee.ProductMemberTypeCode,
                CustomerSegmentCode = AFee.CustomerSegmentCode,
                CustomerSubSegmentCode = AFee.CustomerSubSegmentCode,
                ContractCode = AFee.ContractCode,
                EffectiveFromDate = AFee.EffectiveFromDate,
                EffectiveToDate = AFee.EffectiveToDate,
                CurrencyCode = AFee.CurrencyCode,

                CreatedBy = AFee.CreatedBy,
                CreatedOn = AFee.CreatedOn,
                ApprovedBy = AFee.ApprovedBy,
                ApprovedOn = AFee.ApprovedOn,
                LastUpdatedBy = AFee.LastUpdatedBy,
                LastUpdatedOn = AFee.LastUpdatedOn,
                ApprovalStatus = AFee.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AFee.ApprovalStatus),

                ProductName = AFee.Product.ProductName,
                CustomerSegmentName = AFee.CustomerSegment.SegmentName,
                CustomerSubSegmentName = AFee.CustomerSubSegment.SubSegmentName,
                ContractDescription = AFee.Contract.ContractDescription
            };

            objFeeDTO.FeeTypeDTO.AddRange(FeeTypeToFeeTypeDTO(AFee.FeeType));

            return objFeeDTO;
        }

        #region FeeTypeToFeeTypeDTO
        public static List<FeeTypeDTO> FeeTypeToFeeTypeDTO(List<FeeType> AFeeType)
        {
            List<FeeTypeDTO> lstFeeTypeDTO = new List<FeeTypeDTO>();
            AFeeType.ForEach(x => lstFeeTypeDTO.Add(new FeeTypeDTO()
            {
                FeeCode = x.FeeCode,
                OrganizationCode = x.OrganizationCode,
                FeeTypeCode = x.FeeTypeCode,
                FeeAmount = x.FeeAmount,
                MonthsFrom = x.MonthsFrom,
                ReasonCode = x.ReasonCode,
                FirstYearAmount = x.FirstYearAmount,
                RenwalFee1 = x.RenwalFee1,
                RenwalFee2 = x.RenwalFee2,
                RenwalFee3 = x.RenwalFee3,
                RenwalFee4 = x.RenwalFee4,
                RenwalFee5 = x.RenwalFee5,
                RenwalFee6 = x.RenwalFee6,
                AdjustCode = x.AdjustCode,
                DiscountRatePercent = x.DiscountRatePercent,
                Period = x.Period,
                FlatFee = x.FlatFee,
                MinFee = x.MinFee,
                Flag = x.Flag
            }));
            return lstFeeTypeDTO;
        }
        #endregion
        #endregion

        #region ProductDTO
        public static ProductDTO ProductToProductDTO(Product AProduct)
        {
            ProductDTO objProductDTO = new ProductDTO()
            {
                ProductCode = AProduct.ProductCode,
                OrganizationCode = AProduct.OrganizationCode,
                ProductName = AProduct.ProductName,
                ProductCategoryCode = AProduct.ProductCategoryCode,
                ProductSubCategoryCode = AProduct.ProductSubCategoryCode,
                ProductClassCode = AProduct.ProductClassCode,
                CardNoPattern = AProduct.CardNoPattern,
                CardNoLength = AProduct.CardNoLength,
                ProductFranchiseCode = AProduct.ProductFranchise,
                RevolverChargeFlag = AProduct.RevolverChargeFlag,
                MinimumCreditLine = AProduct.MinimumCreditLine,
                MaximumCreditLine = AProduct.MaximumCreditLine,
                CashLimit = Math.Truncate(AProduct.CashLimit),
                CurrencyCode = AProduct.CurrencyCode,
                ProductMemberTypeCode = AProduct.ProductMemberTypeCode,
                BinValue = AProduct.BinValue,
                SubBinValue = AProduct.SubBinValue,
                ProductExpiryInMonths = AProduct.ProductExpiryInMonths,
                ProductStatusCode = AProduct.ProductStatusCode,
                //RenewalMonth = AProduct.RenewalMonth,
                ServiceCode = AProduct.ServiceCode,
                PaymentAllocationType = AProduct.PaymentAllocationType,
                InterestMethodCode = AProduct.InterestMethodCode,
                CapitalizedInterest = AProduct.CapitalizedInterest,
                MinimumConsumptionAmount = AProduct.MinimumConsumptionAmount,
                BillCycleSysParamId = AProduct.BillCycleSysParamId,
                BillingCycleCode = AProduct.BillingCycleCode,
                //AutoRenewalOption = AProduct.AutoRenewalOption,
                MaximumAddOnCount = AProduct.MaximumAddOnCount,
                CanEmbossPhoto = AProduct.CanEmbossPhoto,
                CanEmbossSignature = AProduct.CanEmbossSignature,
                CanEmbossLogo = AProduct.CanEmbossLogo,
                ImageRefNo = AProduct.ImageRefNo,
                /*DefaultAffinityCode = AProduct.DefaultAffinityCode,*/
                NumberofAffinity = AProduct.NumberofAffinity,
                InterestDiffFlag = AProduct.InterestDiffFlag,
                InternalScore = AProduct.InternalScore,
                ExternalScore = AProduct.ExternalScore,
                CPVFlag = AProduct.CPVFlag,
                CombinedScore = AProduct.CombinedScore,
                ExternalScoreDefinitionId = AProduct.ExternalScoreDefinitionId,
                MinimumCombinedScore = AProduct.MinimumCombinedScore,
                MinimumCardAdvance = AProduct.MinimumCardAdvance,
                MaximumCardAdvance = AProduct.MaximumCardAdvance,
                InternalScoreDefinitionId = AProduct.InternalScoreDefinitionId,
                ScoreFlag = AProduct.ScoreFlag,
                RenewalScoreFlag = AProduct.RenewalScoreFlag,
                CPPLine = AProduct.CPPLine,
                VerificationNeeded = AProduct.VerificationNeeded,
                IsPreviewNeeded = AProduct.IsPreviewNeeded,
                MasterVerificationNeeded = AProduct.MasterVerificationNeeded,
                DeDuplicationOption = AProduct.DeDuplicationOption,

                InterestWaiverFlag = AProduct.InterestWaiverFlag,
                BackgroundImageEmbossFlag = AProduct.BackgroundImageEmbossFlag,
                ProductClassIndicator = AProduct.ProductClassIndicator,
                AutoApprovalFlag = AProduct.AutoApprovalFlag,
                ContractCode = AProduct.ContractCode,
                LegacyProductCode = AProduct.LegacyProductCode,

                AssignedList = new List<string>(AProduct.PaymentAllocationType.Split(',')),
                MemberTypeAssignedList = new List<string>(AProduct.ProductMemberTypeCode.Split(',')),

                CreatedBy = AProduct.CreatedBy,
                CreatedOn = AProduct.CreatedOn,
                ApprovedBy = AProduct.ApprovedBy,
                ApprovedOn = AProduct.ApprovedOn,
                LastUpdatedBy = AProduct.LastUpdatedBy,
                LastUpdatedOn = AProduct.LastUpdatedOn,
                ApprovalStatus = AProduct.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AProduct.ApprovalStatus),

                CurrencyDesc = (AProduct.CurrencyCode != null) ? AProduct.Currency.CurrencyName : null,
                BillingCycleDesc = (AProduct.BillCycleSysParamId != 0) ? AProduct.BillingCycle.Description : null,
                /*DefaultAffinityDesc = (AProduct.DefaultAffinityCode != null) ? AProduct.DefaultAffinity.Description : null,*/
                InterestMethodDesc = (AProduct.InterestMethodCode != null) ? AProduct.InterestMethod.Description : null,
                ProductClassDesc = (AProduct.ProductClassCode != null) ? AProduct.ProductClass.Description : null,
                //ProductMemberTypeDesc = (AProduct.ProductMemberTypeCode != null) ? AProduct.ProductMemberType.Description : null,
                //ProductStatusDesc = (AProduct.ProductStatusCode != null) ? AProduct.ProductStatus.Description : null,
                ProductCategoryDesc = (AProduct.ProductCategoryCode != null) ? AProduct.ProductCategory.Description : null,
                ProductSubCategoryDesc = (AProduct.ProductSubCategoryCode != null) ? AProduct.ProductSubCategory.Description : null,
                ServiceDesc = (AProduct.ServiceCode != null) ? AProduct.Service.Description : null,

            };
            return objProductDTO;
        }
        #endregion

        #region BlockCodeDTO
        public static BlockCodeDTO BlockCodeToBlockCodeDTO(BlockCode ABlockCode)
        {
            BlockCodeDTO objBlockCodeDTO = new BlockCodeDTO()
            {
                Code = ABlockCode.Code,
                OrganizationCode = ABlockCode.OrganizationCode,
                Description = ABlockCode.Description,
                ShortDescription = ABlockCode.ShortDescription,
                IsAccountBlocked = ABlockCode.IsAccountBlocked,
                IsAccountBlockedForFinancialReasons = ABlockCode.IsAccountBlockedForFinancialReasons,
                IsAccountBlockedForNonFinancialReasons = ABlockCode.IsAccountBlockedForNonFinancialReasons,
                FinancialHierarchy = ABlockCode.FinancialHierarchy,
                NonFinancialHierarchy = ABlockCode.NonFinancialHierarchy,
                IsCardBlocked = ABlockCode.IsCardBlocked,
                CardBlockHierarchy = ABlockCode.CardBlockHierarchy,
                IsPermanantBlock = ABlockCode.IsPermanantBlock,
                PlasticRecoveryNeeded = ABlockCode.PlasticRecoveryNeeded,
                InstallmentClose = ABlockCode.InstallmentClose,
                Unblock = ABlockCode.Unblock,

                CreatedBy = ABlockCode.CreatedBy,
                CreatedOn = ABlockCode.CreatedOn,
                ApprovedBy = ABlockCode.ApprovedBy,
                ApprovedOn = ABlockCode.ApprovedOn,
                LastUpdatedBy = ABlockCode.LastUpdatedBy,
                LastUpdatedOn = ABlockCode.LastUpdatedOn,
                ApprovalStatus = ABlockCode.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ABlockCode.ApprovalStatus)
            };
            return objBlockCodeDTO;
        }
        #endregion

        #region CustomerToCustomerDTO
        public static CustomerDTO CustomerToCustomerDTO(Customer ACustomer)
        {
            CustomerDTO objCustomerDTO = new CustomerDTO()
            {
                CustomerId = ACustomer.CustomerId,
                SalesId = ACustomer.SalesId,

                ParentCustomerId = ACustomer.ParentCustomerId,
                BranchCode = ACustomer.BranchCode,
                CampaignSource = ACustomer.CampaignSource,
                ChannelCode = ACustomer.CampaignSource,
                ApplicationReferenceNo = ACustomer.ApplicationReferenceNo,
                OrganizationCode = ACustomer.OrganizationCode,
                ProductMemberTypeCode = ACustomer.ProductMemberTypeCode,
                ProductTypeCode = ACustomer.ProductTypeCode,
                ProductCategoryCode = ACustomer.ProductCategoryCode,
                ProductCode = ACustomer.ProductCode,
                //RUT = ACustomer.RUT,
                RUT = (ACustomer.RUT == null) ? "" : ACustomer.RUT.Insert(ACustomer.RUT.Length - 1, "-"),
                SerialNo = ACustomer.SerialNo,
                ExpiryDate = ACustomer.ExpiryDate,
                CorporateRUT = ACustomer.CorporateRUT,
                //CorporateRUT = (ACustomer.CorporateRUT == null) ? "" : ACustomer.CorporateRUT.Insert(ACustomer.CorporateRUT.Length - 1, "-"),
                PhysicalApplicationRefNo = ACustomer.PhysicalApplicationRefNo,
                PhysicalApplicationRefDate = ACustomer.PhysicalApplicationRefDate,
                ExternalCivilRegistryResult = ACustomer.ExternalCivilRegistryResult,
                InputterId = ACustomer.InputterId,
                InputterDate = ACustomer.InputterDate,
                CreditScore = ACustomer.CreditScore,

                CustomerType = ACustomer.CustomerType,
                Name = ACustomer.Name,
                PaternalSurname = ACustomer.PaternalSurname,
                MaternalSurname = ACustomer.MaternalSurname,
                Nationality = ACustomer.Nationality,
                DateOfBirth = ACustomer.DateOfBirth,
                Gender = ACustomer.Gender,
                MaritalStatus = ACustomer.MaritalStatus,
                NumberofDependents = ACustomer.NumberofDependents,
                Education = ACustomer.Education,
                Profession = ACustomer.Profession,
                Housing = ACustomer.Housing,
                EmailID = ACustomer.EmailID,
                IsActive = ACustomer.IsActive,
                IsBlackListed = ACustomer.IsBlackListed,
                IsCardGenerated = ACustomer.IsCardGenerated,
                CreatedBy = ACustomer.CreatedBy,
                CreatedOn = ACustomer.CreatedOn,
                ApprovedBy = ACustomer.ApprovedBy,
                ApprovedOn = ACustomer.ApprovedOn,
                ApprovalStatus = ACustomer.ApprovalStatus,
                LastUpdatedBy = ACustomer.LastUpdatedBy,
                LastUpdatedOn = ACustomer.LastUpdatedOn,

                ProductName = (ACustomer.ProductCode != null) ? ACustomer.ProductCode + " - " + ACustomer.Product.ProductName : null,
                CustomerTypeDesc = ACustomer.CustomerType == "C" ? "Company" : "Individual",
                NationalityDesc = ACustomer.Country.Nationality,
                BranchName = ACustomer.Branch.BranchName,
                ProductMemberTypeDesc = ACustomer.ProductMemberType.Description,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACustomer.ApprovalStatus),
                LegacyCardNumber = ACustomer.LegacyCardNumber,
                AutoApprove = ACustomer.Product.AutoApprovalFlag
            };

            objCustomerDTO.CustomerOccupationDTO.AddRange(CustomerOccupationToCustomerOccupationDTO(ACustomer.CustomerOccupation));
            objCustomerDTO.CustomerCommericalHouseConsumptionDTO.AddRange(CustomerCommericalHouseConsumptionToCustomerCommericalHouseConsumptionDTO(ACustomer.CustomerCommericalHouseConsumption));
            objCustomerDTO.CustomerBillingDetailDTO.AddRange(CustomerBillingDetailToCustomerBillingDetailDTO(ACustomer.CustomerBillingDetail));
            objCustomerDTO.CustomerDecisionDTO.AddRange(CustomerDecisionToCustomerDecisionDTO(ACustomer.CustomerDecision));
            objCustomerDTO.CustomerDependentDTO.AddRange(CustomerDependentToCustomerDependentDTO(ACustomer.CustomerDependent));
            objCustomerDTO.CustomerAddressDTO.AddRange(CustomerAddressToCustomerAddressDTO(ACustomer.CustomerAddress));
            objCustomerDTO.CustomerCreditCardDetailDTO.AddRange(CustomerCreditCardDetailToCustomerCreditCardDetailDTO(ACustomer.CustomerCreditCardDetail));
            objCustomerDTO.CustomerCurrentAccountDetailDTO.AddRange(CustomerCurrentAccountDetailToCustomerCurrentAccountDetailDTO(ACustomer.CustomerCurrentAccountDetail));

            return objCustomerDTO;
        }


        #region CustomerOccupationToCustomerOccupationDTO
        public static List<CustomerOccupationDTO> CustomerOccupationToCustomerOccupationDTO(List<CustomerOccupation> ACustomerOccupation)
        {
            List<CustomerOccupationDTO> lstCustomerOccupationDTO = new List<CustomerOccupationDTO>();

            ACustomerOccupation.ForEach(x => lstCustomerOccupationDTO.Add(
                new CustomerOccupationDTO()
                {
                    CustomerId = x.CustomerId,
                    CompanyRUT = string.IsNullOrWhiteSpace(x.CompanyRUT) ? string.Empty : x.CompanyRUT,
                    CompanyName = string.IsNullOrWhiteSpace(x.CompanyName) ? string.Empty : x.CompanyName,
                    OccupationId = x.OccupationId,
                    DateOfJoining = x.DateOfJoining,
                    WorkType = x.WorkType,
                    Post = x.Post,
                    IncomeType = x.IncomeType,
                    TotalMonthlyIncome = x.TotalMonthlyIncome == null ? 0 : x.TotalMonthlyIncome,
                    IsIncomeProofAvailable = x.IsIncomeProofAvailable
                }));

            return lstCustomerOccupationDTO;
        }
        #endregion

        #region CustomerCommericalHouseConsumptionToCustomerCommericalHouseConsumptionDTO
        public static List<CustomerCommericalHouseConsumptionDTO> CustomerCommericalHouseConsumptionToCustomerCommericalHouseConsumptionDTO(List<CustomerCommericalHouseConsumption> ACustomerCommericalHouseConsumption)
        {
            List<CustomerCommericalHouseConsumptionDTO> lstCustomerCommericalHouseConsumptionDTO = new List<CustomerCommericalHouseConsumptionDTO>();

            ACustomerCommericalHouseConsumption.ForEach(x => lstCustomerCommericalHouseConsumptionDTO.Add(
                new CustomerCommericalHouseConsumptionDTO()
                {
                    CommericalId = x.CommericalId,
                    CustomerId = x.CustomerId,
                    Type = x.Type,
                    Name = x.Name,
                    CardNumber = x.CardNumber,
                    AccountStatus = x.AccountStatus
                }));

            return lstCustomerCommericalHouseConsumptionDTO;
        }
        #endregion

        #region CustomerBillingDetailToCustomerBillingDetailDTO
        public static List<CustomerBillingDetailDTO> CustomerBillingDetailToCustomerBillingDetailDTO(List<CustomerBillingDetail> ACustomerBillingDetail)
        {
            List<CustomerBillingDetailDTO> lstCustomerBillingDetailDTO = new List<CustomerBillingDetailDTO>();
            ACustomerBillingDetail.ForEach(x => lstCustomerBillingDetailDTO.Add(new CustomerBillingDetailDTO()
            {
                BillingDetailId = x.BillingDetailId,
                CustomerId = x.CustomerId,
                IsApplicationSignedOffByCustomer = x.IsApplicationSignedOffByCustomer,
                SendAccountStatementTo = x.SendAccountStatementTo,
                AccountNo = string.IsNullOrWhiteSpace(x.AccountNo) ? string.Empty : x.AccountNo,
                BranchCode = string.IsNullOrWhiteSpace(x.BranchCode) ? string.Empty : x.BranchCode
                //IsECSRequried = x.IsECSRequried,
                //PaymentDate = x.PaymentDate,
                //ECSFor = x.ECSFor,
                //BankCode = x.BankCode
            }));

            return lstCustomerBillingDetailDTO;
        }
        #endregion

        #region CustomerDecisionToCustomerDecisionDTO
        public static List<CustomerDecisionDTO> CustomerDecisionToCustomerDecisionDTO(List<CustomerDecision> ACustomerDecision)
        {
            List<CustomerDecisionDTO> lstCustomerDecisionDTO = new List<CustomerDecisionDTO>();

            ACustomerDecision.ForEach(x => lstCustomerDecisionDTO.Add(new CustomerDecisionDTO()
            {
                DecisionID = x.DecisionID,
                CustomerId = x.CustomerId,
                DecisionCode = x.DecisionCode,
                ReasonCode = x.ReasonCode,
                Remarks = string.IsNullOrWhiteSpace(x.Remarks) ? string.Empty : x.Remarks,
                ContractCode = x.ContractCode,
                Unemployment = string.IsNullOrWhiteSpace(x.Unemployment) ? string.Empty : x.Unemployment,
                IsContractAcceptedByCustomer = x.IsContractAcceptedByCustomer,
                CreditLimit = x.CreditLimit
            }));
            return lstCustomerDecisionDTO;
        }
        #endregion

        #region CustomerDependentToCustomerDependentDTO
        public static List<CustomerDependentDTO> CustomerDependentToCustomerDependentDTO(List<CustomerDependent> ACustomerDependent)
        {
            List<CustomerDependentDTO> lstCustomerDependentDTO = new List<CustomerDependentDTO>();

            ACustomerDependent.ForEach(x => lstCustomerDependentDTO.Add(new CustomerDependentDTO()
            {
                DependentID = x.DependentID,
                CustomerId = x.CustomerId,
                NoOfChildren = x.NoOfChildren,
                AgeGroup = x.AgeGroup
            }));
            return lstCustomerDependentDTO;
        }
        #endregion

        #region CustomerAddressToCustomerAddressDTO
        public static List<CustomerAddressDTO> CustomerAddressToCustomerAddressDTO(List<CustomerAddress> ACustomerAddress)
        {
            List<CustomerAddressDTO> lstCustomerAddressDTO = new List<CustomerAddressDTO>();

            ACustomerAddress.ForEach(x => lstCustomerAddressDTO.Add(new CustomerAddressDTO()
            {
                AddressId = x.AddressId,
                AddressTypeCode = x.AddressTypeCode,
                CustomerId = x.CustomerId,
                TypeOfResidenceClearance = x.TypeOfResidenceClearance,
                RegionCode = x.RegionCode,
                CityCode = x.CityCode,
                CommunaCode = x.CommunaCode,
                Street = x.Street,
                Number = x.Number,
                Block = string.IsNullOrWhiteSpace(x.Block) ? string.Empty : x.Block,
                ComplementOfAddress = string.IsNullOrWhiteSpace(x.ComplementOfAddress) ? string.Empty : x.ComplementOfAddress,
                Town = string.IsNullOrWhiteSpace(x.Town) ? string.Empty : x.Town,
                Landmark = string.IsNullOrWhiteSpace(x.Landmark) ? string.Empty : x.Landmark,
                PostalCode = string.IsNullOrWhiteSpace(x.PostalCode) ? string.Empty : x.PostalCode,
                POBox = string.IsNullOrWhiteSpace(x.POBox) ? string.Empty : x.POBox,
                Phone = string.IsNullOrWhiteSpace(x.Phone) ? string.Empty : x.Phone,
                Mobile = string.IsNullOrWhiteSpace(x.Mobile) ? string.Empty : x.Mobile,
                Fax = string.IsNullOrWhiteSpace(x.Fax) ? string.Empty : x.Fax
            }));
            return lstCustomerAddressDTO;
        }
        public static CustomerAddressDTO CustomerAddressToCustomerAddressDTO(CustomerAddress ACustomerAddress)
        {
            CustomerAddressDTO objCustAddressDTO = new CustomerAddressDTO()
            {
                AddressId = ACustomerAddress.AddressId,
                AddressTypeCode = ACustomerAddress.AddressTypeCode,
                CustomerId = ACustomerAddress.CustomerId,
                TypeOfResidenceClearance = ACustomerAddress.TypeOfResidenceClearance,
                RegionCode = ACustomerAddress.RegionCode,
                CityCode = ACustomerAddress.CityCode,
                CommunaCode = ACustomerAddress.CommunaCode,
                Street = ACustomerAddress.Street,
                Number = ACustomerAddress.Number,
                Block = string.IsNullOrWhiteSpace(ACustomerAddress.Block) ? string.Empty : ACustomerAddress.Block,
                ComplementOfAddress = string.IsNullOrWhiteSpace(ACustomerAddress.ComplementOfAddress) ? string.Empty : ACustomerAddress.ComplementOfAddress,
                Town = string.IsNullOrWhiteSpace(ACustomerAddress.Town) ? string.Empty : ACustomerAddress.Town,
                Landmark = string.IsNullOrWhiteSpace(ACustomerAddress.Landmark) ? string.Empty : ACustomerAddress.Landmark,
                PostalCode = string.IsNullOrWhiteSpace(ACustomerAddress.PostalCode) ? string.Empty : ACustomerAddress.PostalCode,
                POBox = string.IsNullOrWhiteSpace(ACustomerAddress.POBox) ? string.Empty : ACustomerAddress.POBox,
                Phone = string.IsNullOrWhiteSpace(ACustomerAddress.Phone) ? string.Empty : ACustomerAddress.Phone,
                Mobile = string.IsNullOrWhiteSpace(ACustomerAddress.Mobile) ? string.Empty : ACustomerAddress.Mobile,
                Fax = string.IsNullOrWhiteSpace(ACustomerAddress.Fax) ? string.Empty : ACustomerAddress.Fax
            };
            return objCustAddressDTO;
        }
        #endregion
        #region UpdatedCustomerAddressToUpdatedCustomerAddressDTO
        public static UpdatedCustomerAddressDTO UpdtdCustomerAddressToUpdtdCustomerAddressDTO(UpdatedCustomerAddress ACustomerAddress)
        {
            UpdatedCustomerAddressDTO objUpdtdCustAddressDTO = new UpdatedCustomerAddressDTO()
            {
                AddressId = ACustomerAddress.AddressId,
                CustomerId = ACustomerAddress.CustomerId,
                CardNumber = ACustomerAddress.CardNumber,
                AddressTypeCode = ACustomerAddress.AddressTypeCode,
                TypeOfResidenceClearance = ACustomerAddress.TypeOfResidenceClearance,
                CountryCode = ACustomerAddress.CountryCode,
                RegionCode = ACustomerAddress.RegionCode,
                CityCode = ACustomerAddress.CityCode,
                CommunaCode = ACustomerAddress.CommunaCode,
                Street = ACustomerAddress.Street,
                Number = ACustomerAddress.Number,
                Block = string.IsNullOrWhiteSpace(ACustomerAddress.Block) ? string.Empty : ACustomerAddress.Block,
                ComplementOfAddress = string.IsNullOrWhiteSpace(ACustomerAddress.ComplementOfAddress) ? string.Empty : ACustomerAddress.ComplementOfAddress,
                Town = string.IsNullOrWhiteSpace(ACustomerAddress.Town) ? string.Empty : ACustomerAddress.Town,
                Landmark = string.IsNullOrWhiteSpace(ACustomerAddress.Landmark) ? string.Empty : ACustomerAddress.Landmark,
                PostalCode = string.IsNullOrWhiteSpace(ACustomerAddress.PostalCode) ? string.Empty : ACustomerAddress.PostalCode,
                POBox = string.IsNullOrWhiteSpace(ACustomerAddress.POBox) ? string.Empty : ACustomerAddress.POBox,
                Phone = string.IsNullOrWhiteSpace(ACustomerAddress.Phone) ? string.Empty : ACustomerAddress.Phone,
                Mobile = string.IsNullOrWhiteSpace(ACustomerAddress.Mobile) ? string.Empty : ACustomerAddress.Mobile,
                Fax = string.IsNullOrWhiteSpace(ACustomerAddress.Fax) ? string.Empty : ACustomerAddress.Fax,
                UpdatedBy = string.IsNullOrWhiteSpace(ACustomerAddress.LastUpdatedBy) ? string.Empty : ACustomerAddress.LastUpdatedBy,
                UpdatedOn = ACustomerAddress.LastUpdatedOn,
                ApprovedBy = string.IsNullOrWhiteSpace(ACustomerAddress.ApprovedBy) ? string.Empty : ACustomerAddress.ApprovedBy,
                ApprovedOn = ACustomerAddress.ApprovedOn,
                ApprovalStatus = string.IsNullOrWhiteSpace(ACustomerAddress.ApprovalStatus) ? string.Empty : ACustomerAddress.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACustomerAddress.ApprovalStatus)
            };
            return objUpdtdCustAddressDTO;
        }
        #endregion
        #region CustomerAddressToUpdatedCustomerAddressDTO
        public static UpdatedCustomerAddressDTO CustomerAddressToUpdtdCustomerAddressDTO(CustomerAddress ACustomerAddress)
        {
            UpdatedCustomerAddressDTO objUpdtdCustAddressDTO = new UpdatedCustomerAddressDTO()
           {
               AddressId = ACustomerAddress.AddressId,
               CustomerId = ACustomerAddress.CustomerId,
               AddressTypeCode = ACustomerAddress.AddressTypeCode,
               TypeOfResidenceClearance = ACustomerAddress.TypeOfResidenceClearance,
               RegionCode = ACustomerAddress.RegionCode,
               CityCode = ACustomerAddress.CityCode,
               CommunaCode = ACustomerAddress.CommunaCode,
               Street = ACustomerAddress.Street,
               Number = ACustomerAddress.Number,
               Block = string.IsNullOrWhiteSpace(ACustomerAddress.Block) ? string.Empty : ACustomerAddress.Block,
               ComplementOfAddress = string.IsNullOrWhiteSpace(ACustomerAddress.ComplementOfAddress) ? string.Empty : ACustomerAddress.ComplementOfAddress,
               Town = string.IsNullOrWhiteSpace(ACustomerAddress.Town) ? string.Empty : ACustomerAddress.Town,
               Landmark = string.IsNullOrWhiteSpace(ACustomerAddress.Landmark) ? string.Empty : ACustomerAddress.Landmark,
               PostalCode = string.IsNullOrWhiteSpace(ACustomerAddress.PostalCode) ? string.Empty : ACustomerAddress.PostalCode,
               POBox = string.IsNullOrWhiteSpace(ACustomerAddress.POBox) ? string.Empty : ACustomerAddress.POBox,
               Phone = string.IsNullOrWhiteSpace(ACustomerAddress.Phone) ? string.Empty : ACustomerAddress.Phone,
               Mobile = string.IsNullOrWhiteSpace(ACustomerAddress.Mobile) ? string.Empty : ACustomerAddress.Mobile,
               Fax = string.IsNullOrWhiteSpace(ACustomerAddress.Fax) ? string.Empty : ACustomerAddress.Fax
           };
            return objUpdtdCustAddressDTO;
        }
        #endregion

        #region CustomerCreditCardDetailToCustomerCreditCardDetailDTO
        public static List<CustomerCreditCardDetailDTO> CustomerCreditCardDetailToCustomerCreditCardDetailDTO(List<CustomerCreditCardDetail> ACustomerCreditCardDetail)
        {
            List<CustomerCreditCardDetailDTO> lstCustomerCreditCardDetailDTO = new List<CustomerCreditCardDetailDTO>();
            ACustomerCreditCardDetail.ForEach(x => lstCustomerCreditCardDetailDTO.Add(new CustomerCreditCardDetailDTO()
            {
                CreditCardId = x.CreditCardId,
                CustomerId = x.CustomerId,
                CardType = string.IsNullOrWhiteSpace(x.CardType) ? string.Empty : x.CardType,
                CardBrand = string.IsNullOrWhiteSpace(x.CardBrand) ? string.Empty : x.CardBrand,
                BankInstitutionId = string.IsNullOrWhiteSpace(x.BankInstitutionId) ? string.Empty : x.BankInstitutionId,
                CardNumber = x.CardNumber,
                EffectiveToDate = string.IsNullOrWhiteSpace(x.EffectiveToDate) ? string.Empty : x.EffectiveToDate,
                CreditLimit = x.CreditLimit
            }));
            return lstCustomerCreditCardDetailDTO;
        }
        #endregion

        #region CustomerCurrentAccountDetailToCustomerCurrentAccountDetailDTO
        public static List<CustomerCurrentAccountDetailDTO> CustomerCurrentAccountDetailToCustomerCurrentAccountDetailDTO(List<CustomerCurrentAccountDetail> ACustomerCurrentAccountDetail)
        {
            List<CustomerCurrentAccountDetailDTO> lstCustomerCurrentAccountDetailDTO = new List<CustomerCurrentAccountDetailDTO>();

            ACustomerCurrentAccountDetail.ForEach(x => lstCustomerCurrentAccountDetailDTO.Add(new CustomerCurrentAccountDetailDTO()
            {
                AccountId = x.AccountId,
                CustomerId = x.CustomerId,
                AccountNumber = x.AccountNumber,
                BankType = x.BankType,
                BankCode = x.BankCode
            }));
            return lstCustomerCurrentAccountDetailDTO;
        }
        #endregion
        #endregion

        #region CalendarDateMaintenanceDTO
        public static CalendarDateMaintenanceDTO CalendarDateMaintenanceToCalendarDateMaintenanceDTO(CalendarDateMaintenance ACalendarDateMaintenance)
        {
            CalendarDateMaintenanceDTO objCalendarDateMaintenanceDTO = new CalendarDateMaintenanceDTO()
            {
                Id = ACalendarDateMaintenance.Id,
                OrganizationCode = ACalendarDateMaintenance.OrganizationCode,
                Type = ACalendarDateMaintenance.Type,
                Date = ACalendarDateMaintenance.Date,
                Description = ACalendarDateMaintenance.Description,

                CreatedBy = ACalendarDateMaintenance.CreatedBy,
                CreatedOn = ACalendarDateMaintenance.CreatedOn,
                LastUpdatedBy = ACalendarDateMaintenance.LastUpdatedBy,
                LastUpdatedOn = ACalendarDateMaintenance.LastUpdatedOn,
                ApprovalStatus = ACalendarDateMaintenance.ApprovalStatus,
                ApprovedBy = ACalendarDateMaintenance.ApprovedBy,
                ApprovedOn = ACalendarDateMaintenance.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACalendarDateMaintenance.ApprovalStatus),
                TypeDesc = ACalendarDateMaintenance.Type == "HOLIDAY" ? "HOLIDAY" : "WORKING DAY"
            };
            return objCalendarDateMaintenanceDTO;
        }
        #endregion

        #region TransactionSubCodeDTO
        public static TransactionSubCodeDTO TransactionSubCodeToTransactionSubCodeDTO(TransactionSubCode ATransactionSubCode)
        {
            TransactionSubCodeDTO objTransactionSubCodeDTO = new TransactionSubCodeDTO()
            {
                TransactionSubCodeId = ATransactionSubCode.TransactionSubCodeId,
                TransactionCode = ATransactionSubCode.TransactionCode,
                SubCode = ATransactionSubCode.SubCode,
                Description = ATransactionSubCode.Description,
                CreditSubSegmentCode = ATransactionSubCode.CreditSubSegmentCode,
                MinDuePercentage = ATransactionSubCode.MinDuePercentage,
                IsConsumption = ATransactionSubCode.IsConsumption,
                IsDebitOrCredit = ATransactionSubCode.IsDebitOrCredit,
                IsIVARequired = ATransactionSubCode.IsIVARequired,
                IncomingTransType = ATransactionSubCode.IncomingTransType,
                FeeType = ATransactionSubCode.FeeType,

                ApprovalStatus = ATransactionSubCode.ApprovalStatus,
                CreatedBy = ATransactionSubCode.CreatedBy,
                CreatedOn = ATransactionSubCode.CreatedOn,
                LastUpdatedBy = ATransactionSubCode.LastUpdatedBy,
                LastUpdatedOn = ATransactionSubCode.LastUpdatedOn,
                ApprovedBy = ATransactionSubCode.ApprovedBy,
                ApprovedOn = ATransactionSubCode.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ATransactionSubCode.ApprovalStatus),
                TransactionDesc = (ATransactionSubCode.TransactionCode != null) ? ATransactionSubCode.TransactionCode + " - " + ATransactionSubCode.Transaction.Description : null,
                DebitOrCreditDesc = ATransactionSubCode.IsDebitOrCredit == "CR" ? "Credit" : "Debit"
            };
            return objTransactionSubCodeDTO;
        }

        #endregion

        #region Product Lookup DTO

        public static ProductLookUpDTO ProductToProductLookupDTO(Product AProduct)
        {
            ProductLookUpDTO objProductLookUpDTO = new ProductLookUpDTO()
            {
                ProductCode = AProduct.ProductCode,
                ProductName = AProduct.ProductName
            };
            return objProductLookUpDTO;
        }
        #endregion

        #region User

        #region UserDTO
        public static UserDTO UserToUserDTO(User AUser)
        {
            UserDTO objUserDTO = new UserDTO()
            {
                Username = AUser.Username,
                Password = CC_Ingenio.Common.Utils.Utils.Helper.DecryptPassword(AUser.Password),
                FullName = AUser.FullName,
                Email = AUser.Email,
                Active = AUser.Active,
                OrganizationCode = AUser.OrganizationCode,
                CultureCode = AUser.CultureCode
            };
            return objUserDTO;
        }
        #endregion

        #region UserGroupDTO
        public static UserGroupDTO UserGroupToUserGroupDTO(UserGroup AUserGroup)
        {
            UserGroupDTO objUserGroupDTO = new UserGroupDTO()
            {
                GroupCode = AUserGroup.GroupCode,
                GroupName = AUserGroup.GroupName,
                ApprovalStatus = AUserGroup.ApprovalStatus

            };
            /* foreach(var member in AUsergroup.Users)
                 objUserGroupDTO.Users.Add(UserToUserDTO(member));*/
            return objUserGroupDTO;
        }


        #endregion

        #region Branch
        public static BranchDTO BranchToBranchDTO(Branch ABranch)
        {

            BranchDTO objBranchDTO = new BranchDTO()
            {
                OrganizationCode = ABranch.OrganizationCode,
                BranchCode = ABranch.BranchCode,
                BranchName = ABranch.BranchName,
                CountryCode = ABranch.CountryCode,
                RegionCode = ABranch.RegionCode,
                CommunaCode = ABranch.CommunaCode,
                CityCode = ABranch.CityCode,
                Address = ABranch.Address,
                DepId = ABranch.DepId,

                ApprovalStatus = ABranch.ApprovalStatus,
                CreatedBy = ABranch.CreatedBy,
                CreatedOn = ABranch.CreatedOn,
                LastUpdatedBy = ABranch.LastUpdatedBy,
                LastUpdatedOn = ABranch.LastUpdatedOn,
                ApprovedBy = ABranch.ApprovedBy,
                ApprovedOn = ABranch.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ABranch.ApprovalStatus)
            };
            return objBranchDTO;

        }
        #endregion

        #region UserPrivilegeDTO
        public static UserPrivilegeDTO UserEntityToUserPrivilegeDTO(UserEntity AUserEntity)
        {

            UserPrivilegeDTO objUserPrivilegeDTO = new UserPrivilegeDTO()
            {
                EntityCode = AUserEntity.EntityCode,
                ActionValue = AUserEntity.ActionValue,
                EntityDesc = AUserEntity.EntityDescription
            };
            return objUserPrivilegeDTO;

        }
        #endregion

        #region UserAllowedBranch
        public static UserAllowedBranchDTO UserAllowedBranchToUserAllowedBranchDTO(UserAllowedBranch AUserAllowedBranch)
        {

            UserAllowedBranchDTO objUserAllowedBranchDTO = new UserAllowedBranchDTO()
            {
                Username = AUserAllowedBranch.Username,
                BranchCode = AUserAllowedBranch.BranchCode

            };
            return objUserAllowedBranchDTO;

        }
        #endregion

        #region UserGroupMembers
        public static UserGroupMembersDTO UserGroupMembersToUserGroupMembersDTO(UserGroupMembers AUserGroupMembers)
        {

            UserGroupMembersDTO objUserGroupMembersDTO = new UserGroupMembersDTO()
            {
                GroupCode = AUserGroupMembers.GroupCode,
                UserName = AUserGroupMembers.UserName

            };
            return objUserGroupMembersDTO;

        }
        #endregion

        #region UserPrivilege
        public static UserPrivilegeDTO UserPrivilegeToUserPrivilegeDTO(UserGroupPrivilege AUserPrivilege)
        {

            UserPrivilegeDTO objUserPrivilegeDTO = new UserPrivilegeDTO()
            {
                EntityCode = AUserPrivilege.EntityCode,
                //ActionValue = AUserPrivilege.ActionValue          

            };
            return objUserPrivilegeDTO;

        }
        #endregion

        #endregion

        #region FinancialAccountDTO
        public static FinancialAccountDTO FinancialAccountToFinancialAccountDTO(FinancialAccount AFinancialAccount)
        {
            FinancialAccountDTO objFinancialAccountDTO = new FinancialAccountDTO()
            {
                FinancialAccountId = AFinancialAccount.FinancialAccountId,
                BranchCode = AFinancialAccount.BranchCode,
                ModuleName = AFinancialAccount.ModuleName,
                ProductCode = AFinancialAccount.ProductCode,
                TransactionSubCodeId = AFinancialAccount.TransactionSubCodeId,
                DebitAccountNr = AFinancialAccount.DebitAccountNr,
                DebitAccountCode = AFinancialAccount.DebitAccountCode,
                DebitSubTransactionCode = AFinancialAccount.DebitSubTransactionCode,
                DebitNarration = AFinancialAccount.DebitNarration,
                IsDebitConsolidated = AFinancialAccount.IsDebitConsolidated,
                CreditAccountNr = AFinancialAccount.CreditAccountNr,
                CreditAccountCode = AFinancialAccount.CreditAccountCode,
                CreditSubTransactionCode = AFinancialAccount.CreditSubTransactionCode,
                IsCreditConsolidated = AFinancialAccount.IsCreditConsolidated,
                CreditNarration = AFinancialAccount.CreditNarration,
                EffectiveDate = AFinancialAccount.EffectiveDate,
                Portfolio = AFinancialAccount.Portfolio,

                CreatedBy = AFinancialAccount.CreatedBy,
                CreatedOn = AFinancialAccount.CreatedOn,
                LastUpdatedBy = AFinancialAccount.LastUpdatedBy,
                LastUpdatedOn = AFinancialAccount.LastUpdatedOn,
                ApprovalStatus = AFinancialAccount.ApprovalStatus,
                ApprovedBy = AFinancialAccount.ApprovedBy,
                ApprovedOn = AFinancialAccount.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AFinancialAccount.ApprovalStatus),
                BranchDesc = (AFinancialAccount.BranchCode != null && AFinancialAccount.BranchCode != "ALL") ? AFinancialAccount.BranchCode + " - " + AFinancialAccount.Branch.BranchName : AFinancialAccount.BranchCode != "ALL" ? null : AFinancialAccount.BranchCode,
                ProductName = (AFinancialAccount.ProductCode != null && AFinancialAccount.ProductCode != "ALL") ? AFinancialAccount.ProductCode + " - " + AFinancialAccount.Product.ProductName : AFinancialAccount.ProductCode != "ALL" ? null : AFinancialAccount.ProductCode,
                FinancialActivityDesc = (AFinancialAccount.TransactionSubCodeId != 0) ? AFinancialAccount.TransactionSubCode.TransactionCode + AFinancialAccount.TransactionSubCode.SubCode + " - " + AFinancialAccount.TransactionSubCode.Description : null
            };
            return objFinancialAccountDTO;
        }
        #endregion

        #region BillingCycleParameterDTO
        public static BillingCycleParameterDTO BillingCycleParameterToBillingCycleParameterDTO(BillingCycleParameter ABillingCycleParameter)
        {
            BillingCycleParameterDTO objBillingCycleParameterDTO = new BillingCycleParameterDTO()
            {
                BillingCycleParameterId = ABillingCycleParameter.BillingCycleParameterId,
                BillMonth = ABillingCycleParameter.BillMonth,
                BillCycleSysParamId = ABillingCycleParameter.BillCycleSysParamId,
                BillDate = ABillingCycleParameter.BillDate,
                BillYear = ABillingCycleParameter.BillYear,
                StatementDate = ABillingCycleParameter.StatementDate,
                PaymentDueDate = ABillingCycleParameter.PaymentDueDate,
                BillFrom = ABillingCycleParameter.BillFrom,
                BillTo = ABillingCycleParameter.BillTo,

                CreatedBy = ABillingCycleParameter.CreatedBy,
                CreatedOn = ABillingCycleParameter.CreatedOn,
                LastUpdatedBy = ABillingCycleParameter.LastUpdatedBy,
                LastUpdatedOn = ABillingCycleParameter.LastUpdatedOn,
                ApprovalStatus = ABillingCycleParameter.ApprovalStatus,
                ApprovedBy = ABillingCycleParameter.ApprovedBy,
                ApprovedOn = ABillingCycleParameter.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ABillingCycleParameter.ApprovalStatus),
                BillingCycleDesc = (ABillingCycleParameter.BillCycleSysParamId != 0) ? ABillingCycleParameter.BillingCycle.Description : null,
                BillMonthDesc = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(ABillingCycleParameter.BillMonth)

            };
            return objBillingCycleParameterDTO;
        }
        #endregion

        #region SBIFInterestDTO
        public static SBIFInterestDTO SBIFInterestToSBIFInterestDTO(SBIFInterest ASBIFInterest)
        {
            SBIFInterestDTO objSBIFInterestDTO = new SBIFInterestDTO()
            {
                SBIFInterestId = ASBIFInterest.SBIFInterestId,
                EffectiveDate = ASBIFInterest.EffectiveDate,
                MaxInterestDefinitionFor = ASBIFInterest.MaxInterestDefinitionFor,

                CreatedBy = ASBIFInterest.CreatedBy,
                CreatedOn = ASBIFInterest.CreatedOn,
                LastUpdatedBy = ASBIFInterest.LastUpdatedBy,
                LastUpdatedOn = ASBIFInterest.LastUpdatedOn,
                ApprovalStatus = ASBIFInterest.ApprovalStatus,
                ApprovedBy = ASBIFInterest.ApprovedBy,
                ApprovedOn = ASBIFInterest.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ASBIFInterest.ApprovalStatus),
                MaxInterestDefinitionForDesc = ASBIFInterest.MaxInterestDefinitionFor == "R" ? "Revolving" :
                                                        ASBIFInterest.MaxInterestDefinitionFor == "D" ? "Delinquent" : null
            };

            objSBIFInterestDTO.SBIFInterestDetailsDTO.AddRange(SBIFInterestDetailsToSBIFInterestDetailsDTO(ASBIFInterest.SBIFInterestDetails));

            return objSBIFInterestDTO;
        }

        public static List<SBIFInterestDetailsDTO> SBIFInterestDetailsToSBIFInterestDetailsDTO(List<SBIFInterestDetails> ASBIFInterestDetails)
        {
            List<SBIFInterestDetailsDTO> listSBIFInterestDetailsDTO = new List<SBIFInterestDetailsDTO>();

            ASBIFInterestDetails.ForEach
            (
                x => listSBIFInterestDetailsDTO.Add
                (
                    new SBIFInterestDetailsDTO
                    {
                        SBIFInterestDetailsId = x.SBIFInterestDetailsId,
                        SBIFInterestId = x.SBIFInterestId,
                        CreditLimitFrom = x.CreditLimitFrom,
                        CreditLimitTo = x.CreditLimitTo,
                        SBIFInterestRate = x.SBIFInterestRate
                    }
                )
            );

            return listSBIFInterestDetailsDTO;
        }

        #endregion

        #region InterestMasterDTO
        public static InterestMasterDTO InterestMasterToInterestMasterDTO(InterestMaster AInterestMaster)
        {
            InterestMasterDTO objInterestMasterDTO = new InterestMasterDTO()
            {
                InterestMasterId = AInterestMaster.InterestMasterId,
                ProductCode = AInterestMaster.ProductCode,
                CustomerSegmentCode = AInterestMaster.CustomerSegmentCode,
                CustomerSubSegmentCode = AInterestMaster.CustomerSubSegmentCode,
                EffectiveDate = AInterestMaster.EffectiveDate,
                MaxInterestDefinitionFor = AInterestMaster.MaxInterestDefinitionFor,

                CreatedBy = AInterestMaster.CreatedBy,
                CreatedOn = AInterestMaster.CreatedOn,
                LastUpdatedBy = AInterestMaster.LastUpdatedBy,
                LastUpdatedOn = AInterestMaster.LastUpdatedOn,
                ApprovalStatus = AInterestMaster.ApprovalStatus,
                ApprovedBy = AInterestMaster.ApprovedBy,
                ApprovedOn = AInterestMaster.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AInterestMaster.ApprovalStatus),
                ProductName = (AInterestMaster.ProductCode != null) ? AInterestMaster.ProductCode + " - " + AInterestMaster.Product.ProductName : null,
                CustomerSegmentDesc = (AInterestMaster.CustomerSegmentCode != null) ? AInterestMaster.CustomerSegment.SegmentName : null,
                CustomerSubSegmentDesc = (AInterestMaster.CustomerSubSegmentCode != null) ? AInterestMaster.CustomerSubSegment.SubSegmentName : null

            };

            objInterestMasterDTO.InterestMasterDetailsDTO.AddRange(InterestMasterDetailsToInterestMasterDetailsDTO(AInterestMaster.InterestMasterDetails));

            return objInterestMasterDTO;
        }

        public static List<InterestMasterDetailsDTO> InterestMasterDetailsToInterestMasterDetailsDTO(List<InterestMasterDetails> AInterestMasterDetails)
        {
            List<InterestMasterDetailsDTO> listInterestMasterDetailsDTO = new List<InterestMasterDetailsDTO>();

            AInterestMasterDetails.ForEach
            (
                x => listInterestMasterDetailsDTO.Add
                (
                    new InterestMasterDetailsDTO
                    {
                        InterestMasterDetailsId = x.InterestMasterDetailsId,
                        InterestMasterId = x.InterestMasterId,
                        CreditLimitFrom = x.CreditLimitFrom,
                        CreditLimitTo = x.CreditLimitTo,
                        InterestRatePercent = x.InterestRatePercent
                    }
                )
            );

            return listInterestMasterDetailsDTO;
        }

        #endregion

        #region CommissionDTO
        public static CommissionDTO CommissionToCommissionDTO(Commission ACommissionCode)
        {
            CommissionDTO objCommissionDTO = new CommissionDTO()
            {
                CommissionCode = ACommissionCode.CommissionCode,
                OrganizationCode = ACommissionCode.OrganizationCode,
                Description = ACommissionCode.Description,

                CreatedBy = ACommissionCode.CreatedBy,
                CreatedOn = ACommissionCode.CreatedOn,
                LastUpdatedBy = ACommissionCode.LastUpdatedBy,
                LastUpdatedOn = ACommissionCode.LastUpdatedOn,
                ApprovalStatus = ACommissionCode.ApprovalStatus,
                ApprovedBy = ACommissionCode.ApprovedBy,
                ApprovedOn = ACommissionCode.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACommissionCode.ApprovalStatus)
            };
            return objCommissionDTO;
        }
        #endregion

        #region BillingParameterDTO
        public static BillingParameterDTO BillingParameterToBillingParameterDTO(BillingParameter ABillingParameter)
        {
            BillingParameterDTO objBillingParameterDTO = new BillingParameterDTO()
            {
                ProductCode = ABillingParameter.ProductCode,
                MinTotalAmount = ABillingParameter.MinTotalAmount,
                MinTotalDebt = ABillingParameter.MinTotalDebt,
                ThresholdRevolvingBalance = ABillingParameter.ThresholdRevolvingBalance,
                OCLChargePercentage = ABillingParameter.OCLChargePercentage,
                MinOCLCharge = ABillingParameter.MinOCLCharge,
                MaxOCLCharge = ABillingParameter.MaxOCLCharge,
                MinDelinquencyAmount = ABillingParameter.MinDelinquencyAmount,
                MinCollectionCharges = ABillingParameter.MinCollectionCharges,
                MaxCollectionCharges = ABillingParameter.MaxCollectionCharges,
                IsOCLIncludedInTotalMinDue = ABillingParameter.IsOCLIncludedInTotalMinDue,
                DelinquencyBucketCountForWriteOff = ABillingParameter.DelinquencyBucketCountForWriteOff,
                CreatedBy = ABillingParameter.CreatedBy,
                CreatedOn = ABillingParameter.CreatedOn,
                LastUpdatedBy = ABillingParameter.LastUpdatedBy,
                LastUpdatedOn = ABillingParameter.LastUpdatedOn,
                ApprovedBy = ABillingParameter.ApprovedBy,
                ApprovedOn = ABillingParameter.ApprovedOn,
                ApprovalStatus = ABillingParameter.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ABillingParameter.ApprovalStatus),

                ProductName = (ABillingParameter.ProductCode != null) ? ABillingParameter.ProductCode + " - " + ABillingParameter.Product.ProductName : null,


            };
            return objBillingParameterDTO;
        }
        #endregion

        #region CardMasterDTO
        public static CardMasterDTO CardMasterToCardMasterDTO(CardMaster ACardMaster)
        {
            CardMasterDTO objCardMasterDTO = new CardMasterDTO()
            {
                OrganizationCode = ACardMaster.OrganizationCode,
                CustomerId = ACardMaster.CustomerId,
                //RUT = ACardMaster.RUT,
                RUT = (ACardMaster.RUT == null) ? "" : ACardMaster.RUT.Insert(ACardMaster.RUT.Length - 1, "-"),
                SalesId = ACardMaster.SalesId,
                AccountNo = ACardMaster.AccountNo,
                CardNumber = ACardMaster.CardNumber,
                CardTypeCode = ACardMaster.CardTypeCode,
                CardStatusCode = ACardMaster.CardStatusCode,
                ProductCode = ACardMaster.ProductCode,
                ValidFromDate = ACardMaster.ValidFromDate,
                CardExpiryDate = ACardMaster.CardExpiryDate,
                CashOTB = ACardMaster.CashOTB,
                CreditOTB = ACardMaster.CreditOTB,
                DailyCreditLimit = ACardMaster.DailyCreditLimit,
                WeeklyCreditLimit = ACardMaster.WeeklyCreditLimit,
                DailyCashLimit = ACardMaster.DailyCashLimit,
                WeeklyCashLimit = ACardMaster.WeeklyCashLimit,
                CreatedBy = ACardMaster.CreatedBy,
                CreatedOn = ACardMaster.CreatedOn,
                LastUpdatedBy = ACardMaster.LastUpdatedBy,
                LastUpdatedOn = ACardMaster.LastUpdatedOn,
                ApprovalStatus = ACardMaster.ApprovalStatus,
                ApprovedBy = ACardMaster.ApprovedBy,
                ApprovedOn = ACardMaster.ApprovedOn,
                QueueReferenceNo = ACardMaster.QueueReferenceNo,

                CustomerCardName = ACardMaster.CustomerCardName,
                CountryCode = ACardMaster.CountryCode,
                Gender = ACardMaster.Gender,
                RenewalFeeWaiver = ACardMaster.RenewalFeeWaiver,
                IsInterestWaiver = ACardMaster.IsInterestWaiver,
                IsAutoRenewal = ACardMaster.IsAutoRenewal,
                IsPrinted = ACardMaster.IsPrinted,
                AddressTypeCode = ACardMaster.AddressTypeCode,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACardMaster.ApprovalStatus),

                CustomerSegmentCode = ACardMaster.CustomerSegmentCode,
                CustomerSubSegmentCode = ACardMaster.CustomerSubSegmentCode,
                IsVIP = ACardMaster.IsVIP,
                IsCardRenewable = ACardMaster.IsCardRenewable,
                IsCardRenewed = ACardMaster.IsCardRenewed,
                CardMemberType = ACardMaster.CardMemberType,

                CustomerRefinanceCount = ACardMaster.CustomerRefinanceCount,
                MaxPermitRefinanceCount = ACardMaster.MaxPermitRefinanceCount,
                MaxPermitRenegoCount = ACardMaster.MaxPermitRenegoCount,
                MaxDueRenegoCount = ACardMaster.MaxDueRenegoCount,
                CustomerDueRenegoCount = ACardMaster.CustomerDueRenegoCount,
                MaxOverDueRenegoCount = ACardMaster.MaxOverDueRenegoCount,
                CustomerOverDueRenegoCount = ACardMaster.CustomerOverDueRenegoCount,
                MaxWriteOffRenegoCount = ACardMaster.MaxWriteOffRenegoCount,
                CustomerWriteOffRenegoCount = ACardMaster.CustomerWriteOffRenegoCount,
                CreditLimit = ACardMaster.CreditLimit,
                CashLimit = ACardMaster.CashLimit,

                OrganizationName ="",// ACardMaster.objOrganization.OrganizationDescription,
                ProductName ="",// ACardMaster.objProduct.ProductName,
                CustomerSegmentName = ACardMaster.CustomerSegmentCode == string.Empty ? string.Empty : ACardMaster.objCustomerSegment.SegmentName,
                CustomerSubSegmentName = ACardMaster.CustomerSubSegmentCode == string.Empty ? string.Empty : ACardMaster.objCustomerSubSegment.SubSegmentName
            };

            return objCardMasterDTO;
        }
        #endregion

        #region CommissionChargeDTO
        public static CommissionChargeDTO CommissionChargeToCommissionChargeDTO(CommissionCharge ACommissionCharge)
        {
            CommissionChargeDTO objCommissionChargeDTO = new CommissionChargeDTO()
            {
                CommissionChargeId = ACommissionCharge.CommissionChargeId,
                OrganizationCode = ACommissionCharge.OrganizationCode,
                ProductCode = ACommissionCharge.ProductCode,
                ContractCode = ACommissionCharge.ContractCode,
                CurrencyCode = ACommissionCharge.CurrencyCode,

                CreatedBy = ACommissionCharge.CreatedBy,
                CreatedOn = ACommissionCharge.CreatedOn,
                LastUpdatedBy = ACommissionCharge.LastUpdatedBy,
                LastUpdatedOn = ACommissionCharge.LastUpdatedOn,
                ApprovalStatus = ACommissionCharge.ApprovalStatus,
                ApprovedBy = ACommissionCharge.ApprovedBy,
                ApprovedOn = ACommissionCharge.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACommissionCharge.ApprovalStatus),
                ProductName = (ACommissionCharge.ProductCode != null) ? ACommissionCharge.ProductCode + " - " + ACommissionCharge.Product.ProductName : null,
                ContractDesc = (ACommissionCharge.ContractCode != null) ? ACommissionCharge.Contract.ContractDescription : null,
            };


            objCommissionChargeDTO.CommissionChargeDetailsDTO.AddRange(CommissionChargeDetailsToCommissionChargeDetailsDTO(ACommissionCharge.CommissionChargeDetails, ACommissionCharge.CurrencyCode));

            return objCommissionChargeDTO;
        }

        public static List<CommissionChargeDetailsDTO> CommissionChargeDetailsToCommissionChargeDetailsDTO(List<CommissionChargeDetails> ACommissionChargeDetails, string ACurrencyCode)
        {
            List<CommissionChargeDetailsDTO> listCommissionChargeDetailsDTO = new List<CommissionChargeDetailsDTO>();

            ACommissionChargeDetails.ForEach
            (
                x => listCommissionChargeDetailsDTO.Add
                (
                    new CommissionChargeDetailsDTO
                        {
                            CommissionChargeId = x.CommissionChargeId,
                            CommissionChargeDetailsId = x.CommissionChargeDetailsId,
                            CommissionCode = x.CommissionCode,
                            CommissionAmount = x.CommissionAmount,
                            ConvertedCommissionAmount = (ACurrencyCode == CC_Ingenio.Common.Utils.Utils.UFCurrencyCode ? x.CommissionAmount * CC_Ingenio.Common.Utils.Utils.CurrentUFRate : x.CommissionAmount)
                        }
                )
            );

            return listCommissionChargeDetailsDTO;
        }


        #endregion

        #region DiscountOnMgmtCollectionDTO
        public static DiscountOnMgmtCollectionDTO DiscountOnMgmtCollectionToDiscountOnMgmtCollectionDTO(DiscountOnMgmtCollection ADiscountOnMgmtCollection)
        {
            DiscountOnMgmtCollectionDTO objDiscountOnMgmtCollectionDTO = new DiscountOnMgmtCollectionDTO()
            {
                DiscountOnMgmtCollectionId = ADiscountOnMgmtCollection.DiscountOnMgmtCollectionId,
                ProductCode = ADiscountOnMgmtCollection.ProductCode,
                ContractCode = ADiscountOnMgmtCollection.ContractCode,
                CommissionCode = ADiscountOnMgmtCollection.CommissionCode,
                OrganizationCode = ADiscountOnMgmtCollection.OrganizationCode,

                CreatedBy = ADiscountOnMgmtCollection.CreatedBy,
                CreatedOn = ADiscountOnMgmtCollection.CreatedOn,
                LastUpdatedBy = ADiscountOnMgmtCollection.LastUpdatedBy,
                LastUpdatedOn = ADiscountOnMgmtCollection.LastUpdatedOn,
                ApprovalStatus = ADiscountOnMgmtCollection.ApprovalStatus,
                ApprovedBy = ADiscountOnMgmtCollection.ApprovedBy,
                ApprovedOn = ADiscountOnMgmtCollection.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ADiscountOnMgmtCollection.ApprovalStatus),
                ProductName = (ADiscountOnMgmtCollection.ProductCode != null) ? ADiscountOnMgmtCollection.ProductCode + " - " + ADiscountOnMgmtCollection.Product.ProductName : null,
                ContractDesc = (ADiscountOnMgmtCollection.ContractCode != null) ? ADiscountOnMgmtCollection.ContractCode + " - " + ADiscountOnMgmtCollection.Contract.ContractDescription : null,
                CommissionCodeDesc = (ADiscountOnMgmtCollection.CommissionCode != null) ? ADiscountOnMgmtCollection.CommissionCode + " - " + ADiscountOnMgmtCollection.Commission.Description : null
            };

            objDiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionDetailsDTO.AddRange(DiscountOnMgmtCollectionDetailsToDiscountOnMgmtCollectionDetailsDTO(ADiscountOnMgmtCollection.DiscountOnMgmtCollectionDetails));

            return objDiscountOnMgmtCollectionDTO;
        }

        public static List<DiscountOnMgmtCollectionDetailsDTO> DiscountOnMgmtCollectionDetailsToDiscountOnMgmtCollectionDetailsDTO(List<DiscountOnMgmtCollectionDetails> ADiscountOnMgmtCollectionDetails)
        {
            List<DiscountOnMgmtCollectionDetailsDTO> listDiscountOnMgmtCollectionDetailsDTO = new List<DiscountOnMgmtCollectionDetailsDTO>();

            ADiscountOnMgmtCollectionDetails.ForEach
            (
                x => listDiscountOnMgmtCollectionDetailsDTO.Add
                (
                    new DiscountOnMgmtCollectionDetailsDTO
                    {
                        DiscountOnMgmtCollectionDetailsId = x.DiscountOnMgmtCollectionIdDetailsId,
                        DiscountOnMgmtCollectionId = x.DiscountOnMgmtCollectionId,
                        ConsumptionRangeFrom = x.ConsumptionRangeFrom,
                        ConsumptionRangeTo = x.ConsumptionRangeTo,
                        DiscountPercentage = x.DiscountPercentage
                    }
                )
            );

            return listDiscountOnMgmtCollectionDetailsDTO;
        }

        #endregion

        #region BlockCodeForCommissionExclusionDTO
        public static BlockCodeForCommissionExclusionDTO BlockCodeForCommissionExclusionToBlockCodeForCommissionExclusionDTO(BlockCodeForCommissionExclusion ABlockCodeForCommissionExclusion)
        {
            BlockCodeForCommissionExclusionDTO objBlockCodeForCommissionExclusionDTO = new BlockCodeForCommissionExclusionDTO()
            {
                BlockCodeForCommissionExclusionId = ABlockCodeForCommissionExclusion.BlockCodeForCommissionExclusionId,
                BlockCode = ABlockCodeForCommissionExclusion.BlockCode,
                CommissionCode = ABlockCodeForCommissionExclusion.CommissionCode,
                IsIncludeCharges = ABlockCodeForCommissionExclusion.IsIncludeCharges,
                OrganizationCode = ABlockCodeForCommissionExclusion.OrganizationCode,
                CreatedBy = ABlockCodeForCommissionExclusion.CreatedBy,
                CreatedOn = ABlockCodeForCommissionExclusion.CreatedOn,
                LastUpdatedBy = ABlockCodeForCommissionExclusion.LastUpdatedBy,
                LastUpdatedOn = ABlockCodeForCommissionExclusion.LastUpdatedOn,
                ApprovedBy = ABlockCodeForCommissionExclusion.ApprovedBy,
                ApprovedOn = ABlockCodeForCommissionExclusion.ApprovedOn,
                ApprovalStatus = ABlockCodeForCommissionExclusion.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ABlockCodeForCommissionExclusion.ApprovalStatus),
                BlockCodeDesc = (ABlockCodeForCommissionExclusion.BlockCode != null) ? ABlockCodeForCommissionExclusion.Block.Code + " - " + ABlockCodeForCommissionExclusion.Block.Description : null,
                CommissionCodeDesc = (ABlockCodeForCommissionExclusion.CommissionCode != null) ? ABlockCodeForCommissionExclusion.CommissionCode + " - " + ABlockCodeForCommissionExclusion.Commission.Description : null
            };

            return objBlockCodeForCommissionExclusionDTO;
        }
        #endregion

        #region CollectionChargeDTO
        public static CollectionChargeDTO CollectionChargeToCollectionChargeDTO(CollectionCharge ACollectionCharge)
        {
            CollectionChargeDTO objCollectionChargeDTO = new CollectionChargeDTO()
            {
                CollectionChargeId = ACollectionCharge.CollectionChargeId,
                ProductCode = ACollectionCharge.ProductCode,
                EffectiveDate = ACollectionCharge.EffectiveDate,
                OrganizationCode = ACollectionCharge.OrganizationCode,
                CurrencyCode = ACollectionCharge.CurrencyCode,

                CreatedBy = ACollectionCharge.CreatedBy,
                CreatedOn = ACollectionCharge.CreatedOn,
                LastUpdatedBy = ACollectionCharge.LastUpdatedBy,
                LastUpdatedOn = ACollectionCharge.LastUpdatedOn,
                ApprovedBy = ACollectionCharge.ApprovedBy,
                ApprovedOn = ACollectionCharge.ApprovedOn,
                ApprovalStatus = ACollectionCharge.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACollectionCharge.ApprovalStatus),
                ProductName = (ACollectionCharge.ProductCode != null) ? ACollectionCharge.ProductCode + " - " + ACollectionCharge.Product.ProductName : null,
            };

            objCollectionChargeDTO.CollectionChargeDetailsDTO.AddRange(CollectionChargeDetailsToCollectionChargeDetailsDTO(ACollectionCharge.CollectionChargeDetails));

            return objCollectionChargeDTO;
        }

        public static List<CollectionChargeDetailsDTO> CollectionChargeDetailsToCollectionChargeDetailsDTO(List<CollectionChargeDetails> ACollectionChargeDetails)
        {
            List<CollectionChargeDetailsDTO> listCollectionChargeDetailsDTO = new List<CollectionChargeDetailsDTO>();

            ACollectionChargeDetails.ForEach
            (
                x => listCollectionChargeDetailsDTO.Add
                (
                    new CollectionChargeDetailsDTO
                    {
                        CollectionChargeDetailsId = x.CollectionChargeDetailsId,
                        CollectionChargeId = x.CollectionChargeId,
                        DelinquencyAmountFrom = x.DelinquencyAmountFrom,
                        DelinquencyAmountTo = x.DelinquencyAmountTo,
                        CollectionChargeAmount = x.CollectionChargeAmount
                    }
                )
            );

            return listCollectionChargeDetailsDTO;
        }

        #endregion

        #region InsuranceChargeToInsuranceChargeDTO
        public static InsuranceChargeDTO InsuranceChargeToInsuranceChargeDTO(InsuranceCharge AInsuranceCharge)
        {
            InsuranceChargeDTO objInsuranceChargeDTO = new InsuranceChargeDTO()
            {
                InsuranceChargeId = AInsuranceCharge.InsuranceChargeId,
                OrganizationCode = AInsuranceCharge.OrganizationCode,
                ContractCode = AInsuranceCharge.ContractCode,
                ProductCode = AInsuranceCharge.ProductCode,
                EffectiveDate = AInsuranceCharge.EffectiveDate,
                InsuranceType = AInsuranceCharge.InsuranceType,
                InsuranceOption = AInsuranceCharge.InsuranceOption,
                InsuranceAmount = AInsuranceCharge.InsuranceAmount,
                EligibilityAgeYears = AInsuranceCharge.EligibilityAgeYears,
                EligibilityAgeDays = AInsuranceCharge.EligibilityAgeDays,
                MaxCoverAgeYears = AInsuranceCharge.MaxCoverAgeYears,
                MaxCoverAgeDays = AInsuranceCharge.MaxCoverAgeDays,
                IsGeneralOrSpecial = AInsuranceCharge.IsGeneralOrSpecial,
                CurrencyCode = AInsuranceCharge.CurrencyCode,
                PolicyCode = AInsuranceCharge.PolicyCode,
                PolicyDetails = AInsuranceCharge.PolicyDetails,
                CollectionCharge = AInsuranceCharge.CollectionCharge,

                CreatedBy = AInsuranceCharge.CreatedBy,
                CreatedOn = AInsuranceCharge.CreatedOn,
                LastUpdatedBy = AInsuranceCharge.LastUpdatedBy,
                LastUpdatedOn = AInsuranceCharge.LastUpdatedOn,
                ApprovedBy = AInsuranceCharge.ApprovedBy,
                ApprovedOn = AInsuranceCharge.ApprovedOn,
                ApprovalStatus = AInsuranceCharge.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AInsuranceCharge.ApprovalStatus),

                ContractDescription = AInsuranceCharge.Contract.ContractDescription,
                ProductName = AInsuranceCharge.Product.ProductName,
                CurrencyName = AInsuranceCharge.Currency.CurrencyName,
                InsuranceTypeDesc = (AInsuranceCharge.InsuranceType == "TYP1" ? "Insurance Type1" : (AInsuranceCharge.InsuranceType == "TYP2" ? "Insurance Type2" : string.Empty)),
                InsuranceOptionDesc = (AInsuranceCharge.InsuranceOption == "AMT" ? "Fixed Amount" : (AInsuranceCharge.InsuranceOption == "RATE" ? "Rate" : string.Empty))
            };
            return objInsuranceChargeDTO;
        }
        #endregion

        #region StatementGroupingDTO
        public static StatementGroupingDTO StatementGroupingToStatementGroupingDTO(StatementGrouping AStatementGrouping)
        {
            StatementGroupingDTO objStatementGroupingDTO = new StatementGroupingDTO()
            {
                StatementGroupingId = AStatementGrouping.StatementGroupingId,
                ProductCode = AStatementGrouping.ProductCode,
                NetworkCode = AStatementGrouping.NetworkCode,
                TransactionCode = AStatementGrouping.TransactionCode,
                GroupCode = AStatementGrouping.GroupCode,
                GroupCodeDescription = AStatementGrouping.GroupCodeDescription,
                SubGroupCode = AStatementGrouping.SubGroupCode,
                SubGroupCodeDescription = AStatementGrouping.SubGroupCodeDescription,

                CreatedBy = AStatementGrouping.CreatedBy,
                CreatedOn = AStatementGrouping.CreatedOn,
                LastUpdatedBy = AStatementGrouping.LastUpdatedBy,
                LastUpdatedOn = AStatementGrouping.LastUpdatedOn,
                ApprovedBy = AStatementGrouping.ApprovedBy,
                ApprovedOn = AStatementGrouping.ApprovedOn,
                ApprovalStatus = AStatementGrouping.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AStatementGrouping.ApprovalStatus),
                ProductDesc = (AStatementGrouping.ProductCode != null) ? AStatementGrouping.ProductCode + " - " + AStatementGrouping.Product.ProductName : null,
                NetworkDesc = (AStatementGrouping.NetworkCode != null) ? AStatementGrouping.Network.Description : null,
                TransactionDesc = (AStatementGrouping.TransactionCode != null) ? AStatementGrouping.TransactionCode + " - " + AStatementGrouping.Transaction.Description : null
            };

            return objStatementGroupingDTO;
        }
        #endregion

        #region PaymentAllocationTypeDTO
        public static PaymentAllocationTypeDTO PaymentAllocationTypeToPaymentAllocationTypeDTO(PaymentAllocationType APaymentAllocationType)
        {
            PaymentAllocationTypeDTO objPaymentAllocationTypeDTO = new PaymentAllocationTypeDTO()
            {
                PaymentAllocationTypeId = APaymentAllocationType.PaymentAllocationTypeId,
                OrganizationCode = APaymentAllocationType.OrganizationCode,
                CardType = APaymentAllocationType.CardType,
                SeqNr = APaymentAllocationType.SeqNr,
                PaymentAllocationSequence = APaymentAllocationType.PaymentAllocationSequence,
                AssignedList = new List<string>(APaymentAllocationType.PaymentAllocationSequence.Split(',')),

                CreatedBy = APaymentAllocationType.CreatedBy,
                CreatedOn = APaymentAllocationType.CreatedOn,
                LastUpdatedBy = APaymentAllocationType.LastUpdatedBy,
                LastUpdatedOn = APaymentAllocationType.LastUpdatedOn,
                ApprovedBy = APaymentAllocationType.ApprovedBy,
                ApprovedOn = APaymentAllocationType.ApprovedOn,
                ApprovalStatus = APaymentAllocationType.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(APaymentAllocationType.ApprovalStatus),
                CardTypeDesc = APaymentAllocationType.CardType == "CN" ? "Card No" :
                                                        APaymentAllocationType.CardType == "RN" ? "RUT No" : null
            };

            return objPaymentAllocationTypeDTO;
        }
        #endregion

        #region   PlanMasterDTO
        public static PlanMasterDTO PlanMasterToPlanMasterDTO(PlanMaster APlanMaster)
        {
            PlanMasterDTO objPlanMasterDTO = new PlanMasterDTO()
           {
               PlanID = APlanMaster.PlanID,
               CreditSegmentCode = APlanMaster.CreditSegmentCode,
               ProductCode = APlanMaster.ProductCode,
               TransactionCode = APlanMaster.TransactionCode,
               CustomerSegmentCode = APlanMaster.CustomerSegmentCode,
               CustomerSubSegmentCode = APlanMaster.CustomerSubSegmentCode,
               ExpectedInterestRate = Decimal.Round(APlanMaster.ExpectedInterestRate, 2),
               CreditLimitFrom = APlanMaster.CreditLimitFrom,
               CreditLimitTo = APlanMaster.CreditLimitTo,
               FirstBilling = APlanMaster.FirstBilling,
               SBIFInterest = APlanMaster.SBIFInterest,
               //TenureFrom = APlanMaster.TenureFrom,
               TenureTo = APlanMaster.TenureTo,
               ValidFromDate = APlanMaster.ValidFromDate,
               //ValidToDate = APlanMaster.ValidToDate,
               InterestCalculation = APlanMaster.InterestCalculation,
               DeferredPayment = APlanMaster.DeferredPayment,
               CreatedBy = APlanMaster.CreatedBy,
               CreatedOn = APlanMaster.CreatedOn,
               LastUpdatedBy = APlanMaster.LastUpdatedBy,
               LastUpdatedOn = APlanMaster.LastUpdatedOn,
               ApprovedBy = APlanMaster.ApprovedBy,
               ApprovedOn = APlanMaster.ApprovedOn,
               ApprovalStatus = APlanMaster.ApprovalStatus,
               ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(APlanMaster.ApprovalStatus),

               CreditSegmentDesc = (APlanMaster.CreditSegmentCode != null) ? APlanMaster.CreditSegment.CardSegmentName : null,
               ProductName = (APlanMaster.ProductCode != null && APlanMaster.ProductCode != "ALL") ? APlanMaster.ProductCode + " - " + APlanMaster.Product.ProductName : APlanMaster.ProductCode != "ALL" ? null : APlanMaster.ProductCode,
               CustomerSegmentDesc = (APlanMaster.CustomerSegment != null && APlanMaster.CustomerSegmentCode != "ALL") ? APlanMaster.CustomerSegment.SegmentName : APlanMaster.CustomerSegmentCode != "ALL" ? null : APlanMaster.CustomerSegmentCode,
               CustomerSubSegmentDesc = (APlanMaster.CustomerSubSegmentCode != null && APlanMaster.CustomerSubSegmentCode != "ALL") ? APlanMaster.CustomerSubSegment.SubSegmentName : APlanMaster.CustomerSubSegmentCode != "ALL" ? null : APlanMaster.CustomerSubSegmentCode,
               TransactionDesc = (APlanMaster.TransactionCode != null) ? APlanMaster.TransactionCode + " - " + APlanMaster.Transaction.Description : null
           };
            return objPlanMasterDTO;
        }

        #endregion

        #region   CardCreditSegmentDTO
        public static CardCreditSegmentDTO CardCreditSegmentToCardCreditSegmentDTO(CardCreditSegment ACardCreditSegment)
        {
            CardCreditSegmentDTO objCardCreditSegmentDTO = new CardCreditSegmentDTO()
            {
                OrganizationCode = ACardCreditSegment.OrganizationCode,
                CardNumber = ACardCreditSegment.CardNumber,
                RUT = (ACardCreditSegment.RUT == null) ? "" : ACardCreditSegment.RUT.Insert(ACardCreditSegment.RUT.Length - 1, "-"),
                ProductCode = ACardCreditSegment.ProductCode,
                CustomerSegmentCode = ACardCreditSegment.CustomerSegmentCode,
                CustomerSubSegmentCode = ACardCreditSegment.CustomerSubSegmentCode,
                CreditSegmentCode = ACardCreditSegment.CreditSegmentCode,
                CreditValue = ACardCreditSegment.CreditValue,
                OTB = ACardCreditSegment.OTB,
                ValidFrom = ACardCreditSegment.ValidFrom,
                ValidTo = ACardCreditSegment.ValidTo,
                CreatedBy = ACardCreditSegment.CreatedBy,
                CreatedOn = ACardCreditSegment.CreatedOn,
                LastUpdatedBy = ACardCreditSegment.LastUpdatedBy,
                LastUpdatedOn = ACardCreditSegment.LastUpdatedOn,
                ApprovedBy = ACardCreditSegment.ApprovedBy,
                ApprovedOn = ACardCreditSegment.ApprovedOn,
                ApprovalStatus = ACardCreditSegment.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACardCreditSegment.ApprovalStatus),

                CreditSegmentName = ACardCreditSegment.objCreditSegment.CardSegmentName,
                CustomerSegmentName = ACardCreditSegment.objCustomerSegment.SegmentName,
                CustomerSubSegmentName = ACardCreditSegment.objCustomerSubSegment.SubSegmentName,
                OrganizationDescription = ACardCreditSegment.objOrganization.OrganizationDescription,
                ProductName = (ACardCreditSegment.ProductCode != null) ? ACardCreditSegment.ProductCode + " - " + ACardCreditSegment.objProduct.ProductName : null,
            };
            return objCardCreditSegmentDTO;
        }

        #endregion

        #region ManualTransactionDTO
        public static ManualTransactionDTO ManualTransactionToManualTransactionDTO(ManualTransaction AManualTransaction)
        {
            ManualTransactionDTO objManualTransactionDTO = new ManualTransactionDTO()
            {
                BatchNo = AManualTransaction.BatchNo,
                CardNumber = AManualTransaction.CardNumber,
                RUT = AManualTransaction.RUT.Insert(AManualTransaction.RUT.Length - 1, "-"),
                TxnDate = AManualTransaction.TxnDate,
                AuthCode = AManualTransaction.AuthCode,
                OrganizationCode = AManualTransaction.OrganizationCode,
                IncomingTransType = AManualTransaction.IncomingTransType,
                MerchantCode = AManualTransaction.MerchantCode,
                MCCCode = AManualTransaction.MCCCode,
                SegmentCode = AManualTransaction.SegmentCode,
                TransType = AManualTransaction.TransType,
                Amount = AManualTransaction.Amount,
                IsManualEntry = AManualTransaction.IsManualEntry,
                PostedDate = AManualTransaction.PostedDate,
                IsPosted = AManualTransaction.IsPosted,
                CreatedBy = AManualTransaction.CreatedBy,
                CreatedOn = AManualTransaction.CreatedOn,
                LastUpdatedBy = AManualTransaction.LastUpdatedBy,
                LastUpdatedOn = AManualTransaction.LastUpdatedOn,
                ApprovedBy = AManualTransaction.ApprovedBy,
                ApprovedOn = AManualTransaction.ApprovedOn,
                ApprovalStatus = AManualTransaction.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AManualTransaction.ApprovalStatus),
                Time = AManualTransaction.TxnTime,
                AuthCancellationStatus = AManualTransaction.AuthCancellationStatus,
                ReferenceAuthCode = AManualTransaction.ReferenceAuthCode,
                AuthCancellationRemarks = AManualTransaction.AuthCancellationRemarks,
                InvoiceNo = AManualTransaction.InvoiceNo,
                SalesId = AManualTransaction.SalesId,
                CreditNote = AManualTransaction.CreditNote != null ? AManualTransaction.CreditNote : "",
                OrderNo = AManualTransaction.OrderNo,
                Remarks = AManualTransaction.Remarks,
                BalanceAmount = AManualTransaction.BalanceAmount,
                TransactionCode= AManualTransaction .TransactionCode ,
                TransactionSubCode = AManualTransaction .TransactionSubCode ,
                IncomingTransTypeDesc = AManualTransaction.objSystemParameter != null ?  AManualTransaction.objSystemParameter.DefaultValue + "-" + AManualTransaction.objSystemParameter.Description : "",
                MerchantName = AManualTransaction.objMerchants != null ? AManualTransaction.objMerchants.MerchantCode + "-" + AManualTransaction.objMerchants.MerchantName : ""
            };
            return objManualTransactionDTO;
        }
        #endregion

        #region ManualTransactionDTO
        public static ReversalTransactionDTO ReversalTransactionToReversalTransactionDTO(ManualTransaction AManualTransaction)
        {
            ReversalTransactionDTO objManualTransactionDTO = new ReversalTransactionDTO()
            {
                BatchNo = AManualTransaction.BatchNo,
                CardNumber = AManualTransaction.CardNumber,
                RUT = AManualTransaction.RUT.Insert(AManualTransaction.RUT.Length - 1, "-"),
                TxnDate = AManualTransaction.TxnDate,
                AuthCode = AManualTransaction.AuthCode,
                OrganizationCode = AManualTransaction.OrganizationCode,
                IncomingTransType = AManualTransaction.IncomingTransType,
                MerchantCode = AManualTransaction.MerchantCode,
                MCCCode = AManualTransaction.MCCCode,
                SegmentCode = AManualTransaction.SegmentCode,
                TransType = AManualTransaction.TransType,
                Amount = AManualTransaction.Amount,
                IsManualEntry = AManualTransaction.IsManualEntry,
                PostedDate = AManualTransaction.PostedDate,
                IsPosted = AManualTransaction.IsPosted,
                CreatedBy = AManualTransaction.CreatedBy,
                CreatedOn = AManualTransaction.CreatedOn,
                LastUpdatedBy = AManualTransaction.LastUpdatedBy,
                LastUpdatedOn = AManualTransaction.LastUpdatedOn,
                ApprovedBy = AManualTransaction.ApprovedBy,
                ApprovedOn = AManualTransaction.ApprovedOn,
                ApprovalStatus = AManualTransaction.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AManualTransaction.ApprovalStatus),
                Time = AManualTransaction.TxnTime,
                AuthCancellationStatus = AManualTransaction.AuthCancellationStatus,
                ReferenceAuthCode = AManualTransaction.ReferenceAuthCode,
                AuthCancellationRemarks = AManualTransaction.AuthCancellationRemarks,
                InvoiceNo = AManualTransaction.InvoiceNo,
                SalesId = AManualTransaction.SalesId,
                CreditNote = AManualTransaction.CreditNote != null ? AManualTransaction.CreditNote : "",
                OrderNo = AManualTransaction.OrderNo,
                Remarks = AManualTransaction.Remarks,
                BalanceAmount = AManualTransaction.BalanceAmount,

                IncomingTransTypeDesc = AManualTransaction.objSystemParameter.DefaultValue + "-" + AManualTransaction.objSystemParameter.Description,
                MerchantName = AManualTransaction.objMerchants != null ? AManualTransaction.objMerchants.MerchantCode + "-" + AManualTransaction.objMerchants.MerchantName : ""
            };
            return objManualTransactionDTO;
        }
        #endregion

        #region CardEmbossingDTO
        public static CardEmbossingDTO CardEmbossingToCardEmbossingDTO(CardEmbossing ACardEmbossing)
        {
            CardEmbossingDTO objCardEmbossingDTO = new CardEmbossingDTO()
            {
                //RUT = ACardEmbossing.RUT,
                RUT = (ACardEmbossing.RUT == null) ? "" : ACardEmbossing.RUT.Insert(ACardEmbossing.RUT.Length - 1, "-"),
                CardNumber = ACardEmbossing.CardNumber,
                CardExpiryDate = ACardEmbossing.CardExpiryDate,

                Name = ACardEmbossing.Name,
                ProductName = ACardEmbossing.ProductName,
                ServiceCode = ACardEmbossing.ServiceCode,
                IsPrinted = ACardEmbossing.IsPrinted,
                Discretionary = ACardEmbossing.Discretionary,
                LRC = ACardEmbossing.LRC,
            };
            return objCardEmbossingDTO;
        }

        #endregion

        #region ForceAuthorization
        public static ForceAuthorizationDTO ForceAuthorizationToForceAuthorizationDTO(ForceAuthorization AForceAuthorization)
        {
            ForceAuthorizationDTO objForceAuthorizationDTO = new ForceAuthorizationDTO()
            {


                TransactionCode = AForceAuthorization.TransactionCode,
                CardNumber = AForceAuthorization.CardNumber,
                AccountNo = AForceAuthorization.AccountNo,
                AuthCode = AForceAuthorization.AuthCode,
                TxnDate = AForceAuthorization.TxnDate,
                ResponseCode = AForceAuthorization.ResponseCode,
                MerchantCode = AForceAuthorization.MerchantCode,
                MCCCode = AForceAuthorization.MCCCode,
                Amount = AForceAuthorization.Amount,
                BatchNo = AForceAuthorization.BatchNo,
                IsPosted = AForceAuthorization.IsPosted,
                AuthCancellationStatus = AForceAuthorization.AuthCancellationStatus



            };
            return objForceAuthorizationDTO;
        }

        #endregion

        #region ManualTransactionDTO
        public static EMITransactionDTO EMITransactionToEMITransactionDTO(EMITransaction AEMITransaction)
        {
            EMITransactionDTO objEMITransactionDTO = new EMITransactionDTO()
            {
                CardNumber = AEMITransaction.CardNumber,
                LoanBookedDate = AEMITransaction.LoanBookedDate,
                AuthCode = AEMITransaction.EMIAuthNo,
                PlanId = AEMITransaction.PlanId,
                MerchantCode = AEMITransaction.MerchantCode,
                LoanAmount = AEMITransaction.LoanAmount,
                TotalInstallment = AEMITransaction.TotalInstallment,
                CreatedBy = AEMITransaction.CreatedBy,
                CreatedOn = AEMITransaction.CreatedOn,
                LastUpdatedBy = AEMITransaction.LastUpdatedBy,
                LastUpdatedOn = AEMITransaction.LastUpdatedOn,
                ApprovedBy = AEMITransaction.ApprovedBy,
                ApprovedOn = AEMITransaction.ApprovedOn
            };

            return objEMITransactionDTO;
        }
        #endregion

        #region Transaction Adjustment DTOs
        #region BillingCycleViewDTO
        public static BillingCycleViewDTO BillingCycleViewToBillingCycleViewDTO(BillingCycleView ABillingCycleView)
        {
            BillingCycleViewDTO objBillingCycleViewDTO = new BillingCycleViewDTO()
            {
                AuthCode = ABillingCycleView.AuthCode,
                OrganizationCode = ABillingCycleView.OrganizationCode,
                CardNumber = ABillingCycleView.CardNumber,
                CycleCode = ABillingCycleView.CycleCode,
                CardTypeCode = ABillingCycleView.CardTypeCode,
                MemberTypeCode = ABillingCycleView.MemberTypeCode,
                CurrencyCode = ABillingCycleView.CurrencyCode,
                PrimaryCardNumber = ABillingCycleView.PrimaryCardNumber,
                ActualCardNumber = ABillingCycleView.ActualCardNumber,
                TransactionCode = ABillingCycleView.TransactionCode,
                TransactionDescription = ABillingCycleView.TransactionDescription,
                Amount = ABillingCycleView.Amount,
                SettledAmount = ABillingCycleView.SettledAmount,
                BillFlag = ABillingCycleView.BillFlag,
                PostedDate = ABillingCycleView.PostedDate,
                DrCrFlag = ABillingCycleView.DrCrFlag,
                BatchNo = ABillingCycleView.BatchNo,
                ChargeSlipDate = ABillingCycleView.ChargeSlipDate,
                MCCCode = ABillingCycleView.MCCCode,
                TransactionFlag = ABillingCycleView.TransactionFlag,

                CreatedBy = ABillingCycleView.CreatedBy,
                CreatedOn = ABillingCycleView.CreatedOn,
                LastUpdatedBy = ABillingCycleView.LastUpdatedBy,
                LastUpdatedOn = ABillingCycleView.LastUpdatedOn,
                ApprovedBy = ABillingCycleView.ApprovedBy,
                ApprovedOn = ABillingCycleView.ApprovedOn,
                ApprovalStatus = ABillingCycleView.ApprovalStatus,
            };
            return objBillingCycleViewDTO;
        }
        #endregion
        #region AdjustmentDTO
        public static AdjustmentDTO AdjustmentToAdjustmentDTO(Adjustment AAdjustment)
        {
            AdjustmentDTO objAdjustmentDTO = new AdjustmentDTO()
            {
                AdjustmentCode = AAdjustment.AdjustmentCode,
                AdjustmentNarration = AAdjustment.AdjustmentNarration,
                AuthCode = AAdjustment.AuthCode,
                CardNumber = AAdjustment.CardNumber,
                ActualAmount = AAdjustment.ActualAmount,
                AdjustedAmount = AAdjustment.AdjustedAmount,
                CloseFlag = "N",

                CreatedBy = AAdjustment.CreatedBy,
                CreatedOn = AAdjustment.CreatedOn,
                LastUpdatedBy = AAdjustment.LastUpdatedBy,
                LastUpdatedOn = AAdjustment.LastUpdatedOn,
                ApprovedBy = AAdjustment.ApprovedBy,
                ApprovedOn = AAdjustment.ApprovedOn,
                ApprovalStatus = AAdjustment.ApprovalStatus,
                QueueReferenceNo = AAdjustment.QueueReferenceNo
            };
            return objAdjustmentDTO;
        }
        #endregion
        #endregion

        #region EMIInstallment

        public static EMITransactionInstallmentDTO EMITransactionInstallmentToEMITransactionInstallmentDTO(EMITransactionInstallment AEMITransaction)
        {
            EMITransactionInstallmentDTO objEMITransactionDTO = new EMITransactionInstallmentDTO()
            {
                CardNumber = AEMITransaction.CardNumber,
                EMIAuthNo = AEMITransaction.EMIAuthNo,
                InstallmentNo = AEMITransaction.InstallmentNo,
                DueDate = AEMITransaction.DueDate,
                IsPosted = AEMITransaction.IsPosted,
                IsChecked = AEMITransaction.IsChecked,
                TotalPrincipalAmt = AEMITransaction.TotalPrincipalAmt,
                TotalInterestAmt = AEMITransaction.TotalInterestAmt,
                TotalPrepaymentPrincipalAmount = AEMITransaction.TotalPrepaymentPrincipalAmount,
                TotalPrepaymentInterestAmount = AEMITransaction.TotalPrepaymentInterestAmount
            };

            return objEMITransactionDTO;
        }



        public static EMIAuthorizationDTO EMIAuthorizationToEMIAuthorizationDTO(EMIAuthorization AEMITransaction)
        {
            EMIAuthorizationDTO objEMITransactionDTO = new EMIAuthorizationDTO()
            {
                CardNumber = AEMITransaction.CardNumber,
                EMIAuthNo = AEMITransaction.EMIAuthNo,
                LoanBookedDate = AEMITransaction.LoanBookedDate,
                AccountNo = AEMITransaction.AccountNo,
                MerchantCode = AEMITransaction.MerchantCode,
                IsClosed = AEMITransaction.IsClosed,
                //RUT = AEMITransaction.RUT,
                RUT = (AEMITransaction.RUT == null) ? "" : AEMITransaction.RUT.Insert(AEMITransaction.RUT.Length - 1, "-"),
                IsChecked = AEMITransaction.IsChecked,
                TransactionCode = AEMITransaction.TransactionCode,
                TotalPrincipalAmt = AEMITransaction.TotalPrincipalAmt,
                TotalInterestAmt = AEMITransaction.TotalInterestAmt,
                TotallInterestAmtPaid = AEMITransaction.TotallInterestAmtPaid,
                TotalPrincipalAmtPaid = AEMITransaction.TotalPrincipalAmtPaid,
                AdjInterestAmount = AEMITransaction.AdjInterestAmount,
                AdjPrincipalAmount = AEMITransaction.AdjPrincipalAmount
            };

            return objEMITransactionDTO;
        }

        public static PaymentHolidayDTO PaymentHolidayToPaymentHolidayDTO(PaymentHoliday APaymentHoliday)
        {
            PaymentHolidayDTO objEMITransactionDTO = new PaymentHolidayDTO()
            {
                SlNo = APaymentHoliday.SlNo,
                CardNumber = APaymentHoliday.CardNumber,
                ProductCode = APaymentHoliday.ProductCode,
                FromDate = APaymentHoliday.FromDate,
                ToDate = APaymentHoliday.ToDate,
                RUT = APaymentHoliday .RUT,
                CreatedBy = APaymentHoliday.CreatedBy,
                CreatedOn = APaymentHoliday.CreatedOn,
                LastUpdatedBy = APaymentHoliday.LastUpdatedBy,
                LastUpdatedOn = APaymentHoliday.LastUpdatedOn,
                ApprovedBy = APaymentHoliday.ApprovedBy,
                ApprovedOn = APaymentHoliday.ApprovedOn,
                ApprovalStatus = APaymentHoliday.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(APaymentHoliday.ApprovalStatus),
                ProductName = APaymentHoliday.ProductCode + " - " + APaymentHoliday.objProduct.ProductName
            };

            return objEMITransactionDTO;
        }
        #endregion

        #region AuthorizationCancellation
        public static AuthorizationCancellationDTO AuthorizationCancellationToAuthorizationCancellationDTO(AuthorizationCancellation objAuthorizationCancellation)
        {
            AuthorizationCancellationDTO objAuthorizationCancellationDTO = new AuthorizationCancellationDTO()
            {
                MerchantCode = objAuthorizationCancellation.MerchantCode,
                MerchantName = objAuthorizationCancellation.MerchantName,
                CardNumber = objAuthorizationCancellation.CardNumber,
                CardExpiryDate = objAuthorizationCancellation.CardExpiryDate,
                //RUT = objAuthorizationCancellation.RUT,
                RUT = (objAuthorizationCancellation.RUT == null) ? "" : objAuthorizationCancellation.RUT.Insert(objAuthorizationCancellation.RUT.Length - 1, "-"),
                CustomerName = objAuthorizationCancellation.CustomerName,

                Address = objAuthorizationCancellation.Address,
                City = objAuthorizationCancellation.City,
                PostalCode = objAuthorizationCancellation.PostalCode,
                Phone = objAuthorizationCancellation.Phone,

                AuthCode = objAuthorizationCancellation.AuthCode,
                Amount = objAuthorizationCancellation.Amount,
                TransactionCode = objAuthorizationCancellation.TransactionCode,
                TransactionDate = objAuthorizationCancellation.TransactionDate,
                TransactionType = objAuthorizationCancellation.TransactionType,
                SegmentName = objAuthorizationCancellation.SegmentName
            };

            return objAuthorizationCancellationDTO;
        }

        #endregion

        #region CustomerServiceDetailsToCustomerServiceDetailsDTO
        public static CustomerServiceDetailsDTO CustomerServiceDetailsToCustomerServiceDetailsDTO(CustomerServiceDetails ACustomerServiceDetails)
        {
            CustomerServiceDetailsDTO objCustomerServiceDetailsDTO = new CustomerServiceDetailsDTO()
            {
                ServiceID = ACustomerServiceDetails.ServiceID,

                CustomerId = ACustomerServiceDetails.CustomerId,
                ProductCode = ACustomerServiceDetails.ProductCode,
                ProductName = ACustomerServiceDetails.ProductName,
                CardNumber = ACustomerServiceDetails.CardNumber,
                AccountNumber = ACustomerServiceDetails.AccountNumber,
                CardTypeDesc = ACustomerServiceDetails.CardTypeDesc,
                CardMemberTypeCode = ACustomerServiceDetails.CardMemberTypeCode,
                ApplicationReferenceNo = ACustomerServiceDetails.ApplicationReferenceNo,
                SerialNo = ACustomerServiceDetails.SerialNo,
                CustomerName = ACustomerServiceDetails.CustomerName,
                DateOfBirth = ACustomerServiceDetails.DateOfBirth,
                Gender = ACustomerServiceDetails.Gender,
                //RUT = ACustomerServiceDetails.RUT,
                RUT = (ACustomerServiceDetails.RUT == null) ? "" : ACustomerServiceDetails.RUT.Insert(ACustomerServiceDetails.RUT.Length - 1, "-"),
                IsBlackListed = ACustomerServiceDetails.IsBlackListed,

                AddressDescription = ACustomerServiceDetails.AddressDescription,
                ReferenceAddressTypeCode = ACustomerServiceDetails.ReferenceAddressTypeCode,
                Village = ACustomerServiceDetails.Village,
                CountryCode = ACustomerServiceDetails.CountryCode,
                RegionName = ACustomerServiceDetails.RegionName,
                CityName = ACustomerServiceDetails.CityName,
                CommunaName = ACustomerServiceDetails.CommunaName,
                Number = ACustomerServiceDetails.Number,
                Street = ACustomerServiceDetails.Street,
                Block = ACustomerServiceDetails.Block,
                Town = ACustomerServiceDetails.Town,
                PostalCode = ACustomerServiceDetails.PostalCode,
                Phone = ACustomerServiceDetails.Phone,
                Mobile = ACustomerServiceDetails.Mobile,
                EmailID = ACustomerServiceDetails.EmailID,
                Nationality = ACustomerServiceDetails.Nationality,

                CardStatus = ACustomerServiceDetails.CardStatus,
                BlockStatusCode = ACustomerServiceDetails.BlockStatusCode,
                CardActivatedDate = ACustomerServiceDetails.CardActivatedDate,
                StatusChangeDate = ACustomerServiceDetails.StatusChangeDate,
                ParentCard = ACustomerServiceDetails.ParentCard,
                CardExpiryDate = ACustomerServiceDetails.CardExpiryDate,
                CardEmbossedDate = ACustomerServiceDetails.CardEmbossedDate,
                CardGeneratedDate = ACustomerServiceDetails .CardGeneratedDate ,
                EmbossedBranch = ACustomerServiceDetails.EmbossedBranch,
                BillingCycleCode = ACustomerServiceDetails.BillingCycleCode,
                CustomerSegmentName = ACustomerServiceDetails.CustomerSegmentName,
                CustomerSubSegmentName = ACustomerServiceDetails.CustomerSubSegmentName,
                ContractCode = ACustomerServiceDetails.ContractCode,
                ContractDescription = ACustomerServiceDetails.ContractDescription,

                MindueAmount = ACustomerServiceDetails.MindueAmount,
                DelinquencyAmount = ACustomerServiceDetails.DelinquencyAmount,
                OpeningRevolvingBalance = ACustomerServiceDetails.OpeningRevolvingBalance,
                CreditLimit = ACustomerServiceDetails.CreditLimit,
                CashLimit = ACustomerServiceDetails.CashLimit,
                CashOTB = ACustomerServiceDetails.CashOTB,
                CreditOTB = ACustomerServiceDetails.CreditOTB,
                BillDate = ACustomerServiceDetails.BillDate,

                TotalBillAmount = ACustomerServiceDetails.TotalBillAmount,
                CurrentBalance = ACustomerServiceDetails.CurrentBalance,
                UnMatchedAuthAmount = ACustomerServiceDetails.UnMatchedAuthAmount,
                CurrentDelinquentMonth = ACustomerServiceDetails.CurrentDelinquentMonth,
                HighestBilledAmount = ACustomerServiceDetails.HighestBilledAmount,
                HighestClearedAmount = ACustomerServiceDetails.HighestClearedAmount,
                BounceCount = ACustomerServiceDetails.BounceCount,
                BounceDate = ACustomerServiceDetails.BounceDate,
                DPDCount = ACustomerServiceDetails.DPDCount,

                UnbilledPurchaseCount = ACustomerServiceDetails.UnbilledPurchaseCount,
                UnbilledPurchaseAmount = ACustomerServiceDetails.UnbilledPurchaseAmount,
                UnBilledFinancialCreditAmount = ACustomerServiceDetails.UnBilledFinancialCreditAmount,
                UnBilledFinancialCreditCount = ACustomerServiceDetails.UnBilledFinancialCreditCount,
                UnbilledCreditNoteCount = ACustomerServiceDetails.UnbilledCreditNoteCount,
                UnbilledCreditNoteAmount = ACustomerServiceDetails.UnbilledCreditNoteAmount,
                UnbilledServiceTaxCount = ACustomerServiceDetails.UnbilledServiceTaxCount,
                UnbilledServiceTaxAmount = ACustomerServiceDetails.UnbilledServiceTaxAmount,
                UnbilledCashCount = ACustomerServiceDetails.UnbilledCashCount,
                UnbilledCashAmount = ACustomerServiceDetails.UnbilledCashAmount,
                UnbilledFeeCount = ACustomerServiceDetails.UnbilledFeeCount,
                UnbilledFeeAmount = ACustomerServiceDetails.UnbilledFeeAmount,
                UnbilledOtherChargesCount = ACustomerServiceDetails.UnbilledOtherChargesCount,
                UnbilledOtherChargesAmount = ACustomerServiceDetails.UnbilledOtherChargesAmount,
                UnbilledDebitAdjustmentCount = ACustomerServiceDetails.UnbilledDebitAdjustmentCount,
                UnbilledDebitAdjustmentAmount = ACustomerServiceDetails.UnbilledDebitAdjustmentAmount,
                UnbilledCreditAdjustmentCount = ACustomerServiceDetails.UnbilledCreditAdjustmentCount,
                UnbilledCreditAdjustmentAmount = ACustomerServiceDetails.UnbilledCreditAdjustmentAmount,
                UnbilledPaymentCount = ACustomerServiceDetails.UnbilledPaymentCount,
                UnbilledPayments = ACustomerServiceDetails.UnbilledPayments,
                UnbilledEMICount = ACustomerServiceDetails.UnbilledEMICount,
                UnbilledEMIAmount = ACustomerServiceDetails.UnbilledEMIAmount,
                InterestCount = ACustomerServiceDetails.InterestCount,
                InterestAmount = ACustomerServiceDetails.InterestAmount,
                UnPostedAmount = ACustomerServiceDetails .UnPostedAmount,
                UnPostedCount = ACustomerServiceDetails .UnPostedCount,
                PaymentAgainstRefinanceCount = ACustomerServiceDetails.PaymentAgainstRefinanceCount,
                PaymentAgainstRefinanceAmount = ACustomerServiceDetails.PaymentAgainstRefinanceAmount,
                PaymentAgainstRenegotiationCount = ACustomerServiceDetails.PaymentAgainstRenegotiationCount,
                PaymentAgainstRenegotiationAmount = ACustomerServiceDetails.PaymentAgainstRenegotiationAmount,
                UnbilledPaymentRevAmount= ACustomerServiceDetails .UnbilledPaymentRevAmount,
                UnbilledPaymentRevCount = ACustomerServiceDetails .UnbilledPaymentRevCount,
                CustomerStatus = ACustomerServiceDetails.CustomerStatus,
                ExcessCredit = ACustomerServiceDetails.ExcessCredit
            };

            return objCustomerServiceDetailsDTO;
        }
        #endregion

        #region DiscountPercentageDTO
        public static DiscountPercentageDTO DiscountPercentageToDiscountPercentageDTO(DiscountPercentage ADiscountPercentage)
        {
            DiscountPercentageDTO objDiscountPercentageDTO = new DiscountPercentageDTO()
            {
                DiscountPercentageId = ADiscountPercentage.DiscountPercentageId,
                ProductCode = ADiscountPercentage.ProductCode,
                SegmentCode = ADiscountPercentage.SegmentCode,
                SubSegmentCode = ADiscountPercentage.SubSegmentCode,
                ValidFrom = ADiscountPercentage.ValidFrom,
                ValidTo = ADiscountPercentage.ValidTo,

                CreatedBy = ADiscountPercentage.CreatedBy,
                CreatedOn = ADiscountPercentage.CreatedOn,
                LastUpdatedBy = ADiscountPercentage.LastUpdatedBy,
                LastUpdatedOn = ADiscountPercentage.LastUpdatedOn,
                ApprovalStatus = ADiscountPercentage.ApprovalStatus,
                ApprovedBy = ADiscountPercentage.ApprovedBy,
                ApprovedOn = ADiscountPercentage.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ADiscountPercentage.ApprovalStatus),
                ProductName = (ADiscountPercentage.ProductCode != null) ? ADiscountPercentage.ProductCode + " - " + ADiscountPercentage.Product.ProductName : null,
                SegmentName = (ADiscountPercentage.SegmentCode != null) ? ADiscountPercentage.CustomerSegment.SegmentName : null,
                SubSegmentName = (ADiscountPercentage.SubSegmentCode != null) ? ADiscountPercentage.CustomerSubSegment.SubSegmentName : null,
            };

            objDiscountPercentageDTO.DiscountPercentageDetailsDTO.AddRange(DiscountPercentageDetailsToDiscountPercentageDetailsDTO(ADiscountPercentage.DiscountPercentageDetails));

            return objDiscountPercentageDTO;
        }

        public static List<DiscountPercentageDetailsDTO> DiscountPercentageDetailsToDiscountPercentageDetailsDTO(List<DiscountPercentageDetails> ADiscountPercentageDetails)
        {
            List<DiscountPercentageDetailsDTO> listDiscountPercentageDetailsDTO = new List<DiscountPercentageDetailsDTO>();

            ADiscountPercentageDetails.ForEach
            (
                x => listDiscountPercentageDetailsDTO.Add
                (
                    new DiscountPercentageDetailsDTO
                    {
                        DiscountPercentageDetailsId = x.DiscountPercentageDetailsId,
                        DiscountPercentageId = x.DiscountPercentageId,
                        PurchaseAmountFrom = x.PurchaseAmountFrom,
                        PurchaseAmountTo = x.PurchaseAmountTo,
                        DPDDaysFrom = x.DPDDaysFrom,
                        DPDDaysTo = x.DPDDaysTo,
                        DiscountPercentage = x.DiscountPercentage
                    }
                )
            );

            return listDiscountPercentageDetailsDTO;
        }

        #endregion

        #region NotepadEntryToNotepadEntryDTO
        public static NotepadEntryDTO NotepadEntryToNotepadEntryDTO(NotepadEntry ANotepadEntry)
        {
            NotepadEntryDTO objNotepadEntryDTO = new NotepadEntryDTO()
            {
                NotepadEntryId = ANotepadEntry.NotepadEntryId,
                CustomerId = ANotepadEntry.CustomerId,
                CardNumber = ANotepadEntry.CardNumber,
                MessageType = ANotepadEntry.MessageType == "OTHR" ? "Other" : "Address",
                MessageDescription = ANotepadEntry.MessageDescription,
                CreatedBy = ANotepadEntry.CreatedBy,
                CreatedOn = ANotepadEntry.CreatedOn,
                LastUpdatedBy = ANotepadEntry.LastUpdatedBy,
                LastUpdatedOn = ANotepadEntry.LastUpdatedOn,
                MessageStatus = ANotepadEntry.MessageStatus
            };
            return objNotepadEntryDTO;
        }
        #endregion

        #region CardClosureToCardClosureDTO
        public static CardClosureDTO CardClosureToCardClosureDTO(CardClosure ACardClosure)
        {
            CardClosureDTO objCardClosureDTO = new CardClosureDTO()
            {
                ClosureID = ACardClosure.ClosureID,
                CustomerId = ACardClosure.CustomerId,
                CardNumber = ACardClosure.CardNumber,

                ClosureReasonCode = ACardClosure.ClosureReasonCode,
                ClosuredDate = ACardClosure.ClosureDate,
                IsCardReceived = ACardClosure.IsCardReceived,
                IsDocumentReceived = ACardClosure.IsDocumentReceived,
                CreatedBy = ACardClosure.CreatedBy,
                CreatedOn = ACardClosure.CreatedOn,
                LastUpdatedBy = ACardClosure.LastUpdatedBy,
                LastUpdatedOn = ACardClosure.LastUpdatedOn,
                Remarks = ACardClosure.Remarks,

            };
            return objCardClosureDTO;
        }
        #endregion

        #region CardReactivationToCardReactivationDTO
        public static CardReactivationDTO CardReactivationToCardReactivationDTO(CardReactivation ACardReactivation)
        {
            CardReactivationDTO objCardReactivationDTO = new CardReactivationDTO()
            {
                ClosureID = ACardReactivation.ClosureID,
                CustomerId = ACardReactivation.CustomerId,
                CardNumber = ACardReactivation.CardNumber,
                ClosureReasondesc = ACardReactivation.objCardClosure.objClosureReason.ReasonDescription,
                ReactivationID = ACardReactivation.ReactivationID,
                ReactivationReason = ACardReactivation.ReactivationReasonCode,
                CreatedBy = ACardReactivation.CreatedBy,
                CreatedOn = ACardReactivation.ReactivatedOn,
                LastUpdatedBy = ACardReactivation.LastUpdatedBy,
                LastUpdatedOn = ACardReactivation.LastUpdatedOn,
                ReactivationRemarks = ACardReactivation.ReactivationRemarks,
                ApprovalStatus = ACardReactivation.ApprovalStatus,
                ApprovalStatusdesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ACardReactivation.ApprovalStatus)

            };
            return objCardReactivationDTO;
        }
        #endregion
        
        #region FeeType
        public static FeeTypesDTO ConvertFeeTypesToFeeTypesDTO(FeeTypes AFeeTypes)
        {
            FeeTypesDTO objFeeTypesDTO = new FeeTypesDTO()
            {
                FeeTypeCode = AFeeTypes.FeeTypeCode,
                FeeTypeDescription = AFeeTypes.FeeTypeDescription
            };

            return objFeeTypesDTO;
        }
        #endregion

        #region Block Segment Card
        public static BlockSegmentCardDTO BlockSegmentCardtoBlockSegmentCardDTO(BlockSegmentCard ABlockSegmentCard)
        {
            BlockSegmentCardDTO objBlockSegmentCardDTO = new BlockSegmentCardDTO()
            {
                BlockSegmentCardId = ABlockSegmentCard.BlockSegmentCardId,
                CardNumber = ABlockSegmentCard.CardNumber,
                AccountNumber = ABlockSegmentCard.AccountNumber,
                BlockedType = ABlockSegmentCard.BlockedType,
                BlockCode = ABlockSegmentCard.BlockCode,
                CreditSegmentCode = ABlockSegmentCard.CreditSegmentCode,
                IsBlocked = ABlockSegmentCard.IsBlocked,
                CreatedBy = ABlockSegmentCard.CreatedBy,
                CreatedOn = ABlockSegmentCard.CreatedOn,
                LastUpdatedBy = ABlockSegmentCard.LastUpdatedBy,
                LastUpdatedOn = ABlockSegmentCard.LastUpdatedOn

            };
            return objBlockSegmentCardDTO;

        }
        #endregion

        #region OnlinePaymentDTO
        public static OnlinePaymentDTO OnlinePaymentToOnlinePaymentDTO(OnlinePayment AOnlinePayment)
        {
            OnlinePaymentDTO objOnlinePaymentDTO = new OnlinePaymentDTO()
            {
                OnlinePaymentId = AOnlinePayment.OnlinePaymentId,
                CardNumber = AOnlinePayment.CardNumber,
                //RUT = AOnlinePayment.RUT,
                RUT = (AOnlinePayment.RUT == null) ? "" : AOnlinePayment.RUT.Insert(AOnlinePayment.RUT.Length - 1, "-"),
                PaymentModeCode = AOnlinePayment.PaymentModeCode,
                ChequeDate = AOnlinePayment.ChequeDate,
                ChequeNo = AOnlinePayment.ChequeNo,
                PaymentCurrency = AOnlinePayment.PaymentCurrency,
                PaymentAmount = AOnlinePayment.PaymentAmount,
                TxnTime = AOnlinePayment.TxnTime,

                CreatedBy = AOnlinePayment.CreatedBy,
                CreatedOn = AOnlinePayment.CreatedOn,
                LastUpdatedBy = AOnlinePayment.LastUpdatedBy,
                LastUpdatedOn = AOnlinePayment.LastUpdatedOn,
                ApprovedBy = AOnlinePayment.ApprovedBy,
                ApprovedOn = AOnlinePayment.ApprovedOn,
                ApprovalStatus = AOnlinePayment.ApprovalStatus
            };
            return objOnlinePaymentDTO;
        }
        #endregion

        #region RefinancingDTO
        public static RefinancingDTO RefinancingToRefinancingDTO(Refinancing ARefinancing)
        {
            RefinancingDTO objRefinancingDTO = new RefinancingDTO()
            {
                CardNumber = ARefinancing.CardNumber,
                ProductCode = ARefinancing.ProductCode,
                CustomerSegmentCode = ARefinancing.CustomerSegmentCode,
                CustomerSubSegmentCode = ARefinancing.CustomerSubSegmentCode,
                //RUT = ARefinancing.RUT,
                RUT = (ARefinancing.RUT == null) ? "" : ARefinancing.RUT.Insert(ARefinancing.RUT.Length - 1, "-"),
                CustomerCardName = ARefinancing.CustomerCardName,
                RefinanceCount = ARefinancing.RefinanceCount,
                DPDCount = ARefinancing.DPDCount,
                BilledOS = ARefinancing.BilledOS,
                MinDueAmount = ARefinancing.MinDueAmount,
                //TotalOS = ARefinancing.TotalOS,
                PrincipalAmount = ARefinancing.PrincipalAmount,
                InterestAmount = ARefinancing.InterestAmount,
                OtherCharge = ARefinancing.OtherCharge,
                //DelinqInterest = ARefinancing.DelinqInterest,
                DownPayment = ARefinancing.DownPayment,
                NetAmount = ARefinancing.NetAmount,
                //NetAmount = ARefinancing.NetAmount == 0 ? ARefinancing.TotalOS : ARefinancing.NetAmount,
                DiscountPercentage = ARefinancing.DiscountPercentage,
                DiscountAmount = ARefinancing.DiscountAmount,
                NetRefinancedAmount = ARefinancing.NetRefinancedAmount,
                //NetRefinancedAmount = ARefinancing.NetRefinancedAmount == 0 ? ARefinancing.TotalOS : ARefinancing.NetRefinancedAmount,
                Tenure = ARefinancing.Tenure,
                TransactionCode = ARefinancing.TransactionCode,
                InterestPercentage = ARefinancing.InterestPercentage
            };
            return objRefinancingDTO;
        }
        #endregion

        #region RenegotiationDTO
        public static RenegotiationDTO RenegotiationToRenegotiationDTO(Renegotiation ARenegotiation)
        {
            RenegotiationDTO objRenegotiationDTO = new RenegotiationDTO()
            {
                CardNumber = ARenegotiation.CardNumber,
                ProductCode = ARenegotiation.ProductCode,
                CustomerSegmentCode = ARenegotiation.CustomerSegmentCode,
                CustomerSubSegmentCode = ARenegotiation.CustomerSubSegmentCode,
                //RUT = ARenegotiation.RUT,
                RUT = (ARenegotiation.RUT == null) ? "" : ARenegotiation.RUT.Insert(ARenegotiation.RUT.Length - 1, "-"),
                CustomerCardName = ARenegotiation.CustomerCardName,

                Due = ARenegotiation.Due,
                OverDue = ARenegotiation.OverDue,
                WriteOff = ARenegotiation.WriteOff,
                DPDCount = ARenegotiation.DPDCount,
                UnBilledOS = ARenegotiation.UnBilledOS,
                BilledOS = ARenegotiation.BilledOS,
                TotalOS = ARenegotiation.TotalOS,
                PrincipalAmount = ARenegotiation.PrincipalAmount,
                InterestAmount = ARenegotiation.InterestAmount,
                CollectionCharges = ARenegotiation.CollectionCharges,
                DelinqInterest = ARenegotiation.DelinqInterest,

                DownPayment = ARenegotiation.DownPayment,
                NetAmount = ARenegotiation.NetAmount == 0 ? ARenegotiation.TotalOS : ARenegotiation.NetAmount,
                DiscountPercentage = ARenegotiation.DiscountPercentage,
                DiscountAmount = ARenegotiation.DiscountAmount,
                NetRenegotiatedAmount = ARenegotiation.NetRenegotiatedAmount == 0 ? ARenegotiation.TotalOS : ARenegotiation.NetRenegotiatedAmount,
                Tenure = ARenegotiation.Tenure,
                TransactionCode = ARenegotiation.TransactionCode,
                InterestPercentage = ARenegotiation.InterestPercentage
            };
            return objRenegotiationDTO;
        }
        #endregion

        #region StatementGenerationDTO
        public static StatementGenerationDTO StatementGenerationToStatementGenerationDTO(StatementGeneration AStatementGeneration)
        {
            StatementGenerationDTO objStatementGenerationDTO = new StatementGenerationDTO()
            {
                CardNumber = AStatementGeneration.CardNumber,
                CustomerCardName = AStatementGeneration.CustomerCardName,
                RUT = AStatementGeneration.RUT.Insert(AStatementGeneration.RUT.Length - 1, "-"),
                BillDate = AStatementGeneration.BillDate,
                BillMonth = AStatementGeneration.BillMonth,
                MinDueAmount = AStatementGeneration.MinDueAmount,
                BillYear = AStatementGeneration.BillYear,
                OpeningRevolvingBalance = AStatementGeneration.OpeningRevolvingBalance,
                ClosingRevolvingBalance = AStatementGeneration.ClosingRevolvingBalance,
                PaymentDueDate = AStatementGeneration.PaymentDueDate,
                IsChecked = false,
                BillMonthDesc = AStatementGeneration.BillMonth == 1 ? "January" :
                                    AStatementGeneration.BillMonth == 2 ? "February" :
                                    AStatementGeneration.BillMonth == 3 ? "March" :
                                    AStatementGeneration.BillMonth == 4 ? "April" :
                                    AStatementGeneration.BillMonth == 5 ? "May" :
                                    AStatementGeneration.BillMonth == 6 ? "June" :
                                    AStatementGeneration.BillMonth == 7 ? "July" :
                                    AStatementGeneration.BillMonth == 8 ? "August" :
                                    AStatementGeneration.BillMonth == 9 ? "September" :
                                    AStatementGeneration.BillMonth == 10 ? "October" :
                                    AStatementGeneration.BillMonth == 11 ? "November" :
                                    "December"

            };
            return objStatementGenerationDTO;
        }
        #endregion

        #region MerchatsDTO
        public static MerchantsDTO MerchantsToMerchantsDTO(Merchants AMerchant)
        {
            MerchantsDTO objMerchantDTO = new MerchantsDTO()
            {
                MerchantCode = AMerchant.MerchantCode,
                OrganizationCode = AMerchant.OrganizationCode,
                MerchantName = AMerchant.MerchantName,
                CountryCode = AMerchant.CountryCode,
                RegionCode = AMerchant.RegionCode,
                CityCode = AMerchant.CityCode,
                CommunaCode = AMerchant.CommunaCode,
                SettlementInDays = AMerchant.SettlementInDays,
                CommercialUEN = AMerchant.CommercialUEN,
                CommercialUENName = AMerchant.CommercialUENName,

                CreatedBy = AMerchant.CreatedBy,
                CreatedOn = AMerchant.CreatedOn,
                LastUpdatedBy = AMerchant.LastUpdatedBy,
                LastUpdatedOn = AMerchant.LastUpdatedOn,
                ApprovedBy = AMerchant.ApprovedBy,
                ApprovedOn = AMerchant.ApprovedOn,
                ApprovalStatus = AMerchant.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AMerchant.ApprovalStatus),

                //CityDesc = (AMerchant.City != null) ? AMerchant.City.CityName : null,
                CountryDesc = (AMerchant.CountryCode != null) ? AMerchant.Country.CountryName : null,
                //RegionDesc = (AMerchant.RegionCode != null) ? AMerchant.Region.RegionName : null,
                //CommunaDesc = (AMerchant.CommunaCode != null) ? AMerchant.Communa.CommunaName : null,

            };
            return objMerchantDTO;
        }
        #endregion

        #region MerchatDiscountDTO
        public static MerchantDiscountDTO MerchantDiscountToMerchantDiscountDTO(MerchantDiscount AMerchantDiscount)
        {
            MerchantDiscountDTO objMerchantDiscountDTO = new MerchantDiscountDTO()
            {
                MerchantDiscountId = AMerchantDiscount.MerchantDiscountId,
                OrganizationCode = AMerchantDiscount.OrganizationCode,
                MerchantCode = AMerchantDiscount.MerchantCode,               
                EffectiveFromDate = AMerchantDiscount.EffectiveFromDate,
              
                TransactionCheckFrequencyCode = AMerchantDiscount.TransactionCheckFrequencyCode,
                DiscountRate = AMerchantDiscount.DiscountRate,

                CreatedBy = AMerchantDiscount.CreatedBy,
                CreatedOn = AMerchantDiscount.CreatedOn,
                LastUpdatedBy = AMerchantDiscount.LastUpdatedBy,
                LastUpdatedOn = AMerchantDiscount.LastUpdatedOn,
                ApprovedBy = AMerchantDiscount.ApprovedBy,
                ApprovedOn = AMerchantDiscount.ApprovedOn,
                ApprovalStatus = AMerchantDiscount.ApprovalStatus,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AMerchantDiscount.ApprovalStatus),

                MerchantName = (AMerchantDiscount.MerchantCode != null) ? AMerchantDiscount.ObjMerchant.MerchantName : null,
                TransactionCheckFrequencyDesc = (AMerchantDiscount.TransactionCheckFrequencyCode != null) ? AMerchantDiscount.ObjTransactionCheckFrequency.TransactionCheckFrequencyDescription : null
            };
            return objMerchantDiscountDTO;
        }
        #endregion

        #region ChangeBusinessDateToChangeBusinessDateDTO
        public static ChangeBusinessDateDTO ChangeBusinessDateToChangeBusinessDateDTO(ChangeBusinessDate AChangeBusinessDate)
        {
            ChangeBusinessDateDTO objChangeBusinessDateDTO = new ChangeBusinessDateDTO()
            {
                OrganizationCode = AChangeBusinessDate.OrganizationCode,
                BusinessDate = AChangeBusinessDate.BusinessDate
            };
            return objChangeBusinessDateDTO;
        }
        #endregion

        #region UFRateSettingsToUFRateSettingsDTO
        public static UFRateSettingsDTO UFRateSettingsToUFRateSettingsDTO(UFRateSettings AUFRateSettings)
        {
            UFRateSettingsDTO objUFRateSettingsDTO = new UFRateSettingsDTO()
            {
                OrganizationCode = AUFRateSettings.OrganizationCode,
                EffectiveDate = AUFRateSettings.EffectiveDate,
                UFRate = AUFRateSettings.UFRate,

                CreatedBy = AUFRateSettings.CreatedBy,
                CreatedOn = AUFRateSettings.CreatedOn,
                LastUpdatedBy = AUFRateSettings.LastUpdatedBy,
                LastUpdatedOn = AUFRateSettings.LastUpdatedOn,
            };
            return objUFRateSettingsDTO;
        }
        #endregion
        
        #region Branch
        public static SalesIdDTO SalesIdToSalesIdDTO(SaleId ASaleId)
        {

            SalesIdDTO objBranchDTO = new SalesIdDTO()
            {
                OrganizationCode = ASaleId.OrganizationCode,
                BranchCode = ASaleId.BranchCode,
                SalesId = ASaleId.SalesId,
                RUT = ASaleId.RUT.Insert(ASaleId.RUT.Length - 1, "-"),          
                CommunaCode = ASaleId.CommunaCode,
                CityCode = ASaleId.CityCode,
                Address = ASaleId.Address,
                ApprovalStatus = ASaleId.ApprovalStatus,
                CreatedBy = ASaleId.CreatedBy,
                CreatedOn = ASaleId.CreatedOn,
                LastUpdatedBy = ASaleId.LastUpdatedBy,
                LastUpdatedOn = ASaleId.LastUpdatedOn,
                ApprovedBy = ASaleId.ApprovedBy,
                ApprovedOn = ASaleId.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ASaleId.ApprovalStatus)
            };
            return objBranchDTO;

        }
        #endregion

        #region StatementInterest

        public static StatementInterestDTO StatementInterestToStatementInterestDTO(StatementInterest AStatementInterest)
        {
            StatementInterestDTO objStatementInterestDTO = new StatementInterestDTO()
            {
                OrganizationCode = AStatementInterest.OrganizationCode,
                BillingMonth = AStatementInterest.BillingMonth,
                BillingYear = AStatementInterest.BillingYear,
                InstallmentInterest = AStatementInterest.InstallmentInterest,
                RevolvingInterest = AStatementInterest.RevolvingInterest,
                RevolvingCAE = AStatementInterest.RevolvingCAE,
                InstallmentCAE=AStatementInterest.InstallmentCAE,
                ApprovalStatus = AStatementInterest.ApprovalStatus,
                CreatedBy = AStatementInterest.CreatedBy,
                CreatedOn = AStatementInterest.CreatedOn,
                LastUpdatedBy = AStatementInterest.LastUpdatedBy,
                LastUpdatedOn = AStatementInterest.LastUpdatedOn,
                ApprovedBy = AStatementInterest.ApprovedBy,
                ApprovedOn = AStatementInterest.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AStatementInterest.ApprovalStatus),
                BillMonthDesc = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert .ToInt32 ( AStatementInterest.BillingMonth.ToString ()))
            };
            return objStatementInterestDTO;
        }
        #endregion

        #region DirectSalesAgent
        public static DirectSalesAgentDTO DirectSalesAgentToDirectSalesAgentDTO(DirectSalesAgent ADirectSalesAgent)
        {

            DirectSalesAgentDTO objDirectSalesAgentDTO = new DirectSalesAgentDTO()
            {
                OrganizationCode = ADirectSalesAgent.OrganizationCode,
                DSACode = ADirectSalesAgent.DSACode,
                DSAName = ADirectSalesAgent.DSAName,
                UEN = ADirectSalesAgent.UEN,
                RUT = ADirectSalesAgent.RUT.Insert(ADirectSalesAgent.RUT.Length - 1, "-"),          
                CommercialBranchCode = ADirectSalesAgent.CommercialBranchCode,

                ApprovalStatus = ADirectSalesAgent.ApprovalStatus,
                CreatedBy = ADirectSalesAgent.CreatedBy,
                CreatedOn = ADirectSalesAgent.CreatedOn,
                LastUpdatedBy = ADirectSalesAgent.LastUpdatedBy,
                LastUpdatedOn = ADirectSalesAgent.LastUpdatedOn,
                ApprovedBy = ADirectSalesAgent.ApprovedBy,
                ApprovedOn = ADirectSalesAgent.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(ADirectSalesAgent.ApprovalStatus)
            };
            return objDirectSalesAgentDTO;

        }

        #endregion

        #region AdjustmentFinancialAccountDTO
        public static AdjustmentFinancialAccountDTO AdjustmentFinancialAccountToAdjustmentFinancialAccountDTO(AdjustmentFinancialAccount AAdjustmentFinancialAccount)
        {
            AdjustmentFinancialAccountDTO objAdjustmentFinancialAccountDTO = new AdjustmentFinancialAccountDTO()
            {
                AdjustmentFinancialAccountId = AAdjustmentFinancialAccount.AdjustmentFinancialAccountId,
                BranchCode = AAdjustmentFinancialAccount.BranchCode,
                ModuleName = AAdjustmentFinancialAccount.ModuleName,
                ProductCode = AAdjustmentFinancialAccount.ProductCode,
                TransactionSubCodeId = AAdjustmentFinancialAccount.TransactionSubCodeId,
                AdjustmentCode = AAdjustmentFinancialAccount.AdjustmentCode,
                AdjustmentFinancialActivityCode = AAdjustmentFinancialAccount.AdjustmentFinancialActivityCode,
                DebitAccountNr = AAdjustmentFinancialAccount.DebitAccountNr,
                DebitNarration = AAdjustmentFinancialAccount.DebitNarration,
                CreditAccountNr = AAdjustmentFinancialAccount.CreditAccountNr,
                CreditNarration = AAdjustmentFinancialAccount.CreditNarration,
                EffectiveDate = AAdjustmentFinancialAccount.EffectiveDate,

                CreatedBy = AAdjustmentFinancialAccount.CreatedBy,
                CreatedOn = AAdjustmentFinancialAccount.CreatedOn,
                LastUpdatedBy = AAdjustmentFinancialAccount.LastUpdatedBy,
                LastUpdatedOn = AAdjustmentFinancialAccount.LastUpdatedOn,
                ApprovalStatus = AAdjustmentFinancialAccount.ApprovalStatus,
                ApprovedBy = AAdjustmentFinancialAccount.ApprovedBy,
                ApprovedOn = AAdjustmentFinancialAccount.ApprovedOn,
                ApprovalStatusDesc = CC_Ingenio.Common.Utils.Utils.GetRecordStatus(AAdjustmentFinancialAccount.ApprovalStatus),
                BranchDesc = (AAdjustmentFinancialAccount.BranchCode != null && AAdjustmentFinancialAccount.BranchCode != "ALL") ? AAdjustmentFinancialAccount.BranchCode + " - " + AAdjustmentFinancialAccount.Branch.BranchName : AAdjustmentFinancialAccount.BranchCode != "ALL" ? null : AAdjustmentFinancialAccount.BranchCode,
                ProductName = (AAdjustmentFinancialAccount.ProductCode != null && AAdjustmentFinancialAccount.ProductCode != "ALL") ? AAdjustmentFinancialAccount.ProductCode + " - " + AAdjustmentFinancialAccount.Product.ProductName : AAdjustmentFinancialAccount.ProductCode != "ALL" ? null : AAdjustmentFinancialAccount.ProductCode,
                FinancialActivityDesc = (AAdjustmentFinancialAccount.TransactionSubCodeId != 0) ? AAdjustmentFinancialAccount.TransactionSubCode.TransactionCode + AAdjustmentFinancialAccount.TransactionSubCode.SubCode + " - " + AAdjustmentFinancialAccount.TransactionSubCode.Description : null
            };
            return objAdjustmentFinancialAccountDTO;
        }
        #endregion

    }
}
