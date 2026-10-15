using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.MasterRepo.MasterAggregate
{
    [Table("MT_FeeType")]
    public class FeeType
    {
        #region Model

        [Key,Column(Order=0)]
        public string FeeCode { get; set; }
        [Key, Column(Order = 1)]
        public string FeeTypeCode { get; set; }
        [Key, Column(Order = 2)]
        public string OrganizationCode { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal MonthsFrom { get; set; }
        public string ReasonCode { get; set; }
        public decimal FirstYearAmount { get; set; }
        public decimal RenwalFee1 { get; set; }
        public decimal RenwalFee2 { get; set; }
        public decimal RenwalFee3 { get; set; }
        public decimal RenwalFee4 { get; set; }
        public decimal RenwalFee5 { get; set; }
        public decimal RenwalFee6 { get; set; }

        public string  AdjustCode{ get; set; }
	    public string  Period{ get; set; } 
	    public decimal  DiscountRatePercent{ get; set; }
	    public decimal  FlatFee{ get; set; }
	    public decimal  MinFee{ get; set; } 
	    public bool  Flag{ get; set; }  
        
        public virtual Fee Fee { get; set; }

        #endregion
    }
}
