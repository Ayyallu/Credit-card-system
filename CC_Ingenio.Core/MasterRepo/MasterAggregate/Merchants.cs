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
     [Table("MT_Merchant")]
   public class Merchants
    {
        [Key,Column (Order = 0 )]
        public string MerchantCode { get; set; }
        [Key, Column(Order = 1)]
        public String OrganizationCode { get; set; }
        //[Column(Order = 1)]
        public string CityCode { get; set; }
        public string MerchantName { get; set; }
        public string CountryCode { get; set; }
        public string RegionCode { get; set; }
        public string CommunaCode { get; set; }
        public int SettlementInDays { get; set; }
        public string CommercialUEN { get; set; }
        public string CommercialUENName { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual Country Country { get; set; }
        //public virtual Region Region { get; set; }
        //public virtual City City { get; set; }
        //public virtual Communa Communa { get; set; }
    }
}
