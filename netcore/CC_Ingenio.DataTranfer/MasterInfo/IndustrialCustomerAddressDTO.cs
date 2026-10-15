using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class IndustrialCustomerAddressDTO
    {
        public int AddressId { get; set; }
        public int CustomerId { get; set; }
        public string AddressTypeCode { get; set; }
        public string ReferenceAddressTypeCode { get; set; }
        public string AddressDesc { get; set; }
        public string Village { get; set; }
        public string CommunaCode { get; set; }
        public string CityCode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string EmailID { get; set; }
        public string ResidenceType { get; set; }
        public string Other { get; set; }
    }
}
