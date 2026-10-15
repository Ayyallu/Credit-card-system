using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CC_Ingenio_Resources;
namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class NotepadEntryDTO
    {
        public int NotepadEntryId { get; set; }
        public int CustomerId { get; set; }
        public string CardNumber { get; set; }
        [Display(Name = "MessageType", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseSelect", ErrorMessageResourceType = typeof(Resources))]
        public string MessageType { get; set; }

        [Display(Name = "Message", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "MsgErrPleaseEnter2", ErrorMessageResourceType = typeof(Resources))]
        public string MessageDescription { get; set; }

        public string MessageStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public bool IsSaved { get; set; }
    }
}
