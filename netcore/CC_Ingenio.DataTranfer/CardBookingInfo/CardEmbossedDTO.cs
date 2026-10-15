using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.DataTranfer.CardBookingInfo
{
    public class CardEmbossedDTO
    {
        public string RUT { get; set; }
        public string CardNumber { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }

        public string EmbossedFileName { get; set; }
        public string EmbossedKitFileName { get; set; }

        public string MaskedRUT { 
            get 
            {
                return this.RUT.Insert(this.RUT.Length-1,"-"); 
            } 
        }

    }
}
