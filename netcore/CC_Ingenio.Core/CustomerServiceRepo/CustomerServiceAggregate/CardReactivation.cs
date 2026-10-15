using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CC_Ingenio.Core.CustomerServiceRepo;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
    [Table("CS_Card_Reactivation")]
    public class CardReactivation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReactivationID { get; set; }
        [ForeignKey("objCardClosure")]
        public int ClosureID { get; set; }
        public int CustomerId {get;set;}
        public string CardNumber { get; set; }
        public string ReactivationReasonCode { get; set; }
        public string ReactivationRemarks { get; set; }
	    public string CreatedBy {get;set;}
        public DateTime ReactivatedOn { get; set; }
	    public string LastUpdatedBy {get;set;}
	    public DateTime LastUpdatedOn {get;set;}
        public string ApprovalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public string QueueReferenceNo { get; set; }

        public virtual CardClosure objCardClosure { get; set; } 
    }
}
