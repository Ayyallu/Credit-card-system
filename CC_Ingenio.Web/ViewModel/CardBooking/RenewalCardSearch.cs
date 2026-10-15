using CC_Ingenio_Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CC_Ingenio.Web.ViewModel.CardBooking
{
    public class RenewalCardSearch
    {

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

    }
    public class ReportCriteria
    {
        [Display(Name = "Type", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string ReportType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }
}