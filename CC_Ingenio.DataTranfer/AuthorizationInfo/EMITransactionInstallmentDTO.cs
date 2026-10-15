using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CC_Ingenio.DataTranfer.AuthorizationInfo
{
    public class EMITransactionInstallmentDTO
    {
        public string EMIAuthNo { get; set; }
        public string CardNumber { get; set; }
        public int InstallmentNo { get; set; }
        public decimal TotalPrincipalAmt { get; set; }
        public decimal TotalInterestAmt { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPosted { get; set; }
        public bool IsChecked { get; set; }
        public string RUT { get; set; }
        public int   TotalPrepaymentPrincipalAmount { get; set; }
        public int TotalPrepaymentInterestAmount { get; set; }
        public List<EMITransactionInstallmentDTO> TransList { get; set; }

    }
}
