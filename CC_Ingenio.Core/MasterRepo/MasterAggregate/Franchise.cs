using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_Franchise")]
    public class Franchise
    {
        #region Model

        [Key,Column (Order = 0)]
        public string FranchiseCode { get; set; }
        public string FranchiseName { get; set; }
        [ForeignKey("Organization")]
        [Key, Column(Order = 1)]
        public string OrganizationCode { get; set; }
        public string Allocation { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual Organization Organization { get; set; }

        #endregion
    }
}
