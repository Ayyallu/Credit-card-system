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
    public class AlertMessageDTO
    {
        public int AlertId { get; set; }
        public string CardNumber { get; set; }

        [Display(Name = "AlertType", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string AlertType { get; set; }

        [Display(Name = "Message", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string Message { get; set; }

        public bool IsMobileAlert { get; set; }
        public bool IsEmailAlert { get; set; }

        [Display(Name = "Phone", ResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_VALID_PHONENO, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public string MobileNo { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Resources))]
        [RegularExpression(Utils.MSG_VALID_EMAIL, ErrorMessageResourceName = "MsgErrInvalid", ErrorMessageResourceType = typeof(Resources))]
        public string EmailId { get; set; }

        public bool IsCharged { get; set; }
        public bool IsSaved { get; set; }
    }
}
