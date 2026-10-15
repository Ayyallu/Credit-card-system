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
    [Table("MT_SalesId")]
    public class SaleId
    {
        [Key,Column(Order = 0)]
        public string SalesId { get; set; }
        [Key, Column(Order = 1)]
        public string RUT { get; set; }
         [Key, Column(Order = 2)]
        public string OrganizationCode { get; set; }      

        public string BranchCode { get; set; }
        public string CityCode { get; set; }       
        public string CommunaCode { get; set; }
        public string Address { get; set; }

        public string ApprovalStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

    }
}
