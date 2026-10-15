using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_CollectionChargeDetails")]
    public class CollectionChargeDetails
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int CollectionChargeDetailsId { get; set; }
        public int CollectionChargeId { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0#####}", ApplyFormatInEditMode = true)]
        public decimal DelinquencyAmountFrom { get; set; }
        public decimal DelinquencyAmountTo { get; set; }
        public decimal CollectionChargeAmount { get; set; }
    }
}
