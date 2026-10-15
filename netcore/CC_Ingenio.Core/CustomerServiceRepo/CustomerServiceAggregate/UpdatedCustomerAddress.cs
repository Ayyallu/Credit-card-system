using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{

    [Table("CS_CustomerAddressUpdate")]
    public class UpdatedCustomerAddress
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }
        public int CustomerId { get; set; }
        public string CardNumber { get; set; }
        public string AddressTypeCode { get; set; }
        public string TypeOfResidenceClearance { get; set; }
        public string CountryCode { get; set; }
        public string RegionCode { get; set; }
        public string CityCode { get; set; }
        public string CommunaCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Block { get; set; }
        public string ComplementOfAddress { get; set; }
        public string Town { get; set; }
        public string Landmark { get; set; }
        public string PostalCode { get; set; }
        public string POBox { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string QueueReferenceNo { get; set; }
    }
}
