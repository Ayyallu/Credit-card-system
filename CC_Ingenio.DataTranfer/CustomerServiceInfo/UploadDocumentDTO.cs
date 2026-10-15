using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.CustomerServiceInfo
{
    public class UploadDocumentDTO
    {
        public string DocumentName { get; set; }        
        public DateTime UpLoadDate { get; set; }
        public string UploadedBy { get; set; }

    }
}
