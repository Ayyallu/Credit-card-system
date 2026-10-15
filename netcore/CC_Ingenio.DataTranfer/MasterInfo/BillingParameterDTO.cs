using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Resources;
using CC_Ingenio.Common.Utils;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class BillingParameterDTO
    {
        [Display(Name = "Product", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string ProductCode { get; set; }
        public string OrganizationCode { get; set; }

        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "MinTotalAmount", ResourceType = typeof(Resources))]
        public decimal MinTotalAmount { get; set; }
        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "MinTotalDebt", ResourceType = typeof(Resources))]
        public decimal MinTotalDebt { get; set; }
        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ThresholdRevolvingBalance", ResourceType = typeof(Resources))]
        public decimal ThresholdRevolvingBalance { get; set; }


        [Display(Name = "OCLChargePercentage", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources)), Range(0, 100, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        public decimal OCLChargePercentage { get; set; }
        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "MinOCLCharge", ResourceType = typeof(Resources))]
        public decimal MinOCLCharge { get; set; }
        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "MaxOCLCharge", ResourceType = typeof(Resources))]
        public decimal MaxOCLCharge { get; set; }
        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "MinDelinquencyAmount", ResourceType = typeof(Resources))]
        public decimal MinDelinquencyAmount { get; set; }
        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "MinCollectionCharges", ResourceType = typeof(Resources))]
        public decimal MinCollectionCharges { get; set; }
        [Range(0d, 999999999d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "MaxCollectionCharges", ResourceType = typeof(Resources))]
        public decimal MaxCollectionCharges { get; set; }
        public bool IsOCLIncludedInTotalMinDue { get; set; }
        public int DelinquencyBucketCountForWriteOff { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }

        public string ProductName { get; set; }
       

    }
}