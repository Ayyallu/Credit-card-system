using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class CustomerDependentDTO
    {
        [Required]
        public int DependentID { get; set; }
        public int CustomerId { get; set; }
        [Required(ErrorMessage="Please enter Age Group")]
        public string AgeGroup { get; set; }
        public int NoOfChildren { get; set; }
    }
}
