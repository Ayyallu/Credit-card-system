using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_DirectSalesAgent")]
    public class DirectSalesAgent
    {
        [Key, Column(Order = 0)]
        public string DSACode { get; set; }
        public string DSAName { get; set; }
        [Key, Column(Order = 1)]
        public string OrganizationCode { get; set; }
        public string CommercialBranchCode { get; set; }
        public string UEN { get; set; }
        public string RUT { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
