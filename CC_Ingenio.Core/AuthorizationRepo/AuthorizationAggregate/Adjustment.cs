using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate
{
    [Table("TR_Adjustment")]
    public class Adjustment
    {
        [Key]
        public string AdjustmentCode { get; set; }
        public string AdjustmentNarration { get; set; }
        public string AuthCode { get; set; }
        public string CardNumber { get; set; }
        public decimal ActualAmount { get; set; }
        public decimal AdjustedAmount { get; set; }
        public string CloseFlag { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string QueueReferenceNo { get; set; }
    }
}
