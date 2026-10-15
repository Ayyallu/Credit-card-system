using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
     [Table("CS_BlockSegmentCard")]
   public class BlockSegmentCard
    {
         [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public int BlockSegmentCardId { get; set; }
         public string CardNumber { get; set; }
         public string AccountNumber { get; set; }
         public string BlockedType { get; set; }
         public string BlockCode { get; set; }
         public string CreditSegmentCode { get; set; }
         public bool IsBlocked { get; set; }

         public string CreatedBy { get; set; }
         public DateTime CreatedOn { get; set; }
         public string ApprovedBy { get; set; }
         public DateTime? ApprovedOn { get; set; }
         public string ApprovalStatus { get; set; }
         public string LastUpdatedBy { get; set; }
         public DateTime LastUpdatedOn { get; set; }
    }
}
