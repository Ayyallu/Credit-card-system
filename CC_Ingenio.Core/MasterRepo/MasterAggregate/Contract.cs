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
    [Table("MT_Contract")]
    public class Contract
    {
        [Key,Column (Order = 0)]
        public string ContractCode { get; set; }
        [Key, Column(Order = 2)]
        public string OrganizationCode { get; set; }
        public string ContractDescription { get; set; }
        [Column(Order = 1)]       
        public string ContractTemplateCode { get; set; }
        [Column(Order = 3)]
        //public string ProductCode { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
        public string NotaryName { get; set; }
        public DateTime NotarySignDate { get; set; }
        public string Repertoire { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        //public virtual Product Product { get; set; }
        public virtual ContractTemplate ContractTemplate { get; set; }

    }
}
