using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate
{

    [Table("VW_EMITransactionMaster")]
    public class EMIAuthorization
    {
        [Key]
        public string  EMIAuthNo { get; set; }
        public string  TransactionCode { get; set; }
        public string RUT { get; set; }
        public string  MerchantCode { get; set; }
        public string  CardNumber { get; set; }
        public string  AccountNo { get; set; }      
        public int     TotalInstallment { get; set; }
        public int TotalPrincipalAmt { get; set; }
        public int TotalInterestAmt { get; set; }
        public int TotalPrincipalAmtPaid { get; set; }
        public int TotallInterestAmtPaid { get; set; }
        public int AdjPrincipalAmount { get; set; }
        public int AdjInterestAmount { get; set; }
        public System.Nullable<DateTime> LoanBookedDate { get; set; }    
        public bool IsChecked { get; set; }
        public bool IsClosed { get; set; }


       
    }
}
