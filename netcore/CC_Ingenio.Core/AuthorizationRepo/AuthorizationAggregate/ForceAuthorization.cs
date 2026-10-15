using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.AuthorizationRepo.AuthorizationAggregate
{

    [Table("VW_Transaction")]
    public class ForceAuthorization
    {
        [Key]
        public string AuthCode { get; set; }
        public string TransactionCode { get; set; }
        public string BatchNo { get; set; }
        public string CardNumber { get; set; }
        public string AccountNo { get; set; }
        public string OrganizationCode { get; set; }
      
        public string ResponseCode { get; set; }
        public System.Nullable<DateTime> TxnDate { get; set; }
        public string MerchantCode { get; set; }
        public string MCCCode { get; set; }
        public decimal Amount { get; set; }
        public bool IsPosted { get; set; }
        public bool IsManualEntry { get; set; }
        public bool IsChecked { get; set; }
        public string AuthCancellationStatus { get; set; }

    }
}
