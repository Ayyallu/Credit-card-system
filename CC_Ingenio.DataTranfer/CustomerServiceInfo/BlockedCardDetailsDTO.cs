using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class BlockedCardDetailsDTO
    {
        public string Code { get; set; }
        public string Segment { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public bool IsAccountBlocked { get; set; }
        public bool IsAccountBlockedForFinancialReasons { get; set; }
        public bool IsAccountBlockedForNonFinancialReasons { get; set; }
        public decimal FinancialHierarchy { get; set; }
        public decimal NonFinancialHierarchy { get; set; }
        public bool IsCardBlocked { get; set; }
        public decimal CardBlockHierarchy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class UserActivationBlockedDetails
    {
        public string CardNumber { get; set; }
        public string BlockStatus { get; set; }
        public string Segment { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
