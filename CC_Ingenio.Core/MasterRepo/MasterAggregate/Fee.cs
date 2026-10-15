using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_Fee")]
    public class Fee
    {
        #region Model
        [Key, Column(Order = 0)]
        public string FeeCode { get; set; }             
        public string ProductMemberTypeCode { get; set; }        
        [Column(Order = 1)]
        public string CustomerSegmentCode { get; set; }      
        [Column(Order = 2)]
        public string CustomerSubSegmentCode { get; set; }
        [Column(Order = 3)]       
        public string ContractCode { get; set; }
        [Key, Column(Order = 4)]              
        public string OrganizationCode { get; set; }
        [Column(Order = 5)]
        public string ProductCode { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
        public string CurrencyCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; } 

        public virtual List<FeeType> FeeType { get; set; }

        public virtual Product Product { get; set; }
        [ForeignKey("CustomerSegmentCode, OrganizationCode")]
        public virtual CustomerSegment CustomerSegment { get; set; }
        [ForeignKey("CustomerSubSegmentCode, OrganizationCode")]
        public virtual CustomerSubSegment CustomerSubSegment { get; set; }
        public virtual Contract Contract { get; set; }

        public Fee()
        {
            FeeType = new List<FeeType>();
        }

        #endregion
    }
}
