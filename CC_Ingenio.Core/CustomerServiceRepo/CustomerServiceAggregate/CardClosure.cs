using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
    [Table("CS_Card_Closure")]
    public class CardClosure
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClosureID { get; set; }
        public int CustomerId {get;set;}
        public string CardNumber { get; set; }
        [ForeignKey("objClosureReason")]
        public string ClosureReasonCode { get; set; }
        public DateTime ClosureDate { get; set; }
        public bool IsCardReceived { get; set; }
        public bool IsDocumentReceived { get; set; }
        public string Remarks { get; set; }
	    public string CreatedBy {get;set;}
	    public DateTime CreatedOn {get;set;}
	    public string LastUpdatedBy {get;set;}
	    public DateTime LastUpdatedOn {get;set;}
        public string ApprovalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool IsActive { get; set; }
     

        public virtual ClosureReason objClosureReason { get; set; }

    }
}
