using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class LostStolenCardBlockDTO
    {
        public string RUT { get; set; }
        public string CardNumber { get; set; }
        public bool IsBlocked { get; set; }
        public string CardType { get; set; }
        public string CardStatus { get; set; }
        public string CardMemberType { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public bool IsSaved { get; set; }
    }
}
