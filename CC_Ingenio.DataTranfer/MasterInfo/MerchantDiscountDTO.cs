using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class MerchantDiscountDTO
    {      
        public int MerchantDiscountId { get; set; }
        public String OrganizationCode { get; set; }
        [Display(Name = "Merchant", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string MerchantCode { get; set; }
        [Display(Name = "EffectiveFromDate", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public DateTime EffectiveFromDate { get; set; }
        //[Display(Name = "EffectiveToDate", ResourceType = typeof(Resources))]
        //[Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        //public DateTime EffectiveToDate { get; set; }
        //public bool IsTransactionBased { get; set; }
        [Display(Name = "TransactionCheckFrequency", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string TransactionCheckFrequencyCode { get; set; }
        [Display(Name = "DiscountRatePercent", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources)), Range(0, 100, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        public decimal DiscountRate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }

        public string MerchantName { get; set; }
        public string TransactionCheckFrequencyDesc { get; set; }
    }
}
