using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.MasterInfo
{
    public class ChangeBusinessDateDTO
    {
        public string OrganizationCode { get; set; }
        public DateTime BusinessDate { get; set; }
        public DateTime OldBusinessDate { get; set; }
    }
}
