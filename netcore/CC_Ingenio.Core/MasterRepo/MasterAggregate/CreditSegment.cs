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
    [Table("MT_CreditSegment")]
    public class CreditSegment
    {
      

        [Key,Column(Order = 0)]
        public string CardSegmentCode { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("objOrganization")]
        public string OrganizationCode { get; set; }

        public string CardSegmentName { get; set; }
        [ForeignKey("LegalEntity")]
        public string LegalEntityCode { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
        public bool IsOTBReplenishment { get; set; }
        public string CreditSegmentType { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual Organization objOrganization { get; set; }
        public virtual LegalEntity LegalEntity { get; set; }

    }
}
