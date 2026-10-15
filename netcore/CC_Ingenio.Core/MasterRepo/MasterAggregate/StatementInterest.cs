using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_InterestRate")]
   public class StatementInterest
    {
     
        [Key,Column(Order=1)]
        public string OrganizationCode { get; set; }
         [Key, Column(Order = 2)]
        public int? BillingMonth { get; set; }
         [Key, Column(Order = 3)]
        public int? BillingYear { get; set; }
        public decimal InstallmentInterest { get; set; }
        public decimal RevolvingInterest { get; set; }
        public decimal RevolvingCAE { get; set; }
        public decimal InstallmentCAE { get; set; }
        public string ApprovalStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

    }
}
