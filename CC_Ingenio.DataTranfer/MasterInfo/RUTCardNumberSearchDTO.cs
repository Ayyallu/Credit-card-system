using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class RUTCardNumberSearchDTO
    {
        public string RUT { get; set; }
        public string CardNumber { get; set; }
        public string AccountNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSegmentCode { get; set; }
        public string CustomerSubSegmentCode { get; set; }
        public string ProductCode { get; set; }
        public string OrganizationCode { get; set; }
        public int CustomerId { get; set; }
        public string SalesId { get; set; }
        public string ProductMemberTypeCode { get; set; }
        public string ProductMemberTypeDesc { get; set; }

        public string MaskedRUT
        {
            get
            {
                return this.RUT.Insert(this.RUT.Length - 1, "-");
            }
        }
    }
}
