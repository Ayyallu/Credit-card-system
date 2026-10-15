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
    [Table ("VW_EMITransaction")]
    public class EMITransactionInstallment
    {
        [Key, Column(Order = 0)]
        public string EMIAuthNo { get; set; }

        [Key, Column(Order = 1)]
        public int InstallmentNo { get; set; }            
      
      
        public string CardNumber { get; set; }    
   
        public int TotalPrincipalAmt { get; set; }
        public int TotalInterestAmt { get; set; }

        public int TotalPrepaymentPrincipalAmount { get; set; }
        public int TotalPrepaymentInterestAmount { get; set; }

        public DateTime DueDate { get; set; }
        public bool IsPosted { get; set; }       
        public bool IsChecked { get; set; }
        public string Status { get; set; }
        public string EMISlno { get; set; }
    }
}
