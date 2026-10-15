using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class IndustrialCustomerDTO
    {
        public int CustomerId { get; set; }
        public int ParentCustomerId { get; set; }
        public string OrganizationCode { get; set; }
        public string CustomerType { get; set; }     

        public string RUT { get; set; }
        public string SalesId { get; set; }
        public string BranchCode { get; set; }
        public string ProductMemberTypeCode { get; set; }
        public string ProductCategoryCode { get; set; }
        public string ProductCode { get; set; }
        public string Nationality { get; set; }
        public string LegacyCardNumber { get; set; }

        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int AgeMonths { get; set; }
        public int AgeYears { get; set; }
        public bool AlcoholLicenseFlag { get; set; }
        public string CurrentAccountNo { get; set; }
        public string BankCode { get; set; }
        public bool CreditLineRequestedFlag { get; set; }

        public decimal Relevance { get; set; }
        public decimal GrowthPotentialPercentage { get; set; }
        public string PurchaseConsumptionOccasion { get; set; }
        public bool CreditBureauDicomFlag { get; set; }
        public decimal CreditBureauScore { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }
        public bool IsCustomerForUnderWriting { get; set; }
        public bool IsAutoApprove { get; set; }

        public string DSACode { get; set; }
        public string DSAName { get; set; }
        public string UEN { get; set; }
        public string DSARUT { get; set; }
        public string CommercialBranch { get; set; }
        public string ParentCustomerName { get; set; }
        public bool NeedToSendToApprovalQueue { get; set; }

        public List<IndustrialCustomerAddressDTO> ObjIndustrialCustomerAddressDTO { get; set; }
        public List<IndustrialCustomerDecisionDTO> ObjIndustrialCustomerDecisionDTO { get; set; }
        public IndustrialCustomerDTO()
        {
            ObjIndustrialCustomerAddressDTO = new List<IndustrialCustomerAddressDTO>();
            ObjIndustrialCustomerDecisionDTO = new List<IndustrialCustomerDecisionDTO>();
        }
    }

    public class IndustrialCustomerDecisionDTO
    {
        public int DecisionID { get; set; }
        public int CustomerId { get; set; }       
        public string DecisionCode { get; set; }
        public string ReasonCode { get; set; }
        public string Remarks { get; set; }
        public string ContractCode { get; set; }
        public string Unemployment { get; set; }
        public bool IsContractAcceptedByCustomer { get; set; }
        public decimal CreditLimit { get; set; }
        public string BillingCycleCode { get; set; }
        public int BillCycleSysParamId { get; set; }
    }

    public class CustomerInfo
    {
        public byte ReturnValue { get; set; }
        public int CustomerId { get; set; }
        public string SalesId { get; set; }
        public int ParentCustomerId { get; set; }
        public string ParentCustomerName { get; set; }
        public string ProductMemberTypeCode { get; set; }
    }
}
