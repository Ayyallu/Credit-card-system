using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class CustomerCommericalHouseConsumptionDTO
    {
        [Required]
        public int CommericalId { get; set; }
        public int CustomerId { get; set; }
        [Required(ErrorMessage="Please select Type")]
        public string Type { get; set; }
        [Required(ErrorMessage="Please enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage="Please enter Card Number")]
        public string CardNumber { get; set; }
        [Required(ErrorMessage="Please select Account Status")]
        public string AccountStatus { get; set; }
    }
}
