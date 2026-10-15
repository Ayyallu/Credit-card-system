using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class LineUtilizationDTO
    {
        public string CardNumber { get; set; }
        public string Segment { get; set; }
        public string CardSegmentCode { get; set; }
        public decimal Limit { get; set; }
        public decimal UtilizedAmount { get; set; }
        public decimal UtilizedPercentage { get; set; }

    }
    
}
