using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CC_Ingenio_Resources;
namespace CC_Ingenio.DataTranfer.MasterInfo
{
     public class PlanMasterDTO
    {
            public int PlanID { get; set; }

            [Display(Name = "CreditSegment", ResourceType = typeof(Resources))]
            [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
            public string CreditSegmentCode { get; set; }

            [Display(Name = "Product", ResourceType = typeof(Resources))]
            //[Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
            public string ProductCode { get; set; }

            [Display(Name = "CustomerSegment", ResourceType = typeof(Resources))]
            //[Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
            public string CustomerSegmentCode { get; set; }

            [Display(Name = "CustomerSubSegment", ResourceType = typeof(Resources))]
            //[Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
            public string CustomerSubSegmentCode { get; set; }

            [Display(Name = "TransactionCode", ResourceType = typeof(Resources))]
            [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
            public string TransactionCode  { get; set; }

            [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
            public decimal CreditLimitFrom { get; set; }
            [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
	        public decimal CreditLimitTo { get; set; }
            //public decimal TenureFrom { get; set; }
            [Range(1d, 99d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
            [Display(Name = "Tenure", ResourceType = typeof(Resources))]  
            public decimal TenureTo { get; set; }

            [Display(Name = "EffectiveFromDate", ResourceType = typeof(Resources))]
	        public DateTime ValidFromDate { get; set; }
            //public DateTime ValidToDate { get; set; }
            [Range(1d, 100d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
            [Display(Name = "SBIFInterestRatePercentage", ResourceType = typeof(Resources))]  
            //[Range(1d, 100d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]     
	        public decimal SBIFInterest { get; set; }

           // [DisplayFormat(DataFormatString = "{0:0.00}")]//ApplyFormatInEditMode = true, 
            [Display(Name = "InterestRate", ResourceType = typeof(Resources))]  
	        public decimal ExpectedInterestRate { get; set; }

            public decimal DeferredPayment { get; set; }

            [Display(Name = "InterestCalculation", ResourceType = typeof(Resources))]
            [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
            public string InterestCalculation { get; set; }

            [Display(Name = "FirstBilling", ResourceType = typeof(Resources))]
            [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
            public string FirstBilling { get; set; }

            public string OrganizationCode { get; set; }

            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public string ApprovedBy { get; set; }
            public DateTime? ApprovedOn { get; set; }
            public string LastUpdatedBy { get; set; }
            public DateTime LastUpdatedOn { get; set; }
            public string ApprovalStatus { get; set; }
            public string ApprovalStatusDesc { get; set; }

            public string CreditSegmentDesc { get; set; }
            public string ProductName { get; set; }
            public string CustomerSegmentDesc { get; set; }
            public string CustomerSubSegmentDesc { get; set; }
            public string TransactionDesc { get; set; }

        }
    }

