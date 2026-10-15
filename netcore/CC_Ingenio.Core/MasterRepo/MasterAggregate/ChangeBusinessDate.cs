using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("VW_BusinessDate")]
    public class ChangeBusinessDate
    {
        [Key]
        public string OrganizationCode { get; set; }
        public DateTime BusinessDate { get; set; }
    }
}
