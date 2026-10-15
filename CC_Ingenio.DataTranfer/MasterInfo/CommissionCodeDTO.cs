
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class CommissionCodeDTO
    {
        [Required(ErrorMessage = "Please enter Commission Name"), StringLength(5, ErrorMessage = "Commission Name should be less than 5 characters")]
        public string CommissionName { get; set; }
        [Required(ErrorMessage = "Please enter Description"), StringLength(50, ErrorMessage = "Description should be less than 50 characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter Commission Amount")]
        public decimal CommissionAmount { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
