using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.AuthorizationInfo
{
    public class EMIAuthorizationDTO
    {
        [Required]

        public string EMIAuthNo { get; set; }
        public string TransactionCode { get; set; }
        public string MerchantCode { get; set; }
        public string CardNumber { get; set; }
        public string RUT { get; set; }
        public string AccountNo { get; set; }
        public int TotalInstallment { get; set; }
        public int TotalPrincipalAmt { get; set; }
        public int TotalInterestAmt { get; set; }
        public int TotalPrincipalAmtPaid { get; set; }
        public int TotallInterestAmtPaid { get; set; }
    
        public int AdjPrincipalAmount { get; set; }
        public int AdjInterestAmount { get; set; }
        public System.Nullable<DateTime> LoanBookedDate { get; set; }
        public bool IsChecked { get; set; }
        public bool IsClosed { get; set; }
        public List<EMIAuthorizationDTO> TransList { get; set; }

    }
}
