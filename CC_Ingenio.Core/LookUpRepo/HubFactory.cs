using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Core.LookUpRepo
{
    public class HubFactory
    {
        public static Hub CreateHubLookUpFactory(string ACode, string ADescription)
        {
            Hub objHub = new Hub()
            {
                HubCode = ACode,
                HubName = ADescription
            };
            return objHub;
        }
    }
}
