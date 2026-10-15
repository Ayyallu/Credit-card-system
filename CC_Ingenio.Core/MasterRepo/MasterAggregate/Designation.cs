using CC_Ingenio.Core.LookUpRepo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_Designation")]
    public class Designation
    {
        #region Model

        [Key,Column (Order = 0)]
        public string DesignationCode { get; set; }
        [Key, Column(Order = 1)]
        public string OrganizationCode { get; set; }
        public string DesignationDescription { get; set; }
        [ForeignKey("Positions")]
        public string Position { get; set; }
        public bool NTP { get; set; }
        public bool HRP { get; set; }
        public string Rank { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual Position Positions { get; set; }

        #endregion
    }
}
