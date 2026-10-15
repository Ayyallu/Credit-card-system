using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class CustomerAddressDTO
    {
        [Required]
        public int AddressId { get; set; }
        [Required(ErrorMessage="Please enter Address Type")]
        public string AddressTypeCode { get; set; }
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Please enter Type Of Residence Clearance")]
        public string TypeOfResidenceClearance { get; set; }
        [Required(ErrorMessage="Please select Region")]
        public string RegionCode { get; set; }
        [Required(ErrorMessage="Please select City")]
        public string CityCode { get; set; }
        [Required(ErrorMessage="Please Communa")]
        public string CommunaCode { get; set; }
        [Required(ErrorMessage="Please enter Street")]
        public string Street { get; set; }
        [Required(ErrorMessage="Please enter Number")]
        public string Number { get; set; }
        public string Block { get; set; }
        public string ComplementOfAddress { get; set; }
        public string Town { get; set; }
        public string Landmark { get; set; }
        public string PostalCode { get; set; }
        public string POBox { get; set; }
        [Required(ErrorMessage="Please enter Phone")]
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
    }
}
