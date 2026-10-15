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
    [Table("VW_CardClosure_History")]
    public class CardClosureHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClosureID { get; set; }
        public int CustomerId {get;set;}
        public string CardNumber { get; set; }
        public string ClosureReasonCode { get; set; }
        public DateTime ClosureDate { get; set; }
        public string Reactivatedon { get; set; }
        public bool IsActive { get; set; }
       

        public virtual ClosureReason objClosureReason { get; set; }

    }
}
