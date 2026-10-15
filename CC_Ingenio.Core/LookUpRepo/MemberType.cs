using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.LookUpRepo
{
    [Table("LT_MemberType")]
    public class MemberType
    {
        [Key]
        public string MemberTypeCode { get; set; }
        public string MemberTypeName { get; set; }
    }
}
