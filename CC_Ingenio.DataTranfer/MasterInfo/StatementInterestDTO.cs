using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio_Resources;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
   public class StatementInterestDTO
    {

       public int? BillingMonth { get; set; }
       public int? BillingYear { get; set; }


       // [Display(Name = "BillMonth", ResourceType = typeof(Resources))]
       // [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
       //public int? BillingMonth { get; set; }

       // [Display(Name = "BillYear", ResourceType = typeof(Resources))]
       // [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
       // public int? BillingYear { get; set; }


       
       [Display(Name = "InstallmentInterest", ResourceType = typeof(Resources))]
       [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources)), Range(0, 100, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
       public decimal InstallmentInterest { get; set; }

       
       [Display(Name = "RevolvingInterest", ResourceType = typeof(Resources))]
       [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources)), Range(0, 100, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
       public decimal RevolvingInterest { get; set; }

       
       [Display(Name = "RevolvingCAE", ResourceType = typeof(Resources))]
       [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources)), Range(0, 100, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
       public decimal RevolvingCAE { get; set; }

       
       [Display(Name = "InstallmentCAE", ResourceType = typeof(Resources))]
       [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources)), Range(0, 100, ErrorMessageResourceName = "MsgErrValueShouldBeBetween", ErrorMessageResourceType = typeof(Resources))]
       public decimal InstallmentCAE { get; set; }



       public string OrganizationCode { get; set; }

       public string CreatedBy { get; set; }
       public DateTime CreatedOn { get; set; }
       public string ApprovedBy { get; set; }
       public DateTime? ApprovedOn { get; set; }
       public string ApprovalStatus { get; set; }
       public string LastUpdatedBy { get; set; }
       public DateTime LastUpdatedOn { get; set; }
       public string ApprovalStatusDesc { get; set; }


       public string BillMonthDesc { get; set; }
       public string BillingCycleDesc { get; set; }

    }
}
