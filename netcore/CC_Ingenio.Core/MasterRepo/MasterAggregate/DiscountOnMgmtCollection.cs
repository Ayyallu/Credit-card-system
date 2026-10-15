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
    [Table("MT_DiscountOnMgmtCollection")]
    public class DiscountOnMgmtCollection
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiscountOnMgmtCollectionId { get; set; }      
        public string ProductCode { get; set; }              
        public string ContractCode { get; set; }        
        public string OrganizationCode { get; set; }
        public string CommissionCode { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual Product Product { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual Commission Commission { get; set; }

        public virtual List<DiscountOnMgmtCollectionDetails> DiscountOnMgmtCollectionDetails { get; set; }

        public DiscountOnMgmtCollection()
        {
            DiscountOnMgmtCollectionDetails = new List<DiscountOnMgmtCollectionDetails>();
        }

    }
}
