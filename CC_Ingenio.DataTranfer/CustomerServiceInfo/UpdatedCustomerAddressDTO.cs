using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;
using CC_Ingenio.Common.Utils;
namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class UpdatedCustomerAddressDTO
    {       
        public int AddressId { get; set; }

        [Display(Name = "AddressType", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string AddressTypeCode { get; set; }

        public int CustomerId { get; set; }
        public string CardNumber { get; set; }

        [Display(Name = "TypeOfResidenceClearance", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string TypeOfResidenceClearance { get; set; }

        public string CountryCode { get; set; }

        [Display(Name = "Region", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string RegionCode { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string CityCode { get; set; }

        [Display(Name = "Communa", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string CommunaCode { get; set; }

        [Display(Name = "Street", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string Street { get; set; }

        [Display(Name = "Number", ResourceType = typeof(Resources))]
        //[RegularExpression(Utils.MSG_VALID_PHONENO, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public string Number { get; set; }

        public string Block { get; set; }
        public string ComplementOfAddress { get; set; }
        public string Town { get; set; }
        public string Landmark { get; set; }
        public string PostalCode { get; set; }
        public string POBox { get; set; }

        [Display(Name = "Phone", ResourceType = typeof(Resources))]
        //[RegularExpression(Utils.MSG_VALID_PHONENO, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public string Phone { get; set; }

        [Display(Name = "Mobile", ResourceType = typeof(Resources))]
        //[RegularExpression(Utils.MSG_VALID_PHONENO, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public string Mobile { get; set; }

        public string Fax { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalStatusDesc { get; set; }
        public bool IsSaved { get; set; }
    }
}
