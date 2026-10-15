using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using CC_Ingenio.Core.LookUpRepo;

namespace CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate
{
    [Table("TR_TransactionEMIMaster")]
    public class EMITransaction
    {
        [Key ]
        public string EMIAuthNo { get; set; }
        public string CardNumber { get; set; }
        public string MerchantCode { get; set; }
        public int PlanId { get; set; }       
        public int TotalInstallment { get; set; }
        public decimal LoanAmount { get; set; }   
        public DateTime LoanBookedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }       
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public decimal SBIFInterestRate { get; set; }
        public int EMISlno { get; set; }
        public decimal RateOfInterest { get; set; }
        public string Status { get; set; }
        public string TransactionCode { get; set; }
    }
}
