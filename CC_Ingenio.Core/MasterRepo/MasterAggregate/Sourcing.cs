using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_Sourcing")]
    public class Sourcing
    {
       [Key , Column (Order = 0)]
       public string SourcingChannelCode { get; set; }
       [Key, Column(Order = 1)]
       public string OrganizationCode { get; set; }
	   public string SourcingChannelName { get; set; }
	   public decimal FloorLimit { get; set; }
	   public decimal CeilingLimit { get; set; }
	   public decimal LetterGenerationDelayDays { get; set; }
	   public string CreatedBy { get; set; }
	   public DateTime CreatedOn { get; set; }
	   public string ApprovedBy { get; set; }
	   public DateTime? ApprovedOn { get; set; }
       public string ApprovalStatus { get; set; }
	   public string LastUpdatedBy { get; set; }
       public DateTime LastUpdatedOn { get; set; }

    }
}
