using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_CardStatus")]
    public class CardStatus
    {
        #region Model

        [Key, Column(Order = 0)]
        public string CardStatusCode { get; set; }
        [Key, Column(Order = 1)]
        public string OrganizationCode { get; set; }
        public string DecisionCode { get; set; }
        public string Narration { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public virtual Decision Decision { get; set; }

        #endregion
    }
}
