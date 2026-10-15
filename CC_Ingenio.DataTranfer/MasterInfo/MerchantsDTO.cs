using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;
using CC_Ingenio.Common.Utils;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
   public class MerchantsDTO
    {
        [Display(Name = "MerchantCode", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression("^([0-9]{5,9})*(\\-[kK0-9]{1,1})$", ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        //[RegularExpression(Utils.MSG_VALID_MERCHANTCODE, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(11, ErrorMessageResourceName = "MsgErrMinimumLength", ErrorMessageResourceType = typeof(Resources), MinimumLength = 7)]

        public string MerchantCode { get; set; }

        [Display(Name = "MerchantName", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessageResourceName = "MsgErrMaximumLength", ErrorMessageResourceType = typeof(Resources))]
        public string MerchantName { get; set; }

        public string OrganizationCode { get; set; }
        public string CountryCode { get; set; }
        public string RegionCode { get; set; }
        public string CityCode { get; set; }
        public string CommunaCode { get; set; }
        public int SettlementInDays { get; set; }
        public string CommercialUEN { get; set; }
        public string CommercialUENName { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string ApprovalStatusDesc { get; set; }

        public string CountryDesc { get; set; }
        public string RegionDesc { get; set; }
        public string CityDesc { get; set; }
        public string CommunaDesc { get; set; }
    }
}
