using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class TestDTO
    {


     
         [Required(ErrorMessage = "Please enter  code")]
        public string Code { get; set; }
          [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }    

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
