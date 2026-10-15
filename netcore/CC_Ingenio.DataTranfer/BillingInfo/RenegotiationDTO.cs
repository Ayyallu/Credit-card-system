using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio_Resources;

namespace CC_Ingenio.DataTranfer.BillingInfo
{
    public class RenegotiationDTO
    {
        public string CardNumber { get; set; }
        public string RUT { get; set; }
        public string ProductCode { get; set; }
        public string CustomerSegmentCode { get; set; }
        public string CustomerSubSegmentCode { get; set; }
        public string CustomerCardName { get; set; }

        public int Due { get; set; }
        public int OverDue { get; set; }
        public int WriteOff { get; set; }
        public int OverAll { get; set; }
        public int ActualDue { get; set; }
        public int ActualOverDue { get; set; }
        public int ActualWriteOff { get; set; }
        public int ActualOverAll { get; set; }
        public int DPDCount { get; set; }
        
        public decimal UnBilledOS { get; set; }
        public decimal BilledOS { get; set; }
        public decimal TotalOS { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal CollectionCharges { get; set; }
        public decimal DelinqInterest { get; set; }

        [Display(Name = "DownPayment", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public decimal DownPayment { get; set; }

        [Display(Name = "NetAmount", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public decimal NetAmount { get; set; }

        [Range(0d, 100d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]     
        [Display(Name = "DiscountPercentage", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public decimal DiscountPercentage { get; set; }

        //[Required(ErrorMessage = "Please enter Discount Amount")]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "NetRenegotiatedAmount", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public decimal NetRenegotiatedAmount { get; set; }

        [Range(1d, 100d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Tenure", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public int Tenure { get; set; }

        [Display(Name = "TransactionCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string TransactionCode { get; set; }

        [Range(1d, 100d, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "InterestRatePercentage", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public decimal InterestPercentage { get; set; }

        public string CreatedBy { get; set; }
    }
}
