using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
    [Table("CS_LostStolenCardReissueRequest")]
    public class LostStolenReissueRequest
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LostStolenCardReissueRequestId { get; set; }
        public string RUT { get; set; }
        public string RUTVerificationDigit { get; set; }
        public string CardNumber { get; set; }
        public string SerialNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        //public string ContractCode { get; set; }
        public string NewContractCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string QueueReferenceNo { get; set; }
    }
}
