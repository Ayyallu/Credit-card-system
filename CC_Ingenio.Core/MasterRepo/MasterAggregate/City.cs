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
    [Table("MT_City")]
    public class City
    {
        #region Model

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int CityCommunaCityRegionId { get; set; }

        public string CityCode { get; set; }
           //[Key, Column(Order = 1)]
         [ForeignKey("objOrganization")]
        public string OrganizationCode { get; set; }
        public string CityName { get; set; }
        [ForeignKey("Country")]
        public string CountryCode { get; set; }
        [ForeignKey("Region")]
        public string RegionCode { get; set; }
        [ForeignKey("CityCategory")]
        public string CityCategoryCode { get; set; }
        [ForeignKey("Hub")]
        public string HubCode { get; set; }
        [ForeignKey("Spoke")]
        public string SpokeCode { get; set; }
        [ForeignKey("Communa")]
        public string CommunaCode { get; set; }
        public string CommunaName { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual Country Country { get; set; }
        public virtual Region Region { get; set; }
        public virtual CityCategory CityCategory { get; set; }
        public virtual Hub Hub { get; set; }
        public virtual Spoke Spoke { get; set; }
        public virtual Communa Communa { get; set; }
        public virtual Organization objOrganization { get; set; }

        #endregion
    }
}
