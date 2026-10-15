using CC_Ingenio_Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.AuthorizationInfo
{
    public class TransactionAdjustmentDTO
    {
        public string CardNumber { get; set; }
        public string CustomerCardName { get; set; }
        public string RUT { get; set; }
        public string AuthorizationCode { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionDescription { get; set; }
        public string TransactionSubCode { get; set; }
        public string InvoiceNo { get; set; }
        [Display(Name = "AdjustmentCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string AdjustmentCode { get; set; }
        [Display(Name = "TransactionAdjustmentCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string TransactionAdjustmentCode { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal AdjustedAmount { get; set; }
        [Display(Name = "AmountToAdjust", ResourceType = typeof(Resources))]
        [Range(1.00, 999999999.00, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
        public decimal AmountToAdjust { get; set; }
        public string ReferenceAuthCode{ get; set; }
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
		public string Remarks{ get; set; }
        public string TransType { get; set; }
        public DateTime TxnDate { get; set; }
        public DateTime PostedDate { get; set; }
        public decimal BalanceAmount { get; set; }

		public DateTime CreatedOn{ get; set; }
		public string CreatedBy { get; set; }
    }
}
