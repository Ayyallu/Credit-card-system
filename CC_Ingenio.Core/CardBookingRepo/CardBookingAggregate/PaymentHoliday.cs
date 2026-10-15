using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using CC_Ingenio.Core.MasterRepo.MasterAggregate;

namespace CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate
{
    [Table("MT_PaymentHoliday")]
    public class PaymentHoliday
    {
       [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SlNo { get; set; }
        public string CardNumber { get; set; }      
        public string ProductCode { get; set; }     
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string OrganizationCode { get; set; }
        public string RUT { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime? ApprovedOn { get; set; }       
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public virtual Product objProduct { get; set; }

    }
}
