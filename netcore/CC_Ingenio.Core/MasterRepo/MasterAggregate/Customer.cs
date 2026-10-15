using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_Customer")]
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        public int? ParentCustomerId { get; set; }
        public string BranchCode { get; set; }
        public string CampaignSource { get; set; }
        public string ChannelCode { get; set; }
        public string ApplicationReferenceNo { get; set; }
        public string OrganizationCode { get; set; }
        public string ProductTypeCode { get; set; }
        public string ProductCategoryCode { get; set; }       
        public string ProductCode { get; set; }
        public string SerialNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CorporateRUT { get; set; }
        public string PhysicalApplicationRefNo { get; set; }
        public DateTime? PhysicalApplicationRefDate { get; set; }
        public string ExternalCivilRegistryResult { get; set; }
        public string InputterId { get; set; }
        public DateTime? InputterDate { get; set; }
        public decimal CreditScore { get; set; }
        public string SalesId { get; set; }
        public string RUT { get; set; }
        public string CustomerType { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        [ForeignKey("Country")]
        public string Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public decimal? NumberofDependents { get; set; }
        public string Education { get; set; }
        public string Profession { get; set; }
        public string Housing { get; set; }
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
        public string ProductMemberTypeCode { get; set; }
        public string LegacyCardNumber { get; set; }        

        public virtual List<CustomerOccupation> CustomerOccupation { get; set; }
        public virtual List<CustomerCommericalHouseConsumption> CustomerCommericalHouseConsumption { get; set; }
        public virtual List<CustomerBillingDetail> CustomerBillingDetail { get; set; }
        public virtual List<CustomerDecision> CustomerDecision { get; set; }

        public virtual List<CustomerAddress> CustomerAddress { get; set; }
        public virtual List<CustomerCreditCardDetail> CustomerCreditCardDetail { get; set; }
        public virtual List<CustomerCurrentAccountDetail> CustomerCurrentAccountDetail { get; set; }
        public virtual List<CustomerDependent> CustomerDependent { get; set; }

        public virtual Product Product { get; set; }
        public virtual Country Country { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual ProductMemberType ProductMemberType { get; set; }

        public Customer()
        {
            CustomerOccupation = new List<CustomerOccupation>();
            CustomerCommericalHouseConsumption = new List<CustomerCommericalHouseConsumption>();
            CustomerBillingDetail = new List<CustomerBillingDetail>();
            CustomerDecision = new List<CustomerDecision>();

            CustomerAddress = new List<CustomerAddress>();
            CustomerCreditCardDetail = new List<CustomerCreditCardDetail>();
            CustomerCurrentAccountDetail = new List<CustomerCurrentAccountDetail>();
            CustomerDependent = new List<CustomerDependent>();
        }

    }
}
