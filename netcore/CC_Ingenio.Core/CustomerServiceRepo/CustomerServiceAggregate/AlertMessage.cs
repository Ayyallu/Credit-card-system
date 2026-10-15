using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.CustomerServiceRepo.CustomerServiceAggregate
{
    [Table("CS_AlertMessage")]
    public class AlertMessage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AlertId { get; set; }
        public string CardNumber { get; set; }
        public string AlertType { get; set; }
        public string Message { get; set; }
        public bool IsMobileAlert { get; set; }
        public bool IsEmailAlert { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public bool IsCharged { get; set; }      
    }
}
