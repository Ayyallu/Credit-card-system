using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class ContractTemplateDTO
    {
        [Required(ErrorMessage="Please enter Contract Template Code")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Special characters and space are not allowed")]
        public string ContractTemplateCode { get; set; }
        [Required(ErrorMessage = "Please enter Contract Template Description")]
        public string ContractTemplateDescription { get; set; }
        public string OrganizationCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

    }
}
