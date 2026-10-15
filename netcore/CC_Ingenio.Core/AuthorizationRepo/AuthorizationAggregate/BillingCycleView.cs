using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate
{
    [Table("TR_TempBillingCycle")]
    public class BillingCycleView
    {
        [Key]
        public string AuthCode { get; set; }
        public string OrganizationCode { get; set; }
        public string CardNumber { get; set; }
        public string CycleCode { get; set; }
        public string CardTypeCode { get; set; }
        public string MemberTypeCode { get; set; }
        public string CurrencyCode { get; set; }
        public string PrimaryCardNumber { get; set; }
        public string ActualCardNumber { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionDescription { get; set; }
        public decimal Amount { get; set; }
        public decimal SettledAmount { get; set; }
        public bool BillFlag { get; set; }
        public DateTime PostedDate { get; set; }
        public string DrCrFlag { get; set; }
        public string BatchNo { get; set; }
        public DateTime ChargeSlipDate { get; set; }
        public string MCCCode { get; set; }
        public string TransactionFlag { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string ApprovalStatus { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }

    }
}
