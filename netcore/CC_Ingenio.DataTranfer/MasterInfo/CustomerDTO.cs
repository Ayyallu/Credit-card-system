using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }
        public int? ParentCustomerId { get; set; }

        [Display(Name = "Branch", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string BranchCode { get; set; }

        public string CampaignSource { get; set; }
     //   [Required(ErrorMessage = "Please select Channel")]
        public string ChannelCode { get; set; }
        public string ApplicationReferenceNo { get; set; }

        [Display(Name = "Organization", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string OrganizationCode { get; set; }

        public string ProductMemberTypeCode { get; set; }
     //   [Required(ErrorMessage = "Please select Product Type")]
        public string ProductTypeCode { get; set; }

        [Display(Name = "ProductCategory", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string ProductCategoryCode { get; set; }

        [Display(Name = "Product", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string ProductCode { get; set; }

      //  [Required(ErrorMessage = "Please select Serial No")]
        public string SerialNo { get; set; }
        public string SalesId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CorporateRUT { get; set; }
        public string PhysicalApplicationRefNo { get; set; }
        public DateTime? PhysicalApplicationRefDate { get; set; }
        public string ExternalCivilRegistryResult { get; set; }
        public string InputterId { get; set; }
        public DateTime? InputterDate { get; set; }

        [Display(Name = "CreditScore", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public decimal CreditScore { get; set; }

        [Display(Name = "RUT", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string RUT { get; set; }

        [Display(Name = "CustomerType", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string CustomerType { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string Name { get; set; }

        [Display(Name = "PaternalSurname", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string PaternalSurname { get; set; }

        [Display(Name = "MaternalSurname", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string MaternalSurname { get; set; }

        [Display(Name = "Nationality", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string Nationality { get; set; }

        [Display(Name = "DateOfBirth", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Gender", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string Gender { get; set; }

        [Display(Name = "MaritalStatus", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string MaritalStatus { get; set; }

        [Display(Name = "NumberOfDependents", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public decimal? NumberofDependents { get; set; }

        [Display(Name = "Education", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string Education { get; set; }

        [Display(Name = "Profession", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string Profession { get; set; }

        [Display(Name = "Housing", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string Housing { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string EmailID { get; set; } 
       
        public bool IsActive { get; set; }
        public bool IsBlackListed { get; set; }
        public bool IsCardGenerated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }
        public string LegacyCardNumber { get; set; }
        public bool AutoApprove { get; set; }

        public List<CustomerOccupationDTO> CustomerOccupationDTO { get; set; }
        public List<CustomerCommericalHouseConsumptionDTO> CustomerCommericalHouseConsumptionDTO { get; set; }
        public List<CustomerBillingDetailDTO> CustomerBillingDetailDTO { get; set; }
        public List<CustomerDependentDTO> CustomerDependentDTO { get; set; }

        public List<CustomerAddressDTO> CustomerAddressDTO { get; set; }
        public List<CustomerCreditCardDetailDTO> CustomerCreditCardDetailDTO { get; set; }
        public List<CustomerCurrentAccountDetailDTO> CustomerCurrentAccountDetailDTO { get; set; }
        public List<CustomerDecisionDTO> CustomerDecisionDTO { get; set; }

        public string ProductName { get; set; }
        public string CustomerTypeDesc { get; set; }
        public string NationalityDesc { get; set; }
        public string BranchName { get; set; }
        public string ProductMemberTypeDesc { get; set; }
        public string ParentCustomerName { get; set; }

        public CustomerDTO()
        {
            CustomerOccupationDTO = new List<CustomerOccupationDTO>();
            CustomerCommericalHouseConsumptionDTO = new List<CustomerCommericalHouseConsumptionDTO>();
            CustomerBillingDetailDTO = new List<CustomerBillingDetailDTO>();
            CustomerDecisionDTO = new List<CustomerDecisionDTO>();
            
            CustomerAddressDTO = new List<CustomerAddressDTO>();
            CustomerCreditCardDetailDTO = new List<CustomerCreditCardDetailDTO>();
            CustomerCurrentAccountDetailDTO = new List<MasterInfo.CustomerCurrentAccountDetailDTO>();
            CustomerDependentDTO = new List<CustomerDependentDTO>();
        }

    }
}
