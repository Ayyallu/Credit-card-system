using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class MaritialStatusFactory
    {
        public static MaritialStatus CreateMaritialStatusFactory(string ACode, string ADescription)
        {
            MaritialStatus objMaritialStatus = new MaritialStatus()
            {
                MaritalStatusCode = ACode,
                Description = ADescription
            };
            return objMaritialStatus;

        }
    }
}
