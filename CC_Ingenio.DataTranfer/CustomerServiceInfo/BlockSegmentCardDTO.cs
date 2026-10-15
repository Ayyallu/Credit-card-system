using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
   public class BlockSegmentCardDTO
    {
        public int BlockSegmentCardId { get; set; }
        public string CardNumber { get; set; }
        public string AccountNumber { get; set; }
        public string BlockedType { get; set; }
        public string BlockCode { get; set; }
        public string CreditSegmentCode { get; set; }
        public bool IsBlocked { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public bool IsSaved { get; set; }
        public string ApprovalStatus { get; set; }
        public string PageStatus { get; set; }

        public List<String> BlockCodeList { get; set; }
        public List<String> SelectedBlockCodeList { get; set; }
    }
}
