using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    public class CustomerFactory
    {
        public static Customer CreateCustomer(int ACustomerId, int? AParentCustomerId,
            string ABranchCode,string ACampaignSource,string AChannelCode,string AApplicationReferenceNo,string AOrganizationCode,
            string AProductMemberTypeCode, string AProductTypeCode, string AProductCategoryCode, string AProductCode, string ARUT, 
            string ASerialNo, DateTime? AExpiryDate, string ACorporateRUT, string ASalesId,string APhysicalApplicationRefNo,DateTime? APhysicalApplicationRefDate,
            string AExternalCivilRegistryResult,string AInputterId,DateTime? AInputterDate, decimal ACreditScore,string ACustomerType, 
            string AName, string APaternalSurname, string AMaternalSurname,string ANationality, DateTime? ADateOfBirth, string AGender, 
            string AMaritalStatus, decimal? ANumberofDependents, string AEducation,string AProfession, string AHousing, string AEmailID, 
            string ALegacyCardNumber,bool AIsActive, bool AIsBlackListed, string ACreatedBy, string AApprovalStatus, 
            string ALastUpdatedBy,string AApprovedBy = null)
        {
            Customer objCustomer = new Customer()
            {
                CustomerId = ACustomerId,
                SalesId= ASalesId ,
                ParentCustomerId = AParentCustomerId,
                BranchCode = ABranchCode,
                CampaignSource = ACampaignSource,
                ChannelCode = AChannelCode,
                ApplicationReferenceNo = AApplicationReferenceNo,
                OrganizationCode = AOrganizationCode,
                ProductMemberTypeCode = AProductMemberTypeCode,
                ProductTypeCode  = AProductTypeCode,
                ProductCategoryCode = AProductCategoryCode,
                ProductCode = AProductCode,
                RUT = ARUT.Contains("-") ? ARUT.Replace("-", string.Empty) : ARUT,
                SerialNo = ASerialNo,
                ExpiryDate = AExpiryDate,
                CorporateRUT = ACorporateRUT,
                PhysicalApplicationRefNo = APhysicalApplicationRefNo,
                PhysicalApplicationRefDate = APhysicalApplicationRefDate,
                ExternalCivilRegistryResult = AExternalCivilRegistryResult,
                InputterId = AInputterId,
                InputterDate = AInputterDate,
                CreditScore = ACreditScore,

                CustomerType = ACustomerType,
                Name = AName,
                PaternalSurname = APaternalSurname,
                MaternalSurname = AMaternalSurname,
                Nationality = ANationality,
                DateOfBirth = ADateOfBirth,
                Gender = AGender,
                MaritalStatus = AMaritalStatus,
                NumberofDependents = ANumberofDependents,
                Education = AEducation,
                Profession = AProfession,
                Housing = AHousing,
                EmailID = AEmailID,
                IsActive = AIsActive,
                IsBlackListed = AIsBlackListed,
                CreatedBy = ACreatedBy,
                CreatedOn = CC_Ingenio.Common.Utils.  Utils.BusinessDate,
                ApprovedBy = AApprovedBy,
                ApprovedOn = null,
                ApprovalStatus = AApprovalStatus,
                LastUpdatedBy = ALastUpdatedBy,
                LastUpdatedOn = CC_Ingenio.Common.Utils. Utils.BusinessDate,
                LegacyCardNumber = ALegacyCardNumber
            };

            return objCustomer;
        }
    }
}