using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
    [Table("CS_ChangeCreditLimit")]
    public class ChangeCreditLimit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChangeCreditLimitID { get; set; }
        public int CustomerId { get; set; }
        public string CardNumber { get; set; }
        public decimal CreditLimit { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string QueueReferenceNo { get; set; }
    }
}
